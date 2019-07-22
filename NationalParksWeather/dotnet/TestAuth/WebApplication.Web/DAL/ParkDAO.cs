using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class ParkDAO : IParkDAO
    {
        private const string _getLastIdSQL = "select cast(SCOPE_IDENTITY() as int);";
        private string connectionString;

        public ParkDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Gets all Parks from the Database
        /// </summary>
        /// <returns>List of Parks</returns>
        public IList<Park> GetAllPark()
        {
            IList<Park> parks = new List<Park>();

            string productSearchSql = @"select * from park;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(productSearchSql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parks.Add(MapRowToPark(reader));
                }
            }

            return parks;
        }
        /// <summary>
        /// Maps a park from the database to a 'Park' object.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Park</returns>
        private Park MapRowToPark(SqlDataReader reader)
        {
            return new Park()
            {

                parkCode = Convert.ToString(reader["parkCode"]),
                parkName = Convert.ToString(reader["parkName"]),
                state = Convert.ToString(reader["state"]),
                acreage = Convert.ToInt32(reader["acreage"]),
                elevation = Convert.ToInt32(reader["elevationInFeet"]),
                milesofTrail = Convert.ToInt32(reader["milesOfTrail"]),
                numberofCampsites = Convert.ToInt32(reader["numberOfCampsites"]),
                climate = Convert.ToString(reader["climate"]),
                yearFounded = Convert.ToInt32(reader["yearFounded"]),
                annualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]),
                inspirationalQuote = Convert.ToString(reader["inspirationalQuote"]),
                inspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]),
                entryFee = Convert.ToInt32(reader["entryFee"]),
                parkDescription = Convert.ToString(reader["parkDescription"]),
                numberofAnimalSpecies = Convert.ToInt32(reader["numberofAnimalSpecies"]),
            };

        }
        /// <summary>
        /// Gets a park from the db by the parkCode.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Park</returns>
        public Park GetById(string code)
        {
            Park park = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM park WHERE parkCode = @code", connection);
                    command.Parameters.AddWithValue("@code", code);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        park = MapRowToPark(reader);
                    }

                    return park;
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine($"An error occurred reading product {park.parkCode} - ${ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all weather from DB that has a specific parkCode
        /// </summary>
        /// <param name="code"></param>
        /// <returns>List of 'weather' objects</returns>
        public IList<Weather> GetWeather(string code)
        {
            {
                IList<Weather> weathers = new List<Weather>();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand("SELECT * FROM weather WHERE parkCode = @code", connection);
                        command.Parameters.AddWithValue("@code", code);

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            weathers.Add(MapRowToWeather(reader));
                        }

                        return weathers;
                    }
                }
                catch (SqlException ex)
                {
                    Console.Error.WriteLine($"An error occurred reading product {weathers} - ${ex}");
                    throw;
                }
            }
       
        }
        /// <summary>
        /// Maps a weather record from the databse to a 'Weather' object.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>A 'weather' object</returns>
        private Weather MapRowToWeather(SqlDataReader reader)
        {
            return new Weather()
            {

                parkCode = Convert.ToString(reader["parkCode"]),
                fiveDayForecastValue = Convert.ToString(reader["fiveDayForecastValue"]),
                low = Convert.ToInt16(reader["low"]),
                high = Convert.ToInt16(reader["high"]),
                forecast = Convert.ToString(reader["forecast"]),

            };

        }
        /// <summary>
        /// Saves a new survey to the database
        /// </summary>
        /// <param name="svm"></param>
        /// <returns>boolean true if completed entry</returns>
        public bool SaveNewSurvey(SurveyViewModel svm)
        {
            bool inserted = false;
            const string sql = @"insert into survey_result (parkCode, emailAddress, state, activityLevel) 
                                values(@parkCode, @emailAddress, @state, @activityLevel)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql + _getLastIdSQL, conn);
                    cmd.Parameters.AddWithValue("@parkCode", svm.parkCode);
                    cmd.Parameters.AddWithValue("@emailAddress", svm.email);
                    cmd.Parameters.AddWithValue("@state", svm.state);
                    cmd.Parameters.AddWithValue("@activityLevel", svm.activityLevel);

                    int postId = (int)cmd.ExecuteScalar();
                    inserted = true;
                }
                return inserted;
            }
            catch (Exception) { return inserted; }

        }
        /// <summary>
        /// Gets all surveys grouped by park, orderd by count(high ->low) and alphabetical.
        /// </summary>
        /// <returns>List of SurveyAggregateViewModels</returns>
        public IList<SurveyAggregateViewModel> GetAllSurveys()
        {
            IList<SurveyAggregateViewModel> surveys = new List<SurveyAggregateViewModel>();

            string surveySearchSql = @"   select count(survey_result.parkCode) as 'Count' , park.parkName, park.parkCode from survey_result 
                                          join park on park.parkCode = survey_result.parkCode
                                          group by park.parkName, park.parkCode
                                          order by count(survey_result.parkCode) desc, park.parkName asc; ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(surveySearchSql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    surveys.Add(MapRowToAggregate(reader));
                }
            }

            return surveys;
        }
        /// <summary>
        /// Maps a aggregate from the database to an SurveyAggregateViewModel.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>SurveyAggregateViewModel</returns>
        private SurveyAggregateViewModel MapRowToAggregate(SqlDataReader reader)
        {
            return new SurveyAggregateViewModel()
            {
                count = Convert.ToInt16(reader["Count"]),
                parkName = Convert.ToString(reader["parkName"]),
                parkCode = Convert.ToString(reader["parkCode"]),

            };

        }
    }
}
