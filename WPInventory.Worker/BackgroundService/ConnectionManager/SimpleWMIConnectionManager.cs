using System.Management;

namespace WPInventory.Worker.BackgroundService.ConnectionManager
{
    public class SimpleWMIConnectionManager : IWMIConnectionManager
    {
        public ConnectionOptions GetOptions()
        {
            ConnectionOptions options = new ConnectionOptions
            {
                //use domain admin credentials
                Username = @"",
                Password = @"",
                EnablePrivileges = true,
                Authentication = AuthenticationLevel.Packet
            };
            return options;
        }
    }
}
