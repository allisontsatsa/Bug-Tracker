using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        //Admin or PM (User who caused generation of notification
        public string SenderId { get; set; }
        //Developer notification is sent to
        public string RecipientId { get; set; }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
        public string NotificationBody { get; set; }
        public DateTime Created { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

    }
}