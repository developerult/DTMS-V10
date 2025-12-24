using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DATIntegration
{
	public class Search : DAT
	{
		public Search(ConfiguredProperties properties) : base(properties) {}

		protected override bool RequireDistinctUserAccounts
		{
			get { return true; }
		}

		protected override int Execute(DTO.tblLoadTender lt)
		{
			int result;
            DateTime when = DateTime.Now;
			PostAssetRequest sampleLoad = SampleFactory.BuildLoad(when);

			// build a search request based on the load to be posted
			CreateSearchRequest searchRequest = SampleFactory.BuildSearch(sampleLoad);

			SessionFacade session1;
			SessionFacade session2;
			if (Account1FailsLogin(out session1) || Account2FailsLogin(out session2))
			{
				result = Result.Invalid;
			}
			else
			{
				session1.Post(sampleLoad);
				session2.Search(searchRequest);
				result = Result.Success;
			}

			return result;
		}
	}
}