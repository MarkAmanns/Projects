using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    /// <summary>
    /// Holds results for entered survey
    /// </summary>
    public class SurveyResult
    {
        [Required]
        public string ParkCode { get; set; }

        [Required(ErrorMessage ="You must enter an email address")]
        [EmailAddress(ErrorMessage ="Email address is invalid")]
        public string EmailAddress { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ActivityLevel { get; set; }
    }
}