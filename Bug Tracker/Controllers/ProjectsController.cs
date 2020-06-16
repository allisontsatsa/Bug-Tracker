using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using Bug_Tracker.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectsHelper = new ProjectsHelper();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();


        // GET: Projects
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult ManageProjectAssignments()
        {
            var emptyCustomUserDataList = new List<CustomUserData>();
            var users = db.Users.ToList();

            ViewBag.UserId = new SelectList(users, "Id", "FullName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            foreach (var user in users)
            {
                emptyCustomUserDataList.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    ProjectNames = projectsHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                });
            }

            return View(emptyCustomUserDataList);
        }
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult ListView()
        {
            List<Project> projects = db.Projects.ToList();
            ProjectListViewModel model = new ProjectListViewModel();

            foreach (var item in projects)
            {
                var vm = new ProjectAndPM();
                var pm = ProjectDisplayHelper.DisplayPMName(item.ProjectManagerId);
                vm.Project = item;
                vm.PMName = pm;

                model.Projects.Add(vm);

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ManageProjectAssignments(string userId, int projectId, bool addUser)
        {
            if (userId != null && projectId > 0)
            {
                if (addUser)
                {
                    projectsHelper.AddUserToProject(userId, projectId);
                }
                else
                {
                    projectsHelper.RemoveUserFromProject(userId, projectId);
                }
            }
            return RedirectToAction("ManageProjectAssignments");
        }



        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);

            ViewBag.PMName = ProjectDisplayHelper.DisplayPMName(project.ProjectManagerId);
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("PM", "DemoPM").ToList(), "Id", "FullName", project.ProjectManagerId);

            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }



        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("ManageProjectAssignments");
            }

            return View();
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = project.Id });
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Projects/ManageProjects/5
        
        public ActionResult ManageProjects()
        {
            RoleHelper roleHelper = new RoleHelper();

            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var model = new ProjectListViewModel();
            var user = db.Users.Find(userId);
            var myTickets = new List<Ticket>();
            var myProjects = new List<Project>();

            switch (myRole)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets = db.Tickets.ToList();
                    myProjects = db.Projects.ToList();
                    break;

                case "PM":
                case "DemoPm":
                    myProjects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();
                    myTickets = myProjects.SelectMany(p => p.Tickets).ToList();
                    break;

                case "Dev":
                case "DemoDev":
                    myProjects = user.Projects.ToList();
                    myTickets = myProjects.SelectMany(t => t.Tickets).Where(t => t.DeveloperId == userId).ToList();
                    break;

                case "Sub":
                case "DemoSub":
                    myProjects = user.Projects.ToList();
                    myTickets = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                    break;

                default:
                    return RedirectToAction("Register", "Account");
            }


            foreach (var item in myProjects)
            {
                var vm = new ProjectAndPM();
                var pm = ProjectDisplayHelper.DisplayPMName(item.ProjectManagerId);
                vm.Project = item;
                vm.PMName = pm;

                model.Projects.Add(vm);

            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "PM, Admin, DemoAdmin, DemoPm")]
        public ActionResult AssignPM(int id)
        {

            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("PM", "DemoPM").ToList(), "Id", "FullName");

            Project model = db.Projects.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPM([Bind(Include = "Id, ProjectManagerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                var prj = db.Projects.Find(project.Id);
                prj.ProjectManagerId = project.ProjectManagerId;
                db.SaveChanges();
                projectsHelper.AddUserToProject(project.ProjectManagerId, project.Id);
                //if (project.ProjectManagerId != null)
                //{
                //    db.SaveChanges();
                //    projectsHelper.RemoveUserFromProject(project.ProjectManagerId, project.Id);
                //}
            }
            //redirect
            return RedirectToAction("Details", new { id = project.Id });
        }

        [Authorize(Roles = "Admin, DemoAdmin, DemoPM, PM")]
        public ActionResult AssignDeveloper(int id)
        {

            var proj = db.Projects.Find(id);
            var viewData = new List<CustomUserData>();
            var users = roleHelper.UsersInRole("Dev", id).ToList();


            foreach (var user in users)
            {
                if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() != "Admin")
                {
                    viewData.Add(new CustomUserData
                    {

                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Tickets = ticketHelper.ListUserTickets(user.Id).ToList()
                    });
                }

            }
            ViewBag.TicketId = new SelectList(proj.Tickets.Where(t => t.DeveloperId == null) , "Id", "Title");
            ViewBag.UserId = new SelectList(users, "Id", "Email");

            return View(viewData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDeveloper(string userId, int ticketId)
        {
                Ticket oldTicket = new Ticket();
                oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t =>t.Id == ticketId);

            if (userId != null)
            {
                int projId = 0;

                var ticket = db.Tickets.Find(ticketId);
                projId = ticket.ProjectId;
                ticket.DeveloperId = userId;
                var status = db.TicketStatuses.FirstOrDefault(s => s.Name == "Assigned").Id;
                ticket.TicketStatusId = status;
                ticket.Updated = DateTime.Now;

                db.SaveChanges();
                notificationHelper.ManageNotifications(oldTicket, ticket);
                return RedirectToAction("Details", new { id = projId });
            }
            return RedirectToAction("AssignDeveloper");

        }
    }
}