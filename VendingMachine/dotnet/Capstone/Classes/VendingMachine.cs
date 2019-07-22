using Capstone.Classes.Exceptions;
using Capstone.Classes.ProductClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        // Maps the products location to the product.
        public Dictionary<string, Product> Inventory { get; }
        public double Balance { get; private set; } = 0;


        public VendingMachine(string file)
        {
            Inventory = new Dictionary<string, Product> { };

            // Sets up File to read in to the inventory from a txt file
            string directory = Environment.CurrentDirectory;
            string inventoryFile = file;
            string inventoryFullPath = Path.Combine(directory, inventoryFile);

            using (StreamReader sr = new StreamReader(inventoryFullPath))
            {
                while (!sr.EndOfStream)
                {
                    Product prod = null;
                    string line = sr.ReadLine();
                    string[] breaks = line.Split("|");
                    string slot = breaks[0];
                    string productName = breaks[1];
                    string productPrice = breaks[2];
                    string productType = breaks[3];
                    // Check to see if we get a vaild price of a product
                    if (!double.TryParse(productPrice, out double result))
                    {
                        //Log.AddTransactiontoLog($"{slot}: Bad Inventory Input: Price needs to be decimal", 0, this.Balance);
                        continue;
                    }
                    // Make sure we did not already have in item in that location
                    if (Inventory.ContainsKey(slot))
                    {
                        //Log.AddTransactiontoLog($"{slot}: Bad Inventory Input: This Slot is already filled with another product!", 0, this.Balance);
                        continue;
                    }
                    // Checks type and add the correct type to the inventory
                    if (productType.Contains("Chip"))
                    {
                        prod = new Chip(productName, double.Parse(productPrice));
                    }
                    else if (productType.Contains("Drink"))
                    {
                        prod = new Drink(productName, double.Parse(productPrice));
                    }
                    else if (productType.Contains("Gum"))
                    {
                        prod = new Gum(productName, double.Parse(productPrice));
                    }
                    else if (productType.Contains("Candy"))
                    {
                        prod = new Candy(productName, double.Parse(productPrice));
                    }
                    else 
                    {
                        // We did not get a valid input so we report and move on
                        //Log.AddTransactiontoLog($"{slot}:Bad Inventory Input: No Product of this type exists.", 0, this.Balance);
                        continue;
                    }
                    Inventory.Add(slot, prod);
                    
                }
                Report.SetupReportWithInventory(Inventory);
            }
        }
        /// <summary>
        /// Adds a number greater than zero to the Balance
        /// </summary>
        /// <param name="fedMoney">Money to be added to the vending machine</param>
        public void AddMoneyToBalance(double fedMoney)
        {
            if (fedMoney < 0)
            {    
                return;
            }
            Balance += fedMoney;
            Log.AddTransactiontoLog("Added Money", fedMoney, this.Balance);
        }

        /// <summary>
        /// Checks to see if you can Purchase an item and does the transaction.
        /// </summary>
        /// <param name="slot">location of the item to be purchased</param>
        public void PurchaseItem(string slot)
        {
            
            if (Inventory.ContainsKey(slot))
            {
                if (Balance < Inventory[slot].Price)
                {
                    throw new NotEnoughMoneyException();
                }
                else
                {
                    Inventory[slot].PurchaseOneItem();
                    double oldBalance = Balance;
                    Balance -= Inventory[slot].Price;
                    Log.AddTransactiontoLog($"{Inventory[slot].Name} {slot}", oldBalance, Balance);
                    Report.ReportSale(Inventory[slot], Inventory);

                }
            }
            else
            {
                throw new InvalidSelectionException();
            }

       
        }

        /// <summary>
        /// Convets the balance to change and returns the change.  also sets the balance to zero
        /// </summary>
        /// <returns>A change opject that corresponds to the balance</returns>
        public Change CashOut()
        {
            double oldBalance = this.Balance;
            Change change = new Change(Balance); 
            Balance = 0;
            Log.AddTransactiontoLog("Cash Out", oldBalance, Balance);
            return change;
        }
        

    }
}
