#region usings
using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
#endregion

namespace NGL.FM.DATIntegration
{
	public class LookupDobCarriers : DAT
	{
		private readonly string _exampleCarrierId;

		public LookupDobCarriers(ConfiguredProperties properties)
			: base(properties)
		{
			_exampleCarrierId = properties.ExampleCarrierId;
		}

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
				session.LookupDobCarriersByCarrierId(_exampleCarrierId);
				result = Result.Success;
			}
			return result;
		}
	}
}
