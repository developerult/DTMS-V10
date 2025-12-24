using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;
using DModel = Ngl.FreightMaster.Data.Models;

namespace DynamicsTMS365.Models
{
    public class AMSCarrierAvailableSlotsToken: DModel.AMSCarrierAvailableSlots
    {
        public string sToken { get; set; }
    }
}