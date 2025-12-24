using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSCompDockTimeCalcFactor class for Dockdoor time calculation factor
	/// </summary>
	public class AMSCompDockTimeCalcFactor
	{
		/// <summary>				 
		/// PropertyDockTCFControl/ID as INT
		/// </summary>						
		public int DockTCFControl { get; set; }

		/// <summary>
		/// PropertyDockTCFCompDockContol as INT
		/// </summary>							
		public int DockTCFCompDockContol { get; set; }

		/// <summary>
		/// PropertyDockTCFCalcFactorTypeControl as INT
		/// </summary>								   
		public int DockTCFCalcFactorTypeControl { get; set; }

		/// <summary>
		/// PropertyDockTCFAmount as INT
		/// </summary>					
		public int DockTCFAmount { get; set; }

		/// <summary>
		/// PropertyDockTCFTimeFactor as INT
		/// </summary>						
		public int DockTCFTimeFactor { get; set; }

		/// <summary>
		/// PropertyDockTCFModDate as DateTime
		/// </summary>						  
		public DateTime DockTCFModDate { get; set; }
		private string _DockTCFModDate;
													
		/// <summary>								
		/// PropertyDockTCFOn as bool				
		/// </summary>								
		public bool DockTCFOn { get; set; }			

		/// <summary>								
		/// PropertyDockTCFName as STRING			
		/// </summary>								
		public string DockTCFName { get; set; }		

		/// <summary>								
		/// PropertyDockTCFDescription as STRING	
		/// </summary>								
		public string DockTCFDescription { get; set; }

		/// <summary>								  
		/// PropertyDockTCFUOM as STRING			  
		/// </summary>								  
		public string DockTCFUOM { get; set; }


		/// <summary>								  
		/// PropertyDockTCFModUser as STRING		  
		/// </summary>								  
		public string DockTCFModUser { get; set; }
													  
		private byte[] _DockTCFUpdated;
		/// <summary>
		/// DockTCFUpdated Property as STRING
		/// </summary>
		public string DockTCFUpdated
		{
			get
			{
				if (this._DockTCFUpdated != null)
				{

					return Convert.ToBase64String(this._DockTCFUpdated);

				}
				return string.Empty;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{

					this._DockTCFUpdated = null;

				}

				else
				{

					this._DockTCFUpdated = Convert.FromBase64String(value);

				}

			}
		}
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _DockTCFUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the Appointment Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _DockTCFUpdated; }
	}
}