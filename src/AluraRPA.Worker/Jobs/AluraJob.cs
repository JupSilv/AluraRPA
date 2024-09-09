

namespace AluraRPA.Worker.Jobs;
public class AluraJob : JobBase
{
    private INavigator _navigator { get; init; }
    private IProcessManagerService _processManagerService { get; init; }
    public CancellationToken CancellationToken { get; init; }
    protected IDriverFactoryService _driverFactory { get; init; }
    private IConfiguration _configuration { get; init; }
    private IAluraRepository _aluraRepository { get; init; }

    public AluraJob(ILogger<AluraJob> logger,
                            INavigator navigator,
                            IProcessManagerService processManagerService,
                            IDriverFactoryService driverFactory,
                            IConfiguration configuration,
                            IAluraRepository aluraRepository) : base(logger)
    {
        _navigator = navigator;
        _processManagerService = processManagerService;
        CancellationToken = new CancellationTokenSource().Token;
        _driverFactory = driverFactory;
        _configuration = configuration;
        _aluraRepository = aluraRepository;
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Inicializando aplicação...");

            var credential = await _aluraRepository.GetCredential();
            if (credential is null)
            {
                _logger.LogWarning($"Não há usuário disponível no banco de dados para login!");
                return;
            }

            SetupDriver(_configuration["AppSettings:ChromeDownloadsPath"]);

            string url = _configuration["Alura:URLs:Principal"];
            string searchWord = _configuration["Alura:Words:SearchWord"];

            var navigationResult = await _navigator.NavigationAlura(url, searchWord, credential);
            if (!navigationResult.sucess)
                _logger.LogInformation(navigationResult.obs.ToString());
            _driverFactory.Quit();

            _logger.LogInformation("Encerrando aplicação...");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Falha... {ex.Message}");
            throw;
        }
    }
    private void SetupDriver(string? downloadPath = null)
    {
        downloadPath ??= Assembly.GetExecutingAssembly().Location;
        Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

        _logger.LogInformation("configurando chrome download path: {path}", downloadPath);

        var opts = new ChromeOptions();
        opts.AddUserProfilePreference("download.default_directory", downloadPath);

        // Configs baixo para forcar download de PDF imediatamente sem abrir o PDF VIEWER

        // opts.AddUserProfilePreference("download.prompt_for_download", false);
        // opts.AddArguments("kiosk-printing", "--kiosk-printing");

        _driverFactory.StartDriver(opts: opts);
    }
}

