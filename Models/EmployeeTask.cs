using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class EmployeeTask
    {
        public int TaskId { get; set; }
        public int TaskCount { get; set; }
        public Guid Guidemployy { get; set; }

        public virtual Employee GuidemployyNavigation { get; set; }
    }
}
