using System;
namespace DependencyResolution
{
    public class CircularReferenceException : Exception
    {
        public CircularReferenceException(string message)
            : base(message)
        {
        }
    }
}
