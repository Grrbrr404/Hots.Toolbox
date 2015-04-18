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

		public IEnumerable<ClanMember> GetMembers() {
			var web = new HtmlWeb();
			var doc = web.Load(PageUrl);
			var result = new List<ClanMember>();
			var playerBattleTags = doc.DocumentNode.SelectNodes("//table/tbody/tr/td[3]");
			var memberRosterColumns = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[1]/div[1]/div[4]/div[1]/div[1]/div[5]/div[1]/div[1]/blockquote[1]/div[2]/div[2]/table[1]/tbody[1]/tr");
			memberRosterColumns.RemoveAt(0); // remove because column 0 is the table header and does not contain usefull data
			var columnForumName = 1;
			var columnBnetName = 4;


			if (memberRosterColumns.Any()) {
				foreach (var column in memberRosterColumns) {
					if (column.ChildNodes.Count >= 5) {
						var bnetNames = column.ChildNodes[columnBnetName].InnerText.Split(new[] { ';' });

						if (bnetNames.Any()) {
							foreach (var bnetName in bnetNames) {
								var forumName = column.ChildNodes[columnForumName].InnerText;
								if (forumName.Contains("[zG]"))
									forumName = forumName.Replace("[zG]", "");
								var member = new ClanMember() { BnetName = bnetName, ForumName = forumName };
								result.Add(member);
							}
						}
					}
				}
			}
			return result;
		}
	}
}
