using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GreyconChallenge.Base
{
    /// <summary>
    /// Hard disk container class
    /// </summary>
    public class HardDiskContainer
    {
        private List<HardDisk> _disks = new List<HardDisk>();

        /// <summary>
        /// Public constructor
        /// </summary>
        public HardDiskContainer()
        {
            
        }

        /// <summary>
        /// Add disk of the specified used size and total
        /// </summary>
        /// <param name="used">Used MB</param>
        /// <param name="total">Total MB</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void AddDisk(int used, int total)
        {
            if (_disks.Count == 50)
            {
                throw new HardDiskException("Maximum number of disks exceeded");
            }
            
            HardDisk disk = new HardDisk(used, total);
            
            this.AddDisk(disk);
        }

        /// <summary>
        /// Adds a hard disk
        /// </summary>
        /// <param name="disk">The disk to add</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void AddDisk(HardDisk disk)
        {
            if (_disks.Count == 50)
            {
                throw new HardDiskException("Maximum number of disks exceeded");
            }
            
            _disks.Add(disk);
        }

        /// <summary>
        /// Get specified disk index
        /// </summary>
        /// <param name="index">The index to get</param>
        /// <returns>The hard disk in teh specified index</returns>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public HardDisk GetDisk(int index)
        {
            if (index < 0 || index >= _disks.Count)
            {
                throw new HardDiskException("Index is not valid");
            }

            return _disks[index];
        }

        /// <summary>
        /// Deletes disk of specified index
        /// </summary>
        /// <param name="index">The index of the disk to delete</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void DeleteDisk(int index)
        {
            if (index < 0 || index >= _disks.Count)
            {
                throw new HardDiskException("Index is not valid");
            }
            
            _disks.RemoveAt(index);
        }

        /// <summary>
        /// Set disk used MB
        /// </summary>
        /// <param name="index">The index of the disk to set</param>
        /// <param name="size">Used MB to set</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void SetDiskUsed(int index, int size)
        {
            if (index < 0 || index >= _disks.Count)
            {
                throw new HardDiskException("Index is not valid");
            }

            _disks[index].Used = size;
        }
        
        /// <summary>
        /// Set disk total MB
        /// </summary>
        /// <param name="index">The index of the disk to set</param>
        /// <param name="size">The total MB to set</param>
        /// <exception cref="HardDiskException">A hard disk exception is thrown</exception>
        public void SetDiskTotal(int index, int size)
        {
            if (index < 0 || index >= _disks.Count)
            {
                throw new HardDiskException("Index is not valid");
            }
            
            _disks[index].Total = size;
        }

        /// <summary>
        /// List all the hard disks in the container
        /// </summary>
        /// <returns>All the hard disks in the container as an array</returns>
        public HardDisk[] ListDisks()
        {
            return _disks.ToArray();
        }

        /// <summary>
        /// Get the number of disks in the container
        /// </summary>
        /// <returns>The number of the disks in the container</returns>
        public int DiskCount()
        {
            return _disks.Count;
        }

        /// <summary>
        /// Preconciliates the disks
        /// </summary>
        /// <returns>The minimum number of disks that can hold all the data</returns>
        public int Preconciliate()
        {
            // Create a copy of the drives
            List<HardDisk> diskscopy = new List<HardDisk>();

            foreach (HardDisk disk in _disks)
            {
                HardDisk ndisk = new HardDisk(disk.Used, disk.Total);

                diskscopy.Add(ndisk);
            }

            // Make conciliation
            bool dataMoved = true;
            
            while (dataMoved == true)
            {
                dataMoved = false;

                for (int i = diskscopy.Count - 1; i >= 0; i--)
                {
                    if (i != 0 && diskscopy[i].Used > 0 && diskscopy[i - 1].Free > 0)
                    {
                        dataMoved = true;
                        diskscopy[i].MoveData(diskscopy[i - 1].Free, diskscopy[i - 1]);
                    }
                }
            }
            
            // Remove unused disks
            for (int i = diskscopy.Count - 1; i >= 0; i--) 
            {
                if (diskscopy[i].Used == 0)
                {
                    diskscopy.RemoveAt(i);
                }
            }

            return diskscopy.Count;
        }
        
        /// <summary>
        /// Conciliates the disks
        /// </summary>
        public void Conciliate()
        {
            // Make conciliation
            bool dataMoved = true;
            
            while (dataMoved == true)
            {
                dataMoved = false;
                
                for (int i = _disks.Count - 1; i >= 0; i--)
                {
                    if (i != 0 && _disks[i].Used > 0 && _disks[i - 1].Free > 0)
                    {
                        dataMoved = true;
                        _disks[i].MoveData(_disks[i - 1].Free, _disks[i - 1]);
                    }
                }
            }
            
            // Remove unused disks
            for (int i = _disks.Count - 1; i >= 0; i--) 
            {
                if (_disks[i].Used == 0)
                {
                    _disks.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Loads the disks from a file
        /// </summary>
        /// <param name="fileName">File path to load</param>
        public void LoadFromFile(string fileName)
        {
            _disks = JsonConvert.DeserializeObject<List<HardDisk>>(File.ReadAllText(fileName));
        }

        /// <summary>
        /// Saves the disks to a file
        /// </summary>
        /// <param name="fileName">File path to save</param>
        public void SaveToFile(string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(_disks));
        }

        /// <summary>
        /// Generate a random disks environment for testing purposes
        /// </summary>
        public void GenerateRandom()
        {
            _disks.Clear();
            
            Random rnd = new Random();

            int numdisks = rnd.Next(1, 50);

            for (int i = 0; i < numdisks - 1; i++)
            {
                this.AddDisk(HardDisk.GenerateRandom());
            }
        }
    }
}