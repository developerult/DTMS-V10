using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
    public class Delete : DAT
    {
        public Delete(ConfiguredProperties properties) : base(properties) { }

        //protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        //{
        //    var datReturn = new DTO.DATResults();
        //    datReturn.Success = false;

        //    SessionFacade session;
        //    if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
        //    {
        //        string s = getFailedLoginMsg();
        //        string sp = "Could not execute DAT Delete Post because " + s + "Check the log for more details.";

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
        //        string[] strID = { lt.LTDATRefID };
        //        datReturn = session.DeleteAssetByAssetIDs(strID, lt, oWCF);
        //    }
        //    return datReturn;
        //}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        {
            //LVV CHANGE
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;

            SessionFacade session;

            if (DAT.mustLogin())
            {
                if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
                {
                    string s = getFailedLoginMsg();
                    string sp = "Could not execute DAT Delete Post because " + s + "Check the log for more details.";

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
            
            string[] strID = { lt.LTDATRefID };
            datReturn = session.DeleteAssetByAssetIDs(strID, lt, oWCF);
            
            return datReturn;
        }
       

    }
}