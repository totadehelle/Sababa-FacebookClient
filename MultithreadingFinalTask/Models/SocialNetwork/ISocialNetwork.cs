﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingFinalTask.Models.SocialNetwork
{
	public interface ISocialNetwork
	{
		Task<List<Music>> GetMusic(IProgress<double> progress, CancellationToken token);
		Task<List<Friend>> GetFriends();
		Task<bool> LogIn();
		Task<bool> LogOut();
	}
}