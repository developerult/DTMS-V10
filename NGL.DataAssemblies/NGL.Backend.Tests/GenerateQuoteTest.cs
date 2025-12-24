using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ngl.FreightMaster.Data;
using Ngl.FreightMaster.Data.DataTransferObjects;
using NGL.FM.BLL;
using System;
using System.Collections.Generic;
using Ngl.FreightMaster.Data.Models;
using Ngl.FreightMaster.Core.Model;
using Newtonsoft.Json;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Serilog.Enrichers.Span;
using Serilog.Enrichers.WithCaller;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using SerilogTracing;

namespace NGL.Backend.Tests
{
    [TestClass]
    public class GenerateQuoteTest
    {
        public GenerateQuoteTest()
        {
            _logger = new Serilog.LoggerConfiguration()
                .WriteTo
                    .Seq("https://ngl-monitoring-tools.salmonwater-864728d4.westus2.azurecontainerapps.io/")
                .Enrich
                    .WithProperty("baseUrl", "UnitTest")
                .Enrich
                    .WithSpan()
                .Enrich
                    .WithThreadId()
                .Enrich
                    .WithCaller()
                .Enrich
                    .WithDemystifiedStackTraces() 
                
                .Enrich.WithCaller(maxDepth:5)
                .WriteTo.Debug()
                .Destructure.ToMaximumDepth(3)
                .Destructure.ToMaximumCollectionCount(10)
                .CreateLogger();

            Serilog.Log.Logger = _logger;
            using (_ = new SerilogTracing.ActivityListenerConfiguration()
                .InitialLevel.Information()
                .ActivityEvents.AsLogEvents()
                .Instrument.SqlClientCommands().TraceToSharedLogger()) ;
            

            
            
     }


        private Serilog.ILogger _logger = null;

        #region Plumbing

        private string GetLocalDBTestConnection()
        {
            //Hack connection string. Connect to MS SQL hosted in docker.
            string s = "Server=localhost;Database=NGLMASPROD;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;";
            return s;
        }

        private RateRequestOrder CreateBaselineOrder()
        {
            var order = new Ngl.FreightMaster.Data.Models.RateRequestOrder
            {

                //RRControl = 1, // Using LoadTenderControl from JSON
                //RRUserSecurityControl = 0, // Not provided in JSON, using default
                //RRBookSHID = "C377465", // Using customerCode from JSON
                BookConsPrefix = "Prefix", // Using placeholder value
                ShipDate = "2024-11-19", // Using sShipDate from JSON (date part only)
                DeliveryDate = "2024-11-20", // Using placeholder value
                BookCustCompControl = 54321, // Using placeholder value
                CompName = "CompanyName", // Using placeholder value
                CompNumber = 98765, // Using placeholder value
                CompAlphaCode = "COMP1", // Using placeholder value
                BookCarrierControl = 67890, // Using placeholder value
                CarrierName = "CarrierName", // Using placeholder value
                CarrierNumber = 24, // Using placeholder value
                CarrierAlphaCode = "CARR1", // Using placeholder value
                TotalCases = 426, // Using totalCases from JSON
                TotalWgt = 5976, // Using totalWeight from JSON
                TotalPL = 6, // Using totalPL from JSON
                TotalCube = 1267, // Using totalCube from JSON (rounded to integer)
                TotalStops = 2, // Origin + 1 stop from JSON
                Pickup = new Ngl.FreightMaster.Data.Models.RateRequestStop
                {
                    StopNumber = 0,
                    CompCity = "ATLANTA", // City from JSON (origin)
                    CompState = "GA", // State from JSON (origin)
                    CompCountry = "USA", // Country from JSON (origin)
                    CompPostalCode = "30349", // Zip from JSON (origin)
                    IsPickup = true,
                    TotalCases = 426, // totalCases from JSON
                    TotalWgt = 5976, // totalWeight from JSON
                    TotalPL = 6, // totalPL from JSON
                    TotalCube = 1267, // totalCube from JSON
                    LoadDate = "2024-11-19", // ShipDate from JSON
                    RequiredDate = "2024-11-19" // Same as LoadDate for pickup
                },
                Stops = new Ngl.FreightMaster.Data.Models.RateRequestStop[]
                {
                    new Ngl.FreightMaster.Data.Models.RateRequestStop
                    {
                        StopNumber = 1,
                        CompCity = "HOUSTON",
                        CompState = "TX",
                        CompCountry = "USA",
                        CompPostalCode = "77004",
                        IsPickup = false,
                        TotalCases = 426,
                        TotalWgt = 5976,
                        TotalPL = 6,
                        TotalCube = 1267,
                        LoadDate = "2024-11-20",
                        RequiredDate = "2024-11-20"
                    }
                },
                //ModDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                CommCodeType = "D53", // Using eMode from JSON
                CommCodeDescription = "Description", // Using placeholder value
                Accessorials = new string[] { "ACH" }, // Using oAccessorials from JSON
                AccessorialValues = new string[] { "Value1" }, // Using placeholder value
                ShipKey = "ShipKey123", // Using placeholder value
                ID = 1, // Using placeholder value
                TariffTempType = 1, // Using placeholder value
                BookTransType = 1, // Using placeholder value
                Inbound = true // Using placeholder value
            };

            return order;
        }

