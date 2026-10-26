using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace MyTests;

[TestClass]
public class BloodPressureCalculatorSeleniumTests
{
    private const string DefaultBaseUrl =
        "https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/";

    private IWebDriver? _driver;

    private IWebDriver Driver =>
        _driver ?? throw new InvalidOperationException("The Selenium WebDriver has not been initialized.");

    [TestInitialize]
    public void SetUp()
    {
        _driver = new ChromeDriver();
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? DefaultBaseUrl;
        Driver.Navigate().GoToUrl(baseUrl);
    }

    [TestCleanup]
    public void TearDown()
    {
        _driver?.Dispose();
        _driver = null;
    }

    [TestMethod]
    public void CalculatorPage_DisplaysTitleAndDefaultReadings()
    {
        Assert.AreEqual("BP Category Calculator", Driver.Title);
        Assert.AreEqual("100", Driver.FindElement(By.Id("BP_Systolic")).GetAttribute("value"));
        Assert.AreEqual("60", Driver.FindElement(By.Id("BP_Diastolic")).GetAttribute("value"));
    }

    [TestMethod]
    [DataRow("80", "50", "Low:")]
    [DataRow("110", "70", "Ideal:")]
    [DataRow("125", "75", "Pre-High:")]
    [DataRow("150", "95", "High:")]
    public void SubmittingReading_DisplaysExpectedCategory(
        string systolic,
        string diastolic,
        string expectedCategory)
    {
        var systolicInput = Driver.FindElement(By.Id("BP_Systolic"));
        systolicInput.Clear();
        systolicInput.SendKeys(systolic);

        var diastolicInput = Driver.FindElement(By.Id("BP_Diastolic"));
        diastolicInput.Clear();
        diastolicInput.SendKeys(diastolic);

        Driver.FindElement(By.CssSelector("#form1 input[type='submit']")).Click();

        var badge = new WebDriverWait(Driver, TimeSpan.FromSeconds(10))
            .Until(driver => driver.FindElement(By.CssSelector("h4 .badge")));

        Assert.IsTrue(
            badge.Text.StartsWith(expectedCategory, StringComparison.Ordinal),
            $"Expected category to start with '{expectedCategory}', but got '{badge.Text}'.");
    }

    [TestMethod]
    public void SubmittingSystolicNotGreaterThanDiastolic_DisplaysValidationError()
    {
        var systolicInput = Driver.FindElement(By.Id("BP_Systolic"));
        systolicInput.Clear();
        systolicInput.SendKeys("110");

        var diastolicInput = Driver.FindElement(By.Id("BP_Diastolic"));
        diastolicInput.Clear();
        diastolicInput.SendKeys("110");

        Driver.FindElement(By.CssSelector("#form1 input[type='submit']")).Click();

        var validationSummary = new WebDriverWait(Driver, TimeSpan.FromSeconds(10))
            .Until(driver => driver.FindElement(By.CssSelector(".validation-summary-errors")));

        //StringAssert.Contains(validationSummary.Text, "Systolic must be greater than Diastolic");
        Assert.Contains(validationSummary.Text, "Systolic must be greater than Diastolic");
        Assert.AreEqual(0, Driver.FindElements(By.CssSelector("h4 .badge")).Count);
        //Assert.IsEmpty(0, Driver.FindElements(By.CssSelector("h4 .badge")).Count);
    }
}
