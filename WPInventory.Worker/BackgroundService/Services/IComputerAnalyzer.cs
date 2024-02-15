using System.Threading.Tasks;
using WPInventory.Data.Models.Entities;
using WPInventory.Worker.BackgroundService.PropCreators;

namespace WPInventory.Worker.BackgroundService.Services
{
    public interface IComputerAnalyzer
    {
        Task AddOrUpdateComputerInDatabase(Computer computer);
    }
}
