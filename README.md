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

Because WPF theme mode support is still experimental in `.NET 10`, your project may need:

```xml
<NoWarn>$(NoWarn);WPF0001</NoWarn>
```

