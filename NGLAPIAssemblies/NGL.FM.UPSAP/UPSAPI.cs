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

namespace NGL.FM.UPSAPI
{
    public class UPSAPI
    {



        public UPSAPI(bool bUseTLs12 = true)
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

        public static string getMessageNotLocalized(UPSAPI.MessageEnum key, string sDefault)
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
        Enum:"SHID" "MBOL" "PO" "BOL" "CFRM" "CRID" "CUSTPO" "DEL" "DIST" "FIN" "GL" "JNO" "PU" "RANumber" "RLPRO" "MREF" "Empty" "PRO" "CCN" "SBOL" "CON" "APPT" "UPSW_ID" "FDA" "IREF" "LIC" "ORD" "PLU" "SCAC" "SEAL" "SKU" "SREF" "STCC" "TEB" "VPO" "WB" "XXXX" "TRIA" "TRANS" "GCC" "SDATE" "CDATE" "ACCT" "CCTR" "IGM" "GIGM" "LIGM" "SMTP" "PERMIT" "RNO" "DVCN" "PRCL" "CPUC" "SHPMD" "AFR" "BUYID" "VNDID" "DIV" "DEPT" "POTYPE" "GRGI" "DUNS" "PALLETREF" "CITTN" "CIN" "SHIPWITH" "PN" "UPC" "PRCD" "VESSEL" "VOYAGE" "FN" "FEEDVESS" "FEEDVOY" "SHIPREF" "CBL" "SPAC" "POLN" "VPOLN" "EIN" "CNSG" "PROFITCTR" "MFGPART" "PACKSLIP" "THIRDPRTY" "DEFATTR" "FSTRK" "FOItemID" "POLineNum" "PARCELSPC" "CNS1" "CNS2" "CNS3" "CNS4" "CNS5" "CNS6" "CNS7" "TREF" "VREF" "UPRICE" "FWPARTY" "BYPARTY" "HBL" "CBP" "TRANSFER" "SO" "ORDTRK" "QUOTEID" "EQT" "BOOTH" "TS" "OH #" "CNTNR" "VAS" "Size" "Color" "Style" "HCERT" "B#" "ETMSID" "214-IMO" "SHPWeight" "SHPVolume" "SHPDims" "SERIALIZED" "SERIALNUM" "CPSIA" "EDIINVREF" "SQID" "QSeqNum" "OuterPkStu" "GLC" "COMP" "COST" "DTYGL" "DTYCC" "BRKGL" "BRKCC" "GPS" "IAC" "WHRCT" "ERGUID" "SONUM" "CCNUM" "PrimCCNUM" "PrevCCNUM" "CMRN" "TRANSSHIP" "PGIDATE" "AESFIL" "CUSTQUOTID" "QUOTETRACE" "ODATE"

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

        /***** Start UPS Enums or Enum Queries ***************************/


        public enum UPSServiceMode
        {
            UPSNextDayAir =  1,
            UPS2ndDayAir = 2,
            UPSGround = 3,
            UPSWorldwideExpress = 7,
            UPSWorldwideExpedited = 8,
            UPSStandard	 = 11,
            UPS3DaySelect = 12,
            UPSNextDayAirSaver  =  13,
            UPSNextDayAirEarly = 14	,
            UPSWorldwideExpressPlus = 54,
            UPS2ndDayAirAM =  59,
            UPSWorldwideSaver = 65

        }
        
        public static string getUPSServiceModeDesc(UPSServiceMode eMode)
        {
            string sRet = "UPS Ground";
            switch (eMode)
            {
                case UPSServiceMode.UPSNextDayAir:
                    sRet = "UPS Next Day Air"; break;
                case UPSServiceMode.UPS2ndDayAir:
                    sRet = "UPS 2nd Day Air"; break;
                case UPSServiceMode.UPSGround:
                    sRet = "UPS Ground"; break;
                case UPSServiceMode.UPSWorldwideExpress:
                    sRet = "UPS Worldwide Express"; break;
                case UPSServiceMode.UPSWorldwideExpedited:
                    sRet = "UPS Worldwide Expedited"; break;
                case UPSServiceMode.UPSStandard:
                    sRet = "UPS Standard"; break;
                case UPSServiceMode.UPS3DaySelect:
                    sRet = "UPS 3 Day Select"; break;
                case UPSServiceMode.UPSNextDayAirSaver:
                    sRet = "UPS Next Day Air Saver"; break;
                case UPSServiceMode.UPSNextDayAirEarly:
                    sRet = "UPS Next Day Air Early"; break;
                case UPSServiceMode.UPSWorldwideExpressPlus:
                    sRet = "UPS Worldwide Express Plus"; break;
                case UPSServiceMode.UPS2ndDayAirAM:
                    sRet = "UPS 2nd Day Air A.M."; break;
                case UPSServiceMode.UPSWorldwideSaver:
                    sRet = "UPS Worldwide Saver"; break;

                default: break;
            }

                    return sRet;
        }

        public static string getDTMSServiceCode(UPSServiceMode eMode)
        {
            string sRet = "Ground";
            switch (eMode)
            {
                case UPSServiceMode.UPSNextDayAir:
                    sRet = "Next Day"; break;
                case UPSServiceMode.UPS2ndDayAir:
                    sRet = "2nd Day"; break;
                case UPSServiceMode.UPSGround:
                    sRet = "Ground"; break;
                case UPSServiceMode.UPSWorldwideExpress:
                    sRet = "Global Express"; break;
                case UPSServiceMode.UPSWorldwideExpedited:
                    sRet = "Global Expedited"; break;
                case UPSServiceMode.UPSStandard:
                    sRet = "Standard"; break;
                case UPSServiceMode.UPS3DaySelect:
                    sRet = "3 Day"; break;
                case UPSServiceMode.UPSNextDayAirSaver:
                    sRet = "Next Day"; break;
                case UPSServiceMode.UPSNextDayAirEarly:
                    sRet = "Next Day Early"; break;
                case UPSServiceMode.UPSWorldwideExpressPlus:
                    sRet = "Express Plus"; break;
                case UPSServiceMode.UPS2ndDayAirAM:
                    sRet = "2nd Day Early"; break;
                case UPSServiceMode.UPSWorldwideSaver:
                    sRet = "Global Saver"; break;

                default: break;
            }

            return sRet;
        }

        public static string getUPSServiceModeCode(UPSServiceMode eMode)
        {
            string sRet = "03";
            switch (eMode)
            {
                case UPSServiceMode.UPSNextDayAir:
                    sRet = "01"; break;
                case UPSServiceMode.UPS2ndDayAir:
                    sRet = "02"; break;
                case UPSServiceMode.UPSGround:
                    sRet = "03"; break;
                case UPSServiceMode.UPSWorldwideExpress:
                    sRet = "07"; break;
                case UPSServiceMode.UPSWorldwideExpedited:
                    sRet = "08"; break;
                case UPSServiceMode.UPSStandard:
                    sRet = "11"; break;
                case UPSServiceMode.UPS3DaySelect:
                    sRet = "12"; break;
                case UPSServiceMode.UPSNextDayAirSaver:
                    sRet = "13"; break;
                case UPSServiceMode.UPSNextDayAirEarly:
                    sRet = "14"; break;
                case UPSServiceMode.UPSWorldwideExpressPlus:
                    sRet = "54"; break;
                case UPSServiceMode.UPS2ndDayAirAM:
                    sRet = "59"; break;
                case UPSServiceMode.UPSWorldwideSaver:
                    sRet = "65"; break;
                default: break;
            }


            return sRet;
        }

        public static UPSServiceMode getUPSServiceModeEnum(string sMode)
        {
            UPSServiceMode eRet = UPSServiceMode.UPSGround;
            switch (sMode)
            {
                case "01": eRet = UPSServiceMode.UPSNextDayAir; break;
                case "02": eRet = UPSServiceMode.UPS2ndDayAir; break;
                case "03": eRet = UPSServiceMode.UPSGround; break;
                case "07": eRet = UPSServiceMode.UPSWorldwideExpress; break;
                case "08": eRet = UPSServiceMode.UPSWorldwideExpedited; break;
                case "11": eRet = UPSServiceMode.UPSStandard; break;
                case "12": eRet = UPSServiceMode.UPS3DaySelect; break;
                case "13": eRet = UPSServiceMode.UPSNextDayAirSaver; break;
                case "14": eRet = UPSServiceMode.UPSNextDayAirEarly; break; 
                case "54": eRet = UPSServiceMode.UPSWorldwideExpressPlus; break;
                case "59": eRet = UPSServiceMode.UPS2ndDayAirAM; break;
                case "65": eRet = UPSServiceMode.UPSWorldwideSaver; break;
                default: break;
            }

            return eRet;
        }

