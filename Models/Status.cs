using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Status
    {
        public Status()
        {
            Applications = new HashSet<Application>();
        }

        public int StatusId { get; set; }
        public string Status1 { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
