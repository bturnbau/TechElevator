using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using ParkReservation;

namespace Capstone
{
    public class CLIHelper
    {
 
        private List<int> ValidParkIDs = new List<int> { 1, 2, 3 };
        public int ParkInputCheck(string userInput)
        {
            int input = 0;
            if (int.TryParse(userInput, out input))
            {
                if (!ValidParkIDs.Contains(input))
                {
                    input = 0;
                }
            }
            return input;
        }
        public Reservation ReserveSite(IList<Site> AvailableSites, DateTime arrivalDate, DateTime departDate, int siteNum, string resName)
        {
            Reservation reservation = null;
            foreach (Site site in AvailableSites)
            {
                if (site.site_number == siteNum)
                {
                    reservation = new Reservation(site.site_id, resName, arrivalDate, departDate);
                }

            }
            return reservation;
        }
        public bool CheckParkOpenMonths(Campground campground, DateTime arrivalDate, DateTime departDate)
        {
            bool isOpen = true;
            int arriveMonth = arrivalDate.Month;
            int departMonth = departDate.Month;
            if (arriveMonth < campground.open_from_mm || departMonth > campground.open_to_mm)
            {
                isOpen = false;
            }
            return isOpen;
        }
        public string CheckArrivalDates(DateTime Today, DateTime arrivalDate)
        {
            string errorMsg = "";
            if (arrivalDate < DateTime.Today)
            {
                errorMsg = "     This date in the past. You will need to enter a future date.";
            }
            
            return errorMsg;
        }
        public string CheckDepartDates(DateTime Today, DateTime arrivalDate, DateTime departDate)
        {
            string errorMsg = "";
             if (departDate < DateTime.Today || departDate < arrivalDate)
            {
                errorMsg = "    This date in the past or prior to your arrival.You will need to enter a future date.";
            }
            return errorMsg;
        }
        public IList<Site> DoCampSiteSearch(ICampgroundDAO campgroundDAO, int campgroundID_input, DateTime arrivalDate, DateTime departDate)
        {
            IList<Reservation> allReservationsinCampground = campgroundDAO.GetAllReseravations(campgroundID_input, arrivalDate, departDate);

            IList<Site> allSitesinCampground = campgroundDAO.GetAllSites(campgroundID_input);

            IList<Site> AvailableSites = campgroundDAO.AvailbleSites(allReservationsinCampground, allSitesinCampground);

            return AvailableSites;
        }

    }
}
