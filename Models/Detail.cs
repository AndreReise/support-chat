using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Detail
    {
        public int DetailsId { get; set; }
        public DateTime CreatingDate { get; set; }
        public string Data { get; set; }
        public int? Chat { get; set; }
        public int? RequestType { get; set; }
        public int? CommunicationType { get; set; }
        public Guid Guidinteracting { get; set; }

        public virtual Chat ChatNavigation { get; set; }
        public virtual CommunicationType CommunicationTypeNavigation { get; set; }
        public virtual RequestType RequestTypeNavigation { get; set; }
    }
}
