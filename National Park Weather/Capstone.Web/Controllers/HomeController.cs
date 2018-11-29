using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.DAL;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class HomeController : Controller
    {
        private IParkSqlDAL _dal = null;

        /// <summary>
        /// Uses Dependency Injection to access the DAL for the NPGeek DB
        /// </summary>
        /// <param name="dal">DAO to pass in</param>
        public HomeController(IParkSqlDAL dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// Gets the Homepage View
        /// </summary>
        /// <returns>Homepage View</returns>
        [HttpGet]
        public ActionResult Index()
        {
            ParkList parkList = new ParkList();
            parkList.Parks.AddRange(_dal.GetAllParks());
            return View("Index", parkList);
        }

        /// <summary>
        /// Gets the detail view for the park based on the park ID parameter passed in
        /// </summary>
        /// <param name="id">Park ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string id)
        {
            ParkDetailViewModel model = new ParkDetailViewModel();
            Park park = new Park();
            park = _dal.GetParkById(id);
            model.ParkDetails = park;
            model.Forecast = _dal.GetForecast(park);

            //Initializes the default temperature type if no temperature type has been set prior
            if(Session["TempType"] == null)
            {
                Session["TempType"] = 'F';
            }
            model.TempType = (char)Session["TempType"];

            return View("Detail", model);
        }

        /// <summary>
        /// Posts a new temperature type to the session data to display Fahrenheit or Celsius
        /// persistently.
        /// </summary>
        /// <param name="tempType">Character designating Fahrenheit or Celsius</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Detail(char tempType)
        {
            Session["TempType"] = tempType;
            return RedirectToAction("Detail");
        }

        /// <summary>
        /// Gets the survey form to select a favorite park and vote
        /// </summary>
        /// <returns>Survey Form View</returns>
        [HttpGet]
        public ActionResult Survey()
        {
            SurveyViewModel model = new SurveyViewModel();

            ParkList list = new ParkList();
            list.Parks.AddRange(_dal.GetAllParks());

            foreach(var park in list.Parks)
            {
                model.Codes[park.ParkCode] = park.ParkName;
            }

            TempData["ParkCodes"] = model;
            //saves the list of parks to choose from to display in case the model validation fails

            return View("Survey", model);
        }

        /// <summary>
        /// Posts the new survey to the NPGeek DB if it passes validation
        /// </summary>
        /// <param name="result">Result of the previous survey</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Survey(SurveyResult result)
        {
            if (!ModelState.IsValid || !_dal.SaveSurvey(result))
            {
                SurveyViewModel model = new SurveyViewModel();
                model = TempData["ParkCodes"] as SurveyViewModel; //passes codes for dropdown back to view
                TempData["ParkCodes"] = model;
                return View("Survey", model); //this means you don't have to hit the DB every time you fail validation
            }

            return RedirectToAction("FavoriteParks");
        }

        /// <summary>
        /// Gets and displays list of the most highly voted parks
        /// </summary>
        /// <returns>View of all parks ordered by votes</returns>
        [HttpGet]
        public ActionResult FavoriteParks()
        {
            List<FavoriteParks> parks = _dal.GetFavoriteParks();
            return View("FavoriteParks", parks);
        }
    }
}