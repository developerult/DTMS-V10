using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace NGL.FM.CHRAPI
{
    public class CHRAPI
    {


        public CHRAPI(bool bUseTLs12 = true)
        {
            if (bUseTLs12)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }
        }

        public enum MessageEnum
        {
            None,
            E_UnExpected, //"An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
            E_OptionalCharge, //"Additional charges if required
            E_NoRatesFound, //No Rates Found
            E_InvalidCarrierNumber, // "Invalid Carrier Configuration for API";
            E_CommunicationFailure, // "Communication with API Service Failed"
            E_InvalidShipDate, // "Invalid Ship Date"
            E_WeightTooLowForLTL, //"The weight is too low for LTL
            E_WeightTooHighForLTL, //"The weight is too high for LTL and too low for truckload
            E_WeightTooLowForTL, //"The weight is too low for Truckload
            E_WeightTooHighForTL //"The weight is too high for truckload
        }

        public static string getMessageNotLocalized(CHRAPI.MessageEnum key, string sDefault)
        {
            string sRet = sDefault;
            switch (key)
            {
                case MessageEnum.None:
                    sRet = "";
                    break;
                case MessageEnum.E_UnExpected:
                    sRet = "An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes.";
                    break;
                case MessageEnum.E_OptionalCharge:
                    sRet = "Additional charges if required";
                    break;
                case MessageEnum.E_NoRatesFound:
                    sRet = "No Rates Found";
                    break;
                case MessageEnum.E_InvalidCarrierNumber:
                    sRet = "Invalid Carrier Configuration for API";
                    break;
                case MessageEnum.E_CommunicationFailure:
                    sRet = "Communication with API Service Failed";
                    break;
                case MessageEnum.E_InvalidShipDate:
                    sRet = "Invalid Ship Date";
                    break;
                case MessageEnum.E_WeightTooLowForLTL:
                    sRet = "The weight is too low for LTL";
                    break;
                case MessageEnum.E_WeightTooHighForLTL:
                    sRet = "The weight is too high for LTL and too low for truckload";
                    break;
                case MessageEnum.E_WeightTooLowForTL:
                    sRet = "The weight is too low for Truckload";
                    break;
                case MessageEnum.E_WeightTooHighForTL:
                    sRet = "The weight is too high for truckload";
                    break;
                default:
                    break;
            }
            return sRet;
        }
        //public Enum:"Customer" "20" "55" "65" "115" "170" "180" "205" "215" "230" "260" "275" "280" "285" "300" "315" "350" "360" "365" "370" "385" "400" "405" "420" "425" "445" "450" "455" "470" "485" "495" "497" "510" "520" "540" "545" "590" "593" "605" "635" "665" "670" "685" "690" "695" "740" "761" "800" "801" "802" "803" "804" "898" "899" "900" "901" "902" "903" "904" "905" "906" "907" "908" "909" "910" "911" "ACC" "AGR" "AGS" "AIR" "ALH" "AMC" "AMS" "APT" "ATR" "BAT" "BIO" "BLC" "BLK" "BND" "BRD" "BSC" "CAA" "CEX" "CHE" "CHN" "CIS" "CLA" "CLN" "CLS" "CNS" "COA" "COL" "COU" "CPC" "CPE" "CRE" "CRS" "CSE" "CTC" "CTG" "CTL" "CUF" "CUS" "DAM" "DCF" "DEL" "DEN" "DEP" "DET" "DFD" "DFG" "DHF" "DIC" "DIS" "DMC" "DOC" "DOV" "DRC" "DRF" "DSC" "DTC" "DTL" "DTU" "DTV" "DUT" "DYA" "EBD" "EEX" "EMT" "ENC" "ESD" "EUC" "EXC" "EXD" "EXL" "EXM" "EXP" "EXR" "EXW" "FDA" "FDT" "FFR" "FLP" "FTH" "FTW" "FTZ" "FWA" "FWC" "GST" "HMF" "HOL" "HST" "IDL" "IFS" "IHT" "INC" "INS" "INV" "IPU" "ISO" "IST" "ITS" "LAA" "LAY" "LEC" "LFT" "LMF" "LOA" "LOC" "MIC" "MPF" "MRK" "MSG" "NDD" "NYD" "NYP" "OFU" "OHF" "OPT" "ORD" "ORF" "ORM" "OVR" "PAE" "PCS" "PEC" "PER" "PFA" "PKG" "PMT" "POD" "PPH" "PPN" "PRT" "PSC" "PSH" "PSS" "PST" "PUC" "QST" "RAJ" "RCC" "RCL" "REC" "REF" "REP" "RES" "RET" "SAT" "SCL" "SCS" "SDL" "SEC" "SEG" "SER" "SET" "SOC" "SPS" "SPT" "SRA" "SRG" "SUR" "SYA" "TAK" "TAL" "TAR" "TDT" "THC" "TMF" "TRC" "TRL" "TTC" "UNL" "URC" "VOR" "WAR" "WBF" "WBL" "WCT" "WDC" "WFL" "WHC" "WHF" "WIO" "WIS" "WKT" "WLB" "WLC" "WLS" "WMS" "WOP" "WOT" "WPC" "WPK" "WPL" "WPP" "WRC" "WRE" "WRI" "WRS" "WSS" "WSW" "WTV" "WWT" "WXD" "XTN" "496" "BYD" "BYP" "DCV" "ICT" "PDD" "PDO" "TSA" "UND" "HAZ" "ISF" "913" "914" "915" "916" "316" "BPC" "BPR" "BPS" "BPT" "CEM" "CMX" "COT" "CUV" "MXC" "MXH" "MXR" "MXV" "OVT" "PEN" "CTS" "LRC" "AMF" "BPI" "SCR" "AES" "NAV" "DLA" "PLA" "PSF" "CSF" "894" "895" "896" "897" "EMN" "SPF" "TSC" "HCR" "CNT" "CMM" "RNT" "PRF" "CSD" "HCD" "CSP" "GVP" "GVD" "HCP" "NCP" "NCD" "BUV" "COW" "HOS" "INP" "RAP" "SIE" "TAW" "OTD" "MES" "ENS" "ACF" "SLF" "BLR" "FLT" "917" "FCP" "GSI" "SMA" "CFI" "EXT" "EXY" "AAI" "CAS" "CCF" "CMC" "DCC" "DOF" "EDC" "EMS" "ERF" "FLC" "FUC" "GAS" "GTA" "HBL" "LBE" "LUC" "MLC" "PAC" "PCH" "PRC" "PRS" "PUD" "SCH" "STR" "STU" "TPA" "OCT" "STC" "EPC" "DPC" "STD" "RFC" "SYC" "SED" "MEC" "SUF" "IAC" "CFS" "BTF" "ESS" "AMD" "ORC" "LIF" "PCF" "GIO" "TLX" "GAT" "LBL" "LCG" "LBF" "HLS" "IMP" "CIF" "CCE" "EDN" "HET" "PMU" "HEL" "STM" "TOL" "VAC" "PRP" "PDC" "SBC" "PFC" "TDR" "OTH" "DTH" "ODO" "DDO" "AWB" "CYC" "DEC" "FAF" "GRI" "LCL" "PCC" "PPF" "TPF" "WVF" "CSR" "27" "GSC" "ATC" "ECV" "GAR" "BTR" "CRD" "IVT" "TAP" "SEN" "IVP" "IBP" "COC" "DRY" "FOB" "EWC" "HTS" "DSB" "HTC" "FAH" "EIC" "NPC" "LSF" "AHD" "ESV" "FCC" "PEP" "OGA" "MFE" "CAH" "GOA" "75" "CBT" "DCT" "921" "922" "COV" "DBR" "IAT" "POM" "ACI" "ASF" "BOK" "CDD" "CDF" "COF" "CVE" "DCH" "DDP" "DKF" "DMI" "ESC" "EXA" "EXO" "LCH" "PES" "VMS" "SRC" "CFE" "CDA" "IEC" "DGS" "HAS" "701" "750" "751" "753" "754" "755" "756" "1401" "1402" "1403" "DTF" "OTF" "SRD" "SRO" "1900" "2000" "2100" "INF" "DDT" "DUF" "EMF" "LYO" "LYD" "999" "145" "ADV" "AMB" "ARB" "BEY" "BKA" "BSS" "CBL" "CFL" "CTF" "DAA" "DEM" "FAK" "FCB" "GSS" "HAN" "HHB" "HRS" "HZC" "IFC" "LAB" "LFC" "MIN" "MNC" "MSC" "OCH" "PIR" "POS" "PTS" "PWT" "PYS" "RAM" "RLS" "RMC" "SPC" "SSF" "STO" "STP" "SWC" "TAY" "TER" "TRM" "WEA" "WFG" "WFH" "IFF" "CME" "MDF" "CPV" "EMA" "EPF" "TPC" "GOF" "LOM" "ISE" "STF" "CSU" "AHF" "MEF" "FID" "LCK" "BYC" "BCD" "SO1" "SO2" "SO3" "COR" "TRE" "LSK" "DON" "CTM" "DCK" "RPK" "FGT" "CHF" "BRF" "MKF" "FES" "CLM" "PDR" "AVP" "GBB" "GLF" "GLT" "MOTLF" "MOTMF" "TROLF" "TROMF" "REBAT" "ASCMB" "SPCRV" "SPCAS" "PENNY" "WELLF" "WELMF" "BAMLF" "BAMMF" "SWTLF" "ECOLF" "ECOMF" "SRVLF" "SRVMF" "N22LF" "N23LF" "OURLF" "GLYLF" "GLYMF" "PNKLF" "PECLF" "GRNLF" "SWTMF" "MKU" "EVC" "APA" "ARA" "GAV" "REB" "SEL" "ABG" "ICG" "TET" "PAF" "OSO" "RSP" "MIF" "SPL" "SPA" "SPO" "AFS" "NCH" "ASM" "LGP" "444" "CNA" "DEB" "DOM" "EGY" "FER" "FIM" "FOR" "FRA" "FUM" "HRT" "MAU" "PCD" "PNL" "PPC" "RED" "REI" "SLU" "VET" "WAS" "WAT" "BIN" "206" "PLS" "OCS" "TPS" "BUP" "RBU" "CMR" "SPQ" "IMF" "IMV" "BLD" "1404" "1405" "1406" "ENF" "ENV" "PRO" "PRV" "WAF" "WAV" "EPS" "AKS" "CCT" "ECC" "HIS" "PRD" "TRR" "RIT" "GIT" "BAC" "CCR" "CLC" "DCR" "SC1" "FCH" "AFC" "WMF" "IFP" "OSP" "OCP" "DSP" "DCP" "CBC" "BRT" "ECH" "OID" "OCR" "RPT" "SOS" "TGS" "TKP" "WSC" "SFA" "HHC" "RHC" "IFA" "FLB" "LWS" "MFD" "SEP" "SCA" "PHL" "JAM" "VGM" "HSB" "PFS" "CCS" "DTY" "RNR" "RCD" "DRP" "PKU" "EBP" "LFD" "CPL" "AGM" "APH" "FSI" "FWS" "NHT" "EPA" "CPS" "USL" "NMF" "RCO" "ROS" "OVS" "TIV" "TMR" "RER" "FTA" "RBY" "RBI" "RBV" "RBE" "WAC" "ABT" "CON" "QCC" "SUN" "SBS" "SDV" "NGT" "GTD" "ASR" "SGR" "SOG" "1013" "101" "1014" "DTZ" "919" "923" "WTT"
        /* 
         *C.H.Robinson defined code for rating code.Here is a list of the most commonly used rate codes:

         400 - Line Haul, 405 - Fuel Surcharge, 260 - Deliver Surchage, IDL - Inside Delivery, IPU - Inside Pickup LFT - Lift Gate or Forklift Service, DET - Detention, 
        DLA - Delivery Limited Access, PLA - Pickup Limited Access RES - Residential Delivery Fee, REP - Residential Pickup Fee, CSD - Construction Site Delivery here.
        */
        public enum SupportedFees
        {
            IDL, // - Inside Delivery
            IPU, //- Inside Pickup 
            LFT, //- Lift Gate or Forklift Service,
            DET, // - Detention, -- not supported for  quotes
            DLA, //- Delivery Limited Access, -- not supported for  quotes
            PLA, //- Pickup Limited Access-- not supported for  quotes
            RES, // - Residential Delivery Fee, 
            REP, //- Residential Pickup Fee, 
            CSD, // - Construction Site Delivery here.

        }

        public static string getCHRFeeCodeMapping(string sFeeCode)
        {
            string sRet = "";


            switch (sFeeCode)
            {

                case "IPU":
                    sRet = "IPU"; break;
                case "INDEL":
                case "INEDEL":
                case "INGDEL":
                case "INNEDEL":
                case "IDL":
                    sRet = "IDL"; break;
                case "LGPU":
                case "LGDEL":
                case "LFT":
                    sRet = "LFT"; break;
                case "REP":
                    sRet = "REP"; break;
                case "RESDEL":
                case "RES":
                    sRet = "RES"; break;
                case "CONDEL":
                case "ACH":
                    sRet = "CSD"; break;
                default: break;
            }

            return sRet;
        }



        public enum SpecialRequirement
        {
            liftGate,
            insidePickup,
            insideDelivery,
            residentialNonCommercial,
            limitedAccess,
            tradeShoworConvention,
            constructionSite,
            dropOffAtCarrierTerminal,
            pickupAtCarrierTerminal
        }

        //Enum:"USD" "CAD" "EUR" "MXN" "CHF" "GBP" "HUF" "CZK" "NZD" "PLN" "ZAR" "VEB" "VND" "TWD" "TRY" "SEK" "SGD" "RUB" "NOK" "HKD" "INR" "JPY" "CLP" "CNY" "DKK" "AUD" "BRL" "AED" "PEN" "THB" "ARS" "MYR" "VEF" "SAR" "BBD" "BHD" "BMD" "BSD" "ILS" "ISK" "KES" "KRW" "KWD" "OMR" "PHP" "PKR" "QAR" "RON" "TTD" "UAH" "LKR" "XPF" "UYU" "COP" "CRC" "FJD"

        /*
         * Code to indicate the currency being used. The most common currently in use with C.H. Robinson:

        USD - US Dollar
        CAD - Canadian Dollar
        MXN - Mexican Peso
        EUR - European Euro
        */

        /*

                Enum:"BAL" "BIN" "BOT" "BXT" "JAR" "LSE" "LUG" "OTHR" "REL" "SAK" "SKD" "SKE" "SLP" "TLD" "TRY" "Empty" "BAG" "BOX" "BDL" "CTN" "CAS" "CRT" "DZ" "DRM" "EAC" "GYL" "HDW" "LNFT" "PKG" "PAL" "PLT" "PCS" "LBS" "ROL" "TOT" "YD" "Skids" "GAL" "KIT" "FT" "PAD" "PR" "SET" "SHT" "M" "KG" "IBC" "BAT" "AST" "CUB" "RKS" "PCK" "TH" "24BIN" "30BIN" "36BIN" "40BIN" "BSHL" "CLLPK" "DRC" "ECO" "EURO" "FLAT" "HFBIN" "HFCTN" "HVYPK" "IFCO" "I6408" "I6411" "I6413" "I6416" "I6419" "I6423" "I6425" "I6428" "ORBIS" "O6409" "O6411" "O6413" "O6416" "O6419" "O6425" "O6428" "P4311" "P4317" "P6408" "P6411" "P6413" "P6416" "P6419" "P6423" "P6425" "P6428" "RPC" "R6411" "R6413" "R6416" "TNBIN" "TOSCA" "T6408" "T6411" "T6413" "T6416" "T6419" "T6423" "T6425" "T6428" "WDCRT" "BRL" "RCK" "TUB" "CG" "DSP" "SQF" "CYL" "KEG" "IFCLB" "PLT1" "PLT2" "PLT3" "CAR" "COIL" "PI" "GOH" "BLK" "POG" "L" "IPLT"
        C.H.Robinson defined code for packaging type/code.Here is a list of the most commonly used packagingCodes:

        BAL - Bale, BAG - Bags, BIN - Bin, BOX - Boxes, CTN - Cartons
        CAS - Case, CRT - Crates, PKG - Package, EAC - Eaches, PLT - Pallet, PCS - Pieces

                */

        /*
                string (temperatureSensitiveEnum)
                Enum:"Dry" "Fresh" "Frozen" "Chilled" "DeepFrozen" "Refrigerated" "Protect" "Produce" "Mixed" "Empty"
        Indicates if temperature control is needed, and what level:

        Dry - Dry
        Fresh - Fresh(25 to 32 degrees)
        Frozen - Frozen(-10 to 0 degrees)
        Chilled - Chilled(40 to 60 degrees)
        DeepFrozen - Deep Frozen(-20 to -20 degrees)
        Refrigerated - Refrigerated(33 to 39 degrees)
        Protect - Protect(33 to 60 degrees)
        Custom - Custom
        Produce - Produce
        Mixed - Mixed
        Empty - Empty
        temperatureUnit
        string (temperatureUomEnum)
        Enum:"Fahrenheit" "Celsius" "Empty"

        */


        /*      
        string array of referenceNumbers
        Enum:"SHID" "MBOL" "PO" "BOL" "CFRM" "CRID" "CUSTPO" "DEL" "DIST" "FIN" "GL" "JNO" "PU" "RANumber" "RLPRO" "MREF" "Empty" "PRO" "CCN" "SBOL" "CON" "APPT" "CHRW_ID" "FDA" "IREF" "LIC" "ORD" "PLU" "SCAC" "SEAL" "SKU" "SREF" "STCC" "TEB" "VPO" "WB" "XXXX" "TRIA" "TRANS" "GCC" "SDATE" "CDATE" "ACCT" "CCTR" "IGM" "GIGM" "LIGM" "SMTP" "PERMIT" "RNO" "DVCN" "PRCL" "CPUC" "SHPMD" "AFR" "BUYID" "VNDID" "DIV" "DEPT" "POTYPE" "GRGI" "DUNS" "PALLETREF" "CITTN" "CIN" "SHIPWITH" "PN" "UPC" "PRCD" "VESSEL" "VOYAGE" "FN" "FEEDVESS" "FEEDVOY" "SHIPREF" "CBL" "SPAC" "POLN" "VPOLN" "EIN" "CNSG" "PROFITCTR" "MFGPART" "PACKSLIP" "THIRDPRTY" "DEFATTR" "FSTRK" "FOItemID" "POLineNum" "PARCELSPC" "CNS1" "CNS2" "CNS3" "CNS4" "CNS5" "CNS6" "CNS7" "TREF" "VREF" "UPRICE" "FWPARTY" "BYPARTY" "HBL" "CBP" "TRANSFER" "SO" "ORDTRK" "QUOTEID" "EQT" "BOOTH" "TS" "OH #" "CNTNR" "VAS" "Size" "Color" "Style" "HCERT" "B#" "ETMSID" "214-IMO" "SHPWeight" "SHPVolume" "SHPDims" "SERIALIZED" "SERIALNUM" "CPSIA" "EDIINVREF" "SQID" "QSeqNum" "OuterPkStu" "GLC" "COMP" "COST" "DTYGL" "DTYCC" "BRKGL" "BRKCC" "GPS" "IAC" "WHRCT" "ERGUID" "SONUM" "CCNUM" "PrimCCNUM" "PrevCCNUM" "CMRN" "TRANSSHIP" "PGIDATE" "AESFIL" "CUSTQUOTID" "QUOTETRACE" "ODATE"

        C.H.Robinson defined code for reference number types.Here is a list of the most commonly used reference number types:

        SHID - Shipment ID #, MBOL - Master Bill of Lading #, PO - Purchase Order #, BOL - Bill of Lading #, CRID - Customer Specific Ref Number, CUSTPO - Customer PO Number, DEL - Delivery #, JNO - Job Number, PU - Pickup #, CON - Customer Order Number, APPT - Appointment Number

                */

        public enum RefNumbers
        {
            SHID, // - Shipment ID #,
            MBOL, // - Master Bill of Lading #,
            PO, //- Purchase Order #,
            BOL, // - Bill of Lading #,
            CRID, // - Customer Specific Ref Number,
            CUSTPO, // - Customer PO Number,
            DEL, // - Delivery #, 
            JNO, // - Job Number, 
            PU, // - Pickup #, 
            CON, // - Customer Order Number,
            APPT, // - Appointment Number
        }

        public enum TransMode
        {
            LTL,
            TL,
            Air,
            Ocean,
            Bulk,
            Consol,
            Flatbed

        }

        /*
        Defines the mode:

        LTL - Less Than Truckload
        TL - Truckload
        Air - Air
        Ocean - Ocean
        Bulk - Bulk
        Consol - Consolidated
        Flatbed - Flatbed
        */

        public enum EquipType { Van, Reefer, Flatbed, Lcl, Container20, Container40, Container40HighCube, Container45HighCube, Container20FlatRack, Container40FlatRack, Container40HighCubeFlatRack, Container45FlatRack, Container20OpenTop, Container40OpenTop, Container20Reefer, Container40Reefer, Container40HighCubeReefer, Container45Reefer, Container20ReeferDryUsage, Container40ReeferDryUsage, Container40HighCubeReeferDryUsage, Container45ReeferDryUsage, Container20Platform }
        /*
         * **Only one container type per rating request is currently supported.
         * */

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
        public string grant_type { get; set; }
        public CHRTokenData getToken(string sclient_id = "0oa9kmcecjIeC1Rgn357", string sclient_secret = "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", string saudience = "https://inavisphere.chrobinson.com", string sgrant_type = "client_credentials")
        {
            CHRTokenResponse oRet = new CHRTokenResponse();
            // object tmp = null;
            try
            {
                //// For testing purposes only added on 24-09-2025
                //return null;

                // Commented out for testing purposes only added on 24-09-2025
                var client = new RestClient("https://sandbox-api.navisphere.com/v1/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("client_id", sclient_id, ParameterType.GetOrPost);
                request.AddParameter("client_secret", sclient_secret, ParameterType.GetOrPost);
                request.AddParameter("audience", saudience, ParameterType.GetOrPost);
                request.AddParameter("grant_type", sgrant_type, ParameterType.GetOrPost);
                //var response = client.Get<MarketplaceSearchResponse>(request);
                oRet.response = client.Execute(request);
                //List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(oRet.response.Content).ToList();
                CHRTokenData oTokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<CHRTokenData>(oRet.response.Content);
                return oTokenData;

                // Old Code

                //var result = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(oRet.response.Content);
                //System.Diagnostics.Debug.WriteLine(oRet.ToString());
                //var sToken = result.access_token;
                //if (result.Count > 0)
                //{
                //    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(result[0].ToString());
                //    if (values == null)
                //    {
                //        oRet.success = false;
                //    }
                //    oRet.tokenresponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CHRGetTokenResponse>(values["content"].ToString());
                //    //Newtonsoft.Json.JsonConvert.DeserializeObject<CHRGetTokenResponse>(values["content"].ToString()); values["access_token"].ToString();
                //    //            oRet.expires_in = values["expires_in"].ToString();
                //    //            oRet.token_type = values["token_type"].ToString();

                //    //lstPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<TrimbleServiceReference.Coordinates>(values["Coords"].ToString());
                //    //            if (lstPoints != null)
                //    //                sResults = lstPoints.Lat + "," + lstPoints.Lon;
                //    //            else
                //    //                return -1;

                //}
                //else
                //{
                //    oRet.success = false;
                //}
                // tmp = result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        public string getTestOrigin()
        {
            CHRSpecialRequirement oOrigSpecialReq = new CHRSpecialRequirement()
            {
                liftGate = "false",
                insidePickup = "false",
                insideDelivery = "false",
                residentialNonCommercial = "false",
                limitedAccess = "false",
                tradeShoworConvention = "false",
                constructionSite = "false",
                dropOffAtCarrierTerminal = "false",
                pickupAtCarrierTerminal = "false"
            };
            CHRReferenceNumbers[] oOrigRefs = new CHRReferenceNumbers[1];
            oOrigRefs[0] = new CHRReferenceNumbers { type = "PU", value = "PickUpNumber1" };


            CHRAddress oOrigin = new CHRAddress()
            {
                locationName = "Origin Location",
                address1 = "14800 Charlson Rd",
                //not required address2 = "Building 1",
                //not required address3 = "Room 212",
                city = "Eden Prairie",
                stateProvinceCode = "MN",
                countryCode = "US",
                postalCode = "55347",
                //not required latitude = 31.717096,
                //not required longitude = -99.132553,
                specialRequirement = oOrigSpecialReq,
                //not required isPort = "false",
                //not required unLocode = "US AC8",
                //not required iata = "ACB",
                customerLocationId = "W541849",
                referenceNumbers = oOrigRefs

            };
            string jOrigin = JsonConvert.SerializeObject(oOrigin);
            return jOrigin;
        }

        public string getTestDestination()
        {
            CHRSpecialRequirement oSpecialReq = new CHRSpecialRequirement()
            {
                liftGate = "false",
                insidePickup = "false",
                insideDelivery = "false",
                residentialNonCommercial = "false",
                limitedAccess = "false",
                tradeShoworConvention = "false",
                constructionSite = "false",
                dropOffAtCarrierTerminal = "false",
                pickupAtCarrierTerminal = "false"
            };
            CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[1];
            oRefs[0] = new CHRReferenceNumbers { type = "DEL", value = "DeliverNumber1" };


            CHRAddress oAddress = new CHRAddress()
            {
                locationName = "Destination Location",
                address1 = "800 Washington Avenue North",
                //not required address2 = "Building 1",
                //not required address3 = "Suite 550",
                city = "Minneapolis",
                stateProvinceCode = "MN",
                countryCode = "US",
                postalCode = "55401",
                //not required latitude = 44.989386,
                //not required longitude = -93.278626,
                specialRequirement = oSpecialReq,
                //not required isPort = "false",
                //not required unLocode = "US AC8",
                //not required iata = "ACB",
                customerLocationId = "W541849",
                referenceNumbers = oRefs

            };
            string jOrigin = JsonConvert.SerializeObject(oAddress);
            return jOrigin;
        }

        public string getTestMode(string sType, int iQty, string sMode)
        {

            CHREquipment[] oEquipments = new CHREquipment[1];
            oEquipments[0] = new CHREquipment()
            {
                equipmentType = sType,
                quantity = iQty
            };

            CHRTransportMode oMode = new CHRTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
            string jMode = JsonConvert.SerializeObject(oMode);
            return jMode;
        }

        public string getTestRateReferenceNumbers()
        {

            CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[1];
            oRefs[0] = new CHRReferenceNumbers { type = "MBOL", value = "MBOL12345" };
            string jRefs = JsonConvert.SerializeObject(oRefs);
            return jRefs;
        }



        public string getTestItems()
        {
            CHRItem[] aItems = new CHRItem[1];



            aItems[0] = new CHRItem
            {
                description = "widgets",
                freightClass = 400,
                actualWeight = 250,
                weightUnit = "Pounds",
                length = 24,
                width = 10,
                height = 10,
                linearUnit = "Inches",
                pallets = 1,
                pieces = 1,
                palletSpaces = 1,
                packagingCode = "BIN",
                productName = "widgets",
                temperatureSensitive = "Dry",
                temperatureUnit = "Fahrenheit",
                requiredTemperatureHigh = 85,
                requiredTemperatureLow = 35,
                isStackable = "true",
                isOverWeightOverDimensional = "false",
                isUsedGood = "false",
                isHazardous = "false"
            };

            //aItems[0] = new CHRItem
            //{
            //    description = "misc products",
            //    freightClass = 50,
            //    actualWeight = 33000,
            //    weightUnit = "Pounds",
            //    length = 42,
            //    width = 42,
            //    height = 48,
            //    linearUnit = "Inches",
            //    pallets = 24,
            //    pieces = 24,
            //    palletSpaces = 24,
            //    declaredValue = 1000,
            //    packagingCode = "PLT",
            //    productCode = "wdgt",
            //    productName = "goods",
            //    temperatureSensitive = "Dry",
            //    temperatureUnit = "Fahrenheit",
            //    requiredTemperatureHigh = 85,
            //    requiredTemperatureLow = 35,
            //    isStackable = "false",
            //    isOverWeightOverDimensional = "false",
            //    isUsedGood = "false",
            //    isHazardous = "false"
            //};

            //aItems[0] = new CHRItem
            //{
            //    description = "widgets",
            //    freightClass = 50,
            //    actualWeight = 1000,
            //    weightUnit = "Pounds",
            //    length = 40,
            //    width = 40,
            //    height = 40,
            //    linearUnit = "Inches",
            //    pallets = 1,
            //    pieces = 1,
            //    palletSpaces = 1,
            //    packagingCode = "BIN",
            //    productName = "widgets",
            //    temperatureSensitive = "Dry",
            //    temperatureUnit = "Fahrenheit",
            //    requiredTemperatureHigh = 85,
            //    requiredTemperatureLow = 35,
            //    isStackable = "true",
            //    isOverWeightOverDimensional = "false",
            //    isUsedGood = "false",
            //    isHazardous = "false"
            //};

            CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[5];
            oRefs[0] = new CHRReferenceNumbers { type = "SHID", value = "CNS-10-201" };
            oRefs[1] = new CHRReferenceNumbers { type = "CON", value = "SO-HLB-29030-1120" };
            oRefs[2] = new CHRReferenceNumbers { type = "CRID", value = "HBS-10-57" };
            oRefs[3] = new CHRReferenceNumbers { type = "PU", value = "CNS-10-201" };
            oRefs[4] = new CHRReferenceNumbers { type = "DEL", value = "SO-HLB-29030-1120" };
            aItems[0].referenceNumbers = oRefs;
            string jItems = JsonConvert.SerializeObject(aItems);
            return jItems;
        }

        //public bool getRateRequest(string sToken)
        //{
        //    var client = new RestClient("https://sandbox-api.navisphere.com/v1/quotes");
        //    var request = new RestRequest(Method.POST);
        //   // request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization","Bearer " + sToken);
        //    request.AddHeader("Content-Type", "application/json");
        //    //request.AddHeader("Authorization", sToken);
        //    //request.AddParameter("client_id", "0oa9kmcecjIeC1Rgn357", ParameterType.GetOrPost);
        //    // request.AddParameter("client_secret", "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", ParameterType.GetOrPost);
        //   // request.AddParameter("audience", "https://inavisphere.chrobinson.com", ParameterType.GetOrPost);
        //   // request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);          

        //    string sRoute = "{ \"items\": " + getItems() + ",\"origin\":" + getOrigin() + ",\"destination\":" + getDestination() + ",\"shipDate\": \"2022-01-20T20:30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [" + getMode("Van",1,"LTL") + "],\"referenceNumbers\": " + getRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";

        //    // string sRoute = "{ \"items\": [{}],\"origin\": {},\"destination\": {},\"shipDate\": \"2022-02-20T20:30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [{}],\"referenceNumbers\": {}, \"optionalAccessorials\": [\"APT\" ]}";
        //    //string sRoute = "{ \"items\": [{}],\"origin\": {},\"destination\": {}}";
        //    var data = Encoding.ASCII.GetBytes(sRoute);           
        //    //System.Diagnostics.Debug.WriteLine(sRoute);
        //    request.AddHeader("Content-Length", data.Length.ToString());
        //    //request.AddParameter("undefined", sRoute, ParameterType.RequestBody);
        //    request.AddParameter("undefined", sRoute, "application/json", ParameterType.RequestBody);

        //    //request.AddParameter("sample", "{}", ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    return true;

        //}
        //public bool getRateRequestTest(string sToken)
        //{
        //    var client = new RestClient("https://sandbox-api.navisphere.com/v1/quotes");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Authorization", "Bearer " + sToken);
        //    request.AddHeader("Content-Type", "application/json");
        //    CHREquipment[] oEquipments = new CHREquipment[1];
        //    oEquipments[0] = new CHREquipment()
        //    {
        //        equipmentType = "Van",
        //        quantity = 1
        //    };

        //    CHRTransportMode oMode = new CHRTransportMode()
        //    {
        //        mode = "LTL",
        //        equipments = oEquipments
        //    };
        //    string jMode = JsonConvert.SerializeObject(oMode);

        //    CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[1];
        //    oRefs[0] = new CHRReferenceNumbers { type = "MBOL", value = "MBOL12345" };
        //    string jRefs = JsonConvert.SerializeObject(oRefs);

        //    string sRoute = "{ \"items\": [{\"description\":\"widgets\",\"freightClass\":400,\"actualWeight\":250.0,\"weightUnit\":\"Pounds\",\"length\":24.0,\"width\":10.0,\"height\":10.0,\"linearUnit\":\"Inches\",\"pallets\":1,\"pieces\":1,\"palletSpaces\":1,\"volume\":3.0,\"volumeUnit\":\"CubicFeet\",\"density\":25.0,\"linearSpace\":8.0,\"declaredValue\":50000.0,\"packagingCode\":\"BIN\",\"productCode\":\"wdgt\",\"productName\":\"widgets\",\"temperatureSensitive\":\"Dry\",\"temperatureUnit\":\"Fahrenheit\",\"requiredTemperatureHigh\":85.0,\"requiredTemperatureLow\":35.0,\"unitsPerPallet\":36,\"unitWeight\":14.0,\"unitVolume\":3.0,\"isStackable\":\"true\",\"isOverWeightOverDimensional\":\"false\",\"isUsedGood\":\"false\",\"isHazardous\":\"false\",\"hazardousDescription\":\"Car Battery\",\"hazardousEmergencyPhone\":\"5555555555\",\"nmfc\":\"156600\",\"upc\":\"1234567890\",\"sku\":\"01234 - 001 - F10 - 6\",\"plu\":\"4026\",\"referenceNumbers\":[{\"type\":\"PO\",\"value\":\"PO12345\"}]}],\"origin\":{\"locationName\":\"Origin Location\",\"address1\":\"14800 Charlson Rd\",\"address2\":\"Building 1\",\"address3\":\"Room 212\",\"city\":\"Eden Prairie\",\"stateProvinceCode\":\"MN\",\"countryCode\":\"US\",\"postalCode\":\"55347\",\"latitude\":31.717096,\"longitude\":-99.132553,\"specialRequirement\":{\"liftGate\":\"false\",\"insidePickup\":\"false\",\"insideDelivery\":\"false\",\"residentialNonCommercial\":\"false\",\"limitedAccess\":\"false\",\"tradeShoworConvention\":\"false\",\"constructionSite\":\"false\",\"dropOffAtCarrierTerminal\":\"false\",\"pickupAtCarrierTerminal\":\"false\"},\"isPort\":\"false\",\"unLocode\":\"US AC8\",\"iata\":\"ACB\",\"customerLocationId\":\"W541849\",\"referenceNumbers\":[{\"type\":\"PU\",\"value\":\"PickUpNumber1\"}]},\"destination\":{\"locationName\":\"Destination Location\",\"address1\":\"800 Washington Avenue North\",\"address2\":\"Building 1\",\"address3\":\"Suite 550\",\"city\":\"Minneapolis\",\"stateProvinceCode\":\"MN\",\"countryCode\":\"US\",\"postalCode\":\"55401\",\"latitude\":44.989386,\"longitude\":-93.278626,\"specialRequirement\":{\"liftGate\":\"false\",\"insidePickup\":\"false\",\"insideDelivery\":\"false\",\"residentialNonCommercial\":\"false\",\"limitedAccess\":\"false\",\"tradeShoworConvention\":\"false\",\"constructionSite\":\"false\",\"dropOffAtCarrierTerminal\":\"false\",\"pickupAtCarrierTerminal\":\"false\"},\"isPort\":\"false\",\"unLocode\":\"US AC8\",\"iata\":\"ACB\",\"customerLocationId\":\"W541849\",\"referenceNumbers\":[{\"type\":\"DEL\",\"value\":\"DeliverNumber1\"}]},\"shipDate\": \"2022 - 02 - 20T20: 30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [{\"mode\":\"LTL\",\"equipments\":[{\"equipmentType\":\"Van\",\"quantity\":1}]}],\"referenceNumbers\": [{\"type\":\"MBOL\",\"value\":\"MBOL12345\"}], \"optionalAccessorials\": [\"APT\" ]}";

        //    request.AddParameter("undefined", sRoute, ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    return true;

        //}



        public CHRQuoteResponse getTestHTTPRateRequest(string sToken, string sCCode)
        {
            DateTime dShipDate = DateTime.Now.AddDays(5);

            string sShipDate = string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dShipDate.ToUniversalTime());


            var request = (HttpWebRequest)WebRequest.Create("https://sandbox-api.navisphere.com/v1/quotes");
            string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\":\"" + sShipDate + "\", \"customerCode\": \"" + sCCode + "\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\": \"2022-03-20T20:30:00.0000000Z\", \"customerCode\": \"C6953660\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //C6953660
            var data = Encoding.ASCII.GetBytes(postData);
            CHRQuoteResponse oData = new CHRQuoteResponse();
            request.Headers.Add("Authorization", "Bearer " + sToken);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {

                var response = (HttpWebResponse)request.GetResponse();
                var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
                string sRet = responseJSON.ToString();
                oData = Newtonsoft.Json.JsonConvert.DeserializeObject<CHRQuoteResponse>(sRet);

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream eData = response.GetResponseStream())
                    using (var reader = new StreamReader(eData))
                    {
                        oData = new CHRQuoteResponse { postMessagesOnly = true };
                        string sTxtMsg = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sTxtMsg))
                        {
                            oData.AddMessage(CHRAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your CHR account manager.  The actual Error is: " + e.Message, "", "");
                        }
                        else
                        {
                            oData.AddMessage(CHRAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                        }

                    }
                }

            }
            return oData;
        }

        public CHRQuoteResponse getHTTPRateRequest(string sToken, RateRequest oRequest, string sDataURL)
        {
            CHRQuoteResponse oData = new CHRQuoteResponse();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(sDataURL);
                string postData = oRequest.postData();
                var data = Encoding.ASCII.GetBytes(postData);
                request.Headers.Add("Authorization", "Bearer " + sToken);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseJSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    string sRet = responseJSON.ToString();
                    oData = Newtonsoft.Json.JsonConvert.DeserializeObject<CHRQuoteResponse>(sRet);

                    if (oRequest.oFees != null && oData != null)
                    {
                        oData.oFees = oRequest.oFees;
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream eData = response.GetResponseStream())
                        using (var reader = new StreamReader(eData))
                        {
                            oData = new CHRQuoteResponse { postMessagesOnly = true };
                            string sTxtMsg = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(sTxtMsg))
                            {
                                oData.AddMessage(CHRAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your CHR account manager.  The actual Error is: " + e.Message, "", "");
                            }
                            else
                            {
                                oData.AddMessage(CHRAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oData.postMessagesOnly = true;
                oData.AddMessage(CHRAPI.MessageEnum.E_UnExpected, "CHR API Failed check your shipping information or contact CHR for more details", "", "");
            }
            return oData;
        }

    }

    //        {"quoteSummaries":[{"quoteId":2159912944,"customer":{},"totalCharge":599.35,"totalFreightCharge":499.46,"totalAccessorialCharge":99.89,"transit":{"minimumTransitDays":1,"maximumTransitDays":2},"rates":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}],"transportModeType":"LTL","equipmentType":"Van","quoteSource":"Contractual"}]}

    public class CHRMessage
    {
        public string Message { get; set; }
        public string MessageLocalCode { get; set; }
        public string VendorMessage { get; set; }
        public string VendorErrorCode { get; set; }
        public string FieldName { get; set; }
        public string Details { get; set; }
        public bool bLogged { get; set; } = false;


    }

    public class CHRQuoteResponse
    {
        public CHRQuoteSummary[] quoteSummaries { get; set; }
        public bool postMessagesOnly { get; set; } = false;
        private List<CHRMessage> messages { get; set; }

        public void AddMessage(CHRAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            CHRMessage msg = new CHRMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = CHRAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<CHRMessage>(); }
            messages.Add(msg);
        }

        public void AddMessage(CHRMessage msg)
        {
            if (messages == null) { messages = new List<CHRMessage>(); }
            msg.bLogged = false;
            messages.Add(msg);
        }

        public List<CHRMessage> GetMessages()
        {
            if (messages == null) { messages = new List<CHRMessage>(); }
            return messages;
        }

        public string concateMessages()
        {
            string sRet = "";
            if (messages != null && messages.Count() > 0)
            {
                foreach (CHRMessage m in messages)
                {
                    sRet += m.Message + ": " + m.Details + " ";
                }
            }

            return sRet;
        }
        /// <summary>
        /// Array of CHRFees used to Maps to BookFees
        /// </summary>
        public CHRFees[] oFees { get; set; }
    }
    public class CHRQuoteSummary
    {
        public Int64 quoteId { get; set; } //":2159912944
        public CHRCarrier carrier { get; set; }
        public CHRCustomer customer { get; set; }
        public double totalCharge { get; set; } //":599.35
        public double totalFreightCharge { get; set; } //":499.46
        public double totalAccessorialCharge { get; set; } //":99.89
        public CHRTransit transit { get; set; } //":{"minimumTransitDays":1,"maximumTransitDays":2},"
        public CHRRate[] rates { get; set; } //":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}]
        public string transportModeType { get; set; } //":"LTL"
        public string equipmentType { get; set; } //":"Van"
        public CHRCargoLiability cargoLiability { get; set; }
        public double distance { get; set; }
        //
        public string quoteSource { get; set; } //Contractual"
    }

    public class CHRCargoLiability
    {
        public double perPound { get; set; } //integer Carrier’s liability per pound based on product attributes.

        public double max { get; set; } //integer Carrier’s maximum liability based on product attributes.

        public double amount { get; set; } //integer Calculated liability coverage for specific quote based on product attributes.

        public string currencyCode { get; set; } //string (currencyCodeEnum)
    }

    public class CHRRate
    {
        public double rateId { get; set; } //number<float> ID of the rate that is provided on successfully calling the Rating API.

        public double totalRate { get; set; } //number <float> Currency amount of the total rate

        public double unitRate { get; set; } //number <float> Currency amount of the unit rate

        public double quantity { get; set; } //number <float> Defines how many units there are
        public string rateCode { get; set; }
        public string rateCodeValue { get; set; } //string (rateCodeEnum)
        public string currencyCode { get; set; } //string (currencyCodeEnum)
        public bool isOptional { get; set; } //Defines if the rate is optional. If the isOptional flag is set to false, the currency amount of the assecorial will be included in the totalCharge.
    }
    public class CHRTransit
    {
        public double minimumTransitDays { get; set; } // number<float> Minimum transit days.
        public double maximumTransitDays { get; set; } // number <float> Maximum transit days.
        public string minimumDeliveryDate { get; set; } // string <date-time> An ISO8601 UTC date-time used to indicate the minimum delivery date.
        public string maximumDeliveryDate { get; set; } //string <date-time> An ISO8601 UTC date-time used to indicate the maximum delivery date.
    }

    public class CHRCustomer
    {
        public string customerName { get; set; }
    }

    public class CHRCarrier
    {
        public string carrierName { get; set; }
        public string scac { get; set; }
    }

    public class CHRTokenResponse
    {
        public CHRTokenData tokenresponse { get; set; }

        public string error_code { get; set; }
        public string error_msg { get; set; }
        public bool success { get; set; }
        public IRestResponse response { get; set; }

    }

    public class CHRTokenData
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }



    public class CHRItem
    {
        public string description { get; set; } = "widgets";
        public int freightClass { get; set; } = 100;
        public int actualWeight { get; set; } // 250,
        public string weightUnit { get; set; } = "Pounds";
        public int length { get; set; } = 48; // 24,
        public int width { get; set; } = 40; // 10,
        public int height { get; set; } = 42; // 10,
        public string linearUnit { get; set; } = "Inches";
        public int pallets { get; set; } = 1;
        public int pieces { get; set; } = 1;
        public int palletSpaces { get; set; } = 1;
        //public int volume { get; set; } // 3,
        //public string volumeUnit { get; set; } // "CubicFeet",
        //public int density { get; set; } // 25,
        //public int linearSpace { get; set; } // 8,
        public int declaredValue { get; set; } = 50000;
        public string packagingCode { get; set; } = "PLT"; // "BIN",
        public string productCode { get; set; } = "wdgt";
        public string productName { get; set; } = "widgets";
        public string temperatureSensitive { get; set; } = "Dry";
        public string temperatureUnit { get; set; } = "Fahrenheit";
        public int requiredTemperatureHigh { get; set; } = 105;
        public int requiredTemperatureLow { get; set; } = 35;
        //public int unitsPerPallet { get; set; } // 36,
        //public int unitWeight { get; set; } // 14,
        //public int unitVolume { get; set; } // 3,
        public string isStackable { get; set; } = "false";
        public string isOverWeightOverDimensional { get; set; } = "false";
        public string isUsedGood { get; set; } = "false";
        public string isHazardous { get; set; } = "false";
        //public string hazardousDescription { get; set; } // "Car Battery",
        //public string hazardousEmergencyPhone { get; set; } // "5555555555",
        //public string nmfc { get; set; } // "156600",
        //public string upc { get; set; } // "1234567890",
        //public string sku { get; set; } // "01234-001-F10-6",
        //public string plu { get; set; } // "4026",
        //public int pickupSequenceNumber { get; set; }
        //public int dropSequenceNumber { get; set; }
        public CHRReferenceNumbers[] referenceNumbers { get; set; }

    }

    /// <summary>
    /// Accessorial Fee Mapping to BookFees
    /// </summary>
    public class CHRFees
    {
        public int BookAcssControl { get; set; }
        public int BookAcssNACControl { get; set; }
        public decimal BookAcssValue { get; set; }
        public string NACCode { get; set; }
        public string NACName { get; set; }
        public int AccessorialCode { get; set; }
        public string AccessorialName { get; set; }

    }

    public class CHRReferenceNumbers
    {
        public string type { get; set; } //"PO",
        public string value { get; set; } //"PO12345"

        public static void addReferenceNumber(CHRAPI.RefNumbers etype, string sVal, ref List<CHRReferenceNumbers> lref)
        {
            if (lref == null)
            {
                lref = new List<CHRReferenceNumbers>();
            }
            lref.Add(new CHRReferenceNumbers() { type = etype.ToString(), value = sVal });
        }

        public static void addReferenceNumber(CHRAPI.RefNumbers etype, string sVal, ref CHRReferenceNumbers[] referenceNumbers)
        {
            List<CHRReferenceNumbers> lref = new List<CHRReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(new CHRReferenceNumbers() { type = etype.ToString(), value = sVal });

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumber(CHRReferenceNumbers oRefNbr, ref CHRReferenceNumbers[] referenceNumbers)
        {
            List<CHRReferenceNumbers> lref = new List<CHRReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(oRefNbr);

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumbers(List<CHRReferenceNumbers> lref, ref CHRReferenceNumbers[] referenceNumbers)
        {
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref.AddRange(referenceNumbers.ToList());
            }

            referenceNumbers = lref.ToArray();
        }

        public static void repklaceReferenceNumbers(List<CHRReferenceNumbers> lref, ref CHRReferenceNumbers[] referenceNumbers)
        {
            referenceNumbers = lref.ToArray();
        }
    }

    public class CHRAddress
    {
        //Note: non-requried properties have been removed to meet CHR non-empty data requiremnts
        public string locationName { get; set; } // "Origin Location",
        public string address1 { get; set; } // "14800 Charlson Rd",
                                             //public string address2 { get; set; } // "Building 1",
                                             //public string address3 { get; set; } // "Room 212",
        public string city { get; set; } // "Eden Prairie",
        public string stateProvinceCode { get; set; } // "MN",
        public string countryCode { get; set; } // "US",
        public string postalCode { get; set; } // "55347",
                                               //public double latitude { get; set; } //31.717096,
                                               //public double longitude { get; set; } //-99.132553,
        public CHRSpecialRequirement specialRequirement { get; set; }
        //public string isPort { get; set; } // "false",
        //public string unLocode { get; set; } // "US AC8",
        //public string iata { get; set; } // "ACB",
        public string customerLocationId { get; set; } // "W541849",
        public CHRReferenceNumbers[] referenceNumbers { get; set; }

    }

    public class CHRSpecialRequirement
    {
        public string liftGate { get; set; } = "false";
        public string insidePickup { get; set; } = "false";
        public string insideDelivery { get; set; } = "false";
        public string residentialNonCommercial { get; set; } = "false";
        public string limitedAccess { get; set; } = "false";
        public string tradeShoworConvention { get; set; } = "false";
        public string constructionSite { get; set; } = "false";
        public string dropOffAtCarrierTerminal { get; set; } = "false";
        public string pickupAtCarrierTerminal { get; set; } = "false";


        public string getSpecialRequirementsString()
        {
            StringBuilder sbRet = new StringBuilder();

            sbRet.AppendFormat("\"liftGate\":\"{0}\",", this.liftGate.ToLower());
            sbRet.AppendFormat("\"insidePickup\":\"{0}\",", this.insidePickup.ToLower());
            sbRet.AppendFormat("\"insideDelivery\":\"{0}\",", this.insideDelivery.ToLower());
            sbRet.AppendFormat("\"limitedAccess\":\"{0}\",", this.limitedAccess.ToLower());
            sbRet.AppendFormat("\"tradeShoworConvention\":\"{0}\",", this.tradeShoworConvention.ToLower());
            sbRet.AppendFormat("\"constructionSite\":\"{0}\",", this.constructionSite.ToLower());
            sbRet.AppendFormat("\"dropOffAtCarrierTerminal\":\"{0}\",", this.dropOffAtCarrierTerminal.ToLower());
            sbRet.AppendFormat("\"pickupAtCarrierTerminal\":\"{0}\"", this.pickupAtCarrierTerminal.ToLower());

            return sbRet.ToString();
        }


        public void setSpecialRequrement(CHRAPI.SpecialRequirement eReq)
        {
            switch (eReq)
            {
                case CHRAPI.SpecialRequirement.liftGate:
                    this.liftGate = "true";
                    break;
                case CHRAPI.SpecialRequirement.insidePickup:
                    this.insidePickup = "true";
                    break;
                case CHRAPI.SpecialRequirement.insideDelivery:
                    this.insideDelivery = "true";
                    break;
                case CHRAPI.SpecialRequirement.residentialNonCommercial:
                    this.residentialNonCommercial = "true";
                    break;
                case CHRAPI.SpecialRequirement.limitedAccess:
                    this.limitedAccess = "true";
                    break;
                case CHRAPI.SpecialRequirement.tradeShoworConvention:
                    this.tradeShoworConvention = "true";
                    break;
                case CHRAPI.SpecialRequirement.constructionSite:
                    this.constructionSite = "true";
                    break;
                case CHRAPI.SpecialRequirement.dropOffAtCarrierTerminal:
                    this.dropOffAtCarrierTerminal = "true";
                    break;
                case CHRAPI.SpecialRequirement.pickupAtCarrierTerminal:
                    this.pickupAtCarrierTerminal = "true";
                    break;
                default:
                    break;
            }


        }

    }

    public class CHRTransportMode
    {
        public string mode { get; set; } = "TL";
        public CHREquipment[] equipments { get; set; }

    }

    public class CHREquipment
    {
        public string equipmentType { get; set; } = "Van";
        public int quantity { get; set; } = 1; //1
    }


    /// <summary>
    /// Configure the Load to be shipped.  then call CHRAPI.getHTTPRateRequest with a valid token and a RateRequest Object
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.5.1.002 on 01/31/2022
    ///     Instructions:
    ///     populate the following properties
    ///     oAccessorials
    ///     sSpecial
    ///     oOrigin
    ///     lStops (one stop for now)
    ///     lItems
    ///     customerCode
    ///     declaredValue (value of items)
    ///     oRefs
    ///  Methods
    ///     use setShipDate to populate sShipDate
    ///     use setMode to populate oMode
    ///     
    /// </remarks>
    public class RateRequest
    {

        public CHRTransportMode tMode { get; set; } = new CHRTransportMode();
        public CHRSpecialRequirement sSpecial { get; set; }

        //note: caller must populate  CHRSpecialRequirement oOrigSpecialReq = new CHRSpecialRequirement()
        //        {
        //            liftGate = "false",
        //            insidePickup = "false",
        //            insideDelivery = "false",
        //            residentialNonCommercial = "false",
        //            limitedAccess = "false",
        //            tradeShoworConvention = "false",
        //            constructionSite = "false",
        //            dropOffAtCarrierTerminal = "false",
        //            pickupAtCarrierTerminal = "false"
        //        };
        //CHRReferenceNumbers[] oOrigRefs = new CHRReferenceNumbers[1];
        //oOrigRefs[0] = new CHRReferenceNumbers { type = "PU", value = "PickUpNumber1" };
        public CHRAddress oOrigin { get; set; }

        public List<CHRAddress> lStops { get; set; }

        //note: caller must populate  Item CHRReferenceNumbers[] oRefs 
        // example oRefs[0] = new CHRReferenceNumbers { type = "PO", value = "PO12345" };
        public List<CHRItem> lItems { get; set; }

        public string sShipDate { get; set; }
        private DateTime? dtNGLShipDate { get; set; }
        public void setShipDate(DateTime dShipDate)
        {
            dtNGLShipDate = dShipDate;
            sShipDate = convertDatetoUTCWebFormat(dShipDate);

        }

        public DateTime? getNGLShipDate()
        {
            return dtNGLShipDate;
        }


        public string customerCode { get; set; }  //\": \"C377465\",\"
        public double declaredValue { get; set; } //\": 50000

        public CHRTransportMode oMode { get; set; }

        public CHRReferenceNumbers[] oRefs { get; set; }

        /// <summary>
        /// String Array of NAC Accessorial Codes Used by TMS mapping
        /// </summary>
        public string[] oAccessorials { get; set; }

        /// <summary>
        /// Array of CHRFees used to Maps to BookFees
        /// </summary>
        public CHRFees[] oFees { get; set; }
        public string postData()
        {
            string sReturn = "";

            if (oAccessorials != null && oAccessorials.Count() > 0)
            {
                sReturn = "{ \"items\": " + getItems() + ",\"origin\":" + getOrigin() + ",\"destination\":" + getDestination() + ",\"shipDate\": \"" + sShipDate + "\", \"customerCode\": \"" + customerCode + "\",\"declaredValue\": " + declaredValue.ToString() + ",\"transportModes\": [" + getMode() + "],\"referenceNumbers\": " + getRateReferenceNumbers() + ", \"optionalAccessorials\": " + getAccessorials() + "}";

            }
            else
            {
                sReturn = "{ \"items\": " + getItems() + ",\"origin\":" + getOrigin() + ",\"destination\":" + getDestination() + ",\"shipDate\": \"" + sShipDate + "\", \"customerCode\": \"" + customerCode + "\",\"declaredValue\": " + declaredValue.ToString() + ",\"transportModes\": [" + getMode() + "],\"referenceNumbers\": " + getRateReferenceNumbers() + "}";

            }

            return sReturn;
        }

        public string convertDatetoUTCWebFormat(DateTime dtVal)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dtVal.ToUniversalTime());
        }

        public string getItems()
        {
            string jItems = JsonConvert.SerializeObject(lItems.ToArray());
            return jItems;
        }


        public string getOrigin()
        {

            string jOrigin = JsonConvert.SerializeObject(oOrigin);
            return jOrigin;
        }

        public string getDestination()
        {
            // for now we only allow single stop
            CHRAddress oDest = lStops[0];
            string jDest = JsonConvert.SerializeObject(oDest);
            return jDest;
        }

        public void setMode(string sType, int iQty, string sMode)
        {
            CHREquipment[] oEquipments = new CHREquipment[1];
            oEquipments[0] = new CHREquipment()
            {
                equipmentType = sType,
                quantity = iQty
            };

            this.oMode = new CHRTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
        }

        public void setMode(CHRAPI.EquipType eEquipType, int iEquipQty, CHRAPI.TransMode eMode)
        {


            CHREquipment[] oEquipments = new CHREquipment[1];
            oEquipments[0] = new CHREquipment()
            {
                equipmentType = eEquipType.ToString(),
                quantity = iEquipQty
            };

            oMode = new CHRTransportMode()
            {
                mode = eMode.ToString(),
                equipments = oEquipments
            };
        }

        public string getMode()
        {
            string jMode = JsonConvert.SerializeObject(oMode);
            return jMode;
        }



        public string getRateReferenceNumbers()

        {
            string jRefs = "[{}]";
            if (oRefs != null && oRefs.Count() > 0)
            {
                jRefs = JsonConvert.SerializeObject(oRefs);
            }

            //CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[1];
            //oRefs[0] = new CHRReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }

        public string getAccessorials()

        {
            string jRefs = "[]";
            if (oAccessorials != null && oAccessorials.Count() > 0)
            {
                jRefs = JsonConvert.SerializeObject(oAccessorials);
            }

            //CHRReferenceNumbers[] oRefs = new CHRReferenceNumbers[1];
            //oRefs[0] = new CHRReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }


        private bool postMessagesOnly { get; set; } = false;
        private List<CHRMessage> messages { get; set; }

        public bool getPostMessageOnlyFlag()
        {
            return postMessagesOnly;
        }

        public void setPostMessageOnlyFlag(bool bval)
        {
            postMessagesOnly = bval;
        }

        public void AddMessage(CHRAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            CHRMessage msg = new CHRMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = CHRAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<CHRMessage>(); }
            messages.Add(msg);
        }

        public List<CHRMessage> GetMessages()
        {
            if (messages == null) { messages = new List<CHRMessage>(); }
            return messages;
        }


    }

}
