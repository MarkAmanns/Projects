using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Web.DAL;
using Capstone.Web.Models;

namespace Capstone.Web.Tests.DAL_Tests
{
    [TestClass]
    public class ParkSqlDALTests
    {
        private TransactionScope tran = null;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=NPGeek;Integrated Security=True";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();
               
                cmd = new SqlCommand("INSERT INTO park VALUES ('ABCD', 'Test Park', 'Ohio', 23423, 555, 555, 1, 'Woodland', 2018, 123456, 'This is a quote', 'Tony Slack', 'This is a park description', 0, 400);", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO weather VALUES ('ABCD', 1, 25, 50, 'rain')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather VALUES ('ABCD', 2, 25, 50, 'rain')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather VALUES ('ABCD', 3, 25, 50, 'rain')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather VALUES ('ABCD', 4, 25, 50, 'rain')", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO weather VALUES ('ABCD', 5, 25, 50, 'rain')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetAllParksTest()
        {
            ParkSqlDAL _dal = new ParkSqlDAL(connectionString);
            List<Park> parks = _dal.GetAllParks();

            Assert.IsNotNull(parks, "Method should retrieve at least one park.");
        }

        [TestMethod]
        public void GetForecastTest()
        {
            ParkSqlDAL _dal = new ParkSqlDAL(connectionString);
            Park park = new Park();
            park.ParkCode = "ABCD";
            Forecast forecast = _dal.GetForecast(park);
            Assert.AreEqual(5, forecast.FiveDayForecast.Count, "Forecast should recieve 5 different Weather Models");

        }

        [TestMethod]
        public void GetParkByIdTest()
        {
            ParkSqlDAL _dal = new ParkSqlDAL(connectionString);
            Park park = _dal.GetParkById("ABCD");

            Assert.IsNotNull(park, "Method should retrieve a park");
            Assert.AreEqual(park.ParkName, "Test Park", "Should Retrieve Test park");
            
        }

        [TestMethod]
        public void SaveSurveyTestAndFavoriteParksTest()
        {
            ParkSqlDAL _dal = new ParkSqlDAL(connectionString);
            SurveyResult survey = new SurveyResult();
            survey.State = "Ohio";
            survey.ParkCode = "ABCD";
            survey.EmailAddress = "test123@gmail.com";
            survey.ActivityLevel = "Inactive";
            bool result = _dal.SaveSurvey(survey);
            Assert.IsTrue(result, "Survey should successfully insert");
            
            List<FavoriteParks> parks = _dal.GetFavoriteParks();
            Assert.IsNotNull(parks, "Should retrieve results from test survey");
            foreach (var park in parks)
            {
                Assert.IsTrue(park.FavoriteParksCount > 0, "Should not contain parks without votes");
            }

        }
    }
}
