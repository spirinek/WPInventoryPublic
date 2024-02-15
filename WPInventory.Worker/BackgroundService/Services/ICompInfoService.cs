using System.Threading.Tasks;

namespace WPInventory.Worker.BackgroundService.Services
{
    public interface ICompInfoService
    {
        Task CreateInfoAsync();
    }
}
