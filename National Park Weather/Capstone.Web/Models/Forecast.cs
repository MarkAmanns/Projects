using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Container for Weather Models
    /// </summary>
    public class Forecast
    {
        /// <summary>
        /// Contains a list of Weather objects 
        /// </summary>
        public List<WeatherModel> FiveDayForecast = new List<WeatherModel>();
    }
}