using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.P44
{
    public enum TypeEnum
    {
        //
        // Summary:
        //     /// Enum BILLOFLADING for "BILL_OF_LADING" ///
        BILLOFLADING = 0,
        //
        // Summary:
        //     /// Enum PURCHASEORDER for "PURCHASE_ORDER" ///
        PURCHASEORDER = 1,
        //
        // Summary:
        //     /// Enum CUSTOMERREFERENCE for "CUSTOMER_REFERENCE" ///
        CUSTOMERREFERENCE = 2,
        //
        // Summary:
        //     /// Enum PICKUP for "PICKUP" ///
        PICKUP = 3,
        //
        // Summary:
        //     /// Enum PRO for "PRO" ///
        PRO = 4,
        //
        // Summary:
        //     /// Enum SYSTEMGENERATED for "SYSTEM_GENERATED" ///
        SYSTEMGENERATED = 5,
        //
        // Summary:
        //     /// Enum EXTERNAL for "EXTERNAL" ///
        EXTERNAL = 6
    }

    //
    // Summary:
    //     /// Initializes a new instance of the P44SDK.V4.Model.ShipmentIdentifier class.
    //     ///
    //
    // Parameters:
    //   Type:
    //     The type of this shipment identifier or reference number. (required).
    //
    //   Value:
    //     The value of this shipment identifier or reference number. (required).

    // temporary class
    //  may be needed when we upgrade to new version
    class LocalShipmentIdentifier
    {
        public LocalShipmentIdentifier(
            TypeEnum? type = default(TypeEnum?), 
            string value = null)
        {
            this.Type = type;
            this.Value = value;
  
        }
        public TypeEnum? Type { get; set; }
        public string Value { get; set; }
      
    }
}
