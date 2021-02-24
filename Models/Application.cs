using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Application
    {
        public int ApplicationId { get; set; }
        public string Topic { get; set; }
        public int Chat { get; set; }
        public int Status { get; set; }

     
        public virtual Status StatusNavigation { get; set; }
    }
}
