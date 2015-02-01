using System;
using System.Collections.Generic;
using System.Linq;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;
using ArkaChart.Tools;
using NUnit.Framework;

namespace ArkaChart.Tests.Data {
    [TestFixture]
    public class FileLineRepositoryTest : BaseRepositoryTest {
        [SetUp]
        public override void SetUp() {
            base.SetUp();
        }
        [TearDown]
        public override void TearDown() {
            base.TearDown();
        }
        [Test]
        public void CalculateSumByDateShouldCalculateValues() {
            DataFile file1 = CreateDataFile("myName_Waiting", StatusHelper.Waiting);
            DataFile file2 = CreateDataFile("myName_Processing", StatusHelper.Processing);
            DataFile file3 = CreateDataFile("myName_Cancelled", StatusHelper.Cancelled);
            file1.AddLine("1229212214291", "10", "Pays-Bas");
            file1.AddLine("1229212214291", "5", "Pays-Bas");
            file2.AddLine("1229212211641", "22", "Italie");
            file2.AddLine("1229212211641", "3", "Italie");
            Repositories.Files.Add(file1);
            Repositories.Files.Add(file2);
            Repositories.Files.Add(file3);
            Repositories.SaveChanges();

            List<SumValueDateBean> result = Repositories.FileLines.CalculateSumByDate();

            Assert.AreEqual(1, result.Count());
            SumValueDateBean element = result.FirstOrDefault();
            Assert.AreEqual(GetDateToShort("1229212214291"), element.GetDate());
            Assert.AreEqual(40, element.Value);
        }

        [Test]
        public void CalculateSumByOriginShouldCalculateValues() {
            DataFile file1 = CreateDataFile("myName_Waiting", StatusHelper.Waiting);
            DataFile file2 = CreateDataFile("myName_Processing", StatusHelper.Processing);
            DataFile file3 = CreateDataFile("myName_Cancelled", StatusHelper.Cancelled);
            file1.AddLine("1229212214291", "10", "Pays-Bas");
            file1.AddLine("1229212214291", "5", "Pays-Bas");
            file2.AddLine("1229212211641", "22", "Italie");
            file2.AddLine("1229212211641", "3", "Italie");
            var timestamp = ConvertToTimestamp(new DateTime(2012, 10, 10)).ToString();
            file2.AddLine(timestamp, "3", "Italie");
            Repositories.Files.Add(file1);
            Repositories.Files.Add(file2);
            Repositories.Files.Add(file3);
            Repositories.SaveChanges();

            List<SumValueOriginBean> result = Repositories.FileLines.CalculateSumByOrigin();

            Assert.AreEqual(3, result.Count());
            var element = result.ElementAt(0);
            Assert.AreEqual("Italie", element.Source);
            var elementDateValue = element.Dates;
            Assert.AreEqual(GetDateToShort("1229212214291"), elementDateValue.GetDate());
            Assert.AreEqual(25, elementDateValue.Value);
            
            element = result.ElementAt(1);
            Assert.AreEqual("Italie", element.Source);
            elementDateValue = element.Dates;
            Assert.AreEqual(GetDateToShort(timestamp), elementDateValue.GetDate());
            Assert.AreEqual(3, elementDateValue.Value);

           element = result.ElementAt(2);
            Assert.AreEqual("Pays-Bas", element.Source);
            elementDateValue = element.Dates;
            Assert.AreEqual(GetDateToShort("1229212214291"), elementDateValue.GetDate());
            Assert.AreEqual(15, elementDateValue.Value);
        }

        private string GetDateToShort(string timestamp) {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return dateTime.AddMilliseconds(long.Parse(timestamp)).ToShortDateString();
        }

        private double ConvertToTimestamp(DateTime value) {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (double)span.TotalSeconds;
        } 
    }
}