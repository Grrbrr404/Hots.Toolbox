using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Heroes.ReplayParser;

namespace HotS.ActivityGenerator {
	public class ActivityGenerator {
		private IEnumerable<Replay> _replays;

		public ActivityGenerator() {

		}

		public string CreateActivityFromReplays(IEnumerable<Replay> replays, string hotsProfile = "") {
			var templateBuilder = new StringBuilder(2048)
				.AppendLine("I would like to request acitivty points for games played with other zG-Hots members.")
				.AppendLine()
				.AppendLine("Replay's and match history can be found here: " + hotsProfile)
				.AppendLine()
				.AppendLine("Members I played with:");

			var listOfPlayersPlayedWith = GetAllPlayersPlayedWith(replays);

			foreach (var playerName in listOfPlayersPlayedWith) {
				templateBuilder.AppendFormat("@{0} ", playerName);
			}

			return templateBuilder.ToString();
		}

		private List<string> GetAllPlayersPlayedWith(IEnumerable<Replay> replays) {
			var listOfPlayersPlayedWith = new List<string>();
			var rosterProvider = new RosterProvider();
			var playerNames = rosterProvider.GetPlayerNames();

			if (replays.Any()) {
				foreach (var name in playerNames) {
					var playerFound = false;
					foreach (var replay in replays) {
						if (replay.Players.Any(p => name.StartsWith(p.Name))) {
							playerFound = true;
							break;
						}
					}
					if (playerFound) {
						listOfPlayersPlayedWith.Add(name);
					}
				}
			}

			return listOfPlayersPlayedWith;
		}
	}
}
