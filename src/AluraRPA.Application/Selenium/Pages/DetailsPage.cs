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

        public DetailsPage(IDriverFactoryService driverManager
                            , IConfiguration configuration)
        {
            _driverManager = driverManager;
            _configuration = configuration;
        }

        public ResultProcess ExtractDetails()
        {
            /**
             * Executa coleta dos dados de:
             * Título
             * Professor
             * Carga horária
             * Descrição
             * */

            if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:Conclusao"])) is not null)
            {
                //Título


            }

            return new(false, "Falha na extração", "Falha ao extrair detalhes da página");
        }

    }
}
