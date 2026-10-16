namespace BPCalculatorE2EPlaywright
{
    [TestClass]
    public class Playwright : PageTest
    {
        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingToTheIntroPage()
        {
            var baseUrl = Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL")
                ?? "https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/";
            await Page.GotoAsync(baseUrl);
            await Expect(Page).ToHaveTitleAsync(new Regex("BP Category Calculator"));

            await Page.Locator("#BP_Systolic").FillAsync("140");
            await Page.Locator("#BP_Diastolic").FillAsync("90");
            await Page.Locator("input[type=submit]").ClickAsync();

            await Expect(Page.Locator("body")).ToContainTextAsync("High Blood Pressure");
        }
    }
}
