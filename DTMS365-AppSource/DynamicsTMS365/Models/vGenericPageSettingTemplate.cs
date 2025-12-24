using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{


    /// <summary>
    /// Created by RHR for v-8.2 on 2/08/2019
    ///    used in content management to map user workflow settings to a generic
    ///    data source.a table does not actually exist so the backend controller
    ///    must determine how the data is saved and/or used, typically the data
    ///    is saved as a key value pair JSON data ins the tblUserPageSettings.UserPSMetaData
    ///    for the assoicated page.
    ///    NOTE:  an LTS object is not generally used just a model in the client.  if an LTS object is required
    ///        a new mapping must be created in one of the DAL libraries
    /// </summary>
    public class vGenericPageSettingTemplate
    {
        public string TextValue1  { get; set; }
        public string TextValue2  { get; set; }
        public string TextValue3  { get; set; }
        public string TextValue4  { get; set; }
	    public double NumericValue1 { get; set; }
        public double NumericValue2  { get; set; }
        public double NumericValue3  { get; set; }
        public double NumericValue4  { get; set; }
        public DateTime DateValue1  { get; set; }
	    public DateTime DateValue2  { get; set; }
        public DateTime DateValue3 { get; set; }
        public DateTime DateValue4  { get; set; }

        public bool BoolValue1 { get; set; }
        public bool BoolValue2 { get; set; }
        public bool BoolValue3 { get; set; }
        public bool BoolValue4 { get; set; }

    }
}