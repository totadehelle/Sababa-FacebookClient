using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MultithreadingFinalTask.Exceptions;
using Newtonsoft.Json;
using winsdkfb;
using winsdkfb.Graph;

namespace MultithreadingFinalTask.Models.SocialNetwork
{
	public class FacebookSocialNetwork : ISocialNetwork
	{
		private const double Percent = 100;
		private MusicContainer _musicContainer;

		public async Task<List<Music>> GetMusic(IProgress<double> progress, CancellationToken token)
		{
			try
			{
				var session = FBSession.ActiveSession;

				if (!session.LoggedIn)
					throw new AccessDeniedException("User is not logged in on Facebook, access denied");

				var friends = await GetFriends();

				if (friends.Count == 0)
					return new List<Music>();

				var step = Percent / friends.Count;
				_musicContainer = new MusicContainer(progress, step);

				token.ThrowIfCancellationRequested();
				var friendsTasks = (from friend in friends
					select Task.Run(async
						() => await GetMusicForOneFriend(friend, token), token)).ToArray();
				await Task.WhenAll(friendsTasks);

				return _musicContainer.GetMusicList();
			}
			finally
			{
				_musicContainer?.AddWaitHandle?.Close();
			}
		}

		public async Task<List<Friend>> GetFriends()
		{
			FBSession session = FBSession.ActiveSession;

			if (!session.LoggedIn) 
				throw new AccessDeniedException("User is not logged in on Facebook, access denied");

			string graphPath = "/me/friends";
			var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<Friend>);
			//FBPaginatedArray is used instead of FBSingleObject because FB uses auto paging for responses usually.
			var fbPaginatedArray = new FBPaginatedArray(graphPath, null, factory);
			var result = await fbPaginatedArray.FirstAsync();
			var friends = new List<Friend>();
			
			if(result == null || !result.Succeeded)
					throw new HttpRequestException("Cannot get response from Facebook server");
				
			var friendsObjects = (IReadOnlyList<object>)result.Object;
			foreach (var friend in friendsObjects)
			{
				friends.Add((Friend)friend);
			}
				
			while(fbPaginatedArray.HasNext)
			{
				result = await fbPaginatedArray.NextAsync();
				if(result == null || !result.Succeeded)
					throw new HttpRequestException("Cannot get response from Facebook server");
				
				var friendsObjects = (IReadOnlyList<object>)result.Object;
				foreach (var friend in friendsObjects)
				{
					friends.Add((Friend)friend);
				}			
			}
							
			return friends;
		}

		public async Task<bool> LogIn()
		{
			// Get active session
			var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			FBSession session = FBSession.ActiveSession;
			session.WinAppId = localSettings.Values["WinAppId"].ToString();
			session.FBAppId = localSettings.Values["FBAppId"].ToString();
			
			// Add permissions required by the app
			List<String> permissionList = new List<String>();
			permissionList.Add("public_profile");
			permissionList.Add("user_friends");
			permissionList.Add("user_likes");
			FBPermissions permissions = new FBPermissions(permissionList);

			// Login to Facebook
			FBResult result = await session.LoginAsync(permissions);

			if (!result.Succeeded) 
				throw new AuthorizationFailedException("Authorization on Facebook failed");
			
			localSettings.Values["IsAuthorized"] = true;
			return result.Succeeded;
		}

		public async Task<bool> LogOut()
		{
			FBSession session = FBSession.ActiveSession;
			await session.LogoutAsync();
			if (session.LoggedIn)
				throw new HttpRequestException("Logging out failed");
			return session.LoggedIn;
		}

		private async Task GetMusicForOneFriend(Friend friend, CancellationToken token)
		{
			var friendMusic = new List<Music>();
			var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<Music>);
			var graphPath = friend.Id + "/music";
			//FBPaginatedArray is used instead of FBSingleObject because FB uses auto paging for responses usually.
			var fbPaginatedArray = new FBPaginatedArray(graphPath, null, factory);
			var result = await fbPaginatedArray.FirstAsync();
			
			token.ThrowIfCancellationRequested();
			if (result == null || !result.Succeeded)
				throw new HttpRequestException("Cannot get response from Facebook server");

			var friendsObjects = (IReadOnlyList<object>)result.Object;
			foreach (var music in friendsObjects)
			{ 
				friendMusic.Add((Music)music);
			}
			
			while(fbPaginatedArray.HasNext){
				token.ThrowIfCancellationRequested();
				result = await fbPaginatedArray.NextAsync();
				if (result == null || !result.Succeeded)
					throw new HttpRequestException("Cannot get response from Facebook server");

				var friendsObjects = (IReadOnlyList<object>)result.Object;
				foreach (var music in friendsObjects)
				{ 
					friendMusic.Add((Music)music);
				}
			}
			
			_musicContainer?.AddWaitHandle.WaitOne();
			_musicContainer?.Add(friendMusic);
		}
	}
}