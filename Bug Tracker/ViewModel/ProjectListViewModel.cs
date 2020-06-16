using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.ViewModel
{
    public class ProjectListViewModel
    {
        public List<ProjectAndPM> Projects { get; set; }

        public ProjectListViewModel()
        {
            Projects = new List<ProjectAndPM>();
        }
    }

    public class ProjectAndPM
    {
        public Project Project { get; set; }
        public string PMName { get; set; }
    }
}

