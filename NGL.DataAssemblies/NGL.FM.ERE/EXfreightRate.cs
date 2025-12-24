using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Dynamic;
using System.Xml.Linq;
using System.Xml;

namespace NGL.FM.ERE
{
    public class EXFreightRate
    {

        public const string CENTIMETERS = "cm";
        public const string INCHES = "in";
        public const string KILOGRAM = "kg";
        public const string POUNDS = "lb";
         
        //public enum EnumClasses  { , 55, 60, 65, 70, 77.5, 85, 92.5, 100, 110, 125, 150, 175, 200, 250, 300, 400, 500 };

        public enum requiredItems { 
            Inside_Pickup, Lift_Gate_Pickup,
            Residential_Pickup,
            Construction_Site_Pickup,
            Hotel_Pickup,
            Military_Base_Pickup,
            School_Pickup,
            Limited_Access_Pickup,
            Per_Piece_Over_Length_12_20,
            Per_Piece_Over_Length_20_28,
            Per_Piece_Over_Length_Over_28,
            Inside_Delivery,
            Lift_Gate_Delivery,
            Residential_Delivery,
            Construction_Site_Delivery,
            Hotel_Delivery,
            Military_Base_Delivery,
            School_Delivery,
            Limited_Access_Delivery,
            Hazmat,
            Sort_and_Segment,
            CA_US_Border_Fee,
            Delivery_Appointment 
        };

        public enum RateProperties
        {
            API,
            Username,
            Shipment_Type,
            Rate_Type,
            Freight_Class,
            User_Calculated_Freight_Class,
            Ocean_FCL_Commodity,
            Pickup_Zipcode,
            Delivery_Zipcode,
            Ship_Date,
            Origin_Source_Country,
            Air_Destination_Port,
            Ocean_International_Destination_Port,
            Ocean_Domestic_Destination_Port,
            Ocean_FCL_Destination_Port,
            Air_Import_Origin_Port,
            Ocean_Import_Origin_Port,
            Ocean_FCL_Import_Origin_Port,
        }

        public EXFreightRate(string un, string url)
        {
            this.UserName = un;
          //  this.Password = pw;
            
            this.RATING_URL = url;
            this.RequiredItems = new Dictionary<requiredItems, int?>();

            this.RequiredItems.Add(requiredItems.Inside_Pickup, null);
            this.RequiredItems.Add(requiredItems.Lift_Gate_Pickup, null);
            this.RequiredItems.Add(requiredItems.Residential_Pickup, null);
            this.RequiredItems.Add(requiredItems.Construction_Site_Pickup, null);
            this.RequiredItems.Add(requiredItems.Hotel_Pickup, null);
            this.RequiredItems.Add(requiredItems.Military_Base_Pickup, null);
            this.RequiredItems.Add(requiredItems.School_Pickup, null);
            this.RequiredItems.Add(requiredItems.Limited_Access_Pickup, null);
            this.RequiredItems.Add(requiredItems.Per_Piece_Over_Length_12_20, null);
            this.RequiredItems.Add(requiredItems.Per_Piece_Over_Length_20_28, null);
            this.RequiredItems.Add(requiredItems.Per_Piece_Over_Length_Over_28, null);
            this.RequiredItems.Add(requiredItems.Inside_Delivery, null);
            this.RequiredItems.Add(requiredItems.Lift_Gate_Delivery, null);
            this.RequiredItems.Add(requiredItems.Residential_Delivery, null);
            this.RequiredItems.Add(requiredItems.Construction_Site_Delivery, null);
            this.RequiredItems.Add(requiredItems.Hotel_Delivery, null);
            this.RequiredItems.Add(requiredItems.Military_Base_Delivery, null);
            this.RequiredItems.Add(requiredItems.School_Delivery, null);
            this.RequiredItems.Add(requiredItems.Limited_Access_Delivery, null);
            this.RequiredItems.Add(requiredItems.Hazmat, null);
            this.RequiredItems.Add(requiredItems.Sort_and_Segment, null);
            this.RequiredItems.Add(requiredItems.CA_US_Border_Fee, null);            
            this.RequiredItems.Add(requiredItems.Delivery_Appointment, null);
             
        }

        public void setLTLRateParameters(List<ExFreight_Carton> cartons, 
            string freightClass, 
            string pickup_zip, 
            string delivery_zip, 
            DateTime shipdate,
            List<requiredItems> requiredItems)
        { 
            this.Freight_Class = freightClass;  
            this.Pickup_Zipcode = pickup_zip;
            this.Delivery_Zipcode = delivery_zip;
            this.Ship_Date = shipdate; 
                        
            foreach (requiredItems item in requiredItems)
            {
                this.RequiredItems[item] = 1;               
            }

            this.Cartons = cartons;

            
        }



