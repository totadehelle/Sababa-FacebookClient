using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultithreadingFinalTask.ViewModels
{
	class CommandHandler : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private readonly Func<Task> _func;

		public CommandHandler(Func<Task> func)
		{
			this._func = func;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			this._func();
		}
	}
}
