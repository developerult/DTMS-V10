using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGLTrimbleServices
{

    [Serializable()]
    public class clsGlobalStopData
    {

        private List<int> mintBadAddressControls;

        public List<int> BadAddressControls
        {
            get
            {
                return mintBadAddressControls;
            }
            set
            {               
                mintBadAddressControls = value;
            }
        }

        public  int getBadAddressControl(int index)
        {
            if (mintBadAddressControls.Count() > index)
            {
                return mintBadAddressControls.ToArray()[index];
            } else
            {
                return 0;
            }            
        }
        public void setBadAddressControl(int Control)
        {
            if (!mintBadAddressControls.Contains(Control))
            {
                mintBadAddressControls.Add(Control);
            }            
        }

        private string mstrFailedAddressMessage = "";
        public string FailedAddressMessage
        {
            get
            {
                return mstrFailedAddressMessage;
            }
            set
            {
                mstrFailedAddressMessage = value;
            }
        }

        private int mintBadAddressCount = 0;
        public int BadAddressCount
        {
            get
            {
                return mintBadAddressCount;
            }
            set
            {
                mintBadAddressCount = value;
            }
        }

        private double mdblTotalMiles = 0;
        public double TotalMiles
        {
            get
            {
                return mdblTotalMiles;
            }
            set
            {
                mdblTotalMiles = value;
            }
        }

        private string mstrOriginZip = "";
        public string OriginZip
        {
            get
            {
                return mstrOriginZip;
            }
            set
            {
                mstrOriginZip = value;
            }
        }

        private string mstrDestZip = "";
        public string DestZip
        {
            get
            {
                return mstrDestZip;
            }
            set
            {
                mstrDestZip = value;
            }
        }

        private double mdblAutoCorrectBadLaneZipCodes = 0;
        public double AutoCorrectBadLaneZipCodes
        {
            get
            {
                return mdblAutoCorrectBadLaneZipCodes;
            }
            set
            {
                mdblAutoCorrectBadLaneZipCodes = value;
            }
        }

        private double mdblBatchID = 0;
        public double BatchID
        {
            get
            {
                return mdblBatchID;
            }
            set
            {
                mdblBatchID = value;
            }
        }

        private string mstrLastError = "";
        public string LastError
        {
            get
            {
                return mstrLastError;
            }
        }

    }
}
