using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GreyconChallenge.Base
{
    public class HardDisk
    {
        private int _used;
        private int _total;

        public HardDisk()
        {
            
        }
        
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

        public int Free => _total - _used;

        public void MoveData(int size, HardDisk dest)
        {
            // Check if size to move is allowed
            if (dest.Free < size)
            {
                throw new HardDiskException("Can't move data");
            }

            _used -= size;

            dest.Used += size;
        }

        public static HardDisk GenerateRandom()
        {
            HardDisk disk = new HardDisk();

            Random rnd = new Random();

            disk.Total = rnd.Next(1, 1000);
            disk.Used = rnd.Next(1, disk.Total);

            return disk;
        }
    }
}