using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace AdRotator.UnitTests
{
    [TestClass]
    public class ReflectionHelpersTests
    {
        [TestMethod]
        public void GetAssemblyFromClassNameTest()
        {
            Assembly assemblyA = ReflectionHelpers.GetAssemblyFromClassName("TestData.ClassA");
            Assert.IsNotNull(assemblyA);
            Assert.IsTrue(assemblyA.FullName.StartsWith("TestData,"));

            Assembly assemblyB = ReflectionHelpers.GetAssemblyFromClassName("TestData.Nested.ClassB");
            Assert.IsNotNull(assemblyB);
            Assert.IsTrue(assemblyB.FullName.StartsWith("TestData,"));

            Assembly assemblyC = ReflectionHelpers.GetAssemblyFromClassName("TestData.Nested.ClassC");
            Assert.IsNotNull(assemblyC);
            Assert.IsTrue(assemblyC.FullName.StartsWith("TestData.Nested,"));

            Assembly assemblyD = ReflectionHelpers.GetAssemblyFromClassName("TestData.ClassD");
            Assert.IsNull(assemblyD);
        }
    }
}
