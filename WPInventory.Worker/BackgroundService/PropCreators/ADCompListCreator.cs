using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WPInventory.Worker.BackgroundService.PropCreators
{
    
    public static class ADCompListCreator
    {
        public static List<AdCompResult> GetADComputers(IEnumerable<string> ldapStrings, ILogger logger)
        {
            var adComps = new List<AdCompResult>();

            foreach (var ldap in ldapStrings)
            {
                using var directoryEntry = new DirectoryEntry(ldap);
                using var searcher = new DirectorySearcher(directoryEntry)
                {
                    PageSize = int.MaxValue,
                    Filter = "(objectClass=computer)",
                    SearchScope = SearchScope.Subtree
                };
                searcher.PropertiesToLoad.Add("Name");
                searcher.PropertiesToLoad.Add("Description");

                using var searchСollection = searcher.FindAll();

                var searchedColl = searchСollection.OfType<SearchResult>().ToList();
                if (searchedColl.Any())
                    foreach (SearchResult res in searchСollection)
                    {
                        adComps.Add(new AdCompResult
                        {
                            ComputerName = res.GetDirectoryEntry().Properties["Name"].Value.ToString(),
                            Description = res.GetDirectoryEntry().Properties["Description"].Value?.ToString() ?? ""
                        });
                    }
                logger.LogInformation("Active directory information has been collected");
            }
            return adComps.GroupBy(x => x.ComputerName).Select(x => x.First()).ToList();
        }
    }
    public class AdCompResult
    {
        public string ComputerName { get; set; }
        public string Description { get; set; }
    }
}
