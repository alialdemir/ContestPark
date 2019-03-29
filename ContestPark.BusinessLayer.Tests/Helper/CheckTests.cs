using ContestPark.Entities.ExceptionHandling;
using ContestPark.Entities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContestPark.BusinessLayer.Tests.Helper
{
    [TestClass]
    public class CheckTests
    {
        /// <summary>
        /// entity null giderde exception fırlatmalı
        /// </summary>
        [TestMethod]
        public void IsNull_Should_Exception_When_Object_Null()
        {
            SampleClass sampleClass = null;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => Check.IsNull(sampleClass, nameof(sampleClass)));
        }

        /// <summary>
        /// id null giderde exception fırlatmalı
        /// </summary>
        [TestMethod]
        public void IsNullOrEmpty_Should_Exception_When_id_Null()
        {
            string sampleString = String.Empty;
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => Check.IsNullOrEmpty(sampleString, nameof(sampleString)));
        }

        /// <summary>
        /// id 0 giderde exception fırlatmalı
        /// </summary>
        [TestMethod]
        public void IsLessThanZero_Should_Exception_When_id_Null()
        {
            int sampeInt = 0;
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => Check.IsLessThanZero(sampeInt, nameof(sampeInt)));
        }

        /// <summary>
        /// Parametreden verilen mesaj ile birlikte NotifcationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedExceptionCustom(typeof(NotificationException), "{\"Message\":\"sample-error-message\"}")]
        public void BadStatus_Should_Exception_When_ExceptionMessage()
        {
            string sampleErrorMessage = "sample-error-message";
            // Assert
            Check.BadStatus(sampleErrorMessage, nameof(sampleErrorMessage));
        }
    }

    public class SampleClass
    {
    }
}