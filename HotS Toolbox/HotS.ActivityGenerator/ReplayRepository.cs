using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Heroes.ReplayParser;
using MpqLib.Mpq;

namespace HotS.ActivityGenerator {
	public class ReplayRepository {

		private readonly string _hotsAccountFolder;
		private const int FileLimit = 100;

		public ReplayRepository() {
			_hotsAccountFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");
			if (!Directory.Exists(_hotsAccountFolder)) {
				throw new Exception("Could not found replay folder");
			}
		}

		public IEnumerable<Replay> GetAll() {
			var files = GetReplayFiles();
			var result = new List<Replay>(FileLimit);
			var enumerable = files as IList<string> ?? files.ToList();
			if (enumerable.Any()) {
				int fileCount = enumerable.Count();
				int maxLoops = fileCount >= FileLimit ? FileLimit : fileCount;
				for (int i = 0; i < maxLoops; i++) {
					var currentFile = enumerable.ElementAt(i);
					// Use temp directory for MpqLib directory permissions requirements
					var pathToTempFile = Path.GetTempFileName();
					File.Copy(currentFile, pathToTempFile, true);
					var replay = new Replay();
					try {
						MpqHeader.ParseHeader(replay, pathToTempFile);
						result.Add(replay);
						using (var archive = new CArchive(pathToTempFile)) {
							ReplayInitData.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.initData"));
							//ReplayTrackerEvents.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.tracker.events"));
							ReplayDetails.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.details"));
							ReplayAttributeEvents.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.attributes.events"));
							//if (replay.ReplayBuild >= 32455)
							//	ReplayGameEvents.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.game.events"));
							ReplayServerBattlelobby.Parse(replay, GetMpqArchiveFileBytes(archive, "replay.server.battlelobby"));
						}
					}
					catch (Exception e) {
						Console.WriteLine(e);
					}
					finally {
						File.Delete(pathToTempFile);
					}
				}
			}

			return result;
		}

		private IEnumerable<string> GetReplayFiles() {
			var files =
				Directory.GetFiles(_hotsAccountFolder, "*.StormReplay", SearchOption.AllDirectories)
					.OrderByDescending(d => new FileInfo(d).CreationTime);
			return files;
		}

		private static byte[] GetMpqArchiveFileBytes(CArchive archive, string archivedFileName) {
			var buffer = new byte[archive.FindFiles(archivedFileName).Single().Size];
			archive.ExportFile(archivedFileName, buffer);
			return buffer;
		}

		public int GetCount() {
			return GetReplayFiles().Count();
		}

		public bool Any() {
			return GetCount() > 0;
		}

	}
}
