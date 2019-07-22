using Capstone.Classes;
using Capstone.Classes.ProductClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CapstoneTests
{
    [TestClass]
    public class VenidingMachineTest
    {
        VendingMachine vm;

        [TestInitialize]
        public void initialize()
        {
           vm = new VendingMachine("VendingMachine.txt");

        }

        [TestMethod]
        public void TestNegativeAddMoney()
        {
             double currentBalance = vm.Balance;
             vm.AddMoneyToBalance(-100);
            Assert.AreEqual(currentBalance, vm.Balance, 0.00001);
        }

        [TestMethod]
        public void TestBalance()
        {
            vm.AddMoneyToBalance(99.99);
            Assert.AreEqual(99.99, vm.Balance);
            vm.AddMoneyToBalance(.01);
            Assert.AreEqual(100, vm.Balance);

            Change change = vm.CashOut();
            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(400, change.Quarters);
        }

        [TestMethod]
        public void TestCashout()
        {

            vm.AddMoneyToBalance(0.01);
            Change change = vm.CashOut();

            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.02);
            change = vm.CashOut();

            Assert.AreEqual(2, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.05);
            change = vm.CashOut();

            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.10);
            change = vm.CashOut();

            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.11);
            change = vm.CashOut();

            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.16);
            change = vm.CashOut();

            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.19);
            change = vm.CashOut();

            Assert.AreEqual(4, change.Pennies);
            Assert.AreEqual(1, change.Nickels);
            Assert.AreEqual(1, change.Dimes);
            Assert.AreEqual(0, change.Quarters);

            vm.AddMoneyToBalance(0.25);
            change = vm.CashOut();

            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(1, change.Quarters);

            vm.AddMoneyToBalance(1.00);
            change = vm.CashOut();

            Assert.AreEqual(0, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(4, change.Quarters);

            vm.AddMoneyToBalance(1.01);
            change = vm.CashOut();

            Assert.AreEqual(1, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(0, change.Dimes);
            Assert.AreEqual(4, change.Quarters);

            vm.AddMoneyToBalance(1.99);
            change = vm.CashOut();

            Assert.AreEqual(4, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(2, change.Dimes);
            Assert.AreEqual(7, change.Quarters);

            vm.AddMoneyToBalance(100.73);
            change = vm.CashOut();

            Assert.AreEqual(3, change.Pennies);
            Assert.AreEqual(0, change.Nickels);
            Assert.AreEqual(2, change.Dimes);
            Assert.AreEqual(402, change.Quarters);
        }

        [TestMethod]
        public void TestPurchase()
        { 
            vm.AddMoneyToBalance(100);
            Assert.IsTrue(vm.PurchaseItem("A1"));
            Assert.AreEqual(96.95, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("A1"));
            Assert.AreEqual(93.9, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("A1"));
            Assert.AreEqual(90.85, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("A1"));
            Assert.AreEqual(87.8, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("A1"));
            Assert.AreEqual(84.75, vm.Balance, 0.00001);
            Assert.IsFalse(vm.PurchaseItem("A1"));
            Assert.AreEqual(84.75, vm.Balance, 0.00001);
            Assert.IsFalse(vm.PurchaseItem("A1"));
            Assert.AreEqual(84.75, vm.Balance, 0.00001);

            Assert.IsFalse(vm.PurchaseItem("kjdf"));
            Assert.AreEqual(84.75, vm.Balance, 0.00001);
            Assert.IsFalse(vm.PurchaseItem("fsdkjl"));
            Assert.AreEqual(84.75, vm.Balance, 0.00001);

            Assert.IsTrue(vm.PurchaseItem("B3"));
            Assert.AreEqual(83.25, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("B3"));
            Assert.AreEqual(81.75, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("B3"));
            Assert.AreEqual(80.25, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("B3"));
            Assert.AreEqual(78.75, vm.Balance, 0.00001);
            Assert.IsTrue(vm.PurchaseItem("B3"));
            Assert.AreEqual(77.25, vm.Balance, 0.00001);
            Assert.IsFalse(vm.PurchaseItem("B3"));
            Assert.AreEqual(77.25, vm.Balance, 0.00001);
        }
    }
}
