using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class MenuTree
    {
        public int MenuTreeControl { get; set; } //mtc
        public string Caption { get; set; } //text
        public int LinkPageControl { get; set; } //id
        public string LinkTo { get; set; } //LinksTo
        public Boolean Expanded { get; set; } //expanded
    }
}