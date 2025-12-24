using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// Appointment validation Object as AMSValidation
	/// </summary>
	public class AMSValidation
	{
		/// <summary>
		/// Property SPRequired   as Boolean
		/// </summary>
		public Boolean SPRequired { get; set; }
		/// <summary>
		/// Property InvalidSP    as Boolean
		/// </summary>
		public Boolean InvalidSP { get; set; }
		/// <summary>
		/// Property RCRequired    as Boolean
		/// </summary>
		public Boolean RCRequired { get; set; }
		/// <summary>
		/// Property InvalidRC    as Boolean
		/// </summary>
		public Boolean InvalidRC { get; set; }
		/// <summary>
		/// Property Success    as Boolean
		/// </summary>
		public Boolean Success { get; set; }
		/// <summary>
		/// Property NoOverride    as Boolean
		/// </summary>
		public Boolean NoOverride { get; set; }
		/// <summary>
		/// Property BitString  as String
		/// </summary>
		public String BitString { get; set; }
		/// <summary>
		/// Property Input as String
		/// </summary>
		public String Input { get; set; }
		/// <summary>
		/// Property ReasonCode   as String
		/// </summary>
		public String ReasonCode { get; set; }
		/// <summary>
		/// Property ReasonDesc  as String
		/// </summary>
		public String ReasonDesc { get; set; }
		/// <summary>
		/// Property FailedMsg   as String
		/// </summary>
		public String FailedMsg { get; set; }
		/// <summary>
		/// Property FailedMsgDetails  as  List
		/// </summary>
		public List<String> FailedMsgDetails { get; set; }
	}
}