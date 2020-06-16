using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.ViewModel
{
    public class ChartVM
    {
        public List<ChartDataSet> datasets { get; set; }
        public List<string> labels { get; set; }
        public ChartVM()
        {
            datasets = new List<ChartDataSet>();
            labels = new List<string>();
        }
    }
}
public class ChartDataSet
{
    public List<int> data { get; set; }
    public List<string> backgroundColor { get; set; }
    public ChartDataSet()
    {
        data = new List<int>();
        backgroundColor = new List<string>();
    }
}
