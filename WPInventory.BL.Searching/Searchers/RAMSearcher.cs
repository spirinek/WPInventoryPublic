using System;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public sealed class RAMSearcher : BaseSearcher<SearchedRAM>
    {
        public RAMSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }
        private static string Capacity = "Capacity";
        private static string Manufacturer = "Manufacturer";
        private static string Speed = "Speed";
        private static string PartNumber = "PartNumber";
        private static string SMBIOSMemoryType = "SMBIOSMemoryType";
        private static string MemoryType = "MemoryType";
        private static readonly ObjectQuery _query = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
        private readonly ManagementScope _searchScope;

        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                foreach (var manageObj in objectSearcher.Get())
                {
                    var searchedMemory = new SearchedRAM();
                    var props = manageObj.Properties.OfType<PropertyData>().ToList();

                    var ramCapacity = props.FirstOrDefault(x => x.Name == Capacity)?.Value?.ToString();
                    if (ramCapacity != null)
                        searchedMemory.Capacity = Convert.ToInt32(Convert.ToUInt64(ramCapacity) / 1048576);

                    searchedMemory.Manufacturer = props.FirstOrDefault(x => x.Name == Manufacturer)?.Value?.ToString();

                    var ramSpeed = props.FirstOrDefault(x => x.Name == Speed)?.Value?.ToString();
                    if (ramSpeed != null)
                        searchedMemory.Speed = Convert.ToInt32(ramSpeed);

                    searchedMemory.PartNumber = props.FirstOrDefault(x => x.Name == PartNumber)?.Value?.ToString();

                    var ramMemoryType = props.FirstOrDefault(x => x.Name == MemoryType)?.Value?.ToString();
                    if (ramMemoryType != null && Convert.ToUInt32(ramMemoryType) != 0)
                    {
                        searchedMemory.MemoryType = ramMemoryType;
                    }
                    else
                    {
                        var ramBiosMemoryType = props.FirstOrDefault(x => x.Name == SMBIOSMemoryType)?.Value?.ToString();
                        if (ramBiosMemoryType != null && Convert.ToUInt32(ramBiosMemoryType) != 0)
                        {
                            searchedMemory.MemoryType = ramBiosMemoryType;
                        }
                    }

                    _items.Add(searchedMemory);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _searched = true;
            }
        }
    }
}
