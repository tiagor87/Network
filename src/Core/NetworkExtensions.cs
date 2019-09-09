using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Network.Core
{
    public static class NetworkExtensions
    {
        private static ConcurrentDictionary<string, bool> _lastUriResults = new ConcurrentDictionary<string, bool>();
        private static ConcurrentDictionary<IPAddress, bool> _lastIPAddressResults = new ConcurrentDictionary<IPAddress, bool>();
        
        public static IEnumerable<IPAddress> GetPrivateNetworkIPAddresses()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(x => x.GetIPProperties().UnicastAddresses)
                .Where(x => !string.IsNullOrWhiteSpace(x.Address.ToString()))
                .Select(x => x.Address).ToList();
        }

        public static bool HasPrivateNeworkSpecification(this IPAddress ipAddress)
        {
            var bytes = ipAddress.GetAddressBytes();
            switch (bytes[0])
            {
                case 127:
                    return true;
                case 10:
                    return true;
                case 172:
                    return bytes[1] >= 16 && bytes[1] <= 31;
                case 192:
                    return bytes[1] == 168;
                default:
                    return false;
            }
        }

        public static bool IsPrivate(this IPAddress ipAddress)
        {
            if (_lastIPAddressResults.TryGetValue(ipAddress, out var result))
            {
                return result;
            }
            result = ipAddress.HasPrivateNeworkSpecification() ||
                   GetPrivateNetworkIPAddresses().Contains(ipAddress);
            _lastIPAddressResults.AddOrUpdate(ipAddress, result, (key, value) => result);
            return result;
        }

        public static bool IsPrivate(this Uri uri)
        {
            return IsPrivateAsync(uri).GetAwaiter().GetResult();
        }

        public static async Task<bool> IsPrivateAsync(this Uri uri)
        {
            if (_lastUriResults.TryGetValue(uri.Host, out var isPrivate))
            {
                return isPrivate;
            }
            try
            {
                var hosts = await Dns.GetHostAddressesAsync(uri.Host);
                var result = hosts.Any(ipAddress => ipAddress.IsPrivate());
                _lastUriResults.AddOrUpdate(uri.Host, result, (key, value) => result);
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}