        public EXFreight_Result sendLTLRateRequest()
        {
            EXFreight_Result result = new EXFreight_Result();
            if (this.Cartons !=null)
            {
                this.Last_Row=Cartons.Count;
            }else
            {
                result.Success=false;
                result.message="please create cartons";
                return result;
            }
            if (string.IsNullOrEmpty(this.Freight_Class))
            {
                this.Freight_Class = "100";
            }
            if (string.IsNullOrEmpty(Pickup_Zipcode))
            {
                result.Success = false;
                result.message = "please set pickup zip code";
                return result;
            } 
            if (string.IsNullOrEmpty(Delivery_Zipcode))
            {
                result.Success = false;
                result.message = "please set pickup zip code";
                return result;
            }
            if (this.Ship_Date == null)
            {
                result.Success = false;
                result.message = "please set Ship Date";
                return result;
            }
            this.Shipment_Type = System.Web.HttpUtility.UrlEncode("LTL Trucking");
            this.Rate_Type = System.Web.HttpUtility.UrlEncode("Door To Door");

            WebClient webClient = new WebClient();
            System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();
            
           

            formData[RateProperties.API.ToString()] = "Yes";
            formData["Username"] = System.Web.HttpUtility.UrlEncode(this.UserName);
            // formData["Password"] = this.Password;
            formData[RateProperties.Shipment_Type.ToString()] = this.Shipment_Type;
            formData[RateProperties.Rate_Type.ToString()] = this.Rate_Type;
            formData[RateProperties.Freight_Class.ToString()] = this.Freight_Class;
            formData[RateProperties.User_Calculated_Freight_Class.ToString()] = null;
            formData[RateProperties.Ocean_FCL_Commodity.ToString()] = null; 
            formData[RateProperties.Pickup_Zipcode.ToString()] = this.Pickup_Zipcode;
            formData[RateProperties.Delivery_Zipcode.ToString()] = this.Delivery_Zipcode;
            formData[RateProperties.Ship_Date.ToString()] = this.Ship_Date.Year.ToString() + "-" + this.Ship_Date.Month.ToString() + "-" + this.Ship_Date.Day.ToString();
            formData[RateProperties.Origin_Source_Country.ToString()] = null;
            formData[RateProperties.Air_Destination_Port.ToString()] = null;
            formData[RateProperties.Ocean_International_Destination_Port.ToString()] = null;
            formData[RateProperties.Ocean_Domestic_Destination_Port.ToString()] = null;
            formData[RateProperties.Ocean_FCL_Destination_Port.ToString()] = null;
            formData[RateProperties.Air_Import_Origin_Port.ToString()] = null;
            formData[RateProperties.Ocean_Import_Origin_Port.ToString()] = null;
            formData[RateProperties.Ocean_FCL_Import_Origin_Port.ToString()] = null;
            
            if (this.RequiredItems != null)
            {
                foreach (KeyValuePair<requiredItems,Nullable<int>> item in this.RequiredItems)
                {
                    if (item.Value != null)
                    {
                        formData[item.Key.ToString()] = item.Value.ToString();
                    }
                    else
                    {
                        formData[item.Key.ToString()] = null;
                    }
                }
                
            }
            if (this.Cartons != null && this.Cartons.Count > 0)
            {
                for (int i = 0; i <= this.Cartons.Count - 1; i++)
                {
                    formData[ExFreight_Carton.Carton_Quantity_ + (i + 1)] = this.Cartons[i].Carton_Quantity_Value.ToString();
                    formData[ExFreight_Carton.Carton_Package_ + (i + 1)] =  System.Web.HttpUtility.UrlEncode("My Packaging");
                    formData[ExFreight_Carton.Carton_Length_ + (i + 1)] = this.Cartons[i].Carton_Length_Value.ToString();
                    formData[ExFreight_Carton.Carton_Width_ + (i + 1)] = this.Cartons[i].Carton_Width_Value.ToString();
                    formData[ExFreight_Carton.Carton_Height_ + (i + 1)] = this.Cartons[i].Carton_Height_Value.ToString();
                    formData[ExFreight_Carton.Carton_Measure_ + (i + 1)] = this.Cartons[i].Carton_Measure_Value.ToString();
                    formData[ExFreight_Carton.Carton_Total_Weight_ + (i + 1)] = this.Cartons[i].Carton_Total_Weight_Value.ToString();
                    formData[ExFreight_Carton.Carton_Weight_Measure_ + (i + 1)] = this.Cartons[i].Carton_Weight_Measure_Value.ToString();
                }
                if (this.Cartons.Count == 1)
                {
                    //Carton_Quantity_2=&Carton_Package_2=&Carton_Length_2=&Carton_Width_2=&Carton_Height_2=&Carton_Measure_2=&Carton_Total_Weight_2=&Carton_Weight_Measure_2=
                    formData[ExFreight_Carton.Carton_Quantity_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Package_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Length_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Width_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Height_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Measure_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Total_Weight_ + (2)] = null;
                    formData[ExFreight_Carton.Carton_Weight_Measure_ + (2)] = null;
                }
            }
            else
            {
                formData[ExFreight_Carton.Carton_Quantity_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Package_ + (  1)] =  System.Web.HttpUtility.UrlEncode("My Packaging");
                formData[ExFreight_Carton.Carton_Length_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Width_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Height_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Measure_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Total_Weight_ + (1)] = null;
                formData[ExFreight_Carton.Carton_Weight_Measure_ + (1)] = null;
            }
             
            formData[EXFreight_Truck.Truck_Quantity_ + (  1)] = null;
            formData[EXFreight_Truck.Truck_Type_ + ( 1)] = null;
            formData[EXFreight_Truck.Truck_Length_ + (1)] = null;
            formData[EXFreight_Truck.Truck_Width_ + (1)] = null;
            formData[EXFreight_Truck.Truck_Total_Weight_ + (1)] = null;
            formData[EXFreight_Truck.Truck_Weight_Measure_ + (1)] = null;

            formData[EXFreight_Container.Container_Size_ + (1)] = null;
            formData[EXFreight_Container.Container_Total_Weight_ + (1)] = null;
            formData[EXFreight_Container.Container_Weight_Measure_ + (1)] = null;

            string url = this.RATING_URL + "?" ;
            foreach (string key in formData)
            {
                var value = formData[key];
                url += key + "=" + value + "&";
            }
            url += "Last_Row=" + this.Cartons.Count.ToString();
            webClient.Headers.Add("user-agent",
                   "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            //byte[] responseBytes = webClient.UploadString(this.RATING_URL, "POST", formData);
         
            string test = "https://exfreight.service.com/exfreight/cgi/rate_calculator2.cgi?API=Yes&Username=ccada%40nextgeneration.com&Shipment_Type=LTL+Trucking&Rate_Type=Door+To+Door&Freight_Class=100&User_Calculated_Freight_Class=&Ocean_FCL_Commodity=&Pickup_Zipcode=94306&Delivery_Zipcode=08820&Ship_Date=2015-1-15&Origin_Source_Country=&Air_Destination_Port=&Ocean_International_Destination_Port=&Ocean_Domestic_Destination_Port=&Ocean_FCL_Destination_Port=&Air_Import_Origin_Port=&Ocean_Import_Origin_Port=&Ocean_FCL_Import_Origin_Port=&Inside_Pickup=&Lift_Gate_Pickup=&Residential_Pickup=&Construction_Site_Pickup=&Hotel_Pickup=&Military_Base_Pickup=&School_Pickup=&Limited_Access_Pickup=&Per_Piece_Over_Length_12_20=&Per_Piece_Over_Length_20_28=&Per_Piece_Over_Length_Over_28=&Inside_Delivery=&Lift_Gate_Delivery=&Residential_Delivery=&Construction_Site_Delivery=&Hotel_Delivery=&Military_Base_Delivery=&School_Delivery=&Limited_Access_Delivery=&Hazmat=&Sort_and_Segment=&CA_US_Border_Fee=&Delivery_Appointment=&Carton_Quantity_1=1&Carton_Package_1=My+Packaging&Carton_Length_1=12&Carton_Width_1=12&Carton_Height_1=12&Carton_Measure_1=in&Carton_Total_Weight_1=12&Carton_Weight_Measure_1=lb&Carton_Quantity_2=&Carton_Package_2=&Carton_Length_2=&Carton_Width_2=&Carton_Height_2=&Carton_Measure_2=&Carton_Total_Weight_2=&Carton_Weight_Measure_2=&Truck_Quantity_1=&Truck_Type_1=&Truck_Length_1=&Truck_Width_1=&Truck_Total_Weight_1=&Truck_Weight_Measure_1=&Container_Size_1=&Container_Total_Weight_1=&Container_Weight_Measure_1=&Last_Row=1";
            string tes1 = "https://exfreight.service.com/exfreight/cgi/rate_calculator2.cgi?API=Yes&Username=ccada%40nextgeneration.com&Shipment_Type=LTL+Trucking&Rate_Type=Door+To+Door&Freight_Class=100&User_Calculated_Freight_Class=&Ocean_FCL_Commodity=&Pickup_Zipcode=94306&Delivery_Zipcode=08820&Ship_Date=2015-1-15&Origin_Source_Country=&Air_Destination_Port=&Ocean_International_Destination_Port=&Ocean_Domestic_Destination_Port=&Ocean_FCL_Destination_Port=&Air_Import_Origin_Port=&Ocean_Import_Origin_Port=&Ocean_FCL_Import_Origin_Port=&Inside_Pickup=&Lift_Gate_Pickup=&Residential_Pickup=&Construction_Site_Pickup=&Hotel_Pickup=&Military_Base_Pickup=&School_Pickup=&Limited_Access_Pickup=&Per_Piece_Over_Length_12_20=&Per_Piece_Over_Length_20_28=&Per_Piece_Over_Length_Over_28=&Inside_Delivery=&Lift_Gate_Delivery=&Residential_Delivery=&Construction_Site_Delivery=&Hotel_Delivery=&Military_Base_Delivery=&School_Delivery=&Limited_Access_Delivery=&Hazmat=&Sort_and_Segment=&CA_US_Border_Fee=&Delivery_Appointment=&Carton_Quantity_1=1&Carton_Package_1=My+Packaging&Carton_Length_1=12&Carton_Width_1=12&Carton_Height_1=12&Carton_Measure_1=in&Carton_Total_Weight_1=12&Carton_Weight_Measure_1=lb&Carton_Quantity_2=&Carton_Package_2=&Carton_Length_2=&Carton_Width_2=&Carton_Height_2=&Carton_Measure_2=&Carton_Total_Weight_2=&Carton_Weight_Measure_2=&Truck_Quantity_1=&Truck_Type_1=&Truck_Length_1=&Truck_Width_1=&Truck_Total_Weight_1=&Truck_Weight_Measure_1=&Container_Size_1=&Container_Total_Weight_1=&Container_Weight_Measure_1=&Last_Row=1";
            string tes2 = "https://exfreight.service.com/exfreight/cgi/rate_calculator2.cgi?API=Yes&Username=ccada%40nextgeneration.com&Shipment_Type=LTL+Trucking&Rate_Type=Door+To+Door&Freight_Class=100&User_Calculated_Freight_Class=&Ocean_FCL_Commodity=&Pickup_Zipcode=94306&Delivery_Zipcode=08820&Ship_Date=2015-1-15&Origin_Source_Country=&Air_Destination_Port=&Ocean_International_Destination_Port=&Ocean_Domestic_Destination_Port=&Ocean_FCL_Destination_Port=&Air_Import_Origin_Port=&Ocean_Import_Origin_Port=&Ocean_FCL_Import_Origin_Port=&Inside_Pickup=&Lift_Gate_Pickup=&Residential_Pickup=&Construction_Site_Pickup=&Hotel_Pickup=&Military_Base_Pickup=&School_Pickup=&Limited_Access_Pickup=&Per_Piece_Over_Length_12_20=&Per_Piece_Over_Length_20_28=&Per_Piece_Over_Length_Over_28=&Inside_Delivery=&Lift_Gate_Delivery=&Residential_Delivery=&Construction_Site_Delivery=&Hotel_Delivery=&Military_Base_Delivery=&School_Delivery=&Limited_Access_Delivery=&Hazmat=&Sort_and_Segment=&CA_US_Border_Fee=&Delivery_Appointment=&Carton_Quantity_1=1&Carton_Package_1=My+Packaging&Carton_Length_1=12&Carton_Width_1=12&Carton_Height_1=12&Carton_Measure_1=in&Carton_Total_Weight_1=12&Carton_Weight_Measure_1=lb&Carton_Quantity_2=&Carton_Package_2=&Carton_Length_2=&Carton_Width_2=&Carton_Height_2=&Carton_Measure_2=&Carton_Total_Weight_2=&Carton_Weight_Measure_2=&Truck_Quantity_1=&Truck_Type_1=&Truck_Length_1=&Truck_Width_1=&Truck_Total_Weight_1=&Truck_Weight_Measure_1=&Container_Size_1=&Container_Total_Weight_1=&Container_Weight_Measure_1=&Last_Row=1";
            string tes3 = "https://shipments.exfreight.com/exfreight/cgi/rate_calculator2.cgi?API=Yes&Username=ccada%40nextgeneration.com&Shipment_Type=LTL+Trucking&Rate_Type=Door+To+Door&Freight_Class=100&User_Calculated_Freight_Class=&Ocean_FCL_Commodity=&Pickup_Zipcode=60660&Delivery_Zipcode=60661&Ship_Date=2015-1-27&Origin_Source_Country=US&Air_Destination_Port=&Ocean_International_Destination_Port=&Ocean_Domestic_Destination_Port=&Ocean_FCL_Destination_Port=&Air_Import_Origin_Port=&Ocean_Import_Origin_Port=&Ocean_FCL_Import_Origin_Port=&Inside_Pickup=&Lift_Gate_Pickup=&Residential_Pickup=&Construction_Site_Pickup=&Hotel_Pickup=&Military_Base_Pickup=&School_Pickup=&Limited_Access_Pickup=&Per_Piece_Over_Length_12_20=&Per_Piece_Over_Length_20_28=&Per_Piece_Over_Length_Over_28=&Inside_Delivery=&Lift_Gate_Delivery=&Residential_Delivery=&Construction_Site_Delivery=&Hotel_Delivery=&Military_Base_Delivery=&School_Delivery=&Limited_Access_Delivery=&Hazmat=&Sort_and_Segment=&CA_US_Border_Fee=&Delivery_Appointment=&Carton_Quantity_1=1&Carton_Package_1=My+Packaging&Carton_Length_1=12&Carton_Width_1=12&Carton_Height_1=12&Carton_Measure_1=in&Carton_Total_Weight_1=124&Carton_Weight_Measure_1=lb&Carton_Quantity_2=&Carton_Package_2=&Carton_Length_2=&Carton_Width_2=&Carton_Height_2=&Carton_Measure_2=&Carton_Total_Weight_2=&Carton_Weight_Measure_2=&Truck_Quantity_1=&Truck_Type_1=&Truck_Length_1=&Truck_Width_1=&Truck_Total_Weight_1=&Truck_Weight_Measure_1=&Container_Size_1=&Container_Total_Weight_1=&Container_Weight_Measure_1=&Last_Row=1";
            string xml = webClient.DownloadString(url);
//            string xml = @"<Rating>
//<Rate_Estimate_For>Next Generation Logistics, Inc - LTL Trucking</Rate_Estimate_For>
//<Reference>1419363163015611 / 2014-12-23</Reference>
//<From>94306 Palo Alto, CA</From>
//<To>08820 Edison, NJ</To>
//<Freight_Class>100</Freight_Class>
//<Weight>12.00 lbs / 6.00 kgs (1.00)</Weight>
//<Carriers>
//<Carrier>
//<Carrier_Name>TOWNE AIR FREIGHT, INC.</Carrier_Name>
//<SCAC>TOWE</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>107.09</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>FORWARD AIR, INC</Carrier_Name>
//<SCAC>FWDN</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>112.87</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>FRONTLINE FREIGHT</Carrier_Name>
//<SCAC>FCSY</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>136.89</Rate>
//<Days>7</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>YRC</Carrier_Name>
//<SCAC>RDWY</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>147.23</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ROADRUNNER TRANSPORTATION SERVICES</Carrier_Name>
//<SCAC>RDFS</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>163.80</Rate>
//<Days>6</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>CLEAR LANE FREIGHT SYSTEMS</Carrier_Name>
//<SCAC>7CLN</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>187.06</Rate>
//<Days>6</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ESTES EXPRESS LINES</Carrier_Name>
//<SCAC>EXLA</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>190.83</Rate>
//<Days>10</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ABF Freight</Carrier_Name>
//<SCAC>ABFS</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>207.41</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>USF REDDAWAY</Carrier_Name>
//<SCAC>RETL</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>217.53</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>OLD DOMINION FREIGHT LINE, INC</Carrier_Name>
//<SCAC>ODFL</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>274.86</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>SHIFT FREIGHT</Carrier_Name>
//<SCAC>SHIF</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>300.69</Rate>
//<Days>7</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>SAIA MOTOR FREIGHT</Carrier_Name>
//<SCAC>SAIA</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>462.22</Rate>
//<Days>5</Days>
//</Carrier>
//</Carriers>
//<Notes>
//BE ADVISED: Please ensure you have accurate dimensions and weights entered for your shipment. Each carrier has different maximum weights and volume limits per shipment. Penalties will be incurred if your shipment is reweighed or measured and found to be larger than the carriers' allowable volume or weight limit. Carriers will only be displayed if your shipment is within their maximum thresholds. If you have any questions please call Exfreight at (877) 208-5645. If you request additional services on the special instructions of your bill of lading which you have neglected to select as an additional service on the quote screen you will be charged additional charges. If you are shipping to a residence or from a residence additional charges will apply as per the residential pick up or delivery fee. Residential deliveries over 50lbs per piece require a lift gate. If you are requesting the carrier to notify consignee on arrival additional fees may be required. If you are shipping to or from a restricted access site, for example: Construction site, Military Base, Exhibition Site, Hospital, School, you will be charged an additional fee. Please check with your sales person for the additional fees which vary per carrier.
//</Notes>
//<Notice>
//Are you sure your freight class is correct? We have calculated your freight class to be Class 85 based on the density. IF YOUR FREIGHT IS RECLASSIFED YOUR RATE WILL CHANGE ACCORDINGLY AND YOU WILL BE BILLED ADDITIONALLY
//</Notice>
//</Rating>";
            XmlDocument doc = new XmlDocument();
             doc.LoadXml(xml);
            XmlNode root = doc.DocumentElement;
            XmlNodeList carriers = root.SelectNodes("Carriers//Carrier");
 
            result.Success = true;
            result.rootNode = root;
            return result;
        }
 

        public string RATING_URL { get; set; }//https://exfreight.service.com/exfreight/cgi/rate_calculator2.cgi
        public const string API = "YES";//Always set to "Yes"
        public string  UserName { get; set; }
        public string  Password { get; set; }

        /// <summary>
        /// Choices are: 
        /// LTL Trucking (example) 
        /// Courier - Domestic 
        /// Full_Truckload 
        /// Ocean_Domestic 
        /// Ocean_International (example) 
        /// Ocean_Import 
        /// Ocean_FCL 
        /// Ocean_FCL_Import (example) 
        /// Air (example) 
        /// Air_Import
        /// </summary>
        public string Shipment_Type { get; set; }

        /// <summary>
        /// Choices are: Door To Door (LTL Trucking and Full_Truckload) Door To Port (Air, Ocean Domestic and Ocean International) Port To Port Port To Door
        /// </summary>
        public string  Rate_Type { get; set; } 

        /// <summary>
        /// Required for LCL Trucking. Valid classes are: 50, 55, 60, 65, 70, 77.5, 85, 92.5, 100, 110, 125, 150, 175, 200, 250, 300, 400, 500
        /// </summary>
        public string Freight_Class { get; set; }

        /// <summary>
        /// If set to 1, we will overide the entered freight class with the calculated freight class
        /// </summary>
        public Nullable<bool> Use_Calculated_Freight_Class { get; set; }

        /// <summary>
        /// Required for Full Container shipments
        /// </summary>
        public string Ocean_FCL_Commodity { get; set; } 

        /// <summary>
        /// 5 digit US postal code or 6 digit CA postal code Required for LCL Trucking, Air, Ocean_Domestic, Ocean_International and Ocean_FCL
        /// </summary>
        public string Pickup_Zipcode { get; set; }

        /// <summary>
        /// 5 digit US postal code or 6 digit CA postal code Required for LCL Trucking, Air_Import, Ocean_Import
        /// </summary>
        public string Delivery_Zipcode { get; set; }

        /// <summary>
        /// Entered as YYYY-MM-DD
        /// </summary>
        public DateTime Ship_Date { get; set; }

        /// <summary>
        /// For LCL Trucking, Air, Ocean_Domestic, Ocean_International and Ocean_FCL either "US" or "CA"
        /// </summary>
        public string Origin_Source_Country { get; set; }

        /// <summary>
        /// examples at :https://exfreight.service.com/exfreight/api/air_destinations.tab
        /// </summary>
        public string Air_Destination_Port { get; set; }

        /// <summary>
        /// examples: https://exfreight.service.com/exfreight/api/ocean_international_destinations.tab
        /// </summary>
        public string Ocean_International_Destination_Port { get; set; }

        /// <summary>
        /// https://exfreight.service.com/exfreight/api/ocean_domestic_destinations.tab
        /// </summary>
        public string Ocean_Domestic_Destination_Port { get; set; }

        /// <summary>
        /// https://exfreight.service.com/exfreight/api/ocean_fcl_destinations.tab
        /// </summary>
        public string Ocean_FCL_Destination_Port { get; set; }

        /// <summary>
        /// https://exfreight.service.com/exfreight/api/air_import_origins.tab
        /// </summary>
        public string Air_Import_Origin_Port { get; set; }

        /// <summary>
        /// https://exfreight.service.com/exfreight/api/ocean_lcl_import_origins.tab
        /// </summary>
        public string Ocean_Import_Origin_Port { get; set; }

        /// <summary>
        /// https://exfreight.service.com/exfreight/api/ocean_fcl_import_origins.tab
        /// </summary>
        public string Ocean_FCL_Import_Origin_Port { get; set; }

       /// <summary>
        /// Set to 1 if needed
       /// </summary>

        public Dictionary<requiredItems,Nullable<int>> RequiredItems { get; set; }
        public List<ExFreight_Carton> Cartons { get; set; }
        public List<EXFreight_Truck> Trucks { get; set; }
        public List<EXFreight_Container> Containers { get; set; }

        /// <summary>
        /// This is the index of the last set of cartons, trucks or container
        /// 
        /// 
        /// </summary>
        public int Last_Row { get; set; }
    }
     
