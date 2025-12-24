using System;

namespace DynamicsTMS365.Models
{
    public class CompDashboard
    {
        public string CompName { get; set; }
        public int TotalOrders { get; set; }
        public double TotalWgt { get; set; }
        public int TotalOnTime{ get; set; }
        public int TotalLate { get; set; }
        public double OTLPerc { get; set; }
    }

    public class CarrierDashboard
    {       
        public int iMonth { get; set; }
        public int iYear { get; set; }
        public int Loads { get; set; }
        public string Month { get; set; }
       

    }
    public class CarrierDashboardGroup
    {
        public int iMonth { get; set; }
        public int iYear { get; set; }
      


    }

    public class PlanningDashboard
    {
        public DateTime LoadDate { get; set; }
        public int Loads { get; set; }
      
    }
}