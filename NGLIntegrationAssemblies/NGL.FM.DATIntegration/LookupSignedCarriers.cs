#region usings
using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
#endregion

namespace NGL.FM.DATIntegration
{
	public class LookupSignedCarriers : DAT
	{
		public LookupSignedCarriers(ConfiguredProperties properties)
			: base(properties) {}

		protected override int Execute(DTO.tblLoadTender lt)
		{
			int result;

			SessionFacade session;
			if (DobAccountFailsLogin(out session))
			{
				result = Result.Invalid;
			}
			else
			{
				session.LookupSignedCarriers();
				result = Result.Success;
			}
			return result;
		}
	}
}
