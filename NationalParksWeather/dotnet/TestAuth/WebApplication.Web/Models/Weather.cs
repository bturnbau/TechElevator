using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Weather
    {
        public string parkCode { get; set; }
        public string fiveDayForecastValue { get; set; }
        public int low { get; set; }
        public int high { get; set; }
        public string forecast { get; set; }
        public bool isFarenheit { get; set; }

        public int CalculateTemp(bool isFarenheit, bool isHigh)
        {
            int result = isHigh? high: low;
            if (!isFarenheit)
            {
               result = Convert.ToInt32((result - 32) * (5.00 / 9.00));
            }
            return result;
            
        }
       
    }
}
