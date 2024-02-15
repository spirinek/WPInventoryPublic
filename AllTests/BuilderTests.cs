using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators;
using Xunit;
using Xunit.Abstractions;

namespace AllTests
{
    public class BuilderTests
    {
        private readonly ITestOutputHelper _output;
        private readonly XunitLogger<BuilderTests> _logger;

        public BuilderTests(ITestOutputHelper output)
        {
            _output = output;
            _logger = new XunitLogger<BuilderTests>(output);
    }

        [Fact]
        public async Task TestBuilding()
        {
            var adResult = new AdCompResult()
            {
                ComputerName = Environment.MachineName
            };
            var options = new ConnectionOptions();
            var builder = new ComputerBuilder(adResult, options, _logger);
            var computer =await builder.Build();

            if (computer == null)
            {
                _output.WriteLine("An Error occurred");
                Assert.False(true);
            }
            var computerProps = computer.GetType().GetProperties();

            foreach (var computerProp in computerProps)
            {
                if (computerProp.PropertyType.IsPrimitive || computerProp.PropertyType == typeof(string))
                {
                    _output.WriteLine($"{computerProp.Name} : {computerProp.Name} : {computerProp.GetValue(computer)?.ToString() ?? "null"}");
                }
                else if (computerProp.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(computerProp.PropertyType.GetGenericTypeDefinition()))
                {
                    var list = computerProp.GetValue(computer) as IEnumerable;
                    var enumerator = list.GetEnumerator();
                    enumerator.MoveNext();
                    var firstItem = enumerator.Current;
                    list.GetEnumerator().Reset();
                    var itemProps = firstItem?.GetType().GetProperties();
                    if (itemProps != null)
                    {
                        foreach (var item in list)
                        {
                            foreach (var prop in itemProps)
                            {
                                if (prop.PropertyType != computer.GetType())
                                {
                                    _output.WriteLine($"{computerProp.Name} : {prop.Name} : {prop.GetValue(item)?.ToString() ?? "null"}");
                                }
                            }
                        }
                    }
                }

                else if (computerProp.PropertyType.IsClass)
                {
                    var item = computerProp.GetValue(computer);
                    var itemProps = item.GetType().GetProperties();
                    foreach (var prop in itemProps)
                    {
                        if (prop.PropertyType != computer.GetType())
                        {
                            _output.WriteLine($"{computerProp.Name} : {prop.Name} : {prop.GetValue(item)?.ToString() ?? "null"}");
                        }
                    }
                }
            }
        }
    }
}
