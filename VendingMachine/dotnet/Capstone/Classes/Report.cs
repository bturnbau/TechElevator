using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    class Report
    {
        /// <summary>
        /// Adds all Items to in the current inventroy to the report file or creates 
        /// a new report file
        /// </summary>
        /// <param name="inventory">current inventory of the vending machine</param>
        public static void SetupReportWithInventory(Dictionary<string, Product> inventory)
        {
            double totalSales = -1;
            //pulls out all the data form the old report
            Dictionary<string, int> pastSales = PulloutSalesReport(ref totalSales);

            // updates the sales totals
            foreach (Product product in inventory.Values)
            {
                if (!pastSales.ContainsKey(product.Name))
                {
                    pastSales[product.Name] = 0;
                }
            }
            //writes the new report
            Report.WriteSalesReport(pastSales, totalSales);

        }

        /// <summary>
        /// reports the sale of one item
        /// </summary>
        /// <param name="product"></param>
        /// <param name="inventory"></param>
        public static void ReportSale(Product product, Dictionary<string, Product> inventory)
        {

            double totalSales = -1;
            // gets the past sales
            Dictionary<string, int> pastSales = PulloutSalesReport(ref totalSales);

            if (pastSales.ContainsKey(product.Name))
            {
                pastSales[product.Name] += 1;
            }
            // if something happened and the report had no sales data totalSalse will be set to 0
            if(totalSales < 0)
            {
                totalSales = 0;
                //Addes each item with its current price
                foreach(var inventoryItem in inventory)
                {
                    if (pastSales.ContainsKey(inventoryItem.Value.Name))
                    {
                        totalSales +=  inventoryItem.Value.Price * pastSales[inventoryItem.Value.Name];
                    }
                }
            }
            totalSales += product.Price;
            // writes anew report with updated values
            Report.WriteSalesReport(pastSales, totalSales);
        }

        /// <summary>
        /// Reads the current report file and pulls out the sales data.
        /// </summary>
        /// <param name="totalSales">used to return a total sales if found</param>
        /// <returns>A dictionary that maps items names to number sold</returns>
        private static Dictionary<string, int> PulloutSalesReport(ref double totalSales)
        {
            Dictionary<string, int> sales = new Dictionary<string, int>();
            totalSales = -1;
            if (File.Exists("SalesReport.txt"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader("SalesReport.txt"))
                    {
                        while (!sr.EndOfStream)
                        {
                            // parses the file to get a item sold or total sales
                            string line = sr.ReadLine();
                            string[] lineSplit = line.Split("|");
                            if (lineSplit.Length == 2)
                            {
                                int numOfSales = 0;
                                if (int.TryParse(lineSplit[1], out numOfSales))
                                {
                                    sales[lineSplit[0]] = numOfSales;
                                }
                            }
                            else
                            {
                                if (line.Length > 1 && line[0] == '*')
                                {
                                    lineSplit = line.Split("$");
                                    if (double.TryParse(lineSplit[1], out totalSales))
                                    {

                                    }
                                    else
                                    {
                                        totalSales = -1;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                    Console.ReadLine();
                }
            }

            return sales;
        }

        /// <summary>
        /// Write a new file that contains a sales report
        /// </summary>
        /// <param name="sales">Dictionary that maps a item name to sales</param>
        /// <param name="salesTotal">Total sales to be reported</param>
        private static void WriteSalesReport(Dictionary<string, int> sales, double salesTotal)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("SalesReport.txt", false))
                {
                    foreach(var sale in sales)
                    {
                        sw.WriteLine($"{sale.Key}|{sale.Value}");
                    }
                    sw.WriteLine();
                    sw.WriteLine();
                    if (salesTotal < 0)
                    {
                        sw.WriteLine($"***TOTAL SALES * * ${0:0.00}");
                    }
                    else
                    {
                        sw.WriteLine($"***TOTAL SALES * * ${salesTotal:0.00}");
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
