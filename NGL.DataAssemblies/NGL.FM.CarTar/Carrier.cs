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
    public class Carrier : TarBaseClass
    {
        /*******  Standard WCF Data Methods ************
         * GetFirstRecord
         * GetPreviousRecord
         * GetNextRecord
         * GetLastRecord
         * Get{object}Filtered
         * Get{object}sFiltered
         * CreateRecord
         * DeleteRecord
         * UpdateRecord
         * UpdateRecordQuick
         * UpdateRecordNoReturn
         * *********************************************/

        #region " Constructors "
        public Carrier(DAL.WCFParameters oParameters)
                : base()
	        {
                if (oParameters == null)
                {
                    populateSampleParameterData();  //this will not really be used in production
                }
                else
                {
                    Parameters = oParameters;
                }

	        }

	    #endregion
        
        #region " Properties"

        private string _SourceClass = "NGL.FM.CarTar.Carrier";
        public override  string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        #endregion

        #region " Methods"

       

        #endregion

        #region " Test Methods"

        
        #endregion
   
    }
}
