namespace NGL.FM.DAT.DisplayHelpers
{
	public class ServiceErrorDisplayHelper : DisplayHelper<ServiceError>
	{
		public ServiceErrorDisplayHelper(ServiceError data) : base(data) {}

		protected override void Display(int indent)
		{
			DisplayLabel("Service Error", indent);
			DisplayValue("Name", Data.name, indent + 1);
			DisplayValue("Message", Data.message, indent + 1);
			DisplayValue("Detailed Message", Data.detailedMessage, indent + 1);
			DisplayValue("Code", Data.code, indent + 1);
			if (Data.faultCodeSpecified)
			{
				DisplayValue("Fault Code", Data.faultCode, indent + 1);
			}
		}

		public void DisplayAt(int indent)
		{
			Display(indent);
		}

        public string GetStringAt(int indent)
        {
            return getServiceErrorString(indent);
        }

        protected string getServiceErrorString(int indent)
        {
            string res = "";
            res += getStringLabel("Service Error", indent);
            res += getStringValue("Name", Data.name, indent + 1);
            res+= getStringValue("Message", Data.message, indent + 1);
            res+= getStringValue("Detailed Message", Data.detailedMessage, indent + 1);
            res+= getStringValue("Code", Data.code, indent + 1);
            if (Data.faultCodeSpecified)
            {
                res+= getStringValue("Fault Code", Data.faultCode, indent + 1);
            }
            return res;
        }

        protected static string getStringValue(string name, object value, int indent)
        {
            string res = "";
            if (value == null)
            {
                return res;
            }
            var prefix = new string(' ', indent * 2);
            res = string.Format("{0}: {1}\n", prefix + name, value);
            return res;
        }


        protected static string getStringLabel(string name, int indent)
        {
            string res = "";
            if (name == null)
            {
                return res;
            }
            var prefix = new string(' ', indent * 2);
            res = string.Format("{0}\n", prefix + name);
            return res;
        }
	}
}