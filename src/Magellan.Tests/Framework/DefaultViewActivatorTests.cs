using System;
using System.Data;
using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class DefaultViewActivatorTests
    {
        [Test]
        public void UsesActivatorClassToCreateObjects()
        {
            var activator = new DefaultViewActivator();
            var instance = activator.Instantiate(typeof(DataSet));
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<DataSet>(instance);
        }

        [Test]
        public void DoesNotCacheInstances()
        {
            var activator = new DefaultViewActivator();
            var instance1 = activator.Instantiate(typeof(DataSet));
            var instance2 = activator.Instantiate(typeof(DataSet));
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void ViewTypeIsRequired()
        {
            var activator = new DefaultViewActivator();
            Assert.Throws<ArgumentNullException>(() => activator.Instantiate(null));
        }

        [Test]
        public void OnlySupportsTypesWithPublicParameterlessConstructor()
        {
            var activator = new DefaultViewActivator();
            Assert.Throws<NotSupportedException>(() => activator.Instantiate(typeof(X)));
        }

        private class X
        {
            private X()
            {
                
            }
        }
    }
}
