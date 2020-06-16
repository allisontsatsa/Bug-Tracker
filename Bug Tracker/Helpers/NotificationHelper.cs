using Bug_Tracker.Models;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            GenerateTicketAssignmentNotifications(oldTicket, newTicket);

            GenerateTicketChangeNotifications(oldTicket, newTicket);
        }
        private void GenerateTicketAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId !=newTicket.DeveloperId; 

            if(assigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "You have a new ticket!",
                    Created = created,
                    NotificationBody = $"You have been assigned to ticket Id: {newTicket.Id} on {created.ToString("MMM dd, yyyy")}"
                });
            }
            db.SaveChanges();
        }
        private void GenerateTicketChangeNotifications(Ticket oldTicket, Ticket newTicket)
        {

            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (reassigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "You have been assigned a ticket!",
                    Created = created,
                    NotificationBody = $"You have been assigned to ticket Id: {newTicket.Id} on {created.ToString("MMM dd, yyyy")}"
                });
            }
            db.SaveChanges();
        }
    }
}