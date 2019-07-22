using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class DetailViewModel
    {
        public DetailViewModel(Park park, IList<Weather> weathers, bool isFarenheit)
        {
            this.weathers = weathers;
            this.park = park;
            this.isFarenheit = isFarenheit;
        }

        public Park park { get; set; }
        public IList<Weather> weathers { get; set; }
        public bool isFarenheit { get; set; }
    }
}
