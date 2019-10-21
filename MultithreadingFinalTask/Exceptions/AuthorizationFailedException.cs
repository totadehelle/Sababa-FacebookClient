using System;

namespace MultithreadingFinalTask.Exceptions 
{
	class AuthorizationFailedException : Exception
	{
		public AuthorizationFailedException()
		{

		}

		public AuthorizationFailedException(string message)
			: base(message)
		{

		}

		public AuthorizationFailedException(string message, Exception inner)
			: base(message, inner)
		{

		}
	}
}
