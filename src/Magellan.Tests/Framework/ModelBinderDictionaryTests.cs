using System;
using System.ComponentModel;
using System.Data;
using Magellan.Framework;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ModelBinderDictionaryTests
    {
        protected IModelBinder FakeBinder = new Mock<IModelBinder>().Object;

        [Test]
        public void ShouldAllowModelBindersToBeRegistered()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);
        }

        [Test]
        public void MultipleBindersCannotBeRegisteredForSameType()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);
            Assert.Throws<ArgumentException>(() => dictionary.Add(typeof(DataSet), FakeBinder));
        }

        [Test]
        public void RemoveCanBeUsedToUnregister()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);
            dictionary.Remove(FakeBinder);
            dictionary.Add(typeof(DataSet), FakeBinder);
            dictionary.Remove(FakeBinder);
            dictionary.Add(typeof(DataSet), FakeBinder);
        }

        [Test]
        public void ContainsCanBeUsedToCheckForPriorRegistration()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            Assert.IsFalse(dictionary.Contains(FakeBinder));

            dictionary.Add(typeof(DataSet), FakeBinder);
            Assert.IsTrue(dictionary.Contains(FakeBinder));

            dictionary.Remove(FakeBinder);
            Assert.IsFalse(dictionary.Contains(FakeBinder));
        }

        [Test]
        public void ClearCanBeUsedToRemoveAllRegistrations()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);
            Assert.IsTrue(dictionary.Contains(FakeBinder));
            dictionary.Clear();
            Assert.IsFalse(dictionary.Contains(FakeBinder));
        }

        [Test]
        public void ShouldResolveBinder()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);

            var binder = dictionary.GetBinder(typeof(DataSet));

            Assert.AreSame(FakeBinder, binder);
        }

        [Test]
        public void ShouldResolveBindersViaInheritance()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(IListSource), FakeBinder);

            var binder = dictionary.GetBinder(typeof(DataSet)); // DataSet implements IListSource

            Assert.AreSame(FakeBinder, binder);
        }

        [Test]
        public void ShouldFallBackToDefaultBinder()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.Add(typeof(DataSet), FakeBinder);

            var binder = dictionary.GetBinder(typeof(int));

            Assert.IsInstanceOf<DefaultModelBinder>(binder);
        }

        [Test]
        public void ShouldRespectExplicitDefaultBinder()
        {
            var dictionary = new ModelBinderDictionary(new DefaultModelBinder());
            dictionary.DefaultModelBinder = FakeBinder;

            var binder = dictionary.GetBinder(typeof(int));

            Assert.AreSame(FakeBinder, binder);
        }
    }
}
