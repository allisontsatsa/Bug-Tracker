using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Helpers
{
    public class ProjectDisplayHelper
    {
        public static string DisplayPMName(string ProjectManagerId)
        {
            var db = new ApplicationDbContext();
            var data = "";
            if (!string.IsNullOrEmpty(ProjectManagerId))
            {
                data = db.Users.FirstOrDefault(u => u.Id == ProjectManagerId).FullName;
            }
            return data;
        }

    }
}