        private WCFParameters GetBaselineWCFParameters()
        {
            return new WCFParameters
            {
                WCFAuthCode = "NGLSystem",
                DBServer = "localhost",
                Database = "NGLMASPROD",
                UserName = "ngl\rramsey",
                
                UserLEControl = 1,  //needed to match tblSSOALEConfig.SSOALELEAdminControl, else no carrier APIs returned. Expects the 5 rows.
                ConnectionString = GetLocalDBTestConnection()
            };
        }

        private OrderInput GetBaselineOrderInput()
        {
            var input = new OrderInput();

            input.order = CreateBaselineOrder();

            input.tenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.LoadBoard };

            input.bidTypes = new List<tblLoadTender.BidTypeEnum>
            {
                
                tblLoadTender.BidTypeEnum.NGLTariff
            }.ToArray();

            input.bookControl = 0;

            input.tariffOptions = new GetCarriersByCostParameters(true, true, true, 0, 0);

            return input;
        }

        #endregion

        [TestMethod, Ignore]
        public void GenerateQuote_RateShopping_1()
        {
            //Arrange
            WCFParameters oParameters = GetBaselineWCFParameters();
            OrderInput input = GetBaselineOrderInput();

            #region Order

            string jsonString = """
            {
              "ID": 0,
              "ShipKey": "(Auto)",
              "BookConsPrefix": "",
              "ShipDate": "2024-11-16T06:00:00.000Z",
              "DeliveryDate": "2024-11-19T06:00:00.000Z",
              "BookCustCompControl": 0,
              "CompName": "",
              "CompNumber": 0,
              "CompAlphaCode": "",
              "BookCarrierControl": 0,
              "CarrierName": "",
              "CarrierNumber": 0,
              "CarrierAlphaCode": "",
              "TotalCases": 12,
              "TotalWgt": 15000,
              "TotalPL": 12,
              "TotalCube": 0,
              "TotalStops": 0,
              "Pickup": {
                "ID": 0,
                "ParentID": 0,
                "BookControl": 0,
                "BookProNumber": null,
                "StopIndex": 1,
                "BookCarrOrderNumber": null,
                "CompControl": 0,
                "CompName": "",
                "CompAddress1": "",
                "CompAddress2": "",
                "CompAddress3": "",
                "CompCity": "LA GRANGE",
                "CompState": "IL",
                "CompCountry": "US",
                "CompPostalCode": "60525",
                "IsPickup": true,
                "StopNumber": 0,
                "TotalCases": 12,
                "TotalWgt": 15000,
                "TotalPL": 12,
                "TotalCube": 0,
                "LoadDate": "2024-11-16T06:00:00.000Z",
                "SHID": null,
                "RequiredDate": "2024-11-19T06:00:00.000Z",
                "Items": null
              },
              "Stops": [
                {
                  "ID": 0,
                  "ParentID": 0,
                  "BookControl": 0,
                  "BookProNumber": null,
                  "StopIndex": 1,
                  "BookCarrOrderNumber": null,
                  "CompControl": 0,
                  "CompName": "",
                  "CompAddress1": "",
                  "CompAddress2": "",
                  "CompAddress3": "",
                  "CompCity": "BOISE",
                  "CompState": "ID",
                  "CompCountry": "US",
                  "CompPostalCode": "83707",
                  "IsPickup": false,
                  "StopNumber": 1,
                  "TotalCases": 12,
                  "TotalWgt": 15000,
                  "TotalPL": 12,
                  "TotalCube": 0,
                  "LoadDate": "2024-11-16T06:00:00.000Z",
                  "SHID": "(Auto)",
                  "RequiredDate": "2024-11-19T06:00:00.000Z",
                  "Items": [
                    {
                      "ID": 10276,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-1",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Desc",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 48,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": false
                    },
                    {
                      "ID": 10277,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-2",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 2",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10278,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-3",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 3",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10279,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-4",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 4",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10280,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-5",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 5",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10281,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-6",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 6",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10282,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-7",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 7",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10283,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-8",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 8",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10284,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-9",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 9",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10285,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-10",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 10",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10286,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-11",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 11",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    },
                    {
                      "ID": 10287,
                      "ParentID": 6562,
                      "LoadID": 6750,
                      "ItemStopIndex": 1,
                      "ItemControl": 0,
                      "ItemNumber": "QI-12",
                      "Weight": 1250,
                      "WeightUnit": null,
                      "FreightClass": "65",
                      "PalletCount": 1,
                      "NumPieces": 1,
                      "Description": "Item 12",
                      "Quantity": "1",
                      "HazmatId": null,
                      "Code": null,
                      "HazmatClass": null,
                      "IsHazmat": false,
                      "Pieces": null,
                      "PackageType": "PLT",
                      "Length": 48,
                      "Width": 40,
                      "Height": 66,
                      "Density": "0",
                      "NMFCItem": "",
                      "NMFCSub": "",
                      "Stackable": true
                    }
                  ]
                }
              ],
              "AccessorialValues": null,
              "Accessorials": [
                "TARP"
              ],
              "CommCodeType": null,
              "Inbound": false,
              "BookTransType": 0,
              "CommCodeDescription": "Dry",
              "TariffTempType": 1
            }
            """;

            #endregion

            input.order = JsonConvert.DeserializeObject<RateRequestOrder>(jsonString);
            input.order.ShipDate = AddBusinessDaysAsISOString(2);
            input.order.DeliveryDate = AddBusinessDaysAsISOString(5);
            input.order.Accessorials = new string[] { "TARP" };

            input.bookControl = 0;
            input.tariffOptions = null;
            input.tenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.RateShopping };
            input.bidTypes = new[] { tblLoadTender.BidTypeEnum.NGLTariff, tblLoadTender.BidTypeEnum.P44 };

