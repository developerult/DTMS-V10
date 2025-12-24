using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Models
{
    public class AMSCarrierBAWrapper
    {

        public DModel.AMSCarrierAvailableSlots AMSCarrierAvailableSlots { get; set; }
        public bool blnIsPickup { get; set; }
    }
}