using System;
using System.Linq;
using System.Management;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels;

namespace WPInventory.Worker.BackgroundService.PropCreators.Searchers
{
    public sealed class MotherBoardSearcher : BaseSearcher<SearchedMotherBoard>
    {
        public MotherBoardSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }
        private static readonly ObjectQuery _query = new ObjectQuery("Select * FROM Win32_BaseBoard");
        private readonly ManagementScope _searchScope;
        private static string Manufacturer = "Manufacturer";
        private static string Product = "Product";
        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                var manageObj = objectSearcher.Get().OfType<ManagementObject>().FirstOrDefault(); //больше двух материнок не бывает

                var searchedMainBoard = new SearchedMotherBoard();

                var props = manageObj?.Properties.OfType<PropertyData>().ToList();
                searchedMainBoard.Manufacturer = props.FirstOrDefault(x => x.Name == Manufacturer)?.Value?.ToString();
                searchedMainBoard.Product = props.FirstOrDefault(x => x.Name == Product)?.Value?.ToString();

                _items.Add(searchedMainBoard);
            }
            catch (Exception e)
            {
            }
            finally
            {
                _searched = true;
            }
        }
    }
}
