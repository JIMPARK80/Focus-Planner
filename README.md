# Focus Planner v0.3

A WPF-based task management application (Front-End Only)

[한국어 버전 / Korean Version](README.ko.md)

## Features

- ✅ Add tasks
- ✅ Mark tasks as complete
- ✅ Delete completed items
- ✅ Refresh task list
- ✅ Quick add with Enter key
- ✅ Separate display for completed tasks on the right

## How to Run

### Method 1: Using Solution File (Recommended)

```bash
# Build solution
dotnet build "Focus Planner.sln"

# Run application
dotnet run --project FocusPlanner.csproj
```

### Method 2: Direct Project File

```bash
# Build project
dotnet build FocusPlanner.csproj

# Run application
dotnet run --project FocusPlanner.csproj
```

### Method 3: Visual Studio

Open `Focus Planner.sln` in Visual Studio and press F5 to run

## Tech Stack

- .NET 7.0
- WPF (Windows Presentation Foundation)
- Pure C# code-only UI (no XAML)

## Next Steps

- Google Sheets API integration
- Backend integration
