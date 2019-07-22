using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO : IReservationDAO
    {
        private const string _getLastIdSQL = "select cast(SCOPE_IDENTITY() as int);";
        private string connectionString;
        public ReservationDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int InsertNewBooking(Reservation newReservation)
        {


            const string sql = "insert into reservation (site_id, name, from_date, to_date) " +
                                "values (@siteID, @name, @arrive, @depart)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@siteID", newReservation.site_id);
                cmd.Parameters.AddWithValue("@name", newReservation.reservation_name);
                cmd.Parameters.AddWithValue("@arrive", newReservation.from_date);
                cmd.Parameters.AddWithValue("@depart", newReservation.to_date);

                newReservation.reservation_id = (int)cmd.ExecuteScalar();

            }
            return newReservation.reservation_id;

        }

        public Reservation FindReservation(int reservationID)
        {
            Reservation reservation = null;
            const string sql = "SELECT * From reservation WHERE reservation_id = @reservationID;";
           
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@reservationID", reservationID);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservation = GetReservationFromReader(reader);
                }
            }
              
            if (reservation == null)
            {
                throw new Exception("Reservation does not exist.");
            }

            return reservation;
        }

        public Reservation GetReservationFromReader(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.reservation_id = Convert.ToInt32(reader["reservation_id"]);
            reservation.reservation_name = Convert.ToString(reader["name"]);
            reservation.site_id = Convert.ToInt32(reader["site_id"]);
            reservation.from_date = Convert.ToDateTime(reader["from_date"]);
            reservation.to_date = Convert.ToDateTime(reader["to_date"]);
            reservation.booking_created = Convert.ToDateTime(reader["create_date"]);

            return reservation;
        }

        public IList<Reservation> shownext30()
        {
            List<Reservation> reservations = new List<Reservation>();

            const string sql = "select * from reservation "+
                                "where(from_date) between GETDATE() and(GETDATE() + 30) order by from_date; ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservations.Add(GetReservationFromReader(reader));
                }
            }

            if (reservations.Count == 0)
            {
                throw new Exception("There are no Reservations in next 30 days");
            }

            return reservations;

        }
    }
}

 