        public static string getUPSServiceModeDesc(string sMode)
        {            
            return getUPSServiceModeDesc(getUPSServiceModeEnum(sMode));
        }

        public static double  getUPSTransitDaysByServiceMode(string sMode)
        {
            double dRet = 5;
           
            switch (sMode)
            {
                case "01": dRet = 1; break; // UPSServiceMode.UPSNextDayAir; break;
                case "02": dRet = 2; break; //UPSServiceMode.UPS2ndDayAir; break;
                case "03": dRet = 5; break; //UPSServiceMode.UPSGround; break;
                case "07": dRet = 10; break; //UPSServiceMode.UPSWorldwideExpress; break;
                case "08": dRet = 5; break; //UPSServiceMode.UPSWorldwideExpedited; break;
                case "11": dRet = 5; break; //UPSServiceMode.UPSStandard; break;
                case "12": dRet = 3; break; //UPSServiceMode.UPS3DaySelect; break;
                case "13": dRet = 1; break; //UPSServiceMode.UPSNextDayAirSaver; break;
                case "14": dRet = 1; break; //UPSServiceMode.UPSNextDayAirEarly; break;
                case "54": dRet = 3; break; //UPSServiceMode.UPSWorldwideExpressPlus; break;
                case "59": dRet = 2; break; //UPSServiceMode.UPS2ndDayAirAM; break;
                case "65": dRet = 10; break; //UPSServiceMode.UPSWorldwideSaver; break;
                default: break;
            }

            return dRet;
        }

        public static string getUPSChargeCodeDesc(int iCode)
        {
            string sRet = "N/A";
            
            switch (iCode) 
            {
                case 100: sRet = "ADDITIONAL HANDLING";  break;
                case 110: sRet = "COD"; break;
                case 120: sRet = "DELIVERY CONFIRMATION";  break;
                case 121: sRet = "SHIP DELIVERY CONFIRMATION";  break;
                case 153: sRet = "PKG EMAIL SHIP NOTIFICATION";  break;
                case 154: sRet = "PKG EMAIL RETURN NOTIFICATION";  break;
                case 155: sRet = "PKG EMAIL INBOUND RETURN NOTIFICATION";  break;
                case 156: sRet = "PKG EMAIL QUANTUM VIEW SHIP NOTIFICATION";  break;
                case 157: sRet = "PKG EMAIL QUANTUM VIEW EXCEPTION NOTIFICATION";  break;
                case 158: sRet = "PKG EMAIL QUANTUM VIEW DELIVERY NOTIFICATION";  break;
                case 165: sRet = "PKG FAX INBOUND RETURN NOTIFICATION";  break;
                case 166: sRet = "PKG FAX QUANTUM VIEW SHIP NOTIFICATION";  break;
                case 171: sRet = "SHIP EMAIL ERL NOTIFICATION"; break;
                case 173: sRet = "SHIP EMAIL SHIP NOTIFICATION";  break;
                case 174: sRet = "SHIP EMAIL RETURN NOTIFICATION";  break;
                case 175: sRet = "SHIP EMAIL INBOUND RETURN NOTIFICATION";  break;
                case 176: sRet = "SHIP EMAIL QUANTUM VIEW SHIP NOTIFICATION";  break;
                case 177: sRet = "SHIP EMAIL QUANTUM VIEW EXCEPTION NOTIFICATION";  break;
                case 178: sRet = "SHIP EMAIL QUANTUM VIEW DELIVERY NOTIFICATION";  break;
                case 179: sRet = "SHIP EMAIL QUANTUM VIEW NOTIFY";  break;
                case 187: sRet = "SHIP UPS ACCESS POINT NOTIFICATION";  break;
                case 188: sRet = "SHIP EEI FILING NOTIFICATION";  break;
                case 189: sRet = "SHIP UAP SHIPPER NOTIFICATION";  break;
                case 190: sRet = "EXTENDED AREA";  break;
                case 199: sRet = "HAZ MAT";  break;
                case 200: sRet = "DRY ICE";  break;
                case 201: sRet = "ISC SEEDS";  break;
                case 202: sRet = "ISC PERISHABLES";  break;
                case 203: sRet = "ISC TOBACCO";  break;
                case 204: sRet = "ISC PLANTS";  break;
                case 205: sRet = "ISC ALCOHOLIC BEVERAGES";  break;
                case 206: sRet = "ISC BIOLOGICAL SUBSTANCES";  break;
                case 207: sRet = "ISC SPECIAL EXCEPTIONS";  break;
                case 220: sRet = "HOLD FOR PICKUP";  break;
                case 240: sRet = "ORIGIN CERTIFICATE";  break;
                case 250: sRet = "PRINT RETURN LABEL";  break;
                case 258: sRet = "EXPORT LICENSE VERIFICATION";  break;
                case 260: sRet = "PRINT N MAIL";  break;
                case 270: sRet = "RESIDENTIAL ADDRESS";  break;
                case 280: sRet = "RETURN SERVICE 1ATTEMPT";  break;
                case 290: sRet = "RETURN SERVICE 3ATTEMPT";  break;
                case 300: sRet = "SATURDAY DELIVERY";  break;
                case 310: sRet = "SATURDAY INTERNATIONAL PROCESSING FEE";  break;
                case 350: sRet = "ELECTRONIC RETURN LABEL";  break;
                case 372: sRet = "QUANTUM VIEW NOTIFY DELIVERY";  break;
                case 374: sRet = "UPS PREPARED SED FORM";  break;
                case 375: sRet = "FUEL SURCHARGE";  break;
                case 376: sRet = "DELIVERY AREA";  break;
                case 377: sRet = "LARGE PACKAGE";  break;
                case 378: sRet = "SHIPPER PAYS DUTY TAX";  break;
                case 379: sRet = "SHIPPER PAYS DUTY TAX UNPAID";  break;
                case 380: sRet = "EXPRESS PLUS SURCHARGE";  break;
                case 400: sRet = "INSURANCE";  break;
                case 401: sRet = "SHIP ADDITIONAL HANDLING";  break;
                case 402: sRet = "SHIPPER RELEASE";  break;
                case 403: sRet = "CHECK TO SHIPPER";  break;
                case 404: sRet = "UPS PROACTIVE RESPONSE";  break;
                case 405: sRet = "GERMAN PICKUP";  break;
                case 406: sRet = "GERMAN ROAD TAX";  break;
                case 407: sRet = "EXTENDED AREA PICKUP";  break;
                case 410: sRet = "RETURN OF DOCUMENT";  break;
                case 430: sRet = "PEAK SEASON";  break;
                case 431: sRet = "PEAK SEASON SURCHARGE - LARGE PACKAGE";  break;
                case 432: sRet = "PEAK SEASON SURCHARGE - ADDITIONAL HANDLING"; break;
                case 440: sRet = "SHIP LARGE PACKAGE";  break;
                case 441: sRet = "CARBON NEUTRAL";  break;
                case 442: sRet = "PKG QV IN TRANSIT NOTIFICATION";  break;
                case 443: sRet = "SHIP QV IN TRANSIT NOTIFICATION";  break;
                case 444: sRet = "IMPORT CONTROL";  break;
                case 445: sRet = "COMMERCIAL INVOICE REMOVAL";  break;
                case 446: sRet = "IMPORT CONTROL ELECTRONIC LABEL";  break;
                case 447: sRet = "IMPORT CONTROL PRINT LABEL";  break;
                case 448: sRet = "IMPORT CONTROL PRINT AND MAIL LABEL";  break;
                case 449: sRet = "IMPORT CONTROL ONE PICK UP ATTEMPT LABEL";  break;
                case 450: sRet = "IMPORT CONTROL THREE PICK UP ATTEMPT LABEL";  break;
                case 452: sRet = "REFRIGERATION";  break;
                case 454: sRet = "PAC 1A BOX1";  break;
                case 455: sRet = "PAC 3A BOX1";  break;
                case 456: sRet = "PAC 1A BOX2";  break;
                case 457: sRet = "PAC 3A BOX2";  break;
                case 458: sRet = "PAC 1A BOX3";  break;
                case 459: sRet = "PAC 3A BOX3";  break;
                case 460: sRet = "PAC 1A BOX4";  break;
                case 461: sRet = "PAC 3A BOX4";  break;
                case 462: sRet = "PAC 1A BOX5";  break;
                case 463: sRet = "PAC 3A BOX5";  break;
                case 464: sRet = "EXCHANGE PRINT RETURN LABEL";  break;
                case 465: sRet = "EXCHANGE FORWARD";  break;
                case 466: sRet = "SHIP PREALERT NOTIFICATION";  break;
                case 470: sRet = "COMMITTED DELIVERY WINDOW";  break;
                case 480: sRet = "SECURITY SURCHARGE";  break;
                case 492: sRet = "CUSTOMER TRANSACTION FEE";  break;
                case 500: sRet = "SHIPMENT COD";  break;
                case 510: sRet = "LIFT GATE FOR PICKUP";  break;
                case 511: sRet = "LIFT GATE FOR DELIVERY";  break;
                case 512: sRet = "DROP OFF AT UPS FACILITY";  break;
                case 515: sRet = "UPS PREMIUM CARE";  break;
                case 520: sRet = "OVERSIZE PALLET";  break;
                case 524: sRet = "MI DUAL LABEL RETURN";  break;
                case 530: sRet = "FREIGHT DELIVERY SURCHARGE";  break;
                case 531: sRet = "FREIGHT PICKUP SURCHARGE";  break;
                case 540: sRet = "DIRECT TO RETAIL";  break;
                case 541: sRet = "DIRECT DELIVERY ONLY";  break;
                case 542: sRet = "DELIVER TO ADDRESSEE ONLY";  break;
                case 543: sRet = "DIRECT TO RETAIL COD";  break;
                case 544: sRet = "RETAIL ACCESS POINT";  break;
                case 545: sRet = "SHIPPING TICKET NOTIFICATION";  break;
                case 546: sRet = "ELECTRONIC PACKAGE RELEASE AUTHENTICATION";  break;
                case 547: sRet = "PAY AT STORE";  break;
                case 548: sRet = "ICOD NOTIFICATION";  break;
                case 550: sRet = "ITEM DISPOSAL";  break;
                case 551: sRet = "UK BORDER FEE";  break;
                case 552: sRet = "MASTER CARTON";  break;
                case 553: sRet = "SIMPLE RATE ACCESSORIAL";  break;
                case 555: sRet = "UPS PREMIER GOLD";  break;
                case 556: sRet = "UPS PREMIER SILVER";  break;
                case 557: sRet = "UPS PREMIER PLATINUM";  break;
                case 558: sRet = "DDU OVERSIZE";  break;
                default:  break;
            }

            return sRet;
         }

