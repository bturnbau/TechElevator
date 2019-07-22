using System;
using System.Collections.Generic;
using System.Text;

namespace ParkReservation
{
    public interface IParkDAO
    {
        IList<Park> ViewAllParks();

        Park ShowParkDetails(int Key);
    }
}
