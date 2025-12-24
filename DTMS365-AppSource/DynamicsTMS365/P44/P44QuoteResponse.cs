using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.P44
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RateQuoteResponse", Namespace = "", IsNullable = false)]
    public partial class P44QuoteResponse
    {
        private string scacField;

        private string vendorField;

        private string contractIdField;

        private terminalInfo originTerminalField;

        private terminalInfo destinationTerminalField;

        private List<serviceError> errorsField;

        private string carrierNoteField;

        private string deliveryDateField;

        private string quoteNumberField;

        private string expirationDateField;

        private string quoteDateField;

        private rateDetail rateDetailField;

        private string serviceTypeField;

        private int totalPalletsField;

        private bool totalPalletsFieldSpecified;

        private int totalPiecesField;

        private bool totalPiecesFieldSpecified;

        private int totalWeightField;

        private bool totalWeightFieldSpecified;

        private int transitTimeField;

        private bool transitTimeFieldSpecified;

        private List<rateDetail> alternateRatesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string scac
        {
            get
            {
                return this.scacField;
            }
            set
            {
                this.scacField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vendor
        {
            get
            {
                return this.vendorField;
            }
            set
            {
                this.vendorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contractId
        {
            get
            {
                return this.contractIdField;
            }
            set
            {
                this.contractIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public terminalInfo originTerminal
        {
            get
            {
                return this.originTerminalField;
            }
            set
            {
                this.originTerminalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public terminalInfo destinationTerminal
        {
            get
            {
                return this.destinationTerminalField;
            }
            set
            {
                this.destinationTerminalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("error", IsNullable = false)]
        public List<serviceError> errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string carrierNote
        {
            get
            {
                return this.carrierNoteField;
            }
            set
            {
                this.carrierNoteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string deliveryDate
        {
            get
            {
                return this.deliveryDateField;
            }
            set
            {
                this.deliveryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string quoteNumber
        {
            get
            {
                return this.quoteNumberField;
            }
            set
            {
                this.quoteNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string quoteDate
        {
            get
            {
                return this.quoteDateField;
            }
            set
            {
                this.quoteDateField = value;
            }
        }

        /// <remarks/>
        public rateDetail rateDetail
        {
            get
            {
                return this.rateDetailField;
            }
            set
            {
                this.rateDetailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceType
        {
            get
            {
                return this.serviceTypeField;
            }
            set
            {
                this.serviceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int totalPallets
        {
            get
            {
                return this.totalPalletsField;
            }
            set
            {
                this.totalPalletsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalPalletsSpecified
        {
            get
            {
                return this.totalPalletsFieldSpecified;
            }
            set
            {
                this.totalPalletsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int totalPieces
        {
            get
            {
                return this.totalPiecesField;
            }
            set
            {
                this.totalPiecesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalPiecesSpecified
        {
            get
            {
                return this.totalPiecesFieldSpecified;
            }
            set
            {
                this.totalPiecesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int totalWeight
        {
            get
            {
                return this.totalWeightField;
            }
            set
            {
                this.totalWeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalWeightSpecified
        {
            get
            {
                return this.totalWeightFieldSpecified;
            }
            set
            {
                this.totalWeightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int transitTime
        {
            get
            {
                return this.transitTimeField;
            }
            set
            {
                this.transitTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transitTimeSpecified
        {
            get
            {
                return this.transitTimeFieldSpecified;
            }
            set
            {
                this.transitTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("alternateRate", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<rateDetail> alternateRates
        {
            get
            {
                return this.alternateRatesField;
            }
            set
            {
                this.alternateRatesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("error", Namespace = "", IsNullable = false)]
    public partial class serviceError
    {

        private string errorCodeField;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

}