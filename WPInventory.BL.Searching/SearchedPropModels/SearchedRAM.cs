namespace WPInventory.BL.Searching.SearchedPropModels
{
    public class SearchedRAM : SearchedItem
    {
        public int Capacity { get; set; }
        public int Speed { get; set; }
        public string PartNumber { get; set; }
        public string MemoryType { get; set; }
        public string Manufacturer { get; set; }
    }
}
