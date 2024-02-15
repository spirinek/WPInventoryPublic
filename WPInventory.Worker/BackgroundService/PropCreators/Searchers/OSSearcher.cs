using System;
using System.Linq;
using System.Management;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels;

namespace WPInventory.Worker.BackgroundService.PropCreators.Searchers
{
    public sealed class OSSearcher : BaseSearcher<SearchedOS>
    {
        public OSSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }
        private static readonly ObjectQuery _query = new ObjectQuery("SELECT version FROM Win32_OperatingSystem");
        private readonly ManagementScope _searchScope;
        private static string Version = "Version";

        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                foreach (var manageObj in objectSearcher.Get())
                {
                    var searchedOS = new SearchedOS();
                    var props = manageObj?.Properties.OfType<PropertyData>().ToList();

                    searchedOS.Version = props?.FirstOrDefault(x => x.Name == Version)?.Value?.ToString();
                    _items.Add(searchedOS);
                }
            }
            catch (Exception ex)
            {
                // 
            }
            finally
            {
                _searched = true;
            }
        }
    }
}
