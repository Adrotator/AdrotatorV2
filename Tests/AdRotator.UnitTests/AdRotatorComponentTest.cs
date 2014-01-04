using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AdRotator.UnitTests
{
    [TestClass]
    public class AdRotatorComponentTest
    {
        [TestMethod]
        public void DefaultConstructorTest()
        {
            var fileHelpers = new Mock<AdRotator.Utilities.IFileHelpers>();
            var component = new AdRotatorComponent("test-test", fileHelpers.Object);
            Assert.AreEqual(component.AdWidth, 480);
            Assert.AreEqual(component.AdHeight, 80);
            Assert.IsTrue(component.IsAdRotatorEnabled);
            Assert.AreEqual(AdRotatorComponent.PlatformSupportedAdProviders.Count, 0);
            Assert.AreEqual(AdRotatorComponent.PlatformAdProviderComponents.Count, 0);
        }
        
    }
}
