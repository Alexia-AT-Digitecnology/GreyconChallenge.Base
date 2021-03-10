using System;

namespace GreyconChallenge.Base
{
    public class HardDiskException : Exception
    {
        public HardDiskException()
        {
            
        }

        public HardDiskException(string message) : base(message)
        {
            
        }

        public HardDiskException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}