    /// <summary>
    /// Repeat the following for each set of cartons/packages. The index counter "_1" must increase for each set of cartons
    /// </summary>
    public class ExFreight_Carton
    {
        public enum Handling { Loose, Pallet, Skid, Other };
        /// <summary>
        /// this constructor is used for rating
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="measure"></param>
        /// <param name="totalweight"></param>
        /// <param name="weightmeasure"></param>
        public ExFreight_Carton(int qty,double length,double width,double height,string measure,double totalweight,string weightmeasure)
        {
            this.Carton_Quantity_Value=qty;
            this.Carton_Length_Value=length;
            this.Carton_Width_Value=width;
            this.Carton_Height_Value=height;
            this.Carton_Measure_Value=measure;
            this.Carton_Total_Weight_Value=totalweight;
            this.Carton_Weight_Measure_Value=weightmeasure;
            this.UsedForRating = true;
        }

        public ExFreight_Carton(string productdescription,Handling handling)
        {
            this.Carton_Product_Value = productdescription;
            this.Carton_Handling_Value = handling;
            this.UsedForRating = false;
        }


        public bool UsedForRating { get; set; }

        public const string Carton_Product_ = "Carton_Product_"; 
        public string Carton_Product_Value { get; set; }

        /// <summary>
        /// Choices are Loose, Pallet, Skid, Other
        /// </summary>
        public const string Carton_Handling_ = "Carton_Handling_";
        public Handling Carton_Handling_Value { get; set; }

