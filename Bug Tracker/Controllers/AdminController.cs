﻿using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using Bug_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    [Authorize(Roles = "Admin, DemoAdmin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        public ActionResult ManageRoles()
        {
            var viewData = new List<CustomUserData>();
            var users = db.Users.ToList();
            foreach(var user in users)
            {
                viewData.Add(new CustomUserData {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "Unassigned"
                });
            }
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            return View(viewData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

                    if (userRole != null)
                    {
                        roleHelper.RemoveUserFromRole(userId, userRole); 
                    }
                    roleHelper.AddUserToRole(userId, roleName);
                }
            }
            return RedirectToAction("ManageRoles");
        }

    }
}