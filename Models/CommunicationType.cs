using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class CommunicationType
    {
        public CommunicationType()
        {
            Details = new HashSet<Detail>();
        }

        public int CommunicationTypeId { get; set; }
        public string CommunicationType1 { get; set; }

        public virtual ICollection<Detail> Details { get; set; }
    }
}
