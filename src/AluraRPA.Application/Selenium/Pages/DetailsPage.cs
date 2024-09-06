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

        public ResultProcess ExtractDetails()
        {
            /**
             * Executa coleta dos dados de:
             * Título
             * Professor
             * Carga horária
             * Descrição
             * */

            DataExtracted dataExtracted = new DataExtracted();

            if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:Conclusao"])) is not null)
            {
                dataExtracted.titulo = _driver.WaitElement(By.XPath("/html/body/section[1]/div/div[1]/p[2]")).Text;
                dataExtracted.professor = _driver.WaitElement(By.XPath("//*[@id='section-icon']/div[1]/section/div/div/div/h3")).Text;
                dataExtracted.cargaHoraria = _driver.WaitElement(By.XPath("/html/body/section[1]/div/div[2]/div[1]/div/div[1]/div/p[1]")).Text;
                dataExtracted.descricao = _driver.WaitElement(By.XPath("//*[@id='section-icon']/div[1]/div/div/p")).Text;

                var record = _aluraRepository.InsertData(dataExtracted);

                return new(true, "Processado", "Extração detalhes da página");

            }

            return new(false, "Falha na extração", "Falha ao extrair detalhes da página");
        }

    }
}
