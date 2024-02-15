using System;

namespace WPInventory.Data.Models.Entities
{
    public class CPU : IEquatable<CPU>
    {
        // [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Computer Computer { get; set; }
        public int ComputerId { get; set; }
        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public string MaxClockSpeed { get; set; }
        public string SerialNumber { get; set; }

        public bool Equals(CPU other)
        {
            if (Name == other.Name
                && NumberOfCores == other.NumberOfCores
                && MaxClockSpeed == other.MaxClockSpeed)
                return true;
            return false;
        }
    }
}
