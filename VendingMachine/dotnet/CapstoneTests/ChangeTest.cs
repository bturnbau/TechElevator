using Capstone.Classes;
using Capstone.Classes.ProductClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CapstoneTests
{
    [TestClass]
    public class ChangeTest
    {
       
        [TestMethod]
        public void TestChange()
        {
            Change change = new Change(0.00);
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.01);
            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change (0.02);
            Assert.AreEqual(2, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.05);
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.10);
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.11);
            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.16);
            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.19);
            Assert.AreEqual(4, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            change = new Change(0.25);
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(1, change.Quarters);

            change = new Change(1.00);
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(4, change.Quarters);

            change = new Change(1.01);
            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(4, change.Quarters);

            change = new Change(1.99);
            Assert.AreEqual(4, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(2, change.Dimes);
            Assert.AreEqual(7, change.Quarters);

            change = new Change(100.73);
            Assert.AreEqual(3, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(2, change.Dimes);
            Assert.AreEqual(402, change.Quarters);

        }

        
    }
}
