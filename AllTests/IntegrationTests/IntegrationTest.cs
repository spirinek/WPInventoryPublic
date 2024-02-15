using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;
using WPInventory.Data.Models.Helpers;
using WPInventory.Worker.BackgroundService.ConnectionManager;
using WPInventory.Worker.BackgroundService.PropCreators;
using Xunit;
using Xunit.Abstractions;

namespace AllTests.IntegrationTests
{
    public class IntegrationTest
    {
        private readonly ITestOutputHelper _output;
        private readonly XunitLogger<ComputerAnalyzer> _logger;
        private readonly IWMIConnectionManager _connectionManager;
        private readonly DbContextOptions<ComputerInfoContext> _dbOptions;
        public IntegrationTest(ITestOutputHelper output)
        {
            _output = output;
            _logger = new XunitLogger<ComputerAnalyzer>(output);
            var mock = new Mock<IWMIConnectionManager>();
            mock.Setup(x => x.GetOptions()).Returns(new ConnectionOptions());
            _connectionManager = mock.Object;

            var dbOptionsBuilder =
                new DbContextOptionsBuilder<ComputerInfoContext>().UseSqlServer(
                    "Data Source =.;Database=WpInventory; Integrated Security = true");

            _dbOptions = dbOptionsBuilder.Options;
        }
        [Fact]
        public async Task AnalyzerAddingTest()
        {
            var adComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "Egor's Computer"
            };

            var computer = await new ComputerBuilder(adComp, new ConnectionOptions(), _logger).Build();

            var analyzer = new ComputerAnalyzer(_logger, _dbOptions);
            await analyzer.AddOrUpdateComputerInDatabase(computer);

            using (var dbContext = new ComputerInfoContext(_dbOptions))
            {
                var result = await dbContext.Computers.Where(x => x.Guid == computer.Guid)
                    .FirstOrDefaultAsync();

                Assert.NotNull(result);

                dbContext.Computers.RemoveRange(result);
                await dbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AnalyzerDescriptionUpdating()
        {
            var adComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };

            var computer = await new ComputerBuilder(adComp, new ConnectionOptions(), _logger).Build();

            var otherAdComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "Test Description"
            };
            var otherComputer = await new ComputerBuilder(otherAdComp, new ConnectionOptions(), _logger).Build();

            var analyzer = new ComputerAnalyzer(_logger, _dbOptions);

            await analyzer.AddOrUpdateComputerInDatabase(computer);
            await analyzer.AddOrUpdateComputerInDatabase(otherComputer);

            using (var dbContext = new ComputerInfoContext(_dbOptions))
            {
                var archived = await dbContext.Computers.Where(x => x.Name == computer.Name && x.IsArchived)
                    .FirstOrDefaultAsync();

                var nonArchived = await dbContext.Computers.Where(x => x.Name == computer.Name && !x.IsArchived)
                    .FirstOrDefaultAsync();

                Assert.Equal(archived.Guid, nonArchived.Guid);

                dbContext.Computers.RemoveRange(archived, nonArchived);
                await dbContext.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task AnalyzerMonitorsUpdating()
        {
            var adComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };

            var computer = await new ComputerBuilder(adComp, new ConnectionOptions(), _logger).Build();

            var otherAdComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };
            var otherComputer = await new ComputerBuilder(otherAdComp, new ConnectionOptions(), _logger).Build();
            otherComputer.Monitors.Add(new Monitor()
            {
                Computer = otherComputer,
                Name = "Test Monitor",
                SerialNumber = "testserialnumber",
                LastSeen = DateTime.Now,
                YearOfManufacture = "2020"
            });
            var analyzer = new ComputerAnalyzer(_logger, _dbOptions);

            await analyzer.AddOrUpdateComputerInDatabase(computer);

            await analyzer.AddOrUpdateComputerInDatabase(otherComputer);

            using (var dbContext = new ComputerInfoContext(_dbOptions))
            {
                var archived = await dbContext.Computers.Where(x => x.Name == computer.Name && x.IsArchived)
                    .FirstOrDefaultAsync();

                var nonArchived = await dbContext.Computers.Where(x => x.Name == computer.Name && !x.IsArchived)
                    .FirstOrDefaultAsync();

                Assert.Equal(archived.Guid, nonArchived.Guid);

                dbContext.Computers.RemoveRange(archived, nonArchived);
                await dbContext.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task AnalyzerHWConfigurationUpdating()
        {
            var adComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };

            var computer = await new ComputerBuilder(adComp, new ConnectionOptions(), _logger).Build();
            
            var analyzer = new ComputerAnalyzer(_logger, _dbOptions);

            await analyzer.AddOrUpdateComputerInDatabase(computer);

            var otherAdComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };

            var otherComputer = await new ComputerBuilder(otherAdComp, new ConnectionOptions(), _logger).Build();

            otherComputer.PhisicalDisks.Add(new HDD()
            {
                Computer = otherComputer,
                Model = "Test HDD model",
                SerialNumber = "Test HDD sn",
                Size = "one milliard bytes"
            });

            await analyzer.AddOrUpdateComputerInDatabase(otherComputer);

            using (var dbContext = new ComputerInfoContext(_dbOptions))
            {
                var archived = await dbContext.Computers.Where(x => x.Name == computer.Name && x.IsArchived)
                    .FirstOrDefaultAsync();

                var nonArchived = await dbContext.Computers.Where(x => x.Name == computer.Name && !x.IsArchived)
                    .FirstOrDefaultAsync();

                Assert.Equal(archived.Guid, nonArchived.Guid);

                dbContext.Computers.RemoveRange(archived, nonArchived);
                await dbContext.SaveChangesAsync();
            }
        }
        [Fact]
        public async Task AnalyzerMonitorsOneDayWaiting()
        {
            var adComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };

            var computer = await new ComputerBuilder(adComp, new ConnectionOptions(), _logger).Build();
            computer.Monitors.Add(new Monitor()
            {
                Computer = computer,
                Name = "Test Monitor",
                SerialNumber = "testserialnumber",
                LastSeen = DateTime.Now.AddHours(-20),
                YearOfManufacture = "2020"
            });

            var analyzer = new ComputerAnalyzer(_logger, _dbOptions);

            await analyzer.AddOrUpdateComputerInDatabase(computer);

            var otherAdComp = new AdCompResult()
            {
                ComputerName = Environment.MachineName,
                Description = "My Computer"
            };
            
            var otherComputer = await new ComputerBuilder(otherAdComp, new ConnectionOptions(), _logger).Build();

            otherComputer.Monitors.Add(new Monitor()
            {
                Computer = otherComputer,
                Name = "Test Monitor",
                SerialNumber = "testserialnumber",
                LastSeen = otherComputer.ScanDates.LastSeen,
                YearOfManufacture = "2020"
            });

            await analyzer.AddOrUpdateComputerInDatabase(otherComputer);

            using (var dbContext = new ComputerInfoContext(_dbOptions))
            {
                var archived = await dbContext.Computers.Where(x => x.Name == computer.Name && x.IsArchived)
                    .Include(x=>x.Monitors)
                    .FirstOrDefaultAsync();

                var nonArchived = await dbContext.Computers.Where(x => x.Name == computer.Name && !x.IsArchived)
                    .Include(x => x.Monitors)
                    .FirstOrDefaultAsync();


                Assert.Null( archived);
                Assert.Equal(nonArchived.Monitors.ToList()[0].LastSeen, nonArchived.Monitors.ToList()[1].LastSeen);

                dbContext.Computers.RemoveRange(nonArchived);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
