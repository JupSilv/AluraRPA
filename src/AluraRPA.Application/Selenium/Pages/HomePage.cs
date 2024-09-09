using AluraRPA.Application.Selenium.Extensions;
using AluraRPA.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;
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

        public HomePage(IDriverFactoryService driverManager,
                        ILogger<Navigator> logger,
                        IConfiguration configuration,
                        IAluraRepository aluraRepository)
        {
            _logger = logger;
            _driverManager = driverManager;
            _configuration = configuration;
            _aluraRepository = aluraRepository;
        }

        public ResultProcess HomePageAlura(string url)
        {

            try
            {
                //Inicializa a página
                _driver.Navigate().GoToUrl(url);
                _driver.WaitTime();
                _driver.Manage().Window.Maximize();

                //Valida se a página carregou
                if (_driver.WaitElement(By.XPath(_configuration["Alura:HomePage:txtSearch"])) is not null)
                {
                    return new(true, "Acesso à página", "Página carregada com sucesso");
                    //_logger.LogInformation("Página carregada com sucesso");

                    ////Redireciona para o login
                    //if (_driver.WaitElement(By.XPath("/html/body/main/section[1]/header/div/nav/div[3]/a[1]")) is not null)
                    //{
                    //    _logger.LogInformation("Acessa o login");
                    //    _driver.WaitElement(By.XPath("/html/body/main/section[1]/header/div/nav/div[3]/a[1]")).Click();
                    //    return new(true, "Acesso à página", "Página carregada com sucesso");
                    //}
                    //return new(false, "Falha da página", "Falha ao acesso a URL");
                }

                _logger.LogError("Falha ao acesso a URL");
                return new(false, "Falha da página", "Falha ao acesso a URL");

            }
            catch (Exception ex)
            {

                return new(false, "Erro", ex.StackTrace);

            }

        }
        public ResultProcess Search(string searchWord)
        {
            try
            {
                var buscaElement = _driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:busca"]))
                   ?? _driver.WaitElement(By.XPath("//*[@id='header-barraBusca-form-campoBusca']"));

                if (buscaElement is not null)
                {
                    //Insere texto na caixa de busca
                    buscaElement.SendKeys(searchWord + Keys.Enter);
                    Thread.Sleep(3000);

                    if (_driver.WaitElement(By.XPath("//*[@id='busca-resultados']")) is not null)
                    {
                        _logger.LogInformation("Busca efetuada");
                        return new(true, "Busca", "Busca efetuada");
                    }

                    _logger.LogError("Falha ao executar a busca na caixa de pesquisa");
                    return new(false, "Falha na busca", "Falha ao executar a busca na caixa de pesquisa");

                }

                _logger.LogError("Falha na busca");
                return new(false, "Falha da página", "Falha na busca");
            }
            catch (Exception ex)
            {

                return new(false, "Erro", ex.StackTrace);

            }
        }

        public ResultProcess Details()
        {
            try
            {
                //Valida se retornou a busca
                if (_driver.WaitElement(By.XPath("//*[@id='busca-resultados']/ul")) is not null)
                {

                    //Lista os resultados da busca
                    IList<IWebElement> elementList = _driver.FindElements(By.XPath("//*[@id='busca-resultados']/ul")).ToList();

                    //Total de itens retornado na busca
                    var total = elementList.Count;

                    List<DataExtracted> dataExtracted = new List<DataExtracted>();


                    for (int i = 0; i < total; i++)
                    {
                        _driver.WaitElement(By.XPath($"//*[@id='busca-resultados']/ul/li[{i + 1}]/a")).Click();
                        //_driver.WaitElement(By.XPath($"//*[@id='busca-resultados']/ul[1]/li[{i + 1}]/div/a/div[1]/div[1]/h4")).Click();
                        Thread.Sleep(3000);

                        try
                        {
                            dataExtracted.Add(new DataExtracted
                            {

                                titulo = _driver.WaitElement(By.XPath("/html/body/section[1]/div/div[1]/p[2]")).Text,
                                professor = _driver.WaitElement(By.XPath("//*[@id='section-icon']/div[1]/section/div/div/div/h3")).Text,
                                cargaHoraria = _driver.WaitElement(By.XPath("/html/body/section[1]/div/div[2]/div[1]/div/div[1]/div/p[1]")).Text,
                                descricao = _driver.WaitElement(By.XPath("//*[@id='section-icon']/div[1]/div/div/p")).Text,
                            });

                            var record = _aluraRepository.InsertData(dataExtracted.ToList());

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Falha ao acessar detalhes {ex.Message}");

                        }

                        //retorna para resultado da busca
                        _driver.Navigate().Back();

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