            // Act
            var oRet = RunGenerateQuote(oParameters, input);

            int iLoadTenderControl = Convert.ToInt32(oRet.KeyFields["LoadTenderControl"]);

            //TODO -> get grid (from tblBid)
            //TODO -> get subgrid (adjustments)
            /*
                --Main grid
                select top 20 BidControl, * from tblBid
                where BidLoadTenderControl = 9082 --{iLoadTenderControl}
                order by BidModDate desc;

                --Adjustments per row
                select * from tblBidCostAdj
                where BidCostAdjBidControl = 46984
                order by BidCostAdjModDate desc; 
            */

            // Assert
            //Assert.IsNotNull(oRet);
            //Assert.IsTrue(oRet.Success);
        }

        [TestMethod, Ignore]
        public void GenerateQuote_RateShopping_FSC_1()
        {
            //Arrange
            WCFParameters oParameters = GetBaselineWCFParameters();
            OrderInput input = GetBaselineOrderInput();

            #region Order

            string jsonString = """
            {
                "$type": "RateRequestOrder",
                "AccessorialValues": null,
                "Accessorials": [
                    "LGSER"
                ],
                "BookCarrierControl": 0,
                "BookConsPrefix": "",
                "BookCustCompControl": 0,
                "BookTransType": 0,
                "CarrierAlphaCode": "",
                "CarrierName": "",
                "CarrierNumber": 0,
                "CommCodeDescription": "Frozen",
                "CommCodeType": null,
                "CompAlphaCode": "",
                "CompName": "FROZEN ASSETS COLD STORAGE",
                "CompNumber": 0,
                "DeliveryDate": "2024-11-29T06:00:00.000Z",
                "ID": 0,
                "Inbound": false,
                "Pickup": {
                    "$type": "RateRequestStop",
                    "BookCarrOrderNumber": null,
                    "BookControl": 0,
                    "BookProNumber": null,
                    "CompAddress1": "6800 SANTA FE DR., Suite F",
                    "CompAddress2": "",
                    "CompAddress3": "",
                    "CompCity": "HODGKINS",
                    "CompControl": 0,
                    "CompCountry": "US",
                    "CompName": "FROZEN ASSETS COLD STORAGE",
                    "CompPostalCode": "60525",
                    "CompState": "IL",
                    "ID": 0,
                    "IsPickup": true,
                    "Items": null,
                    "LoadDate": "2024-11-25T06:00:00.000Z",
                    "ParentID": 0,
                    "RequiredDate": "2024-11-29T06:00:00.000Z",
                    "SHID": null,
                    "StopIndex": 1,
                    "StopNumber": 0,
                    "TotalCases": 1,
                    "TotalCube": 0,
                    "TotalPL": 1,
                    "TotalWgt": 1200
                },
                "ShipDate": "2024-11-25T06:00:00.000Z",
                "ShipKey": "(Auto)",
                "Stops": [
                    {
                        "$type": "RateRequestStop",
                        "BookCarrOrderNumber": null,
                        "BookControl": 0,
                        "BookProNumber": null,
                        "CompAddress1": "3781 EAST AIRPORT DRIVE",
                        "CompAddress2": "",
                        "CompAddress3": "",
                        "CompCity": "BOISE",
                        "CompControl": 0,
                        "CompCountry": "US",
                        "CompName": "ONTARIO DC",
                        "CompPostalCode": "83707",
                        "CompState": "ID",
                        "ID": 0,
                        "IsPickup": false,
                        "Items": [
                            {
                                "$type": "RateRequestItem",
                                "Code": null,
                                "Density": null,
                                "Description": "Item 1",
                                "FreightClass": "60",
                                "HazmatClass": null,
                                "HazmatId": null,
                                "Height": 66,
                                "ID": 0,
                                "IsHazmat": false,
                                "ItemControl": 0,
                                "ItemNumber": "1",
                                "ItemStopIndex": 0,
                                "Length": 48,
                                "LoadID": 0,
                                "NMFCItem": "",
                                "NMFCSub": "",
                                "NumPieces": 1,
                                "PackageType": "PLT",
                                "PalletCount": 1,
                                "ParentID": 0,
                                "Pieces": null,
                                "Quantity": "1",
                                "Stackable": true,
                                "Weight": 1200,
                                "WeightUnit": null,
                                "Width": 40
                            }
                        ],
                        "LoadDate": "2024-11-25T06:00:00.000Z",
                        "ParentID": 0,
                        "RequiredDate": "2024-11-29T06:00:00.000Z",
                        "SHID": "(Auto)",
                        "StopIndex": 1,
                        "StopNumber": 1,
                        "TotalCases": 1,
                        "TotalCube": 0,
                        "TotalPL": 1,
                        "TotalWgt": 1200
                    }
                ],
                "TariffTempType": 2,
                "TotalCases": 1,
                "TotalCube": 0,
                "TotalPL": 1,
                "TotalStops": 0,
                "TotalWgt": 1200
            }           
            """;

            #endregion

            input.order = JsonConvert.DeserializeObject<RateRequestOrder>(jsonString);
            input.order.ShipDate = "2024-12-02T06:00:00.000Z"; //AddBusinessDaysAsISOString(3);
            input.order.DeliveryDate = "2024-12-06T06:00:00.000Z"; // AddBusinessDaysAsISOString(6);
            input.order.Accessorials = new string[] { };
            //input.order.Accessorials = new string[] { "FSC", };
            //input.order.AccessorialValues = new string[] { "2" }; //drives vbery different code path!!!

            input.bookControl = 0;
            input.tariffOptions = null;
            input.tenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.RateShopping };
            input.bidTypes = new[] { tblLoadTender.BidTypeEnum.NGLTariff };

            // Act
            var oRet = RunGenerateQuote(oParameters, input);

            int iLoadTenderControl = Convert.ToInt32(oRet.KeyFields["LoadTenderControl"]);
        }

        [TestMethod, Ignore]
        public void GenerateQuote_LoadBoard_FSC_1()
        {
            //Arrange
            WCFParameters oParameters = GetBaselineWCFParameters();
            OrderInput input = GetBaselineOrderInput();


            input.order = null;
            //input.order.ShipDate = AddBusinessDaysAsISOString(3);
            //input.order.DeliveryDate = AddBusinessDaysAsISOString(6);
            //input.order.Accessorials = new string[] { "INDEL", "FSC", "FUE" };

            input.bookControl = 469;
            input.tariffOptions = null;
            input.tenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.LoadBoard };
            input.bidTypes = new[] { tblLoadTender.BidTypeEnum.NGLTariff };

            // Act
            _logger.Information("Run test GenerateQuote_LoadBoard_FSC_1");
            var oRet = RunGenerateQuote(oParameters, input);

            int iLoadTenderControl = Convert.ToInt32(oRet.KeyFields["LoadTenderControl"]);
        }

        [TestMethod]
        public void GenerateQuote_LoadBoard_1()
        {
            //Arrange
            WCFParameters oParameters = GetBaselineWCFParameters();
            OrderInput input = GetBaselineOrderInput();

            input.order = null; //Loadboard has null!
            //input.order.ShipDate = AddBusinessDaysAsISOString(2);
            //input.order.DeliveryDate = AddBusinessDaysAsISOString(5);
            //input.order.Accessorials = new string[] {}; //clear it
            
            input.bookControl = 469;    //From AppInsights
            input.tariffOptions = new GetCarriersByCostParameters(false, true, true, 3, 2); // null;
            input.tariffOptions.AllowAsync = true;
         //   input.order = new RateRequestOrder();
          //  input.order.AccessorialValues = new[] { "2" };
           
            input.tenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.LoadBoard };   //8
            input.bidTypes = new[] {tblLoadTender.BidTypeEnum.NGLTariff }; //3 One or more of the Lane profile specific fees assoc
            
            // Act
            var oRet = RunGenerateQuote(oParameters, input);
            _logger.Information("RunGenerateQuoteTestResults: {@oRet}", oRet);
            int iLoadTenderControl = Convert.ToInt32(oRet.KeyFields["LoadTenderControl"]);

            //TODO -> get grid (from tblBid)
            //TODO -> get subgrid (adjustments)
            /*
                --Main grid
                select top 20 BidControl, * from tblBid
                where BidLoadTenderControl = 9082 --{iLoadTenderControl}
                order by BidModDate desc;

                --Adjustments per row
                select * from tblBidCostAdj
                where BidCostAdjBidControl = 46984
                order by BidCostAdjModDate desc; 
            */

            // Assert
            //Assert.IsNotNull(oRet);
            //Assert.IsTrue(oRet.Success);
        }

        [TestMethod, Ignore]
        public void TestGenerateQuote()
        {
            // Arrange
            var oParameters = new WCFParameters
            {
                WCFAuthCode = "NGLSystem",
                DBServer = "localhost",
                Database = "NGLMASPROD",
                UserName = "sa",
                UserLEControl = 1,
                ConnectionString = GetLocalDBTestConnection()
            };

            var BookRevenueBLL = new NGLBookRevenueBLL(oParameters);
            var order = CreateBaselineOrder();

            tblLoadTender.LoadTenderTypeEnum[] TenderTypes = new[] { tblLoadTender.LoadTenderTypeEnum.LoadBoard };
            var bidTypes = new List<tblLoadTender.BidTypeEnum>
            {
                tblLoadTender.BidTypeEnum.CHRAPI,
                tblLoadTender.BidTypeEnum.FFEAPI,
                tblLoadTender.BidTypeEnum.GTZAPI,
                tblLoadTender.BidTypeEnum.HMBayAPI,
                //tblLoadTender.BidTypeEnum.EstesAPI, //Estes does not exist
                tblLoadTender.BidTypeEnum.NGLTariff
            };
            int BookControl = 0;
            var tariffOptions = new GetCarriersByCostParameters(true, true, true, 0, 0);
            _logger.Information("Order: {order}\nParameters: {oParameters}", order, oParameters);
            // Act
            var oRet = BookRevenueBLL.GenerateQuote(order, TenderTypes, bidTypes.ToArray(), BookControl, tariffOptions);

            // Assert
            //Assert.IsNotNull(oRet);
            //Assert.IsTrue(oRet.Success);
        }


        private WCFResults RunGenerateQuote(WCFParameters oParameters, OrderInput input)
        {
            var BookRevenueBLL = new NGLBookRevenueBLL(oParameters);
            var oRet = BookRevenueBLL.GenerateQuote(input.order, input.tenderTypes, input.bidTypes, input.bookControl, input.tariffOptions);
            return oRet;
        }

        [TestMethod, Ignore]
        public void getCarrierTariffsPivotWithPrecision_1()
        {
            var bookRevenue = new NGL.FM.CarTar.BookRev(GetBaselineWCFParameters());
            int bookControl = 123; // Example book control number

            // Act
            var results = bookRevenue.estimatedCarriersByCost(
                bookControl,
                carrierControl: 0,
                prefered: true,
                noLateDelivery: false,
                validated: false,
                optimizeByCapacity: true,
                modeTypeControl: 0,
                tempType: 0,
                tariffTypeControl: 0,
                carrTarEquipMatClass: null,
                carrTarEquipMatClassTypeControl: 0,
                carrTarEquipMatTarRateTypeControl: 0,
                agentControl: 0,
                page: 1,
                pagesize: 1000
            );

        }
        [TestMethod]
        public void GetCarrierAccessorialsTest()
        {

            NGLLECarrierAccessorialData accessorialData = new NGLLECarrierAccessorialData(GetBaselineWCFParameters());
           var results = accessorialData.GetLECarrierAccessorialsByCarrierControl(196);

            foreach (var item in results)
            {
                Console.WriteLine("AccessorialControl: {1}, AccessorialName: {2}",  item.LECAAccessorialCode, item.LECACaption);
            }

            Assert.IsNotNull(results);
            Assert.IsTrue(results?.Length > 0);
        }
        private static string AddBusinessDaysAsISOString(int daysToAdd)
        {
            if (daysToAdd < 0)
            {
                throw new ArgumentException("Days to add must be a non-negative value.", nameof(daysToAdd));
            }

            DateTime currentDate = DateTime.Today;

            while (daysToAdd > 0)
            {
                currentDate = currentDate.AddDays(1);

                // Check if the new date is a weekday
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysToAdd--;
                }
            }

            return currentDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
    }

    public class OrderInput
    {
        public RateRequestOrder order { get; set; }

        public tblLoadTender.LoadTenderTypeEnum[] tenderTypes { get; set; }
        public tblLoadTender.BidTypeEnum[] bidTypes { get; set; }

        public int bookControl { get; set; }

        public GetCarriersByCostParameters tariffOptions { get; set; }
    }
}