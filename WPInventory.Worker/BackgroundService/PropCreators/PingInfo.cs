using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WPInventory.Worker.BackgroundService.PropCreators
{
    public static class PingInfo
    {
        public static async Task<bool> Pinging(string name, ILogger logger)
        {
            using var pingSender = new Ping();
            try
            {
                PingReply reply = await pingSender.SendPingAsync(name);
                if (reply.Status == IPStatus.Success)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                logger.LogWarning($"An Error occurred during pinging host {name}: {e.Message}");
                return false;
            }
        }
    }
}
