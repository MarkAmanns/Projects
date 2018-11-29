using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds information needed in the ParkDetails View
    /// </summary>
    public class ParkDetailViewModel
    {
        /// <summary>
        /// Holds Park Information
        /// </summary>
        public Park ParkDetails = new Park();
        /// <summary>
        /// Holds Weather information
        /// </summary>
        public Forecast Forecast = new Forecast();
        /// <summary>
        /// Holds the char to represent displaying fahrenheit or celsius
        /// </summary>
        public char TempType;
        /// <summary>
        /// Dictionary to hold extra advice for each weather condition
        /// </summary>
        public Dictionary<string, string> Advice = new Dictionary<string, string>()
        {
            {"snow","Don't forget to pack snowshoes!" },
            {"rain","Pack rain gear and wear waterproof shoes." },
            {"thunderstorms", "Seek shelter! Avoid hiking on exposed ridges." },
            {"sunny", "Pack sunblock!" },
            {"hot", "Bring an extra gallon of water." },
            {"difference", "Wear breathable layers."  },
            {"cold", "Exposure to frigid temperatures may cause injury or death, stay safe!" }
        };

        /// <summary>
        /// Converts Fahrenheit temperatures from the DB to Celsius
        /// </summary>
        public void ShowTemps()
        {
            foreach(var model in Forecast.FiveDayForecast)
            {
                if (TempType == 'C')
                {
                    model.ShowHigh = (int)Math.Round((model.High - 32) * (5.0 / 9.0));
                    model.ShowLow = (int)Math.Round((model.Low - 32) * (5.0 / 9.0));
                }
                else
                {
                    model.ShowHigh = model.High;
                    model.ShowLow = model.Low;
                }
            }
        }
    }
}