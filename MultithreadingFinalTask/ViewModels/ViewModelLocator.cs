using GalaSoft.MvvmLight.Views;
using MultithreadingFinalTask.Models;
using Autofac;
using MultithreadingFinalTask.Models.SocialNetwork;

namespace MultithreadingFinalTask.ViewModels
{/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary> 
	public class ViewModelLocator
	{
		private IContainer Container { get; set; }

		public ViewModelLocator()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<NavigationService>().As<INavigationService>().InstancePerLifetimeScope();
			builder.RegisterType<StartPageViewModel>().AsSelf().InstancePerDependency();
			builder.RegisterType<StartPageModel>().As<IStartPageModel>().InstancePerDependency();
			builder.RegisterType<FacebookSocialNetwork>().As<ISocialNetwork>().InstancePerDependency();
			Container = builder.Build();
		}

		public StartPageViewModel StartPageInstance => Container.Resolve<StartPageViewModel>();

		public static void Cleanup()
		{
			
		}
	}


}