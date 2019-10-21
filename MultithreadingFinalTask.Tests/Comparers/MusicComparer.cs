using System;
using System.Collections;
using MultithreadingFinalTask.Models;

namespace MultithreadingFinalTask.Tests.ModelsTests
{
	public class MusicComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Music xMusic = x as Music;
			Music yMusic = y as Music;
			if(xMusic == null || yMusic == null)
				throw new ArgumentNullException();
			if (
				xMusic.Id == yMusic.Id
				&& xMusic.Name == yMusic.Name
				&& xMusic.Rank == yMusic.Rank)
				return 0;
			else if (xMusic.Rank > yMusic.Rank)
				return 1;
			else
				return -1;
		}
	}
}