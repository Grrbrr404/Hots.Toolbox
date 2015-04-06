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

			var listOfMembersPlayedWith = GetAllPlayersPlayedWith(replays);

			foreach (var playerName in listOfMembersPlayedWith) {
				templateBuilder.AppendFormat("@{0} ", playerName.ForumName);
			}

			return templateBuilder.ToString();
		}

		private List<ClanMember> GetAllPlayersPlayedWith(IEnumerable<Replay> replays) {
			var listOfMembersPlayedWith = new List<ClanMember>();
			var rosterProvider = new RosterProvider();
			var members = rosterProvider.GetMembers();

			if (replays.Any()) {
				foreach (var player in members) {
					var playerFound = false;
					foreach (var replay in replays) {
						if (replay.Players.Any(p => player.BnetName.StartsWith(p.Name))) {
							playerFound = true;
							break;
						}
					}
					if (playerFound) {
						listOfMembersPlayedWith.Add(player);
					}
				}
			}

			return listOfMembersPlayedWith;
		}
	}
}
