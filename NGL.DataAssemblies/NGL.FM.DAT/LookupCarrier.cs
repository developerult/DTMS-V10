using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
	public class LookupCarrier : DAT
	{
		private const int DOT_NUMBER = 1258500;
		private const int MC_NUMBER = 177051;
		private const int USER_ID = 12;

		public LookupCarrier(ConfiguredProperties properties) : base(properties) {}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
		{
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;
            string s = "LookupCarrier is not available at this time.";
            var p = new string[] { s };
            datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);

            //int result = 1;
            //int result;
            //SessionFacade session;
            //    if (Account1FailsLogin(lt.UserSecurityControl, oWCF, out session))
            //        {
            //            result = Result.Invalid;
            //        }
            //        else
            //        {
            //            session.LookupCarrierByDotNumber(DOT_NUMBER);
            //            session.LookupCarrierByMcNumber(MC_NUMBER);
            //            session.LookupCarrierByUserId(USER_ID);
            //            result = Result.Success;
            //        }

            
			//return result;
            return datReturn;
		}
	}
}