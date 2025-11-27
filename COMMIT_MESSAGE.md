# Git Commit Message

```
refactor: migrate Focus Planner to code-only WPF UI

- Remove MainWindow.xaml and MainWindow.xaml.cs
- Add MainWindow.cs with pure C# layout and controls
- Replace App.xaml/App.xaml.cs with App.cs (code-only Application)
- Remove StartupUri and XAML dependencies
- Clean up FocusPlanner.csproj XAML page references
- Add StartupObject to point to App.Main entry point

Result: app builds and runs with a purely C#-based WPF UI, no XAML required.

Files changed:
- Deleted: MainWindow.xaml, MainWindow.xaml.cs
- Deleted: App.xaml, App.xaml.cs
- Added: MainWindow.cs (pure C# Window implementation)
- Added: App.cs (pure C# Application with Main entry point)
- Modified: FocusPlanner.csproj (removed XAML dependencies, added StartupObject)
```

