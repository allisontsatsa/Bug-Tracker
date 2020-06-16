namespace Bug_Tracker.Migrations
{
    using Bug_Tracker.Helpers;
    using Bug_Tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "NewUser"))
            {
                roleManager.Create(new IdentityRole { Name = "NewUser" });
            }
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "PM"))
            {
                roleManager.Create(new IdentityRole { Name = "PM" });
            }
            if (!context.Roles.Any(r => r.Name == "Dev"))
            {
                roleManager.Create(new IdentityRole { Name = "Dev" });
            }
            if (!context.Roles.Any(r => r.Name == "Sub"))
            {
                roleManager.Create(new IdentityRole { Name = "Sub" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoPM"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoPM" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoDev"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoDev" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoSub"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoSub" });
            }
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];

            if (!context.Users.Any(u => u.Email == "allison_tsatsa@hotmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "allison_tsatsa@hotmail.com",
                    Email = "allison_tsatsa@hotmail.com",
                    FirstName = "Allison",
                    LastName = "Tsatsa",
                    DisplayName = "Admin",
                    AvatarPath = "/Avatars/process-thinking-svg-png-icon-download-6.png"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "Admin");

            }

            if (!context.Users.Any(u => u.Email == "demoadmin@mailinator.com"))
               
            {
                var user = new ApplicationUser
                {
                    UserName = "demoadmin@mailinator.com",
                    Email = "demoadmin@mailinator.com",
                    FirstName = "Ricki",
                    LastName = "Lake",
                    DisplayName = "DemoAdmin",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id, "DemoAdmin");

            }

            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "arussell@coderfoundry.com",
                    Email = "arussell@coderfoundry.com",
                    FirstName = "Drew",
                    LastName = "Russell",
                    DisplayName = "Walking Glow Stick",
                    AvatarPath = "/Avatars/avatar_boy2.png"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "PM");
            }

            if (!context.Users.Any(u => u.Email == "demopm@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demopm@mailinator.com",
                    Email = "demopm@mailinator.com",
                    FirstName = "Mark",
                    LastName = "Wahlburg",
                    DisplayName = "DemoPM",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id, "DemoPM");
            }
            if (!context.Users.Any(u => u.Email == "ipfreely@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "ipfreely@gmail.com",
                    Email = "ipfreely@gmail.com",
                    FirstName = "IP",
                    LastName = "Freely",
                    DisplayName = "IP Freely"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "Dev");
            }

            if (!context.Users.Any(u => u.Email == "demodev@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demodev@mailinator.com",
                    Email = "demodev@mailinator.com",
                    FirstName = "John",
                    LastName = "Smith",
                    DisplayName = "Johnnie",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id, "Dev");
            }

            if (!context.Users.Any(u => u.Email == "ChesterCopperpot@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "ChesterCopperpot@gmail.com",
                    Email = "ChesterCopperpot@gmail.com",
                    FirstName = "Chester",
                    LastName = "Copperpot",
                    DisplayName = "Chester Copperpot"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "Dev");
            }
            if (!context.Users.Any(u => u.Email == "Ty@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "Ty@gmail.com",
                    Email = "Ty@gmail.com",
                    FirstName = "TyMay",
                    LastName = "Iavanother",
                    DisplayName = "TyMay"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "Sub");
            }
            if (!context.Users.Any(u => u.Email == "Demo@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "Demo@gmail.com",
                    Email = "Demo@gmail.com",
                    FirstName = "Demo",
                    LastName = "Sub",
                    DisplayName = "DemoSub"
                };

                userManager.Create(user, "ABC&456");

                userManager.AddToRoles(user.Id, "DemoSub");
            }

            if (!context.Users.Any(u => u.Email == "demosub@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demosub@mailinator.com",
                    Email = "demosub@mailinator.com",
                    FirstName = "David",
                    LastName = "Tenant",
                    DisplayName = "DemoSub",
                    AvatarPath = "/Avatars/download.jpg"
                };

                userManager.Create(user, demoPassword);

                userManager.AddToRoles(user.Id, "DemoSub");
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region Load Up Ticket Type

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType {Name = "Defect", Description = "Classic Bug" },
             new TicketType {Name = "New Functionality", Description = "Would like additional application functionality" },
             new TicketType { Name = "Other", Description = "Issue of another kind" }
            ); 

            #endregion

            #region Load Up Ticket Priority

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
             new TicketPriority { Name = "Low", Description = "Can Wait" },
                 new TicketPriority { Name = "Medium", Description = "Get to Soon" },
             new TicketPriority { Name = "High", Description = "Immediately, if not sooner." }
            ); ;

            #endregion

            #region Load Up Ticket Status

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New", Description = "Newly created" },
             new TicketStatus { Name = "Assigned", Description = "Assigned  to a Developer" },
                  new TicketStatus { Name = "Unassigned", Description = "Not yet assigned  to a Developer" },
                       new TicketStatus { Name = "Reassigned", Description = "Assigned  to a new Developer" },
            new TicketStatus { Name = "Closed", Description = "Ticket has been resolved" }); 

            #endregion
            #region Seed a Demo Project

            context.Projects.AddOrUpdate(
                t => t.Name,
                    new Project { 
                        Name = "Demo Project",
                        Created = DateTime.Now
                });
            
            #endregion

            #region Seed a Demo Ticket
            var demoProject = context.Projects.FirstOrDefault(p => p.Name == "Demo Project").Id;
            var demoTicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id;
            var demoTicketPriorityId = context.TicketPriorities.FirstOrDefault(pr => pr.Name == "High").Id;
            var demoTicketStatusId = context.TicketStatuses.FirstOrDefault(pr => pr.Name == "New").Id;
            var demoSubmitterId = context.Users.FirstOrDefault(u => u.Email == "Demo@gmail.com").Id;
            var projectsHelper = new ProjectsHelper();
            projectsHelper.AddUserToProject(demoSubmitterId, demoProject);

            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket { 
                    Title = "Demo Ticket",
                    Created = DateTime.Now,
                    ProjectId = demoProject,
                    TicketTypeId = demoTicketTypeId,
                    TicketPriorityId = demoTicketPriorityId,
                    TicketStatusId = demoTicketStatusId,
                    SubmitterId = demoSubmitterId


                });
            context.SaveChanges();

            #endregion

            #region PROJECTS & TICKETS
            var rand = new Random();
            var rolesHelper = new RoleHelper();
            var projHelper = new ProjectsHelper();

            var submitters = rolesHelper.UsersInRole("Sub").ToList();
            submitters.AddRange(rolesHelper.UsersInRole("Admin").ToList());
            var seedTicketTypeId = context.TicketTypes.Select(t => t.Id).ToList();
            var seedTicketPriorityId = context.TicketPriorities.Select(t => t.Id).ToList();
            var seedTicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New").Id;

            for (int i = 1; i <= 10; i++)
            {
                var seededProjectName = $"Project {21 - i} (Seeded)";

                ApplicationDbContext db = new ApplicationDbContext();
                db.Projects.AddOrUpdate(t => t.Name, new Project
                {
                    Name = seededProjectName,
                    Description = "This is a demo Project that has been seeded.",
                    Created = DateTime.Now.AddDays(-i * 3)
                });
                db.SaveChanges();

                var seededProject = db.Projects.FirstOrDefault(p => p.Name == seededProjectName);
                var seedSub = submitters[rand.Next(0, submitters.Count)];

                seededProject.Users.Add(seedSub);

                for (int j = 1; j <= 5; j++)
                {
                    ApplicationDbContext dbcontext = new ApplicationDbContext();
                    dbcontext.Tickets.AddOrUpdate(t => t.Title, new Ticket
                    {
                        Title = $"Ticket {i}.{j}",
                        ProjectId = seededProject.Id,
                        Description = "This is a seeded demo Ticket.",
                        TicketTypeId = seedTicketTypeId[rand.Next(0, seedTicketTypeId.Count)],
                        TicketPriorityId = seedTicketPriorityId[rand.Next(0, seedTicketPriorityId.Count)],
                        TicketStatusId = seedTicketStatusId,
                        SubmitterId = seedSub.Id,
                        Created = DateTime.Now
                    });
                    dbcontext.SaveChanges();
                }
            }
            #endregion
        }
    }
}


