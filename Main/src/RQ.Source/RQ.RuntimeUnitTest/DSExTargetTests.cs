using Moq;
using NUnit.Framework;
using RQ.Common;
using RQ.Common.Container;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityManager;

namespace RQ.RuntimeUnitTest
{
    public class DSExTargetTests
    {
        [Test]
        public void CreateContextTest()
        {
            Mock<IEntityContainer> mockEntityContainer = new Mock<IEntityContainer>();
            var dsexTarget = new DSExTarget(mockEntityContainer.Object);
            //var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDSE> mockDSE = new Mock<IDSE>();
            Mock<IComponentRepository> mockRepo = new Mock<IComponentRepository>();

            //Mock<IConsideration> mockConsideration1 = new Mock<IConsideration>();
            //mockConsideration1.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration1.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //Mock<IConsideration> mockConsideration2 = new Mock<IConsideration>();
            //mockConsideration2.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration2.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //dse.Considerations = new List<IConsideration>()
            //{
            //    mockConsideration1.Object,
            //    mockConsideration2.Object
            //};
            //var value = dse.Score(mockDC.Object, 1, 0f);

            dsexTarget.CreateContext(mockDSE.Object, mockRepo.Object, null, null, null);
            var context = dsexTarget.GetContext();

            Assert.NotNull(context);
            Assert.NotNull(context.Self);
            Assert.Null(context.AllyEntity);
            Assert.Null(context.EnemyEntity);
            //Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void CreateContextTest2()
        {
            Mock<IEntityContainer> mockEntityContainer = new Mock<IEntityContainer>();
            var dsexTarget = new DSExTarget(mockEntityContainer.Object);
            //var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDSE> mockDSE = new Mock<IDSE>();
            Mock<IComponentRepository> mockRepo = new Mock<IComponentRepository>();

            //Mock<IConsideration> mockConsideration1 = new Mock<IConsideration>();
            //mockConsideration1.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration1.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //Mock<IConsideration> mockConsideration2 = new Mock<IConsideration>();
            //mockConsideration2.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration2.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //dse.Considerations = new List<IConsideration>()
            //{
            //    mockConsideration1.Object,
            //    mockConsideration2.Object
            //};
            //var value = dse.Score(mockDC.Object, 1, 0f);

            var enemyTags = new string[] { "Enemy", "Boss" };

            dsexTarget.CreateContext(mockDSE.Object, mockRepo.Object, null, enemyTags, null);
            var context = dsexTarget.GetContext();

            Assert.NotNull(context);
            Assert.NotNull(context.Self);
            Assert.Null(context.AllyEntity);
            Assert.Null(context.EnemyEntity);
            Assert.NotNull(context.EnemyEntities);
            //Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void CreateContextTest3()
        {
            Mock<IEntityContainer> mockEntityContainer = new Mock<IEntityContainer>();
            var dsexTarget = new DSExTarget(mockEntityContainer.Object);
            //var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDSE> mockDSE = new Mock<IDSE>();
            Mock<IComponentRepository> mockRepo = new Mock<IComponentRepository>();

            //Mock<IConsideration> mockConsideration1 = new Mock<IConsideration>();
            //mockConsideration1.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration1.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //Mock<IConsideration> mockConsideration2 = new Mock<IConsideration>();
            //mockConsideration2.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration2.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //dse.Considerations = new List<IConsideration>()
            //{
            //    mockConsideration1.Object,
            //    mockConsideration2.Object
            //};
            //var value = dse.Score(mockDC.Object, 1, 0f);

            var allyTags = new string[] { "Player" };

            dsexTarget.CreateContext(mockDSE.Object, mockRepo.Object, allyTags, null, null);
            var context = dsexTarget.GetContext();

            Assert.NotNull(context);
            Assert.NotNull(context.Self);
            Assert.NotNull(context.AllyEntity);
            Assert.Null(context.EnemyEntity);
            Assert.Null(context.EnemyEntities);
            //Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }

        [Test]
        public void CreateContextTest4()
        {
            Mock<IEntityContainer> mockEntityContainer = new Mock<IEntityContainer>();
            var dsexTarget = new DSExTarget(mockEntityContainer.Object);
            //var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            Mock<IDSE> mockDSE = new Mock<IDSE>();
            Mock<IComponentRepository> mockRepo = new Mock<IComponentRepository>();
            Mock<IComponentRepository> mockTarget = new Mock<IComponentRepository>();

            //Mock<IConsideration> mockConsideration1 = new Mock<IConsideration>();
            //mockConsideration1.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration1.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //Mock<IConsideration> mockConsideration2 = new Mock<IConsideration>();
            //mockConsideration2.Setup(x => x.Score(It.IsAny<IDecisionContext>(), It.IsAny<List<Parameter>>())).Returns(0.5f);
            //mockConsideration2.Setup(x => x.ComputeResponseCurve(It.IsAny<float>())).Returns(0.9f);
            //dse.Considerations = new List<IConsideration>()
            //{
            //    mockConsideration1.Object,
            //    mockConsideration2.Object
            //};
            //var value = dse.Score(mockDC.Object, 1, 0f);

            //var allyTags = new string[] { "Player" };

            dsexTarget.CreateContext(mockDSE.Object, mockRepo.Object, null, null, mockTarget.Object);
            var context = dsexTarget.GetContext();

            Assert.NotNull(context);
            Assert.NotNull(context.Self);
            Assert.Null(context.AllyEntity);
            Assert.NotNull(context.EnemyEntity);
            Assert.Null(context.EnemyEntities);
            //Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }
    }
}
