using System;

namespace WPInventory.Data.Models.Entities
{
   public class Monitor : IEquatable<Monitor>
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string YearOfManufacture { get; set; }
        public DateTime? LastSeen { get; set; }
        
        public bool Equals(Monitor other)
        {
            if (SerialNumber == other.SerialNumber
                && Name==other.Name)
                return true;
            return false;
        }

        public Monitor Copy()
        {
            return new Monitor()
            {
                SerialNumber = this.SerialNumber,
                Name = this.Name,
                YearOfManufacture = this.YearOfManufacture,
                LastSeen = this.LastSeen
            };
        }
    }
}
