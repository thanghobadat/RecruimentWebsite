using System;

namespace ViewModel.Exceptions
{
    public class OnlineLibraryException : Exception
    {
        public OnlineLibraryException()
        {
        }

        public OnlineLibraryException(string message)
            : base(message)
        {
        }

        public OnlineLibraryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
