using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IParkDAO
    {
        IList<Park> GetAllPark();
        Park GetById(string code);
        IList<Weather> GetWeather(string code);
        bool SaveNewSurvey(SurveyViewModel svm);
        IList<SurveyAggregateViewModel> GetAllSurveys();

    }
}
