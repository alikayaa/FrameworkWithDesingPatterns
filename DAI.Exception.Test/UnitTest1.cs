using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DAI.Exception.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ExceptionHandlingFailCallBackIleCalisiyorMu()
        {
            string error = "";

            ExceptionManager.getInstance().TryCatch(() =>
            {
                Islem1.IslemYap();
            }, MethodBase.GetCurrentMethod(), null, 
                i => {
                    error=ExceptionManager.getInstance().HandleTheError(i, "Benim Başlağım 1");
                }, false);
            Assert.AreNotEqual(error, "");

        }

        [TestMethod]
        public void ExceptionHandlingCalisiyorMu()
        {
            string error = "";
            try
            {
                Islem1.IslemYap();
            }
            catch (System.Exception ex)
            {
                error = ExceptionManager.getInstance().HandleTheError(ex, "Benim Başlığım");
            }
            Assert.AreNotEqual(error, "");

        }
    }
}
