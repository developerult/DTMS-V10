using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
//using DTO = 

namespace NGL.FM.ERE.Transform
{
    public class TransformERE
    {
        public TransformERE()
        {
        }
        public DTO.CarrierCostResults Rate(DTO.RateShop rateShop)
        {
            DTO.CarrierCostResults result = new DTO.CarrierCostResults();
            List<DTO.CarriersByCost> resultList = new List<DTO.CarriersByCost>();
            if (rateShop != null && rateShop.BookRevs != null && rateShop.BookRevs.Count > 0)
            {
                if (rateShop.BookRevs.Count == 1 &&
                    rateShop.BookRevs[0].BookLoads != null &&
                    rateShop.BookRevs[0].BookLoads.Count >= 1 &&
                    rateShop.BookRevs[0].BookLoads[0].BookItems != null &&
                    rateShop.BookRevs[0].BookLoads[0].BookItems.Count >= 1)
                {
                    ITransform exfT = new EXFreightTransform();
                    DTO.CarrierCostResults exResults = exfT.getRates(rateShop);
                    if (exResults != null)
                    {
                        result.CarriersByCost = exResults.CarriersByCost;
                        result.Messages = exResults.Messages;
                        result.Success = exResults.Success;
                    }
                }
                else
                {
                    result.Messages = new Dictionary<string, List<DTO.NGLMessage>>();
                    result.AddMessage(DTO.CarrierCostResults.MessageEnum.M_ReqFieldMissingDensityRating, "");
                }
            }
            return result;
        }
    }
    public class RateResult
    {
        public RateResult()
        {
        }
        public bool Success { get; set; }
        public string message { get; set; }
        public List<DTO.CarriersByCost> result { get; set; }
    }

    public interface ITransform
    {
       DTO.CarrierCostResults getRates(DTO.RateShop rateShop);
    }
}
