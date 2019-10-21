using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using MultithreadingFinalTask.Exceptions;
using MultithreadingFinalTask.Models;

namespace MultithreadingFinalTask.ViewModels
{
	public class StartPageViewModel : ViewModelBase
	{
		#region BINDING_PROPERTIES
		private InvertableBool _isAuthorized;
		public InvertableBool IsAuthorized
		{
			get => _isAuthorized;
			set
			{
				_isAuthorized = value;
				IsVisibleIfAuthorize = value ? Visibility.Visible : Visibility.Collapsed;
				RaisePropertyChanged("IsAuthorized");
			}
		}

		private InvertableVisibility _isVisibleIfAuthorize;
		public InvertableVisibility IsVisibleIfAuthorize
		{
			get => _isVisibleIfAuthorize;
			set
			{
				_isVisibleIfAuthorize = value;
				RaisePropertyChanged("IsVisibleIfAuthorize");
			}
		}

		private bool _isLoading = false;
		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				_isLoading = value;
				RaisePropertyChanged("IsLoading");
			}
		}
		
		private List<Music> _musicCollection;
		public List<Music> MusicCollection
		{
			get => _musicCollection;
			set
			{
				_musicCollection = value;
				RaisePropertyChanged("MusicCollection");
			}
		}

		private List<Friend> _friendsList;
		public List<Friend> FriendsList
		{
			get => _friendsList;
			set
			{
				_friendsList = value;
				RaisePropertyChanged("FriendsList");
			}
		}

		private double _musicProgress;
		public double MusicProgress { get => _musicProgress;
			set
			{
				_musicProgress = value;
				IsVisibleIfProgressAboveZero = value > 0.0 ? Visibility.Visible : Visibility.Collapsed;
				RaisePropertyChanged("MusicProgress");
			}
		}

		private Visibility _isVisibleIfProgressAboveZero;
		public Visibility IsVisibleIfProgressAboveZero
		{
			get => _isVisibleIfProgressAboveZero;
			set
			{
				_isVisibleIfProgressAboveZero = value;
				RaisePropertyChanged("IsVisibleIfProgressAboveZero");
			}
		}
		#endregion
		private readonly IStartPageModel _model;
		private readonly ApplicationDataContainer _localSettings;
		private CancellationTokenSource _tokenSource;

		public StartPageViewModel(IStartPageModel model)
		{
			_localSettings = ApplicationData.Current.LocalSettings;
			IsAuthorized = (bool)_localSettings.Values["IsAuthorized"];
			MusicProgress = 0;
			_model = model;
			if (IsAuthorized.Value)
				_model.LogIn();
			FriendsList = new List<Friend>();
			MusicCollection = new List<Music>();
		}

		#region COMMANDS
		public ICommand LogInCommand => new CommandHandler(LogIn);
		public async Task LogIn()
		{
			_tokenSource?.Cancel();
			try
			{
				IsAuthorized = await _model.LogIn();
				_localSettings.Values["IsAuthorized"] = IsAuthorized.Value;
			}
			catch (AuthorizationFailedException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
		}

		public ICommand LogOutCommand => new CommandHandler(LogOut);
		public async Task LogOut()
		{
			_tokenSource?.Cancel();
			try
			{
				IsAuthorized = await _model.LogOut();
				_localSettings.Values["IsAuthorized"] = IsAuthorized.Value;
			}
			catch (HttpRequestException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
		}

		public ICommand GetFriendsCommand => new CommandHandler(GetFriends);
		public async Task GetFriends()
		{
			_tokenSource?.Cancel();
			FriendsList.Clear();
			try
			{
				var result = await _model.GetFriends();
				FriendsList = result;
				if (FriendsList?.Count == 0)
				{
					await new MessageDialog("Sorry, you have no friends installed this app").ShowAsync();
				}
			}
			catch (AccessDeniedException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
			catch (HttpRequestException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
		}

		public ICommand GetMusicCommand => new CommandHandler(GetMusic);
		public async Task GetMusic()
		{
			MusicCollection.Clear();
			_tokenSource = new CancellationTokenSource();
			var token = _tokenSource.Token;
			try
			{
				var progress = new Progress<double>(pr => MusicProgress = pr);
				var result = await _model.GetMusic(progress, token);
				MusicCollection = result;
				MusicProgress = 0;
				if (MusicCollection?.Count == 0)
				{
					await new MessageDialog("Sorry, your friends do not have any music liked").ShowAsync();
				}
			}
			catch (AccessDeniedException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
			catch (HttpRequestException e)
			{
				await new MessageDialog(e.Message).ShowAsync();
			}
			catch (OperationCanceledException e)
			{
				
			}
			finally
			{
				MusicProgress = 0;
				_tokenSource.Dispose();
				_tokenSource = null;
			}
		}
		#endregion
	}
}