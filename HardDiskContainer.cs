using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GreyconChallenge.Base
{
    public class HardDiskContainer
    {
        private List<HardDisk> _disks = new List<HardDisk>();

        public HardDiskContainer()
        {
            
        }

        public void AddDisk(HardDisk disk)
        {
            _disks.Add(disk);
        }

        public void DeleteDisk(int index)
        {
            if (index > 0 || index >= _disks.Count)
            {
                throw new HardDiskException("Index is not valid");
            }
            
            _disks.RemoveAt(index);
        }

        public HardDisk[] ListDisks()
        {
            return _disks.ToArray();
        }

        public int DiskCount()
        {
            return _disks.Count;
        }

        public int Preconciliate()
        {
            // Create a copy of the drives
            List<HardDisk> diskscopy = new List<HardDisk>(_disks.ToArray());

            // Make conciliation
            bool dataMoved = false;
            
            while (dataMoved == true)
            {
                dataMoved = false;
                
                for (int i = diskscopy.Count - 1; i >= 0; i--)
                {
                    if (diskscopy[i - 1].Free > 0 && i != 0)
                    {
                        dataMoved = true;
                        diskscopy[i].MoveData(diskscopy[i - 1].Free, diskscopy[i - 1]);
                    }
                }
            }
            
            // Remove unused disks
            foreach (HardDisk disk in diskscopy)
            {
                if (disk.Used == 0)
                {
                    diskscopy.Remove(disk);
                }
            }

            return diskscopy.Count;
        }
        
        public void Conciliate()
        {
            // Make conciliation
            bool dataMoved = false;
            
            while (dataMoved == true)
            {
                dataMoved = false;
                
                for (int i = _disks.Count - 1; i >= 0; i--)
                {
                    if (_disks[i - 1].Free > 0 && i != 0)
                    {
                        dataMoved = true;
                        _disks[i].MoveData(_disks[i - 1].Free, _disks[i - 1]);
                    }
                }
            }
            
            // Remove unused disks
            foreach (HardDisk disk in _disks)
            {
                if (disk.Used == 0)
                {
                    _disks.Remove(disk);
                }
            }
        }

        public void LoadFromFile(string fileName)
        {
            _disks = JsonConvert.DeserializeObject<List<HardDisk>>(File.ReadAllText(fileName));
        }

        public void SaveToFile(string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(_disks));
        }

        public void GenerateRandom()
        {
            Random rnd = new Random();

            int numdisks = rnd.Next(1, 50);

            for (int i = 0; i < numdisks - 1; i++)
            {
                this.AddDisk(HardDisk.GenerateRandom());
            }
        }
    }
}