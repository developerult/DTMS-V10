#region usings
using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
#endregion

namespace NGL.FM.DATIntegration
{
	public class LookupDobEvents : DAT
	{
		private readonly DateTime _exampleSinceDate;

		public LookupDobEvents(ConfiguredProperties properties)
			: base(properties)
		{
			_exampleSinceDate = properties.DobCarrierExampleSinceDate;
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
				session.LookupDobEvents(_exampleSinceDate);
				result = Result.Success;
			}
			return result;
		}
	}
}
