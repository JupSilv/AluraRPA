namespace AluraRPA.Application.Selenium;
public class Navigator : INavigator
{
    private ILogger<Navigator> _logger { get; init; }
    private IConfiguration _configuration { get; init; }
    private IAluraRepository _aluraRepository { get; init; }
    private IProcessManagerService _processManagerService { get; init; }

    public Navigator(ILogger<Navigator> logger,
                    IConfiguration configuration,
                    IProcessManagerService processManagerService,
                    IAluraRepository aluraRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _processManagerService = processManagerService;
        _aluraRepository = aluraRepository;
    }

    public async Task<NavigationResult> StartNavigationAsync(Credential credential)
    {
        throw new NotImplementedException();
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