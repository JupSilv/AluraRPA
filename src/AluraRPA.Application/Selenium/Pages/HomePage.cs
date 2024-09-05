using AluraRPA.Application.Selenium.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraRPA.Application.Selenium.Pages
{
    public class HomePage
    {

        private IDriverFactoryService _driverManager { get; init; }
        private IWebDriver _driver => _driverManager.Instance;
        private IConfiguration _configuration { get; set; }

        public HomePage(IDriverFactoryService driverManager
                        , IConfiguration configuration)
        {
            _driverManager = driverManager;
            _configuration = configuration;
        }

        public ResultProcess HomePageAlura(string url)
        {

            try
            {
                _driver.Navigate().GoToUrl(url);
                _driver.WaitTime();

                if (_driver.WaitElement(By.XPath("//*[@id='header-barraBusca-form-campoBusca']")) is not null)
                    return new(true, "Acesso à página", "Página carregada com sucesso");

                return new(false, "Falha da página", "Falha ao acesso a URL");

            }
            catch (Exception ex)
            {

                return new(false, "Erro", ex.StackTrace);

            }

        }
        public ResultProcess Search(string searchWord)
        {

            //Insere texto na caixa de busca
            if (_driver.WaitElement(By.XPath(_configuration["Alura:HomePage:txtSearch"])) is not null)
            {
                _driver.WaitElement(By.XPath(_configuration["Alura:HomePage:txtSearch"])).SendKeys(searchWord + Keys.Enter);

                Thread.Sleep(5000);

                if (_driver.WaitElement(By.XPath(_configuration["Alura:HomePage:filter"])) is not null)
                    return new(true, "Busca", "Busca efetuada");

                return new(false, "Falha na busca", "Falha ao executar a busca na caixa de pesquisa");

            }

            return new(false, "Falha da página", "Falha ao acesso a URL");
        }

        public ResultProcess Details()
        {
            //Abre detalhes do primeiro item retornado da busca
            if (_driver.WaitElement(By.XPath(_configuration["Alura:HomePage:searchResult"])) is not null)
            {
                _driver.WaitElement(By.XPath(_configuration["Alura:HomePage:searchResult"])).Click();

                Thread.Sleep(5000);

                if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:Conclusao"])) is not null)
                    return new(true, "Detalhes", "Detalhes exibido");

                return new(false, "Falha no item", "Falha ao exibir detalhes");
            }

            return new(false, "Falha da página", "Falha ao acessar detalhes");
        }

    }
}
