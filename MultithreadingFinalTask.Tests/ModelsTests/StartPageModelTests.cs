using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultithreadingFinalTask.Models;
using MultithreadingFinalTask.Models.SocialNetwork;

namespace MultithreadingFinalTask.Tests.ModelsTests
{
	[TestClass]
	public class StartPageModelTests
	{
		[TestMethod]
		public void GetFriends_Always_CallsSocialNetworkGetFriendsMethod()
		{
			Mock<ISocialNetwork> socialNetwork = new Mock<ISocialNetwork>();
			var sut = new StartPageModel(socialNetwork.Object);

			sut.GetFriends().Wait();

			socialNetwork.Verify(s => s.GetFriends(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void GetMusic_Always_CallsSocialNetworkGetMusicMethod()
		{
			Mock<ISocialNetwork> socialNetwork = new Mock<ISocialNetwork>();
			Mock<IProgress<double>> progress = new Mock<IProgress<double>>();
			Mock<CancellationTokenSource> tokenSource = new Mock<CancellationTokenSource>();
			var sut = new StartPageModel(socialNetwork.Object);

			sut.GetMusic(progress.Object, tokenSource.Object.Token).Wait();

			socialNetwork.Verify(s => s.GetMusic(
				It.IsAny<IProgress<double>>(), It.IsAny<CancellationToken>()), 
				Times.AtLeastOnce);
		}

		[TestMethod]
		public void LogIn_Always_CallsSocialNetworkLogInMethod()
		{
			Mock<ISocialNetwork> socialNetwork = new Mock<ISocialNetwork>();
			var sut = new StartPageModel(socialNetwork.Object);

			sut.LogIn().Wait();

			socialNetwork.Verify(s => s.LogIn(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void LogOut_Always_CallsSocialNetworkLogOutMethod()
		{
			Mock<ISocialNetwork> socialNetwork = new Mock<ISocialNetwork>();
			var sut = new StartPageModel(socialNetwork.Object);

			sut.LogOut().Wait();

			socialNetwork.Verify(s => s.LogOut(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void GetMusic_MusicCollectionIsNotEmpty_ReturnsOrderedByRankMusicCollection()
		{
			var unorderedMusicCollection = new List<Music>()
			{
				new Music() {Name = "Test1"},
				new Music() {Name = "Test2"},
				new Music() {Name = "Test1"},
				new Music() {Name = "Test2"},
				new Music() {Name = "Test3"},
				new Music() {Name = "Test2"}
			};
			var expectedCollection = new List<Music>()
			{
				new Music() {Name = "Test2", Rank = 3},
				new Music() {Name = "Test1", Rank = 2},
				new Music() {Name = "Test3", Rank = 1},
			};
			Mock<ISocialNetwork> socialNetwork = new Mock<ISocialNetwork>();
			socialNetwork.Setup(s => s.GetMusic(It.IsAny<IProgress<double>>(),
				It.IsAny<CancellationToken>())).ReturnsAsync(unorderedMusicCollection);
			Mock<IProgress<double>> progress = new Mock<IProgress<double>>();
			Mock<CancellationTokenSource> tokenSource = new Mock<CancellationTokenSource>();
			var sut = new StartPageModel(socialNetwork.Object);

			var actualCollection = sut.GetMusic(progress.Object, tokenSource.Object.Token).Result;
			
			CollectionAssert.AreEqual(actualCollection, expectedCollection, new MusicComparer());
		}
	}
}