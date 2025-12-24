using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
	public class Post : DAT
	{
		public Post(ConfiguredProperties properties) : base(properties) {}

        //protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        //{
        //    var datReturn = new DTO.DATResults();
        //    datReturn.Success = false;

        //    PostAssetRequest sampleLoad = BuildLoad(lt);

        //    SessionFacade session;
        //    if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
        //    {
        //        string s = getFailedLoginMsg();
        //        string sp = "Could not execute DAT Post because " + s + "Check the log for more details.";

        //        var p = new string[] { sp };
        //        datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
        //        datReturn.Success = false;
        //        datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error;
        //        datReturn.LTMessage = sp;
        //        datReturn.LTControl = lt.LoadTenderControl;
        //        datReturn.UserName = lt.UserName;
        //        datReturn.LTBookControl = lt.LTBookControl;
        //        datReturn.LTCarrierName = lt.LTCarrierName;
        //        datReturn.LTCarrierNumber = lt.LTCarrierNumber;
        //        datReturn.LTBookSHID = lt.LTBookSHID;
        //    }
        //    else
        //    {
        //        datReturn = session.Post(sampleLoad, lt, oWCF);
        //    }
        //    return datReturn;
        //}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        {
            //LVV CHANGE
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;

            PostAssetRequest sampleLoad = BuildLoad(lt);

            SessionFacade session;

            if (DAT.mustLogin())
            {
                if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
                {
                    string s = getFailedLoginMsg();
                    string sp = "Could not execute DAT Post because " + s + "Check the log for more details.";

                    var p = new string[] { sp };
                    datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
                    datReturn.Success = false;
                    datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.DATError;
                    datReturn.LTMessage = sp;
                    datReturn.LTControl = lt.LoadTenderControl;
                    datReturn.UserName = lt.UserName;
                    datReturn.LTBookControl = lt.LTBookControl;
                    datReturn.LTCarrierName = lt.LTCarrierName;
                    datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                    datReturn.LTBookSHID = lt.LTBookSHID;
                    datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
                    return datReturn;
                }
            }
            else
            {
                getSession(out session);
            }

            datReturn = session.Post(sampleLoad, lt, oWCF);

            return datReturn;
        }

        public static PostAssetRequest BuildLoad(DTO.tblLoadTender lt)
        {
            //string refId = when.Millisecond.ToString();
            
            var pcOrig = new PostalCode {code = lt.LTBookOrigZip, country = getCountryCode(lt.LTBookOrigCountry)};
            var pcDest = new PostalCode {code = lt.LTBookDestZip, country = getCountryCode(lt.LTBookDestCountry)};

            var Origin = new NamedPostalCode {city = lt.LTBookOrigCity, stateProvince = getStateProvince(lt.LTBookOrigState), postalCode = pcOrig};
            var Destination = new NamedPostalCode {city = lt.LTBookDestCity, stateProvince = getStateProvince(lt.LTBookDestState), postalCode = pcDest};
            EquipmentType? equip = eParse(lt.LTDATEquipType);
            //FIX THIS FIGURE OUT WHAT TO DO
            if (equip == null)
            {
                return null;
                //return Result.Invalid;
            }

            var shipment = new Shipment
            {
                equipmentType = equip.Value,
                origin = new Place { Item = Origin },
                destination = new Place { Item = Destination },
                rate = new ShipmentRate(),
                //truckStops = new TruckStops()
                truckStops =
                                new TruckStops
                                {
                                    //enhancements = new[] { TruckStopVideoEnhancement.Flash, TruckStopVideoEnhancement.Highlight },
                                    Item = new ClosestTruckStops(),
                                    //posterDisplayName = "12345"
                                }
            };

            bool blnHasVol = false, blnHasWgt = false;
            int vol = 0, wgt = 0;
            if (lt.LTBookTotalCube != 0)
            {
                blnHasVol = true;
                vol = lt.LTBookTotalCube;
            }
            if (lt.LTBookTotalWgt != 0)
            {
                blnHasWgt = true;
                wgt = Convert.ToInt32(lt.LTBookTotalWgt);
            }

            var eAvail = DateTime.Now;
            var lAvail = DateTime.Now;
            if (lt.LTDATEarliestAvailable.HasValue)
            {
                eAvail = lt.LTDATEarliestAvailable.Value;
            }
            if (lt.LTDATLastestAvailable.HasValue)
            {
                lAvail = lt.LTDATLastestAvailable.Value;
            }

            var postAssetOperation = new PostAssetOperation
            {
                availability =
                    new Availability
                    {
                        earliest = eAvail,
                        earliestSpecified = true,
                        latest = lAvail,
                        latestSpecified = true
                    },
                comments = new[] {lt.LTDATComment1, lt.LTDATComment2},
                count = 1,
			    countSpecified = true,
                ltlSpecified = false,
                dimensions =
                    new Dimensions
                    {
                        heightInchesSpecified = false,
                        lengthFeetSpecified = false,
                        volumeCubicFeet = vol,
                        volumeCubicFeetSpecified = blnHasVol,
                        weightPounds = wgt,
                        weightPoundsSpecified = blnHasWgt
                    },
                includeAsset = true,
                includeAssetSpecified = true,
                Item = shipment,
                //postersReferenceId = "test",
                //stops = 0,
                stopsSpecified = false
            };

            return new PostAssetRequest { postAssetOperations = new[] { postAssetOperation } };
        }

	}
}