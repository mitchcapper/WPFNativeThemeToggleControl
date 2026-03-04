using System.Windows;
using WPFNativeThemeToggleControl;

namespace TestApp {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void ThemeToggle_ThemeChanged(object sender, ThemeChangedEventArgs e) {
			Title = $"TestApp — Theme: {e.NewTheme}";
		}

	}
}
