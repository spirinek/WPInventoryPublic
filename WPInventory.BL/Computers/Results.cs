using System;
using System.Collections.Generic;

namespace WPInventory.BL.Computers
{
    public class GetAllComputersSimpleModelResult
    {
        public List<ComputerSimpleModelDto> Computers { get; set; }
    }

    public class GetConcreteComputerResult
    {
        public List<ConcreteComputerDto> AllStates{ get; set; }
    }
    public class ComputerSimpleModelDto
    {
        public Guid Guid { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Changed { get; set; }
        public DateTime LastSeen { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ConcreteComputerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OperatingSystem { get; set; }
        public List<MonitorDto> Monitors { get; set; }
        public List<NWAdapterDto> NWAdapters { get; set; }
        public List<CPUDto> CPUs { get; set; }
        public List<RAMDto> RAMs { get; set; }
        public List<VideoCardDto> VideoCards { get; set; }
        public MotherBoardDto MotherBoard { get; set; }
        public List<HDDDto> PhisicalDisks { get; set; }
        public Guid Guid { get; set; }
        public bool IsArchived { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Changed { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class MonitorDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string YearOfManufacture { get; set; }
        public DateTime? LastSeen { get; set; }
    }

    public class CPUDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public string MaxClockSpeed { get; set; }
        public string SerialNumber { get; set; }
    }

    public class RAMDto
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public int Speed { get; set; }
        public string PartNumber { get; set; }
        public string Type { get; set; }
    }

    public class VideoCardDto
    {
        public int Id { get; set; }
        public string CardModel { get; set; }
    }

    public class MotherBoardDto
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }
    }

    public class HDDDto
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
    }
    public class NWAdapterDto
    {
        public int Id { get; set; }
        public string MAC { get; set; }
        public string ProductName { get; set; }
        public string ServiceName { get; set; }
    }
}