        /// <summary>
        /// Integer value
        /// </summary>
        public const string Carton_Quantity_ = "Carton_Quantity_";
        public int Carton_Quantity_Value { get; set; }

        /// <summary>
        /// Always "My Packaging"
        /// </summary>
        public const string Carton_Package_ = "Carton_Package_";
        public const string Carton_Package_Value = "My Packaging";

        /// <summary>
        /// Decimal Value
        /// </summary>
        public const string Carton_Length_ = "Carton_Length_";
        public double Carton_Length_Value { get; set; }

        /// <summary>
        /// Decimal Value
        /// </summary>
        public const string Carton_Width_ = "Carton_Width_";
        public double Carton_Width_Value { get; set; }

        /// <summary>
        /// Decimal Value
        /// </summary>
        public const string Carton_Height_ = "Carton_Height_";
        public double Carton_Height_Value { get; set; }

        /// <summary>
        /// either "cm" or "in"
        /// </summary>
        public const string Carton_Measure_ = "Carton_Measure_";
        public string Carton_Measure_Value { get; set; }

        /// <summary>
        /// Decimal Value
        /// </summary>
        public const string Carton_Total_Weight_ = "Carton_Total_Weight_";
        public double Carton_Total_Weight_Value { get; set; }

        /// <summary>
        /// Either "kg" or "lb"
        /// </summary>
        public const string Carton_Weight_Measure_ = "Carton_Weight_Measure_";
        public string Carton_Weight_Measure_Value { get; set; }

    }

