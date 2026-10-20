using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MyTests;

[TestClass]
public class HomePageTests
{
    [TestMethod]
    public void HomePage_HasExpectedTitle()
    {
        using IWebDriver driver = new ChromeDriver();

        driver.Navigate().GoToUrl("https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/");

        Assert.AreEqual(
            "BP Category Calculator",
            driver.Title);
    }
}