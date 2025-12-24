using NGL.Test.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.Backend.Tests
{
    [TestClass]
    public class InjectorTest
    {
        [TestInitialize]
        public void Setup()
        {
            Injector.Instance.Clear();
        }

        [TestMethod]
        public void Clear_1()
        {
            Assert.AreEqual(0, Injector.Instance.Count());
            //Was set, therefore use override
            Injector.Instance.SetObject("myKey1", () => MockOverrideFunction(5));

            Assert.AreEqual(1, Injector.Instance.Count());

            Injector.Instance.Clear();
            Assert.AreEqual(0, Injector.Instance.Count());

            //...other code...
            int iResult = Injector.Instance.GetObject<Int32>("myKey1", () => RealExternalHit(5));

            Assert.AreEqual(10, iResult);
        }

        [TestMethod]
        public void Basic_Found_1()
        {
            //Was set, therefore use override
            Injector.Instance.SetObject("myKey1", () => MockOverrideFunction(5));

            //...other code...
            int iResult = Injector.Instance.GetObject<Int32>("myKey1", () => RealExternalHit(5));

            Assert.AreEqual(6, iResult);
        }

        [TestMethod]
        public void Basic_NotFound_1()
        {
            //Was never set, therefore use default
            int iResult = Injector.Instance.GetObject<Int32>("myKey1", () => RealExternalHit(5));

            Assert.AreEqual(10, iResult);
        }

        private int MockOverrideFunction(int v)
        {
            //No database hit!
            return v + 1;
        }

        private int RealExternalHit(int i)
        {
            //This would be an external hit that we mock out
            return i * 2;
        }
    }
}
