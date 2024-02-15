using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.Searchers;
using WPInventory.Data.Models.Entities;

namespace WPInventory.BL.Searching
{
    public class ComputerBuilder
    {
        private readonly Computer _computer;
        private readonly CPUSearcher _cpuSearcher;
        private readonly HDDSearcher _hddSearcher;
        private readonly MonitorSearcher _monitorSearcher;
        private readonly MotherBoardSearcher _motherBoardSearcher;
        private readonly NWAdapterSearcher _nwAdapterSearcher;
        private readonly OSSearcher _osSearcher;
        private readonly RAMSearcher _ramSearcher;
        private readonly VideoCardSearcher _videoCardSearcher;
        private readonly ManagementScope _scopeCimv2;
        private readonly ManagementScope _scopeWMI;


        public ComputerBuilder(string computerName, ConnectionOptions connectionOptions)
        {
            _computer = new Computer
            {
                Name = computerName
            };

            _scopeCimv2 = new ManagementScope(new ManagementPath { NamespacePath = @"root\cimv2", Server = computerName }, connectionOptions);
            _scopeWMI = new ManagementScope(new ManagementPath { NamespacePath = @"root\wmi", Server = computerName }, connectionOptions);

            _cpuSearcher = new CPUSearcher(_scopeCimv2);
            _hddSearcher = new HDDSearcher(_scopeCimv2);
            _monitorSearcher = new MonitorSearcher(_scopeWMI);
            _motherBoardSearcher = new MotherBoardSearcher(_scopeCimv2);
            _nwAdapterSearcher = new NWAdapterSearcher(_scopeCimv2);
            _osSearcher = new OSSearcher(_scopeCimv2);
            _ramSearcher = new RAMSearcher(_scopeCimv2);
            _videoCardSearcher = new VideoCardSearcher(_scopeCimv2);
        }

        public Computer Build(out bool isOk)
        {
            _scopeCimv2.Connect();
            if (!_scopeCimv2.IsConnected)
            {
                isOk = false;
                return null;
            }
            _computer.CPUs = new List<CPU>();
            foreach (var searchedCPU in _cpuSearcher.Items)
            {
                var cpu = new CPU()
                {
                    Computer = _computer,
                    Name = searchedCPU.Name,
                    NumberOfCores = searchedCPU.NumberOfCores,
                    SerialNumber = searchedCPU.ProcessorId,
                    MaxClockSpeed = searchedCPU.MaxClockSpeed
                };
                _computer.CPUs.Add(cpu);
            }

            _computer.PhisicalDisks = new List<HDD>();
            foreach (var searchedHDD in _hddSearcher.Items)
            {
                if (!searchedHDD.Model.Contains("USB"))
                {
                    var hdd = new HDD()
                    {
                        Computer = _computer,
                        SerialNumber = searchedHDD.SerialNumber,
                        Size = searchedHDD.Size,
                        Model = searchedHDD.Model
                    };
                    _computer.PhisicalDisks.Add(hdd);
                }
            }

            _computer.Monitors = new List<Monitor>();
            _scopeWMI.Connect();
            if (_scopeWMI.IsConnected)
            {
                foreach (var searchedMonitor in _monitorSearcher.Items)
                {
                    var monitor = new Monitor()
                    {
                        Name = searchedMonitor.Name,
                        Computer = _computer,
                        SerialNumber = searchedMonitor.SerialNumber,
                        YearOfManufacture = searchedMonitor.YearOfManufacture,
                        LastSeen = DateTime.Now
                    };
                    _computer.Monitors.Add(monitor);
                }
            }

            var searchedMotherBoard = _motherBoardSearcher.Items.FirstOrDefault();
            if (searchedMotherBoard != null)
            {
                _computer.MotherBoard = new MotherBoard()
                {
                    Computer = _computer,
                    Manufacturer = searchedMotherBoard.Manufacturer,
                    Product = searchedMotherBoard.Product
                };
            }

            _computer.NWAdapters = new List<NWAdapter>();
            foreach (var searchedNWAdapter in _nwAdapterSearcher.Items)
            {
                if (searchedNWAdapter.NetConnectionStatus == 2)
                {
                    var adapter = new NWAdapter
                    {
                        Computer = _computer,
                        MAC = searchedNWAdapter.MACAddress,
                        ProductName = searchedNWAdapter.ProductName,
                        ServiceName = searchedNWAdapter.ServiceName
                    };
                    _computer.NWAdapters.Add(adapter);
                }
            }

            _computer.OperatingSystem = _osSearcher.Items.FirstOrDefault()?.Version;

            _computer.VideoCards = new List<VideoCard>();
            foreach (var searchedVC in _videoCardSearcher.Items)
            {
                if (!searchedVC.Name.Contains("Radmin"))
                {
                    var vc = new VideoCard()
                    {
                        Computer = _computer,
                        CardModel = searchedVC.Name
                    };
                    _computer.VideoCards.Add(vc);
                }
            }
            _computer.RAMs = new List<RAM>();
            foreach (var searchedRam in _ramSearcher.Items)
            {
                var memory = new RAM()
                {
                    Computer = _computer,
                    Manufacturer = searchedRam.Manufacturer,
                    Type = searchedRam.MemoryType,
                    PartNumber = searchedRam.PartNumber,
                    Capacity = searchedRam.Capacity,
                    Speed = searchedRam.Speed
                };
                _computer.RAMs.Add(memory);
            }

            _computer.ScanDates = new ScanDate()
            {
                Computer = _computer,
                Added = DateTime.Now,
                LastSeen = DateTime.Now
            };

            _computer.Guid = Guid.NewGuid();

            isOk = true;
            return _computer;
        }
    }
}
