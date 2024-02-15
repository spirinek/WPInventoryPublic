using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using WPInventory.BL.Searching.SearchedPropModels;

namespace WPInventory.BL.Searching.Searchers
{
    public sealed class MonitorSearcher : BaseSearcher<SearchedMonitor>
    {
        public MonitorSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }

        private static readonly ObjectQuery _query = new ObjectQuery("Select * from WmiMonitorId");
        private readonly ManagementScope _searchScope;
        private static string UserFriendlyName = "UserFriendlyName";
        private static string SerialNumberID = "SerialNumberID";
        private static string ManufacturerName = "ManufacturerName";
        private static string YearOfManufacture = "YearOfManufacture";

        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                var manageObjCollection = objectSearcher.Get();
                foreach (var manageObj in manageObjCollection)
                {
                    var monitor = new SearchedMonitor();
                    var props = manageObj.Properties.OfType<PropertyData>().ToList();

                    monitor.Name = GetPropertyValue(props, UserFriendlyName);
                    monitor.SerialNumber = GetPropertyValue(props, SerialNumberID);
                    monitor.Manufacturer = GetPropertyValue(props, ManufacturerName);
                    monitor.YearOfManufacture = props.FirstOrDefault(x => x.Name == YearOfManufacture)?.Value?.ToString();

                    _items.Add(monitor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске мониторов у {_searchScope.Path.Server} : {ex.Message}");
            }
            finally
            {
                _searched = true;
            }
        }

        //эти параметры хранятся в виде  массива символов в кодировке ASCII (Dec) и содержат еще пачку нулей в конце, конвертируем:

        public static string GetPropertyValue(List<PropertyData> props, string propertyName)
        {
            var obj = props?.FirstOrDefault(x => x.Name == propertyName)?.Value;

            if (obj != null && obj is short[])
            {
                Span<short> dirtySymbols = (short[])obj;
                var dirtyIndex = dirtySymbols.IndexOf((short)0);
                var clearSymbols = dirtySymbols.Slice(0, dirtyIndex);
                var resultArray = new char[dirtyIndex];
                for (int i = 0; i < dirtyIndex; i++)
                {
                    resultArray[i] = Convert.ToChar(clearSymbols[i]);
                }
                var result = new string(resultArray);
                return result;
            }
            return null;
        }
    }
}

