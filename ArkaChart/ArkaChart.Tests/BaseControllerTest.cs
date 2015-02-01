using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace ArkaChart.Tests {
    [TestFixture]
    public class BaseControllerTest {
        protected const string ERROR = "danger";
        protected const string SUCCESS = "success";
        protected Mock<IFileRepository> FileRepositoryMock;
        protected Mock<IFileLineRepository> FileLineRepositoryMock;

        [SetUp]
        protected virtual void SetUp() {
            FileRepositoryMock = new Mock<IFileRepository>(MockBehavior.Strict);
            FileLineRepositoryMock = new Mock<IFileLineRepository>(MockBehavior.Strict);
            var RepositoriesMock = new Mock<IRepositories>(MockBehavior.Strict);
            RepositoriesMock.Setup(x => x.GetFileRepository()).Returns(FileRepositoryMock.Object);
            RepositoriesMock.Setup(x => x.GetFileLineRepository()).Returns(FileLineRepositoryMock.Object);
            Repositories.Load(RepositoriesMock.Object);
        }
        [TearDown]
        public void TearDown() {
            FileRepositoryMock.VerifyAll();
            FileLineRepositoryMock.VerifyAll();
        }
    }
}