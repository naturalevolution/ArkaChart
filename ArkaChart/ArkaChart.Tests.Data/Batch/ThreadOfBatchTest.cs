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
    public class ThreadOfBatchTest : BaseRepositoryTest {
        private Mock<PathHelper> PathHelperMock;
        private ThreadOfBatch ThreadOfBatch;
        [SetUp]
        public virtual void SetUp() {
           base.SetUp();
            PathHelperMock = new Mock<PathHelper>();
            ThreadOfBatch = new ThreadOfBatch("otherName");
         }

        [TearDown]
        public virtual void TearDown() {
            base.TearDown();
        }
        /*
        [Test]
        public void BatchFileCanBeInPause() {
            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.Processing);
            Repositories.Files.Add(dataFile);
            Repositories.SaveChanges();

            ThreadOfBatch.RequestPause();;

            var actualFile = Repositories.Files.FindDistinctBy(x => x.Id == dataFile.Id);
            Assert.AreEqual((int)StatusHelper.Waiting, actualFile.Status);
        }

        [Test]
        public void BatchShouldReadAndInsertAllLines() {
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.NeverProcessed);
            Repositories.Files.Add(dataFile);
            Repositories.SaveChanges();

            BatchHelper.StartProcessing(dataFile);

            var actualFile = Repositories.Files.FindDistinctBy(x => x.Id == dataFile.Id);
            Assert.AreEqual("TestFile.txt", actualFile.Name);
            Assert.AreEqual(2, actualFile.Lines.Count);
            var currentLine = actualFile.Lines.ElementAt(0);
            Assert.AreNotEqual(0, currentLine.Id);
            Assert.AreEqual("1229212211641", currentLine.Timestamp);
            Assert.AreEqual("81", currentLine.Value);
            Assert.AreEqual("Italie", currentLine.Origin);
            currentLine = actualFile.Lines.ElementAt(1);
            Assert.AreNotEqual(0, currentLine.Id);
            Assert.AreEqual("1229212214291", currentLine.Timestamp);
            Assert.AreEqual("18", currentLine.Value);
            Assert.AreEqual("Pays-Bas", currentLine.Origin);
            Assert.AreEqual(2, actualFile.LastLine);
            Assert.AreEqual((int)StatusHelper.Finished,actualFile.Status);
        }

        [Test]
        public void BatchShouldReadAndInsertLinesStartingFromTheLastOne() {
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.NeverProcessed);
            Repositories.Files.Add(dataFile);
            Repositories.SaveChanges();
            dataFile.AddLine("1229212211641", "81", "Italie");
            Repositories.FileLines.Add(dataFile.Lines.ElementAt(0));
            Repositories.SaveChanges();
            dataFile.LastLine = 1;
            Repositories.Files.Update(dataFile);
            Repositories.SaveChanges();

            BatchHelper.StartProcessing(dataFile);

            var actualFile = Repositories.Files.FindDistinctBy(x => x.Id == dataFile.Id);
            Assert.AreEqual("TestFile.txt", actualFile.Name);
            Assert.AreEqual(2, actualFile.Lines.Count);
            var currentLine = actualFile.Lines.ElementAt(0);
            Assert.AreNotEqual(0, currentLine.Id);
            Assert.AreEqual("1229212211641", currentLine.Timestamp);
            Assert.AreEqual("81", currentLine.Value);
            Assert.AreEqual("Italie", currentLine.Origin);
            currentLine = actualFile.Lines.ElementAt(1);
            Assert.AreNotEqual(0, currentLine.Id);
            Assert.AreEqual("1229212214291", currentLine.Timestamp);
            Assert.AreEqual("18", currentLine.Value);
            Assert.AreEqual("Pays-Bas", currentLine.Origin);
            Assert.AreEqual(2, actualFile.LastLine);
            Assert.AreEqual((int)StatusHelper.Finished, actualFile.Status);
        }

        [Test]
        public void BatchFileShouldChangeStateIfAnExceptionOccured() {
            var FileRepositoryMock = new Mock<IFileRepository>(MockBehavior.Strict);
            var FileLineRepositoryMock = new Mock<IFileLineRepository>(MockBehavior.Strict);
            var RepositoriesMock = new Mock<IRepositories>(MockBehavior.Strict);
            RepositoriesMock.Setup(x => x.GetFileRepository()).Returns(FileRepositoryMock.Object);
            RepositoriesMock.Setup(x => x.GetFileLineRepository()).Returns(FileLineRepositoryMock.Object);
            Repositories.Load(RepositoriesMock.Object);
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            FileRepositoryMock.Setup(x => x.Update(It.IsAny<DataFile>()));
            FileLineRepositoryMock.Setup(x => x.Add(It.IsAny<DataLine>())).Throws(new Exception());

            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.NeverProcessed);
            dataFile.LastLine = 1;
            BatchHelper.StartProcessing(dataFile);


            FileRepositoryMock.Verify(x => x.Update(It.IsAny<DataFile>()), Times.Exactly(2));
            FileLineRepositoryMock.Verify(x => x.Add(It.IsAny<DataLine>()), Times.Once);
            Assert.AreEqual((int)StatusHelper.Waiting, dataFile.Status);
            InitializeContext();
        }

        [Test]
        public void BatchShouldCallRepositoryTwiceWhenLastLineRequired() {
            var FileRepositoryMock = new Mock<IFileRepository>(MockBehavior.Strict);
            var FileLineRepositoryMock = new Mock<IFileLineRepository>(MockBehavior.Strict);
            var RepositoriesMock = new Mock<IRepositories>(MockBehavior.Strict);
            RepositoriesMock.Setup(x => x.GetFileRepository()).Returns(FileRepositoryMock.Object);
            RepositoriesMock.Setup(x => x.GetFileLineRepository()).Returns(FileLineRepositoryMock.Object);
            Repositories.Load(RepositoriesMock.Object);
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            FileRepositoryMock.Setup(x => x.Update(It.IsAny<DataFile>()));
            FileLineRepositoryMock.Setup(x => x.Add(It.IsAny<DataLine>()));

            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.Waiting);
            dataFile.LastLine = 1;
            BatchHelper.StartProcessing(dataFile);

            FileRepositoryMock.Verify(x => x.Update(It.IsAny<DataFile>()), Times.Exactly(2));
            FileLineRepositoryMock.Verify(x => x.Add(It.IsAny<DataLine>()), Times.Once);
            Assert.AreEqual((int)StatusHelper.Finished, dataFile.Status);
            InitializeContext();
        }

        [Test]
        public void BatchShouldCallRepositoryWhenLastLineIsNotRequired() {
            var FileRepositoryMock = new Mock<IFileRepository>(MockBehavior.Strict);
            var FileLineRepositoryMock = new Mock<IFileLineRepository>(MockBehavior.Strict);
            var RepositoriesMock = new Mock<IRepositories>(MockBehavior.Strict);
            RepositoriesMock.Setup(x => x.GetFileRepository()).Returns(FileRepositoryMock.Object);
            RepositoriesMock.Setup(x => x.GetFileLineRepository()).Returns(FileLineRepositoryMock.Object);
            Repositories.Load(RepositoriesMock.Object);
            PathHelperMock.Setup(x => x.AbsolutePath(It.IsAny<string>()))
                .Returns(@"G:\Workspace\ArkaChart\ArkaChart\ArkaChart.Tests.Data\FakeDatas\TestFile.txt");
            FileRepositoryMock.Setup(x => x.Update(It.IsAny<DataFile>()));
            FileLineRepositoryMock.Setup(x => x.Add(It.IsAny<DataLine>()));

            var dataFile = new DataFile("TestFile.txt", (int)StatusHelper.NeverProcessed);
            BatchHelper.StartProcessing(dataFile);

            FileRepositoryMock.Verify(x => x.Update(It.IsAny<DataFile>()), Times.Exactly(2));
            FileLineRepositoryMock.Verify(x => x.Add(It.IsAny<DataLine>()), Times.Exactly(2));
            Assert.AreEqual((int)StatusHelper.Finished, dataFile.Status);
            InitializeContext();
        }*/
    }
}
