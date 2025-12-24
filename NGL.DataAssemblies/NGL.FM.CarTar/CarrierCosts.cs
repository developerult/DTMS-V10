using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DTran = Ngl.Core.Utility.DataTransformation;


namespace NGL.FM.CarTar
{
    /// <summary>
    /// CarrierCosts Class.
    /// This class is used to store carrier cost information during the rating process.
    /// </summary>
    public class CarrierCosts
    {
        #region " Constructors "

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierCosts()
        {
            _bookFees = new List<DTO.BookFee>();
        }


        #endregion

        #region " Properties"

        private decimal _lineHaulCharge = -1;   // -1 = not assigned
        public decimal LineHaulCharge
        {
            get { return _lineHaulCharge; }
            set { _lineHaulCharge = value; }
        }

        private List<DTO.BookFee> _bookFees = null;
        public List<DTO.BookFee> BookFees
        {
            get { return _bookFees; }
            set { _bookFees = value; }
        }
        
        

        #endregion

        #region "Public Methods"


        #endregion

        #region "Helper Methods"

 
        #endregion
    }
}
