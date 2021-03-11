using System;

namespace GreyconChallenge.Base
{
    /// <summary>
    /// Hard disk exception class
    /// </summary>
    public class HardDiskException : Exception
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public HardDiskException()
        {
            
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public HardDiskException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public HardDiskException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}