using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes.ProductClasses
{
    public class Drink : Product
    {
        public Drink(string name, double price): base(name, price)
        {
            Type = "Drink";
        }

        public override string OutputMessageOnPurchase()
        {
            return "Glug Glug, Yum!";
        }
    }
}
