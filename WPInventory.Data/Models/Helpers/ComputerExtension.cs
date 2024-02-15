using System;
using System.Linq;
using WPInventory.Data.Models.Entities;

namespace WPInventory.Data.Models.Helpers
{
    public static class ComputerExtension
    {
        public static ComputerEqualResult CompareComputers(this Computer computer, Computer other)
        {
            var hardwareEqual = computer.HardwareEquals(other);
            var osEqual = computer.OSEqual(other);
            var signatureEqual = computer.SignatureEquals(other);

            if (!signatureEqual)
            {
                return ComputerEqualResult.WrongComputer;
            }

            if (!osEqual || !hardwareEqual)
            {
                return ComputerEqualResult.SameComputerWithChangedConfig;
            }

            return ComputerEqualResult.SameComputerWithSameConfig;
        }
        public static bool HardwareEquals(this Computer computer, Computer other) //all hardware exclude network adapters
        {
            if (
                computer.RAMs.Count == other.RAMs.Count
                && computer.RAMs.OrderBy(r => r.Capacity).ThenBy(r => r.Manufacturer).SequenceEqual(other.RAMs.OrderBy(r => r.Capacity).ThenBy(r => r.Manufacturer))
                && computer.PhisicalDisks.Count == other.PhisicalDisks.Count
                && computer.PhisicalDisks.OrderBy(r => r.Size).ThenBy(r => r.Model).SequenceEqual(other.PhisicalDisks.OrderBy(r => r.Size).ThenBy(r => r.Model))
                && computer.MotherBoard.Equals(other.MotherBoard)
                && computer.CPUs.Count == other.CPUs.Count
                && computer.CPUs.OrderBy(c => c.Name).ThenBy(c => c.NumberOfCores).ThenBy(c => c.MaxClockSpeed)
                    .SequenceEqual(other.CPUs.OrderBy(c => c.Name).ThenBy(c => c.NumberOfCores).ThenBy(c => c.MaxClockSpeed))
                && computer.VideoCards.Count == other.VideoCards.Count
                && computer.VideoCards.OrderBy(v => v.CardModel).SequenceEqual(other.VideoCards.OrderBy(v => v.CardModel))
                && computer.NWAdapters.Count == other.NWAdapters.Count
                && computer.NWAdapters.OrderBy(m => m.MAC).SequenceEqual(other.NWAdapters.OrderBy(m => m.MAC)))
            {
                return true;
            }
            return false;
        }

        public static bool SignatureEquals(this Computer computer, Computer other)
        {
            var firstOrDefault = computer.NWAdapters.FirstOrDefault(x => !string.IsNullOrEmpty(x.MAC));
            if (computer.MotherBoard.Equals(other.MotherBoard)
                && computer.CPUs.Count == other.CPUs.Count
                && computer.CPUs.OrderBy(c => c.Name).ThenBy(c => c.NumberOfCores).ThenBy(c => c.MaxClockSpeed)
                    .SequenceEqual(other.CPUs.OrderBy(c => c.Name).ThenBy(c => c.NumberOfCores).ThenBy(c => c.MaxClockSpeed))
                && (computer.PhisicalDisks.Any(x => other.PhisicalDisks.Contains(x))
                    || (firstOrDefault != null
                    && firstOrDefault.Equals(other.NWAdapters.FirstOrDefault(x => !string.IsNullOrEmpty(x.MAC))))))

            {
                return true;
            }
            return false;
        }

        public static bool OSEqual(this Computer computer, Computer other)
        {
            if (computer.OperatingSystem == other.OperatingSystem
                && computer.Description == other.Description)
            {
                return true;
            }
            return false;
        }
        public static bool NWAdaptersEqual(this Computer computer, Computer other)
        {
            if (computer.NWAdapters.Count == other.NWAdapters.Count
                && computer.NWAdapters.OrderBy(m => m.MAC).SequenceEqual(other.NWAdapters.OrderBy(m => m.MAC)))
            {
                return true;
            }
            return false;
        }
        public static bool MonitorEquals(this Computer computer, Computer other)
        {
            if (computer.Monitors == null && other.Monitors == null)
                return true;
            if (computer.Monitors?.Count != other.Monitors?.Count)
                return false;
            if (computer.Monitors.OrderBy(m => m.SerialNumber).SequenceEqual(other.Monitors.OrderBy(m => m.SerialNumber)))
                return true;
            return false;
        }
    }
    public enum ComputerEqualResult
    {
        WrongComputer,
        SameComputerWithSameConfig,
        SameComputerWithChangedConfig
    }
}

