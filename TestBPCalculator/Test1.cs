using BPCalculator;

namespace TestBPCalculator
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        [DataRow(140, 85, BPCategory.High)]
        [DataRow(130, 90, BPCategory.High)]
        [DataRow(-1, 90, BPCategory.High)]
        public void TestBPHighReturned(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.AreEqual(expected, bp.Category);
        }
    }
}
