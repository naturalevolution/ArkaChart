using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkaChart.Domain.Batch;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Repositories;
using ArkaChart.Tests.Data.Context;
using ArkaChart.Tools;
using Moq;
using NUnit.Framework;

namespace ArkaChart.Tests.Data.Batch {
    [TestFixture]
    public class BatchHelperTest : BaseRepositoryTest {
       /* private Mock<PathHelper> PathHelperMock;
        private Mock<ThreadOfBatch> ThreadOfBatchMock;
        [SetUp]
        public virtual void SetUp() {
           base.SetUp();
           ThreadOfBatchMock = new Mock<ThreadOfBatch>(MockBehavior.Strict);
           PathHelperMock = new Mock<PathHelper>(MockBehavior.Strict);
         }

        [TearDown]
        public virtual void TearDown() {
            base.TearDown();
            ThreadOfBatchMock.VerifyAll();
        }

        [Test]
        public void PauseProcessingShouldCallThreadPause() {
            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.Processing);
            ThreadOfBatchMock.Setup(x => x.RequestPause());

            BatchHelper.threadOfBatch = ThreadOfBatchMock.Object;
            BatchHelper.PathHelper = PathHelperMock.Object;
            BatchHelper.PauseProcessing(dataFile);
        }

        [Test]
        public void StartProcessingShouldUpdateStatus() {
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.NeverProcessed);
            Repositories.Files.Add(dataFile);
            Repositories.SaveChanges();

            BatchHelper.threadOfBatch = ThreadOfBatchMock.Object;
            BatchHelper.PathHelper = PathHelperMock.Object;
            BatchHelper.StartProcessing(dataFile);

            var actualFile = Repositories.Files.FindDistinctBy(x => x.Id == dataFile.Id);
            Assert.AreEqual("TestFile.txt", actualFile.Name);
            Assert.AreEqual((int)StatusHelper.Processing, actualFile.Status);
        }*/

    }
}
