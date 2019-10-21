using Windows.UI.Xaml;

namespace MultithreadingFinalTask.ViewModels
{
	/// <summary>
	/// Helper class to make some elements of View visible and some other elements collapsed being Bound to the same property in ViewModel
	/// </summary>
	public class InvertableVisibility
	{
		private Visibility _value = Visibility.Collapsed;

		public Visibility Value => _value;

		public Visibility Invert
		{
			get
			{
				if (_value == Visibility.Collapsed)
					return Visibility.Visible;
				else return Visibility.Collapsed;
			}
		}

		public InvertableVisibility(Visibility visibility)
		{
			_value = visibility;
		}

		public static implicit operator InvertableVisibility(Visibility visibility)
		{
			return new InvertableVisibility(visibility);
		}

		public static implicit operator Visibility(InvertableVisibility visibility)
		{
			return visibility._value;
		}
	}
}