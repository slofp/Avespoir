using Avespoir.Core.Modules.Logger;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Main {

	class UpdateChecker {

		private static readonly GitHubClient GitHub_Client = new GitHubClient(new ProductHeaderValue("Avespoir_Client"));

		public static async Task GetUpdate() {
			IReadOnlyList<RepositoryTag> RepositoryTagList = await GitHub_Client.Repository.GetAllTags("Fairy-Phy", "Avespoir");

			foreach (RepositoryTag Repository_Tag in RepositoryTagList) {
				string[] RepositoryTagSplit = Repository_Tag.Name.Split("-");
				if (RepositoryTagSplit.Length != 2) {
					Log.Warning("Incorrect tag name format");
					continue;
				}

				if (!float.TryParse(RepositoryTagSplit[1], out float TagVersion) || !float.TryParse(VersionInfo.VersionTag[1], out float CurrentVersion)) {
					Log.Warning("Could not convert tag version to numeric");
					continue;
				}

				if (TagVersion <= CurrentVersion) break;
				else {
					Log.Info(string.Format("Found update: {0} {1}", RepositoryTagSplit[0], RepositoryTagSplit[1]));
					break;
				}
			}
		}
	}
}
