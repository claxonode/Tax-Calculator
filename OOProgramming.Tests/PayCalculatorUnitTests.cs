using OOProgramming;
using System;
using OOProgramming.Domain;
using OOProgramming.Persistence;
using System.Security.Cryptography.X509Certificates;

namespace OOProgramming.Tests
{
    public class PayCalculatorUnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Theory]
        [TestCase(100,40)]
        [TestCase(500,40)]
        [TestCase(900,40)]
        [TestCase(3000,40)]
        [TestCase(5000,40)]
        [TestCase(100, 20)]
        [TestCase(500, 20)]
        [TestCase(900, 20)]
        [TestCase(3000, 20)]
        [TestCase(5000, 20)]
        public void CalculateGrossPayShouldReturnCorrectValue(decimal payRate,int hours)
        {
            decimal grossPay = payRate * hours;
            decimal actualGrossPay = PayCalculator.CalculateGross(payRate, hours);

            Assert.AreEqual(grossPay, actualGrossPay);
        }

        [Theory]
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(900)]
        [TestCase(3000)]
        [TestCase(5000)]
        public void CalculateSuperShouldReturnCorrectValue(decimal grossPay)
        {
            decimal actual = PayCalculator.CalculateSuper(grossPay);
            decimal expected = 0.105m * grossPay;

            Assert.AreEqual(expected, actual);
        }

        [Theory]
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(900)]
        [TestCase(3000)]
        [TestCase(5000)]
        public void CalculateWithTaxShouldReturnCorrectValue(decimal grossPay) 
        {
            decimal[] weeklyPayWithTax = { 359, 438, 548, 721, 865, 1282, 2307, 3461, 99999999 };
            decimal[] taxAWithTax = { 0, 0.1900m, 0.2900m, 0.2100m, 0.2190m, 0.3477m, 0.3450m, 0.3900m, 0.4700m };
            decimal[] taxBWithTax = { 0, 68.3462m, 112.1942m, 68.3465m, 74.8369m, 186.2119m, 182.7504m, 286.5965m, 563.5196m };
            PayCalculator payCalculator = new PayCalculator(weeklyPayWithTax, taxAWithTax, taxBWithTax);

            decimal tax = 0;
            for (int i = 0; i < weeklyPayWithTax.Length; i++)
            {
                if (grossPay <= weeklyPayWithTax[i])
                {
                    tax = (grossPay + 0.99m) * taxAWithTax[i] - taxBWithTax[i];
                    break;
                }
            }
            tax = Math.Round(tax);
            decimal net = grossPay - tax;


            decimal actualTax = payCalculator.CalculateTax(grossPay);
            decimal actualNet = PayCalculator.CalculateNet(grossPay,actualTax);
            Assert.AreEqual(tax, actualTax);
            Assert.AreEqual(net, actualNet);

        }
        [Theory]
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(900)]
        [TestCase(3000)]
        [TestCase(5000)]
        public void CalculateWithoutTaxAndNetShouldReturnCorrectValue(decimal grossPay)
        {
            decimal[] weeklyPayWithTax = { 88,371,515,932,1957,3111,99999999};
            decimal[] taxAWithTax = { 0, 0.19m,0.2348m,0.219m,0.3477m,0.345m,0.39m,0.47m};
            decimal[] taxBWithTax = { 0, 0.19m,3.9639m,-1.9003m,64.4297m,61.9132m,150.0093m,398.9324m };
            PayCalculator payCalculator = new PayCalculator(weeklyPayWithTax, taxAWithTax, taxBWithTax);

            decimal tax = 0;
            for (int i = 0; i < weeklyPayWithTax.Length; i++)
            {
                if (grossPay <= weeklyPayWithTax[i])
                {
                    tax = (grossPay + 0.99m) * taxAWithTax[i] - taxBWithTax[i];
                    break;
                }
            }
            tax = Math.Round(tax);
            decimal net = grossPay - tax;
            decimal actualTax = payCalculator.CalculateTax(grossPay);
            decimal actualNet = PayCalculator.CalculateNet(grossPay, actualTax);
            Assert.AreEqual(tax, actualTax);
            Assert.AreEqual(net, actualNet);
        }


    }
}