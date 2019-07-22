using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        IList<Campground> ViewAllCampgrounds(int key);

        Campground FindCampground(int key);

        IList<Reservation> GetAllReseravations(int campground_id, DateTime arrival, DateTime depart);

        IList<Site> GetAllSites(int campground_id);

        IList<Site> AvailbleSites(IList<Reservation> allReservations, IList<Site> allSites);
    }
}
