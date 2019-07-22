using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation 
    {
        public int reservation_id { get; set; }
        public int site_id { get; set; }
        public string reservation_name { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public DateTime booking_created { get; set; }
        public int totalDays
        {
            get
            {
                return (to_date - from_date).Days;
            }
        }
        public Reservation()
        {

        }
        public Reservation(int site_id, string name, DateTime from_date, DateTime to_date)
        {
     
            this.site_id = site_id;
            this.reservation_name = name;
            this.from_date = from_date;
            this.to_date = to_date;
            this.booking_created = DateTime.Now;

        }
    }
}
