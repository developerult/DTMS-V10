using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompDockApptTimeSetting class for Dockdoor appt time Settings
	/// </summary>
	public class AMSCompDockApptTimeSetting
	{

		/// <summary>		
		/// PropertyDockControl/ID as INT	 
		/// </summary>
		public int DockControl { get; set; }

		/// <summary>		
		/// DockSettingCompDockContol/ID as INT	 
		/// </summary>
		public int DockSettingCompDockContol { get; set; }

		/// <summary>												
		/// PropertyMonStart as DateTime		 
		/// </summary>	
		public DateTime? MonStart { get; set; }
		private string _MonStart;

		/// <summary>												
		/// PropertyMonEnd as DateTime		 
		/// </summary>
		public DateTime? MonEnd  { get; set; }
		private string _MonEnd ;

		/// <summary>		
		/// PropertyMonMaxAppt as INT	 
		/// </summary>
		public int MonMaxAppt { get; set; }

		/// <summary>												
		/// PropertyTueStart as DateTime		 
		/// </summary>
		public DateTime? TueStart { get; set; }
		private string _TueStart;

		/// <summary>												
		/// PropertyTueEnd as DateTime		 
		/// </summary>
		public DateTime? TueEnd { get; set; }
		private string _TueEnd;

		/// <summary>		
		/// PropertyTueMaxAppt as INT	 
		/// </summary>
		public int TueMaxAppt { get; set; }

		/// <summary>												
		/// PropertyWedStart as DateTime		 
		/// </summary>
		public DateTime? WedStart  { get; set; }
		private string _WedStart;

		/// <summary>												
		/// PropertyWedEnd as DateTime		 
		/// </summary>
		public DateTime WedEnd { get; set; }
		private string _WedEnd;

		/// <summary>		
		/// PropertyWedMaxAppt as INT	 
		/// </summary>
		public int WedMaxAppt { get; set; }

		/// <summary>												
		/// PropertyThuStart as DateTime		 
		/// </summary>
		public DateTime? ThuStart { get; set; }
		private string _ThuStart;

		/// <summary>												
		/// PropertyThuEnd as DateTime		 
		/// </summary>
		public DateTime ThuEnd { get; set; }
		private string _ThuEnd;

		/// <summary>		
		/// PropertyThuMaxAppt as INT	 
		/// </summary>
		public int ThuMaxAppt { get; set; }

		/// <summary>												
		/// PropertyFriStart as DateTime		 
		/// </summary>
		public DateTime? FriStart { get; set; }
		private string _FriStart;

		/// <summary>												
		/// PropertyFridEnd as DateTime		 
		/// </summary>
		public DateTime? FridEnd { get; set; }
		private string _FridEnd;

		/// <summary>		
		/// PropertyFriMaxAppt as INT	 
		/// </summary>
		public int FriMaxAppt { get; set; }

		/// <summary>												
		/// PropertySatStart as DateTime		 
		/// </summary>
		public DateTime? SatStart { get; set; }
		private string _SatStart;

		/// <summary>												
		/// PropertySatEnd as DateTime		 
		/// </summary>
		public DateTime? SatEnd { get; set; }
		private string _SatEnd;

		/// <summary>		
		/// PropertySatMaxAppt as INT	 
		/// </summary>
		public int SatMaxAppt { get; set; }

		/// <summary>												
		/// PropertySunStart as DateTime		 
		/// </summary>
		public DateTime? SunStart { get; set; }
		private string _SunStart;

		/// <summary>												
		/// PropertySunEnd as DateTime		 
		/// </summary>
		public DateTime? SunEnd { get; set; }
		private string _SunEnd;

		/// <summary>		
		/// PropertySunMaxAppt as INT	 
		/// </summary>
		public int SunMaxAppt { get; set; }

		/// <summary>		
		/// PropertyApptMinsMin as INT	 
		/// </summary>
		public int ApptMinsMin { get; set; }

		/// <summary>		
		/// PropertyApptMinsAvg as INT	 
		/// </summary>
		public int ApptMinsAvg { get; set; }

		/// <summary>		
		/// PropertyApptMinsMax as INT	 
		/// </summary>
		public int ApptMinsMax { get; set; }

		/// <summary>
		/// PropertyDockSettingControl as INT
		/// </summary>						 
		public int DockSettingControl { get; set; }

		/// <summary>						 
		/// PropertyDockSettingName as STRING
		/// </summary>								
		public string DockSettingName { get; set; }

		/// <summary>						 
		/// PropertyDockSettingDescription as STRING
		/// </summary>								
		public string DockSettingDescription { get; set; }

		/// <summary>									  
		/// PropertyDockSettingOn as bool				  
		/// </summary>									  
		public bool DockSettingOn { get; set; }
														  
		/// <summary>									  
		/// PropertyDockSettingRequireReasonCode as bool  
		/// </summary>									  
		public bool DockSettingRequireReasonCode { get; set; }
														  
		/// <summary>									  
		/// PropertyDockSettingRequireSupervisorPwd as bool
		/// </summary>									   
		public bool DockSettingRequireSupervisorPwd { get; set; }
	}
}