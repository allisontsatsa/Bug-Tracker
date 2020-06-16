using Bug_Tracker.Helpers;
using Bug_Tracker.Models;
using Bug_Tracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
   

    public class GetTicketPriorityChartController : Controller
       
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        // GET: GetTicketPriorityChart
        public JsonResult GetTicketPriorityChart()

        {
            var colorList = new List<string>();

            colorList.Add("#D3D3D3");
            colorList.Add("#ffa1a1");
            colorList.Add("#232323");
            colorList.Add("#FFFF00");

            var rand = new Random();
            var chartVM = new ChartVM();
            var chartDataSet = new ChartDataSet();
            var priorities = db.TicketPriorities.ToList();
            var dataKey = 0;
            //barChartVM.Datasets.Insert(dataKey, "Priorities");
            foreach (var priority in priorities)
            {
                var count = db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count();

                //barChartVM.Datasets.Add(new KeyValuePair<int, string>(count, colorList[dataKey]));

                chartDataSet.data.Add(count);
                chartDataSet.backgroundColor.Add(colorList[dataKey]);
                chartVM.labels.Add(priority.Name);
                dataKey++;
            }
            chartVM.datasets.Add(chartDataSet);

            return Json(chartVM, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTicketStatusChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#D3D3D3");
            colorList.Add("#ffa1a1");
            colorList.Add("#F2f3f7");
            colorList.Add("#6c757d");
            colorList.Add("#232323");
            var rand = new Random();
            var barChartVM = new ChartVM();
            var chartDataSet = new ChartDataSet();
            var statuses = db.TicketStatuses.ToList();
            var dataKey = 0;
            foreach (var status in statuses)
            {
                var count = db.Tickets.Where(t => t.TicketStatusId == status.Id).Count();
                //barChartVM.Datasets.Add(new KeyValuePair<int, string>(count, colorList[dataKey]));
                chartDataSet.data.Add(count);
                chartDataSet.backgroundColor.Add(colorList[dataKey]);
                barChartVM.labels.Add(status.Name);
                dataKey++;
            }
            barChartVM.datasets.Add(chartDataSet);
            return Json(barChartVM, JsonRequestBehavior.AllowGet);
        }
    }
}