using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierTruck
    {
        public int CarrierControl { get; set; }
        public string CarrierName { get; set; }
        public int? CarrierNumber { get; set; }
        public double? CarrierTruckBkhlCostPerc { get; set; }
        public int? CarrierTruckCarrierControl { get; set; }
        public decimal? CarrierTruckCaseRate { get; set; }
        public double CarrierTruckCasesAvailable { get; set; }
        public double CarrierTruckCasesCommitted { get; set; }
        public double CarrierTruckCasesOpen { get; set; }
        public int CarrierTruckControl { get; set; }
        public decimal? CarrierTruckCubeRate { get; set; }
        public double CarrierTruckCubesAvailable { get; set; }
        public double CarrierTruckCubesCommitted { get; set; }
        public double CarrierTruckCubesOpen { get; set; }
        public int? CarrierTruckCurType { get; set; }
        public decimal? CarrierTruckCwtRate { get; set; }
        public string CarrierTruckDescription { get; set; }
        public float? CarrierTruckDisc { get; set; }
        public bool CarrierTruckDLFri { get; set; }
        public bool CarrierTruckDLMon { get; set; }
        public bool CarrierTruckDLSat { get; set; }
        public bool CarrierTruckDLSun { get; set; }
        public bool CarrierTruckDLThu { get; set; }
        public bool CarrierTruckDLTue { get; set; }
        public bool CarrierTruckDLWed { get; set; }
        public double CarrierTruckDropCost { get; set; }
        public string CarrierTruckEquipment { get; set; }
        public string CarrierTruckFAK { get; set; }
        public decimal? CarrierTruckFlatRate { get; set; }
        public double? CarrierTruckFuelSurChargePerc { get; set; }
        public bool CarrierTruckHazmat { get; set; }
        public bool CarrierTruckLTL { get; set; }
        public int CarrierTruckMaxLoadsByMonth { get; set; }
        public int CarrierTruckMaxLoadsByWeek { get; set; }
        public decimal? CarrierTruckMileRate { get; set; }
        public int? CarrierTruckMiles { get; set; }
        public DateTime? CarrierTruckModDate { get; set; }
        public string CarrierTruckModUser { get; set; }
        public DateTime? CarrierTruckMonthDate { get; set; }
        public decimal? CarrierTruckPalletCostPer { get; set; }
        public double? CarrierTruckPayTolCurHi { get; set; }
        public double? CarrierTruckPayTolCurLo { get; set; }
        public double? CarrierTruckPayTolPerHi { get; set; }
        public double? CarrierTruckPayTolPerLo { get; set; }
        public decimal? CarrierTruckPltRate { get; set; }
        public int CarrierTruckPltsAvailable { get; set; }
        public int CarrierTruckPltsCommitted { get; set; }
        public int CarrierTruckPltsOpen { get; set; }
        public bool CarrierTruckPUFri { get; set; }
        public bool CarrierTruckPUMon { get; set; }
        public bool CarrierTruckPUSat { get; set; }
        public bool CarrierTruckPUSun { get; set; }
        public bool CarrierTruckPUThu { get; set; }
        public bool CarrierTruckPUTue { get; set; }
        public bool CarrierTruckPUWed { get; set; }
        public DateTime? CarrierTruckRateExpires { get; set; }
        public DateTime? CarrierTruckRateStarts { get; set; }
        public string CarrierTruckRoute { get; set; }
        public double? CarrierTruckStopCharge { get; set; }
        public string CarrierTruckTempType { get; set; }
        public bool CarrierTruckTL { get; set; }
        public int? CarrierTruckTLT { get; set; }
        public string CarrierTruckTMode { get; set; }
        public int CarrierTruckTotalLoadsForMonth { get; set; }
        public int CarrierTruckTotalLoadsForWeek { get; set; }
        public int CarrierTruckTrucksAvailable { get; set; }
        public double CarrierTruckUnloadDiff { get; set; }
        public byte[] _CarrierTruckUpdated { get; set; }
        public DateTime? CarrierTruckWeekDate { get; set; }
        public double CarrierTruckWgtAvailable { get; set; }
        public double CarrierTruckWgtCommitted { get; set; }
        public int? CarrierTruckWgtFrom { get; set; }
        public double CarrierTruckWgtOpen { get; set; }
        public int? CarrierTruckWgtTo { get; set; }
        public string CurrencyName { get; set; }
        public string EquipmentName { get; set; }

        public string TMode { get; set; }
        public string TempType { get; set; }
        public string CarrierTruckUpdated
        {
            get
            {
                if (this._CarrierTruckUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrierTruckUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrierTruckUpdated = null;

                }

                else
                {

                    this._CarrierTruckUpdated = Convert.FromBase64String(value);

                }

            }
        }
        public void setUpdated(byte[] val) { _CarrierTruckUpdated = val; }
        public byte[] getUpdated() { return _CarrierTruckUpdated; }

    }
}