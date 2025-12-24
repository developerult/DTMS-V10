using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompDockBlockOutPeriod class for dock block out periods
	/// </summary>
	public class AMSCompDockBlockOutPeriod
	{
        /// <summary>
        /// Property DockBlockControl as INT
        /// </summary>
        public int DockBlockControl { get; set; }

        /// <summary>
        /// Property DockControl as INT
        /// </summary>
        public int DockControl { get; set; }
        public bool DockBlockExpired { get; set; }
        public bool DockBlockOn { get; set; }


        /// <summary>
        /// Property RecurrenceType as INT
        /// </summary>
        public int RecurrenceType { get; set; }

        /// <summary>
        /// Property Title as string
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Property Description as string
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Property StartTime as DateTime
        /// </summary>
        public DateTime StartTime { get; set; }
		private string _StartTime;

        /// <summary>
        /// Property EndTime as DateTime
        /// </summary>
        public DateTime EndTime { get; set; }
		private string _EndTime;

        /// <summary>
        /// Property StartDate as DateTime
        /// </summary>
        public DateTime StartDate { get; set; }
		private string _StartDate;

        /// <summary>
        /// Property Until as DateTime
        /// </summary>
        public DateTime? Until { get; set; }
		private string _Until;

        /// <summary>
        /// Property Count as INT
        /// </summary>
        public int Count { get; set; }

        public bool blnSun { get; set; }
        public bool blnMon { get; set; }
        public bool blnTue { get; set; }
        public bool blnWed { get; set; }
        public bool blnThu { get; set; }
        public bool blnFri { get; set; }
        public bool blnSat { get; set; }

	}
}