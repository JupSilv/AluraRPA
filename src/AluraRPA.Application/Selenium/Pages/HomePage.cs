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
        private ILogger<Navigator> _logger { get; init; }
        private IAluraRepository _aluraRepository { get; set; }

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
            try
            {
                //Filtra por cursos
                if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:filtroBusca"])) is not null)
                    _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:filtroBuscaCursos"])).Click();

                Thread.Sleep(3000);

                //Valida se retornou a busca
                if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:pagina"])) is not null)
                {

                    //Lista os resultados da busca
                    IList<IWebElement> elementList = _driver.FindElements(By.XPath(_configuration["Alura:SearchPage:resultadoBusca"])).ToList();

                    //Total de itens retornado na busca
                    var total = elementList.Count;

                    List<DataExtracted> dataExtracted = new List<DataExtracted>();


                    for (int i = 0; i < total; i++)
                    {
                        _driver.WaitElement(By.XPath($"//*[@id='busca-resultados']/ul[1]/li[{i + 1}]/div/a/div[1]/div[1]/h4")).Click();

                        //Valida se carregou a página
                        if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:enroll"])) is not null)
                        {

                            try
                            {
                                dataExtracted.Add(new DataExtracted
                                {

                                    titulo = _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:titulo"])).Text,
                                    professor = _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:professor"])).Text,
                                    cargaHoraria = _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:cargaHoraria"])).Text,
                                    descricao = _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:descricao"])).Text,
                                });

                                var record = _aluraRepository.InsertData(data: dataExtracted);

                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Falha ao acessar detalhes {ex.Message}");

                            }

                            //retorna para resultado da busca
                            _driver.Navigate().Back();

                        }

                    }

                }

                _logger.LogInformation("Extração concluída");
                return new(true, "Processado", "Extração concluída");

            }
            catch (Exception ex)
            {

                return new(false, "Falha da página", $"Falha ao acessar detalhes: {ex.Message}");

            }

        }

    }
}
