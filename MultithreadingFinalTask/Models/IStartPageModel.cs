using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingFinalTask.Models
{
	public interface IStartPageModel
	{
		Task<List<Music>> GetMusic(IProgress<double> progress, CancellationToken token);
		Task<List<Friend>> GetFriends();
		Task<bool> LogIn();
		Task<bool> LogOut();
	}
}