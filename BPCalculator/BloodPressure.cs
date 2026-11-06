using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    /// <summary>
    /// Represents the category assigned to a blood pressure reading.
    /// </summary>
    public enum BPCategory
    {
        /// <summary>Indicates a low blood pressure reading.</summary>
        [Display(Name = "Low: Consider consulting a healthcare professional if you experience dizziness, fainting, or other symptoms.")]
        Low,

        /// <summary>Indicates an ideal blood pressure reading.</summary>
        [Display(Name = "Ideal: Maintain your healthy habits and continue regular monitoring.")]
        Ideal,

        /// <summary>Indicates a pre-high blood pressure reading.</summary>
        [Display(Name = "Pre-High: Adopt lifestyle changes such as reducing salt intake and increasing physical activity.")]
        PreHigh,

        /// <summary>Indicates a high blood pressure reading.</summary>
        [Display(Name = "High: Consult a healthcare professional about managing your blood pressure and reducing health risks.")]
        High
    }

    /// <summary>
    /// Calculates a blood pressure score.
    /// </summary>
    public class BloodPressure
    {
        /// <summary>The minimum valid systolic blood pressure, in mmHg.</summary>
        public const int SystolicMin = 70;

        /// <summary>The maximum valid systolic blood pressure, in mmHg.</summary>
        public const int SystolicMax = 190;

        /// <summary>The minimum valid diastolic blood pressure, in mmHg.</summary>
        public const int DiastolicMin = 40;

        /// <summary>The maximum valid diastolic blood pressure, in mmHg.</summary>
        public const int DiastolicMax = 100;

        /// <summary>Gets or sets the systolic blood pressure, in mmHg.</summary>
        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }

        /// <summary>Gets or sets the diastolic blood pressure, in mmHg.</summary>
        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }

        /// <summary>
        /// Gets the category calculated from the systolic and diastolic values.
        /// </summary>
        public BPCategory Category
        {
            get
            {
                if (this.Systolic >= 140 || this.Diastolic >= 90)
                {
                    return BPCategory.High;
                }
                else if (this.Systolic >= 120 || this.Diastolic >= 80)
                {
                    return BPCategory.PreHigh;
                }
                else if (this.Systolic < 90 || this.Diastolic < 60)
                {
                    return BPCategory.Low;
                }
                else
                {
                    return BPCategory.Ideal;
                }
            }
        }
    }
}
