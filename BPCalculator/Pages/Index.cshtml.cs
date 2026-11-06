using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    /// <summary>
    /// Handles requests for the blood pressure calculator page.
    /// </summary>
    public class BloodPressureModel : PageModel
    {
        /// <summary>
        /// Gets or sets the blood pressure values submitted by the user.
        /// </summary>
        [BindProperty]// bound on POST
        public BloodPressure BP { get; set; }

        /// <summary>
        /// Initializes the page with default blood pressure values.
        /// </summary>
        public void OnGet()
        {
            this.BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
        }

        // POST, validate
        // create a unit test for this!!!

        /// <summary>
        /// Validates the submitted blood pressure values and returns the page.
        /// </summary>
        /// <returns>The page result with any validation errors.</returns>
        public IActionResult OnPost()
        {
            // extra validation
            if (!(this.BP.Systolic > this.BP.Diastolic))
            {
                this.ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }

            return this.Page();
        }
    }
}