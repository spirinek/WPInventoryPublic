using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPInventory.Data.Models.Entities
{
    public class ScanDate 
    {

        //[Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [ForeignKey("Computer")]
        public int Id { get; set; }
        public Computer Computer { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Changed { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
