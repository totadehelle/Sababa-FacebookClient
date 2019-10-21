using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MultithreadingFinalTask.Models.SocialNetwork
{
	/// <summary>
	/// Helper class for getting results and reporting progress for ISocialNetwork GetMusic method
	/// </summary>
	public class MusicContainer
	{
		private readonly ConcurrentBag<List<Music>> _musicBag;
		private double _counter;
		private readonly IProgress<double> _progress;
		private readonly double _step;
		public AutoResetEvent AddWaitHandle;

		public MusicContainer(IProgress<double> progress, double step)
		{
			_musicBag = new ConcurrentBag<List<Music>>();
			_counter = 0.0;
			_progress = progress;
			_step = step;
			AddWaitHandle = new AutoResetEvent(true);
		}

		public void Add(List<Music> list)
		{
			_musicBag.Add(list);
			_progress.Report(_counter+=_step);
			AddWaitHandle.Set();
		}

		public List<Music> GetMusicList()
		{
			var musicList = new List<Music>();
			foreach (var list in _musicBag)
			{
				musicList.AddRange(list);
			}
			
			return musicList;
		}
	}
}