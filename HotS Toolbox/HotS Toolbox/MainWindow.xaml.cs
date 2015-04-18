using HotS.ActivityGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotS_Toolbox {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainViewModel Context { get; set; }
		public MainWindow() {
			InitializeComponent();
			Context = new MainViewModel();
			DataContext = Context;
			SelectAllReplaysSinceLastSunday();
			if (string.IsNullOrEmpty(Properties.Settings.Default.hotsprofile) == false)
				txtHotsProfile.Text = Properties.Settings.Default.hotsprofile;
		}

		private void SelectAllReplaysSinceLastSunday() {
			var today = DateTime.Today;
			
			if (today.DayOfWeek == DayOfWeek.Sunday) {
				today = today.AddDays(-7);
			} else {
				while (today.DayOfWeek != DayOfWeek.Sunday) {
					today = today.AddDays(-1);
				}
			}

			foreach (var replay in Context.Replays) {
				if (replay.Timestamp >= today) {
					replay.IsChecked = true;
				} else {
					break;
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			var selectedReplays = Context.Replays.Where(r => r.IsChecked).ToList();
			
			
			var activityGenerator = new ActivityGenerator();
			var acticityText = activityGenerator.CreateActivityFromReplays(selectedReplays, txtHotsProfile.Text);

			Clipboard.SetText(acticityText);
			Properties.Settings.Default.hotsprofile = txtHotsProfile.Text;
			Properties.Settings.Default.Save();
			MessageBox.Show(this, "Activity Report has been generated and copied to your clipboard. Just paste it where you want.");
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
			var infoWindow = new InfoWindow();
			infoWindow.Show();
		}
	}
}
