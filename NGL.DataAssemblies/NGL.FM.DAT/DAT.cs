using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;
using Ngl.Core;

namespace NGL.FM.DAT
{
    public class DAT
    {
        private readonly ConfiguredProperties _properties;
        private static SessionToken _token;
         
        #region Constructors

        //changed from protected so I could use it in testing. find out if this was ok or not. why protected?
        public DAT(string url, string username, string password)
        {
            // TODO REMOVE USER2 PASS2 FROM PROD ONLY HERE FOR TEST
            ConfiguredProperties properties = new ConfiguredProperties { Url = url, User1 = username, Password1 = password, User2 = "nxt_cnx2", Password2 = "nextgen" };
            _properties = properties;
        }
        protected DAT(ConfiguredProperties properties)
        {
            _properties = properties;
            //AlarmUrl = BuildAlarmUrl(_properties);
        }

        #endregion

        /// <summary>
        /// Control flow begins here.
        /// </summary>
        /// <param name="lt"></param>
        /// <returns> DTO.DATResults</returns>
        public static DTO.DATResults processData(DTO.tblLoadTender lt, DAL.WCFParameters oWCFPar)
        {
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;
            //var oWCFPar = new DAL.WCFParameters { Database = lt.Database, DBServer = lt.DBServer, UserName = lt.UserName, ConnectionString = lt.ConnectionString, WCFAuthCode = "NGLSystem" };
            var oSysData = new DAL.NGLSystemLogData(oWCFPar);
            var oLTData = new DAL.NGLLoadTenderData(oWCFPar);
            string source = "NGL.FM.DATIntegration.DAT.processData";
            //LVV CHANGE
            SessionToken token = getTokenfromDTOIfValid(lt.TokenString, lt.TokenExpiresDate);
            _token = token;

            //get the feature (action to be performed) and parse from int to enum
            Feature? feature = fParse(lt.DATFeature);
            if (feature == null)
            {              
                var msg = "DAT Program Failed to Execute: " + getFeaturesString();
                oSysData.AddApplicaitonLog(msg, source);
                var p = new string[]{lt.DATFeature.ToString()};
                datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATInvalidFeature, p);
                datReturn.Success = false;
                datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.DATError;
                datReturn.LTMessage = msg;
                datReturn.LTDATRefID = lt.LTDATRefID;
                datReturn.LTControl = lt.LoadTenderControl;
                datReturn.UserName = lt.UserName;
                datReturn.LTBookControl = lt.LTBookControl;
                datReturn.LTCarrierName = lt.LTCarrierName;
                datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                datReturn.LTCarrierControl = lt.LTCarrierControl;
                datReturn.LTBookCustCompControl = lt.LTBookCustCompControl;
                datReturn.LTCompName = lt.LTCompName;
                datReturn.LTBookSHID = lt.LTBookSHID;
                datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;

                return datReturn;
            }

            // hijack Console.Out so it writes to a text file
            // TODO Turn this off for Prod or maybe turn it on during Debug Mode?
            //RedirectOutput(@"\\NGLRDP06D\Development\Lauren Van Vleet\Docs EDI\DAT\ConnexionTest.txt");

            // TODO REMOVE USER2 PASS2 FROM PROD ONLY HERE FOR TEST
            //var properties = new ConfiguredProperties { Url = lt.SSOALoginURL, User1 = lt.SSOAUserName, Password1 = lt.SSOAPassword, User2 = "nxt_cnx3", Password2 = "nextgen" };
            var properties = new ConfiguredProperties { Url = lt.SSOALoginURL, User1 = lt.SSOAUserName, Password1 = lt.SSOAPassword };

            // map the feature and configurable properties to a DAT instance
            DAT prg = GetProgramInstance(feature.Value, properties, oSysData);

