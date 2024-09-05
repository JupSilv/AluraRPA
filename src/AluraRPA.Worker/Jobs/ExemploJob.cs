namespace AluraRPA.Worker.Jobs;
public class ExemploJob : JobBase
{
    private INavigator _navigator { get; init; }
    private IProcessManagerService _processManagerService { get; init; }
    public CancellationToken CancellationToken { get; init; }
    protected IDriverFactoryService _driverFactory { get; init; }
    private IConfiguration _configuration { get; init; }
    private IAluraRepository _exemploRepository { get; init; }

    public ExemploJob(ILogger<ExemploJob> logger,
                            INavigator navigator,
                            IProcessManagerService processManagerService,
                            IDriverFactoryService driverFactory,
                            IConfiguration configuration,
                            IAluraRepository exemploRepository) : base(logger)
    {
        _navigator = navigator;
        _processManagerService = processManagerService;
        CancellationToken = new CancellationTokenSource().Token;
        _driverFactory = driverFactory;
        _configuration = configuration;
        _exemploRepository = exemploRepository;
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        try
        {

            _logger.LogInformation("Capturando fila para atuação");
            var demanda = await _exemploRepository.GetDemandaAsync(CancellationToken);

            if (demanda is null)
            {
                _logger.LogInformation($"Não foram encontradas demandas.");
                await Task.Delay(TimeSpan.FromSeconds(int.Parse(_configuration["AppSettings:ExecutionDelaySeconds"])));
                return;
            }

            var credential = await _exemploRepository.GetCredentialAsync(Guid.Empty, CancellationToken);
            _logger.LogInformation("Usuário recuperado para login: {user}", credential.User);

            SetupDriver(_configuration["AppSettings:ChromeDownloadsPath"]);

            var navigationResult = await _navigator.StartNavigationAsync(credential);

            if (navigationResult.Obs is "Falha no login")
            {
                _logger.LogWarning("Não foi possivel efetuar login no site.\nAguardando {delayTime} minutos para reestabelecimento do site!", _configuration["AppSettings:FalhaLoginDelayMinutes"]);
                await Task.Delay(TimeSpan.FromMinutes(int.Parse(_configuration["AppSettings:FalhaLoginDelayMinutes"])));
                return;
            }

            if (navigationResult.Success is false)
            {
                _logger.LogWarning($"Não foi possível realizar a Navegacao.");
                return;
            }
            await _exemploRepository.UpdateDemandaAsync(demanda.Id, CancellationToken);

            _logger.LogInformation("Processamento da demanda '{id}' foi concluido com sucesso.", demanda.Id);
        }
        catch (WebDriverException ex)
        {
            if (ex.Message.Contains("HTTP request to the remote WebDriver server")
                || ex.Message.Contains("Unable to get browser")
                || ex.Message.Contains("chrome not reachable")
                || ex.Message.Contains("does not exist"))
            {
                _driverFactory?.Quit();
                _driverFactory?.SetInstance(null);
                _processManagerService.TaskKillCmd("chromedriver.exe");
                _processManagerService.TaskKillCmd("chrome.exe");
            }
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
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