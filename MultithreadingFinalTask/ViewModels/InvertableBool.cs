namespace MultithreadingFinalTask.ViewModels
{
	/// <summary>
	/// Helper class to make some elements of View enabled and some other elements disabled being Bound to the same property in ViewModel
	/// </summary>
	public class InvertableBool
	{
		private bool value = false;

		public bool Value { get { return value; } }
		public bool Invert { get { return !value; } }

		public InvertableBool(bool b)
		{
			value = b;
		}

		public static implicit operator InvertableBool(bool b)
		{
			return new InvertableBool(b);
		}

		public static implicit operator bool(InvertableBool b)
		{
			return b.value;
		}

	}
}
