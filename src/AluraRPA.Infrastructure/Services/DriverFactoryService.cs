namespace AluraRPA.Infrastructure.Services;
public class DriverFactoryService : IDriverFactoryService
{
    public IWebDriver? Instance => _driver;
    private static IWebDriver? _driver { get; set; }
    public DriverFactoryService() { }

    public IWebDriver StartDriver(string driverType = "chrome", DriverOptions? opts = null)
    {
        if (_driver is null)
            _driver = Instantiate(driverType, opts);
        return _driver;
    }

    public void SetInstance(IWebDriver? driver) => _driver = driver;

    private static IWebDriver Instantiate(string driverType, DriverOptions? opts = null)
    {
        return driverType switch
        {
            "IE" => new InternetExplorerDriver(opts as InternetExplorerOptions),
            "chrome" => new ChromeDriver(opts as ChromeOptions),
            _ => throw new ArgumentException($"drvier '{driverType}' is not supported.")
        };
    }

    public void Quit() => _driver?.Quit();
}