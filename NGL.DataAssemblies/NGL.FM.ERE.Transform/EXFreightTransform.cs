using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.ERE.Transform
{
    public class EXFreightTransform : ITransform
    {
       public EXFreightTransform()
       {

       }
       public DTO.CarrierCostResults getRates(DTO.RateShop rateShop)
       {
           DTO.CarrierCostResults results = new DTO.CarrierCostResults();
           List<DTO.CarriersByCost> resultList = new List<DTO.CarriersByCost>(); 
           if (rateShop != null && rateShop.BookRevs != null && rateShop.BookRevs.Count > 0)
           {
               if (rateShop.BookRevs.Count == 1 &&
                   rateShop.BookRevs[0].BookLoads != null &&
                   rateShop.BookRevs[0].BookLoads.Count >= 1 &&
                   rateShop.BookRevs[0].BookLoads[0].BookItems != null &&
                   rateShop.BookRevs[0].BookLoads[0].BookItems.Count >= 1)
               {
                   EXFreightTransform exfT = new EXFreightTransform();

                   string un = "ccada@nextgeneration.com";
                   string pw = "";// "inverness";
                   //string url = "https://exfreight.service.com/exfreight/cgi/rate_calculator2.cgi";
                   string url = "https://shipments.exfreight.com/exfreight/cgi/rate_calculator2.cgi";
                   EXFreightRate target = new EXFreightRate(un, url);
                   DateTime shipdate = rateShop.BookRevs[0].BookDateLoad.HasValue ? rateShop.BookRevs[0].BookDateLoad.Value : DateTime.MinValue;
                   string pickupzip = rateShop.BookRevs[0].BookOrigZip;
                   string delzip = rateShop.BookRevs[0].BookDestZip;
                   string freightclass = rateShop.BookRevs[0].BookLoads[0].BookItems[0].BookItemFAKClass;

                   List<NGL.FM.ERE.EXFreightRate.requiredItems> reqItems = new List<NGL.FM.ERE.EXFreightRate.requiredItems>();
                   //loop through the bookfees.
                   //reqItems.Add(EXFreightRate.requiredItems.Lift_Gate_Pickup);
                   //reqItems.Add(EXFreightRate.requiredItems.Residential_Pickup);
                   List<NGL.FM.ERE.ExFreight_Carton> cartons = new List<NGL.FM.ERE.ExFreight_Carton>();
                   foreach (DTO.BookItem item in rateShop.BookRevs[0].BookLoads[0].BookItems)
                   {
                       NGL.FM.ERE.ExFreight_Carton carton = new NGL.FM.ERE.ExFreight_Carton(item.BookItemQtyOrdered, item.BookItemQtyLength, item.BookItemQtyWidth, item.BookItemQtyHeight, EXFreightRate.INCHES, item.BookItemWeight, EXFreightRate.POUNDS);
                       cartons.Add(carton);
                   }
                   target.setLTLRateParameters(cartons, freightclass, pickupzip, delzip, shipdate, reqItems);
                   EXFreight_Result EXresult = target.sendLTLRateRequest();
                   if (EXresult != null && EXresult.Success && EXresult.rootNode != null)
                   {
                       XmlNodeList carriers = EXresult.rootNode.SelectNodes("Carriers//Carrier");

                       foreach (XmlNode node in carriers)
                       {
                           DTO.CarriersByCost carCost = new DTO.CarriersByCost();
                           carCost.CarrierName = node.SelectSingleNode(".//Carrier_Name").InnerText;
                           carCost.CarrierCost = decimal.Parse(node.SelectSingleNode(".//Rate").InnerText);
                           carCost.CarrTarEquipMatMaxDays = int.Parse(node.SelectSingleNode(".//Days").InnerText);
                           resultList.Add(carCost);
                       }
                       string noticemessage =  Regex.Replace(EXresult.rootNode.SelectSingleNode(".//Notice").InnerText, @"\r\n?|\n", "");                       
                       if (!string.IsNullOrEmpty(noticemessage))
                       {
                           results.AddMessage(DTO.CarrierCostResults.MessageEnum.None, noticemessage);
                       }
                   } 
                   results.CarriersByCost = resultList; 
                   results.Success = EXresult.Success;
                   return results;
               }
               return results;
           }
           return results;
       }
    }
}
