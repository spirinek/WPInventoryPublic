using System;

namespace WPInventory.Data.Models.Entities
{

    public class RAM :  IEquatable<RAM>
    {
        //[Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public int Speed { get; set; }
        public string PartNumber { get; set; }
        public string Type { get; set; }

        public bool Equals(RAM other)
        {
            if (Capacity == other.Capacity
                && Manufacturer == other.Manufacturer
                && Speed == other.Speed
                && PartNumber == other.PartNumber)
                return true;
            return false;
        }
    }
}
