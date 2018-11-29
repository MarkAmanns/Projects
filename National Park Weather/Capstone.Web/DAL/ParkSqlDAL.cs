using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Capstone.Web.DAL
{
    public class ParkSqlDAL : IParkSqlDAL
    {
        private string _connectionString;

        public ParkSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Park> GetAllParks()
        {
            List<Park> result = new List<Park>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_AllDepartments = "SELECT * FROM park";
                cmd.CommandText = SQL_AllDepartments;
                cmd.Connection = connection;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = PopulateParkFromReader(reader);
                    result.Add(item);
                }
            }

            return result;
        }

        public Forecast GetForecast(Park park)
        {
            Forecast result = new Forecast();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_AllDepartments = "SELECT * FROM weather " +
                                            "JOIN park ON park.parkCode = weather.parkCode " +
                                            "WHERE weather.parkCode = @parkCode;";
                cmd.CommandText = SQL_AllDepartments;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@parkCode", park.ParkCode);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = PopulateWeatherModelFromReader(reader);
                    result.FiveDayForecast.Add(item);
                }
            }

            return result;
        }

        public Park GetParkById(string id)
        {
            Park result = new Park();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_AllDepartments = "SELECT * FROM park WHERE parkCode = @id";
                cmd.CommandText = SQL_AllDepartments;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = PopulateParkFromReader(reader);
                    result = item;
                }
            }

            return result;
        }

        public bool SaveSurvey(SurveyResult result)
        {
            int saved = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_SaveSurvey = "INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) " +
                    "VALUES(@ParkCode, @EmailAddress, @State, @ActivityLevel); " +
                    "SELECT CAST(SCOPE_IDENTITY() AS int);";
                cmd.CommandText = SQL_SaveSurvey;
                cmd.Parameters.AddWithValue("@ParkCode", result.ParkCode);
                cmd.Parameters.AddWithValue("@EmailAddress", result.EmailAddress);
                cmd.Parameters.AddWithValue("@State", result.State);
                cmd.Parameters.AddWithValue("@ActivityLevel", result.ActivityLevel);
                cmd.Connection = connection;
                saved = (int)cmd.ExecuteScalar();
            }

            return (saved > 0 ? true : false);
        }

        public List<FavoriteParks> GetFavoriteParks()
        {
            List<FavoriteParks> result = new List<FavoriteParks>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                string SQL_FavoriteParks = "SELECT park.parkCode, park.parkName, " +
                        "COUNT(survey_result.parkCode) AS favoriteParksCount " +
                        "FROM survey_result JOIN park ON park.parkCode = survey_result.parkCode " +
                        "GROUP BY park.parkCode, park.parkName ORDER BY COUNT(survey_result.parkCode) DESC;";
                cmd.CommandText = SQL_FavoriteParks;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = PopulateFavoriteParksFromReader(reader);
                    result.Add(item);
                }
            }

            return result;
        }

        private Park PopulateParkFromReader(SqlDataReader reader)
        {
            Park item = new Park();
            item.ParkCode = (string)reader["parkCode"];
            item.ParkName = (string)reader["parkName"];
            item.State = (string)reader["state"];
            item.Acreage = (int)reader["acreage"];
            item.ElevationInFeet = (int)reader["elevationInFeet"];
            item.MilesOfTrail = (float)reader["milesOfTrail"];
            item.NumberOfCampsites = (int)reader["numberOfCampsites"];
            item.TypeOfClimate = (string)reader["climate"];
            item.YearFounded = (int)reader["yearFounded"];
            item.AnnualVisitorCount = (int)reader["annualVisitorCount"];
            item.InspirationalQuote = (string)reader["inspirationalQuote"];
            item.InspirationalQuoteSource = (string)reader["inspirationalQuoteSource"];
            item.ParkDescription = (string)reader["parkDescription"];
            item.EntryFee = (int)reader["entryFee"];
            item.NumberOfAnimalSpecies = (int)reader["numberOfAnimalSpecies"];
            return item;
        }

        private WeatherModel PopulateWeatherModelFromReader(SqlDataReader reader)
        {
            WeatherModel item = new WeatherModel();
            item.Forecast = (string)reader["forecast"];
            item.Low = (int)reader["low"];
            item.High = (int)reader["high"];
            return item;
        }

        private FavoriteParks PopulateFavoriteParksFromReader(SqlDataReader reader)
        {
            FavoriteParks item = new FavoriteParks();
            item.ParkCode = (string)reader["parkCode"];
            item.ParkName = (string)reader["parkName"];
            item.FavoriteParksCount = (int)reader["favoriteParksCount"];
            return item;
        }
    }
}
