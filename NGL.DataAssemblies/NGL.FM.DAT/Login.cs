using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
	public class Login : DAT
	{
		public Login(ConfiguredProperties properties) : base(properties) {}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
		{
            var datReturn = new DTO.DATResults();
            datReturn.Success = true;
            //var strRes = new string[2];

			SessionFacade session;
            if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session))
			{
                string s = getFailedLoginMsg() + "Check the log for more details.";
                var p = new string[] { s };
                datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
                datReturn.Success = false;
                datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.DATError;
                datReturn.LTMessage = s;
                datReturn.LTControl = lt.LoadTenderControl;
                datReturn.UserName = lt.UserName;
                datReturn.LTBookControl = lt.LTBookControl;
                datReturn.LTCarrierName = lt.LTCarrierName;
                datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                datReturn.LTBookSHID = lt.LTBookSHID;
                datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
			}
			//Console.WriteLine("Login successful.");
            return datReturn;
		}
	}
}