        public static string getUPSChargeCodeEDICode(int iCode)
        {
            string sRet = "N/A";

            switch (iCode)
            {
                case 100: sRet = "HCD"; break; // ADDITIONAL HANDLING maps to Dlvry High Cost  break;
                case 110: sRet = "COL"; break; // COD
                case 120: sRet = "NFY"; break; // DELIVERY CONFIRMATION maps to Notify Before Del
                case 121: sRet = "NFY"; break; //SHIP DELIVERY CONFIRMATION maps to Notify Before Del
                case 153: sRet = "NFY"; break; //PKG EMAIL SHIP NOTIFICATION" maps to Notify Before Del
                case 154: sRet = "NFY"; break; //PKG EMAIL RETURN NOTIFICATION" maps to Notify Before Del
                case 155: sRet = "NFY"; break; //PKG EMAIL INBOUND RETURN NOTIFICATION" maps to Notify Before Del
                case 156: sRet = "NFY"; break; //PKG EMAIL QUANTUM VIEW SHIP NOTIFICATION" maps to Notify Before Del
                case 157: sRet = "NFY"; break; //PKG EMAIL QUANTUM VIEW EXCEPTION NOTIFICATION" maps to Notify Before Del
                case 158: sRet = "NFY"; break; //PKG EMAIL QUANTUM VIEW DELIVERY NOTIFICATION" maps to Notify Before Del
                case 165: sRet = "NFY"; break; //PKG FAX INBOUND RETURN NOTIFICATION" maps to Notify Before Del
                case 166: sRet = "NFY"; break; //PKG FAX QUANTUM VIEW SHIP NOTIFICATION" maps to Notify Before Del
                case 171: sRet = "NFY"; break; //SHIP EMAIL ERL NOTIFICATION" maps to Notify Before Del
                case 173: sRet = "NFY"; break; //SHIP EMAIL SHIP NOTIFICATION" maps to Notify Before Del
                case 174: sRet = "NFY"; break; //SHIP EMAIL RETURN NOTIFICATION" maps to Notify Before Del
                case 175: sRet = "NFY"; break; //SHIP EMAIL INBOUND RETURN NOTIFICATION" maps to Notify Before Del
                case 176: sRet = "NFY"; break; //SHIP EMAIL QUANTUM VIEW SHIP NOTIFICATION" maps to Notify Before Del
                case 177: sRet = "NFY"; break; //SHIP EMAIL QUANTUM VIEW EXCEPTION NOTIFICATION" maps to Notify Before Del
                case 178: sRet = "NFY"; break; //SHIP EMAIL QUANTUM VIEW DELIVERY NOTIFICATION" maps to Notify Before Del
                case 179: sRet = "NFY"; break; //SHIP EMAIL QUANTUM VIEW NOTIFY" maps to Notify Before Del
                case 187: sRet = "NFY"; break; //SHIP UPS ACCESS POINT NOTIFICATION" maps to Notify Before Del
                case 188: sRet = "NFY"; break; //SHIP EEI FILING NOTIFICATION" maps to Notify Before Del
                case 189: sRet = "NFY"; break; //SHIP UAP SHIPPER NOTIFICATION" maps to Notify Before Del
                case 190: sRet = "EXD"; break; //EXTENDED AREA maps to Extended Delivery
                case 199: sRet = "HAZ"; break; //HAZ MAT maps to Hazmat
                case 200: sRet = "HCD"; break; //DRY ICE maps to Dlvry High Cost
                case 201: sRet = "HCD"; break; //ISC SEEDS maps to Dlvry High Cost
                case 202: sRet = "HCD"; break; //ISC PERISHABLES maps to Dlvry High Cost
                case 203: sRet = "HCD"; break; //ISC TOBACCO maps to Dlvry High Cost
                case 204: sRet = "HCD"; break; //ISC PLANTS maps to Dlvry High Cost
                case 205: sRet = "HCD"; break; //ISC ALCOHOLIC BEVERAGES maps to Dlvry High Cost
                case 206: sRet = "HCD"; break; //ISC BIOLOGICAL SUBSTANCES maps to Dlvry High Cost
                case 207: sRet = "HCD"; break; //ISC SPECIAL EXCEPTIONS maps to Dlvry High Cost
                case 220: sRet = "HCP"; break; //HOLD FOR PICKUP maps to Pick High Cost
                case 240: sRet = "HCP"; break; //ORIGIN CERTIFICATE maps to Pick High Cost
                case 250: sRet = "HCP"; break; //PRINT RETURN LABEL maps to Pick High Cost
                case 258: sRet = "HCP"; break; //EXPORT LICENSE VERIFICATION maps to Pick High Cost
                case 260: sRet = "HCP"; break; //PRINT N MAIL"; break maps to Pick High Cost
                case 270: sRet = "RES"; break; //RESIDENTIAL ADDRESS maps to Residential Delivery
                case 280: sRet = "RCL"; break; //RETURN SERVICE 1ATTEMPT maps to Redelivery Charges
                case 290: sRet = "RCL"; break; //RETURN SERVICE 3ATTEMPT maps to Redelivery Charges
                case 300: sRet = "HOU"; break; //SATURDAY DELIVERY maps to Weekend Delivery
                case 310: sRet = "HOU"; break; //SATURDAY INTERNATIONAL PROCESSING FEE maps to Weekend Delivery
                case 375: sRet = "FUE"; break; //FUEL SURCHARGE maps to Flat Fuel Fee
                case 376: sRet = "EXD"; break; //DELIVERY AREA map to Extended Delivery
                case 377: sRet = "EXL"; break; //LARGE PACKAGE maps to Excessive Length
                case 380: sRet = "EEF"; break; //EXPRESS PLUS SURCHARGE maps to EXPEDITE
                case 400: sRet = "INS"; break; //INSURANCE maps to Insurance Fees
                case 407: sRet = "HCP"; break; //EXTENDED AREA PICKUP maps to Pick High Cost
                case 430: sRet = "HCD"; break; //PEAK SEASON maps to Dlvry High Cost
                case 431: sRet = "HCD"; break; //PEAK SEASON SURCHARGE - LARGE PACKAGE maps to Dlvry High Cost
                case 432: sRet = "HCD"; break; //PEAK SEASON SURCHARGE - ADDITIONAL HANDLING maps to Dlvry High Cost
                case 440: sRet = "EXL"; break; //SHIP LARGE PACKAGE maps to Dlvry High Cost
                case 442: sRet = "NFY"; break; //PKG QV IN TRANSIT NOTIFICATION maps to Notify Before Del
                case 443: sRet = "NFY"; break; //SHIP QV IN TRANSIT NOTIFICATION  maps to Notify Before Del
                case 466: sRet = "NFY"; break; //SHIP PREALERT NOTIFICATION  maps to Notify Before Del
                case 500: sRet = "COL"; break; //SHIPMENT COD maps to COD
                case 510: sRet = "LFT"; break; //LIFT GATE FOR PICKUP maps to Lift Gate Del Charge
                case 511: sRet = "LFT"; break; //LIFT GATE FOR DELIVERY maps to Lift Gate Del Charge
                case 520: sRet = "EXL"; break; //OVERSIZE PALLET maps to Excessive Length
                case 530: sRet = "HCD"; break; //FREIGHT DELIVERY SURCHARGE maps to Dlvry High Cost
                case 531: sRet = "HCP"; break; //FREIGHT PICKUP SURCHARGE maps to Pick High Cost
                default: sRet = "MIS"; break; // maps to MISC 
            }

            return sRet;
        }
        /***** End UPS Enums ****************************/

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string audience { get; set; }
        public string grant_type { get; set; }
        public UPSTokenData getToken(string sclient_id = "0oa9kmcecjIeC1Rgn357", string sclient_secret = "Wy9DFX_AQRoVlC4rMIcqUdgVcFJ0SpgcvoRnolKY", string saudience = "https://inavisphere.UPSobinson.com", string sgrant_type = "client_credentials")
        {
            UPSTokenResponse oRet = new UPSTokenResponse();
            // object tmp = null;
            try
            {

                var client = new RestClient("https://sandbox-api.navisphere.com/v1/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("client_id", sclient_id, ParameterType.GetOrPost);
                request.AddParameter("client_secret", sclient_secret, ParameterType.GetOrPost);
                request.AddParameter("audience", saudience, ParameterType.GetOrPost);
                request.AddParameter("grant_type", sgrant_type, ParameterType.GetOrPost);
                //var response = client.Get<MarketplaceSearUPSesponse>(request);
                oRet.response = client.Execute(request);
                //List<object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(oRet.response.Content).ToList();
                UPSTokenData oTokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSTokenData>(oRet.response.Content);
                return oTokenData;
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
                //    oRet.tokenresponse = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSGetTokenResponse>(values["content"].ToString());
                //    //Newtonsoft.Json.JsonConvert.DeserializeObject<UPSGetTokenResponse>(values["content"].ToString()); values["access_token"].ToString();
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
            UPSSpecialRequirement oOrigSpecialReq = new UPSSpecialRequirement()
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
            UPSReferenceNumbers[] oOrigRefs = new UPSReferenceNumbers[1];
            oOrigRefs[0] = new UPSReferenceNumbers { type = "PU", value = "PickUpNumber1" };


            UPSAddress oOrigin = new UPSAddress()
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
            UPSSpecialRequirement oSpecialReq = new UPSSpecialRequirement()
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
            UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[1];
            oRefs[0] = new UPSReferenceNumbers { type = "DEL", value = "DeliverNumber1" };


            UPSAddress oAddress = new UPSAddress()
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

            UPSEquipment[] oEquipments = new UPSEquipment[1];
            oEquipments[0] = new UPSEquipment()
            {
                equipmentType = sType,
                quantity = iQty
            };

            UPSTransportMode oMode = new UPSTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
            string jMode = JsonConvert.SerializeObject(oMode);
            return jMode;
        }

        public string getTestRateReferenceNumbers()
        {

            UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[1];
            oRefs[0] = new UPSReferenceNumbers { type = "MBOL", value = "MBOL12345" };
            string jRefs = JsonConvert.SerializeObject(oRefs);
            return jRefs;
        }

        public string getTestShipmentJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"RateRequest\": {");
            sb.Append("\"Request\": {");
            sb.Append("\"SubVersion\": \"1703\",");
             sb.Append("\"TransactionReference\": {");
            sb.Append("\"CustomerContext\": \" \" } },");
            sb.Append("\"Shipment\": {");
            sb.Append("\"ShipmentRatingOptions\": {");
            sb.Append("\"UserLevelDiscountIndicator\": \"TRUE\"},");
            sb.Append("\"Shipper\": {");
            sb.Append("\"Name\": \"Brett Susick\",");
            sb.Append("\"ShipperNumber\": \" \",");
            sb.Append("\"Address\": {");
            sb.Append("\"AddressLine\": \"400 POYDRAS ST 10\",");
            sb.Append("\"City\": \"NEW ORLEANS\",");
            sb.Append("\"StateProvinceCode\": \"LA\",");
            sb.Append("\"PostalCode\": \"70130\",");
            sb.Append("\"CountryCode\": \"US\"}},");
            sb.Append("\"ShipTo\": {");
            sb.Append("\"Name\": \"Sarita Lynn\",");
            sb.Append("\"Address\": {");
            sb.Append("\"AddressLine\": \"355 West San Fernando Street\",");
            sb.Append("\"City\": \"San Jose\",");
            sb.Append("\"StateProvinceCode\": \"CA\",");
            sb.Append("\"PostalCode\": \"95113\",");
            sb.Append("\"CountryCode\": \"US\"}},");
            sb.Append("\"ShipFrom\": {");
            sb.Append("\"Name\": \"Brett Susick\",");
            sb.Append("\"Address\": {");
            sb.Append("\"AddressLine\": \"400 POYDRAS ST 10\",");
            sb.Append("\"City\": \"NEW ORLEANS\",");
            sb.Append("\"StateProvinceCode\": \"LA\",");
            sb.Append("\"PostalCode\": \"70130\",");
            sb.Append("\"CountryCode\": \"US\"}},");
            sb.Append("\"Service\": {");
            sb.Append("\"Code\": \"03\",");
            sb.Append("\"Description\": \"Ground\"},");
            sb.Append("\"ShipmentTotalWeight\": {");
            sb.Append("\"UnitOfMeasurement\": {");
            sb.Append("\"Code\": \"LBS\",");
            sb.Append("\"Description\": \"Pounds\"},");
            //sb.Append("\"Weight\": \"20\"},");
            sb.Append("\"Weight\": \"146\"},");
            sb.Append("\"Package\": {");
            sb.Append("\"PackagingType\": {");
            sb.Append("\"Code\": \"02\",");
            sb.Append("\"Description\": \"Package\"},");
            //sb.Append("\"Dimensions\": {");
            //sb.Append("\"UnitOfMeasurement\": {");
            //sb.Append("\"Code\": \"IN\"},");
            //sb.Append("\"Length\": \"10\",");
            //sb.Append("\"Width\": \"7\",");
            //sb.Append("\"Height\": \"5\"},");
            sb.Append("\"PackageWeight\": {");
            sb.Append("\"UnitOfMeasurement\": {");
            sb.Append("\"Code\": \"LBS\"},");
            sb.Append("\"Weight\": \"146\"}}}}}");
            return sb.ToString();
        }


