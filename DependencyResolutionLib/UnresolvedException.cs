using System;
namespace DependencyResolution
{
    public class UnresolvedException : Exception
    {
        public UnresolvedException(string message) : base(message)
        {
        }
    }
}
