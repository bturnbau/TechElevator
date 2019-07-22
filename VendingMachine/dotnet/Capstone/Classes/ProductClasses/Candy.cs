using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes.ProductClasses
{
    public class Candy : Product
    {
        public Candy(string name, double price):base(name, price)
        {
            Type = "Candy";
        }

        public override string OutputMessageOnPurchase()
        {
            return "Munch Munch, Yum!";
        }
    }
}
