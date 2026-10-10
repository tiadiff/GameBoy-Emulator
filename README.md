# GBZ

> A retro Game Boy handheld emulator and frontend for Windows built with .NET 10 WinForms and the GonFox emulation core.

---

## Overview

**GBZ** brings classic Game Boy gaming to Windows with a realistic handheld shell design. Featuring a custom borderless rounded window contour, integrated volume slider, low-latency audio via NAudio, and full XInput gamepad integration, GBZ provides a nostalgic, immersive retro experience right on your desktop.

## Features

- **Realistic Handheld Shell**: Custom non-rectangular window shape (rounded corners with classic Game Boy bottom-right contour) and borderless presentation.
- **Draggable Anywhere**: Click and drag from any point on the handheld casing to move the window freely across your screen.
- **GonFox Emulation Core**: High-accuracy Game Boy emulation powered by `GonFox.GameBoy.Core` with fast boot bypass support.
- **Low-Latency Audio**: 48kHz 16-bit stereo sound playback powered by `NAudio` with real-time buffer management.
- **Custom Volume Slider**: On-chassis custom slider control for dynamic master volume adjustment.
- **XInput Controller Support**: Plug-and-play support for Xbox 360, Xbox One, Xbox Series X|S, and compatible XInput gamepads.
- **ROM Loading**: Quick ROM loading dialog for `.gb` and `.gbc` files.

## Controls

### Gamepad (XInput)

| Controller Button | Game Boy Action |
| ----------------- | --------------- |
| D-Pad Up / Left Stick Up | Up |
| D-Pad Down / Left Stick Down | Down |
| D-Pad Left / Left Stick Left | Left |
| D-Pad Right / Left Stick Right | Right |
| **A** | A Button |
| **B** or **X** | B Button |
| **Start** | Start |
| **Back** / View | Select |

### Mouse & Interface

- **Load ROM**: Click the control panel area on the bottom-left to select and load `.gb` / `.gbc` ROMs.
- **Volume**: Drag the volume slider on the right side of the device.
- **Move Window**: Click and drag anywhere on the console shell to reposition.
- **Exit**: Click the **X** icon on the bottom-left corner to cleanly stop emulation and close the app.

## Requirements

- **Operating System**: Windows 10 (Build 19041+) or Windows 11
- **Runtime**: [.NET 10.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) (Windows x64)
- **Controller**: XInput-compatible controller (recommended for gameplay)

## Getting Started

### Download Release

1. Download the latest `v1.0.0.zip` from the [Releases](https://github.com/tiadiff/gbz/releases) page.
2. Extract the archive to your preferred directory.
3. Launch `gbz.exe`.
4. Click the load area on the console to pick your Game Boy ROM (`.gb` or `.gbc`).

### Building From Source

1. Clone the repository:
   ```bash
   git clone https://github.com/tiadiff/gbz.git
   cd gbz
   ```

2. Build in Release mode:
   ```bash
   dotnet build -c Release
   ```

3. Run the executable:
   ```bash
   dotnet run --project gbz/gbz.csproj -c Release
   ```

## Tech Stack & Dependencies

- **Framework**: [.NET 10.0 (net10.0-windows)](https://dotnet.microsoft.com/)
- **UI**: Windows Forms (WinForms) with custom GDI+ rendering
- **Emulation**: `GonFox.GameBoy.Core` & `GonFox.GameBoy.Platform.Shared`
- **Audio**: `NAudio`
- **Input**: `SharpDX.XInput`

## Contributing

Contributions, bug reports, and suggestions are welcome! Please check out [CONTRIBUTING.md](CONTRIBUTING.md) for details on guidelines and workflow.

## Security

For security vulnerability reporting procedures, please review our [Security Policy](SECURITY.md).

## License

This project is licensed under the [MIT License](LICENSE).
