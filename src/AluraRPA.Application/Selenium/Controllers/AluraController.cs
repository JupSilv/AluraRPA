using AluraRPA.Application.Selenium.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraRPA.Application.Selenium.Controllers
{
    public class AluraController
    {

        private HomePage _homePage { get; set; }
        private DetailsPage _detailsPage { get; set; }
        public AluraController(HomePage homePage)
        {
            _homePage = homePage;
        }

        public string Home(string url)
        {
            var result = _homePage.HomePageAlura(url);
            if (!result.sucess)
            {
                return null;
            }
            return result.obs.ToString();

        }

        public string Search(string searchWord)
        {
            var result = _homePage.Search(searchWord);
            if (!result.sucess)
            {
                return null;
            }
            return result.obs.ToString();
        }

        public string Details()
        {
            var result = _homePage.Details();
            if (!result.sucess)
            {
                return null;
            }
            return result.obs.ToString();
        }

        public string Extraction()
        {
            var result = _detailsPage.ExtractDetails();
            if (!result.sucess)
            {
                return null;
            }
            return result.obs.ToString();
        }
    }
}
