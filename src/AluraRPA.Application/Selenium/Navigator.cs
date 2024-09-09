using AluraRPA.Application.Selenium.Controllers;

namespace AluraRPA.Application.Selenium;
public class Navigator : INavigator
{
    private ILogger<Navigator> _logger { get; init; }
    private IConfiguration _configuration { get; init; }
    private IAluraRepository _aluraRepository { get; init; }
    private IProcessManagerService _processManagerService { get; init; }
    private AluraController _aluraController { get; init; }

    public Navigator(ILogger<Navigator> logger,
                    IConfiguration configuration,
                    IProcessManagerService processManagerService,
                    IAluraRepository aluraRepository, 
                    AluraController aluraController)
    {
        _logger = logger;
        _configuration = configuration;
        _processManagerService = processManagerService;
        _aluraRepository = aluraRepository;
        _aluraController = aluraController;
    }

    public async Task<ResultProcess> NavigationAlura(string url, string searchWord, Credential credential)
    {
        _logger.LogInformation("Acessando URL");
        if (_aluraController.Home(url) is null)
            return new(false, "Erro", "Falha ao acessar URL");

        _logger.LogInformation("Realiza o Logon");
        //if (_aluraController.LoginPage(credential) is null)
        //{
        //    _logger.LogError("Falha ao realizar o login");
        //    return new(false, "Erro", "Falha ao realizar o login");
        //}

        _logger.LogInformation("Efetua pesquisa");
        if (_aluraController.Search(searchWord) is null)
            return new(false, "Erro", "Falha ao efetuar pesquisa");

        _logger.LogInformation("Acessa primeiro item da pesquisa");
        if (_aluraController.Details() is null)
        {
            _logger.LogError("Falha ao exibir detalhes");
            return new(false, "Erro", "Falha ao exibir detalhes");
        }

        _logger.LogInformation("Executa a extração dos dados");


        return new(true, "Concluído", "Navegação concluída");
    }
    public static bool RetryAction(Action<int> action, int retryLimit = 10)
    {
        var retries = 0;
        while (++retries <= retryLimit)
        {
            try
            {
                action.Invoke(retries);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("HTTP request to the remote WebDriver server")
                    || ex.Message.Contains("Unable to get browser")
                    || ex.Message.Contains("does not exist"))
                    throw;
            }
        }
        return false;
    }

}