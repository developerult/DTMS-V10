using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace DynamicsTMS365
{   
    public class FaultExceptionEventArgs
    {
        public bool _Cancelled { get; }
        public Exception _Error { get; }
        public object _UserState { get; }

        public FaultExceptionEventArgs()
        {
          
        }

        public FaultExceptionEventArgs(Exception error, bool canceled, object userState, string reason, string message)
        {
            _Reason = reason;
            _Message = message;
            _Error = error;
            _Cancelled = canceled;
            _UserState = userState;
        }

        public FaultExceptionEventArgs(Exception error, bool canceled, object userState, string reason, string message, string detail)
        {
            _Reason = reason;
            _Message = message;
            _Detail = detail;
            _Error = error;
            _Cancelled = canceled;
            _UserState = userState;
        }

        public FaultExceptionEventArgs(Exception error, bool canceled, object userState, string reason, string message, string detail, string exceptiondetails)
        {
            _Reason = reason;
            _Message = message;
            _Detail = detail;
            _ExceptionDetails = exceptiondetails;
            _Error = error;
            _Cancelled = canceled;
            _UserState = userState;
        }

        public FaultExceptionEventArgs(Exception error, bool canceled, object userState, string reason, string message, string detail, List<string> exceptiondetaillist)
        {
            _Reason = reason;
            _Message = message;
            _Detail = detail;
            _DetailsList = exceptiondetaillist;
            _Error = error;
            _Cancelled = canceled;
            _UserState = userState;
        }

        

        //public FaultExceptionEventArgs(Exception error, bool canceled, object userState, string reason, string detail, string exceptiondetails, object dList)
        //{
        //    _Reason = reason;
        //    _Detail = detail;
        //    _ExceptionDetails = exceptiondetails;
        //    _Error = error;
        //    _Cancelled = canceled;
        //    _UserState = userState;
        //    try
        //    {
        //        if ((dList != null))
        //        {
        //            foreach (object s in dList)
        //            {
        //                this.DetailsList.Add(s.Text);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ignore any errors when process the details list
        //    }

        //}


        public string Detail
        {
            get { return _Detail; }
        }

        private string _Detail;

        public string Message
        {
            get { return _Message; }
        }

        private string _Message;

        public string Reason
        {
            get { return _Reason; }
        }

        private string _Reason;
        private string _ExceptionDetails;
        public string ExceptionDetails
        {
            get { return _ExceptionDetails; }
        }

        private List<string> _DetailsList = new List<string>();
        public List<string> DetailsList
        {
            get
            {
                if (_DetailsList == null)
                    _DetailsList = new List<string>();
                return _DetailsList;
            }
            set { _DetailsList = value; }
        }

        private HttpStatusCode _StatusCode = HttpStatusCode.InternalServerError;
        public HttpStatusCode StatusCode
        {
            get
            {
                return _StatusCode;
            }
            set { _StatusCode = value; }
        }

        public string formatMessageShort()
        {
            return string.Format("There was a problem processing your request. Reason: {0}, Message: {1}", Utilities.getLocalizedMsg(Reason), Utilities.getLocalizedMsg(Message));
        }

        public string formatMessageNotLocalized()
        {
            return string.Format("There was a problem processing your request. Reason: {0}, Message: {1}", Reason, Message);
        }

        public string formatMessage()
        {
            string sMsg = "";
            if (!string.IsNullOrWhiteSpace(Detail))
            {
                sMsg = Utilities.getLocalizedMsg(Detail);
            }            
            if (this.DetailsList != null && this.DetailsList.Count > 0)
            {
                List<string> lDetails = new List<string>();
                foreach (string sd in this.DetailsList)
                {
                    lDetails.Add(Utilities.getLocalizedMsg(sd));
                }
                if (!string.IsNullOrWhiteSpace(sMsg))
                {
                    sMsg = string.Format(sMsg, lDetails.ToArray());
                }else
                {
                    sMsg = String.Join(",", lDetails);
                }
            }
            sMsg = Utilities.getLocalizedMsg(Reason) + ":\n\r </br>" + Utilities.getLocalizedMsg(Message) + ":\n\r </br> " + sMsg;

            return sMsg;
        }

        public string formatMessage(string Reason, string sMsg, List<string> DetailsList)
        {
            sMsg = Utilities.getLocalizedMsg(sMsg);
            if (DetailsList != null && DetailsList.Count > 0)
            {
                sMsg = string.Format(sMsg, DetailsList.ToArray());
            }
            if (!string.IsNullOrWhiteSpace(Reason))
            {
                sMsg = Utilities.getLocalizedMsg(Reason) + ":  " + sMsg;
            }

            return sMsg;
        }

        public void GetLocalReasonAndMessage()
        {
            _Reason = Utilities.getLocalizedMsg(Reason);
            _Message = Utilities.getLocalizedMsg(Message);
           
        }


    }
}