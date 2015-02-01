using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;
using ArkaChart.Tools;
using NUnit.Framework;

namespace ArkaChart.Tests.Data {
    [TestFixture]
    public class FileRepositoryTest : BaseRepositoryTest {
        [SetUp]
        public override void SetUp() {
            base.SetUp();
        }
        [TearDown]
        public override void TearDown() {
            base.TearDown();
        }
        [Test]
        public void CalculateSumOfAllValuesShouldCalculateValues() {
            DataFile file1 = CreateDataFile("myName_Waiting", StatusHelper.Waiting);
            DataFile file2 = CreateDataFile("myName_Processing", StatusHelper.Processing);
            DataFile file3 = CreateDataFile("myName_Cancelled", StatusHelper.Cancelled);
            file1.AddLine("1229212214291", "10", "Pays-Bas");
            file1.AddLine("1229212214291", "20", "Pays-Bas");
            file2.AddLine("1229212211641", "12", "Italie");
            file2.AddLine("1229212211641", "5", "Italie");
            Repositories.Files.Add(file1);
            Repositories.Files.Add(file2);
            Repositories.Files.Add(file3);
            Repositories.SaveChanges();

            List<SumValueStringBean> result = Repositories.Files.CalculateSumOfAllValues();

            Assert.AreEqual(2, result.Count());
            SumValueStringBean element = result.ElementAt(0);
            Assert.AreEqual("myName_Waiting", element.Source);
            Assert.AreEqual(30, element.Value);
            element = result.ElementAt(1);
            Assert.AreEqual("myName_Processing", element.Source);
            Assert.AreEqual(17, element.Value);
        }

        [Test]
        public void CanAddFile() {
            Repositories.Files.Add(CreateDataFile("name", StatusHelper.Processing));
            Repositories.SaveChanges();

            IList<DataFile> result = Repositories.Files.FindAll();

            Assert.AreEqual(1, result.Count);
            DataFile actual = result.ElementAt(0);
            Assert.AreEqual("name", actual.Name);
            Assert.AreNotEqual(0, actual.Id);
        }

        [Test]
        public void CanUpdateFile() {
            Repositories.Files.Add(CreateDataFile("name", StatusHelper.Processing));
            Repositories.SaveChanges();
            DataFile result = Repositories.Files.FindAll().ElementAt(0);

            result.Name = "toto";
            Repositories.Files.Update(result);
            Repositories.SaveChanges();

            DataFile actual = Repositories.Files.FindAll().ElementAt(0);
            Assert.AreEqual("toto", result.Name);
        }

        [Test]
        public void ProcessingFilesShouldRetreiveFilesRunning() {
            DataFile file1 = CreateDataFile("myName_Waiting", StatusHelper.Waiting);
            DataFile file2 = CreateDataFile("myName_Processing", StatusHelper.Processing);
            DataFile file3 = CreateDataFile("myName_Cancelled", StatusHelper.Cancelled);
            DataFile file4 = CreateDataFile("myName_Finished", StatusHelper.Finished);
            Repositories.Files.Add(file1);
            Repositories.Files.Add(file2);
            Repositories.Files.Add(file3);
            Repositories.Files.Add(file4);
            Repositories.SaveChanges();

            List<DataFile> result = Repositories.Files.ProcessingFiles();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(file1, result.ElementAt(0));
            Assert.AreEqual(file2, result.ElementAt(1));
        }

        [Test]
        public void ProcessingFilesShouldRetreiveFilesRunningDependantOfReceiving() {
            DataFile file1 = CreateDataFile("myName_Waiting", StatusHelper.Waiting);
            DataFile file2 = CreateDataFile("myName_Processing", StatusHelper.Processing);
            DataFile file3 = CreateDataFile("myName_Cancelled", StatusHelper.Cancelled);
            Repositories.Files.Add(file1);
            Repositories.Files.Add(file2);
            Repositories.Files.Add(file3);
            Repositories.SaveChanges();

            var receivedFiles = new List<FileInfo>();
            receivedFiles.Add(new FileInfo(file1.Name));

            List<DataFile> result = Repositories.Files.ProcessingFiles();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(file1, result.ElementAt(0));
            Assert.AreEqual(file2, result.ElementAt(1));
        }

        [Test]
        public void ProcessingFilesShouldReturnEmptyList() {
            List<DataFile> result = Repositories.Files.ProcessingFiles();
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result);
        }
    }
}