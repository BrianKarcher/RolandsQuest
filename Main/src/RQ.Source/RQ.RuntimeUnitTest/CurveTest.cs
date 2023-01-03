using NUnit.Framework;
using System;
using UtilityManager;

namespace RQ.RuntimeUnitTest
{
    public class CurveTest
    {
        [Test]
        public void QuadraticCurveTest()
        {
            Curve curve = new Curve(CurveType.Quadratic,
               -3.5f,
                2,
                1,
                0.5f);

            var value = curve.Score(0.5f);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void QuadraticCurveTest2()
        {
            Curve curve = new Curve(CurveType.Quadratic,
                -1.5f,
                 2,
                 0.9f,
                 0);

            var value = curve.Score(0.6f);
            Assert.That(value, Is.EqualTo(0.36f).Within(.0001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void LogisticCurveTest()
        {
            Curve curve = new Curve(CurveType.Logistic,
                1f,
                0.95f,
                0.05f,
                0.6f);

            var value = curve.Score(0.4f);
            Assert.That(value, Is.EqualTo(0.212047131f).Within(.0000001f));
            //Assert.AreEqual(0.36f, value);
        }
    }
}
