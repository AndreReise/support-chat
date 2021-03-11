using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models.ServiceModels
{
    public class Log
    {
        public int LogId { get; set; }
        public DateTime Time { get; set; }
        public string LogMessage { get; set; }
    }
}
