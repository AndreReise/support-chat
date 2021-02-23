using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Applications = new HashSet<Application>();
            Details = new HashSet<Detail>();
        }

        public int ChatId { get; set; }
        public Guid? Guidemployee { get; set; }
        public Guid? Guiduser { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}
