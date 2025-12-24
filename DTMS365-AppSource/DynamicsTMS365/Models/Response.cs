using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// Model Response
    /// </summary>
    /// <remarks>
    /// Modified by RHR for v-8.3.0.002 on 12/21/2020
    /// Modified by RHR for v-8.5.4.002 on 08/09/2023 added new properties for Token Authentication
    /// </remarks>
    public class Response
    {
        // properties 
        /// <summary>
        /// Data Property as Array
        /// </summary>
        public Array Data { get; set; }

        public string ErrTitle { get; set; }
        public string WarningTitle { get; set; }
        public string LogTitle { get; set; }
        public string MsgTitle { get; set; }
        public DTO.NGLMessage[]  Log { get; set; }

        public DTO.NGLMessage[] Err { get; set; }
        
        public DTO.NGLMessage[]  Message { get; set; }

        public DTO.NGLMessage[] Warn { get; set; }

        /// <summary>
        /// Total Number of Elements
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Errors
        /// </summary>
        public string Errors { get; set; }
        /// <summary>
        /// Warnings are displayed in an NGL Alert popup 
        /// </summary>
        public string Warnings { get; set; }
        /// <summary>
        /// Messages are displayed in an NGL Alert popup 
        /// </summary>
        public string Messages { get; set; }
        /// <summary>
        /// StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public bool Status { get; set; }
        //Begin Added by RHR for v-8.3.0.002 on 12/21/2020
        public bool AsyncMessagesPossible { get; set; }
        public long AsyncMessageKey { get; set; }
        public int AsyncTypeKey { get; set; }
        // End
        //Begin Added by RHR for v-8.5.5.002 on 08/3/2023
        public string TokenAuthCode { get; set; }   
        public string TokenSecrectCode { get; set; } 
        public string TokenClientID { get; set; }      

        // constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public Response(Array data, int count)
        {
            this.Data = data; this.Count = count;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errors"></param>
        public Response(string errors)
        {
            this.Errors = errors;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public Response()
        {

        }

        private byte[] _LastUpdatedTimeStamp;

        /// <summary>
        /// cmLocalUpdated should be bound to UI, _cmLocalUpdated is only bound on the controller
        /// </summary>
        public string LastUpdatedTimeStamp
        {
            get
            {
                if (this._LastUpdatedTimeStamp != null)
                {

                    return Convert.ToBase64String(this._LastUpdatedTimeStamp);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LastUpdatedTimeStamp = null;

                }

                else
                {

                    this._LastUpdatedTimeStamp = Convert.FromBase64String(value);

                }

            }
        }
        /// <summary>
        /// setUpdated is for Updating TimeStamp Data
        /// </summary>
        public void setUpdated(byte[] val) { _LastUpdatedTimeStamp = val; }
        /// <summary>
        /// getting thr Timestamp data
        /// </summary>
        /// <returns>Returning Byte Array</returns>
        public byte[] getUpdated() {
            return _LastUpdatedTimeStamp == null ? new byte[0] : _LastUpdatedTimeStamp;
        }


        /// <summary>
        /// Setting the Last Updated TimeStamp
        /// </summary>
        /// <param name="request"></param>
        public void fillLastUpdatedTimeStampFromRequest(HttpRequest request)
        {
            this.LastUpdatedTimeStamp = request["LastUpdatedTimeStamp"] ?? "";


        }

        public void populateDefaultUnathorizedResponseMessage()
        {
            FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
            Errors = fault.formatMessage("E_NotAuthProcedure", "M_ContactSupportForAccess", null);
            StatusCode = HttpStatusCode.Unauthorized;            
        }

        public void populateDefaultInvalidFilterResponseMessage()
        {            
                FaultExceptionEventArgs fault = new FaultExceptionEventArgs();
                StatusCode = HttpStatusCode.BadRequest;
                List<string> DetailsList = new List<string>();
                Errors = fault.formatMessage("SelectFilter", "E_InvalidFilter", DetailsList);               
            
        }
    }
}