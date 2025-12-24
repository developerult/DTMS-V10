using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.P44
{
    //
    // Summary:
    //     /// Initializes a new instance of the P44SDK.V4.Model.ShipmentConfirmation class.
    //     ///
    //
    // Parameters:
    //   ShipmentIdentifiers:
    //     A list of identifiers for the confirmed shipment. Nearly all capacity providers
    //     provide a pickup confirmation number, which will appear in this list with type
    //     'PICKUP'. A few capacity providers also provide a PRO number when a shipment
    //     is confirmed. Shipment identifiers provided by the customer will show up here,
    //     as well..
    //
    //   CapacityProviderBolUrl:
    //     URL pointing to a PDF document of the capacity provider's Bill of Lading, if
    //     available..
    //
    //   PickupNote:
    //     The final note that was sent through the capacity provider's pickup note API
    //     field, as constructed by project44 according to the requested shipment note configuration..
    //
    //   PickupDateTime:
    //     The pickup date and time as provided by the capacity provider in the timezone
    //     of origin location of the shipment. (format: yyyy-MM-dd'T'HH:mm:ss).
    //
    //   InfoMessages:
    //     System messages and messages from the capacity provider with severity 'INFO'
    //     or 'WARNING'. No messages with severity 'ERROR' will be returned here..
    // temporary class
    //  may be needed when we upgrade to new version
    class LocalShipmentConfirmation
    { 
        public LocalShipmentConfirmation(
            List<NGL.FM.P44.LocalShipmentIdentifier> shipmentIdentifiers = null, 
            string capacityProviderBolUrl = null, 
            string pickupNote = null, 
            DateTime? pickupDateTime = default(DateTime?), 
            List<NGL.FM.P44.Message> infoMessages = null)
        {
            this.CapacityProviderBolUrl = capacityProviderBolUrl;
            this.ShipmentIdentifiers = shipmentIdentifiers;
            this.PickupNote = pickupNote;
            this.PickupDateTime = pickupDateTime;
            this.InfoMessages = infoMessages;
        }

        public string CapacityProviderBolUrl { get; set; }
            public List<NGL.FM.P44.Message> InfoMessages { get; set; }
        public DateTime? PickupDateTime { get; set; }

        public string PickupNote { get; set; }

        public List<NGL.FM.P44.LocalShipmentIdentifier> ShipmentIdentifiers { get; set; }

        
    }
}
