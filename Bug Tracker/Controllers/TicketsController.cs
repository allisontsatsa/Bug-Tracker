using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.DynamicData;
using System.Web.Mvc;
using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using Bug_Tracker.ViewModel;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            TicketHistoryViewModel model = new TicketHistoryViewModel();

            var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Sub, DemoSub")]
        public ActionResult Create(int? projectId)
        {
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");
            //Need a list of only my projects and feed that to select list
            var myUserId = User.Identity.GetUserId();
            var myProjects = projectHelper.ListUserProjects(myUserId);
            var newTicket = new Ticket();

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");


            if (projectId != null)
                newTicket.ProjectId = (int)projectId;
            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,FileUrl,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New").Id;
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);

            var currentUserId = User.Identity.GetUserId();

            var authorized = true;

            if ((User.IsInRole("Dev") && ticket.DeveloperId != currentUserId)
                || (User.IsInRole("Sub") && ticket.SubmitterId != currentUserId)
                || (User.IsInRole("DemoDev") && ticket.DeveloperId != currentUserId)
                || (User.IsInRole("DemoSub") && ticket.SubmitterId != currentUserId))
            {
                authorized = false;
            }

            if (!authorized)
            {
                TempData["UnauthorizedAccess"] = $"You do not have permission to access this feature.";
                return RedirectToAction("Dashboard", "Home");
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName", ticket.DeveloperId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,SubmitterId,DeveloperId,Title,Description,Created,Updated,IsArchived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                ticket.Updated = DateTime.Now;
                var status = db.TicketStatuses.FirstOrDefault(s => s.Name == "Assigned").Id;
                ticket.TicketStatusId = status;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
                notificationHelper.ManageNotifications(oldTicket, newTicket);

                return RedirectToAction("ManageTickets", "Tickets");

            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            return View(ticket);
        }


        public ActionResult ManageTickets()
        {

            RoleHelper roleHelper = new RoleHelper();
            //var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter);
            var userId = User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            //var model = new List<Ticket>();
            var user = db.Users.Find(userId);
            var myTickets = new List<Ticket>();
            var myProjects = projectHelper.ListUserProjects(userId);

            switch (myRole)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets = db.Tickets.ToList();
                    myProjects = db.Projects.ToList();
                    break;
                case "PM":
                case "DemoPM":
                    myProjects = db.Projects.Where(p => p.ProjectManagerId == userId).ToList();
                    //myTickets.AddRange(db.Projects.Where(p => p.IsArchived == false).Where(p => p.ProjectManagerId == userId).SelectMany(p => p.Tickets));
                    myTickets = myProjects.Where(p => p.IsArchived == false).SelectMany(p => p.Tickets).ToList();
                    break;
                case "Dev":
                case "DemoDev":
                    myProjects = user.Projects.ToList();
                    myTickets = db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                    break;
                case "Sub":
                case "DemoSub":
                    myProjects = user.Projects.ToList();
                    myTickets = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                    break;
                default:
                    myTickets = db.Tickets.ToList();
                    break;
                    //return RedirectToAction("Dashboard", "Home");
            }
            return View(myTickets);
        }

        //[Authorize(Roles = "Admin, DemoAdmin, DemoPM, PM")]
        //public ActionResult AssignDeveloper()
        //{

        //    var userId = User.Identity.GetUserId();
        //    var viewData = new List<CustomUserData>();
        //    var users = roleHelper.UsersInRole("Dev").ToList();
        //    var myTickets = new List<Ticket>();

        //    foreach (var user in users)
        //    {
        //        if (roleHelper.ListUserRoles(user.Id).FirstOrDefault() != "Admin")
        //        {
        //            viewData.Add(new CustomUserData
        //            {

        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                Email = user.Email,
        //                Tickets = ticketHelper.ListUserTickets(user.Id).ToList()
        //            });
        //        }

        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
        //    ViewBag.UserId = new SelectList(roleHelper.UsersInRole("Dev").ToList(), "Id", "Email");

        //    return View(viewData);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignDeveloper(string userId, int ticketId)
        //{

        //    if (userId != null)
        //    {
        //        Ticket oldTicket = new Ticket();
        //        var ticket = oldTicket = db.Tickets.Find(ticketId);
        //        ticket.DeveloperId = userId;

        //        db.SaveChanges();
        //        notificationHelper.ManageNotifications(oldTicket, ticket);
        //    }

        //    return RedirectToAction("AssignDeveloper");
        //}

        public ActionResult Dashboard(int id)
        {
            return View(db.Tickets.Find(id));
        }

    }
}
