using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;

namespace BPCalculator.ReqnrollTests.Steps;

[Binding]
public sealed class BloodPressureCalculatorSteps
{
    private static readonly WebApplicationFactory<Program> Application = new();
    private static readonly Regex CategoryBadge = new(
        "<span\\b(?=[^>]*\\bclass=\"[^\"]*\\bbp-category\\b[^\"]*\")[^>]*>(?<text>.*?)</span>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private HttpClient? _client;
    private string? _page;
    private string? _antiforgeryToken;

    [Given("the calculator page is open")]
    public async Task GivenTheCalculatorPageIsOpen()
    {
        _client = Application.CreateClient();
        using var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        _page = await response.Content.ReadAsStringAsync();
        _antiforgeryToken = ReadAntiforgeryToken(_page);
    }

    [When("I submit a reading with systolic {int} and diastolic {int}")]
    public async Task WhenISubmitAReading(int systolic, int diastolic)
    {
        Assert.IsNotNull(_client);
        Assert.IsNotNull(_antiforgeryToken);

        using var response = await _client.PostAsync(
            "/",
            new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("__RequestVerificationToken", _antiforgeryToken),
                new KeyValuePair<string, string>("BP.Systolic", systolic.ToString()),
                new KeyValuePair<string, string>("BP.Diastolic", diastolic.ToString())
            ]));

        response.EnsureSuccessStatusCode();
        _page = await response.Content.ReadAsStringAsync();
    }

    [Then("the page displays category {string} followed by advice {string}")]
    public void ThenThePageDisplaysCategoryFollowedByAdvice(string expectedCategory, string expectedAdvice)
    {
        Assert.IsNotNull(_page);

        var match = CategoryBadge.Match(_page);
        Assert.IsTrue(match.Success, "Expected the displayed category text to include the advice.");

        var expectedText = NormalizeDisplayedText($"{expectedCategory} {expectedAdvice}");
        Assert.AreEqual(expectedText, NormalizeDisplayedText(match.Groups["text"].Value));
    }

    [Then("the page displays the validation error {string}")]
    public void ThenThePageDisplaysTheValidationError(string expectedError)
    {
        Assert.IsNotNull(_page);
        StringAssert.Contains(_page, expectedError);
    }

    [AfterScenario]
    public void DisposeClient()
    {
        _client?.Dispose();
    }

    [AfterTestRun]
    public static void DisposeApplication()
    {
        Application.Dispose();
    }

    private static string ReadAntiforgeryToken(string html)
    {
        var input = Regex.Match(
            html,
            "<input\\b(?=[^>]*\\bname=\"__RequestVerificationToken\")[^>]*>",
            RegexOptions.IgnoreCase);
        var value = Regex.Match(input.Value, "\\bvalue=\"([^\"]+)\"", RegexOptions.IgnoreCase);

        if (!input.Success || !value.Success)
        {
            throw new InvalidOperationException("The calculator page did not contain an antiforgery token.");
        }

        return WebUtility.HtmlDecode(value.Groups[1].Value);
    }

    private static string NormalizeDisplayedText(string value)
    {
        return WebUtility.HtmlDecode(Regex.Replace(value, @"\s+", " ").Trim());
    }
}
