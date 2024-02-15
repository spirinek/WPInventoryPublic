using System.Management;

namespace WPInventory.Worker.BackgroundService.ConnectionManager
{
    public interface IWMIConnectionManager
    {
        ConnectionOptions GetOptions();
    }
}
