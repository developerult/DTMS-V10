using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDIElement class for Elements
    /// </summary>
    /// <remarks>
    /// Created by SRP on 2/22/18
    /// </remarks>
    public class EDIElement
    {
        /// <summary>
        /// Property ElementControl Control/ID as INT
        /// </summary>
        public int ElementControl { get; set; }
        /// <summary>
        /// ElementName Name Property as STRING
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// ElementDescription Property as string
        /// </summary>
        public string ElementDescription { get; set; }
        /// <summary>
        /// ElementEDIDataTypeControl Name Property as int
        /// </summary>
        public int ElementEDIDataTypeControl { get; set; }
        /// <summary>
        /// ElementMinLength Name Property as int
        /// </summary>
        public int ElementMinLength { get; set; }
        /// <summary>
        /// ElementMaxLength Name Property as int
        /// </summary>
        public int ElementMaxLength { get; set; }
        /// <summary>
        /// ElementValidationTypeControl Name Property as int
        /// </summary>
        public int ElementValidationTypeControl { get; set; }
        /// <summary>
        /// ElementFormattingFnControl Name Property as int
        /// </summary>
        public int ElementFormattingFnControl { get; set; }
        /// <summary>
        /// ElementDisabled Property as BOOLEAN
        /// </summary>
        public bool ElementDisabled { get; set; }
        /// <summary>
        /// ElementCreateDate Property as DateTime
        /// </summary>
        public DateTime ElementCreateDate { get; set; }
        private string _ElementCreateUser; //Modified by SRP on 2/22/18
        /// <summary>
        /// ElementCreateUser Property as String
        /// </summary>
        public string ElementCreateUser
        {
            get { return _ElementCreateUser.Left(100); } //uses extension string method Left
            set { _ElementCreateUser = value.Left(100); }//Modified by SRP on 2/22/18
        }
        /// <summary>
        /// ElementModDate Property as DateTime
        /// </summary>
        public DateTime ElementModDate { get; set; }

        private string _ElementModUser;
        /// <summary>
        /// ElementModUser Property as String
        /// </summary>
        public string ElementModUser
        {
            get { return _ElementModUser.Left(100); } //uses extension string method Left
            set { _ElementModUser = value.Left(100); }// Modified by SRP on 2/22/18

        }


        private byte[] _ElementUpdated;
        /// <summary>
        /// ElementUpdated Property as STRING
        /// </summary>
        public string ElementUpdated
        {
            get
            {
                if (this._ElementUpdated != null)
                {

                    return Convert.ToBase64String(this._ElementUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._ElementUpdated = null;

                }

                else
                {

                    this._ElementUpdated = Convert.FromBase64String(value);

                }

            }
        }

        /// <summary>
        /// setUpdated Method for setting Update RowVersion Number
        /// </summary>
        /// <param name="val"></param>
        public void setUpdated(byte[] val) { _ElementUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the ElementUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _ElementUpdated; }
    }
}