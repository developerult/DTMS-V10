using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace DynamicsTMS365.Models
{
    public class SettledItem
    {
        
        public int Control { get; set; }
        
        public string InvoiceNumber { get; set; }
        
        public decimal ContractedCost { get; set; }
        
        public decimal PaidCost { get; set; }
        
        public decimal InvoiceAmount { get; set; }
        
        public string PaidDate { get; set; }
        
        public string CheckNumber { get; set; }
        
        public string ProNumber { get; set; }
        
        public string CnsNumber { get; set; }
        
        public string SHID { get; set; }
        
        public string CarrierPro { get; set; }
        
        public int BookFinAPActWgt { get; set; }
        
        public string BookCarrBLNumber { get; set; }
        
        public string CarrierName { get; set; }

        public static Models.SettledItem selectModelData(DTO.SettledItem d)
        {
            Models.SettledItem modelRecord = new Models.SettledItem();
            if (d != null)
            {
                List<string> skipObjs = new List<string> { "PaidDate" };
                string sMsg = "";
                modelRecord = (Models.SettledItem)DTran.CopyMatchingFields(modelRecord, d, skipObjs, ref sMsg);
                if (modelRecord != null) { 
                    if (d.PaidDate.HasValue)
                    {
                        modelRecord.PaidDate = d.PaidDate.Value.ToShortDateString();
                    }
                    else
                    {
                        modelRecord.PaidDate = "Open";
                    }
                }
            }
            return modelRecord;
        }
    }
}