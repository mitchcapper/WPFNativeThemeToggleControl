# WPFNativeThemeToggleControl
[![Build](https://github.com/mitchcapper/WPFNativeThemeToggleControl/actions/workflows/build.yaml/badge.svg)](https://github.com/mitchcapper/WPFNativeThemeToggleControl/actions/workflows/build.yaml)
[![WPFNativeThemeToggleControl](https://img.shields.io/nuget/v/WPFNativeThemeToggleControl.svg?style=flat&label=WPFNativeThemeToggleControl)](https://www.nuget.org/packages/WPFNativeThemeToggleControl/)


Native-feeling WPF theme toggle button for `.NET 10` apps. It supports `Light`, `Dark`, and optional `System` mode, and updates `Application.Current.ThemeMode` when toggled.  Best used with themes like the [WPF 9+ built in fluent theme](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/whats-new/net90#fluent-theme).

## Features

- Small icon-only `ThemeToggleControl`
- `TriMode` support (`System -> Dark -> Light` cycle)
- Two-mode support (`Light <-> Dark`)
- One or Two Way Binding to properties
- `ThemeChanged` routed event with old/new values

## Install

```powershell
dotnet add package WPFNativeThemeToggleControl
```

## Quick start

Add the XML namespace:

```xml
xmlns:ThemeToggle="clr-namespace:WPFNativeThemeToggleControl;assembly=WPFNativeThemeToggleControl"
```

Use the control:

```xml
<ThemeToggle:ThemeToggleControl
    TriMode="True" CurrentTheme="{Binding OurThemeProp, Mode=TwoWay}"
    ThemeChanged="ThemeToggle_ThemeChanged" />
```

When not using data binding, there are two ways to set the initial theme in XAML.

### Option 1: `CurrentTheme` with `ThemeMode` (strongly typed)

Add the `sw` namespace to your XAML root:

```xml
xmlns:sw="clr-namespace:System.Windows;assembly=PresentationFramework"
```

Then use `x:Static`:

```xml
<ThemeToggle:ThemeToggleControl
    CurrentTheme="{x:Static sw:ThemeMode.Dark}"
    ThemeChanged="ThemeToggle_ThemeChanged" />
```

### Option 2: `CurrentThemeStr` with a string

```xml
<ThemeToggle:ThemeToggleControl
    CurrentThemeStr="DarkMode"
    ThemeChanged="ThemeToggle_ThemeChanged" />
```

Accepted string values include `Light`, `Dark`, `System`, and forms like `DarkMode`.

Handle theme changes (optional):

```csharp
using WPFNativeThemeToggleControl;

private void ThemeToggle_ThemeChanged(object sender, ThemeChangedEventArgs e)
{
    Title = $"Theme: {e.NewTheme}";
}
```

## Advanced usage

If your app needs custom theme application behavior, override `ThemeChangeApplyAction`:

```csharp
ThemeToggle.ThemeChangeApplyAction = newTheme =>
{
    // Your custom theme apply logic
    Application.Current!.ThemeMode = newTheme;
};
```

Because WPF theme mode control support is still experimental in `.NET 9/10`, your project may need:

```xml
<NoWarn>$(NoWarn);WPF0001</NoWarn>
```

