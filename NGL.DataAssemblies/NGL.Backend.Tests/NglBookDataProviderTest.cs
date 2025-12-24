using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ngl.FreightMaster.Data;
using Ngl.FreightMaster.Data;
using Ngl.FreightMaster.Data.DataTransferObjects;
using NGL.Backend.Tests.Wrappers;
using NGL.Test.Core;

namespace NGL.Backend.Tests
{
    [TestClass]
    public class NglBookDataProviderTest
    {
        #region Plumbing

        private WCFParameters GetBaselineWCFParameters()
        {
            return new WCFParameters
            {
                WCFAuthCode = "NGLSystem",
                DBServer = "localhost",
                Database = "NGLMASPROD",
                UserName = "testUser",
                UserLEControl = 1,  //needed to match tblSSOALEConfig.SSOALELEAdminControl, else no carrier APIs returned. Expects the 5 rows.
                ConnectionString = GetLocalDBTestConnection()
            };
        }

        private string GetLocalDBTestConnection()
        {
            //Hack connection string. Connect to MS SQL hosted in docker.
            string s = "FAIL_ONLY_A_UNIT_TEST"; // "Server=localhost;Database=NGLMASPROD;User Id=sa;Password=pwFhf3idfjsd;TrustServerCertificate=True;";
            return s;
        }

        #endregion