    public class EXFreight_Truck
    {
        public EXFreight_Truck()
        {
        }

        /// <summary>
        /// Always set to 1
        /// </summary>
        public const string Truck_Quantity_ = "Truck_Quantity_";
        public Nullable<int> Truck_Quantity_Value = null;

        /// <summary>
        /// Always "Truck"
        /// </summary>
        public const string Truck_Type_ = "Truck_Type_";
        public string Truck_Type_Value = null;

        /// <summary>
        /// Always "53ft Van"
        /// </summary>
        public const string Truck_Length_ = "Truck_Length_";
        public string Truck_Length_Value = null;

        /// <summary>
        /// The number of pieces in the truck
        /// </summary>
        public const string Truck_Width_ = "Truck_Width_";
        public Nullable<int> Truck_Width_Value { get; set; }

        /// <summary>
        /// The number of pieces in the truck
        /// </summary>
        public const string Truck_Total_Weight_ = "Truck_Total_Weight_";
        public Nullable<double> Truck_Total_Weight_Value { get; set; }

        /// <summary>
        /// Either "kg" or "lb"
        /// </summary>
        public const string Truck_Weight_Measure_ = "Truck_Weight_Measure_";
        public string Truck_Weight_Measure_Value { get; set; }

    }

    public class EXFreight_Container
    {
        public EXFreight_Container()
        {
        }

