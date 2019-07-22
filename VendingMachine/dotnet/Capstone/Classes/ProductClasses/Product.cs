using Capstone.Classes.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{   /// <summary>
    /// A Base abstract class for (Chip, Gum, Drink, Candy)
    /// </summary>
    public abstract class Product
    {
        const int DefaultAmount = 5;
        public string Name { get; }
        public int Amount { get; private set; }
        public double Price { get; }
        public string Type { get; protected set; }

        /// <summary>
        /// Constructor to set the Name, Price, and Amount properties of the Product. 
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <param name="price">Price of the product</param>
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
            Amount = DefaultAmount;
        }
   
        /// <summary>
        /// A method which will Return True if able to Purchase an item (at least one item remaining in Amount).
        /// Decreases item Amount by 1.
        /// </summary>
        public void PurchaseOneItem()
        {
            if(Amount > 0)
            {
                Amount--;
            }else
            {
                throw new OutOfSockException();
            }
        }

        public abstract string OutputMessageOnPurchase();
    }
}
