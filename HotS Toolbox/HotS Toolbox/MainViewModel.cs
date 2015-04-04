using Heroes.ReplayParser;
using HotS.ActivityGenerator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HotS_Toolbox {
	public class MainViewModel {
		private ObservableCollection<Replay> _replays;

		public MainViewModel() {
		}

		public ObservableCollection<Replay> Replays {
			get {
				if (_replays == null) {
					var replayRepo = new ReplayRepository();
					_replays = new ObservableCollection<Replay>(replayRepo.GetAll());
				}
				return _replays;
			}
		}

	}
}
