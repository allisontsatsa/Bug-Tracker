using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
      
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectManagerId { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsArchived { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public HashSet<Ticket> Tickets { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
      
    }
}
