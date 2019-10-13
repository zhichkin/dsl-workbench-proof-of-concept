using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IOptimisticConcurrencyObject
    {
        byte[] Version { get; set; } // timestamp | rowversion
    }
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string message) : base(message) { }
    }
}
