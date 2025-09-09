using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace EasyID.SDK.Extensions
{
    /// <summary>
    /// Extensions for the IConfiguration interface to add secrets from the EasyID API.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static IServiceProvider AddEasyAuth(this IServiceProvider _services)
        {
            return _services;
        }
    }
}
