using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPInventory.Data.Models.Entities
{
    public class Computer 
    {
        public Computer()
        {
            Monitors = new HashSet<Monitor>();
            NWAdapters = new HashSet<NWAdapter>();
            CPUs = new HashSet<CPU>();
            VideoCards = new HashSet<VideoCard>();
            PhisicalDisks = new HashSet<HDD>();
            NWAdapters = new HashSet<NWAdapter>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string OperatingSystem { get; set; }
        public ICollection<Monitor> Monitors { get; set; }
        public ICollection<NWAdapter> NWAdapters { get; set; }
        public ICollection<CPU> CPUs { get; set; }
        public ICollection<RAM> RAMs { get; set; }
        public ICollection<VideoCard> VideoCards { get; set; }
        public MotherBoard MotherBoard { get; set; }
        public ICollection<HDD> PhisicalDisks { get; set; }
        public ScanDate ScanDates { get; set; }
        public Guid Guid { get; set; } 
        public bool IsArchived { get; set; }
        public string Description { get; set; }

        
    }
}
