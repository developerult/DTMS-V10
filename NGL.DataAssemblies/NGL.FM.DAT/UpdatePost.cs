using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
    public class UpdatePost : DAT
    {
        public UpdatePost(ConfiguredProperties properties) : base(properties) { }

        //protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        //{
        //    var datReturn = new DTO.DATResults();
        //    datReturn.Success = false;

        //    UpdateAssetRequest u = BuildUpdate(lt);

        //    SessionFacade session;
        //    if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
        //    {
        //        string s = getFailedLoginMsg();
        //        string sp = "Could not execute DAT Update Post because " + s + "Check the log for more details.";

        //        var p = new string[] { sp };
        //        datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
        //        datReturn.Success = false;
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
        //        datReturn = session.UpdatePost(u, lt, oWCF);
        //    }
        //    return datReturn;
        //}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        {
            //LVV CHANGE
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;

            UpdateAssetRequest u = BuildUpdate(lt);

            SessionFacade session;

            if (DAT.mustLogin())
            {
                if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
                {
                    string s = getFailedLoginMsg();
                    string sp = "Could not execute DAT Update Post because " + s + "Check the log for more details.";

                    var p = new string[] { sp };
                    datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
                    datReturn.Success = false;
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
            
            datReturn = session.UpdatePost(u, lt, oWCF);
            
            return datReturn;
        }


        //public static UpdateAssetRequest BuildUpdate(DTO.tblLoadTender lt)
        //{          

        //    bool blnHasVol = false, blnHasWgt = false;
        //    int vol = 0, wgt = 0;
        //    if (lt.LTBookTotalCube != 0)
        //    {
        //        blnHasVol = true;
        //        vol = lt.LTBookTotalCube;
        //    }
        //    if (lt.LTBookTotalWgt != 0)
        //    {
        //        blnHasWgt = true;
        //        wgt = Convert.ToInt32(lt.LTBookTotalWgt);
        //    }
        //    var uAsset = new AssetUpdate{
        //        ltlSpecified = false,
        //        countSpecified = false,
        //        stopsSpecified = false,

        //        //comments = new[] { lt.LTDATComment1, lt.LTDATComment2 },
        //        //comments = new[] { "new comment 1", "new comment 2" },

        //        dimensions =
        //            new Dimensions
        //            {
        //                heightInchesSpecified = false,
        //                lengthFeetSpecified = false,
        //                volumeCubicFeet = vol,
        //                volumeCubicFeetSpecified = blnHasVol,
        //                weightPounds = wgt,
        //                weightPoundsSpecified = blnHasWgt
        //            }
        //    };

        //    var uAssetOperation = new UpdateAssetOperation
        //    {
        //        Item = lt.LTDATRefID,
        //        Item1 = uAsset,
        //        ItemElementName = ItemChoiceType.assetId                                       
        //    };

        //    return new UpdateAssetRequest { updateAssetOperation = uAssetOperation };
        //}


        public static UpdateAssetRequest BuildUpdate(DTO.tblLoadTender lt)
        {
            //var request = new UpdateAssetRequest();

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
            var uShipment = new ShipmentUpdate
            {
                ltlSpecified = false,
                countSpecified = false,
                stopsSpecified = false,

                comments = new[] { lt.LTDATComment1, lt.LTDATComment2 },
                //comments = new[] { "new comment 1", "new comment 2" },

                dimensions =
                    new Dimensions
                    {
                        heightInchesSpecified = false,
                        lengthFeetSpecified = false,
                        volumeCubicFeet = vol,
                        volumeCubicFeetSpecified = blnHasVol,
                        weightPounds = wgt,
                        weightPoundsSpecified = blnHasWgt
                    }
            };

            var uAssetOperation = new UpdateAssetOperation
            {
                Item = lt.LTDATRefID,
                Item1 = uShipment,
                ItemElementName = ItemChoiceType.assetId
            };

            return new UpdateAssetRequest { updateAssetOperation = uAssetOperation };
        }




    }
}