using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Models all information available for a favorites display
    /// </summary>
    public class FavoriteParks
    {
        public string ParkCode { get; set; }

        public string ParkName { get; set; }

        public int FavoriteParksCount { get; set; }
    }
}