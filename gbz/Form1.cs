using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace gbz
{
    public partial class Form1 : Form
    {
        private GonFox.GameBoy.Core.GameBoySystem? _gbSystem;
        private Bitmap? _screenBitmap;
        private bool _isRunning;
        private byte[] _frameBuffer = new byte[160 * 144 * 4];
        private float _masterVolume = 0.5f;

        private SharpDX.XInput.Controller? _controller;
        private NAudio.Wave.WaveOut? _waveOut;
        private NAudio.Wave.BufferedWaveProvider? _waveProvider;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            SetCustomRoundedRegion();
            InitEmulator();
        }

        private void InitEmulator()
        {
            _gbSystem = new GonFox.GameBoy.Core.GameBoySystem();
            _gbSystem.UseBootBypass();

            _screenBitmap = new Bitmap(160, 144, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            panel1.Paint += Panel1_Paint;
            panel2.Click += Panel2_Click;

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panel1, new object[] { true });

            flatSlider1.ValueChanged += (s, e) => { _masterVolume = flatSlider1.Value; };
            _masterVolume = flatSlider1.Value;

            try
            {
                _controller = new SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One);
            }
            catch { }

            try
            {
                _waveOut = new NAudio.Wave.WaveOut();
                _waveProvider = new NAudio.Wave.BufferedWaveProvider(new NAudio.Wave.WaveFormat(48000, 16, 2));
                _waveProvider.DiscardOnBufferOverflow = true;
                _waveOut.Init(_waveProvider);
                _waveOut.Play();
            }
            catch { }
        }

        private void Panel2_Click(object? sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "GameBoy ROMs|*.gb;*.gbc|Tutti i file|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        byte[] romData = System.IO.File.ReadAllBytes(ofd.FileName);
                        var loadResult = GonFox.GameBoy.Core.Cartridge.CartridgeLoader.Load(romData);
                        
                        if (_isRunning)
                        {
                            _isRunning = false;
                            System.Threading.Thread.Sleep(50); 
                        }
                        
                        _gbSystem = new GonFox.GameBoy.Core.GameBoySystem();
                        _gbSystem.UseBootBypass();
                        _gbSystem.InsertCartridge(loadResult.Cartridge);
                        
                        _isRunning = true;
                        System.Threading.Tasks.Task.Run(EmulationLoop);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ROM: " + ex.Message);
            }
        }

        private readonly object _frameLock = new object();

        private void EmulationLoop()
        {
            int cyclesPerFrame = 70224;
            short[] audioFrames = new short[4096];

            while (_isRunning && _gbSystem != null && _screenBitmap != null)
            {
                while (_waveProvider != null && _waveProvider.BufferedDuration.TotalMilliseconds > 50)
                {
                    System.Threading.Thread.Sleep(5);
                }

                if (_controller != null && _controller.IsConnected)
                {
                    var state = _controller.GetState();
                    var buttons = state.Gamepad.Buttons;
                    
                    bool left = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.DPadLeft) || state.Gamepad.LeftThumbX < -16000;
                    bool right = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.DPadRight) || state.Gamepad.LeftThumbX > 16000;
                    bool up = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.DPadUp) || state.Gamepad.LeftThumbY > 16000;
                    bool down = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.DPadDown) || state.Gamepad.LeftThumbY < -16000;
                    bool a = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.A);
                    bool b = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.X) || buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.B);
                    bool start = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.Start);
                    bool select = buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.Back);
                    
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Left, left);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Right, right);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Up, up);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Down, down);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.A, a);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.B, b);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Start, start);
                    _gbSystem.Joypad.SetButtonState(GonFox.GameBoy.Core.Devices.JoypadButton.Select, select);
                }

                _gbSystem.RunForTCycles(cyclesPerFrame);

                lock (_frameLock)
                {
                    _gbSystem.Video.CopyLatestFrame(_frameBuffer);
                }

                if (_waveProvider != null)
                {
                    int framesRead = _gbSystem.Audio.ReadFrames(audioFrames);
                    if (framesRead > 0)
                    {
                        int samplesCount = framesRead * 2; // 2 channels
                        byte[] byteBuf = new byte[samplesCount * 2]; // 2 bytes per sample
                        for (int i = 0; i < samplesCount; i++)
                        {
                            short val = (short)(audioFrames[i] * _masterVolume);
                            byteBuf[i * 2] = (byte)(val & 0xFF);
                            byteBuf[i * 2 + 1] = (byte)(val >> 8);
                        }
                        _waveProvider.AddSamples(byteBuf, 0, byteBuf.Length);
                    }
                }

                try
                {
                    panel1.BeginInvoke(new Action(() =>
                    {
                        lock (_frameLock)
                        {
                            var rect = new Rectangle(0, 0, 160, 144);
                            var bmpData = _screenBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            System.Runtime.InteropServices.Marshal.Copy(_frameBuffer, 0, bmpData.Scan0, _frameBuffer.Length);
                            _screenBitmap.UnlockBits(bmpData);
                        }
                        panel1.Invalidate();
                    }));
                }
                catch { }
            }
        }

        private void Panel1_Paint(object? sender, PaintEventArgs e)
        {
            if (_screenBitmap != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                e.Graphics.DrawImage(_screenBitmap, new Rectangle(0, 0, panel1.Width, panel1.Height));
            }
        }

        private void closeLabel_Click(object? sender, EventArgs e)
        {
            _isRunning = false;
            try
            {
                _waveOut?.Stop();
                _waveOut?.Dispose();
            }
            catch { }
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Permette di trascinare la finestra cliccando in qualsiasi punto
            if (m.Msg == 0x84) // WM_NCHITTEST
            {
                if (m.Result == (IntPtr)1) // HTCLIENT
                {
                    m.Result = (IntPtr)2; // HTCAPTION
                }
            }
        }

        private void SetCustomRoundedRegion()
        {
            int radiusNormal = 20;
            int radiusBottomRight = 105;

            GraphicsPath path = new GraphicsPath();

            // Top Left
            path.AddArc(0, 0, radiusNormal * 2, radiusNormal * 2, 180, 90);

            // Top Right
            path.AddArc(this.Width - radiusNormal * 2, 0, radiusNormal * 2, radiusNormal * 2, 270, 90);

            // Bottom Right
            path.AddArc(this.Width - radiusBottomRight * 2, this.Height - radiusBottomRight * 2, radiusBottomRight * 2, radiusBottomRight * 2, 0, 90);

            // Bottom Left
            path.AddArc(0, this.Height - radiusNormal * 2, radiusNormal * 2, radiusNormal * 2, 90, 90);

            path.CloseFigure();

            this.Region = new Region(path);
        }
    }

    public class FlatSlider : Control
    {
        private float _value = 0.5f;
        private bool _isDragging = false;

        public event EventHandler? ValueChanged;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public float Value
        {
            get => _value;
            set 
            { 
                float newVal = Math.Max(0, Math.Min(1, value));
                if (_value != newVal)
                {
                    _value = newVal;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public FlatSlider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            BackColor = Color.Transparent;
            Size = new Size(100, 20);
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.None;

            int trackHeight = 6;
            int trackY = (Height - trackHeight) / 2;
            using (SolidBrush trackBrush = new SolidBrush(Color.FromArgb(180, 180, 180)))
            {
                e.Graphics.FillRectangle(trackBrush, 0, trackY, Width, trackHeight);
            }

            int thumbWidth = 12;
            int thumbHeight = Height;
            int thumbX = (int)(_value * (Width - thumbWidth));
            int thumbY = 0;
            using (SolidBrush thumbBrush = new SolidBrush(Color.FromArgb(130, 130, 130)))
            {
                e.Graphics.FillRectangle(thumbBrush, thumbX, thumbY, thumbWidth, thumbHeight);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isDragging = true;
            UpdateValue(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isDragging)
            {
                UpdateValue(e.X);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isDragging = false;
        }

        private void UpdateValue(int x)
        {
            int thumbWidth = 12;
            float val = (float)(x - thumbWidth / 2) / (Width - thumbWidth);
            Value = val;
        }
    }
}
