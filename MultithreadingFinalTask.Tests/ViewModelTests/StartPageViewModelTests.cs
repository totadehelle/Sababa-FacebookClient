using System;
using System.Threading;
using Windows.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultithreadingFinalTask.Models;
using MultithreadingFinalTask.ViewModels;

namespace MultithreadingFinalTask.Tests.ViewModelTests
{
	[TestClass]
	public class StartPageViewModelTests
	{
		[TestInitialize()]
		public void Setup()
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values["IsAuthorized"] = false;
		}
		
		[TestMethod]
		public void LogIn_Always_CallsModelLogInMethod()
		{
			Mock<IStartPageModel> model = new Mock<IStartPageModel>();
			var sut = new StartPageViewModel(model.Object);

			sut.LogIn().Wait();

			model.Verify(s => s.LogIn(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void LogOut_Always_CallsModelLogOutMethod()
		{
			Mock<IStartPageModel> model = new Mock<IStartPageModel>();
			var sut = new StartPageViewModel(model.Object);

			sut.LogOut().Wait();

			model.Verify(s => s.LogOut(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void GetFriends_Always_CallsModelGetFriendsMethod()
		{
			Mock<IStartPageModel> model = new Mock<IStartPageModel>();
			var sut = new StartPageViewModel(model.Object);

			sut.GetFriends().Wait();

			model.Verify(s => s.GetFriends(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void GetMusic_Always_CallsModelGetMusicMethod()
		{
			Mock<IStartPageModel> model = new Mock<IStartPageModel>();
			var sut = new StartPageViewModel(model.Object);

			sut.GetMusic().Wait();

			model.Verify(s => s.GetMusic(It.IsAny<IProgress<double>>(), It.IsAny<CancellationToken>()), 
				Times.AtLeastOnce);
		}

		[TestMethod]
		public void Ctor_IfWasAuthorizedWithinPreviousSession_CallsModelLogInMethod()
		{
			Mock<IStartPageModel> model = new Mock<IStartPageModel>();
			var localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values["IsAuthorized"] = true;

			var sut = new StartPageViewModel(model.Object);

			model.Verify(s => s.LogIn(), Times.AtLeastOnce);
		}
	}
}