        [TestMethod]
        public void calculateFuelCharge_1()
        {
            WCFParameters oParameters = GetBaselineWCFParameters();
            NGLBookDataWrapper w = new NGLBookDataWrapper(oParameters);

            #region create

            string json = """
                [
                  {
                    "BookControl": 0,
                    "BookBookRevHistRevision": 0,
                    "BookRevBilledBFC": 0,
                    "BookRevCarrierCost": 0,
                    "BookRevStopQty": 0,
                    "BookRevStopCost": 0,
                    "BookRevOtherCost": 0,
                    "BookRevTotalCost": 0,
                    "BookRevLoadSavings": 0,
                    "BookRevCommPercent": 0,
                    "BookRevCommCost": 0,
                    "BookRevGrossRevenue": 0,
                    "BookRevNegRevenue": 0,
                    "BookRevFreightTax": 0,
                    "BookRevNetCost": 0,
                    "BookRevNonTaxable": 0,
                    "BookFinARBookFrt": 0,
                    "BookFinAPPayAmt": 0,
                    "BookFinAPStdCost": 0,
                    "BookFinAPActCost": 0,
                    "BookFinCommStd": 0,
                    "BookFinServiceFee": 0,
                    "BookTranCode": "",
                    "BookPayCode": "",
                    "BookTypeCode": "",
                    "BookStopNo": 1,
                    "BookConsPrefix": "(Auto)",
                    "BookCustCompControl": 0,
                    "BookODControl": 0,
                    "BookCarrierControl": 0,
                    "BookCarrierContControl": null,
                    "BookCarrierContact": "",
                    "BookCarrierContactPhone": "",
                    "BookOrigCompControl": 445,
                    "BookOrigName": "FROZEN ASSETS COLD STORAGE",
                    "BookOrigAddress1": "6800 SANTA FE DR., Suite F",
                    "BookOrigAddress2": "",
                    "BookOrigAddress3": "",
                    "BookOrigCity": "HODGKINS",
                    "BookOrigState": "IL",
                    "BookOrigCountry": "US",
                    "BookOrigZip": "60525",
                    "BookDestCompControl": 445,
                    "BookDestName": "ONTARIO DC",
                    "BookDestAddress1": "3781 EAST AIRPORT DRIVE",
                    "BookDestAddress2": "",
                    "BookDestAddress3": "",
                    "BookDestCity": "BOISE",
                    "BookDestState": "ID",
                    "BookDestCountry": "US",
                    "BookDestZip": "83707",
                    "BookDateLoad": "2024-11-26T18:00:00-06:00",
                    "BookDateRequired": "2024-12-01T18:00:00-06:00",
                    "BookTotalCases": 1,
                    "BookTotalWgt": 1200,
                    "BookTotalPL": 1,
                    "BookTotalCube": 126720,
                    "BookTotalPX": 1,
                    "BookTotalBFC": 0,
                    "BookRouteFinalDate": null,
                    "BookRouteFinalCode": "",
                    "BookRouteFinalFlag": false,
                    "BookRouteConsFlag": false,
                    "BookComCode": "",
                    "BookCarrOrderNumber": "",
                    "BookOrderSequence": 0,
                    "BookLockAllCosts": false,
                    "BookLockBFCCost": false,
                    "BookShipCarrierProNumber": "",
                    "BookShipCarrierProNumberRaw": "",
                    "BookShipCarrierProControl": null,
                    "BookShipCarrierName": "",
                    "BookShipCarrierNumber": "",
                    "BookShipCarrierDetails": "",
                    "BookRouteTypeCode": 6,
                    "BookDefaultRouteSequence": 0,
                    "BookRouteGuideControl": 0,
                    "BookCarrTruckControl": 0,
                    "BookCarrTarControl": 0,
                    "BookCarrTarRevisionNumber": 0,
                    "BookCarrTarName": "",
                    "BookCarrTarEquipControl": 0,
                    "BookCarrTarEquipName": "",
                    "BookCarrTarEquipMatControl": 0,
                    "BookCarrTarEquipMatName": "",
                    "BookCarrTarEquipMatDetControl": 0,
                    "BookCarrTarEquipMatDetID": 0,
                    "BookCarrTarEquipMatDetValue": null,
                    "BookModeTypeControl": 0,
                    "BookAllowInterlinePoints": true,
                    "BookMilesFrom": 0,
                    "BookTransType": "",
                    "BookRevLaneBenchMiles": null,
                    "BookRevLoadMiles": null,
                    "BookPickupStopNumber": 1,
                    "BookRevDiscount": 0,
                    "BookRevLineHaul": 0,
                    "BookSHID": "(Auto)",
                    "BookModDate": null,
                    "BookModUser": "",
                    "BookUpdated": null,
                    "BookProNumber": "",
                    "CompanyName": "",
                    "CompanyNumber": "",
                    "CompFinUseImportFrtCost": false,
                    "BookLoads": [
                      {
                        "BookLoadControl": 0,
                        "BookLoadBookControl": 0,
                        "BookLoadBuy": "",
                        "BookLoadPONumber": "RateShop",
                        "BookLoadVendor": "",
                        "BookLoadCaseQty": 1,
                        "BookLoadWgt": 1200,
                        "BookLoadCube": 126720,
                        "BookLoadPL": 1,
                        "BookLoadPX": 0,
                        "BookLoadPType": "",
                        "BookLoadCom": "F",
                        "BookLoadPUOrigin": "",
                        "BookLoadBFC": 0,
                        "BookLoadTotCost": 0,
                        "BookLoadActCost": 0,
                        "BookLoadComments": "",
                        "BookLoadStopSeq": 0,
                        "BookLoadModDate": null,
                        "BookLoadModUser": "",
                        "BookLoadUpdated": null,
                        "BookItems": [
                          {
                            "BookItemControl": 0,
                            "BookItemBookLoadControl": 0,
                            "BookItemFixOffInvAllow": 0,
                            "BookItemFixFrtAllow": 0,
                            "BookItemItemNumber": "1",
                            "BookItemQtyOrdered": 1,
                            "BookItemFreightCost": 0,
                            "BookItemActFreightCost": 0,
                            "BookItemItemCost": 0,
                            "BookItemWeight": 1200,
                            "BookItemCube": 126720,
                            "BookItemPack": 0,
                            "BookItemSize": "",
                            "BookItemDescription": "",
                            "BookItemHazmat": "",
                            "BookItemModDate": null,
                            "BookItemModUser": "",
                            "BookItemBrand": "",
                            "BookItemCostCenter": "",
                            "BookItemLotNumber": "",
                            "BookItemLotExpirationDate": null,
                            "BookItemGTIN": "",
                            "BookCustItemNumber": "",
                            "BookItemBFC": 0,
                            "BookItemCountryOfOrigin": "",
                            "BookItemHST": "",
                            "BookItemPalletTypeID": 0,
                            "BookItemHazmatTypeCode": "",
                            "BookItem49CFRCode": "",
                            "BookItemIATACode": "",
                            "BookItemDOTCode": "",
                            "BookItemMarineCode": "",
                            "BookItemNMFCClass": "",
                            "BookItemFAKClass": "60",
                            "BookItemLimitedQtyFlag": false,
                            "BookItemPallets": 1,
                            "BookItemTies": 0,
                            "BookItemHighs": 0,
                            "BookItemQtyPalletPercentage": 0,
                            "BookItemQtyLength": 48,
                            "BookItemQtyWidth": 40,
                            "BookItemQtyHeight": 66,
                            "BookItemStackable": true,
                            "BookItemLevelOfDensity": 0,
                            "BookItemDiscount": 0,
                            "BookItemLineHaul": 0,
                            "BookItemTaxableFees": 0,
                            "BookItemTaxes": 0,
                            "BookItemNonTaxableFees": 0,
                            "BookItemDeficitCostAdjustment": 0,
                            "BookItemDeficitWeightAdjustment": 0,
                            "BookItemWeightBreak": 0,
                            "BookItemDeficit49CFRCode": "",
                            "BookItemDeficitIATACode": "",
                            "BookItemDeficitDOTCode": "",
                            "BookItemDeficitMarineCode": "",
                            "BookItemDeficitNMFCClass": "",
                            "BookItemDeficitFAKClass": "",
                            "BookItemRated49CFRCode": "",
                            "BookItemRatedIATACode": "",
                            "BookItemRatedDOTCode": "",
                            "BookItemRatedMarineCode": "",
                            "BookItemRatedNMFCClass": "",
                            "BookItemRatedFAKClass": "",
                            "BookItemCarrTarEquipMatControl": 0,
                            "BookItemCarrTarEquipMatName": "",
                            "BookItemCarrTarEquipMatDetID": 0,
                            "BookItemCarrTarEquipMatDetValue": null,
                            "BookItemUser1": "",
                            "BookItemUser2": "",
                            "BookItemUser3": "",
                            "BookItemUser4": "",
                            "BookItemUnitOfMeasureControl": 0,
                            "BookItemRatedNMFCSubClass": "",
                            "BookItemCommCode": "",
                            "BookItemHazControl": 0,
                            "BookItemBookPkgControl": 0,
                            "BookItemOrderNumber": "",
                            "BookItemUpdated": null,
                            "TrackingState": 0,
                            "Page": 1,
                            "Pages": 1,
                            "RecordCount": 1,
                            "PageSize": 1
                          }
                        ],
                        "TrackingState": 0,
                        "Page": 1,
                        "Pages": 1,
                        "RecordCount": 1,
                        "PageSize": 1
                      }
                    ],
                    "BookFees": null,
                    "LaneOriginAddressUse": null,
                    "BookMustLeaveByDateTime": null,
                    "BookExpDelDateTime": null,
                    "BookOutOfRouteMiles": 0,
                    "BookSpotRateAllocationFormula": 0,
                    "BookSpotRateAutoCalcBFC": true,
                    "BookSpotRateUseCarrierFuelAddendum": false,
                    "BookSpotRateBFCAllocationFormula": 0,
                    "BookSpotRateTotalUnallocatedBFC": 0,
                    "BookSpotRateTotalUnallocatedLineHaul": 0,
                    "BookSpotRateUseFuelAddendum": false,
                    "BookCreditHold": false,
                    "BookBestDeficitCost": 0,
                    "BookBestDeficitWeight": 0,
                    "BookBestDeficitWeightBreak": 0,
                    "BookRatedWeightBreak": 0,
                    "BookWgtAdjCost": 0,
                    "BookWgtAdjWeight": 0,
                    "BookWgtAdjWeightBreak": 0,
                    "BookBilledLoadWeight": 0,
                    "BookMinAdjustedLoadWeight": 0,
                    "BookSummedClassWeight": 0,
                    "BookWgtRoundingVariance": 0,
                    "BookHeaviestClass": null,
                    "BookAcutalHeaviestClassWeight": 0,
                    "BookRevDiscountRate": 0,
                    "BookRevDiscountMin": 0,
                    "BookRevLoadTenderTypeControl": 0,
                    "BookRevLoadTenderStatusCode": 0,
                    "BookCarrTarInterlinePoint": false,
                    "BookRevPreferredCarrier": false,
                    "BookLaneMustLeaveByDateTime": null,
                    "BookLaneMustLeaveByEndDateTime": null,
                    "BookCarrRequestedService": "",
                    "BookCarrActualService": "",
                    "BookCarrTransitTimeType": null,
                    "BookCarrTransitTime": null,
                    "BookLaneMustArriveByStartDateTime": null,
                    "BookLaneMustArriveByEndDateTime": null,
                    "BookLeadTimeLTLMinimum": null,
                    "BookProductionLeadTimeDays": null,
                    "BookProductionLeadTimeUpdateRequired": null,
                    "BookLeadTimeMultiStopDelayHours": null,
                    "BookLeadTimeHoursofService": null,
                    "BookLeadTimeAutomationDaysByMile": null,
                    "BookProductionLeadTimeApplied": null,
                    "TrackingState": 0,
                    "Page": 1,
                    "Pages": 1,
                    "RecordCount": 1,
                    "PageSize": 1
                  }
                ]
                """;


            #endregion

            BookRevenue[] oBookRevs = Newtonsoft.Json.JsonConvert.DeserializeObject<BookRevenue[]>(json); ;

            //BookRevenue[] oBookRevs = new BookRevenue[] 
            //{
            //    new BookRevenue()
            //    {
            //        BookControl = 1000001,
            //        BookProNumber = "TEST123456",
            //        BookCarrierControl = 9999,
            //        BookDateLoad = DateTime.Now,
            //        BookDateRequired = DateTime.Now.AddDays(3),
            //        BookOrigZip = "30349",
            //        BookDestZip = "77004",
            //        BookTotalWgt = 5976,
            //        BookTotalCube = 1267,
            //        BookTotalPL = 6,
            //        BookTotalCases = 426
            //    }
            //};
            decimal CarrierCost = 0;
            int CarrierControl = 0;
            bool Taxable = false;
            int CarrTarControl = 0;
            int CarrTarEquipControl = 0;
            string Message = "";


            List<Ngl.FreightMaster.Data.LTS.Book> books = new List<Ngl.FreightMaster.Data.LTS.Book>()
            {
                new Ngl.FreightMaster.Data.LTS.Book()
                {
                    BookSHID = "1223A",
                    BookCarrActDate = new DateTime(2010, 1, 1)
                }
            };

            List<Ngl.FreightMaster.Data.LTS.BookFee> bookFees = new List<Ngl.FreightMaster.Data.LTS.BookFee>()
            {
                new Ngl.FreightMaster.Data.LTS.BookFee()
                {
                     BookFeesAccessorialCode = 2,
                     BookFeesValue = 321,
                     BookFeesTaxable = false,
                     BookFeesOverRidden = false,
                     BookFeesMinimum = 7,
                     BookFeesAccessorialFeeAllocationTypeControl = 12,
                     BookFeesAccessorialFeeCalcTypeControl = 17,
                     BookFeesTarBracketTypeControl = 19,
                     BookFeesVariable = 23,
                     BookFeesVariableCode = 27
                }
            };

            Injector.Instance.SetObject("calc_existingBooks", () => { return books; });
            Injector.Instance.SetObject("calc_existingBookFees", () => { return bookFees; });

            List<Ngl.FreightMaster.Data.DataTransferObjects.BookFee> dtoBookFees = bookFees
                .Select(f => new Ngl.FreightMaster.Data.DataTransferObjects.BookFee
                {
                    BookFeesAccessorialCode = (int)f.BookFeesAccessorialCode,
                    BookFeesValue = f.BookFeesValue,
                    BookFeesTaxable = f.BookFeesTaxable,
                    BookFeesOverRidden = f.BookFeesOverRidden
                })
                .ToList();
            //target code requires mapping LTS and setting 
            oBookRevs[0].BookFees = dtoBookFees.ToList();

            var v = w.calculateFuelCharge_wrapper(ref oBookRevs, CarrierCost, CarrierControl, Taxable, CarrTarControl, CarrTarEquipControl, ref Message); ;

        }


    }
}
