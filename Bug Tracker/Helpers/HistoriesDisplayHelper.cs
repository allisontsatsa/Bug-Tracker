using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Microsoft.Ajax.Utilities;

namespace Bug_Tracker.Helpers
{
    public class HistoriesDisplayHelper
    {
        public static string DisplayData(TicketHistory ticketHistory)
        {
            var db = new ApplicationDbContext();
            var data = "";

            switch(ticketHistory.Property)
            {
                case "Title":
                    data = ticketHistory.NewValue;
                    break;
                case "Developer":
                     data = db.Users.FirstOrDefault(u => u.Id == ticketHistory.NewValue).FullName;
                    break;
                case "Description":
                    data = ticketHistory.NewValue;
                    break;
                case "Ticket Type":
                    data = ticketHistory.NewValue;
                    break;
                case "Ticket Status":
                    data = ticketHistory.NewValue;
                    break;
                case "Ticket Priority":
                    data = ticketHistory.NewValue;
                    break;
                default:
                    data = ticketHistory.NewValue;
                    break;
            }

            return data;
        }

        public static string DisplayUser(TicketHistory ticketHistory)
        {
            var db = new ApplicationDbContext();
            var data = "";
            data = db.Users.FirstOrDefault(u => u.Id == ticketHistory.UserId).FullName;

            return data;
        }
    }
}