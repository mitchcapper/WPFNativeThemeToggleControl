using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace WPFNativeThemeToggleControl {
	public class ThemeChangedEventArgs : RoutedEventArgs {
		public ThemeMode OldTheme { get; }
		public ThemeMode NewTheme { get; }

		public ThemeChangedEventArgs(RoutedEvent routedEvent, ThemeMode oldTheme, ThemeMode newTheme) : base(routedEvent) {
			OldTheme = oldTheme;
			NewTheme = newTheme;
		}
	}

	public partial class ThemeToggleControl : UserControl {
		private static readonly bool CachedSystemThemeIsLight = ReadSystemThemeIsLight();

		public static readonly DependencyProperty CurrentThemeProperty = DependencyProperty.Register(
			nameof(CurrentTheme),
			typeof(ThemeMode),
			typeof(ThemeToggleControl),
			new FrameworkPropertyMetadata(
				ThemeMode.System,
				default,
				OnCurrentThemeChanged,
				CoerceCurrentTheme));

		public static readonly DependencyProperty TriModeProperty = DependencyProperty.Register(
			nameof(TriMode),
			typeof(bool),
			typeof(ThemeToggleControl),
			new PropertyMetadata(true, OnTriModeChanged));

		public static readonly RoutedEvent ThemeChangedEvent = EventManager.RegisterRoutedEvent(
			nameof(ThemeChanged),
			RoutingStrategy.Bubble,
			typeof(EventHandler<ThemeChangedEventArgs>),
			typeof(ThemeToggleControl));

		public ThemeMode CurrentTheme {
			get => (ThemeMode)GetValue(CurrentThemeProperty);
			set => SetValue(CurrentThemeProperty, value);
		}

		public bool TriMode {
			get => (bool)GetValue(TriModeProperty);
			set => SetValue(TriModeProperty, value);
		}

		public event EventHandler<ThemeChangedEventArgs> ThemeChanged {
			add => AddHandler(ThemeChangedEvent, value);
			remove => RemoveHandler(ThemeChangedEvent, value);
		}

		public ThemeToggleControl() {
			InitializeComponent();

			var appTheme = Application.Current?.ThemeMode ?? ThemeMode.None;
			if (appTheme == ThemeMode.None) {
				CurrentTheme = ThemeMode.System;
				if (Application.Current is not null)
					Application.Current.ThemeMode = ThemeMode.System;

			} else
				CurrentTheme = appTheme;


			UpdateIcon();
			UpdateToolTip();
		}
		public Action<ThemeMode> ThemeChangeApplyAction { get; set; } = (newTheme) => Application.Current?.ThemeMode = newTheme;

		private static void OnCurrentThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var control = (ThemeToggleControl)d;
			var oldTheme = (ThemeMode)e.OldValue;
			var newTheme = (ThemeMode)e.NewValue;
				
			control.ThemeChangeApplyAction(newTheme);

			control.UpdateIcon();
			control.UpdateToolTip();
			control.RaiseEvent(new ThemeChangedEventArgs(ThemeChangedEvent, oldTheme, newTheme));
		}

		private static void OnTriModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var control = (ThemeToggleControl)d;
			control.CoerceValue(CurrentThemeProperty);
			control.UpdateIcon();
			control.UpdateToolTip();
		}

		private static object CoerceCurrentTheme(DependencyObject d, object baseValue) {
			var control = (ThemeToggleControl)d;
			var candidate = (ThemeMode)baseValue;

			if (!control.TriMode && candidate == ThemeMode.System)
				return GetSystemThemeIsLight() ? ThemeMode.Light : ThemeMode.Dark;


			return candidate;
		}

		private void OnToggleClick(object sender, RoutedEventArgs e) {
			if (TriMode) {
				bool systemIsLight = GetSystemThemeIsLight();

				if (CurrentTheme == ThemeMode.System)
					CurrentTheme = systemIsLight ? ThemeMode.Dark : ThemeMode.Light;
				else if (CurrentTheme == ThemeMode.Dark)
					CurrentTheme = systemIsLight ? ThemeMode.Light : ThemeMode.System;
				else
					CurrentTheme = systemIsLight ? ThemeMode.System : ThemeMode.Dark;

			} else
				CurrentTheme = CurrentTheme == ThemeMode.Light ? ThemeMode.Dark : ThemeMode.Light;

		}

		private void UpdateIcon() {
			if (IconPath is null) {
				return;
			}

			string key;
			if (CurrentTheme == ThemeMode.Light)
				key = "SunGeometry";
			else if (CurrentTheme == ThemeMode.Dark)
				key = "MoonGeometry";
			else
				key = "SystemGeometry";


			if (TryFindResource(key) is Geometry geometry)
				IconPath.Data = geometry;

		}

		private void UpdateToolTip() {

			string next = TriMode
				? "cycle mode"
				: "toggle light/dark";

			if (ToggleButton is not null)
				ToggleButton.ToolTip = $"Theme: {CurrentTheme} (click to {next})";

		}

		private static bool GetSystemThemeIsLight() {
			return CachedSystemThemeIsLight;
		}

		private static bool ReadSystemThemeIsLight() {
			try {
				using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
				var value = key?.GetValue("AppsUseLightTheme");

				return value is int intValue ? intValue != 0 : true;
			} catch {
				return true;
			}
		}
	}

}
