using System;
using System.Collections;
using System.Collections.Generic;

namespace WPInventory.Data.Models.Entities

{
    public class HDD : IEquatable<HDD>
    {
        //[Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
        public string Size { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }


        public bool Equals(HDD other)
        {
            if (other!=null && Size == other.Size && Model == other.Model)
                return true;
            return false;
        }
    }

    public class HddComparer : IEqualityComparer<HDD>
    {
        public bool Equals(HDD x, HDD y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x.SerialNumber == y.SerialNumber && x.Model == y.Model && x.Size == y.Size)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(HDD obj)
        {
            return base.GetHashCode();
        }
    }
}
