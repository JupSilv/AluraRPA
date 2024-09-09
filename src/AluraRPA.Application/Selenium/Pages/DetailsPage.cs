using AluraRPA.Application.Selenium.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraRPA.Application.Selenium.Pages
{
    public class DetailsPage
    {
        private IDriverFactoryService _driverManager { get; init; }
        private IWebDriver _driver => _driverManager.Instance;
        private IConfiguration _configuration { get; set; }
        private IAluraRepository _aluraRepository { get; init; }

        public DetailsPage(IDriverFactoryService driverManager
                            , IConfiguration configuration)
        {
            _driverManager = driverManager;
            _configuration = configuration;
        }

        
    }
}
