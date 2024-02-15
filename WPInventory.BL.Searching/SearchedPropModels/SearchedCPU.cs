namespace WPInventory.BL.Searching.SearchedPropModels
{
    public class SearchedCPU : SearchedItem
    {
        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public string MaxClockSpeed { get; set; }
        public string ProcessorId { get; set; }
    }
}
