using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGL.UTC.Library;

namespace DynamicsTMS365.Tests
{
    [TestClass]
    public class UTCTest
    {
        [TestMethod]
        public void TestGetCultureInfoList()
        {
            var oRet = clsApplication.GenerateCultureInfoList();
            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestGetTimeZoneList()
        {
            var oRet = clsApplication.GetTimeZoneList();
            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertStringDateToDate()
        {
            var cultureInfos = clsApplication.GenerateCultureInfoList();
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC");
            var cultureInfoLookup = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-US");
            var cultureInfoLookup2 = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-GB");

            //Test Case 1
            // Result is {1/10/2025 6:00:00 AM} the date is matched with the document date but the time is 5:00:00 AM 
            //according to google conversion it same as unit test returns.
            var oRet = clsApplication.convertStringDateToDate("01/10/2025", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value);

            //Test Case 2 Exactly Same
            var oRet2 = clsApplication.convertStringDateToDate("01/10/2025", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value);

            //Test Case 3
            var oRet3 = clsApplication.convertStringDateToDate("20250110", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value);

            //Test Case 4
            //Note: As the date is formated it becomes "2025/01/10"
            // As input time zone is CST which is {(UTC-06:00) Central Time (US & Canada)} so After conversion it becomes {1/10/2025 6:00:00 AM}
            var oRet4 = clsApplication.convertStringDateToDate("20250110", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value);

            //Test Case 5
            var oRet5 = clsApplication.convertStringDateToDate("2025", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value, new DateTime(2025, 1, 1));

            //Test Case 6
            var oRet6 = clsApplication.convertStringDateToDate("2025", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value, new DateTime(2025, 1, 1));

            //Test Case 7
            var oRet7 = clsApplication.convertStringDateToDate("01/10/2025 10:00 am", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value, new DateTime(2025, 1, 1));

            //Test Case 8 Exactly Same
            var oRet8 = clsApplication.convertStringDateToDate("01/10/2025 10:00 am", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value, new DateTime(2025, 1, 1));

            //Test Case 9
            var oRet9 = clsApplication.convertStringDateToDate("01/10/2025", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            //Test Case 10 Exactly Same
            var oRet10 = clsApplication.convertStringDateToDate("01/10/2025", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            //Test Case 11
            var oRet11 = clsApplication.convertStringDateToDate("20250110", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            //Test Case 12 Not Matched with the Document
            //Note: Input Date which is in UTC is {1/10/2025 12:00:00 AM}
            //      Output TimeZone is {(UTC-06:00) Central Time (US & Canada)}
            //      So -6 means go back 6 hours so the date becomes {1/9/2025 6:00:00 PM}
            var oRet12 = clsApplication.convertStringDateToDate("20250110", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            //Test Case 13 One Hour Diff due to day light saving in US
            var oRet13 = clsApplication.convertStringDateToDate("01/10/2025 10:00 am", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            //Test Case 14 Exactly Same
            var oRet14 = clsApplication.convertStringDateToDate("01/10/2025 10:00 am", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value, new DateTime(2025, 1, 1));

            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertStringTimeToDate()
        {
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC"); // Example destination time zone

            var oRet = clsApplication.convertStringTimeToDate("14:00", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet2 = clsApplication.convertStringTimeToDate("10:20 am", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet3 = clsApplication.convertStringTimeToDate("8", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet4 = clsApplication.convertStringTimeToDate("22", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet5 = clsApplication.convertStringTimeToDate("235", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet6 = clsApplication.convertStringTimeToDate("2220", new DateTime(2025,10,1), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet7 = clsApplication.convertStringTimeToDate("14: 00", new DateTime(2025,10,1), timeZoneLookup2.value, timeZoneLookup.value);
            var oRet8 = clsApplication.convertStringTimeToDate("03:20 am", new DateTime(2025,10,1), timeZoneLookup2.value, timeZoneLookup.value);
            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertDateToShortDateString()
        {
            var cultureInfos = clsApplication.GenerateCultureInfoList();
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC");
            var cultureInfoLookup = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-US");
            var cultureInfoLookup2 = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-GB");

            var oRet = clsApplication.convertDateToShortDateString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet1 = clsApplication.convertDateToShortDateString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet2 = clsApplication.convertDateToShortDateString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            //Note: Document needs to be update the result should be 09/01/25. My function is generating the right value.
            var oRet3 = clsApplication.convertDateToShortDateString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertDateToDateTimeString()
        {
            var cultureInfos = clsApplication.GenerateCultureInfoList();
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC");
            var cultureInfoLookup = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-US");
            var cultureInfoLookup2 = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-GB");
            string oRet = "";
            //var oRet = clsApplication.convertDateToDateTimeString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            //var oRet1 = clsApplication.convertDateToDateTimeString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            //var oRet2 = clsApplication.convertDateToDateTimeString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            //var oRet3 = clsApplication.convertDateToDateTimeString(new DateTime(2025, 1, 10, 4, 0, 0), CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertStringToDateTime()
        {
            var cultureInfos = clsApplication.GenerateCultureInfoList();
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            // Example destination time zone
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC");
            var cultureInfoLookup = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-US");
            var cultureInfoLookup2 = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-GB");

            string oRet = "";
            //var oRet = clsApplication.convertStringToDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId),timeZoneLookup.value, timeZoneLookup2.value);
            //var oRet2 = clsApplication.convertStringToDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId),timeZoneLookup.value, timeZoneLookup2.value);
            //var oRet3 = clsApplication.convertStringToDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            //var oRet4 = clsApplication.convertStringToDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value);

            System.Diagnostics.Debug.WriteLine(oRet);
        }

        [TestMethod]
        public void TestConvertStringToNullDateTime()
        {
            var cultureInfos = clsApplication.GenerateCultureInfoList();
            var timeZones = clsApplication.GetTimeZoneList();
            // Example selection of source time zone
            var timeZoneLookup = timeZones.FirstOrDefault(tz => tz.value == "Central Standard Time");
            // Example destination time zone
            var timeZoneLookup2 = timeZones.FirstOrDefault(tz => tz.value == "UTC");
            var cultureInfoLookup = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-US");
            var cultureInfoLookup2 = cultureInfos.FirstOrDefault(ci => ci.CultureId == "en-GB");

            var oRet = clsApplication.convertStringToNullDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet2 = clsApplication.convertStringToNullDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup.value, timeZoneLookup2.value);
            var oRet3 = clsApplication.convertStringToNullDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup.CultureId), timeZoneLookup2.value, timeZoneLookup.value);
            var oRet4 = clsApplication.convertStringToNullDateTime("01/10/2025 04:00 AM", CultureInfo.GetCultureInfo(cultureInfoLookup2.CultureId), timeZoneLookup2.value, timeZoneLookup.value);

            System.Diagnostics.Debug.WriteLine(oRet);
        }

        //[TestMethod]
        //public void TestGetUserSettings()
        //{
        //    UsersController userController = new UsersController(); // Create an instance of UserController
        //    var response = userController.GetUserSettings();
        //}
    }
}
