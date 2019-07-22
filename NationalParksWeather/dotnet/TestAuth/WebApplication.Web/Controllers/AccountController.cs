using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{    
    public class AccountController : Controller
    {   /// <summary>
        /// Dependency Inject a DAO and our Authorization Provider.
        /// </summary>
        private readonly IAuthProvider authProvider;
        private readonly IParkDAO parkDAO;
        public AccountController(IAuthProvider authProvider, IParkDAO parkDAO)
        {
            this.authProvider = authProvider;
            this.parkDAO = parkDAO;
        }
        
        //[AuthorizationFilter] // actions can be filtered to only those that are logged in
        [AuthorizationFilter("Admin", "Author", "Manager", "User")]  //<-- or filtered to only those that have a certain role
        [HttpGet]
        public IActionResult Index()
        {

            var user = authProvider.GetCurrentUser();

            IList<SurveyAggregateViewModel> surveys = parkDAO.GetAllSurveys();
            surveys[0].username = user.Username;
            //maybe here I want to get the user on the session and see all their surveys?

            return View(surveys);

    
        }

        [AuthorizationFilter("Admin", "Author", "Manager", "User")]  //<-- or filtered to only those that have a certain role
        [HttpGet]
        public IActionResult Survey()
        {
            SurveyViewModel svm = new SurveyViewModel();
            return View(svm);
        }
        
        [AuthorizationFilter("Admin", "Author", "Manager", "User")]  //<-- or filtered to only those that have a certain role
        [HttpPost]
        public IActionResult Survey(SurveyViewModel svm)
        {
           
            if (!ModelState.IsValid)
            {
                return View(svm);
            }
           
             parkDAO.SaveNewSurvey(svm);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.Email, loginViewModel.Password);
                if (validLogin)
                {
                 
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("Index", "Account");
                }
            }

            return View(loginViewModel);
        }
        
        [HttpGet]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();

            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {            
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".
                authProvider.Register(registerViewModel.Email, registerViewModel.Password, role: "User"); 

                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }
    }
}