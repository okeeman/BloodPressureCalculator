import { expect, test } from "@playwright/test";

test("classifies readings into every blood pressure category", async ({ page }) => {
  const baseUrl =
    process.env.BASE_URL ??
    "https://bloodpressurewebapp-hnd9b7dafpfeawa7.francecentral-01.azurewebsites.net/";
  await page.goto(baseUrl);

  const readings = [
    { systolic: 80, diastolic: 50, category: "Low" },
    { systolic: 110, diastolic: 70, category: "Ideal" },
    { systolic: 125, diastolic: 75, category: "Pre-High" },
    { systolic: 150, diastolic: 95, category: "High" },
  ];

  for (const reading of readings) {
    await page.getByLabel("Systolic").fill(String(reading.systolic));
    await page.getByLabel("Diastolic").fill(String(reading.diastolic));
    await page.getByRole("button", { name: "Submit" }).click();
    await expect(page.getByRole("heading", { level: 4 })).toContainText(
      `${reading.category}:`,
    );
    const layout = await page.evaluate(() => {
      const advice = document.querySelector(".bp-advice")?.getBoundingClientRect();

      return {
        documentWidth: document.documentElement.scrollWidth,
        viewportWidth: document.documentElement.clientWidth,
        adviceLeft: advice?.left,
        adviceRight: advice?.right,
      };
    });

    expect(layout.documentWidth).toBeLessThanOrEqual(layout.viewportWidth);
    expect(layout.adviceLeft).toBeGreaterThanOrEqual(0);
    expect(layout.adviceRight).toBeLessThanOrEqual(layout.viewportWidth);
  }
});
