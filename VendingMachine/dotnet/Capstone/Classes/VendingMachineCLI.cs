using Capstone.Classes.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    class VendingMachineCLI
    {
        private VendingMachine _vm;

        public VendingMachineCLI(VendingMachine vm)
        {
            _vm = vm;
        }

        public void Run()
        {
            while (true)
            {
                // Prints the main meanu
                Console.Clear();
                PrintVendingMachine();
                Console.WriteLine($"Your Current Balance is: ${_vm.Balance:0.00}");
                Console.WriteLine("Choose Option.");
                Console.WriteLine("1. Purchase Item");
                Console.WriteLine("2. Add Money.");
                Console.WriteLine("3. Cash Out");
                Console.WriteLine("4. Cash Out and Quit");
                
                int option = -1;
                string input = Console.ReadLine();
                int.TryParse(input, out option);
                // makes sure we get vaild input for one of the options.
                while (option > 4 || option <= 0)
                {

                    Console.WriteLine("Please enter 1-4 ");
                    input = Console.ReadLine();
                    int.TryParse(input, out option);
                }
                if (option == 1)
                {
                    if (_vm.Balance == 0.0)
                    {
                        Console.WriteLine("Add money first!");
                        Console.ReadLine();
                        continue;
                    }
                    Purchase();
                }
                else if(option == 2)
                {
                    AddMoney();
                }
                else if(option == 3)
                {
                    CashOut();
                }
                else if(option == 4)
                {
                    CashOut();
                    // exits main loop
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter 1-4 ");
                    Console.ReadLine();

                }
            }
        }

        /// <summary>
        /// Takes user input to add money to the vending machine
        /// </summary>
        public void AddMoney()
        {
            Console.Write("What do you want to add? In dollars ");
            double amount = -2;
           // string strAmnt = Console.ReadLine();
            while (true)
            {
                string strAmnt = Console.ReadLine();
                if (strAmnt.Contains('$'))
                {
                    strAmnt = strAmnt.Replace('$', '0');
                }
                if (!double.TryParse(strAmnt, out amount))
                {
                    Console.WriteLine("Please enter dollar amount");
                    continue;
                }
                if (amount < 0)
                {
                    Console.WriteLine(" Did you ever hear the tragedy of Darth Plagueis The Wise? I thought not. It’s not a story the Jedi would tell you. It’s a Sith legend. Darth Plagueis was a Dark Lord of the Sith, so powerful and so wise he could use the Force to influence the midichlorians to create life… He had such a knowledge of the dark side that he could even keep the ones he cared about from dying. The dark side of the Force is a pathway to many abilities some consider to be unnatural. He became so powerful… the only thing he was afraid of was losing his power, which eventually, of course, he did. Unfortunately, he taught his apprentice everything he knew, then his apprentice killed him in his sleep. Ironic. He could save others from death, but not himself.");
                    Console.WriteLine("Please enter dollar amount");
                }
                else
                {
                    _vm.AddMoneyToBalance(amount);
                    break;
                }
            }
        }

        /// <summary>
        /// Calls CahsOut and displays the change as a string
        /// </summary>
        public void CashOut()
        {
            Change change = _vm.CashOut();
            Console.WriteLine($"The Change was {change.Quarters} Quarters, {change.Dimes} Dimes, {change.Nickels} Nickels, and {change.Pennies} Pennies.");
            Console.ReadLine();
        }

        /// <summary>
        /// Takes input as a purchase key then passes it to the Vending Machine
        /// </summary>
        public void Purchase()
        {
            Console.Write("What slot do you want or Q to quit?  ");
            
            while (true)
            {
                string slot = Console.ReadLine();
                if (slot == "Q")
                {
                    break;
                }
                try
                {
                    _vm.PurchaseItem(slot);
                    Console.WriteLine(_vm.Inventory[slot].OutputMessageOnPurchase());
                }
                catch (InvalidSelectionException)
                {
                    Console.WriteLine("Invalid Selection purchase failed.");
                    Console.WriteLine("Please try again.");
                }
                catch (NotEnoughMoneyException)
                {
                    Console.WriteLine($"Please add money to purchase {_vm.Inventory[slot].Name}.");

                }
                catch (OutOfSockException)
                {
                    Console.WriteLine($"Sorry {_vm.Inventory[slot].Name} is Out of Stock ");
                }
                
                Console.WriteLine("Please select agian or Q to quit.");

            }
        }

        /// <summary>
        /// prints the products to the console.
        /// </summary>
        public void PrintVendingMachine()
        {
            Console.WriteLine("WELCOME TO SNACK CITY\n");
            Console.WriteLine("#  | Item                 | Price | Remaining");
            Console.WriteLine("---------------------------------------------");
            Dictionary<string, Product> inventory = _vm.Inventory;
            foreach (KeyValuePair<string, Product> kvp in inventory)
            {             
                if(kvp.Value.Amount > 0)
                {
                    Console.WriteLine(String.Format("{0,-1} | {1,-20} | {2,-5:0.00} | {3, 5} ", kvp.Key, kvp.Value.Name, kvp.Value.Price, kvp.Value.Amount));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-1} | {1,-20} | {2,-5:0.00} | {3, 5} ", kvp.Key, kvp.Value.Name, kvp.Value.Price, "Sold Out"));
                }
            }
            Console.WriteLine();
            
        }
    }
}
