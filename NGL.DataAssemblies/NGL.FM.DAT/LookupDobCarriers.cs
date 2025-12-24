#region usings
using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;
#endregion

namespace NGL.FM.DAT
{
	public class LookupDobCarriers : DAT
	{
		private readonly string _exampleCarrierId;

		public LookupDobCarriers(ConfiguredProperties properties)
			: base(properties)
		{
			_exampleCarrierId = properties.ExampleCarrierId;
		}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
		{
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;
            string s = "LookupDobCarriers is not available at this time.";
            var p = new string[] { s };
            datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);

            //int result = 1;
			//int result;

            //SessionFacade session;
            //if (DobAccountFailsLogin(out session))
            //{
            //    result = Result.Invalid;
            //}
            //else
            //{
            //    session.LookupDobCarriersByCarrierId(_exampleCarrierId);
            //    result = Result.Success;
            //}
			//return result;
            return datReturn;
		}
	}
}
