using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotS.ActivityGenerator.Tests {
	[TestClass]
	public class ActivityGeneratorTests {

		[TestMethod]
		public void CreateActivityRequest_Test() {
			var generator = new ActivityGenerator();
			var replayRepository = new ReplayRepository();
			var replays = replayRepository.GetAll();

			var result = generator.CreateActivityFromReplays(replays);
			Assert.IsFalse(string.IsNullOrEmpty(result));
		}

	}
}
