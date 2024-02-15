using System;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public sealed class NWAdapterSearcher : BaseSearcher<SearchedNWAdapter>
    {
        public NWAdapterSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }

        private static readonly ObjectQuery _query = new ObjectQuery("Select * From win32_NetworkAdapter");
        private readonly ManagementScope _searchScope;
        private static string NetConnectionStatus = "NetConnectionStatus";
        private static string ProductName = "ProductName";
        private static string MACAddress = "MACAddress";
        private static string ServiceName = "ServiceName";
        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                var manageObjCollection = objectSearcher.Get();
                foreach (var manageObj in manageObjCollection)
                {
                    var searchedNwAdapter = new SearchedNWAdapter();
                    var props = manageObj.Properties.OfType<PropertyData>().ToList();

                    searchedNwAdapter.MACAddress = props.First(x => x.Name == MACAddress)?.Value?.ToString();
                    var netStatus = props.First(x => x.Name == NetConnectionStatus)?.Value;
                    if (netStatus != null)
                    {
                        searchedNwAdapter.NetConnectionStatus = Convert.ToInt32(netStatus);
                    }
                    searchedNwAdapter.ProductName = props.First(x => x.Name == ProductName)?.Value?.ToString();
                    searchedNwAdapter.ServiceName = props.First(x => x.Name == ServiceName)?.Value?.ToString();

                    _items.Add(searchedNwAdapter);
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
