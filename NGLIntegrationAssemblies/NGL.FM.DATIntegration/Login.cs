using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DATIntegration
{
	public class Login : DAT
	{
		public Login(ConfiguredProperties properties) : base(properties) {}

		protected override int Execute(DTO.tblLoadTender lt)
		{
			SessionFacade session;
			if (Account1FailsLogin(out session))
			{
				return Result.Failure;
			}
			Console.WriteLine("Login successful.");
			return Result.Success;
		}
	}
}