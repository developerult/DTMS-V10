using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.P44
{

    public enum SeverityEnum
    {
        //
        // Summary:
        //     /// Enum ERROR for "ERROR" ///
        ERROR = 0,
        //
        // Summary:
        //     /// Enum WARNING for "WARNING" ///
        WARNING = 1,
        //
        // Summary:
        //     /// Enum INFO for "INFO" ///
        INFO = 2
    }

    public enum SourceEnum
    {
        //
        // Summary:
        //     /// Enum SYSTEM for "SYSTEM" ///
        SYSTEM = 0,
        //
        // Summary:
        //     /// Enum CAPACITYPROVIDER for "CAPACITY_PROVIDER" ///
        CAPACITYPROVIDER = 1,
        CAPACITY_PROVIDER = 2

    }
    //
    // Summary:
    //     /// Initializes a new instance of the P44SDK.V4.Model.Message class. ///
    //
    // Parameters:
    //   Severity:
    //     The severity of this message..
    //
    //   Msg:
    //     Message informational text..
    //
    //   Diagnostic:
    //     Diagnostic information, often originating from the capacity provider..
    //
    //   Source:
    //     The originator of this message - - either project44 (the system) or the capacity
    //     provider..
    public class Message
    {
        public Message() { }

        public Message(
            SeverityEnum? severity = default(SeverityEnum?), 
            string msg = null, 
            string diagnostic = null, 
            SourceEnum? source = default(SourceEnum?))
        {
            this.Diagnostic = diagnostic;
            if (source == SourceEnum.CAPACITY_PROVIDER)
            {
                source = SourceEnum.CAPACITYPROVIDER;
            }
            this.Source = source;
            this.Severity = severity;
            this.message = msg;
        }
        public string Diagnostic { get; set; }
            
        public SeverityEnum? Severity { get; set; }

        public SourceEnum? Source { get; set; }
        
        public string message { get; set; }

           
          
    }
}
