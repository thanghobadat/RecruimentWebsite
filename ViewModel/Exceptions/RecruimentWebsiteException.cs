using System;

namespace ViewModel.Exceptions
{
    public class RecruimentWebsiteException : Exception
    {
        public RecruimentWebsiteException()
        {
        }

        public RecruimentWebsiteException(string message)
            : base(message)
        {
        }

        public RecruimentWebsiteException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
