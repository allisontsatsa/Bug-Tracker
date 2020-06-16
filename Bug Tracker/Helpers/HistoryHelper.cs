using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Bug_Tracker.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageHistoryRecordCreation(Ticket oldTicket, Ticket newTicket)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var time = (DateTime)newTicket.Updated;


            if (oldTicket.Title != newTicket.Title)
            {
                var newHistoryRecord = new TicketHistory();
              
                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Title";
                newHistoryRecord.OldValue = oldTicket.Title;
                newHistoryRecord.NewValue = newTicket.Title;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Developer";
                newHistoryRecord.OldValue = oldTicket.DeveloperId;
                newHistoryRecord.NewValue = newTicket.DeveloperId;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Description";
                newHistoryRecord.OldValue = oldTicket.Description;
                newHistoryRecord.NewValue = newTicket.Description;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Ticket Type";
                newHistoryRecord.OldValue = oldTicket.TicketType.Name;
                newHistoryRecord.NewValue = newTicket.TicketType.Name;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Ticket Status";
                newHistoryRecord.OldValue = oldTicket.TicketStatus.Name;
                newHistoryRecord.NewValue = newTicket.TicketStatus.Name;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
                if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.Changed = time;
                newHistoryRecord.UserId = userId;
                newHistoryRecord.Property = "Ticket Priority";
                newHistoryRecord.OldValue = oldTicket.TicketPriority.Name;
                newHistoryRecord.NewValue = newTicket.TicketPriority.Name;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            db.SaveChanges();
        }

    }
}