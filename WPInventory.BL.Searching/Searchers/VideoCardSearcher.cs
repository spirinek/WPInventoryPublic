using System;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public sealed class VideoCardSearcher : BaseSearcher<SearchedVideoCard>
    {
        public VideoCardSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }
        private static readonly ObjectQuery _query = new ObjectQuery("SELECT * FROM Win32_Videocontroller");
        private readonly ManagementScope _searchScope;
        public static string Name = "Name";
        protected override void Search()
        {
            try
            {
                using var search = new ManagementObjectSearcher(_searchScope, _query);
                foreach (var searchedObj in search.Get())
                {
                    var searchedVideoCard = new SearchedVideoCard();
                    var props = searchedObj.Properties.OfType<PropertyData>();
                    searchedVideoCard.Name = props.FirstOrDefault(x => x.Name == Name)?.Value.ToString();
                    _items.Add(searchedVideoCard);
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
