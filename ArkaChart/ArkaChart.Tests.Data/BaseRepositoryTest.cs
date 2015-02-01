using System.Data.Entity;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Context;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Tests.Data.Context;
using ArkaChart.Tools;
using NUnit.Framework;

namespace ArkaChart.Tests.Data {
    [TestFixture]
    public class BaseRepositoryTest {
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp() {
            Database.SetInitializer(new DropCreateDatabaseAlways<EntitiesContext>());
        }

        [SetUp]
        public virtual void SetUp() {
            InitializeContext();
            CleanDataBase();
        }

        protected void InitializeContext() {
            Repositories.Load(new Repositories());
            Repositories.LoadContext(new EntityObjectContextTest());
        }
        
        [TearDown]
         public virtual void TearDown() {
             CleanDataBase();
        }

        private void CleanDataBase() {
            foreach (var dataLine in Repositories.FileLines.FindAll()) {
                Repositories.FileLines.Remove(dataLine);
            }
            Repositories.SaveChanges();
            foreach (var dataFile in Repositories.Files.FindAll()) {
                Repositories.Files.Remove(dataFile);
            }
            Repositories.SaveChanges();
        }

        protected DataFile CreateDataFile(string name, StatusHelper status) {
            return new DataFile(name, (int) status);
        }
    }
}