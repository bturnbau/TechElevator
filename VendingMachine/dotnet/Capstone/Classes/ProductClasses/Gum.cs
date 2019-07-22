using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes.ProductClasses
{
    public class Gum : Product
    {
        public Gum(string name, double price): base(name, price)
        {
            Type = "Gum";
        }

        public override string OutputMessageOnPurchase()
        {
            return "Chew Chew, Yum!";
        }
    }
}
