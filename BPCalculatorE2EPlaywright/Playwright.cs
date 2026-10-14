namespace BPCalculatorE2EPlaywright
{
    [TestClass]
    public class Playwright : PageTest
    {
        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingToTheIntroPage()
        {
            await Page.GotoAsync("https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("BP Category Calculator"));

            // create a locator
            var submit = Page.Locator("text=Submit");

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(submit).ToHaveAttributeAsync("class", "btn btn-default");

            // Click the get started link.
            await submit.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex("bloodpressure"));
        }
    }
}
