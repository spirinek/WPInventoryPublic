using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPInventory.Data.Models.Entities
{
    public class MotherBoard : IEquatable<MotherBoard>
    {

        //[Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [ForeignKey("Computer")]
        public int Id { get; set; }
        public Computer Computer { get; set; }
        public string Manufacturer { get; set; }
        public string Product { get; set; }

        public bool Equals(MotherBoard other)
        {
            if (Manufacturer == other.Manufacturer
                && Product == other.Product)
                return true;
            return false;
        }
    }
}
