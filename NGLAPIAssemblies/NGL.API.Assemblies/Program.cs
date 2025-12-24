using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ServiceModel;
using System.Data.SqlClient;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;
using BLL = NGL.FM.BLL;
using P44 = NGL.FM.P44;

namespace NGL.API.Assemblies
{
    class Program
    {
        private static DAL.WCFParameters _Parameters;
        public static DAL.WCFParameters Parameters
        {
            get
            {
                if (_Parameters == null)
                {

                    _Parameters = new DAL.WCFParameters
                    {
                        Database = "NGLMASPROD",
                        DBServer = "DESKTOP - 0R0EJUB",
                        ConnectionString = "Server = DESKTOP - 0R0EJUB; User ID = nglweb; Password = xxxx; Database = NGLMASProd",
                        WCFAuthCode = "NGLSystem",
                        UserName = "",
                        ValidateAccess = false
                    };
                }
                return _Parameters;
            }
            set { _Parameters = value; }
        }

        private static DAL.NGLLookupDataProvider _NGLLookupData;
        public static DAL.NGLLookupDataProvider NGLLookupData
        {
            get
            {
                if (_NGLLookupData == null)
                {
                    if ((Parameters != null))
                    {
                        _NGLLookupData = new DAL.NGLLookupDataProvider(Parameters);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Parameters Are Required");
                    }

                }
                return _NGLLookupData;
            }
            set
            {
                _NGLLookupData = value;
            }
        }


        private static DAL.NGLtblSingleSignOnAccountData _NGLtblSingleSignOnAccountData;

        public static DAL.NGLtblSingleSignOnAccountData NGLtblSingleSignOnAccountData
        {
            get
            {
                if (_NGLtblSingleSignOnAccountData == null)
                {
                    if ((Parameters != null))
                    {
                        _NGLtblSingleSignOnAccountData = new DAL.NGLtblSingleSignOnAccountData(Parameters);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Parameters Are Required");
                    }

                }
                return _NGLtblSingleSignOnAccountData;
            }
            set
            {
                _NGLtblSingleSignOnAccountData = value;
            }
        }

        static void Main(string[] args)
        {


            Console.WriteLine("Hit Enter to Contine");
            string sKey = Console.ReadLine();

        }

        static DTO.WCFResults GenerateQuote(DAL.Models.RateRequestOrder order, int BookControl)
        {
            DTO.WCFResults oRet = new DTO.WCFResults();
            int iLoadTenderControl = 0;
            string strMsg = "";
            DAL.NGLLoadTenderLogData oLTLogData = new DAL.NGLLoadTenderLogData(Parameters);
            oRet.Success = false;
            oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString());
            DAL.NGLLoadTenderData oLT = new DAL.NGLLoadTenderData(Parameters);
            BLL.NGLDATBLL oDATBLL = new BLL.NGLDATBLL(Parameters);
            DAL.NGLBookData oBookDAL = new DAL.NGLBookData(Parameters);
            DAL.NGLBookRevenueData oBookRev = new DAL.NGLBookRevenueData(Parameters);
            List<DAL.Models.RateRequestItem> oItems = new List<DAL.Models.RateRequestItem>();

            P44.P44Proxy oP44Proxy = new P44.P44Proxy("https://cloud.p-44.com", "rramsey@nextgeneration.com", "NGL2016!");
            P44.RateRequest oP44Data = new P44.RateRequest();
            bool blnUseP44API = false;
            bool blnUseNGLTariff = false;
            bool blnUseCHRAPI = false;
            bool blnUseUPSAPI = false;
            bool blnUseJTSAPI = false;

            LTS.tblSSOALEConfig oCHRLEConfig = new LTS.tblSSOALEConfig();
            List<LTS.tblSSOAConfig> lCHRCompConfig = new List<LTS.tblSSOAConfig>();

            LTS.tblSSOALEConfig oJTSLEConfig = new LTS.tblSSOALEConfig();
            List<LTS.tblSSOAConfig> lJTSCompConfig = new List<LTS.tblSSOAConfig>();

            LTS.tblSSOALEConfig oUPSLEConfig = new LTS.tblSSOALEConfig();
            List<LTS.tblSSOAConfig> lUPSCompConfig = new List<LTS.tblSSOAConfig>();
            int iCompControl = 0;
            blnUseCHRAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.CHRAPI, ref oCHRLEConfig, ref lCHRCompConfig, BookControl);
            blnUseJTSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.JTSAPI, ref oJTSLEConfig, ref lJTSCompConfig, BookControl);
            blnUseUPSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.UPSAPI, ref oUPSLEConfig, ref lUPSCompConfig, BookControl);
            bool blnProxyReady = oLT.CreateRateRequestOrderQuote(ref order, ref iLoadTenderControl, ref oP44Proxy, ref oP44Data, DAL.Utilities.SSOAAccount.P44, ref strMsg, ref blnUseP44API, ref oLTLogData);
            bool blnQuotesExist = false;
            DTO.GetCarriersByCostParameters tariffOptions = new DTO.GetCarriersByCostParameters(false, true, true, 0, 0); //use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later

            if (blnProxyReady && iLoadTenderControl != 0)
            {
                oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString());
                if (blnUseNGLTariff)
                {
                    if (blnUseP44API && tariffOptions.AllowAsync)
                    {
                        oRet.configureForAsyncMessages(iLoadTenderControl);
                        oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions);
                    }
                    else
                    {
                        oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, ref strMsg);
                    }

                    if (blnUseP44API)
                    {
                        blnQuotesExist = oLT.ProcessP44RateRequest(ref oP44Proxy, oP44Data, iLoadTenderControl, ref strMsg);
                    }

                    if (blnUseCHRAPI)
                    {
                        blnQuotesExist = oLT.ProcessCHRRateRequest(order, iLoadTenderControl, oCHRLEConfig, lCHRCompConfig, ref strMsg);
                    }

                    if (blnUseUPSAPI)
                    {
                        blnQuotesExist = oLT.ProcessUPSRateRequest(order, iLoadTenderControl, oUPSLEConfig, lUPSCompConfig, ref strMsg);
                    }

                    if (blnUseJTSAPI)
                    {
                        blnQuotesExist = oLT.ProcessJTSRateRequest(order, iLoadTenderControl, oJTSLEConfig, lJTSCompConfig, ref strMsg);
                    }

                    if (iLoadTenderControl != 0)
                    {
                        oRet.Success = true;
                    }
                    else
                    {
                        oRet.Success = false;
                    }

                }

            }

            return oRet;

        }
    }
}
