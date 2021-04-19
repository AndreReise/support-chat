using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils.Cache
{
    public static class CacheEntityType
    {
        /// <summary>
        /// Client-list cache dictionary name
        /// </summary>
        public const string Client = "CACHE_CLIENT_LIST";
        /// <summary>
        /// Employee-list cache dictionary name
        /// </summary>
        public const string Employee = "EMPLOYEE_CLIENT_LIST";
        /// <summary>
        /// Log-list cache dictionary name
        /// </summary>
        public const string Logs = "ERROR_LOGS";
    }
}
