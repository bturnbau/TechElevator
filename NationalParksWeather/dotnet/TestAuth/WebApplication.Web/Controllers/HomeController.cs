using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Dependency Inject a DAO. 
        /// </summary>
        private readonly IParkDAO parkDAO;
        public HomeController(IParkDAO parkDAO)
        {
            this.parkDAO = parkDAO;
        }

        public IActionResult Index()
        {
            IList<Park> parks = parkDAO.GetAllPark();
            return View(parks);
        }

        /// <summary>
        /// Returns detail view of Park with temperature stored in session data. 
        /// </summary>
        /// <param name="code">Park Code</param>
        /// <param name="isFarenheit"> Defaults to 'true' from Home/index view. </param>
        /// <param name="isSwitch"> To determine if user is toggling temperature or switching page</param>
        /// <returns></returns>
        public IActionResult Detail(string code, bool isFarenheit, bool isSwitch = false)
        {
            Park parkDetail = parkDAO.GetById(code);
            IList<Weather> weathers = parkDAO.GetWeather(code);

            // See if the user has visited the page. Store in Session Data   
            bool state = true;
            if (HttpContext.Session.GetString("test") == null)
            {
                //save true
                HttpContext.Session.SetString("test", state.ToString());
            }
            else
            {
                state = Convert.ToBoolean(HttpContext.Session.GetString("test")); 
            }

            //if toggling temperature   >>>> change
            if (isSwitch)
            {
                if (state)
                {
                    HttpContext.Session.SetString("test", "false");
                }
                else
                {
                    HttpContext.Session.SetString("test", "true");
                }
            }
          

            bool a = Convert.ToBoolean(HttpContext.Session.GetString("test"));
            DetailViewModel dvm = new DetailViewModel(parkDetail, weathers, a);
            
            return View(dvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
