using Microsoft.Playwright;
using Xunit;

namespace Tests.UiTests
{
    public class FrontendTests
    {
        [Fact]
        public async Task HomePage_ShouldHaveCorrectTitle()
        {
            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            await page.GotoAsync("https://localhost:7006/");

            var title = await page.TitleAsync();

            Assert.Equal("Strona Główna", title);
        }
    }
}