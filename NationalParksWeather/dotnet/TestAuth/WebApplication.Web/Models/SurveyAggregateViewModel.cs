using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class SurveyAggregateViewModel
    {
        public string parkName { get; set; }
        public string parkCode { get; set; }
        public int count { get; set; }
        public string username { get; set; }
    }
}
