using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ByteWizardApi.BackgroundService
{
    /// <summary>
    /// Represents a background service that monitors the WireGuard connection and attempts to reconnect if the connection is lost.
    /// </summary>
    public sealed class WireGuardMonitoringService : Microsoft.Extensions.Hosting.BackgroundService
    {
        /// <summary>
        /// An instance of <see cref="ILogger{TCategoryName}"/> used for logging diagnostic information and tracing within the WireGuardMonitoringService.
        /// </summary>
        private readonly ILogger<WireGuardMonitoringService> _logger;

        /// <summary>
        /// An instance of <see cref="IHostApplicationLifetime"/> used to manage application lifetime events within the WireGuardMonitoringService.
        /// </summary>
        private readonly IHostApplicationLifetime _lifetime;

        /// <summary>
        /// The IP address to ping in order to check the WireGuard connection status.
        /// </summary>
        private const string PingAddress = "**********";

        /// <summary>
        /// Represents a background service that monitors the WireGuard connection and attempts to reconnect if the connection is lost.
        /// </summary>
        public WireGuardMonitoringService(ILogger<WireGuardMonitoringService> logger, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
        }

        /// <summary>
        /// Executes the background monitoring process to check the WireGuard connection status and attempts to reconnect if the connection is lost.
        /// </summary>
        /// <param name="stoppingToken">A token that indicates when the execution should be stopped.</param>
        /// <return>A task that represents the background operation.</return>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int failedPingCount = 0;
            const int maxFailedPing = 5;
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking WireGuard connection...");

                bool isConnected = CheckWireGuardConnection();
                
                if (!isConnected)
                {
                    _logger.LogWarning($"WireGuard is not connected. Attempting (try number {failedPingCount} to reconnect...");
                    RestartWireGuard();
                    failedPingCount++;

                    if(failedPingCount >= maxFailedPing)
                    {
                        _logger.LogError("Max Count for failed attemps reached stopping service");
                        _lifetime.StopApplication();
                    }
                }
                else
                {
                    failedPingCount = 0;   
                }
                await Task.Delay(60000, stoppingToken); // Check every minute
            }
        }

        /// <summary>
        /// Checks the status of the WireGuard connection by pinging a predefined address.
        /// </summary>
        /// <returns>True if the ping is successful, indicating that the WireGuard connection is active; otherwise, false.</returns>
        private bool CheckWireGuardConnection()
        {
            try
            {
                using Ping ping = new Ping();
                PingReply reply = ping.Send(PingAddress, 5000);

                if (reply is { Status: IPStatus.Success })
                {
                    _logger.LogInformation("Ping to {PingAddress} successful.", PingAddress);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Ping to {PingAddress} failed. Status: {Status}", PingAddress, reply?.Status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while pinging {PingAddress}", PingAddress);
                return false;
            }
        }

        /// <summary>
        /// Restart the WireGuard interface by bringing it down and back up using the "wg-quick" command.
        /// </summary>
        private void RestartWireGuard()
        {
            try
            {
                ProcessStartInfo downStartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = "-c \"wg-quick down /etc/wireguard/wg.conf\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                ProcessStartInfo dnsUpdate = new()
                {
                    FileName = "bash",
                    Arguments = "-c \"getent hosts jh38l5g55r202trr.myfritz.net",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? downProcess = Process.Start(downStartInfo))
                {
                    downProcess?.WaitForExit();
                    _logger.LogInformation("WireGuard interface brought down.");
                }
                
                using (Process? dnsProcess = Process.Start(dnsUpdate))
                {
                    dnsProcess?.WaitForExit();
                    _logger.LogInformation("DNS updated.");
                }

                ProcessStartInfo upStartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = "-c \"wg-quick up /etc/wireguard/wg.conf\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process? upProcess = Process.Start(upStartInfo))
                {
                    upProcess?.WaitForExit();
                    _logger.LogInformation("WireGuard interface brought up.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while restarting WireGuard");
            }
        }

    }
}