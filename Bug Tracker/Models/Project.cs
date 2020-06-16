using Microsoft.Owin.Security.DataHandler.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Project
    {
        public Project()
        {
            Users = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //[Display(Name = "Project Manger")]
        public string ProjectManagerId {get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        //[Display(Name = "Archived")]
        public bool IsArchived { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}