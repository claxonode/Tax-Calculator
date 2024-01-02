using OOProgramming;
using System;
using OOProgramming.Domain;
using OOProgramming.Persistence;
using System.Security.Cryptography.X509Certificates;
using System.IO;


namespace OOProgramming.Tests
{
    public class CsvHandlerIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            string path = CsvHandler.GetDirectory("testemp.csv");
            List<Employee> employees = new List<Employee>();
        }


        [Test]
        public void ReadEmployeeCsvWithNotExistentFileShouldThrowFileNotFoundException()
        {
            string path = CsvHandler.GetDirectory("e.csv");
            List<Employee> employees = new List<Employee>();
            var ex = Assert.Throws<FileNotFoundException>(() => { CsvHandler.ReadEmployeeCsvFile(path); });
            Assert.AreEqual($"Could not find file '{ex.FileName}'.", ex.Message);
        }

        [Test]
        public void ReadEmployeeCsvWithNotExidastentFileShouldThrowFileNotFoundException()
        {
            string path = CsvHandler.GetDirectory("testemp.csv");
            List<Employee> employees = new List<Employee>();
            var ex = Assert.Throws<FileNotFoundException>(() => { CsvHandler.ReadEmployeeCsvFile(path); });
            Assert.AreEqual($"Could not find file '{ex.FileName}'.", ex.Message);
        }
    }
}
