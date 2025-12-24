using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.ContentManagement
{
    public class cmHTMLBuilder
    {
        public cmHTMLBuilder()
        {
            
        }

        public cmHTMLBuilder(string tag, string id, string cssClass, string style, string inner)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<{0}", tag);
            if (!string.IsNullOrWhiteSpace(id)) { sb.AppendFormat(" id=\"{0}\"", id); }
            if (!string.IsNullOrWhiteSpace(cssClass)) { sb.AppendFormat(" class=\"{0}\"", cssClass); }
            if (!string.IsNullOrWhiteSpace(style)) { sb.AppendFormat(" style=\"{0}\"", style); }
            sb.Append(">");
            beginTag = sb.ToString();
            innerHTML = inner;
            endTag = string.Format("</{0}>", tag);
        }
        public string beginTag { get; set; }
        public string endTag { get; set; }

        public string innerHTML { get; set; }

        private List<cmHTMLBuilder> _nestedHTML;

        public List<cmHTMLBuilder> nestedHTML {

            get {
                if (_nestedHTML == null) { _nestedHTML = new List<cmHTMLBuilder>(); }
                return _nestedHTML;
            } 
            set { _nestedHTML = value; }
        }

        public void addNestedHTML(cmHTMLBuilder oHTML)
        {
            if (oHTML == null) { return; }
            nestedHTML.Add(oHTML);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(beginTag);
            sb.Append(innerHTML);
            if (nestedHTML.Count > 0)
            {
                foreach (cmHTMLBuilder h in nestedHTML)
                {
                    sb.Append(h.ToString());
                }
            }
            sb.Append(endTag);
            return sb.ToString();
        }
    }
}