        public bool multipackagexample()
        {
            // example in PHP build array
            //    $package['Shipment']['Package'] = array();
            //    foreach ($packaging as $p) {
            //$pkg = array();
            //$pkg['PackagingType'] = array(
            //    'Code' => '02',
            //    'Description' => 'Nails'
            //);
            //$pkg['PackageWeight'] = array(
            //    'Weight' => (int)$p["weight"],
            //    'UnitOfMeasurement' => array('Code' => 'KGS', 'Description' => 'KGS')
            //);
            //$pkg['Dimensions'] = array(
            //    'Length' => (int)$p["length"],
            //    'Width' => (int)$p["width"],
            //    'Height' => (int)$p["height"],
            //    'UnitOfMeasurement' => array('Code' => 'CM', 'Description' => 'CM')
            //);

            //        array_push($package['Shipment']['Package'], $pkg);
            //    }
            return false;
        }
        public string getTestItems()
        {
            UPSItem[] aItems = new UPSItem[1];



            aItems[0] = new UPSItem
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

            //aItems[0] = new UPSItem
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

            //aItems[0] = new UPSItem
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

            UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[5];
            oRefs[0] = new UPSReferenceNumbers { type = "SHID", value = "CNS-10-201" };
            oRefs[1] = new UPSReferenceNumbers { type = "CON", value = "SO-HLB-29030-1120" };
            oRefs[2] = new UPSReferenceNumbers { type = "CRID", value = "HBS-10-57" };
            oRefs[3] = new UPSReferenceNumbers { type = "PU", value = "CNS-10-201" };
            oRefs[4] = new UPSReferenceNumbers { type = "DEL", value = "SO-HLB-29030-1120" };
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
        //   // request.AddParameter("audience", "https://inavisphere.UPSobinson.com", ParameterType.GetOrPost);
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
        //    UPSEquipment[] oEquipments = new UPSEquipment[1];
        //    oEquipments[0] = new UPSEquipment()
        //    {
        //        equipmentType = "Van",
        //        quantity = 1
        //    };

        //    UPSTransportMode oMode = new UPSTransportMode()
        //    {
        //        mode = "LTL",
        //        equipments = oEquipments
        //    };
        //    string jMode = JsonConvert.SerializeObject(oMode);

        //    UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[1];
        //    oRefs[0] = new UPSReferenceNumbers { type = "MBOL", value = "MBOL12345" };
        //    string jRefs = JsonConvert.SerializeObject(oRefs);

        //    string sRoute = "{ \"items\": [{\"description\":\"widgets\",\"freightClass\":400,\"actualWeight\":250.0,\"weightUnit\":\"Pounds\",\"length\":24.0,\"width\":10.0,\"height\":10.0,\"linearUnit\":\"Inches\",\"pallets\":1,\"pieces\":1,\"palletSpaces\":1,\"volume\":3.0,\"volumeUnit\":\"CubicFeet\",\"density\":25.0,\"linearSpace\":8.0,\"declaredValue\":50000.0,\"packagingCode\":\"BIN\",\"productCode\":\"wdgt\",\"productName\":\"widgets\",\"temperatureSensitive\":\"Dry\",\"temperatureUnit\":\"Fahrenheit\",\"requiredTemperatureHigh\":85.0,\"requiredTemperatureLow\":35.0,\"unitsPerPallet\":36,\"unitWeight\":14.0,\"unitVolume\":3.0,\"isStackable\":\"true\",\"isOverWeightOverDimensional\":\"false\",\"isUsedGood\":\"false\",\"isHazardous\":\"false\",\"hazardousDescription\":\"Car Battery\",\"hazardousEmergencyPhone\":\"5555555555\",\"nmfc\":\"156600\",\"upc\":\"1234567890\",\"sku\":\"01234 - 001 - F10 - 6\",\"plu\":\"4026\",\"referenceNumbers\":[{\"type\":\"PO\",\"value\":\"PO12345\"}]}],\"origin\":{\"locationName\":\"Origin Location\",\"address1\":\"14800 Charlson Rd\",\"address2\":\"Building 1\",\"address3\":\"Room 212\",\"city\":\"Eden Prairie\",\"stateProvinceCode\":\"MN\",\"countryCode\":\"US\",\"postalCode\":\"55347\",\"latitude\":31.717096,\"longitude\":-99.132553,\"specialRequirement\":{\"liftGate\":\"false\",\"insidePickup\":\"false\",\"insideDelivery\":\"false\",\"residentialNonCommercial\":\"false\",\"limitedAccess\":\"false\",\"tradeShoworConvention\":\"false\",\"constructionSite\":\"false\",\"dropOffAtCarrierTerminal\":\"false\",\"pickupAtCarrierTerminal\":\"false\"},\"isPort\":\"false\",\"unLocode\":\"US AC8\",\"iata\":\"ACB\",\"customerLocationId\":\"W541849\",\"referenceNumbers\":[{\"type\":\"PU\",\"value\":\"PickUpNumber1\"}]},\"destination\":{\"locationName\":\"Destination Location\",\"address1\":\"800 Washington Avenue North\",\"address2\":\"Building 1\",\"address3\":\"Suite 550\",\"city\":\"Minneapolis\",\"stateProvinceCode\":\"MN\",\"countryCode\":\"US\",\"postalCode\":\"55401\",\"latitude\":44.989386,\"longitude\":-93.278626,\"specialRequirement\":{\"liftGate\":\"false\",\"insidePickup\":\"false\",\"insideDelivery\":\"false\",\"residentialNonCommercial\":\"false\",\"limitedAccess\":\"false\",\"tradeShoworConvention\":\"false\",\"constructionSite\":\"false\",\"dropOffAtCarrierTerminal\":\"false\",\"pickupAtCarrierTerminal\":\"false\"},\"isPort\":\"false\",\"unLocode\":\"US AC8\",\"iata\":\"ACB\",\"customerLocationId\":\"W541849\",\"referenceNumbers\":[{\"type\":\"DEL\",\"value\":\"DeliverNumber1\"}]},\"shipDate\": \"2022 - 02 - 20T20: 30:00.0000000Z\", \"customerCode\": \"C377465\",\"declaredValue\": 50000,\"transportModes\": [{\"mode\":\"LTL\",\"equipments\":[{\"equipmentType\":\"Van\",\"quantity\":1}]}],\"referenceNumbers\": [{\"type\":\"MBOL\",\"value\":\"MBOL12345\"}], \"optionalAccessorials\": [\"APT\" ]}";

        //    request.AddParameter("undefined", sRoute, ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    return true;

        //}



        public UPSQuoteResponse getTestHTTPRateRequest(string sToken, string sCCode)
        {
            UPSQuoteResponse oData = new UPSQuoteResponse();
            DateTime dShipDate = DateTime.Now.AddDays(5);
            string sRet = "";

            string sShipDate = string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dShipDate.ToUniversalTime());


            var request = (HttpWebRequest)WebRequest.Create("https://wwwcie.ups.com/ship/v1/rating/Rate");
            string postData = getTestShipmentJSON(); // "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\":\"" + sShipDate + "\", \"customerCode\": \"" + sCCode + "\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\": \"2022-03-20T20:30:00.0000000Z\", \"customerCode\": \"C6953660\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //C6953660
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("AccessLicenseNumber", "8DB68B854C9118E5");
            request.Headers.Add("Password", "Ups1234567");
            request.Headers.Add("Username", "BRETTREILYFOODS");
            request.Headers.Add("transId", "Tran123");
            request.Headers.Add("transactionSrc", "XOLT");
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
                sRet = responseJSON.ToString();
                oData = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSQuoteResponse>(sRet);
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream eData = response.GetResponseStream())
                    using (var reader = new StreamReader(eData))
                    {
                        oData = new UPSQuoteResponse();
                        oData.postMessagesOnly = true;
                        string sTxtMsg = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sTxtMsg))
                        {
                            sRet = "Error!";
                            oData.AddMessage(UPSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your UPS account manager.  The actual Error is: " + e.Message, "", "");
                        }
                        else
                        {
                            sRet = sTxtMsg;
                            oData.AddMessage(UPSAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                        }

                    }
                }
            }
            return oData;
        }

        public string getTestHTTPRateRequest()
        {
            UPSQuoteResponse oData = new UPSQuoteResponse();
            DateTime dShipDate = DateTime.Now.AddDays(5);
            string sRet = "";

            string sShipDate = string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", dShipDate.ToUniversalTime());


            var request = (HttpWebRequest)WebRequest.Create("https://wwwcie.ups.com/ship/v1/rating/Rate");
            string postData = getTestShipmentJSON(); // "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\":\"" + sShipDate + "\", \"customerCode\": \"" + sCCode + "\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //string postData = "{ \"items\": " + getTestItems() + ",\"origin\":" + getTestOrigin() + ",\"destination\":" + getTestDestination() + ",\"shipDate\": \"2022-03-20T20:30:00.0000000Z\", \"customerCode\": \"C6953660\",\"declaredValue\": 50000,\"transportModes\": [" + getTestMode("Van", 1, "TL") + "],\"referenceNumbers\": " + getTestRateReferenceNumbers() + ", \"optionalAccessorials\": [\"APT\" ]}";
            //C6953660
            var data = Encoding.ASCII.GetBytes(postData);
            request.Headers.Add("AccessLicenseNumber", "8DB68B854C9118E5");
            request.Headers.Add("Password", "Ups1234567");
            request.Headers.Add("Username", "BRETTREILYFOODS");
            request.Headers.Add("transId", "Tran123");
            request.Headers.Add("transactionSrc", "XOLT");
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
                sRet = responseJSON.ToString();
                oData = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSQuoteResponse>(sRet);
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream eData = response.GetResponseStream())
                    using (var reader = new StreamReader(eData))
                    {
                        oData = new UPSQuoteResponse();
                        oData.postMessagesOnly = true;
                        string sTxtMsg = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(sTxtMsg))
                        {
                            sRet = "Error!";
                            oData.AddMessage(UPSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your UPS account manager.  The actual Error is: " + e.Message, "", "");
                        }
                        else
                        {
                            sRet = sTxtMsg;
                            oData.AddMessage(UPSAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                        }

                    }
                }
            }
            //return oData;
            return sRet;
        }

        public UPSQuoteResponse getHTTPRateRequest(string sAccessLicenseNumber, string sUserName, string sPassword, RateRequest oRequest, string sDataURL)
        {
            UPSQuoteResponse oData = new UPSQuoteResponse();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(sDataURL);
                string postData = oRequest.postData();
                //System.Diagnostics.Debug.WriteLine("Begin Post Data:");
                //System.Diagnostics.Debug.WriteLine("");
                //System.Diagnostics.Debug.Write(postData);

                //System.Diagnostics.Debug.WriteLine("");

                //System.Diagnostics.Debug.WriteLine("End Post Data");
                var data = Encoding.ASCII.GetBytes(postData);
                request.Headers.Add("AccessLicenseNumber", sAccessLicenseNumber);
                request.Headers.Add("Password", sPassword);
                request.Headers.Add("Username", sUserName);
                string stransId = DateTime.Now.ToString("yyyyMMddHHmmss");
                string stransactionSrc = "Date";
                string sTempTran = "";
                string sTemptransactionSrc = "";
                if (oRequest.oOrigin != null && oRequest.oOrigin.referenceNumbers != null && oRequest.oOrigin.referenceNumbers.Count() > 0)
                {
                    if (oRequest.oOrigin.referenceNumbers.Any(x => x.type == "SHID"))
                    {
                        sTemptransactionSrc = "SHID";
                        sTempTran = oRequest.oOrigin.referenceNumbers.Where(x => x.type == "SHID").Select(y => y.value).FirstOrDefault();
                        
                    }

                    if(string.IsNullOrWhiteSpace(sTempTran) && oRequest.oOrigin.referenceNumbers.Any(x => x.type == "CON"))
                    {
                        sTemptransactionSrc = "OrderNo";
                        sTempTran = oRequest.oOrigin.referenceNumbers.Where(x => x.type == "CON").Select(y => y.value).FirstOrDefault();
                    }

                    if (string.IsNullOrWhiteSpace(sTempTran) && oRequest.oOrigin.referenceNumbers.Any(x => x.type == "CRID"))
                    {
                        sTemptransactionSrc = "BookPRO";
                        sTempTran = oRequest.oOrigin.referenceNumbers.Where(x => x.type == "CRID").Select(y => y.value).FirstOrDefault();
                    }

                    if (string.IsNullOrWhiteSpace(sTempTran) && oRequest.oOrigin.referenceNumbers.Any(x => x.type == "PO"))
                    {
                        sTemptransactionSrc = "PO";
                        sTempTran = oRequest.oOrigin.referenceNumbers.Where(x => x.type == "PO").Select(y => y.value).FirstOrDefault();
                    }

                    if (!string.IsNullOrWhiteSpace(sTempTran)) { stransId = sTempTran; stransactionSrc = sTemptransactionSrc;  }

                }
                request.Headers.Add("transId", stransId);
                request.Headers.Add("transactionSrc", stransactionSrc);
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
                    oData = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSQuoteResponse>(sRet);
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream eData = response.GetResponseStream())
                        using (var reader = new StreamReader(eData))
                        {
                            oData = new UPSQuoteResponse();
                            oData.postMessagesOnly = true;
                            string sTxtMsg = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(sTxtMsg))
                            {
                                oData.AddMessage(UPSAPI.MessageEnum.E_CommunicationFailure, "Rates are not available at this time or your shipping information is not valid.  Please contact your UPS account manager.  The actual Error is: " + e.Message, "", "");
                            }
                            else
                            {
                                oData.AddMessage(UPSAPI.MessageEnum.E_NoRatesFound, sTxtMsg, "", "");
                            }

                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                oData.postMessagesOnly = true;
                oData.AddMessage(UPSAPI.MessageEnum.E_UnExpected, "UPS API Failed check your shipping information or contact your UPS account manager for more details", "", "");
            }
            return oData;
        }

    }

    //        {"quoteSummaries":[{"quoteId":2159912944,"customer":{},"totalCharge":599.35,"totalFreightCharge":499.46,"totalAccessorialCharge":99.89,"transit":{"minimumTransitDays":1,"maximumTransitDays":2},"rates":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}],"transportModeType":"LTL","equipmentType":"Van","quoteSource":"Contractual"}]}

    public class UPSMessage
    {
        public string Message { get; set; }
        public string MessageLocalCode { get; set; }
        public string VendorMessage { get; set; }
        public string VendorErrorCode { get; set; }
        public string FieldName { get; set; }
        public string Details { get; set; }
        public bool bLogged { get; set; } = false;


    }
    public class UPSQuoteResponse
    {
        public UPSQuoteSummary RateResponse { get; set; }
        public bool postMessagesOnly { get; set; } = false;
        private List<UPSMessage> messages { get; set; }

        public void AddMessage(UPSAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            UPSMessage msg = new UPSMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = UPSAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<UPSMessage>(); }
            messages.Add(msg);
        }

        public void AddMessage(UPSMessage msg)
        {
            if (messages == null) { messages = new List<UPSMessage>(); }
            msg.bLogged = false;
            messages.Add(msg);
        }

        public List<UPSMessage> GetMessages()
        {
            if (messages == null) {                
                messages = new List<UPSMessage>();
                if (RateResponse != null && RateResponse.Response != null && RateResponse.Response.Alert != null && RateResponse.Response.Alert.Count() > 0)
                {
                    foreach (UPSResponseCode a in RateResponse.Response.Alert)
                    {
                        messages.Add(new UPSMessage() { Message = a.Description, VendorErrorCode = a.Code });
                    }
                }      
            }
            return messages;
        }

        public string concateMessages()
        {
            string sRet = "";
            if (messages != null && messages.Count() > 0)
            {
                foreach (UPSMessage m in messages)
                {
                    sRet += m.Message + ": " + m.Details + " ";
                }
            }

            return sRet;
        }
    }
    public class UPSQuoteSummary
    {

        public UPSResponse Response { get; set; } = new UPSResponse();
        public UPSRate[] rates { get; set; } //":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}]

        public UPSRatedShipment RatedShipment { get; set; } = new UPSRatedShipment(); //":[{"rateId":216096619,"totalRate":499.46,"unitRate":499.46,"quantity":1,"rateCode":"400","rateCodeValue":"Line Haul","currencyCode":"USD","isOptional":false},{ "rateId":216096620,"totalRate":99.89,"unitRate":99.89,"quantity":499.46,"rateCode":"405","rateCodeValue":"Fuel Surcharge","currencyCode":"USD","isOptional":false},{ "rateId":216096621,"totalRate":150,"unitRate":150,"quantity":1,"rateCode":"EXL","rateCodeValue":"Excessive Length","currencyCode":"USD","isOptional":true}]


        public Int64 quoteId { get; set; } //":2159912944
        public UPSCarrier carrier { get; set; } = new UPSCarrier();
        public UPSCustomer customer { get; set; } = new UPSCustomer();
        public double totalCharge { get; set; } //":599.35
        public double totalFreightCharge { get; set; } //":499.46
        public double totalAccessorialCharge { get; set; } //":99.89
        public UPSTransit transit { get; set; } = new UPSTransit(); //":{"minimumTransitDays":1,"maximumTransitDays":2},"
        public string transportModeType { get; set; } = "Parcel"; //":"LTL"
        public string equipmentType { get; set; } //":"Van"
        public UPSCargoLiability cargoLiability { get; set; } = new UPSCargoLiability();
        public double distance { get; set; }
        //
        public string quoteSource { get; set; } //Contractual"
    }

    public class UPSResponseTransaction
    {
        public string CustomerContext { get; set; }
        public string TransactionIdentifer { get; set; }
    }
    

    public class UPSResponseCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class UPSResponseCharge
    {
        public string Code { get; set; }
        public decimal MonetaryValue { get; set; }
    }

    public class UPSResponseItemizedCharge
    {
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public decimal MonetaryValue { get; set; }
        public string SubType { get; set; }
    }

    public class UPSResponseBillingWeight
    {
        public UPSResponseCode UnitOfMeasurement { get; set; } 
        public double Weight { get; set; }
    }

    public class UPSResponse
    {
        public UPSResponseCode ResponseStatus { get; set; }
        public UPSResponseCode[] Alert { get; set; }

        public UPSResponseTransaction TransactionReference { get; set; }
    }

      

    public class UPSRatedShipment
    {
        public UPSResponseCode Service { get; set; }
        public UPSResponseCode[] RatedShipmentAlert { get; set; }
        public UPSResponseBillingWeight BillingWeight { get; set; }
        public UPSResponseCharge TransportationCharges { get; set; }
        public UPSResponseCharge BaseServiceCharge { get; set; }
        public UPSResponseCharge ServiceOptionsCharges { get; set; }
        public UPSResponseCharge TotalCharges { get; set; }
        public UPSRatedShipment RatedPackage { get; set; }
        public  UPSResponseItemizedCharge[] ItemizedCharges { get; set; }
        public double Weight { get; set; }


        public double totalRate { get; set; } //number <float> Currency amount of the total rate

        public double unitRate { get; set; } //number <float> Currency amount of the unit rate

        public double quantity { get; set; } //number <float> Defines how many units there are
        public string rateCode { get; set; }
        public string rateCodeValue { get; set; } //string (rateCodeEnum)
        public string currencyCode { get; set; } //string (currencyCodeEnum)
        public bool isOptional { get; set; } //Defines if the rate is optional. If the isOptional flag is set to false, the currency amount of the assecorial will be included in the totalCharge.
    }

    public class UPSRate
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

    public class UPSCargoLiability
    {
        public double perPound { get; set; } //integer Carrier’s liability per pound based on product attributes.

        public double max { get; set; } //integer Carrier’s maximum liability based on product attributes.

        public double amount { get; set; } //integer Calculated liability coverage for specific quote based on product attributes.

        public string currencyCode { get; set; } //string (currencyCodeEnum)
    }
    public class UPSTransit
    {
        public double minimumTransitDays { get; set; } = 3; // number<float> Minimum transit days.
        public double maximumTransitDays { get; set; } = 5; // number <float> Maximum transit days.
        public string minimumDeliveryDate { get; set; } // string <date-time> An ISO8601 UTC date-time used to indicate the minimum delivery date.
        public string maximumDeliveryDate { get; set; } //string <date-time> An ISO8601 UTC date-time used to indicate the maximum delivery date.
    }

    public class UPSCustomer
    {
        public string customerName { get; set; }
    }

    public class UPSCarrier
    {
        public string carrierName { get; set; }
        public string scac { get; set; }
    }

    public class UPSTokenResponse
    {
        public UPSTokenData tokenresponse { get; set; }

        public string error_code { get; set; }
        public string error_msg { get; set; }
        public bool success { get; set; }
        public IRestResponse response { get; set; }

    }

    public class UPSTokenData
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }



    public class UPSItem
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
        public UPSReferenceNumbers[] referenceNumbers { get; set; }

    }

    public class UPSReferenceNumbers
    {
        public string type { get; set; } //"PO",
        public string value { get; set; } //"PO12345"

        public static void addReferenceNumber(UPSAPI.RefNumbers etype, string sVal, ref List<UPSReferenceNumbers> lref)
        {
            if (lref == null)
            {
                lref = new List<UPSReferenceNumbers>();
            }
            lref.Add(new UPSReferenceNumbers() { type = etype.ToString(), value = sVal });
        }

        public static void addReferenceNumber(UPSAPI.RefNumbers etype, string sVal, ref UPSReferenceNumbers[] referenceNumbers)
        {
            List<UPSReferenceNumbers> lref = new List<UPSReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(new UPSReferenceNumbers() { type = etype.ToString(), value = sVal });

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumber(UPSReferenceNumbers oRefNbr, ref UPSReferenceNumbers[] referenceNumbers)
        {
            List<UPSReferenceNumbers> lref = new List<UPSReferenceNumbers>();
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref = referenceNumbers.ToList();
            }
            lref.Add(oRefNbr);

            referenceNumbers = lref.ToArray();
        }

        public static void addReferenceNumbers(List<UPSReferenceNumbers> lref, ref UPSReferenceNumbers[] referenceNumbers)
        {
            if (referenceNumbers != null && referenceNumbers.Count() > 0)
            {
                lref.AddRange(referenceNumbers.ToList());
            }

            referenceNumbers = lref.ToArray();
        }

        public static void repklaceReferenceNumbers(List<UPSReferenceNumbers> lref, ref UPSReferenceNumbers[] referenceNumbers)
        {
            referenceNumbers = lref.ToArray();
        }
    }

    public class UPSAddress
    {
        //Note: non-requried properties have been removed to meet UPS non-empty data requiremnts
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
        public UPSSpecialRequirement specialRequirement { get; set; }
        //public string isPort { get; set; } // "false",
        //public string unLocode { get; set; } // "US AC8",
        //public string iata { get; set; } // "ACB",
        public string customerLocationId { get; set; } // "W541849",
        public UPSReferenceNumbers[] referenceNumbers { get; set; }

    }

    public class UPSSpecialRequirement
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


        public void setSpecialRequrement(UPSAPI.SpecialRequirement eReq)
        {
            switch (eReq)
            {
                case UPSAPI.SpecialRequirement.liftGate:
                    this.liftGate = "true";
                    break;
                case UPSAPI.SpecialRequirement.insidePickup:
                    this.insidePickup = "true";
                    break;
                case UPSAPI.SpecialRequirement.insideDelivery:
                    this.insideDelivery = "true";
                    break;
                case UPSAPI.SpecialRequirement.residentialNonCommercial:
                    this.residentialNonCommercial = "true";
                    break;
                case UPSAPI.SpecialRequirement.limitedAccess:
                    this.limitedAccess = "true";
                    break;
                case UPSAPI.SpecialRequirement.tradeShoworConvention:
                    this.tradeShoworConvention = "true";
                    break;
                case UPSAPI.SpecialRequirement.constructionSite:
                    this.constructionSite = "true";
                    break;
                case UPSAPI.SpecialRequirement.dropOffAtCarrierTerminal:
                    this.dropOffAtCarrierTerminal = "true";
                    break;
                case UPSAPI.SpecialRequirement.pickupAtCarrierTerminal:
                    this.pickupAtCarrierTerminal = "true";
                    break;
                default:
                    break;
            }


        }

    }

    public class UPSTransportMode
    {
        public string mode { get; set; } = "TL";
        public UPSEquipment[] equipments { get; set; }

    }

    public class UPSEquipment
    {
        public string equipmentType { get; set; } = "Van";
        public int quantity { get; set; } = 1; //1
    }


    /// <summary>
    /// Configure the Load to be shipped.  then call UPSAPI.getHTTPRateRequest with a valid token and a RateRequest Object
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

        public UPSTransportMode tMode { get; set; } = new UPSTransportMode();
        public UPSSpecialRequirement sSpecial { get; set; }

        //note: caller must populate  UPSSpecialRequirement oOrigSpecialReq = new UPSSpecialRequirement()
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
        //UPSReferenceNumbers[] oOrigRefs = new UPSReferenceNumbers[1];
        //oOrigRefs[0] = new UPSReferenceNumbers { type = "PU", value = "PickUpNumber1" };
        public UPSAddress oOrigin { get; set; }

        public List<UPSAddress> lStops { get; set; }

        //note: caller must populate  Item UPSReferenceNumbers[] oRefs 
        // example oRefs[0] = new UPSReferenceNumbers { type = "PO", value = "PO12345" };
        public List<UPSItem> lItems { get; set; }
        public string getTotalWgt()
        {
            string sRet = "10";
            if (lItems != null && lItems.Count()  > 0)
            {
                int? iWgt = lItems.Sum(x => x.actualWeight);
                sRet = iWgt.HasValue? iWgt.Value.ToString() : "10";
            }

            return sRet;
        }
        public string ServiceCode { get; set; } = "03";
        public string ServiceDesc { get; set; } = "Ground";

        public void setServiceMode(string sMode)
        {
            ServiceCode = sMode;
            ServiceDesc = UPSAPI.getUPSServiceModeDesc(sMode);
        }

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

        public UPSTransportMode oMode { get; set; }

        public UPSReferenceNumbers[] oRefs { get; set; }

        public string[] oAccessorials { get; set; }

        public string postData()
        {
            
            UPSAddress oShipTo = lStops[0];
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"RateRequest\": {");
            sb.Append("\"Request\": {");
            sb.Append("\"SubVersion\": \"1703\",");
            sb.Append("\"TransactionReference\": {");
            sb.Append("\"CustomerContext\": \" \" } },");
            sb.Append("\"Shipment\": {");
            sb.Append("\"ShipmentRatingOptions\": {");
            sb.Append("\"UserLevelDiscountIndicator\": \"TRUE\"},");
            sb.Append("\"Shipper\": {");
            sb.AppendFormat("\"Name\": \"{0}\",", oOrigin.locationName);
            sb.AppendFormat("\"ShipperNumber\": \"{0}\",",oOrigin.customerLocationId);
            sb.Append("\"Address\": {");
            sb.AppendFormat("\"AddressLine\": \"{0}\",", oOrigin.address1);
            sb.AppendFormat("\"City\": \"{0}\",", oOrigin.city);
            sb.AppendFormat("\"StateProvinceCode\": \"{0}\",", oOrigin.stateProvinceCode);
            sb.AppendFormat("\"PostalCode\": \"{0}\",", oOrigin.postalCode);
            sb.AppendFormat("\"CountryCode\": \"{0}\"", oOrigin.countryCode);
            sb.Append("}},\"ShipTo\": {");
            sb.AppendFormat("\"Name\": \"{0}\",", oShipTo.locationName);
            sb.Append("\"Address\": {");
            sb.AppendFormat("\"AddressLine\": \"{0}\",", oShipTo.address1);
            sb.AppendFormat("\"City\": \"{0}\",", oShipTo.city);
            sb.AppendFormat("\"StateProvinceCode\": \"{0}\",", oShipTo.stateProvinceCode);
            sb.AppendFormat("\"PostalCode\": \"{0}\",", oShipTo.postalCode);
            sb.AppendFormat("\"CountryCode\": \"{0}\"", oShipTo.countryCode);
            sb.Append("}},\"ShipFrom\": {");
            sb.AppendFormat("\"Name\": \"{0}\",", oOrigin.locationName);
            sb.Append("\"Address\": {");
            sb.AppendFormat("\"AddressLine\": \"{0}\",", oOrigin.address1);
            sb.AppendFormat("\"City\": \"{0}\",", oOrigin.city);
            sb.AppendFormat("\"StateProvinceCode\": \"{0}\",", oOrigin.stateProvinceCode);
            sb.AppendFormat("\"PostalCode\": \"{0}\",", oOrigin.postalCode);
            sb.AppendFormat("\"CountryCode\": \"{0}\"", oOrigin.countryCode);
            sb.Append("}},\"Service\": {");
            sb.AppendFormat("\"Code\": \"{0}\",", ServiceCode);
            sb.AppendFormat("\"Description\": \"{0}\"", ServiceDesc);
            sb.Append("},\"ShipmentTotalWeight\": {");
            sb.Append("\"UnitOfMeasurement\": {");
            sb.Append("\"Code\": \"LBS\",");
            sb.Append("\"Description\": \"Pounds\"},");
            //sb.Append("\"Weight\": \"20\"},");
            sb.AppendFormat("\"Weight\": \"{0}\"", getTotalWgt());
            sb.Append("},\"Package\": {");
            sb.Append("\"PackagingType\": {");
            sb.Append("\"Code\": \"02\",");
            sb.Append("\"Description\": \"Package\"},");
            //sb.Append("\"Dimensions\": {");
            //sb.Append("\"UnitOfMeasurement\": {");
            //sb.Append("\"Code\": \"IN\"},");
            //sb.Append("\"Length\": \"10\",");
            //sb.Append("\"Width\": \"7\",");
            //sb.Append("\"Height\": \"5\"},");
            sb.Append("\"PackageWeight\": {");
            sb.Append("\"UnitOfMeasurement\": {");
            sb.Append("\"Code\": \"LBS\"},");
            sb.AppendFormat("\"Weight\": \"{0}\"", getTotalWgt());
            sb.Append("}}}}}");
            return sb.ToString();

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
            UPSAddress oDest = lStops[0];
            string jDest = JsonConvert.SerializeObject(oDest);
            return jDest;
        }

        public void setMode(string sType, int iQty, string sMode)
        {
            UPSEquipment[] oEquipments = new UPSEquipment[1];
            oEquipments[0] = new UPSEquipment()
            {
                equipmentType = sType,
                quantity = iQty
            };

            this.oMode = new UPSTransportMode()
            {
                mode = sMode,
                equipments = oEquipments
            };
        }

        public void setMode(UPSAPI.EquipType eEquipType, int iEquipQty, UPSAPI.TransMode eMode)
        {


            UPSEquipment[] oEquipments = new UPSEquipment[1];
            oEquipments[0] = new UPSEquipment()
            {
                equipmentType = eEquipType.ToString(),
                quantity = iEquipQty
            };

            oMode = new UPSTransportMode()
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

            //UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[1];
            //oRefs[0] = new UPSReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }

        public string getAccessorials()

        {
            string jRefs = "[]";
            if (oAccessorials != null && oAccessorials.Count() > 0)
            {
                jRefs = JsonConvert.SerializeObject(oAccessorials);
            }

            //UPSReferenceNumbers[] oRefs = new UPSReferenceNumbers[1];
            //oRefs[0] = new UPSReferenceNumbers { type = "MBOL", value = "MBOL12345" };

            return jRefs;
        }


        private bool postMessagesOnly { get; set; } = false;
        private List<UPSMessage> messages { get; set; }

        public bool getPostMessageOnlyFlag()
        {
            return postMessagesOnly;
        }

        public void setPostMessageOnlyFlag(bool bval)
        {
            postMessagesOnly = bval;
        }

        public void AddMessage(UPSAPI.MessageEnum key, string sDetails, string sDefault, string sFieldName)
        {
            UPSMessage msg = new UPSMessage();
            msg.MessageLocalCode = key.ToString();
            msg.Message = UPSAPI.getMessageNotLocalized(key, sDefault);
            msg.Details = sDetails;
            msg.FieldName = sFieldName;
            msg.bLogged = false;
            if (messages == null) { messages = new List<UPSMessage>(); }
            messages.Add(msg);
        }

        public List<UPSMessage> GetMessages()
        {
            if (messages == null) { messages = new List<UPSMessage>(); }
            return messages;
        }
    }
}
