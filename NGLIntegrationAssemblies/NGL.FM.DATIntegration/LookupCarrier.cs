using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DATIntegration
{
	public class LookupCarrier : DAT
	{
		private const int DOT_NUMBER = 1258500;
		private const int MC_NUMBER = 177051;
		private const int USER_ID = 12;

		public LookupCarrier(ConfiguredProperties properties) : base(properties) {}

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
				session.LookupCarrierByDotNumber(DOT_NUMBER);
				session.LookupCarrierByMcNumber(MC_NUMBER);
				session.LookupCarrierByUserId(USER_ID);
				result = Result.Success;
			}
			return result;
		}
	}
}