using Capstone.Classes;
using Capstone.Classes.ProductClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CapstoneTests
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestProducts()
        {
            Product candy = new Candy("Bob", 50.00);
            Assert.AreEqual("Bob", candy.Name);
            Assert.AreEqual(50.00, candy.Price);
            Assert.AreEqual("Candy", candy.Type);
            Assert.AreEqual("Munch Munch, Yum!", candy.OutputMessageOnPurchase());

            Product chip = new Chip("Bob", 5.40);
            Assert.AreEqual("Bob", chip.Name);
            Assert.AreEqual(5.40, chip.Price);
            Assert.AreEqual("Chip", chip.Type);
            Assert.AreEqual("Crunch Crunch, Yum!", chip.OutputMessageOnPurchase());

            Product drink = new Drink("Bob", 10.99);
            Assert.AreEqual("Bob", drink.Name);
            Assert.AreEqual(10.99, drink.Price);
            Assert.AreEqual("Drink", drink.Type);
            Assert.AreEqual("Glug Glug, Yum!", drink.OutputMessageOnPurchase());

            Product gum = new Gum("Bob", 0.05);
            Assert.AreEqual("Bob", gum.Name);
            Assert.AreEqual(0.05, gum.Price);
            Assert.AreEqual("Gum", gum.Type);
            Assert.AreEqual("Chew Chew, Yum!", gum.OutputMessageOnPurchase());

            Assert.AreEqual(5, candy.Amount);
            Assert.IsTrue(candy.PurchaseOneItem());
            Assert.AreEqual(4, candy.Amount);
            Assert.IsTrue(candy.PurchaseOneItem());
            Assert.AreEqual(3, candy.Amount);
            Assert.IsTrue(candy.PurchaseOneItem());
            Assert.AreEqual(2, candy.Amount);
            Assert.IsTrue(candy.PurchaseOneItem());
            Assert.AreEqual(1, candy.Amount);
            Assert.IsTrue(candy.PurchaseOneItem());
            Assert.AreEqual(0, candy.Amount);
            Assert.IsFalse(candy.PurchaseOneItem());
            Assert.AreEqual(0, candy.Amount);
            Assert.IsFalse(candy.PurchaseOneItem());
            Assert.AreEqual(0, candy.Amount);

        }

    }
}
