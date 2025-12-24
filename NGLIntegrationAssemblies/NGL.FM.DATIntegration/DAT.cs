using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;
using Ngl.Core;

namespace NGL.FM.DATIntegration
{
    public class DAT
    {
        private readonly ConfiguredProperties _properties;

        /// <summary>
        /// Control flow begins here.
        /// </summary>
        /// <param name="args">Command-line arguments. See <see cref="CommandLineInput.ShowUsage"/> 
        /// for usage.</param>.
        /// <returns>0 on success, -1 on bad input, 1 on runtime error.  See also <see cref="Result"/> 
        /// enum.</returns>
        public static int processData(DTO.tblLoadTender lt)
        {
            var oWCFPar = new DAL.WCFParameters {Database = lt.Database, DBServer = lt.DBServer, UserName = lt.UserName, ConnectionString = lt.ConnectionString, WCFAuthCode = "NGLSystem"};
            var oSysData = new DAL.NGLSystemLogData(oWCFPar);
            string source = "NGL.FM.DATIntegration.DAT.processData";

            EquipmentType? equip = eParse(lt.LTDATEquipType);

            //get the feature (action to be performed) and parse from int to enum
            Feature? feature = fParse(lt.DATFeature);

            if (feature == null)
            {              
                var msg = getFeaturesString();
                oSysData.AddApplicaitonLog(msg, source);
                return Result.Invalid;
            }

            // hijack Console.Out so it writes to a text file
            RedirectOutput(@"C:\Users\ConnexionTest.txt");

            // load configurable properties from a properties file
            //ConfiguredProperties properties = ConfiguredProperties.Load(input.PropertiesFilename);
            var properties = new ConfiguredProperties {Url = lt.SSOALoginURL, User1 = lt.SSOAUserName, Password1 = lt.SSOAPassword };

            // map the feature and configurable properties to a DAT instance
            DAT p = GetProgramInstance(feature.Value, properties, oSysData);

            if (p == null)
            {
                return Result.Invalid;
            }

            return TryToExecute(p, lt, oSysData);
        }

        //changed from protected so I could use it in testing. find out if this was ok or not. why protected?
        public DAT(string url, string username, string password)
        {
            ConfiguredProperties properties = new ConfiguredProperties {Url=url, User1 = username, Password1 = password};
            _properties = properties;
            AlarmUrl = BuildAlarmUrl(_properties);
        }
        protected DAT(ConfiguredProperties properties)
        {
            _properties = properties;
            AlarmUrl = BuildAlarmUrl(_properties);
        }

        protected virtual bool RequireDistinctUserAccounts
        {
            get { return false; }
        }

        protected Uri AlarmUrl { get; private set; }

        protected virtual int Execute(DTO.tblLoadTender lt) { return 0; }

        private static Uri BuildAlarmUrl(ConfiguredProperties configuredProperties)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = configuredProperties.Host,
                Path = configuredProperties.Path,
                Port = configuredProperties.Port
            };
            Uri uri;
            if (!TryUri(uriBuilder, out uri))
            {
                IPAddress ipAddress;
                if (configuredProperties.Host != null && IPAddress.TryParse(configuredProperties.Host, out ipAddress)) { }
                else
                {
                    string hostName = Dns.GetHostName();
                    IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                    ipAddress = addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }
                uriBuilder.Host = ipAddress.ToString();
                uri = uriBuilder.Uri;
            }
            return uri;
        }

        private static bool TryUri(UriBuilder builder, out Uri uri)
        {
            bool wellFormed = false;
            try
            {
                uri = builder.Uri;
                wellFormed = true;
            }
            catch (UriFormatException)
            {
                uri = null;
            }
            return wellFormed;
        }


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

        protected bool Account1FailsLogin(out SessionFacade session)
        {
            return LoginFails(_properties.User1, _properties.Password1, out session);
        }

        protected bool Account2FailsLogin(out SessionFacade session)
        {
            return LoginFails(_properties.User2, _properties.Password2, out session);
        }

        protected bool DobAccountFailsLogin(out SessionFacade session)
        {
            return LoginFails(_properties.DobUser, _properties.DobPassword, out session);
        }

        private bool LoginFails(string user, string password, out SessionFacade session)
        {
            session = new ServiceFacade(_properties.Url).Login(user, password);
            if (session == null)
            {
                WriteFailedLogin(user, password);
                return true;
            }
            return false;
        }

        private static void RedirectOutput(string filename)
        {
            Console.SetOut(new StreamWriter(filename));
        }

        private static int TryToExecute(DAT program, DTO.tblLoadTender lt, DAL.NGLSystemLogData oSysData)
        {
            try
            {
                //if (program.Validate() == Result.Invalid)
                //{
                //    return Result.Invalid;
                //}
                return program.Execute(lt);
            }
            catch (Exception ex)
            {
                string source = "NGL.FM.DATIntegration.DAT.TryToExecute";
                oSysData.AddApplicaitonLog(ex.Message, source);
                if (ex.InnerException != null)
                {
                    oSysData.AddApplicaitonLog(ex.InnerException.Message, source);
                }
            }
            finally
            {
                Console.Out.Flush();
                Console.Out.Close();
            }
            return Result.Failure;
        }

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

        private static void WriteFailedLogin(string user, string password)
        {
            Console.WriteLine("Login failed for user '{0}', password '{1}'.", user, password);
        }

        protected static class Result
        {
            public const int Invalid = -1;
            public const int Success = 0;
            public const int Failure = 1;
        }

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

    }
}
