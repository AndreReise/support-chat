using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class RequestType
    {
        public RequestType()
        {
            Details = new HashSet<Detail>();
        }

        public int RequestTypeId { get; set; }
        public string RequestType1 { get; set; }

        public virtual ICollection<Detail> Details { get; set; }
    }
}
