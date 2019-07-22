using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ParkReservation
{
    public class ParkDAO : IParkDAO
    {
        private string connectionString;
        public ParkDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> ViewAllParks()
        {
            List<Park> AllParks = new List<Park>();
            const string sql = "select * from park order by name;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AllParks.Add(GetParkFromReader(reader));
                }
            }

            return AllParks;
        }
        private Park GetParkFromReader(SqlDataReader reader)
        {
            Park park = new Park();

            park.park_id = Convert.ToInt32(reader["park_id"]);
            park.name = Convert.ToString(reader["name"]);
            park.location = Convert.ToString(reader["location"]);
            park.establish_date = Convert.ToDateTime(reader["establish_date"]);
            park.description = Convert.ToString(reader["description"]);
            park.area = Convert.ToInt32(reader["area"]);
            park.visitors = Convert.ToInt32(reader["visitors"]);
            return park;
        }
        public Park ShowParkDetails(int Key)
        {
            Park park = new Park();
            const string sql = "select * from park where park_id = @key;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@key", SqlDbType.Int);
                cmd.Parameters["@key"].Value = Key;
                
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    park = GetParkFromReader(reader);
                }

            }
            return park;
        }

    }
}