        public const string Container_Size_20 = "20";
        public const string Container_Size_40 = "40";
        public const string Container_Size_40_HC = "40 HC";

        /// <summary>
        /// Valid choices are "20", "40" and "40 HC"
        /// </summary>
        public const string Container_Size_ = "Container_Size_";
        public string Container_Size_Value { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public const string Container_Total_Weight_ = "Container_Total_Weight_";
        public double Container_Total_Weight_Value { get; set; }

        /// <summary>
        ///  Either "kg" or "lb"
        /// </summary>
        public const string Container_Weight_Measure_ = "Container_Weight_Measure_";
        public string Container_Weight_Measure_Value { get; set; }

    }
     
    public class EXFreight_Result 
    {
        public bool Success { get; set; }
        public string message { get; set; }
        public string resultXML { get; set; }
        public XmlNode rootNode { get; set; }
    }

    public static class ExpandoObjectHelper
    {
        private static List<string> KnownLists;
        public static void Parse(dynamic parent, XElement node, List<string> knownLists = null)
    {
        if (knownLists != null)
        {
            KnownLists = knownLists;
        }
        IEnumerable<XElement> sorted = from XElement elt in node.Elements() 
              orderby node.Elements(elt.Name.LocalName).Count() descending select elt;

        if (node.HasElements)
        {
            int nodeCount = node.Elements(sorted.First().Name.LocalName).Count();
            bool foundNode = false;
            if (KnownLists != null && KnownLists.Count > 0)
            {
                foundNode = (from XElement el in node.Elements() 
                  where KnownLists.Contains(el.Name.LocalName) select el).Count() > 0;
            }

            if (nodeCount>1 || foundNode==true)
            {
                // At least one of the child elements is a list
                var item = new ExpandoObject();
                List<dynamic> list = null;
                string elementName = string.Empty;
                foreach (var element in sorted)
                {
                    if (element.Name.LocalName != elementName)
                    {
                        list = new List<dynamic>();
                         //elementName = elementName.name.LocalName;
                        elementName = elementName;
                    }
                    if (element.HasElements ||
                        (KnownLists != null && KnownLists.Contains(element.Name.LocalName)))
                    {
                        Parse(list, element);
                        AddProperty(item, element.Name.LocalName, list);
                    }
                    else
                    {
                        Parse(item, element);
                    }
                }

                foreach (var attribute in node.Attributes())
                {
                    AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                }

                AddProperty(parent, node.Name.ToString(), item);
            }
            else
            {
                var item = new ExpandoObject();

                foreach (var attribute in node.Attributes())
                {
                    AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                }

                //element
                foreach (var element in sorted)
                {
                    Parse(item, element);
                }
                AddProperty(parent, node.Name.ToString(), item);
            }
        }
        else
        {
            AddProperty(parent, node.Name.ToString(), node.Value.Trim());
        }
    }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else
            {
                (parent as IDictionary<string, object>)[name] = value;
            }
        }
    }
    public class Carriers
    {
    }
}




// string xml = @"<Rating>
//<Rate_Estimate_For>Next Generation Logistics, Inc - LTL Trucking</Rate_Estimate_For>
//<Reference>1419363163015611 / 2014-12-23</Reference>
//<From>94306 Palo Alto, CA</From>
//<To>08820 Edison, NJ</To>
//<Freight_Class>100</Freight_Class>
//<Weight>12.00 lbs / 6.00 kgs (1.00)</Weight>
//<Carriers>
//<Carrier>
//<Carrier_Name>TOWNE AIR FREIGHT, INC.</Carrier_Name>
//<SCAC>TOWE</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>107.09</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>FORWARD AIR, INC</Carrier_Name>
//<SCAC>FWDN</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>112.87</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>FRONTLINE FREIGHT</Carrier_Name>
//<SCAC>FCSY</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>136.89</Rate>
//<Days>7</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>YRC</Carrier_Name>
//<SCAC>RDWY</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>147.23</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ROADRUNNER TRANSPORTATION SERVICES</Carrier_Name>
//<SCAC>RDFS</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>163.80</Rate>
//<Days>6</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>CLEAR LANE FREIGHT SYSTEMS</Carrier_Name>
//<SCAC>7CLN</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>187.06</Rate>
//<Days>6</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ESTES EXPRESS LINES</Carrier_Name>
//<SCAC>EXLA</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>190.83</Rate>
//<Days>10</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>ABF Freight</Carrier_Name>
//<SCAC>ABFS</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>207.41</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>USF REDDAWAY</Carrier_Name>
//<SCAC>RETL</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>217.53</Rate>
//<Days>4</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>OLD DOMINION FREIGHT LINE, INC</Carrier_Name>
//<SCAC>ODFL</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>274.86</Rate>
//<Days>5</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>SHIFT FREIGHT</Carrier_Name>
//<SCAC>SHIF</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>300.69</Rate>
//<Days>7</Days>
//</Carrier>
//<Carrier>
//<Carrier_Name>SAIA MOTOR FREIGHT</Carrier_Name>
//<SCAC>SAIA</SCAC>
//<TSA/>
//<Currency>USD</Currency>
//<Rate>462.22</Rate>
//<Days>5</Days>
//</Carrier>
//</Carriers>
//<Notes>
//BE ADVISED: Please ensure you have accurate dimensions and weights entered for your shipment. Each carrier has different maximum weights and volume limits per shipment. Penalties will be incurred if your shipment is reweighed or measured and found to be larger than the carriers' allowable volume or weight limit. Carriers will only be displayed if your shipment is within their maximum thresholds. If you have any questions please call Exfreight at (877) 208-5645. If you request additional services on the special instructions of your bill of lading which you have neglected to select as an additional service on the quote screen you will be charged additional charges. If you are shipping to a residence or from a residence additional charges will apply as per the residential pick up or delivery fee. Residential deliveries over 50lbs per piece require a lift gate. If you are requesting the carrier to notify consignee on arrival additional fees may be required. If you are shipping to or from a restricted access site, for example: Construction site, Military Base, Exhibition Site, Hospital, School, you will be charged an additional fee. Please check with your sales person for the additional fees which vary per carrier.
//</Notes>
//<Notice>
//Are you sure your freight class is correct? We have calculated your freight class to be Class 85 based on the density. IF YOUR FREIGHT IS RECLASSIFED YOUR RATE WILL CHANGE ACCORDINGLY AND YOU WILL BE BILLED ADDITIONALLY
//</Notice>
//</Rating>";