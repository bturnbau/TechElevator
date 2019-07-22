using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundDAO : ICampgroundDAO
    {
        private string connectionString;
        public CampgroundDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public Campground FindCampground(int campground_id)
        {
                Campground campground = null;
                const string sql = "SELECT * From campground WHERE campground_id = @campground_id;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campground_id", campground_id);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        campground = GetCampgroundFromReader(reader);
                    }
                }

                if (campground == null)
                {
                    throw new Exception("Campground does not exist.");
                }

                return campground;
            }
        

        public IList<Campground> ViewAllCampgrounds(int key)
        {
            List<Campground> AllCampgrounds = new List<Campground>();
            const string sql = "select * from campground where park_id = @key";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@key", SqlDbType.Int);
                cmd.Parameters["@key"].Value = key;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AllCampgrounds.Add(GetCampgroundFromReader(reader));
                }
            }

            return AllCampgrounds;
        }

        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {

            Campground campground = new Campground();

            campground.park_id = Convert.ToInt32(reader["park_id"]);
            campground.name = Convert.ToString(reader["name"]);
            campground.campground_id = Convert.ToInt32(reader["campground_id"]);
            campground.open_from_mm = Convert.ToInt32(reader["open_from_mm"]);
            campground.open_to_mm = Convert.ToInt32(reader["open_to_mm"]);
            campground.daily_fee = Convert.ToDouble(reader["daily_fee"]);
            return campground;
        }
        //want to get all current reservations given a current campground

        public IList<Reservation> GetAllReseravations(int campground_id, DateTime arrival, DateTime depart)
        {
            List<Reservation> allReservationsforCampground = new List<Reservation>();

            //want this to return a list of all sites in campground that have a conlfict
            const string sql = "select * from reservation " +
                               "join site on site.site_id = reservation.site_id " +
                                "join campground on campground.campground_id = site.campground_id " +
                                "where(CAST(@arrival AS date) BETWEEN from_date and to_date or " +
                                "CAST(@depart AS date) BETWEEN from_date and to_date or " +
                                "CAST(@arrival AS date) < from_date and cast(@depart AS date) > to_date) and " +
                                "campground.campground_id = @campground_id;";

            //get a list of all sites in the campground


            //find lists which dont exist in conflicts list

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@campground_id", SqlDbType.Int);
                cmd.Parameters.Add("@arrival", SqlDbType.DateTime);
                cmd.Parameters.Add("@depart", SqlDbType.DateTime);
                cmd.Parameters["@campground_id"].Value = campground_id;
                cmd.Parameters["@arrival"].Value = arrival;
                cmd.Parameters["@depart"].Value = depart;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allReservationsforCampground.Add(GetReservationfromCampground(reader));

                }
            }

            return allReservationsforCampground;
        }

        public IList<Site> GetAllSites(int campground_id)
        {
            List<Site> allSitesinCampground = new List<Site>();

            const string sql = "select * from site " +
                                "join campground on campground.campground_id = site.campground_id where campground.campground_id = @campground_id;";
            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@campground_id", SqlDbType.Int);
                cmd.Parameters["@campground_id"].Value = campground_id;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allSitesinCampground.Add(GetAllSitesinCampground(reader));

                }
            }

            return allSitesinCampground;
        }


        private Reservation GetReservationfromCampground(SqlDataReader reader)
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

        private Site GetAllSitesinCampground(SqlDataReader reader)
        {
            Site site = new Site();

            site.site_id = Convert.ToInt32(reader["site_id"]);
            site.campground_id = Convert.ToInt32(reader["campground_id"]);
            site.site_number = Convert.ToInt32(reader["site_number"]);
            site.max_occupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.accessible = Convert.ToBoolean(reader["accessible"]);
            site.utilities = Convert.ToBoolean(reader["utilities"]);
            site.max_rv_length = Convert.ToInt32(reader["max_rv_length"]);
            return site;

        }
       public IList<Site> AvailbleSites(IList<Reservation> allReservations, IList<Site> allSites)
        {
            IList<Site> currentAvailSites = new List<Site>();

            List<int> helperSites_site_id = new List<int>();
            List<int> helperReservations_site_id = new List<int>();

            foreach (Reservation reservation in allReservations)
            {
                helperReservations_site_id.Add(reservation.site_id);
            }
            foreach (Site site in allSites)
            {
                helperSites_site_id.Add(site.site_id);
             
            }
            IEnumerable<int> site_id_avail = helperSites_site_id.Except(helperReservations_site_id);
           foreach(Site site in allSites)
            {
                if (site_id_avail.Contains(site.site_id))
                {
                    currentAvailSites.Add(site);
                }
            }
            return currentAvailSites;


        }


    }
}


