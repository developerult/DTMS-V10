using Ngl.FreightMaster.Data;
using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.Backend.Tests.Wrappers
{
    public class NGLBookDataWrapper : NGLBookRevenueData
    {
        public NGLBookDataWrapper(WCFParameters oParameters) : base(oParameters)
        {
            this.Parameters = oParameters;
        }


        public decimal calculateFuelCharge_wrapper(ref BookRevenue[] oBookRevs, decimal CarrierCost, int CarrierControl, bool Taxable, int CarrTarControl, int CarrTarEquipControl, ref string Message)
        {
            return calculateFuelCharge(ref oBookRevs, CarrierCost, CarrierControl, Taxable, CarrTarControl, CarrTarEquipControl, ref Message);
        }
    }
}
