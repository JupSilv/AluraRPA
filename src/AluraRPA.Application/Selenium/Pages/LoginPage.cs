using AluraRPA.Application.Selenium.Extensions;

namespace AluraRPA.Application.Selenium.Pages
{
    public class LoginPage
    {
        private IDriverFactoryService _driverManager { get; init; }
        private IWebDriver _driver => _driverManager.Instance;
        private IConfiguration _configuration { get; set; }
        private IAluraRepository _aluraRepository { get; init; }
        private ILogger<Navigator> _logger { get; init; }

        public LoginPage(IDriverFactoryService driverManager
                            , IConfiguration configuration)
        {
            _driverManager = driverManager;
            _configuration = configuration;
        }

        public ResultProcess LoginPageAlura(Credential aluraCredential)
        {

            //valida página do login
            _logger.LogInformation("Efetua o login");
            if (_driver.WaitElement(By.XPath(_configuration["Alura:LoginPage:Entrar"])) is not null)
            {
                _logger.LogInformation("Insere credenciais");
                _driver.WaitElement(By.XPath(_configuration["Alura:LoginPage:email"])).SendKeys(aluraCredential.User);
                Thread.Sleep(2000);
                _driver.WaitElement(By.XPath(_configuration["Alura:LoginPage:senha"])).SendKeys(aluraCredential.Password + Keys.Enter);
                if (_driver.WaitElement(By.XPath(_configuration["Alura:SearchPage:Logon"])) is not null)
                    return new(true, "Login página", "Login realizado com sucesso");

            }

            _logger.LogError("Falha ao realizar login");
            return new(false, "Falha no login", "Falha ao realizar login");
        }

    }
}
