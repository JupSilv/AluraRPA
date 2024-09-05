namespace AluraRPA.Domain.Interfaces;
public interface IDriverFactoryService
{
    abstract IWebDriver Instance { get; }
    void SetInstance(IWebDriver? driver);
    IWebDriver StartDriver(string driverType = "chrome", DriverOptions? opts = null);
    void Quit();
}