using BPCalculator;

namespace TestBPCalculator
{
    [TestClass]
    public sealed class UnitTest
    {
        [TestMethod]
        [DataRow(140, 85, BPCategory.High)]
        [DataRow(130, 90, BPCategory.High)]
        [DataRow(-1, 90, BPCategory.High)] // invalid data accepted here, inputs validated on page.
        public void TestBPHighReturned(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, bp.Category);
        }

        [TestMethod]
        [DataRow(120, 80, BPCategory.PreHigh)]
        [DataRow(139, 89, BPCategory.PreHigh)]
        [DataRow(120, 89, BPCategory.PreHigh)]
        public void TestBPPreHighReturned(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, bp.Category);
        }

        [TestMethod]
        [DataRow(90, 60, BPCategory.Ideal)]
        [DataRow(119, 79, BPCategory.Ideal)]
        [DataRow(119, 60, BPCategory.Ideal)]
        public void TestBPIdealReturned(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, bp.Category);
        }


        [TestMethod]
        [DataRow(89, 59, BPCategory.Low)]
        [DataRow(70, 40, BPCategory.Low)]
        [DataRow(70, 59, BPCategory.Low)]
        public void TestBPLowReturned(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, bp.Category);
        }

        // how to test input validation???

        [TestMethod]
        [DataRow(89, 70, BPCategory.Low)]
        [DataRow(110, 59, BPCategory.Low)]
        [DataRow(119, 80, BPCategory.PreHigh)]
        [DataRow(139, 90, BPCategory.High)]
        public void CategoryUsesEitherSystolicOrDiastolicThreshold(int systolic, int diastolic, BPCategory expected)
        {
            var reading = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, reading.Category);
        }
    }
}
