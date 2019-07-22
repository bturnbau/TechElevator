using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        int InsertNewBooking(Reservation reservation);
        Reservation FindReservation(int reservationID);
        Reservation GetReservationFromReader(SqlDataReader reader);
        IList<Reservation> shownext30();
    }
}
