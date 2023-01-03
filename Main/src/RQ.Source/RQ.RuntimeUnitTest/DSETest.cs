using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UtilityManager;

namespace RQ.RuntimeUnitTest
{
    public class DSETest
    {
        [Test]
        public void ConsiderationMyHealthTest()
        {
            var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDecisionContext> mockDC = new Mock<IDecisionContext>();
            Mock<IConsideration> mockConsideration1 = new Mock<IConsideration>();
            mockConsideration1.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            mockConsideration1.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            Mock<IConsideration> mockConsideration2 = new Mock<IConsideration>();
            mockConsideration2.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            mockConsideration2.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            dse.Considerations = new List<IConsideration>()
            {
                mockConsideration1.Object,
                mockConsideration2.Object
            };
            var value = dse.Score(mockDC.Object, 1, 0f);

            Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }
    }
}
