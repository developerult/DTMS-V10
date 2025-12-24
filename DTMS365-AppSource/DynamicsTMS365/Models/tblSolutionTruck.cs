using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class tblSolutionTruck
    {
        public int SolutionTruckAttributeControl { get; set; }
        public int SolutionTruckAttributeTypeControl { get; set; }
        public string SolutionTruckBookNotes { get; set; }
        public int SolutionTruckCapacityPreference { get; set; }
        public int SolutionTruckCarrierControl { get; set; }
        public string SolutionTruckCarrierEquipmentCodes { get; set; }
        public string SolutionTruckCarrierName { get; set; }
        public int SolutionTruckCarrierNumber { get; set; }
        public int SolutionTruckCarrierTruckControl { get; set; }
        public string SolutionTruckCarrierTruckDescription { get; set; }
        public string SolutionTruckCom { get; set; }
        public bool SolutionTruckCommitted { get; set; }
        public DateTime? SolutionTruckCommittedDate { get; set; }
        public string SolutionTruckConsPrefix { get; set; }
        public long SolutionTruckControl { get; set; }
        public bool SolutionTruckIsHazmat { get; set; }
        public string SolutionTruckLaneNames { get; set; }
        public string SolutionTruckLaneNumbers { get; set; }
        public double SolutionTruckMaxCases { get; set; }
        public double SolutionTruckMaxCubes { get; set; }
        public int SolutionTruckMaxPlts { get; set; }
        public double SolutionTruckMaxWgt { get; set; }
        public double SolutionTruckMinCases { get; set; }
        public double SolutionTruckMinCubes { get; set; }
        public int SolutionTruckMinPlts { get; set; }
        public double SolutionTruckMinWgt { get; set; }
        public DateTime? SolutionTruckModDate { get; set; }
        public string SolutionTruckModUser { get; set; }
        public bool SolutionTruckRouteConsFlag { get; set; }
        public int SolutionTruckRouteTypeCode { get; set; }
        public long SolutionTruckSolutionControl { get; set; }
        public double SolutionTruckSplitCases { get; set; }
        public double SolutionTruckSplitCubes { get; set; }
        public int SolutionTruckSplitPlts { get; set; }
        public double SolutionTruckSplitWgt { get; set; }
        public int SolutionTruckStaticRouteControl { get; set; }
        public decimal SolutionTruckTotalBFC { get; set; }
        public int SolutionTruckTotalCases { get; set; }
        public double SolutionTruckTotalCost { get; set; }
        public int SolutionTruckTotalCube { get; set; }
        public double SolutionTruckTotalMiles { get; set; }
        public int SolutionTruckTotalOrders { get; set; }
        public double SolutionTruckTotalPL { get; set; }
        public int SolutionTruckTotalPX { get; set; }
        public double SolutionTruckTotalWgt { get; set; }
        public int SolutionTruckTransType { get; set; }
        public int SolutionTruckTrucksAvailable { get; set; }
        public byte[] _SolutionTruckUpdated { get; set; }
        public tblSolution tblSolution { get; set; }
        public tblSolutionDetail tblSolutionDetails { get; set; }

        public string SolutionTruckUpdated
        {
            get
            {
                if (this._SolutionTruckUpdated != null)
                {

                    return Convert.ToBase64String(this._SolutionTruckUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._SolutionTruckUpdated = null;

                }

                else
                {

                    this._SolutionTruckUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _SolutionTruckUpdated = val; }

        public byte[] getUpdated() { return _SolutionTruckUpdated; }


    }
}