namespace AluraRPA.Application.Selenium.Extensions;
public static class SeleniumExtensions
{
    public static WebDriverWait WaitTime(this IWebDriver driver, int seconds = 10)
        => new(driver, FromSeconds(seconds));
    public static IWebElement WaitElement(this IWebDriver driver, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, FromSeconds(seconds));
            var element = wait.Until(d => d.FindElement(by));
            return element;
        }
        catch
        {
            return null;
        }
    }

    public static IWebElement WaitElement(this IWebElement webElement, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(webElement.GetWebDriver(), FromSeconds(seconds));
            var element = wait.Until(d => d.FindElement(by));
            return element;
        }
        catch
        {
            return null;
        }
    }

    public static IEnumerable<IWebElement> WaitElements(this IWebDriver driver, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, FromSeconds(seconds));
            var elements = wait.Until(d => d.FindElements(by));
            return elements;
        }
        catch
        {
            return default;
        }
    }

    public static IEnumerable<IWebElement> WaitElements(this IWebElement webElement, By by, int seconds = 10)
    {
        try
        {
            var wait = new WebDriverWait(webElement.GetWebDriver(), FromSeconds(seconds));
            var elements = wait.Until(d => d.FindElements(by));
            return elements;
        }
        catch
        {
            return default;
        }
    }

    private static IWebDriver GetWebDriver(this IWebElement webElement) => (webElement as IWrapsDriver).WrappedDriver;

    public static string HasAlert(IWebDriver driver)
    {
        try
        {
            var alert = driver.SwitchTo().Alert().Text;
            driver.SwitchTo().Alert().Accept();

            return alert;
        }
        catch
        {
            System.Console.WriteLine("Validacao de Popup: Não encontrado.");
        }
        return null;
    }

    public static string ValidatePopup(IWebDriver driver)
    {
        try
        {
            driver.SwitchTo().DefaultContent();
            var element = driver.WaitElement(By.Id("divAviso"), seconds: 1);

            if (element?.Displayed is not null)
                return element?.Text;
        }
        catch
        {
            System.Console.WriteLine("Validacao de Popup: Não encontrado.");
        }
        return null;
    }

    public static string GetInnerText(this IWebDriver driver, string selector, int waitSeconds = 5)
    {
        var element = driver.WaitElement(By.CssSelector(selector), waitSeconds);
        if (element is null) return null;
        var text = (string)(driver as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].innerText", element);
        return text?.Trim();
    }

    public static string GetTextValue(this IWebDriver driver, string selector)
    {
        var element = driver.WaitElement(By.CssSelector(selector), 5);
        if (element is null) return null;
        var text = (string)(driver as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].value", element);
        return text?.Trim();
    }

    public static string GetInnerText(this IWebElement element, string selector, int waitSeconds = 3)
    {
        var targetElement = element.WaitElement(By.CssSelector(selector), waitSeconds);
        if (element is null) return null;
        var text = (string)(element.GetWebDriver() as IJavaScriptExecutor)?.ExecuteScript("return arguments[0].innerText", targetElement);
        return text?.Trim();
    }

    public static void SetTextValue(this IWebDriver driver, string selector, string value)
    {
        var element = driver.WaitElement(By.CssSelector(selector), 5);
        (driver as IJavaScriptExecutor)?.ExecuteScript($"arguments[0].value='{value}'", element);
    }
}