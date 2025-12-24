using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DATIntegration
{
	public class LookupRates : DAT
	{
		public LookupRates(ConfiguredProperties properties) : base(properties) {}

		protected override int Execute(DTO.tblLoadTender lt)
		{
			int result;

			SessionFacade session;
			if (Account1FailsLogin(out session))
			{
				result = Result.Invalid;
			}
			else
			{
				session.LookupCurrentRate();
				session.LookupHistoricContract();
				session.LookupHistoricSpot();
				result = Result.Success;
			}
			return result;
		}
	}
}