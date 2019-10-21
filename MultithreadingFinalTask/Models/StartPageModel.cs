using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultithreadingFinalTask.Models.SocialNetwork;
using System.Linq;
using System.Threading;

namespace MultithreadingFinalTask.Models
{
	public class StartPageModel : IStartPageModel
	{
		private readonly ISocialNetwork _socialNetwork;

		public StartPageModel(ISocialNetwork socialNetwork)
		{
			_socialNetwork = socialNetwork;
		}

		public async Task<List<Music>> GetMusic(IProgress<double> progress, CancellationToken token)
		{
			var rawMusic = await _socialNetwork.GetMusic(progress, token);
			var rankedMusic = Rank(rawMusic);
			return rankedMusic;
		}

		public Task<List<Friend>> GetFriends()
		{
			return _socialNetwork.GetFriends();
		}

		public Task<bool> LogIn()
		{
			return _socialNetwork.LogIn();
		}

		public Task<bool> LogOut()
		{
			return _socialNetwork.LogOut();
		}

		private List<Music> Rank(List<Music> rawMusic)
		{
			var rankedMusic = rawMusic?.Select(m => m.Name).GroupBy(x => x)
				.Select(y => new Music() { Name = y.Key, Rank = y.Count() })
				.OrderByDescending(m => m.Rank)
				.ToList();

			return rankedMusic;
		}
	}
}