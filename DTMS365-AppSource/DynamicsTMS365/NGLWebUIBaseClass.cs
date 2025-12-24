using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using CM = DynamicsTMS365.ContentManagement;
using System.Web.Optimization;
using System.Text;
using System.Drawing;

namespace DynamicsTMS365
{
    public class NGLWebUIBaseClass : System.Web.UI.Page
    {

        public int PageControl { get; set; }

        //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
        //used for query string links
        // not all values are used on all pages
        public int BookControlKey { get; set; }  = 0;
        public int LaneControlKey { get; set; } = 0;
        public int CarrierControlKey { get; set; } = 0;
        public int CompControlKey { get; set; } = 0;
        public int TariffControlKey { get; set; } = 0;
        public bool RedirectRequired { get; set; } = false;
        public string sRedirect { get; set; } = "";
        private string _UserTheme = "classic-opal";
        public string UserTheme
        {
            get { return _UserTheme; }
            set { _UserTheme = value; }
        }

        public string MenuControl
        {
            get 
            { 
                return "<div><span>Menu</span></div>";
            }
        }

        public string KendoIconFix
        {
            get
            {
                return "ngl.RunKendoIconFix();";
                //StringBuilder sbTest = new StringBuilder();
                //sbTest.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0)) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "chevron-down", "chevron-down");
                //string sbSection = sbTest.ToString();
                //string sbSection = "if ($('span[class^=\"k-icon k-i-gear\"]')) {{kendo.ui.icon($('span[class^=\"k-icon k-i-gear\"]'), { icon: '{1}' });\n\r";
                //Console.WriteLine(sbSection);
                // code removed by RHR for v-8.5.4.004 on 01/14/2024
                //      we now call the ngl.RunKendoIconFix(); function from core.js
                //StringBuilder sbicon = new StringBuilder();
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "pencil", "pencil");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "edit", "pencil");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "trash", "trash");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "delete", "trash");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "check", "check");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "cancel", "cancel");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "gear", "gear");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "chevron-up", "chevron-up");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "chevron-down", "chevron-down");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "arrow-chevron-up", "chevron-up");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "arrow-chevron-down", "chevron-down");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "plus", "plus"); //k-icon k-i-add
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "add", "plus"); //k-icon k-i-add

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "sort-asc-small", "sort-asc-small");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "k-i-sort-asc-sm ", "sort-asc-small");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "save", "save");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "info-circle", "info-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "information", "info-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "user", "user");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "x-outline", "x-outline");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "close-outline", "x-outline");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "heck-outline", "heck-outline");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "exclamation-circle", "exclamation-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "warning", "exclamation-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "bell", "bell");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "apply-format", "apply-format");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "star", "star");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "bookmark", "star");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "window", "window");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "track-changes-enable", "track-changes-enable");


                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "-x", "x");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "close", "x");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "search", "search");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "file-txt", "file-txt");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "hyperlink-open-sm", "hyperlink-open-sm");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "track-changes-accept-all", "track-changes-accept-all");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "file-wrench", "file-wrench");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "page-properties", "file-wrench");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "check-circle", "check-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "clock", "clock");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "lock", "lock");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "menu", "menu");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "caret-alt-down", "caret-alt-down");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "arrow-60-down", "caret-alt-down");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "gears", "gears");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "unlock", "unlock");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "map-marker-target", "map-marker-target");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "marker-pin-target", "map-marker-target");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "download", "download");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "book", "book");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "dictionary-add", "book");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "map-marker", "map-marker");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "marker-pin", "map-marker");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "minus", "minus");  
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "minus-outline", "minus-outline");  
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "connector", "connector");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "x-circle", "x-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "close-circle", "x-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "info-circle", "info-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "info", "info-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "paperclip", "paperclip");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "clip", "paperclip");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "hyperlink-open-sm", "hyperlink-open-sm");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "upload", "upload");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "grid-layout", "grid-layout");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "check-circle", "check-circle");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "button", "button");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "arrow-rotate-cw-small", "arrow-rotate-cw-small");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "reset", "calculator");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "calculator", "calculator");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "reload", "arrow-rotate-cw-small");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "hyperlink-open", "hyperlink-open-sm");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "hyperlink-open-sm", "hyperlink-open-sm");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "file-excel", "file-excel");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "excel", "file-excel");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "copy", "copy");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "file-pdf", "file-pdf");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "pdf", "file-pdf");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "globe-outline", "globe-outline");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "globe", "globe");

                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "dollar", "dollar");
                //sbicon.AppendFormat("if ($('span[class^=\"k-icon k-i-{0}\"]') && $('span[class^=\"k-icon k-i-{0}\"]').length > 0) {{ kendo.ui.icon($('span[class^=\"k-icon k-i-{0}\"]'), {{ icon: '{1}' }}); }}\r\n", "kpi-status-deny", "kpi-status-deny");


                //                k - i - arrow - rotate - cw - small
                //.k - i - reload - sm
                //.k - i - refresh - sm
                //.k - i - recurrence - sm
                //.k - i - arrows - repeat - sm

                //link   calculator  hyperlink-open-sm  k-i-hyperlink-open
                //k-icon k-i-minus-outline   k-icon k-i-excel  file-excel

                // 'k-icon k-i-copy
                // k-icon k-i-globe --> globe
                // k-icon k-i-dollar -- dollar
                // k-icon k-i-clip paperclip
                // k-i-globe-outline --> globe-outline
                // k-icon k-i-kpi-status-deny -> kpi-status-deny
                //file-pdf

                //return sbicon.ToString();
            }
        }

        public string UserToken
        {
            get
            {
                string strToken = "abc123"; //sample token
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserToken"] != null) { strToken = (string)HttpContext.Current.Session["UserToken"]; }
                return strToken;
            }
            set
            {
                if (HttpContext.Current.Session != null) { HttpContext.Current.Session["UserToken"] = value; }
            }
        }

        public string UserName
        {
            get
            {
                string strUserName = "System"; //sample token
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserName"] != null) { strUserName = (string)HttpContext.Current.Session["UserName"]; }
                return strUserName;
            }
            set
            {
                if (HttpContext.Current.Session != null) { HttpContext.Current.Session["UserName"] = value; }
            }
        }

        public int UserControl
        {
            get
            {
                int intControl = 0;
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserControl"] != null)
                {
                    string suc = HttpContext.Current.Session["UserControl"].ToString();
                    bool blnTry = int.TryParse(suc, out intControl);
                    //bool blnTry = int.TryParse((string)HttpContext.Current.Session["UserControl"], out intControl);
                }
                return intControl;
            }
            set
            {
                if (HttpContext.Current.Session != null) { HttpContext.Current.Session["UserControl"] = value; }
            }
        }

        public string WebBaseURI
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"]; }
            set { return; }
        }

        private string _HomeTabHrefURL = "";
        public string HomeTabHrefURL
        {
            get {
                if (string.IsNullOrWhiteSpace(_HomeTabHrefURL))
                {
                    _HomeTabHrefURL = System.Configuration.ConfigurationManager.AppSettings["HomeTabHrefURL"];
                }
                return _HomeTabHrefURL;
            }
            set {
                _HomeTabHrefURL = value;
                return;
            }
        }

        private string _HomeTabLogo = "";
        public string HomeTabLogo
        {
            get {
                if(string.IsNullOrWhiteSpace(_HomeTabLogo)) { _HomeTabLogo = System.Configuration.ConfigurationManager.AppSettings["HomeTabLogo"]; }
                return _HomeTabLogo;
            }
            set {
                _HomeTabLogo = value;
                return;
            }
        }

        public string RequireAuthenticationOnAllPages
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RequireAuthenticationOnAllPages"]; }
        }

        public string idaClientId
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["idaClientId"]; }
            set { return; }
        }

        public int UserGroupCategory { get; set; } 

        public string FastTabsHTML { get; set; }
        public string FastTabsJS { get; set; }
        public string PageTemplates { get; set; }
        public string PageCustomJS { get; set; }
        public string PageArrayDataJS { get; set; }
        string _PageReadyJS = "null";
        public string PageReadyJS { get { return _PageReadyJS; } set { _PageReadyJS = value; } }
        public string PageErrorsOrWarnings { get; set; }
        public static List<string> datasources = new List<string>();
        public string PageMenuHTML { get; set; }
        public string PageFooterHTML { get; set; }
        public string AuthLoginNotificationHTML { get; set; }

        string _PageMenuTab = "null";
        public string PageMenuTab { get { return _PageMenuTab; } set { _PageMenuTab = value; } }

        public string ADALPropertiesjs {
            get
            {
                System.Text.StringBuilder sbScript = new System.Text.StringBuilder();
                string sCRLF = " ";
                if (System.Diagnostics.Debugger.IsAttached) { sCRLF = " \n\r "; }
                sbScript.Append("var opostLogoutRedirectUri = '" + WebBaseURI + "'; ");
                sbScript.Append(sCRLF);
                sbScript.Append("var oredirectUri = '" + WebBaseURI + "' + getCurentFileName(); ");
                sbScript.Append(sCRLF);
                sbScript.Append("var oidaClient = '" + idaClientId + "'; ");
                sbScript.Append(sCRLF);
                sbScript.Append("var oAuth2instasnce = 'https://login.microsoftonline.com/'; ");
                sbScript.Append(sCRLF);
                sbScript.Append("var oAuth2tenant = 'common'; ");
                sbScript.Append(sCRLF);
                sbScript.Append("loadAuthContext(); ");
                sbScript.Append(sCRLF);
                return sbScript.ToString(); //" //start ADAL properties \n\r  var opostLogoutRedirectUri = '" + WebBaseURI + "'; \n\r  var oredirectUri = '" + WebBaseURI + "' + getCurentFileName(); \n\r var oidaClient = '" + idaClientId + "';  \n\r   var oAuth2instasnce = 'https://login.microsoftonline.com/'; \n\r var oAuth2tenant = 'common'; \n\r   loadAuthContext(); \n\r //End ADAL properties";
            }
        }

        public string NGLOAuth2
        {
            get
            {
                System.Text.StringBuilder sbScript = new System.Text.StringBuilder();
                string sCRLF = " ";
                if (System.Diagnostics.Debugger.IsAttached) { sCRLF = " \n\r "; }
                sbScript.Append("var serverUserControl  = " + this.UserControl.ToString() + "; ");
                sbScript.Append(sCRLF);
                sbScript.Append("var control = localStorage.NGLvar1452;");
                sbScript.Append(sCRLF);
                sbScript.Append("var sRequireAuthentication = '" + this.RequireAuthenticationOnAllPages + "';");

               sbScript.Append(sCRLF);
                if (blnUseSSR)
                {


                    sbScript.Append("localStorage.NGLvar1455 = '" + this.UserName.Replace(@"\", @"\\") + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1452 = '" + this.SSOR.UserSecurityControl + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1451 = '" + this.SSOR.USATUserID + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1454 = '" + this.SSOR.USATToken + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1472 = '" + this.SSOR.SSOAControl + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1458 = '" + this.SSOR.SSOAUserEmail + "';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1474 = '';" + sCRLF);
                    sbScript.Append("localStorage.NGLvar1457 = '" + this.SSOR.UserFriendlyName + "';" + sCRLF);

                    sbScript.Append("var divWelcome = document.getElementById('WelcomeMessage');" + sCRLF);
                    sbScript.Append("if (typeof(divWelcome) !== 'undefined' && ngl.isObject(divWelcome)){" + sCRLF);
                    sbScript.Append("divWelcome.innerHTML = 'Welcome ' + localStorage.NGLvar1455;}" + sCRLF);
                    sbScript.Append("localStorage.SignedIn = 't';" + sCRLF);                    
                } else
                {
                    sbScript.Append("ngl.UserValidated365(sRequireAuthentication,control,getCurentFileName(),serverUserControl);");
                }
                sbScript.Append(sCRLF);
                return sbScript.ToString();
            }
        }


        public string AccountRequestSendToEmail
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["AccountRequestSendToEmail"]; }
        }

        public string AccountRequestEmailSubject
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["AccountRequestEmailSubject"]; }
        }

        public string SmtpFromAddress
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"]; }
        }

        //public string jsIn

        public string jssplitter2Scripts
        {
            get{
                System.Text.StringBuilder sbjs = new System.Text.StringBuilder();
                              
                sbjs.AppendLine("<script src=\"https://code.jquery.com/jquery-3.4.1.min.js\"></script>");
                //sbjs.AppendLine("<script src=\"Scripts/kendoR32023/jquery.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/Stuk-jszip/dist/jszip.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/kendoR32023/kendo.all.min.js\"></script>");
                sbjs.AppendLine("<script>kendo.ui['Button'].fn.options['size']=\"small\";</script>");
                sbjs.AppendLine("<script src=\"https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/core.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLobjects.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLCtrls.js\"></script>"); 
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/splitter2.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/SSOA.js\"></script>");
                return sbjs.ToString();                      
            }
        }

        public string jssplitter4Scripts
        {
            get
            {
                System.Text.StringBuilder sbjs = new System.Text.StringBuilder();
                sbjs.AppendLine("<script src=\"https://code.jquery.com/jquery-3.4.1.min.js\"></script>");
                //sbjs.AppendLine("<script src=\"Scripts/kendoR32023/jquery.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/Stuk-jszip/dist/jszip.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/kendoR32023/kendo.all.min.js\"></script>");
                sbjs.AppendLine("<script>kendo.ui['Button'].fn.options['size']=\"small\";</script>");
                sbjs.AppendLine("<script src=\"https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/core.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLobjects.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLCtrls.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/splitter4.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/SSOA.js\"></script>");
                return sbjs.ToString();
            }
        }

        public string jsnosplitterScripts
        {
            get
            {
                System.Text.StringBuilder sbjs = new System.Text.StringBuilder();
                sbjs.AppendLine("<script src=\"https://code.jquery.com/jquery-3.4.1.min.js\"></script>");
                //sbjs.AppendLine("<script src=\"Scripts/kendoR32023/jquery.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/Stuk-jszip/dist/jszip.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/kendoR32023/kendo.all.min.js\"></script>");
                sbjs.AppendLine("<script>kendo.ui['Button'].fn.options['size']=\"small\";</script>");
                sbjs.AppendLine("<script src=\"https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/core.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLobjects.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/NGLCtrls.js\"></script>");
                sbjs.AppendLine("<script src=\"Scripts/NGL/v-8.5.4.006/SSOA.js\"></script>");
                return sbjs.ToString();
            }
        }


        public string cssReference
        {
            get
            {
                System.Text.StringBuilder sbjs = new System.Text.StringBuilder();

                //sbjs.AppendFormat("<link rel=\"stylesheet\" href=\"https://kendo.cdn.telerik.com/themes/6.3.0/default/default-main.css\" />");

                sbjs.AppendFormat("<link href=\"Content/kendoR32023/{0}.css\" rel=\"stylesheet\" />\n\r", validUserTheme(UserTheme));
                //sbjs.AppendFormat(" <link href=\"Content/kendoR32023/classic-opal.css\" rel=\"stylesheet\" />");
                //sbjs.AppendFormat(" <link href=\"Content/kendoR32023/{0}.css\" rel=\"stylesheet\" />\n\r", UserTheme);

                //sbjs.AppendLine("<link href=\"Content/kendoR32023/kendo.common.min.css\" rel=\"stylesheet\" />");
                //sbjs.AppendFormat("<link href=\"Content/kendoR32023/kendo.{0}.min.css\" rel=\"stylesheet\" />\n\r", UserTheme);
                //sbjs.AppendFormat("<link href=\"Content/kendoR32023/kendo.{0}.mobile.min.css\" rel=\"stylesheet\" />\n\r", UserTheme);
                //sbjs.AppendLine("<link href=\"Content/NGL/v-8.5.4.001/common.css\" rel=\"stylesheet\" />");
                sbjs.AppendFormat("<link href=\"Content/NGL/v-8.5.4.001/ngl-{0}.css\" rel=\"stylesheet\" />\n\r", validUserTheme(UserTheme));
                return sbjs.ToString();
            }
        }

        public string validUserTheme(string sUserTheme)
        {
            string sValidTheme = sUserTheme;
            //create an array of valid themes           

            string[] sThemes = new string[]{ "classic-opal-dark", "classic-opal", "bootstrap-main", "classic-main", "bootstrap-main-dark", "material-main", "material-arctic", "material-eggplant", "material-lime", "classic-metro", "classic-metro-dark", "classic-moonlight", "fluent-main", "classic-silver", "classic-uniform" };
            //create a switch statement to validate the theme
            if (string.IsNullOrEmpty(sValidTheme))
            {
                sValidTheme = "classic-main";
            }
           
                switch (sValidTheme)
                {
                    case "black":
                        sValidTheme = "bootstrap-main-dark";
                        break;
                    case "highcontrast":
                        sValidTheme = "classic-opal-dark";
                        break;
                    case "blueopal":
                        sValidTheme = "classic-opal";
                        break;
                    case "bootstrap":
                        sValidTheme = "bootstrap-main";
                        break;
                    case "default":
                        sValidTheme = "classic-main";
                        break;
                    case "materialblack":
                        sValidTheme = "bootstrap-main-dark";
                        break;
                    case "material":
                        sValidTheme = "material-main";
                        break;
                    case "moonlight":
                        sValidTheme = "classic-moonlight\"";
                        break;
                    case "office365":
                        sValidTheme = "fluent-main";
                        break;
                    case "silver":
                        sValidTheme = "classic-silver";
                        break;
                    case "metroblack":
                        sValidTheme = "classic-metro";
                        break;
                    case "uniform":
                        sValidTheme = "classic-uniform";
                        break;
                    default:
                        break;
                }
            if (!sThemes.Contains(sValidTheme)) { sValidTheme = "classic-opal"; }
            
            return sValidTheme;
        }

        public DAL.Models.SSOResults SSOR { get; set; }
        public bool blnUseSSR { get; set; }
        /// <summary>
        /// refresh the user information using sesion data and querystrings
        /// </summary>
        /// <remarks>
        /// Modified by RHR for v-8.4.x on 03/27/2021
        ///   new rules for validate user 
        ///   we no longer expect to use querystring to pass in usercontrol
        ///   Now this is only possible on the Login.aspx redirect page
        ///   the Login.aspx redirect page will do the following:
        ///   1. check for query strings that include usercontrol and caller
        ///   2. if both are provided the page will create the session variable for useer control and return authentication to the caller
        ///   3. if user control is 0 but caller is not let Javascript read local storage and authenticate the user with local data
        ///         SSOA js will redirect back to Login.aspx page after authentication with valid data 
        ///         or it will redirect to NGLLogin page
        ///   4. if both user and caller are not valid redirect to NGLLogin page
        ///   So: each page will call refreshUserControl on load
        ///         a) check session value if blank (check query string for legacy calls to page)
        ///             if no data redirect to Login.aspx page with caller and zero for usercontrol
        ///         b) if we have a user control check if user is allowed to access the page
        ///         c) if user does not have permissions redirect to home (Default.aspx)
        /// </remarks>
        public void refreshUserControl()
        {
            this.blnUseSSR = false;
            this.SSOR = new DAL.Models.SSOResults();
            HttpRequest request = HttpContext.Current.Request;
            string authUN = "";
            string authT = "";
            // Begin Modified by RHR for v-8.4.x on 03/27/2021
            //a) check session
                if (UserControl != 0) 
            {
                //b)
                if (!CanUserAccessScreen(false))
                {

                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/Default.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                DAL.Models.SSOResults ssosr = DynamicsTMS365.Controllers.SSOAController.NetGetSSOAccount(UserControl);
                if (ssosr != null && ssosr.UserSecurityControl != 0)
                {
                    this.SSOR = ssosr;
                    if (this.PageControl != (int)Utilities.PageEnum.ChangePassword  && this.SSOR.UserMustChangePassword == true)
                    {
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/ChangePassword.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            return;
                    }
                    
                }
            } else
            {
                // check legacy logic 
                // do we have a querystring?
                var sUserControl = request.QueryString["uc"];
                int ius = 0;
                if (sUserControl != null)
                {
                    var suc = sUserControl.ToString();
                    int.TryParse(suc, out ius);                   
                    UserControl = ius;
                    DAL.Models.SSOResults ssosr = DynamicsTMS365.Controllers.SSOAController.NetGetSSOAccount(UserControl);
                    if (ssosr != null && ssosr.UserSecurityControl != 0)
                    {
                        this.SSOR = ssosr;
                    }
                } else if (request.QueryString.GetValues("AuthUN") != null && request.QueryString.GetValues("AuthT") != null)
                {
                    if (request.QueryString.GetValues("AuthUN").Length != 0 && request.QueryString.GetValues("AuthT").Length != 0)
                    {
                        authUN = request.QueryString.GetValues("AuthUN")[0];
                        authT = request.QueryString.GetValues("AuthT")[0];
                        DAL.Models.SSOResults ssosr = DynamicsTMS365.Controllers.SSOAController.GetNGLLegacySSOAForUser(authUN, authT);
                        if (ssosr != null && ssosr.UserSecurityControl != 0)
                        {
                            this.blnUseSSR = true;
                            this.UserControl = ssosr.UserSecurityControl;
                            this.SSOR = ssosr;
                            //s.Replace(@"\", @"\\");
                            this.UserName = authUN;
                            this.UserToken = ssosr.USATToken;
                            this.UserTheme = ssosr.UserTheme365;
                            //save to server user dictionary
                            if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(ssosr.UserSecurityControl))
                            {
                                DynamicsTMS365.Utilities.GlobalSSOResultsByUser[ssosr.UserSecurityControl] = ssosr;
                            }
                            else
                            {
                                DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(ssosr.UserSecurityControl, ssosr);
                            }

                        }
                    }
                }
                if (UserControl != 0)
                {
                    //the idea here is that previously we only used the query string once so if it is passed to the 
                    //page refresh the menutree on long in.
                    if (Utilities.GlobalMenuTreeByUser.ContainsKey(UserControl)) { Utilities.GlobalMenuTreeByUser.Remove(UserControl); }
                    if (!CanUserAccessScreen(false))
                    {

                        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/Default.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    if (this.PageControl != (int)Utilities.PageEnum.ChangePassword && this.SSOR.UserMustChangePassword == true)
                    {
                        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/ChangePassword.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                } else
                {
                    //no user control redirect to Login.aspx with page name
                    //"../Login?uc=" + uc + "&caller=" + caller;
                    string sCaller = "Default.aspx";
                    string[] sArr = this.Page.AppRelativeVirtualPath.Split('/');
                    if (sArr != null && sArr.Count() > 0)
                    {
                        sCaller = sArr[sArr.Count() - 1];
                    }
                    
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/Login.aspx?uc=0&caller=" + sCaller, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                

            }



            // old legacy code removed 
            //if (request.QueryString.GetValues("AuthUN") != null && request.QueryString.GetValues("AuthT") != null)
            //{
            //    if (request.QueryString.GetValues("AuthUN").Length != 0 && request.QueryString.GetValues("AuthT").Length != 0)
            //    {
            //        authUN = request.QueryString.GetValues("AuthUN")[0];
            //        authT = request.QueryString.GetValues("AuthT")[0];
            //        DAL.Models.SSOResults ssosr = DynamicsTMS365.Controllers.SSOAController.GetNGLLegacySSOAForUser(authUN, authT);
            //        if (ssosr != null && ssosr.UserSecurityControl != 0)
            //        {
            //            this.blnUseSSR = true;
            //            this.UserControl = ssosr.UserSecurityControl;
            //            this.SSOR = ssosr;
            //            //s.Replace(@"\", @"\\");
            //            this.UserName = authUN;
            //            this.UserToken = ssosr.USATToken;
            //            this.UserTheme = ssosr.UserTheme365;
            //            //save to server user dictionary
            //            if (DynamicsTMS365.Utilities.GlobalSSOResultsByUser.ContainsKey(ssosr.UserSecurityControl))
            //            {
            //                DynamicsTMS365.Utilities.GlobalSSOResultsByUser[ssosr.UserSecurityControl] = ssosr;
            //            }
            //            else
            //            {
            //                DynamicsTMS365.Utilities.GlobalSSOResultsByUser.Add(ssosr.UserSecurityControl, ssosr);
            //            }

            //        }
            //    }
            //}
            //else if (UserControl == 0) //step 2 UserControl property is linked to session variable see if it is zero
            //{
            //   //check if we have a query string (old logic)
            //    var sUserControl = request.QueryString["uc"];
            //    int ius = 0;
            //    if (sUserControl != null)
            //    {
            //        var suc = sUserControl.ToString();
            //        int.TryParse(suc, out ius);
            //        //the idea here is that previously we only used the query string once so if it is passed to the 
            //        //page refresh the menutree on long in.
            //        if (Utilities.GlobalMenuTreeByUser.ContainsKey(ius)) { Utilities.GlobalMenuTreeByUser.Remove(ius); }
            //    }

            //    UserControl = ius;
            //}
            //if (!CanUserAccessScreen(false))
            //{

            //    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["WebBaseURI"] + "/Default.aspx", false);
            //    Context.ApplicationInstance.CompleteRequest();
            //    return;                
            //}

        }

        public string sWaitMessage
        {
            get
            {
              return "<div id=\"h1Wait\" style=\"margin:10px; position: absolute; z-index: 99999; top:0px; left:400px;\" ><span style=\"vertical-align:middle;\">Please Wait Loading&nbsp;</span><img style=\"vertical-align:middle;\" border=\"0\" alt=\"Waiting\" src=\"../Content/NGL/loading5.gif\" ></div>";
            }
        }

        /// <summary>
        /// Determines if the user has permission to access the screen
        /// </summary>
        /// <remarks>Created By LVV on 6/1/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues</remarks>
        public bool CanUserAccessScreen()
        {
            return CanUserAccessScreen(true);
        }

        /// <summary>
        /// Check if the user can access the selected screen and if the license is valid
        /// </summary>
        /// <param name="blnThrowException"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Modified by RHR for v-8.5.3.006 call Check Auth Code to validate Auth License
        /// </remarks>
        public bool CanUserAccessScreen(bool blnThrowException)
        {
            bool bRet = false;
            int[] iOpenPages = new int[] { 0,10,11,14,19,20,21,22,23};

            if (iOpenPages.Contains(this.PageControl))
            {
                return true; // everyone has access to open pages iOpenPages do not require a license
            } else  {

                // Modified by RHR for v-8.5.3.006 call Check Auth Code to validate Auth License
                //bRet = DynamicsTMS365.Controllers.SSOAController.CheckAuthCode();

                //if (bRet)
                //{
                try {
                    bRet = DynamicsTMS365.Controllers.SSOAController.CanUserAccessScreen(this.PageControl, this.UserControl, blnThrowException);

                }
                catch (Exception ex)
                {
                    FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                    if (fault.Reason == "E_AccessGranted")
                    {
                        PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
                        return true;
                    } else if (fault._Error.Message == "E_AccessGranted")
                    {
                        PageErrorsOrWarnings = "<h4 style='padding:5px; color:red;'>" + fault.formatMessageNotLocalized() + "</ h4>";
                        return true;

                    } else { throw ex; }
                }

                //}
            }
            return bRet;


        }
        public string UserCultureInfo { get; set; }
        public string UserTimeZine { get; set; }
    }
}