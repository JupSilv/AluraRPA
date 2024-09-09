using AluraRPA.Application.Selenium.Pages;
namespace AluraRPA.Application.Selenium.Controllers
{
    public class AluraController
    {

        private HomePage _homePage { get; set; }
        private LoginPage _loginPage { get; set; }
        private DetailsPage _detailsPage { get; set; }
        public AluraController(HomePage homePage
            , LoginPage loginPage
            , DetailsPage detailsPage)
        {
            _homePage = homePage;

            _loginPage = loginPage;
            _detailsPage = detailsPage;
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

        public string LoginPage(Credential credential)
        {
            var result = _loginPage.LoginPageAlura(credential);
            if (!result.sucess)
            {
                return null;
            }
            return result.obs.ToString();
        }
    }
}
