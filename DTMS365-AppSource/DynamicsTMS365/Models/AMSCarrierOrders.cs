using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{   
    /// <summary>
	/// AMSCarrierOrders class for SCH-Appointments
	/// </summary>
	public class AMSCarrierOrders
	{
        //Added by LVV on 6/7/19
        public int BookOrigCompControl { get; set; }
        public int BookDestCompControl { get; set; }

        /// <summary>
        /// Property BookControl Control/ID as INT	  
        /// </summary>		  
        public int BookControl { get; set; }

		/// <summary>
		/// PropertyBookAMSPickupApptControl as INT
		/// </summary>							   
		public int BookAMSPickupApptControl { get; set; }

		/// <summary>
		/// PropertyBookAMSDeliveryApptControl as INT
		/// </summary>								 
		public int BookAMSDeliveryApptControl { get; set; }

		/// <summary>
		/// PropertyBookCarrierControl as INT
		/// </summary>						 
		public int BookCarrierControl { get; set; }

		/// <summary>
		/// PropertyCarrierNumber as INT
		/// </summary>					
		public int CarrierNumber { get; set; }

		/// <summary>
		/// PropertyBookCarrierContControl as INT
		/// </summary>							 
		public int BookCarrierContControl { get; set; }

		/// <summary>
		/// PropertyBookCustCompControl as INT
		/// </summary>						  
		public int BookCustCompControl { get; set; }

		/// <summary>
		/// PropertyCompNumber as INT
		/// </summary>				 
		public int CompNumber { get; set; }

		/// <summary>
		/// PropertyBookTotalCases as INT
		/// </summary>					 
		public int BookTotalCases { get; set; }

		/// <summary>
		/// PropertyBookTotalCube as INT
		/// </summary>					
		public int BookTotalCube { get; set; }

		/// <summary>
		/// PropertyLaneControl as INT
		/// </summary>				  
		public int LaneControl { get; set; }

		/// <summary>
		/// PropertyBookDateLoad as DateTime
		/// </summary>						
		public DateTime? BookDateLoad { get; set; }

		/// <summary>
		/// PropertyBookDateRequired as DateTime
		/// </summary>							
		public DateTime? BookDateRequired { get; set; }
													  
		/// <summary>								  
		/// PropertyScheduledDate as DateTime		  
		/// </summary>								  
		public DateTime? ScheduledDate { get; set; }	  
													  
		/// <summary>								  
		/// PropertyScheduledTime as DateTime		
		/// </summary>								
		public DateTime? ScheduledTime { get; set; }	
													
		/// <summary>								
		/// PropertyBookTotalWgt as Decimal			
		/// </summary>								
		public Decimal BookTotalWgt { get; set; }	

		/// <summary>								
		/// PropertyBookTotalPL as Decimal			
		/// </summary>								
		public Decimal BookTotalPL { get; set; }	

		/// <summary>								
		/// PropertyCompNEXTrack as bool		
		/// </summary>							
		public bool CompNEXTrack { get; set; }	

		/// <summary>
		/// PropertyBookSHID as STRING
		/// </summary>				  
		public string BookSHID { get; set; }

		/// <summary>
		/// PropertyBookConsPrefix as STRING
		/// </summary>						
		public string BookConsPrefix { get; set; }

		/// <summary>							  
		/// PropertyBookCarrOrderNumber as STRING 
		/// </summary>							  
		public string BookCarrOrderNumber { get; set; }

		/// <summary>								   
		/// PropertyBookProNumber as STRING			   
		/// </summary>								   
		public string BookProNumber { get; set; }	   

		/// <summary>								   
		/// PropertyBookLoadPONumber as STRING		   
		/// </summary>								   
		public string BookLoadPONumber { get; set; }
													   
		/// <summary>								   
		/// PropertyBookCarrTrailerNo as STRING		   
		/// </summary>								   
		public string BookCarrTrailerNo { get; set; }  

		/// <summary>								   
		/// PropertyBookNotesVisable2 as STRING		   
		/// </summary>								   
		public string BookNotesVisable2 { get; set; }  

		/// <summary>								   
		/// PropertyBookNotesVisable3 as STRING		   
		/// </summary>								   
		public string BookNotesVisable3 { get; set; }  

		/// <summary>								   
		/// PropertyLaneNumber as STRING			   
		/// </summary>								   
		public string LaneNumber { get; set; }		   
															   
		/// <summary>
		/// PropertyBookCarrierContactPhone as STRING
		/// </summary>								 
		public string BookCarrierContactPhone { get; set; }


		/// <summary>								 
		/// PropertyBookTranCode as STRING			 
		/// </summary>								 
		public string BookTranCode { get; set; }	 

		/// <summary>								 
		/// PropertyBookWhseAuthorizationNo as STRING
		/// </summary>								 
		public string BookWhseAuthorizationNo { get; set; }

		/// <summary>									   
		/// PropertyBookCarrDockPUAssigment as STRING	   
		/// </summary>									   
		public string BookCarrDockPUAssigment { get; set; }
														   
		/// <summary>									   
		/// PropertyBookCarrDockDelAssignment as STRING	   
		/// </summary>									   
		public string BookCarrDockDelAssignment { get; set; }


		/// <summary>									   
		/// PropertyBookNotesVisable1 as STRING			   
		/// </summary>									   
		public string BookNotesVisable1 { get; set; }	   

		/// <summary>									   
		/// PropertyZip as STRING						   
		/// </summary>									   
		public string Zip { get; set; }
														   
		/// <summary>									   
		/// PropertyCarrierName as STRING				   
		/// </summary>									   
		public string CarrierName { get; set; }			   

		/// <summary>									   
		/// PropertyBookShipCarrierProNumber as STRING	   
		/// </summary>									   
		public string BookShipCarrierProNumber { get; set; }

		/// <summary>										
		/// PropertyBookShipCarrierNumber as STRING			
		/// </summary>										
		public string BookShipCarrierNumber { get; set; }	

		/// <summary>										
		/// PropertyBookShipCarrierName as STRING			
		/// </summary>										
		public string BookShipCarrierName { get; set; }		

		/// <summary>										
		/// PropertyBookCarrierContact as STRING			
		/// </summary>										
		public string BookCarrierContact { get; set; }		

		/// <summary>										
		/// PropertyWarehouse as STRING						
		/// </summary>										
		public string Warehouse { get; set; }
															
		/// <summary>										
		/// PropertyAddress1 as STRING						
		/// </summary>										
		public string Address1 { get; set; }				

		/// <summary>										
		/// PropertyAddress2 as STRING						
		/// </summary>										
		public string Address2 { get; set; }
															
		/// <summary>										
		/// PropertyCity as STRING							
		/// </summary>										
		public string City { get; set; }
															
		/// <summary>										
		/// PropertyState as STRING							
		/// </summary>										
		public string State { get; set; }					

		/// <summary>										
		/// PropertyCountry as STRING						
		/// </summary>										
		public string Country { get; set; }

		/// <summary>								
		/// Inbound as bool		
		/// </summary>							
		public bool Inbound { get; set; }

		/// <summary>								
		/// IsTransfer as bool		
		/// </summary>							
		public bool IsTransfer { get; set; }

		/// <summary>								
		/// IsPickup as bool		
		/// </summary>							
		public bool IsPickup { get; set; }

		/// <summary>								
		/// EquipmentID as string		
		/// </summary>							
		public string EquipmentID { get; set; }
	}

	/// <summary>
	/// EquipIDValidation as object
	/// </summary>
	public class EquipIDValidation
	{
		/// <summary>
		/// BookControl as int
		/// </summary>
		public int BookControl { get; set; }
		/// <summary>
		/// ApptControl as int
		/// </summary>
		public int ApptControl { get; set; }
		/// <summary>
		/// EquipID as string
		/// </summary>
		public string EquipID { get; set; }
		/// <summary>
		/// IsPickup as Boolean
		/// </summary>
		public Boolean IsPickup { get; set; }
		/// <summary>
		/// Success as Boolean
		/// </summary>
		public Boolean Success { get; set; }
		/// <summary>
		/// IsAdd as Boolean
		/// </summary>
		public Boolean IsAdd { get; set; }
		/// <summary>
		/// ErrMsg as string
		/// </summary>
		public string ErrMsg { get; set; }
	}

	/// <summary>
	/// UpdateEquipID as object
	/// </summary>
	public class UpdateEquipID
	{
		/// <summary>
		/// order as object
		/// </summary>
		public AMSCarrierOrders order { get; set; }
		/// <summary>
		/// equipID as object
		/// </summary>
		public EquipIDValidation equipID { get; set; }

	}

    /// <summary>
    /// Used by Carrier Scheduler Grouped page 
    /// </summary>
    public class UpdateEquipIDCSG
    {
        public string SHID { get; set; }
        public Ngl.FreightMaster.Data.Models.EquipIDValidation equipIDValidation { get; set; }
    }

    /// <summary>
    /// AMSCarrierOrdersChart as object
    /// </summary>
    public class AMSCarrierOrdersChart
	{
		/// <summary>
		/// BookDateLoad as string
		/// </summary>
		public string OrderDays { get; set; }
		/// <summary>
		/// pickup as int
		/// </summary>
		public int pickup { get; set; }
		/// <summary>
		/// delivery as int
		/// </summary>
		public int delivery { get; set; }
	}
}