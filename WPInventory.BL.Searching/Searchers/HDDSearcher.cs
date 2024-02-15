using System;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public sealed class HDDSearcher : BaseSearcher<SearchedHDD>
    {
        public HDDSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }
        private static readonly ObjectQuery _query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
        private readonly ManagementScope _searchScope;
        private static string SerialNumber = "SerialNumber";
        private static string Model = "Model";
        private static string Size = "Size";
        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                var manageObjCollection = objectSearcher.Get();
                foreach (var manageObj in manageObjCollection)
                {
                    var hdd = new SearchedHDD();
                    var props = manageObj.Properties.OfType<PropertyData>().ToList();

                    hdd.Model = props.FirstOrDefault(x => x.Name == Model)?.Value?.ToString();
                    hdd.SerialNumber = props.FirstOrDefault(x => x.Name == SerialNumber)?.Value?.ToString();
                    hdd.Size = props.FirstOrDefault(x => x.Name == Size)?.Value?.ToString();
                    _items.Add(hdd);
                }
            }
            catch (Exception ex)
            {
                //TODO:logger
            }
            finally
            {
                _searched = true;
            }
        }
    }
}
