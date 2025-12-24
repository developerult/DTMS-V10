using System;
using System.ServiceModel;
using NGL.FM.DAT.DisplayHelpers;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DAT.Infrastructure
{
	/// <summary>
	/// Represents the state of the TFMI service (i.e., its endpoint URL) and
	/// its behavior (logging a user into a session).
	/// </summary>
	/// <remarks>For more details on the Facade design pattern, see 
	/// <seealso cref="http://en.wikipedia.org/wiki/Facade_pattern"/>.  </remarks>
	public class ServiceFacade
	{
		private readonly string _url;

        //LVV CHANGE
        private readonly string app = "Connexion C# .NET Test";
        private readonly string appVersion = "1.0";

		public ServiceFacade(string url)
		{
			_url = url;
		}

        public SessionFacade Login(string user, string password, SessionToken token, int UserSecurityControl, string NGLUserName, int LTControl, DAL.WCFParameters oWCF)
		{
			// build client to TFMI service 
			var remoteAddress = new EndpointAddress(_url);
			var binding = new BasicHttpBinding(BasicHttpSecurityMode.None) {MaxReceivedMessageSize = 2 << 20};
			var client = new TfmiFreightMatchingPortTypeClient(binding, remoteAddress);

			// build request
			var loginRequest = new LoginRequest
			                   {
			                   	loginOperation =
                                    new LoginOperation { loginId = user, password = password, thirdPartyId = "NGL FreightMasterTMS", apiVersion = "2" }
			                   };

			// build various headers required by the service method
            //var applicationHeader = new ApplicationHeader
            //                        {application = "Connexion C# .NET Test", applicationVersion = "1.0"};

            //LVV CHANGE
            var applicationHeader = new ApplicationHeader { application = app, applicationVersion = appVersion };

			var correlationHeader = new CorrelationHeader();
			var sessionHeader = new SessionHeader
			                    {sessionToken = token};

			// invoke the service
			WarningHeader warningHeader;
			LoginResponse loginResponse;
			client.Login(applicationHeader,
			             ref correlationHeader,
			             ref sessionHeader,
			             loginRequest,
			             out warningHeader,
			             out loginResponse);

			// return a SessionFacade, which wraps the login results along with the client object
			var data = loginResponse.loginResult.Item as LoginSuccessData;
			if (data == null) {
				//Console.Error.WriteLine("Error logging in");
				var serviceError = loginResponse.loginResult.Item as ServiceError;
                //serviceError.Display(1); // Remember to remove this line for production
                string strServiceError = serviceError.GetString();
                // add it to the log
                var oSysData = new DAL.NGLSystemLogData(oWCF);
                //var oLTData = new DAL.NGLLoadTenderData(oWCF);
                string source = "NGL.FM.DATIntegration.ServiceFacade.Login";
                oSysData.AddApplicaitonLog(strServiceError, source);
                //oLTData.updateDATPostResultsError(LTControl, strServiceError, NGLUserName);
                //int eLTSC = (int)DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error;
                //oLTData.updateLoadTender(LTControl, NGLUserName, Message: strServiceError, StatusCode: eLTSC);
				return null;
			}
            else {
                // save the token info to the database
                var strtoken = DAT.getStringfromToken(data.token);
                var oUSAT = new DAL.NGLtblUserSecurityAccessTokenData(oWCF);
                var res = oUSAT.InsertOrUpdatetblUserSecurityAccessToken(UserSecurityControl, 2, strtoken, data.expiration);
                if (!res.Success)
                {
                    // TODO THERE WERE ERRORS SO GET THE MESSAGES AND DO SOMETHING WITH THEM
                    foreach (string w in res.Warnings.Keys)
                    {
                        var msgs = res.Warnings[w];
                        var oSysData = new DAL.NGLSystemLogData(oWCF);
                        //var oLTData = new DAL.NGLLoadTenderData(oWCF);
                        string source = "NGL.FM.DATIntegration.ServiceFacade.Login";
                        oSysData.AddApplicaitonLog("There was a problem saving the DAT token string to the database.", source);                       
                    }
                              
                }

            }

			return new SessionFacade(applicationHeader, correlationHeader, data, client);
		}

        //LVV CHANGE
        public SessionFacade getSession(SessionToken token)
        {
            // build client to TFMI service 
            var remoteAddress = new EndpointAddress(_url);
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None) { MaxReceivedMessageSize = 2 << 20 };
            var client = new TfmiFreightMatchingPortTypeClient(binding, remoteAddress);

            // build various headers required by the service method
            var applicationHeader = new ApplicationHeader { application = app, applicationVersion = appVersion };
            var correlationHeader = new CorrelationHeader();
            var sessionHeader = new SessionHeader { sessionToken = token };


            // return a SessionFacade, which wraps the token along with the client object    
            return new SessionFacade(applicationHeader, correlationHeader, token, client);
        }

	}
}