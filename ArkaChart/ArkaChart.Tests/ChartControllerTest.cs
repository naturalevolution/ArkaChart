using System.Collections.Generic;
using System.Web.Mvc;
using ArkaChart.Controllers;
using ArkaChart.Domain.Mapping.Entities.Beans;
using Moq;
using NUnit.Framework;

namespace ArkaChart.Tests {
    [TestFixture]
    public class ChartControllerTest : BaseControllerTest {
        [SetUp]
        protected override void SetUp() {
            base.SetUp();
            ChartController = new ChartController();
        }
        protected ChartController ChartController;

        [Test]
        public void IndexShouldRenderView() {
            FileRepositoryMock.Setup(x => x.CalculateSumOfAllValues()).Returns(new List<SumValueStringBean>());
            FileLineRepositoryMock.Setup(x => x.CalculateSumByDate()).Returns(new List<SumValueDateBean>());
            FileLineRepositoryMock.Setup(x => x.CalculateSumByOrigin()).Returns(new List<SumValueOriginBean>());
           
            ActionResult result = ChartController.Index();

            Assert.IsInstanceOf(typeof (ViewResult), result);
        }
    }
}