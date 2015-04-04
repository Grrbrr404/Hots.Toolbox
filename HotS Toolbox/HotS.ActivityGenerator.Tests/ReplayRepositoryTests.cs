using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotS.ActivityGenerator.Tests {
	[TestClass]
	public class ReplayRepositoryTests {
		[TestMethod]
		public void GetAllTest_Test() {
			var repo = new ReplayRepository();
			var replays = repo.GetAll();
			Assert.IsTrue(replays.Any());
		}
	}
}
