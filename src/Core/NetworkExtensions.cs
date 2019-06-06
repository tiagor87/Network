using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Network.Core
{
    public static class NetworkExtensions
    {
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
            return ipAddress.HasPrivateNeworkSpecification() ||
                   GetPrivateNetworkIPAddresses().Contains(ipAddress);
        }

        public static bool IsPrivate(this Uri uri)
        {
            try
            {
                return Dns.GetHostAddresses(uri.Host).Any(ipAddress => ipAddress.IsPrivate());
            }
            catch
            {
                return false;
            }
        }
    }
}