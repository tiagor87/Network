using System;
using System.Net;
using FluentAssertions;
using Xunit;

namespace Network.Core.UnitTests
{
    public class NetworkExtensionsTests
    {
        [Theory]
        [InlineData("192.168.0.0")]
        [InlineData("192.168.100.0")]
        [InlineData("192.168.254.0")]
        [InlineData("10.0.0.0")]
        [InlineData("10.100.0.0")]
        [InlineData("10.0.100.0")]
        [InlineData("172.16.0.0")]
        [InlineData("172.17.0.0")]
        [InlineData("172.18.0.0")]
        [InlineData("172.19.0.0")]
        [InlineData("172.20.0.0")]
        [InlineData("172.21.0.0")]
        [InlineData("172.22.0.0")]
        [InlineData("172.23.0.0")]
        [InlineData("172.24.0.0")]
        [InlineData("172.25.0.0")]
        [InlineData("172.26.0.0")]
        [InlineData("172.27.0.0")]
        [InlineData("172.28.0.0")]
        [InlineData("172.29.0.0")]
        [InlineData("172.30.0.0")]
        [InlineData("172.31.0.0")]
        [InlineData("127.0.0.0")]
        [InlineData("127.0.0.254")]
        [InlineData("127.0.100.0")]
        [InlineData("127.0.100.254")]
        [InlineData("127.100.0.0")]
        [InlineData("127.100.0.254")]
        public void GivenAddressWhenIsPrivateShouldReturnTrue(string ip)
        {
            IPAddress.Parse(ip).IsPrivate().Should().BeTrue();
        }

        [Theory]
        [InlineData("11.0.0.0")]
        [InlineData("192.169.0.0")]
        [InlineData("172.15.0.0")]
        [InlineData("172.32.0.0")]
        public void GivenAddressWhenIsPublicShouldReturnFalse(string ip)
        {
            IPAddress.Parse(ip).IsPrivate().Should().BeFalse();
        }

        [Theory]
        [InlineData("http://localhost")]
        [InlineData("https://localhost")]
        [InlineData("amqp://localhost")]
        [InlineData("mongodb://localhost")]
        [InlineData("http://127.0.0.1")]
        [InlineData("https://127.0.0.1")]
        [InlineData("amqp://127.0.0.1")]
        [InlineData("mongodb://127.0.0.1")]
        [InlineData("http://192.168.0.0")]
        [InlineData("https://192.168.0.0")]
        [InlineData("amqp://192.168.0.0")]
        [InlineData("mongodb://192.168.0.0")]
        [InlineData("http://10.0.0.0")]
        [InlineData("https://10.0.0.0")]
        [InlineData("amqp://10.0.0.0")]
        [InlineData("mongodb://10.0.0.0")]
        [InlineData("http://172.16.0.0")]
        [InlineData("https://172.16.0.0")]
        [InlineData("amqp://172.16.0.0")]
        [InlineData("mongodb://172.16.0.0")]
        public void GivenUriWhenIsPrivateShouldReturnTrue(string address)
        {
            new Uri(address).IsPrivate().Should().BeTrue();
        }

        [Theory]
        [InlineData("http://localhost2")]
        [InlineData("https://localhost2")]
        [InlineData("amqp://localhost2")]
        [InlineData("mongodb://localhost2")]
        [InlineData("http://128.0.0.2")]
        [InlineData("https://128.0.0.2")]
        [InlineData("amqp://128.0.0.2")]
        [InlineData("mongodb://128.0.0.2")]
        public void GivenUriWhenIsPublicShouldReturnFalse(string address)
        {
            new Uri(address).IsPrivate().Should().BeFalse();
        }

        [Theory]
        [InlineData("http://eurealmentenaoexisto.br")]
        [InlineData("http://ididnsexists.com")]
        public void GivenUriWhenIsNotFoundShouldReturnFalse(string address)
        {
            new Uri(address).IsPrivate().Should().BeFalse();
        }
    }
}