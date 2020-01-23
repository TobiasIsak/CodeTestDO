using System;
using System.Collections.ObjectModel;
using DealerOnTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealerOnUnitTest
{
    [TestClass]
    public class DealerOnTests
    {
        [TestMethod]
        public void ExtensionMethods_ObservableCollectionCleanerEmptiesObservableCollection()
        {
            var testList = new ObservableCollection<SalesItem>
                {
                    new SalesItem("Book", 12.49M, 1, false, false),
                new SalesItem("Music CD", 14.99M, 1, false, true),
                new SalesItem("Packet of headache pills", 9.75M, 1, false, false),
                new SalesItem("Chocolate bar", 0.85M, 1, false, false),
                new SalesItem("Imported box of chocolates", 10.00M, 1, true, false),
                new SalesItem("Imported box of chocolates", 11.25M, 1, true, false),
                new SalesItem("Bottle of perfume", 18.99M, 1, false, true),
                new SalesItem("Imported bottle of perfume", 27.99M, 1, true, true),
                new SalesItem("Imported bottle of perfume", 47.50M, 1, true, true)
                };

            var emptyList = new ObservableCollection<SalesItem>();

            testList.Clean(x => x != null);
            Assert.AreEqual(emptyList.Count, testList.Count);
        }

        [TestMethod]
        public void ExtensionMethods_ObservableCollectionCleanerRemovesItemsBasedOnName()
        {
            var objectToDelete1 = new SalesItem("Imported bottle of perfume", 27.99M, 1, true, true);
            var objectToDelete2 = new SalesItem("Imported bottle of perfume", 47.50M, 1, true, true);

            var testList = new ObservableCollection<SalesItem>
                {
                    new SalesItem("Book", 12.49M, 1, false, false),
                new SalesItem("Music CD", 14.99M, 1, false, true),
                new SalesItem("Packet of headache pills", 9.75M, 1, false, false),
                new SalesItem("Chocolate bar", 0.85M, 1, false, false),
                new SalesItem("Imported box of chocolates", 10.00M, 1, true, false),
                new SalesItem("Imported box of chocolates", 11.25M, 1, true, false),
                new SalesItem("Bottle of perfume", 18.99M, 1, false, true),
                objectToDelete1,
                objectToDelete2
                };

            var emptyList = new ObservableCollection<SalesItem>();

            testList.Clean(x => x.Name == "Imported bottle of perfume");
            Assert.IsFalse(testList.Contains(objectToDelete1) || testList.Contains(objectToDelete2));
            Assert.IsTrue(testList.Count == 7);
        }

        [TestMethod]
        public void SalesItem_SalesItemRoundsValueToTwoDecimals()
        {
            var testItem = new SalesItem("test item", 13.372020m, 1, false, false);

            var decimalCount = BitConverter.GetBytes(decimal.GetBits(testItem.Price)[3])[2];

            Assert.IsTrue(decimalCount == 2);
        }
    }
}
