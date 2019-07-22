using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes.ProductClasses
{
    public class Chip : Product
    {
        public Chip(string name, double price):base(name, price)
        {
            Type = "Chip";
        }

        public override string OutputMessageOnPurchase()
        {
            return "Crunch Crunch, Yum!";
        }
    }
}
