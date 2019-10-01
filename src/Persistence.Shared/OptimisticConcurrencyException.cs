using System;

namespace OneCSharp.Persistence.Shared
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string message) : base(message) { }
    }
}
