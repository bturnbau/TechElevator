using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{   
    /// <summary>
    /// A class which is used to Make Change from the Balance remaining in the machine. 
    /// Change is reported with the least number of coins possible (greedy algorithm).
    /// </summary>
    public class Change
    {
        public int Quarters { get; }
        public int Dimes { get; }
        public int Nickels { get; }
        public int Pennies { get; }

        //A method which takes current machine Balance and determines the least number of coins possible to make change.
        //Sets Change properties of (Quarter, Dimes, Nickel, Pennies) to number of each coin needed. 
        /// <summary>
        /// A method which takes current machine Balance and determines the least number of coins possible to make change.
        /// Sets Change properties of (Quarter, Dimes, Nickel, Pennies) to number of each coin needed. 
        /// </summary>
        /// <param name="balance">Number to be made into change</param>
        public Change(double balance)
        {       
            int balanceInCents = (int) (balance * 100);
            Quarters = balanceInCents / 25;
            balanceInCents = balanceInCents % 25;
            Dimes = balanceInCents / 10;
            balanceInCents = balanceInCents % 10;
            Nickels = balanceInCents / 5;
            balanceInCents = balanceInCents % 5;
            Pennies = balanceInCents;
            
        }
    }
}
