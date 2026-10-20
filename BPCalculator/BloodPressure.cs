using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        // This is the text that displays on the web page.
        [Display(Name= "Low: Consider consulting a healthcare professional if you experience dizziness, fainting, or other symptoms.")] Low,
        [Display(Name= "Ideal: Maintain your healthy habits and continue regular monitoring.")]  Ideal,
        [Display(Name= "Pre-High: Adopt lifestyle changes such as reducing salt intake and increasing physical activity.")] PreHigh,
        [Display(Name = "High: Consult a healthcare professional about managing your blood pressure and reducing health risks.")]  High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }                      // mmHG


        // calculate BP category
        public BPCategory Category
        {
            get
            { 
                if (Systolic >= 140 || Diastolic >= 90)
                {
                    return BPCategory.High;
                }
                else if (Systolic >= 120 || Diastolic >= 80)
                {
                    return BPCategory.PreHigh;
                }
                else if (Systolic < 90 || Diastolic < 60)
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
