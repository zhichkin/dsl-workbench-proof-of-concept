using System;

namespace OneCSharp.Persistence.Shared
{
    public interface IVersion
    {
        byte[] Version { get; set; } // rowversion | timestamp
    }
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException() { }
        public OptimisticConcurrencyException(string message) : base(message) { }
    }
}
