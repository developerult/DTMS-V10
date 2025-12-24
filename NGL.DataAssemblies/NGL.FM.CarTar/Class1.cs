using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.CarTar
{
    public class Class1
    {
        public Class1(int intProp, string prop)
        {
            this.MyIntProperty = intProp;
            this.MyStringProperty = prop;
        }

        
        private bool _somethingIsCompleted = false;
        public bool somethingIsCompleted
        {
        get
        {
            return _somethingIsCompleted;
        }         
        }

        public int MyIntProperty { get; set; }

        public string MyStringProperty { get; set; }

        public void DoSomething()
        {
            this._somethingIsCompleted = true;
        }

    }
}
