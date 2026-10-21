import { expect, test } from "@playwright/test";

test("classifies readings into every blood pressure category", async ({ page }) => {
  await page.goto(
    "https://bloodpressurewebapp-dshyerhxb9h4bwgk.italynorth-01.azurewebsites.net/",
  );

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
  }
});