            if (prg == null)
            {
                string s = "DAT program could not execute because NGL.FM.DATIntegration.DAT.GetProgramInstance failed to return a result. See the log for more details.";
                var p = new string[] { s };
                datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
                datReturn.Success = false;
                //datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error;
                datReturn.LTMessage = s;
                datReturn.LTControl = lt.LoadTenderControl;
                datReturn.UserName = lt.UserName;
                datReturn.LTBookControl = lt.LTBookControl;
                datReturn.LTCarrierName = lt.LTCarrierName;
                datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                datReturn.LTBookSHID = lt.LTBookSHID;
                datReturn.LTCarrierControl = lt.LTCarrierControl;
                datReturn.LTBookCustCompControl = lt.LTBookCustCompControl;
                datReturn.LTCompName = lt.LTCompName;
                datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
                
                return datReturn;                
            }

            return TryToExecute(prg, lt, oWCFPar);
        }


        private static SessionToken getTokenfromDTO(string TokenString, DateTime? TokenExpiresDate)
        {
            SessionToken token = new SessionToken();

            string[] substrings = TokenString.Split('*');

            if ((!String.IsNullOrEmpty(TokenString)) && (TokenExpiresDate.HasValue))
            {
                string[] strP = (substrings[0]).Split('-');
                string[] strS = (substrings[1]).Split('-');

                byte[] primary = new byte[strP.Length];
                byte[] secondary = new byte[strP.Length];

                for (int i = 0; i < strP.Length; i++)
                {
                    try
                    {
                        primary[i] = Byte.Parse(strP[i]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Bad Format: '{0}'", TokenString == null ? "<null>" : TokenString);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("OverflowException: '{0}'", TokenString);
                    }
                }

                for (int i = 0; i < strS.Length; i++)
                {
                    try
                    {
                        secondary[i] = Byte.Parse(strS[i]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Bad Format: '{0}'", TokenString == null ? "<null>" : TokenString);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("OverflowException: '{0}'", TokenString);
                    }
                }
                token.primary = primary;
                token.secondary = secondary;
                token.expiration = TokenExpiresDate.Value;
            }
            else
            {
                token = new SessionToken {primary = new byte[] {}, secondary = new byte[] {}};
            }   
                         
            return token;
        }

        public static string getStringfromToken(SessionToken token)
        {
            string strToken;
            string[] strP = new string[token.primary.Length];
            string[] strS = new string[token.secondary.Length];

            for (int i = 0; i < token.primary.Length; i++)
            {
                strP[i] = token.primary[i].ToString();              
            }

            for (int i = 0; i < token.secondary.Length; i++)
            {
                strS[i] = token.secondary[i].ToString(); 
            }

            strToken = String.Join("-", strP) + '*' + String.Join("-", strS);
           
            return strToken;
        }

        private static SessionToken getTokenfromDTOIfValid(string TokenString, DateTime? TokenExpiresDate)
        {
            //LVV CHANGE
            SessionToken token = new SessionToken();

            string[] substrings = TokenString.Split('*');

            if ((!String.IsNullOrEmpty(TokenString)) && (TokenExpiresDate.HasValue) && (TokenExpiresDate.Value > DateTime.Now))
            {
                string[] strP = (substrings[0]).Split('-');
                string[] strS = (substrings[1]).Split('-');

                byte[] primary = new byte[strP.Length];
                byte[] secondary = new byte[strP.Length];

                for (int i = 0; i < strP.Length; i++)
                {
                    try
                    {
                        primary[i] = Byte.Parse(strP[i]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Bad Format: '{0}'", TokenString == null ? "<null>" : TokenString);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("OverflowException: '{0}'", TokenString);
                    }
                }

                for (int i = 0; i < strS.Length; i++)
                {
                    try
                    {
                        secondary[i] = Byte.Parse(strS[i]);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Bad Format: '{0}'", TokenString == null ? "<null>" : TokenString);
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("OverflowException: '{0}'", TokenString);
                    }
                }
                token.primary = primary;
                token.secondary = secondary;
                token.expiration = TokenExpiresDate.Value;
            }
            else
            {
                token = new SessionToken { primary = new byte[] { }, secondary = new byte[] { } };
            }

            return token;
        }

        //LVV CHANGE
        public static bool mustLogin()
        {
            bool res = true;

            if (_token.primary.Length > 0 && _token.secondary.Length > 0)
            {
                res = false;
            }

            return res;
        }

        protected virtual bool RequireDistinctUserAccounts
        {
            get { return false; }
        }
      
        protected virtual DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF) { return null; }

        private static DAT GetProgramInstance(Feature feature, ConfiguredProperties properties, DAL.NGLSystemLogData oSysData)
        {
            Type programType = typeof(DAT);
            foreach (Type type in programType.Assembly.GetTypes())
            {
                // if a concrete type, derived from DAT
                //Note: removed temporarily due to changes in abstract design
                if (type.IsAbstract == false && type.BaseType == programType)
                {
                    // if the type's name matches a Feature value
                    if (type.Name.Equals(feature.ToString()))
                    {
                        // if it can be constructed from a ConfiguredProperties instance
                        ConstructorInfo constructor = type.GetConstructor(new[] { typeof(ConfiguredProperties) });
                        if (constructor != null)
                        {
                            // use that type
                            object invoked = constructor.Invoke(new object[] { properties });
                            return (DAT)invoked;
                        }
                    }
                }
            }
            string source = "NGL.FM.DATIntegration.DAT.GetProgramInstance";
            var msg = string.Format("Feature '{0}' not recognized.", feature);
            oSysData.AddApplicaitonLog(msg, source);
            return null;
        }

        protected bool Account1FailsLogin(int UserSecurityControl, string NGLUserName, int LTControl, DAL.WCFParameters oWCF, out SessionFacade session)
        {
            return LoginFails(_properties.User1, _properties.Password1, _token, UserSecurityControl, NGLUserName, LTControl, oWCF, out session);
        }

        protected bool Account2FailsLogin(int UserSecurityControl, string NGLUserName, int LTControl, DAL.WCFParameters oWCF, out SessionFacade session)
        {
            return LoginFails(_properties.User2, _properties.Password2, _token, UserSecurityControl, NGLUserName, LTControl, oWCF, out session);
        }

        private bool LoginFails(string user, string password, SessionToken token, int UserSecurityControl, string NGLUserName, int LTControl, DAL.WCFParameters oWCF, out SessionFacade session)
        {    
            session = new ServiceFacade(_properties.Url).Login(user, password, token, UserSecurityControl, NGLUserName, LTControl, oWCF);
            if (session == null)
            {
                //WriteFailedLogin(user, password);
                return true;
            }
            return false;
        }

        //LVV CHANGE
        protected void getSession(out SessionFacade session)
        {
            session = new ServiceFacade(_properties.Url).getSession(_token);
            //if (session == null)
            //{
            //    //WriteFailedLogin(user, password);
            //    return true;
            //}
            //return false;
        }

        private static void RedirectOutput(string filename)
        {
            Console.SetOut(new StreamWriter(filename));
        }

        private static DTO.DATResults TryToExecute(DAT program, DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
        {
            var datReturn = new DTO.DATResults();
            datReturn.Success = false;
            try
            {
                return program.Execute(lt, oWCF);
            }
            catch (Exception ex)
            {
                var oSysData = new DAL.NGLSystemLogData(oWCF);
                var oLTData = new DAL.NGLLoadTenderData(oWCF);
                string source = "NGL.FM.DATIntegration.DAT.TryToExecute";
                oSysData.AddApplicaitonLog(ex.Message, source);
                if (ex.InnerException != null)
                {
                    oSysData.AddApplicaitonLog(ex.InnerException.Message, source);
                    string msg = ex.Message + ' ' + ex.InnerException.Message;
                    datReturn.Success = false;
                    //datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum._Error;
                    datReturn.LTMessage = msg;
                    datReturn.LTControl = lt.LoadTenderControl;
                    datReturn.LTDATRefID = lt.LTDATRefID;
                    datReturn.UserName = lt.UserName;
                    datReturn.LTBookControl = lt.LTBookControl;
                    datReturn.LTCarrierName = lt.LTCarrierName;
                    datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                    datReturn.LTCarrierControl = lt.LTCarrierControl;
                    datReturn.LTBookCustCompControl = lt.LTBookCustCompControl;
                    datReturn.LTCompName = lt.LTCompName;
                    datReturn.LTBookSHID = lt.LTBookSHID;
                    datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
                }
                else
                {
                    datReturn.Success = false;
                    datReturn.LTStatusCode = DTO.tblLoadTender.LoadTenderStatusCodeEnum.DATError;
                    datReturn.LTMessage = ex.Message;
                    datReturn.LTControl = lt.LoadTenderControl;
                    datReturn.LTDATRefID = lt.LTDATRefID;
                    datReturn.UserName = lt.UserName;
                    datReturn.LTBookControl = lt.LTBookControl;
                    datReturn.LTCarrierName = lt.LTCarrierName;
                    datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                    datReturn.LTCarrierControl = lt.LTCarrierControl;
                    datReturn.LTBookCustCompControl = lt.LTBookCustCompControl;
                    datReturn.LTCompName = lt.LTCompName;
                    datReturn.LTBookSHID = lt.LTBookSHID;
                    datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
                }
                                
            }
            finally
            {
                Console.Out.Flush();
                Console.Out.Close();
            }
            string s = "NGL.FM.DATIntegration.DAT.TryToExecute Failure. See the log for more details.";
            var p = new string[] { s };
            datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
            datReturn.Success = false;
            datReturn.LTControl = lt.LoadTenderControl;
            datReturn.UserName = lt.UserName;
            datReturn.LTBookControl = lt.LTBookControl;
            datReturn.LTDATRefID = lt.LTDATRefID;
            datReturn.LTCarrierName = lt.LTCarrierName;
            datReturn.LTCarrierNumber = lt.LTCarrierNumber;
            datReturn.LTCarrierControl = lt.LTCarrierControl;
            datReturn.LTBookCustCompControl = lt.LTBookCustCompControl;
            datReturn.LTCompName = lt.LTCompName;
            datReturn.LTBookSHID = lt.LTBookSHID;
            datReturn.LTTypeControl = lt.LTLoadTenderTypeControl;
            return datReturn;
        }

        public string getFailedLoginMsg()
        {
            return WriteFailedLogin(_properties.User1, _properties.Password1);
        }


        private static string WriteFailedLogin(string user, string password)
        {
            return string.Format("Login failed for user {0}.", user);
        }

        protected static class Result
        {
            public const int Invalid = -1;
            public const int Success = 0;
            public const int Failure = 1;
        }

        #region Enum Methods

        public static Feature? fParse(int f)
        {       
            Feature? feature;
            if (IsFeature(f.ToString(), out feature))
            {
                return feature;
            }
            return null;
        }

        private static bool IsFeature(string s, out Feature? feature)
        {
            try
            {
                feature = (Feature)Enum.Parse(typeof(Feature), s);
                return true;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
            feature = null;
            return false;
        }

        public static EquipmentType? eParse(string e)
        {
            EquipmentType? equip;
            if (IsEquip(e, out equip))
            {
                return equip;
            }
            return null;
        }

        private static bool IsEquip(string s, out EquipmentType? equip)
        {
            try
            {
                equip = (EquipmentType)Enum.Parse(typeof(EquipmentType), s);
                return true;
            }
            catch (ArgumentException) { }
            catch (OverflowException) { }
            equip = null;
            return false;
        }

        public static string getFeaturesString()
        {
            string s = "Feature must be one of the following:";
            
            string[] names = Enum.GetNames(typeof(Feature));
            Array values = Enum.GetValues(typeof(Feature));
            for (int i = 0; i < names.Length; i++)
            {
                s += string.Format(" {0} = {1}", (int)values.GetValue(i), names[i]);
            }

            return s;
        }

        public static StateProvince getStateProvince(string ST)
        {
            StateProvince res = new StateProvince();
            switch (ST.Trim().ToUpper())
            {
                case "AB":
                    res = StateProvince.AB;
                    break;
                case "AG":
                    res = StateProvince.AG;
                    break;
                case "AK":
                    res = StateProvince.AK;
                    break;
                case "AL":
                    res = StateProvince.AL;
                    break;
                case "AR":
                    res = StateProvince.AR;
                    break;
                case "AS":
                    res = StateProvince.AS;
                    break;
                case "AZ":
                    res = StateProvince.AZ;
                    break;
                case "BC":
                    res = StateProvince.BC;
                    break;
                case "BJ":
                    res = StateProvince.BJ;
                    break;
                case "BS":
                    res = StateProvince.BS;
                    break;
                case "CA":
                    res = StateProvince.CA;
                    break;
                case "CH":
                    res = StateProvince.CH;
                    break;
                case "CI":
                    res = StateProvince.CI;
                    break;
                case "CL":
                    res = StateProvince.CL;
                    break;
                case "CO":
                    res = StateProvince.CO;
                    break;
                case "CP":
                    res = StateProvince.CP;
                    break;
                case "CT":
                    res = StateProvince.CT;
                    break;
                case "CU":
                    res = StateProvince.CU;
                    break;
                case "DC":
                    res = StateProvince.DC;
                    break;
                case "DE":
                    res = StateProvince.DE;
                    break;
                case "DF":
                    res = StateProvince.DF;
                    break;
                case "DG":
                    res = StateProvince.DG;
                    break;
                case "EM":
                    res = StateProvince.EM;
                    break;
                case "FL":
                    res = StateProvince.FL;
                    break;
                case "GA":
                    res = StateProvince.GA;
                    break;
                case "GJ":
                    res = StateProvince.GJ;
                    break;
                case "GR":
                    res = StateProvince.GR;
                    break;
                case "GU":
                    res = StateProvince.GU;
                    break;
                case "HG":
                    res = StateProvince.HG;
                    break;
                case "HI":
                    res = StateProvince.HI;
                    break;
                case "IA":
                    res = StateProvince.IA;
                    break;
                case "ID":
                    res = StateProvince.ID;
                    break;
                case "IL":
                    res = StateProvince.IL;
                    break;
                case "IN":
                    res = StateProvince.IN;
                    break;
                case "JA":
                    res = StateProvince.JA;
                    break;
                case "KS":
                    res = StateProvince.KS;
                    break;
                case "KY":
                    res = StateProvince.KY;
                    break;
                case "LA":
                    res = StateProvince.LA;
                    break;
                case "MA":
                    res = StateProvince.MA;
                    break;
                case "MB":
                    res = StateProvince.MB;
                    break;
                case "MD":
                    res = StateProvince.MD;
                    break;
                case "ME":
                    res = StateProvince.ME;
                    break;
                case "MH":
                    res = StateProvince.MH;
                    break;
                case "MI":
                    res = StateProvince.MI;
                    break;
                case "MN":
                    res = StateProvince.MN;
                    break;
                case "MO":
                    res = StateProvince.MO;
                    break;
                case "MR":
                    res = StateProvince.MR;
                    break;
                case "MS":
                    res = StateProvince.MS;
                    break;
                case "MT":
                    res = StateProvince.MT;
                    break;
                case "NA":
                    res = StateProvince.NA;
                    break;
                case "NB":
                    res = StateProvince.NB;
                    break;
                case "NC":
                    res = StateProvince.NC;
                    break;
                case "ND":
                    res = StateProvince.ND;
                    break;
                case "NE":
                    res = StateProvince.NE;
                    break;
                case "NF":
                    res = StateProvince.NF;
                    break;
                case "NH":
                    res = StateProvince.NH;
                    break;
                case "NJ":
                    res = StateProvince.NJ;
                    break;
                case "NL":
                    res = StateProvince.NL;
                    break;
                case "NM":
                    res = StateProvince.NM;
                    break;
                case "NS":
                    res = StateProvince.NS;
                    break;
                case "NT":
                    res = StateProvince.NT;
                    break;
                case "NU":
                    res = StateProvince.NU;
                    break;
                case "NV":
                    res = StateProvince.NV;
                    break;
                case "NY":
                    res = StateProvince.NY;
                    break;
                case "OA":
                    res = StateProvince.OA;
                    break;
                case "OH":
                    res = StateProvince.OH;
                    break;
                case "OK":
                    res = StateProvince.OK;
                    break;
                case "ON":
                    res = StateProvince.ON;
                    break;
                case "OR":
                    res = StateProvince.OR;
                    break;
                case "PA":
                    res = StateProvince.PA;
                    break;
                case "PE":
                    res = StateProvince.PE;
                    break;
                case "PQ":
                    res = StateProvince.PQ;
                    break;
                case "PR":
                    res = StateProvince.PR;
                    break;
                case "PU":
                    res = StateProvince.PU;
                    break;
                case "QA":
                    res = StateProvince.QA;
                    break;
                case "QR":
                    res = StateProvince.QR;
                    break;
                case "RI":
                    res = StateProvince.RI;
                    break;
                case "SC":
                    res = StateProvince.SC;
                    break;
                case "SD":
                    res = StateProvince.SD;
                    break;
                case "SI":
                    res = StateProvince.SI;
                    break;
                case "SK":
                    res = StateProvince.SK;
                    break;
                case "SL":
                    res = StateProvince.SL;
                    break;
                case "SO":
                    res = StateProvince.SO;
                    break;
                case "TA":
                    res = StateProvince.TA;
                    break;
                case "TL":
                    res = StateProvince.TL;
                    break;
                case "TM":
                    res = StateProvince.TM;
                    break;
                case "TN":
                    res = StateProvince.TN;
                    break;
                case "TX":
                    res = StateProvince.TX;
                    break;
                case "UT":
                    res = StateProvince.UT;
                    break;
                case "VA":
                    res = StateProvince.VA;
                    break;
                case "VI":
                    res = StateProvince.VI;
                    break;
                case "VL":
                    res = StateProvince.VL;
                    break;
                case "VT":
                    res = StateProvince.VT;
                    break;
                case "WA":
                    res = StateProvince.WA;
                    break;
                case "WI":
                    res = StateProvince.WI;
                    break;
                case "WV":
                    res = StateProvince.WV;
                    break;
                case "WY":
                    res = StateProvince.WY;
                    break;
                case "YC":
                    res = StateProvince.YC;
                    break;
                case "YT":
                    res = StateProvince.YT;
                    break;
                case "ZT":
                    res = StateProvince.ZT;
                    break;
            }
            return res;
        }

        public static CountryCode getCountryCode(string country)
        {
            CountryCode res = new CountryCode();
            switch (country.Trim().ToUpper())
            {
                case "US":
                    res = CountryCode.US;
                    break;
                case "USA":
                    res = CountryCode.US;
                    break;
                case "CA":
                    res = CountryCode.CA;
                    break;
                case "MX":
                    res = CountryCode.MX;
                    break;
            }
            return res;
        }

        #endregion

        

        #region Not currently using

        //protected Uri AlarmUrl { get; private set; }

        //private static Uri BuildAlarmUrl(ConfiguredProperties configuredProperties)
        //{
        //    var uriBuilder = new UriBuilder
        //    {
        //        Scheme = "http",
        //        Host = configuredProperties.Host,
        //        Path = configuredProperties.Path,
        //        Port = configuredProperties.Port
        //    };
        //    Uri uri;
        //    if (!TryUri(uriBuilder, out uri))
        //    {
        //        IPAddress ipAddress;
        //        if (configuredProperties.Host != null && IPAddress.TryParse(configuredProperties.Host, out ipAddress)) { }
        //        else
        //        {
        //            string hostName = Dns.GetHostName();
        //            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
        //            ipAddress = addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork);
        //        }
        //        uriBuilder.Host = ipAddress.ToString();
        //        uri = uriBuilder.Uri;
        //    }
        //    return uri;
        //}

        //private static bool TryUri(UriBuilder builder, out Uri uri)
        //{
        //    bool wellFormed = false;
        //    try
        //    {
        //        uri = builder.Uri;
        //        wellFormed = true;
        //    }
        //    catch (UriFormatException)
        //    {
        //        uri = null;
        //    }
        //    return wellFormed;
        //}

        //protected bool DobAccountFailsLogin(out SessionFacade session)
        //{
        //    return LoginFails(_properties.DobUser, _properties.DobPassword, out session);
        //}

        //protected virtual int Validate()
        //{
        //    if (RequireDistinctUserAccounts && _properties.UserAccountsAreNotDistinct)
        //    {
        //        Console.WriteLine("User accounts must be different for the demo to work.");
        //        Console.WriteLine("Searches and alarms won't find loads posted from the same account.");
        //        Console.WriteLine("  Shared account was '{0}'.", _properties.User1);
        //        return Result.Invalid;
        //    }
        //    return Result.Success;
        //}



        #endregion

    }
}
