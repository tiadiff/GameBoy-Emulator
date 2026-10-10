# Contributing to GBZ

Thank you for your interest in contributing to **GBZ**! Contributions from the community help make this project better for retro gaming enthusiasts and .NET developers alike.

## Code of Conduct

Please be respectful, collaborative, and considerate when participating in issues, discussions, and pull requests.

## How Can I Contribute?

- **Reporting Bugs**: Check existing issues before opening a new one. Provide reproducible steps, expected behavior, actual behavior, and system specifications.
- **Suggesting Enhancements**: Propose new features or improvements by opening an issue labeled `enhancement`.
- **Submitting Pull Requests**: Implement bug fixes, performance improvements, or new features.

## Development Setup

### Prerequisites

- **Windows 10 / Windows 11** (GBZ utilizes Windows Forms with custom GDI+ regions and XInput)
- **.NET 10 SDK** (or later)
- **Visual Studio 2022+** or **Visual Studio Code** / **JetBrains Rider** with C# support
- An XInput-compatible controller (Xbox 360 / Xbox One / Series X|S controller) for testing controls

### Getting Started

1. **Fork and clone the repository**:
   ```bash
   git clone https://github.com/tiadiff/gbz.git
   cd gbz
   ```

2. **Restore dependencies and build**:
   ```bash
   dotnet restore
   dotnet build -c Release
   ```

3. **Run the application**:
   ```bash
   dotnet run --project gbz/gbz.csproj -c Release
   ```

## Development Guidelines

- **Code Style**: Follow standard C# and .NET coding conventions. Use clear variable and method names.
- **Commit Messages**: Write clear, descriptive commit messages.
- **Branches**: Create feature or bugfix branches from `main` (e.g., `feature/custom-palettes` or `fix/audio-buffer`).
- **Clean Builds**: Ensure your code builds with 0 errors and 0 warnings before submitting a pull request.

## Pull Request Process

1. Create a branch for your changes:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Commit and test your changes thoroughly.
3. Open a Pull Request targeting the `main` branch.
4. Describe your changes clearly in the PR description, referencing any relevant issues.
