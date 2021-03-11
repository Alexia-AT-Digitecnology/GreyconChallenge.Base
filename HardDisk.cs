using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GreyconChallenge.Base
{
    /// <summary>
    /// Hard disk class
    /// </summary>
    public class HardDisk
    {
        private int _used = 1000;
        private int _total = 1000;

        /// <summary>
        /// Public constructor
        /// </summary>
        public HardDisk()
        {
            
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="used">Used MB</param>
        /// <param name="total">Total MB</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public HardDisk(int used, int total)
        {
            if (used < 1 || used > 1000)
            {
                throw new HardDiskException("Value must be between 1 and 1000");
            }
            
            if (total < 1 || total > 1000)
            {
                throw new HardDiskException("Value must be between 1 and 1000");
            }
            
            if (used > total)
            {
                throw new HardDiskException("Can't create hard disk");
            }
            
            _used = used;
            _total = total;
        }
        /// <summary>
        /// Used MB
        /// </summary>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public int Used
        {
            get => _used;
            set
            {
                if (value < 1 || value > 1000)
                {
                    throw new HardDiskException("Value must be between 1 and 1000");
                }
                
                if (value > _total)
                {
                    throw new HardDiskException("Total must be greater than used");
                }
                
                _used = value;
            } 
        }
        
        /// <summary>
        /// Total MB
        /// </summary>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public int Total
        {
            get => _total;
            set
            {
                if (value < 1 || value > 1000)
                {
                    throw new HardDiskException("Value must be between 1 and 1000");
                }

                if (value < _used)
                {
                    throw new HardDiskException("Total must be greater than used");
                }
                
                _total = value;
            } 
        }

        /// <summary>
        /// Free MB
        /// </summary>
        public int Free => _total - _used;

        /// <summary>
        /// Moves data from this disk to another
        /// </summary>
        /// <param name="size">MB to move</param>
        /// <param name="dest">Destination disk</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void MoveData(int size, HardDisk dest)
        {
            // Check if size to move is allowed
            if (dest.Free < size)
            {
                throw new HardDiskException("Can't move data");
            }

            if (_used - size < 0)
            {
                dest.Used += _used;
                
                _used = 0;
            }
            else
            {
                _used -= size;

                dest.Used += size;    
            }
        }

        /// <summary>
        /// Generates a random hard disk
        /// </summary>
        /// <returns>A random hard disk</returns>
        public static HardDisk GenerateRandom()
        {
            Random rnd = new Random();

            int total = rnd.Next(1, 1000);
            int used = rnd.Next(1, total);

            return new HardDisk(used, total);
        }
    }
}