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
        /// <summary>
        /// Integrates EasyID authentication services into the service collection and ASP.NET Core pipeline as well.
        /// </summary>
        /// <param name="_services"></param>
        /// <returns></returns>
        public static IServiceProvider AddEasyAuth(this IServiceProvider _services)
        {
            return _services;
        }
    }
}
