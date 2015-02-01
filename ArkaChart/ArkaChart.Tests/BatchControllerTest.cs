using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using ArkaChart.Controllers;
using ArkaChart.Domain.Batch;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Models;
using ArkaChart.Resources;
using ArkaChart.Tools;
using Moq;
using NUnit.Framework;

namespace ArkaChart.Tests {
    [TestFixture]
    public class BatchControllerTest : BaseControllerTest {
        [SetUp]
        protected override void SetUp() {
            base.SetUp();
            BatchController = new BatchController();
            PathHelperTest = new Mock<PathHelper>(MockBehavior.Strict);
            BatchController.PathHelper = PathHelperTest.Object;
        }
        [TearDown]
        public void TearDown() {
            base.TearDown();
            PathHelperTest.VerifyAll();
        }
        private Mock<ThreadOfBatch> ThreadOfBatchMock;
        private BatchController BatchController;
        private Mock<PathHelper> PathHelperTest;
        private string FakePath;
        [Test]
        public void ContinueShouldRedirectWhenDataileNotFound() {
            var dataFile = new DataFile("name", (int) StatusHelper.Processing);
            dataFile.Id = 2;
            FileRepositoryMock.Setup(x => x.FindDistinctBy(It.IsAny<Expression<Func<DataFile, bool>>>()))
                .Returns(It.IsAny<DataFile>);

            ActionResult actual = BatchController.Continue(dataFile.Id);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
        [Test]
        public void ContinueShouldRedirectWhenIdIsInvalid() {
            ActionResult actual = BatchController.Continue(0);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void IndexShouldRenderAllAvailableFilesWithEmptyProcessingFiles() {
            var files = new List<FileInfo> {
                new FileInfo("path1"),
                new FileInfo("path2"),
                new FileInfo("path3")
            };
            PathHelperTest.Setup(x => x.IsExistReceivingDirectory()).Returns(true);
            PathHelperTest.Setup(x => x.AvalaibleFiles()).Returns(files);
            FileRepositoryMock.Setup(x => x.FindAll()).Returns(new List<DataFile>());

            ActionResult actual = BatchController.Index();

            Assert.IsInstanceOf(typeof (ViewResult), actual);
            var result = actual as ViewResult;
            var resultModel = result.Model as FilesViewModel;
            Assert.AreEqual(3, resultModel.Files.Count);
            DataFileViewModel dataModel = resultModel.Files.ElementAt(0);
            Assert.AreEqual("path1", dataModel.Name);
            Assert.AreEqual((int) StatusHelper.NeverProcessed, dataModel.Status);
            dataModel = resultModel.Files.ElementAt(1);
            Assert.AreEqual("path2", dataModel.Name);
            Assert.AreEqual((int) StatusHelper.NeverProcessed, dataModel.Status);
            dataModel = resultModel.Files.ElementAt(2);
            Assert.AreEqual("path3", dataModel.Name);
            Assert.AreEqual((int) StatusHelper.NeverProcessed, dataModel.Status);
        }

        [Test]
        public void IndexShouldRenderAllAvailableFilesWithProcessed() {
            var files = new List<FileInfo> {
                new FileInfo("path1"),
                new FileInfo("path2"),
                new FileInfo("path3")
            };
            var dataFile = new DataFile(files.ElementAt(1).Name, (int)StatusHelper.Processing);
            var dataFiles = new List<DataFile> { dataFile };
            BatchController.PathHelper = PathHelperTest.Object;
            PathHelperTest.Setup(x => x.IsExistReceivingDirectory()).Returns(true);
            PathHelperTest.Setup(x => x.AvalaibleFiles()).Returns(files);
            FileRepositoryMock.Setup(x => x.FindAll()).Returns(dataFiles);

            ActionResult actual = BatchController.Index();

            Assert.IsInstanceOf(typeof(ViewResult), actual);
            var result = actual as ViewResult;
            var resultModel = result.Model as FilesViewModel;
            Assert.AreEqual(3, resultModel.Files.Count);
            Assert.AreEqual("path1", resultModel.Files.ElementAt(0).Name);
            Assert.AreEqual("path3", resultModel.Files.ElementAt(2).Name);
            DataFileViewModel testedFile = resultModel.Files.ElementAt(1);
            Assert.AreEqual("path2", testedFile.Name);
            Assert.AreEqual((int)StatusHelper.Processing, testedFile.Status);
        }

        [Test]
        public void IndexShouldRedirectWhenErrorOnDirectory() {
           
            BatchController.PathHelper = PathHelperTest.Object;
            PathHelperTest.Setup(x => x.IsExistReceivingDirectory()).Returns(false);

            ActionResult actual = BatchController.Index();

            Assert.IsInstanceOf(typeof(ViewResult), actual);
            var result = actual as ViewResult;
            Assert.IsNull(result.Model);
            Assert.IsNotNull(result.TempData["Message"]);
        }

        [Test]
        public void LaunchShouldRedirectWhenFilePathEmpty() {
            string filePath = "";

            ActionResult actual = BatchController.Launch(filePath);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void LaunchShouldRedirectWhenFilePathNotExist() {
            string filePath = "notExistPath";
            PathHelperTest.Setup(x => x.IsExist(filePath)).Returns(false);

            ActionResult actual = BatchController.Launch(filePath);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void PauseShouldPutProcessInPause() {
            var dataFile = new DataFile("name", (int) StatusHelper.Processing);
            dataFile.Id = 2;
            FileRepositoryMock.Setup(x => x.FindDistinctBy(It.IsAny<Expression<Func<DataFile, bool>>>()))
                .Returns(dataFile);

            ActionResult actual = BatchController.Pause(2);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void PauseShouldRedirectWhenDataileNotFound() {
            var dataFile = new DataFile("name", (int) StatusHelper.Processing);
            dataFile.Id = 2;
            FileRepositoryMock.Setup(x => x.FindDistinctBy(It.IsAny<Expression<Func<DataFile, bool>>>()))
                .Returns(It.IsAny<DataFile>);

            ActionResult actual = BatchController.Pause(dataFile.Id);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
        [Test]
        public void PauseShouldRedirectWhenIdIsInvalid() {
            ActionResult actual = BatchController.Pause(0);

            Assert.IsInstanceOf(typeof (RedirectToRouteResult), actual);
            var result = actual as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}