﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string TicketTypeId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime Changed { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
        public string Property { get; set; }
   


    }

}