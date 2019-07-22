using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site 
    {
        public int site_id { get; set; }
        public int campground_id { get; set; }
        public int site_number { get; set; }
        public int max_occupancy { get; set; }
        public bool accessible { get; set; }
        public int max_rv_length { get; set; }
        public bool utilities { get; set; }
        public string AccessibleMssg
        {
            get { return this.accessible? "Yes" : "No"; }
        }
        public string UtilitiesMssg
        {
            get { return this.accessible ? "Yes" : "No"; }
        }
    }
}
