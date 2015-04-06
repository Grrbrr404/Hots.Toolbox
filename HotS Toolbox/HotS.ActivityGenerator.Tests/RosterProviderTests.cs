using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotS.ActivityGenerator.Tests {
	[TestClass]
	public class RosterProviderTests {
		[TestMethod]
		public void GetAllPlayers_Test() {
			var provider = new RosterProvider();
			var result = provider.GetMembers();
			Assert.IsTrue(result.Any());
		}
	}
}
