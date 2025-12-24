using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.Integration.CSharpSample
{
    class Program
    {

        public enum WebServiceReturnValues
        {
            nglDataIntegrationComplete,
            nglDataConnectionFailure,
            nglDataValidationFailure,
            nglDataIntegrationFailure,
            nglDataIntegrationHadErrors
        }

        /// <summary>
        /// This sample app will create a test company, test carrier, test lane and a test order.  
        /// It does not delete the test data this must be performed manually.
        /// Only sample fields are populated as an example production integration will 
        /// use more data fields.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("begin test");
                // uncomment the functions below to test  change variables as needed

                int CompNumber = 1;  //set to zero to have system assign number
                string CompAlphaCode = "BLUE";
                string CompLegalEntity = "CRONUS USA, Inc.";
                string CompAbrev = "BLU";
                string LaneNumber = "BLU-Customer-01";
                ////create a company
                //if (!createCompany(CompNumber,CompAlphaCode,CompLegalEntity,CompAbrev)) { return; }
                ////create a carrier
                //if (!createCarrier(CompLegalEntity)) { return; }
                ////Create a lane
                //if (!createLane(CompNumber,CompAlphaCode,CompLegalEntity,LaneNumber)) { return; }
                ////Create an Order
               // if (!createBook(CompNumber,CompAlphaCode,CompLegalEntity,LaneNumber)) { return; } 
                if (!createBookWLane(CompNumber, CompAlphaCode, CompLegalEntity, LaneNumber)) { return; }
                ////Read Pick List Records if any are available
                // if (!readPickList()) { return; }
                ////Read AP Export Records if any are available
                //if (!readAPExport()) { return; }
                Console.WriteLine("Services Tested!");
                               

            }
            catch (Exception ex)
            {
                Console.WriteLine("Integration Test Warning: {0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("Press Enter To Continue");
                Console.ReadLine();

            }
        }

       static bool createCompany(int CompNumber,string CompAlphaCode,string CompLegalEntity,string CompAbrev)
        {
            bool blnRet = false;
            // create a company
            DTMSIntegration.clsCompanyHeaderObject70 oCompHeader = new DTMSIntegration.clsCompanyHeaderObject70           
            {
                CompName = "NGL Test Company",
                CompNumber = CompNumber,
                CompAlphaCode = CompAlphaCode,
                CompLegalEntity = CompLegalEntity,
                CompAbrev = CompAbrev,
                CompStreetAddress1 = "123 Some Street",
                CompStreetCity = "Some Town",
                CompStreetState = "IL",
                CompStreetCountry = "US",
                CompStreetZip = "60611",
                CompMailAddress1 = "123 Some Street",
                CompMailCity = "Some Town",
                CompMailState = "IL",
                CompMailCountry = "US",
                CompMailZip = "60611"
            };
            DTMSIntegration.clsCompanyContactObject70 oCompContact = new DTMSIntegration.clsCompanyContactObject70
            {
                CompAlphaCode = CompAlphaCode,
                CompNumber = CompNumber,
                CompLegalEntity = CompLegalEntity,
                CompContName = "Rob",
                CompContEmail = "Rob@SomeDomain.com",
                CompContPhone = "1-800-555-1212"
            };
            //add the data to an array
            DTMSIntegration.clsCompanyHeaderObject70[] oCompHeaders = new DTMSIntegration.clsCompanyHeaderObject70[1];
            oCompHeaders[0] = oCompHeader;
            DTMSIntegration.clsCompanyContactObject70[] oCompContacts = new DTMSIntegration.clsCompanyContactObject70[1];
            oCompContacts[0] = oCompContact;
            //Optional create a calendar object or an empty array 
            DTMSIntegration.clsCompanyCalendarObject70[] oCompCalendar = new DTMSIntegration.clsCompanyCalendarObject70[1];
            DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
            string ReturnMessage = "";
            string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
            DTMSIntegration.clsIntegrationUpdateResults NGLRet = oDTMSERPIntegration.ProcessCompanyData70(WebAuthCode, oCompHeaders, oCompContacts, null, ref ReturnMessage);

            switch (NGLRet.ReturnValue)
            {
                case DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                    Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                    break;
                case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                    Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                    break;
                case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                    Console.WriteLine("Some Errors: " + ReturnMessage);
                    break;
                case DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                    Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                    break;
                case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                    Console.WriteLine("Success! Company Data imported.");
                    Console.WriteLine("Return Message: " + ReturnMessage);
                    blnRet = true;
                    break;
                default:
                    Console.WriteLine("Invalid Return Value.");
                    break;
            }

            return blnRet;

        }
        
       static bool createCarrier(string LegalEntity)
       {
           bool blnRet = false;
           // create a company
           DTMSIntegration.clsCarrierHeaderObject70 oCarrierHeader = new DTMSIntegration.clsCarrierHeaderObject70
           {
               CarrierName = "NGL Test Carrier",
               CarrierNumber = 0,
               CarrierAlphaCode = "NGLCarrAlpha",
               CarrierLegalEntity = LegalEntity,
               CarrierStreetAddress1 = "123 Some Street",
               CarrierStreetCity = "Some Town",
               CarrierStreetState = "IL",
               CarrierStreetCountry = "US",
               CarrierStreetZip = "60611",
               CarrierMailAddress1 = "123 Some Street",
               CarrierMailCity = "Some Town",
               CarrierMailState = "IL",
               CarrierMailCountry = "US",
               CarrierMailZip = "60611"
           };
           DTMSIntegration.clsCarrierContactObject70 oCarrierContact = new DTMSIntegration.clsCarrierContactObject70
           {
               CarrierAlphaCode = "NGLCarrAlpha",
               CarrierNumber = 0,
               CarrierLegalEntity = LegalEntity,
               CarrierContName = "Rob",
               CarrierContactEMail = "Rob@SomeDomain.com",
               CarrierContactPhone = "1-800-555-1212"
           };
           //add the data to an array
           DTMSIntegration.clsCarrierHeaderObject70[] oCarrierHeaders = new DTMSIntegration.clsCarrierHeaderObject70[1];
           oCarrierHeaders[0] = oCarrierHeader;
           DTMSIntegration.clsCarrierContactObject70[] oCarrierContacts = new DTMSIntegration.clsCarrierContactObject70[1];
           oCarrierContacts[0] = oCarrierContact;

           DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
           //you can change the web service URL at run time if desired
           oDTMSERPIntegration.Url ="http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
           string ReturnMessage = "";
           string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
           DTMSIntegration.clsIntegrationUpdateResults NGLRet = oDTMSERPIntegration.ProcessCarrierData70(WebAuthCode, oCarrierHeaders, oCarrierContacts, ref ReturnMessage);

           switch (NGLRet.ReturnValue)
           {
               case DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                   Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                   Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                   Console.WriteLine("Some Errors: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                   Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                   Console.WriteLine("Success! Carrier Data imported.");
                   Console.WriteLine("Return Message: " + ReturnMessage);
                   blnRet = true;
                   break;
               default:
                   Console.WriteLine("Invalid Return Value.");
                   break;
           }

           return blnRet;

       }


        static bool createLane(int CompNumber, string CompAlphaCode, string CompLegalEntity, string LaneNumber)
       {
           bool blnRet = false;
           // create a company
           DTMSIntegration.clsLaneObject80 oLaneHeader = new DTMSIntegration.clsLaneObject80
           {
               LaneName = "Customer Test Lane",
               LaneNumber = LaneNumber,
               LaneCompAlphaCode = CompAlphaCode,
               LaneCompNumber = CompNumber.ToString(),
               LaneOrigCompAlphaCode = CompAlphaCode,
               LaneOrigCompNumber = CompNumber.ToString(),
               LaneLegalEntity = CompLegalEntity,
               LaneDestCompNumber = "1000010",
               //LaneDestName = "Customer 01",
               //LaneDestAddress1 = "123 Any Street",
               //LaneDestCity = "Any Town",
               //LaneDestState = "IL",
               //LaneDestCountry = "US",
               //LaneDestZip = "60611",
               LaneOriginAddressUse = false,
               LanePalletType = "N",
               LaneBFC = 100,
               LaneBFCType = "PERC",
               LaneComments = "comments",
               LaneCommentsConfidential = "confidential comments"
           };
          
           //add the data to an array
           DTMSIntegration.clsLaneObject80[] oLaneHeaders = new DTMSIntegration.clsLaneObject80[1];
           oLaneHeaders[0] = oLaneHeader;
           //create a calendar object or an empty array 
           //DTMSIntegration.clsLaneCalendarObject70[] oLaneCalendar = new DTMSIntegration.clsLaneCalendarObject70[1];
           
           DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded

           string ReturnMessage = "";
           string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
           DTMSIntegration.clsIntegrationUpdateResults NGLRet = oDTMSERPIntegration.ProcessLaneData80(WebAuthCode, oLaneHeaders, null, ref ReturnMessage);

           switch (NGLRet.ReturnValue)
           {
               case DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                   Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                   Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                   Console.WriteLine("Some Errors: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                   Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                   break;
               case DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                   Console.WriteLine("Success! Lane Data imported.");
                   Console.WriteLine("Return Message: " + ReturnMessage);
                   blnRet = true;
                   break;
               default:
                   Console.WriteLine("Invalid Return Value.");
                   break;
           }

           return blnRet;

       }

       static bool createBook(int CompNumber, string CompAlphaCode, string CompLegalEntity, string LaneNumber)
       {
           bool blnRet = false;
           // create a Booking
           string sOrderNumber = "SO-TEST12348"; 
           DTMSIntegration.clsBookHeaderObject80 oBookHeader = new DTMSIntegration.clsBookHeaderObject80
           {
               PONumber = sOrderNumber,
               POVendor = LaneNumber, //Must Match Previous Lane Created
               PODefaultCustomer = CompNumber.ToString(), //Must Match Previous Company Created
               POCompAlphaCode = CompAlphaCode,//Must Match Previous Company Created
               POCompLegalEntity = CompLegalEntity,//Must Match Previous Company Created
               POdate = System.DateTime.Now.ToShortDateString(),
               POShipdate = System.DateTime.Now.ToShortDateString(),
               POWgt = 14000.00,
               POCube = 100,
               POQty = 100,
               POPallets = 1,
               POReqDate = System.DateTime.Now.ToShortDateString(),
               POCustomerPO = "XXX",
               POStatusFlag = 0,
               POUser1 = "User1 Data",
               POUser2 = "User 2 Data",
               POUser3 = "User 3 Data",
               POUser4 = "User 4 Data"
           };
           DTMSIntegration.clsBookDetailObject80 oBookItem = new DTMSIntegration.clsBookDetailObject80
           {
               POItemCompAlphaCode = CompAlphaCode,
               POItemCompLegalEntity = CompLegalEntity,
               CustomerNumber = CompNumber.ToString(),
               ItemPONumber = sOrderNumber,  //must match the order header record above
               ItemNumber = "ABC",
               QtyOrdered = 1,
               Weight = 14000.00,
               Cube = 100,
               POItemPallets = 1,
               POItemUser1 = "User1 Item Data",
               POItemUser2 = "User2 Item Data",
               POItemUser3 = "User3 Item Data",
               POItemUser4 = "User4 Item Data"
           };
           //add the data to an array
           DTMSIntegration.clsBookHeaderObject80[] oBookHeaders = new DTMSIntegration.clsBookHeaderObject80[1];
           oBookHeaders[0] = oBookHeader;
           DTMSIntegration.clsBookDetailObject80[] oBookItems = new DTMSIntegration.clsBookDetailObject80[1];
           oBookItems[0] = oBookItem;

           DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
            string ReturnMessage = "";
           string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
          int iRet = oDTMSERPIntegration.ProcessBookData80(WebAuthCode, oBookHeaders, oBookItems, ref ReturnMessage); //note: the book method retuns an int and must be converted to the enum to process

           switch(iRet)
           {
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                   Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                   Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                   Console.WriteLine("Some Errors: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                   Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                   Console.WriteLine("Success! Book Data imported.");
                   Console.WriteLine("Return Message: " + ReturnMessage);
                   blnRet = true;
                   break;
               default:
                   Console.WriteLine("Invalid Return Value.");
                   break;
           }

           return blnRet;

       }


        static bool createBookWLane(int CompNumber, string CompAlphaCode, string CompLegalEntity, string LaneNumber)
        {
            bool blnRet = false;
            // create a Booking
            string sOrderNumber = "SO-WLane12348";
            DTMSIntegration.clsBookHeaderObject80 oBookHeader = new DTMSIntegration.clsBookHeaderObject80
            {
                PONumber = sOrderNumber,
                POVendor = LaneNumber, //Must Match Previous Lane Created
                PODefaultCustomer = CompNumber.ToString(), //Must Match Previous Company Created
                POCompAlphaCode = CompAlphaCode,//Must Match Previous Company Created
                POCompLegalEntity = CompLegalEntity,//Must Match Previous Company Created
                POdate = System.DateTime.Now.ToShortDateString(),
                POShipdate = System.DateTime.Now.ToShortDateString(),
                POWgt = 14000.00,
                POCube = 100,
                POQty = 100,
                POPallets = 1,
                POReqDate = System.DateTime.Now.ToShortDateString(),
                POCustomerPO = "XXX",
                POStatusFlag = 0,
                POUser1 = "User1 Data",
                POUser2 = "User 2 Data",
                POUser3 = "User 3 Data",
                POUser4 = "User 4 Data"
            };

            oBookHeader.POStatusFlag = 5;
               oBookHeader.POOrigCompNumber = oBookHeader.PODefaultCustomer;
            oBookHeader.POOrigName = "Test Orig";
            oBookHeader.POOrigAddress1 = "123some street";
               oBookHeader.POOrigCity = "Some City";
            oBookHeader.POOrigState = "Some State";
            oBookHeader.POOrigCountry = "US";
            oBookHeader.POOrigZip = "60606";
            oBookHeader.PODestName = "Test Dest";
            oBookHeader.PODestAddress1 = "4506 Some Road";
            oBookHeader.PODestCity = "Tampa";
            oBookHeader.PODestState = "FL";
            oBookHeader.PODestCountry = "US";
            oBookHeader.PODestZip = "33609";
            oBookHeader.POFrt = 8;
            oBookHeader.POModeTypeControl = 2;
            
            DTMSIntegration.clsBookDetailObject80 oBookItem = new DTMSIntegration.clsBookDetailObject80
            {
                POItemCompAlphaCode = CompAlphaCode,
                POItemCompLegalEntity = CompLegalEntity,
                CustomerNumber = CompNumber.ToString(),
                ItemPONumber = sOrderNumber,  //must match the order header record above
                ItemNumber = "ABC",
                QtyOrdered = 1,
                Weight = 14000.00,
                Cube = 100,
                POItemPallets = 1,
                POItemUser1 = "User1 Item Data",
                POItemUser2 = "User2 Item Data",
                POItemUser3 = "User3 Item Data",
                POItemUser4 = "User4 Item Data"
            };
            //add the data to an array
            DTMSIntegration.clsBookHeaderObject80[] oBookHeaders = new DTMSIntegration.clsBookHeaderObject80[1];
            oBookHeaders[0] = oBookHeader;
            DTMSIntegration.clsBookDetailObject80[] oBookItems = new DTMSIntegration.clsBookDetailObject80[1];
            oBookItems[0] = oBookItem;

            DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
            string ReturnMessage = "";
            string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
            int iRet = oDTMSERPIntegration.ProcessBookData80(WebAuthCode, oBookHeaders, oBookItems, ref ReturnMessage); //note: the book method retuns an int and must be converted to the enum to process

            switch (iRet)
            {
                case (int)DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                    Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                    break;
                case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                    Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                    break;
                case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                    Console.WriteLine("Some Errors: " + ReturnMessage);
                    break;
                case (int)DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                    Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                    break;
                case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                    Console.WriteLine("Success! Book Data imported.");
                    Console.WriteLine("Return Message: " + ReturnMessage);
                    blnRet = true;
                    break;
                default:
                    Console.WriteLine("Invalid Return Value.");
                    break;
            }

            return blnRet;

        }
        static bool readPickList()
       {
           bool blnRet = false;
           // create a company
           
           DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
            string ReturnMessage = "";
           string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
           int iRet = 0;
           string sLegalEntity = "CRONUS USA, Inc.";  //Enter company legal entity or empty string to return all records
           int iReadRecords = 10;  //Enter the number of records to read and return
           //Note:  clsPickListData80 will be changed to clsPickListData85 to support new fields 
            DTMSIntegration.clsPickListData85 oPickData = oDTMSERPIntegration.GetPickListData85(WebAuthCode, 3, 5, sLegalEntity, iReadRecords, false, ref iRet, ref ReturnMessage);
           switch (iRet)
           {
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                   Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                   Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                   Console.WriteLine("Some Errors: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                   Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                   Console.WriteLine("Success! PickList Data read.");
                   Console.WriteLine("Return Message: " + ReturnMessage);
                   blnRet = true;
                   break;
               default:
                   Console.WriteLine("Invalid Return Value.");
                   break;
           }

           DTMSIntegration.clsPickListObject80[] oPickListHeaders = oPickData.Headers;
           DTMSIntegration.clsPickDetailObject80[] oPickListDetails = oPickData.Details;
           DTMSIntegration.clsPickListFeeObject80[] oPickListFees = oPickData.Fees;
           if (!(oPickListHeaders == null) && oPickListHeaders.Count() > 0)
           {
               foreach (DTMSIntegration.clsPickListObject70 h in oPickListHeaders )
               {
                    //Sample code used to process pick list records and d confirmation back
                   //to TMS once the data has been read
                   Console.WriteLine("PickList Order Number: {0}",h.BookCarrOrderNumber);
                    string strRetMsg = "";
                   bool bConfirmed = oDTMSERPIntegration.ConfirmPickListExport70(WebAuthCode, h.PLControl ,ref strRetMsg);
                   if (!bConfirmed)
                   {
                       //add custom logic to handle exceptions or errors if confirmation fails
                       Console.WriteLine("Picklist Export Confirmation Failed: " + strRetMsg);
                   }
               }
           }
           return blnRet;

       }


       static bool readAPExport()
       {
           bool blnRet = false;
           // create a company

           
           DTMSIntegration.DTMSERPIntegration oDTMSERPIntegration = new DTMSIntegration.DTMSERPIntegration();
            //you can change the web service URL at run time if desired
            oDTMSERPIntegration.Url = "http://localhost:44320/ws/DTMSERPIntegration.asmx"; //Note: use the URL that was provided.  this should be configureable and not hard coded
            string ReturnMessage = "";
           string WebAuthCode = "WSTEST"; //Note: use the case sensitive Auth Code that was provided.  this should be configureable and not hard coded
          int iRet = 0;
           string sLegalEntity = "";  //Enter company legal entity or empty string to return all records
           int iReadRecords = 10;  //Enter the number of records to read and return
           DTMSIntegration.clsAPExportData80 oAPData = oDTMSERPIntegration.GetAPData80(WebAuthCode, 3, 5, sLegalEntity, iReadRecords, false, ref iRet, ref ReturnMessage);
           switch (iRet)
           {
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataConnectionFailure:
                   Console.WriteLine("Database Connection Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationFailure:
                   Console.WriteLine("Data Integration Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationHadErrors:
                   Console.WriteLine("Some Errors: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataValidationFailure:
                   Console.WriteLine("Data Validation Failure Error: " + ReturnMessage);
                   break;
               case (int)DTMSIntegration.ProcessDataReturnValues.nglDataIntegrationComplete:
                   Console.WriteLine("Success! AP Export Data read.");
                   Console.WriteLine("Return Message: " + ReturnMessage);
                   blnRet = true;
                   break;
               default:
                   Console.WriteLine("Invalid Return Value.");
                   break;
           }

           DTMSIntegration.clsAPExportObject80[] oAPHeader = oAPData.Headers;
           DTMSIntegration.clsAPExportDetailObject80[] oAPDetails = oAPData.Details;
           DTMSIntegration.clsAPExportFeeObject80[] oAPFees = oAPData.Fees;


           if (!(oAPHeader == null) && oAPHeader.Count() > 0)
           {
               foreach (DTMSIntegration.clsAPExportObject80 h in oAPHeader)
               {
                   //Sample code used to process pick list records and sent confirmation back
                   //to TMS once the data has been read
                   ReturnMessage = "";
                   Console.WriteLine("AP Export Order Number: {0}", h.BookCarrOrderNumber);
                   bool bConfirmed = oDTMSERPIntegration.ConfirmAPExport70(WebAuthCode, h.APControl, ref ReturnMessage);
                   if (!bConfirmed)
                   {
                   //add custom logic to handle exceptions or errors if confirmation fails
                        Console.WriteLine("AP Export Confirmation Failed: {0}",ReturnMessage);
                   }
               }
           }
           return blnRet;

       }



       


    }
}
