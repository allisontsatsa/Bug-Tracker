using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class HomeController : Controller
        
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Get In Touch";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactAsync(EmailModel model)
        {
            try
            {
                var EmailAddress = WebConfigurationManager.AppSettings["EmailTo"];
                var emailFrom = $"{model.From} <{EmailAddress}>";
                var FinalBody = $"{model.Name} has sent you the following message: <br/> {model.Body} <hr/> {WebConfigurationManager.AppSettings["LegalText"]}";

                var email = new MailMessage(emailFrom, EmailAddress)
                {
                    Subject = model.Subject,
                    Body = FinalBody,
                    IsBodyHtml = true
                };
                var emailSvc = new EmailService();
                await emailSvc.SendAsync(email);

               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
            return View(new EmailModel());
        }
        public PartialViewResult _SideNav()
        {
            var model = db.Users.Find(User.Identity.GetUserId());
            return PartialView(model);
        }
    }

}