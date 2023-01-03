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
    public class IntelligenceDefTest
    {
        [Test]
        public void IntelligenceDefTest1()
        {
            Mock<IEntityContainer> mockEntityContainer = new Mock<IEntityContainer>();
            Mock<IComponentRepository> mockEntity = new Mock<IComponentRepository>();
            mockEntityContainer.Setup(x => x.GetEntitiesFromTags(It.IsAny<string[]>())).Returns(new IComponentRepository[] { mockEntity.Object, mockEntity.Object });
            Mock<IComponentRepository> mockRepo = new Mock<IComponentRepository>();
            Mock<IDecisionMaker> mockDM = new Mock<IDecisionMaker>();

            Mock<IDSE> mockDSE1 = new Mock<IDSE>();
            mockDSE1.SetupGet(x => x.RunForAllTargets).Returns(true);
            Mock<IDSE> mockDSE2 = new Mock<IDSE>();

            //mockDM.Setup(x => x.GetDSEList()).Returns(new IDSE[] { mockDSE1.Object, mockDSE2.Object });
            mockDM.SetupGet(x => x.DSEList).Returns(new IDSE[] { mockDSE1.Object, mockDSE2.Object });
            var intelligenceDef = new IntelligenceDef();
            intelligenceDef.Init(mockRepo.Object, mockEntityContainer.Object, mockDM.Object);
            var targets = intelligenceDef.GetDsexTargets();
            //var dse = new DSE();
            //consideration.Input = Consideration.InputEnum.MyHealth;

            //Mock<IDSE> mockDSE = new Mock<IDSE>();

            //Mock<IComponentRepository> mockTarget = new Mock<IComponentRepository>();

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

            //dsexTarget.CreateContext(mockDSE.Object, mockRepo.Object, null, null, mockTarget.Object);
            //var context = dsexTarget.GetContext();

            //Assert.True(true);
            Assert.NotNull(targets);
            Assert.AreEqual(3, targets.Count);
            //Assert.NotNull(targets.);
            //Assert.Null(context.AllyEntity);
            //Assert.NotNull(context.EnemyEntity);
            //Assert.Null(context.EnemyEntities);
            //Assert.That(value, Is.EqualTo(0.963f).Within(.001f));
            //Assert.AreEqual(0.36f, value);
        }
    }
}
