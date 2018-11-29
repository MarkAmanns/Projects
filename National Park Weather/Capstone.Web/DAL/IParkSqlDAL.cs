using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Web.DAL
{
    public interface IParkSqlDAL
    {
        /// <summary>
        /// Retrieves all parks from the DB
        /// </summary>
        /// <returns>A list of all parks</returns>
        List<Park> GetAllParks();

        /// <summary>
        /// Retrieves all forecast data available for a specific park in the DB
        /// </summary>
        /// <param name="park">Park you want the forecast data for</param>
        /// <returns>A forecast object containing a list of weather models</returns>
        Forecast GetForecast(Park park);

        /// <summary>
        /// Retrieves a specific park from the database based on ID
        /// </summary>
        /// <param name="id">ID of the park you want to retrieve</param>
        /// <returns>A single park with matching ID</returns>
        Park GetParkById(string id);

        /// <summary>
        /// Writes survey results to survey_result table
        /// </summary>
        /// <returns>Bool whether survey was successfully written to the database<returns>
        bool SaveSurvey(SurveyResult result);

        /// <summary>
        /// Retrieves favorite parks based on the count of each park's favorite posts
        /// </summary>
        /// <returns>A list of park objects with park code, park name and favorites count in descending order<returns>
        List<FavoriteParks> GetFavoriteParks();
    }
}
