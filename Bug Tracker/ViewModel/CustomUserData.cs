using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.ViewModel
{
    public class CustomUserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<string> ProjectNames { get; set; }
        public List<Ticket> Tickets { get; set; }
        public CustomUserData()
        {
            ProjectNames = new List<string>();
            Tickets = new List<Ticket>();
         
        }
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }
    }
}