using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;
using WPInventory.Data.Models.Helpers;
using WPInventory.Worker.BackgroundService.Services;

namespace WPInventory.Worker.BackgroundService.PropCreators
{
    public class ComputerAnalyzer : IComputerAnalyzer
    {
        private readonly ILogger _logger;
        private readonly DbContextOptions<ComputerInfoContext> _options;

        public ComputerAnalyzer(ILogger<ComputerAnalyzer> logger, DbContextOptions<ComputerInfoContext> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task AddOrUpdateComputerInDatabase(Computer comp)
        {
            if (comp == null)
            {
                return;
            }
            try
            {
                using var dbContext = new ComputerInfoContext(_options);
                var dateOfUpdating = comp.ScanDates.LastSeen.Value;
                var foundComp = await FindByName(comp, dbContext);
                if (foundComp != null)
                {
                    var equalResult = comp.CompareComputers(foundComp);
                    switch (equalResult)
                    {
                        case ComputerEqualResult.WrongComputer:  //archive this computer cause of name equality
                            await ArchiveFounded(foundComp, dbContext);
                            break;
                        case ComputerEqualResult.SameComputerWithSameConfig:
                            if (comp.MonitorEquals(foundComp))
                            {
                                foundComp.ScanDates.LastSeen = dateOfUpdating;
                                if (comp.Monitors.Any())
                                {
                                    foreach (var monitor in foundComp.Monitors)
                                    {
                                        monitor.LastSeen = dateOfUpdating;
                                    }
                                }
                                dbContext.Update(foundComp);
                                await dbContext.SaveChangesAsync();
                                _logger.LogInformation($"Configuration of {foundComp.Guid} hasn't changed");
                                return;
                            }
                            if (CompareMonitors(comp, foundComp))
                            {
                                foundComp.ScanDates.LastSeen = dateOfUpdating;
                                dbContext.Update(foundComp);
                                await dbContext.SaveChangesAsync();
                                _logger.LogInformation($"Configuration of {foundComp.Guid} hasn't changed");
                                return;
                            }
                            else
                            {
                                await AddNewState(comp, foundComp, dbContext);
                            }
                            return;
                        case ComputerEqualResult.SameComputerWithChangedConfig:
                            CompareMonitors(comp, foundComp);
                            await AddNewState(comp, foundComp, dbContext);
                            return;
                    }
                }

                foundComp = await FindByMACAddress(comp, dbContext);
                if (foundComp != null)
                {
                    var equalResult = comp.CompareComputers(foundComp);

                    switch (equalResult)
                    {
                        case ComputerEqualResult.WrongComputer: //archive this computer cause of MAC equality
                            await ArchiveFounded(foundComp, dbContext); 
                            break;
                        case ComputerEqualResult.SameComputerWithChangedConfig:
                            CompareMonitors(comp, foundComp);
                            await AddNewState(comp, foundComp, dbContext);
                            return;
                    }
                }

                dbContext.Computers.Add(comp);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"Added new Computer: {comp.Name}; Guid:{comp.Guid}");
            }
            catch (Exception e)
            {
                _logger.LogCritical($"An Error occurred while scanning the computer:{comp.Name}. Error occurred: { e.Message}");
            }
        }
        
        private async Task<Computer> FindByName(Computer comp, ComputerInfoContext dbContext)
        {
            var listOfResult = await dbContext.Computers.Where(x => x.Name == comp.Name)
                .Include(x => x.CPUs)
                .Include(x => x.Monitors)
                .Include(x => x.NWAdapters)
                .Include(x => x.RAMs)
                .Include(x => x.MotherBoard)
                .Include(x => x.PhisicalDisks)
                .Include(x => x.VideoCards)
                .Include(x => x.ScanDates)
                .ToListAsync();

            if (listOfResult.Any())
            {
                var foundComp = listOfResult?.OrderByDescending(x => x.ScanDates.LastSeen).FirstOrDefault();
                return foundComp;
            }

            return null;
        }

        private async Task<Computer> FindByMACAddress(Computer comp, ComputerInfoContext dbContext)
        {
            var listOfResult = await dbContext.Computers.Where(x =>  !string.IsNullOrEmpty(x.NWAdapters.FirstOrDefault().MAC)
                                                                 && x.NWAdapters.FirstOrDefault().MAC ==
                                                                 comp.NWAdapters.FirstOrDefault().MAC)
                .Include(x => x.CPUs)
                .Include(x => x.Monitors)
                .Include(x => x.NWAdapters)
                .Include(x => x.RAMs)
                .Include(x => x.MotherBoard)
                .Include(x => x.PhisicalDisks)
                .Include(x => x.VideoCards)
                .Include(x => x.ScanDates)
                .ToListAsync();

            if (listOfResult.Any())
            {
                var foundComp = listOfResult?.OrderByDescending(x => x.ScanDates.LastSeen).FirstOrDefault();
                return foundComp;
            }

            return null;
        }

        private bool CompareMonitors(Computer comp, Computer foundComp)
        {
            var monitors = new List<Monitor>();
            foreach (var monitor in comp.Monitors)
            {
                monitors.Add(monitor.Copy());
            }

            foreach (var monitor in foundComp.Monitors)
            {
                if (!comp.Monitors.Contains(monitor) && foundComp.ScanDates.LastSeen - monitor.LastSeen < TimeSpan.FromDays(1)) //waiting for 1 full day for monitor inactive (it can be power off)
                {
                    monitors.Add(monitor.Copy());
                }

                if (comp.Monitors.Contains(monitor))
                {
                    monitor.LastSeen = comp.ScanDates.LastSeen;
                }
                
            }
            if (!monitors.OrderBy(x =>x.Name).ThenBy(x=>x.SerialNumber).SequenceEqual(foundComp.Monitors.OrderBy(x => x.Name).ThenBy(x => x.SerialNumber)))
            {
                var newMonitors = new List<Monitor>();
                foreach (var monitor in monitors)
                {
                    var newMonitor = monitor.Copy();
                    newMonitor.Computer = comp;
                    newMonitors.Add(newMonitor);
                }
                comp.Monitors = newMonitors;
                return false;
            }
            return true;
        }

        private async Task AddNewState(Computer comp, Computer foundComp, ComputerInfoContext dbContext)
        {
            foundComp.IsArchived = true;
            comp.Guid = foundComp.Guid;
            comp.ScanDates.Added = foundComp.ScanDates.Added;
            dbContext.Update(foundComp);
            await dbContext.AddAsync(comp);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"Computer with guid {foundComp.Guid} has been found and updated");
        }

        private async Task ArchiveFounded(Computer foundComp, ComputerInfoContext dbContext)
        {
            if (foundComp.IsArchived == false)
            {
                if (foundComp.Monitors.Any())
                {
                    foreach (var monitor in foundComp.Monitors)
                    {
                        monitor.LastSeen = foundComp.ScanDates.LastSeen;
                    }
                }
                foundComp.IsArchived = true;
                dbContext.Update(foundComp);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"Computer Name:{foundComp.Name}; Guid:{foundComp.Guid} has been archived \n");
            }
        }
    }
}