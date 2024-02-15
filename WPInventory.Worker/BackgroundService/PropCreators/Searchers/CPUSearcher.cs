using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators.SearchedPropModels;

namespace WPInventory.Worker.BackgroundService.PropCreators.Searchers
{
    public sealed class CPUSearcher : BaseSearcher<SearchedCPU>
    {
        public CPUSearcher(ManagementScope searchScope)
        {
            _searchScope = searchScope;
        }

        private readonly ManagementScope _searchScope;
        private static readonly ObjectQuery _query = new ObjectQuery("Select * FROM Win32_Processor");
        private static string Name = "Name";
        private static string NumberOfCores = "NumberOfCores";
        private static string MaxClockSpeed = "MaxClockSpeed";
        private static string ProcessorId = "ProcessorId";
        protected override void Search()
        {
            try
            {
                using var objectSearcher = new ManagementObjectSearcher(_searchScope, _query);
                foreach (var manageObj in objectSearcher.Get())
                {
                    var searchedCPU = new SearchedCPU();
                    var props = manageObj.Properties.OfType<PropertyData>().ToList();
                    searchedCPU.Name = props.FirstOrDefault(x => x.Name == Name)?.Value?.ToString();

                    var coreNumbersString = props.FirstOrDefault(x => x.Name == NumberOfCores)?.Value?.ToString();
                    if (coreNumbersString != null)
                    {
                        searchedCPU.NumberOfCores = Convert.ToInt32(coreNumbersString);
                    }

                    searchedCPU.MaxClockSpeed = props.FirstOrDefault(x => x.Name == MaxClockSpeed)?.Value?.ToString();
                    searchedCPU.ProcessorId = props.FirstOrDefault(x => x.Name == ProcessorId)?.Value?.ToString();

                    _items.Add(searchedCPU);
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
    }
}
