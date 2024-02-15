using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using WPInventory.Worker.BackgroundService.PropCreators.Searchers;
using Xunit;
using Xunit.Abstractions;
using Monitor = WPInventory.Data.Models.Entities.Monitor;
namespace AllTests
{

    public class SearchersTest
    {
        private readonly ITestOutputHelper _output;

        public SearchersTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void MonitorSearcherTest()
        {
            var scopeWMI = new ManagementScope(new ManagementPath { NamespacePath = @"root\wmi" });
            var sw = Stopwatch.StartNew();
            var monitorSearcher = new MonitorSearcher(scopeWMI);
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            var searchedMonitors = monitorSearcher.Items;
            WriteProperties(searchedMonitors);
        }

        [Fact]
        public void MotherBoardSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            var sw = Stopwatch.StartNew();
            var mainBoardSearcher = new MotherBoardSearcher(scopeCimv2);
            var searchedItems = mainBoardSearcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);
        }

        [Fact]
        public void OSSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            var sw = Stopwatch.StartNew();
            var searcher = new OSSearcher(scopeCimv2);
            var searchedItems = searcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);

        }

        [Fact]
        public void RAMSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            var sw = Stopwatch.StartNew();
            var searcher = new RAMSearcher(scopeCimv2);
            var searchedItems = searcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);
        }

        [Fact]
        public void CPUSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            var sw = Stopwatch.StartNew();
            var searcher = new CPUSearcher(scopeCimv2);
            var searchedItems = searcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);
        }

        [Fact]
        public void NWAdapterSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            var sw = Stopwatch.StartNew();
            var searcher = new NWAdapterSearcher(scopeCimv2);
            var searchedItems = searcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);
        }


        [Fact]
        public void VideoCardSearcherTest()
        {
            ManagementScope scopeCimv2 = new ManagementScope(new ManagementPath
            {
                NamespacePath = @"root\cimv2"
            });
            scopeCimv2.Connect();
            if (scopeCimv2.IsConnected)
            {
                _output.WriteLine("Connected");
            }
            var sw = Stopwatch.StartNew();
            var searcher = new VideoCardSearcher(scopeCimv2);
            var searchedItems = searcher.Items;
            sw.Stop();
            _output.WriteLine(sw.Elapsed.ToString());
            WriteProperties(searchedItems);
        }
        private void WriteProperties<TItem>(List<TItem> items)
        {
            var props = items.FirstOrDefault()?.GetType().GetProperties();
            foreach (var item in items)
            {
                if (props!=null && props.Any())
                {
                    foreach (var prop in props)
                    {
                        _output.WriteLine($"{prop.Name} : {prop.GetValue(item)?.ToString() ?? "null"}");
                    }
                }
            }
        }
       
    }
}
