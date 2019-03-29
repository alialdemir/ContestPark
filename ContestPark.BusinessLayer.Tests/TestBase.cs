using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContestPark.BusinessLayer.Tests
{
    [TestCategory("UnitTests")]
    public class TestBase
    {
        // Should_BeklenenDavranış_When_Koşul
        //  When_Koşul_Expect_Beklenen davranı
        // Given_ÖnHazırlık_When_Koşul_Then_BeklenenDavranış
        // MethodAdı_Senaryo_Sonuc
        // MethodAdı_sonuç_senarya
        public string TestImageJpg
        {
            get
            {
                return @"F:\ContestPark\ContestPark.BusinessLayer.Tests\App_Data\test.jpg";
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        //[TestMethod]
        //public void x()
        //{
        //// Arrange

        //// Act

        //// Assert
        //}
    }
}