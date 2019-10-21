using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultithreadingFinalTask.Exceptions;
using MultithreadingFinalTask.Models.SocialNetwork;

namespace MultithreadingFinalTask.Tests.ModelsTests.SocialNetworkTests
{
	[TestClass]
	public class FacebookSocialNetworkTests
	{
		[TestMethod]
		[ExpectedException(typeof(AccessDeniedException))]
		public void GetMusic_UserIsNotLoggedIn_ThrowsAccessDeniedException()
		{
			Mock<IProgress<double>> progress = new Mock<IProgress<double>>();
			Mock<CancellationTokenSource> tokenSource = new Mock<CancellationTokenSource>();
			var sut = new FacebookSocialNetwork();

			var music = sut.GetMusic(progress.Object, tokenSource.Object.Token).GetAwaiter().GetResult();
		}

		[TestMethod]
		[ExpectedException(typeof(AccessDeniedException))]
		public void GetFriends_UserIsNotLoggedIn_ThrowsAccessDeniedException()
		{
			Mock<IProgress<double>> progress = new Mock<IProgress<double>>();
			Mock<CancellationTokenSource> tokenSource = new Mock<CancellationTokenSource>();
			var sut = new FacebookSocialNetwork();

			var friends = sut.GetFriends().GetAwaiter().GetResult();
		}
	}
}