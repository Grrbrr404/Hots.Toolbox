using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Heroes.ReplayParser;
using HtmlAgilityPack;

namespace HotS.ActivityGenerator {
	public class RosterProvider {
		private const string PageUrl = "http://www.zealotgaming.com/pages.php?pageid=15&id=hotseu";

		public RosterProvider() {

		}

		public IEnumerable<string> GetPlayerNames() {
			var web = new HtmlWeb();
			var doc = web.Load(PageUrl);
			var result = new List<string>();
			var playerBattleTags = doc.DocumentNode.SelectNodes("//table/tbody/tr/td[3]");

			if (playerBattleTags.Any()) {
				foreach (var playerBattleTag in playerBattleTags) {
					var text = string.IsNullOrEmpty(playerBattleTag.InnerText) ? "<unkown>" : playerBattleTag.InnerText;
					var hashTagIndex = text.IndexOf('#');
					if (hashTagIndex >= 0) {
						text = text.Substring(0, hashTagIndex);
					}
					result.Add(text);
				}
			}
			return result;
		}
	}
}
