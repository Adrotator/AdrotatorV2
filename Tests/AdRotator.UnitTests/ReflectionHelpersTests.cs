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
            ReflectionHelpers reflectionHelpers = new ReflectionHelpers();

            Assembly assemblyA = reflectionHelpers.GetAssemblyFromClassName("TestData.ClassA");
            Assert.IsNotNull(assemblyA);
            Assert.IsTrue(assemblyA.FullName.StartsWith("TestData,"));

            Assembly assemblyB = reflectionHelpers.GetAssemblyFromClassName("TestData.Nested.ClassB");
            Assert.IsNotNull(assemblyB);
            Assert.IsTrue(assemblyB.FullName.StartsWith("TestData,"));

            Assembly assemblyC = reflectionHelpers.GetAssemblyFromClassName("TestData.Nested.ClassC");
            Assert.IsNotNull(assemblyC);
            Assert.IsTrue(assemblyC.FullName.StartsWith("TestData.Nested,"));

            Assembly assemblyD = reflectionHelpers.GetAssemblyFromClassName("TestData.ClassD");
            Assert.IsNull(assemblyD);
        }
    }
}
