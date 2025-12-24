using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    /// <summary>
    /// EDISegment class for Segment Types
    /// </summary>
    /// <remarks>
    /// Created By SRP on 2/26/18
    /// </remarks>
    public class EDISegment
    {
        /// <summary>
        /// Property SegmentControl Control/ID as INT
        /// </summary>
        public int SegmentControl { get; set; }
        /// <summary>
        /// SegmentName Name Property as STRING
        /// </summary>
        public string SegmentName { get; set; }
        /// <summary>
        /// SegmentDesc Property as string
        /// </summary>
        public string SegmentDesc { get; set; }
        /// <summary>
        /// SegmentMinCount Property as INT
        /// </summary>
        public string SegmentMinCount { get; set; }
        /// <summary>
        /// SegmentMaxCount Name Property as INT
        /// </summary>
        public string SegmentMaxCount { get; set; }

        /// <summary>
        /// SegmentDisabled Property as BOOLEAN
        /// </summary>
        public bool SegmentDisabled { get; set; }
        /// <summary>
        /// EDITCreateDate Property as DateTime
        /// </summary>
        public DateTime SegmentCreateDate { get; set; }
        private string _SegmentCreateDate;
        //Modified by SRP on 2/26/18
        /// <summary>
        /// SegmentCreateDate Property as String
        /// </summary>
        public string SegmentCreateUser
        {
                get { return _SegmentCreateDate.Left(100); } //uses extension string method Left
                set { _SegmentCreateDate = value.Left(100); }//Modified by SRP on 2/26/18
        }
        /// <summary>
        /// SegmentModDate Property as DateTime
        /// </summary>
        public DateTime SegmentModDate { get; set; }

            private string _SegmentModUser;
        /// <summary>
        /// SegmentModUser Property as String
        /// </summary>
        public string SegmentModUser
        {
                get { return _SegmentModUser.Left(100); } //uses extension string method Left
                set { _SegmentModUser = value.Left(100); }// Modified by SRP on 2/26/18

        }


            private byte[] _SegmentUpdated;
        /// <summary>
        /// SegmentUpdated Property as STRING
        /// </summary>
        public string SegmentUpdated
        {
                get
                {
                    if (this._SegmentUpdated != null)
                    {

                        return Convert.ToBase64String(this._SegmentUpdated);

                    }
                    return string.Empty;
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                    {

                        this._SegmentUpdated = null;

                    }

                    else
                    {

                        this._SegmentUpdated = Convert.FromBase64String(value);

                    }

                }
            }

            /// <summary>
            /// setUpdated Method for setting Update RowVersion Number
            /// </summary>
            /// <param name="val"></param>
            public void setUpdated(byte[] val) { _SegmentUpdated = val; }
        /// <summary>
        /// getUpdated Method for getting the SegmentUpdated
        /// </summary>
        /// <returns></returns>
        public byte[] getUpdated() { return _SegmentUpdated; }
        }
    }