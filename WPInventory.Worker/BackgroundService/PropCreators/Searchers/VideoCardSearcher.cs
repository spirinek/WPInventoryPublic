using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels;

namespace WPInventory.Worker.BackgroundService.PropCreators.Searchers
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
