using System;

namespace BookVerse.Validation
{
    public class DuplicateTitleException : Exception
    {
        public DuplicateTitleException() : base() { }

        public DuplicateTitleException(string message) : base(message) { }
    }
}