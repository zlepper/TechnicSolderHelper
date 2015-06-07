using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Utils;
using NUnit.Framework;

namespace ModpackHelper.Tests
{
    [TestFixture]
    public class ListsTest
    {
        [Test]
        public void Lists_AreEqual_EqualListsButReferenceIsDifferent()
        {
            List<string> list1 = new List<string>()
            {
                "test",
                "geg",
                "zlepper",
                "bochen",
                "notch"
            };
            List<string> list2 = new List<string>()
            {
                "test",
                "geg",
                "zlepper",
                "bochen",
                "notch"
            };

            bool eq = Lists.AreEqual(list1, list2);

            Assert.IsTrue(eq);
        }

        [Test]
        public void Lists_AreEqual_DifferentContentInListsButLengthIsTheSame()
        {
            List<string> list1 = new List<string>()
            {
                "test",
                "geg",
                "zlepper",
                "bochen",
                "notch"
            };
            List<string> list2 = new List<string>()
            {
                "test",
                "geg",
                "notch",
                "CanVox",
                "Juice"
            };

            bool eq = Lists.AreEqual(list1, list2);

            Assert.IsFalse(eq);
        }

        [Test]
        public void Lists_AreEqual_BothListsAreNull()
        {
            bool eq = Lists.AreEqual(null, null);

            Assert.IsTrue(eq);
        }

        [Test]
        public void Lists_AreEqual_List1IsNull()
        {
            List<string> list2 = new List<string>() { "test", "test2"};

            bool eq = Lists.AreEqual(null, list2);

            Assert.IsFalse(eq);
        }

        [Test]
        public void Lists_AreEqual_List2IsNull()
        {
            List<string> list1 = new List<string>() { "test", "test2" };

            bool eq = Lists.AreEqual(list1, null);

            Assert.IsFalse(eq);
        }

        [Test]
        public void Lists_AreEqual_ReferenceIsTheSame()
        {
            List<string> list1 = new List<string>() {"Test", "test2"};
            List<string> list2 = list1;

            bool eq = Lists.AreEqual(list1, list2);

            Assert.IsTrue(eq);
        }

        [Test]
        public void Lists_AreEqual_BothListsHaveALengthOf0()
        {
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();

            bool eq = Lists.AreEqual(list1, list2);

            Assert.IsTrue(eq);
        }

        [Test]
        public void Lists_AreEqual_ListsHaveDifferentLength()
        {
            List<string> list1 = new List<string>()
            {
                "test1",
                "test2",
                "test3"
            };
            List<string> list2 = new List<string>()
            {
                "test1"
            };

            bool eq = Lists.AreEqual(list1, list2);

            Assert.IsFalse(eq);
        }

       
    }
}
