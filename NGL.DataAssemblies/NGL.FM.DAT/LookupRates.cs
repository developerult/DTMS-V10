using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
	public class LookupRates : DAT
	{
		public LookupRates(ConfiguredProperties properties) : base(properties) {}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
		{
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;
            string s = "LookupRates is not available at this time.";
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
            //            session.LookupCurrentRate();
            //            session.LookupHistoricContract();
            //            session.LookupHistoricSpot();
            //            result = Result.Success;
            //        }

			
			//return result;
            return datReturn;
		}
	}
}