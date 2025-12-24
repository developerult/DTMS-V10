using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class WorkFlowSetting
    {       
            
        public int fieldID { get; set; } //ID used for record counter cmPageDetail.PageDetControl
        public string fieldName { get; set; } //name of field cmPageDetail.PageDetName
        public string fieldDefaultValue { get; set; } //default value for insert cmPageDetail.PageDetMetaData
        public bool fieldVisible { get; set; } //stores the users visible selection option.
        public bool fieldReadOnly { get; set; } //stores the users readonly option enabled true or false

    }

}