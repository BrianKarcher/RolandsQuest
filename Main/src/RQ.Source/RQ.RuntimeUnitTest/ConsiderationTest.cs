using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UtilityManager;

namespace RQ.RuntimeUnitTest
{
    public class ConsiderationTest
    {
        [Test]
        public void ConsiderationMyHealthTest()
        {
            var consideration = new Consideration();
            consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDecisionContext> mockDC = new Mock<IDecisionContext>();
            Mock<IAICharacter> mockEntity = new Mock<IAICharacter>();
            mockEntity.Setup(x => x.GetHealthCurrent()).Returns(5);
            mockEntity.Setup(x => x.GetHealthMax()).Returns(10);
            mockDC.SetupGet(x => x.Self).Returns(mockEntity.Object);

            var value = consideration.Score(mockDC.Object, null);

            Assert.That(value, Is.EqualTo(0.5f).Within(.0000001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void ConsiderationAllyHealthTest()
        {
            var consideration = new Consideration();
            consideration.Input = Consideration.InputEnum.AllyHealth;

            Mock<IDecisionContext> mockDC = new Mock<IDecisionContext>();
            Mock<IAICharacter> mockEntity = new Mock<IAICharacter>();
            mockEntity.Setup(x => x.GetHealthCurrent()).Returns(5);
            mockEntity.Setup(x => x.GetHealthMax()).Returns(10);
            mockDC.SetupGet(x => x.AllyEntity).Returns(mockEntity.Object);

            var value = consideration.Score(mockDC.Object, null);

            Assert.That(value, Is.EqualTo(0.5f).Within(.0000001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void ConsiderationDistanceToTargetTest()
        {
            var consideration = new Consideration();
            consideration.Input = Consideration.InputEnum.DistanceToTarget;
            consideration.Parameters = new List<Parameter>() {
                 new Parameter() {Key = "rangemin", Value = "0"},
                 new Parameter() {Key = "rangemax", Value = "5"}
            };

            Mock<IDecisionContext> mockDC = new Mock<IDecisionContext>();
            Mock<IAICharacter> mockEntity = new Mock<IAICharacter>();
            mockEntity.Setup(x => x.GetPos()).Returns(new Vector2(1, 1));
            mockDC.SetupGet(x => x.Self).Returns(mockEntity.Object);
            Mock<IAICharacter> mockTarget = new Mock<IAICharacter>();
            mockTarget.Setup(x => x.GetPos()).Returns(new Vector2(1.5f, 1.5f));
            mockDC.SetupGet(x => x.EnemyEntity).Returns(mockTarget.Object);

            var value = consideration.Score(mockDC.Object, consideration.Parameters);

            Assert.That(value, Is.EqualTo(0.14142f).Within(.00001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void ConsiderationDistanceToTargetTest2()
        {
            var consideration = new Consideration();
            consideration.Input = Consideration.InputEnum.DistanceToTarget;
            consideration.Parameters = new List<Parameter>() {
                 new Parameter() {Key = "rangemin", Value = "5"},
                 new Parameter() {Key = "rangemax", Value = "0"}
            };

            Mock<IDecisionContext> mockDC = new Mock<IDecisionContext>();
            Mock<IAICharacter> mockEntity = new Mock<IAICharacter>();
            mockEntity.Setup(x => x.GetPos()).Returns(new Vector2(1, 1));
            mockDC.SetupGet(x => x.Self).Returns(mockEntity.Object);
            Mock<IAICharacter> mockTarget = new Mock<IAICharacter>();
            mockTarget.Setup(x => x.GetPos()).Returns(new Vector2(1.5f, 1.5f));
            mockDC.SetupGet(x => x.EnemyEntity).Returns(mockTarget.Object);

            var value = consideration.Score(mockDC.Object, consideration.Parameters);

            Assert.That(value, Is.EqualTo(0.14142f).Within(.00001f));
            //Assert.AreEqual(0.36f, value);
        }
    }
}
