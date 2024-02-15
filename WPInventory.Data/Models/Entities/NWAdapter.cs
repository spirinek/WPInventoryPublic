using System;

namespace WPInventory.Data.Models.Entities
{
    public class NWAdapter : IEquatable<NWAdapter>
    {
        public int Id { get; set; }
        public Computer Computer { get; set; }
        public int ComputerId { get; set; }
        public string MAC { get; set; }
        public string ProductName { get; set; }
        public string ServiceName { get; set; }

        public bool Equals(NWAdapter other)
        {
            if (other != null && this.MAC == other.MAC)
                return true;
            return false;
        }
    }
}
