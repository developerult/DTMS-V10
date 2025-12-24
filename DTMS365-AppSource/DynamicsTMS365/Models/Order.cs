using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Order
    {

         public int BookControl { get; set; }
         public string ShipKey { get; set; }      
 public string BookProNumber { get; set; }
 public string BookConsPrefix { get; set; }
 public string BookRouteConsFlag { get; set; }
 public int BookCustCompControl { get; set; }
public string CompName { get; set; }
public int CompNumber { get; set; }
public string CompAlphaCode { get; set; }
public int BookODControl { get; set; }
public int LaneControl { get; set; }
public Boolean LaneOriginAddressUse { get; set; }
public int BookCarrierControl { get; set; }
public string CarrierName { get; set; }
public int? CarrierNumber { get; set; }
public string CarrierAlphaCode { get; set; }
public string CarrierSCAC { get; set; }
 public string BookCarrierContact { get; set; }
 public string BookCarrierContactPhone { get; set; }
 public string BookOrigName { get; set; }
 public string BookOrigAddress1 { get; set; }
 public string BookOrigCity { get; set; }
 public string BookOrigState { get; set; }
 public string BookOrigCountry { get; set; }
 public string BookOrigZip { get; set; }
 public string BookDestName { get; set; }
 public string BookDestAddress1 { get; set; }
 public string BookDestCity { get; set; }
 public string BookDestState { get; set; }
 public string BookDestCountry { get; set; }
 public string BookDestZip { get; set; }
 public DateTime? BookDateOrdered { get; set; }
 public DateTime? BookDateLoad { get; set; }
 public DateTime? BookDateRequired { get; set; }
 public int? BookTotalCases { get; set; }
 public double? BookTotalWgt { get; set; }
 public double? BookTotalPL { get; set; }
 public int? BookTotalCube { get; set; }
 public string BookTranCode { get; set; }
 public string BookTypeCode { get; set; }
 public Int16? BookStopNo { get; set; }
 public string BookCarrOrderNumber { get; set; }
public string BookSHID { get; set; }
    }
}