using System;
using System.Runtime.InteropServices;

namespace OOProgramming.Domain
{
    /// <summary>
    /// Used to calculate and generate the employee's pay roll
    /// </summary>
    public class PayCalculator
    {
        private static decimal _superPercent = 0.105m;

        private decimal[] _WeeklyPay { get; set; }
        private decimal[] _TaxA { get; set; }
        private decimal[] _TaxB { get; set; }
        /// <summary>
        /// Creates a Pay Calculator
        /// </summary>
        /// <param name="weeklyPay">Range of weekly pay that calculates tax</param>
        /// <param name="taxA">Range of tax coefficent A</param>
        /// <param name="taxB">Range of tax coefficent B</param>
        public PayCalculator(decimal[] weeklyPay, decimal[] taxA, decimal[] taxB)
        {
            _WeeklyPay = weeklyPay;
            _TaxA = taxA;
            _TaxB = taxB;
        }

        // Calculates the Superannuation per week by the gross pay.
        private static decimal CalculateSuper(decimal grossAmount)
        {
            return _superPercent * grossAmount;
        }

        // Calculates the Gross pay per week by the hour pay rate and hours per week
        private static decimal CalculateGross(decimal payRate, int hours)
        {
            return payRate * hours;
        }
        // Calculate Tax uses the formula  y=ax=b; where y is the tax amount per week,
        // x is the number of whole dollars in weekly earnings plus 99 cents
        // a and b are the coefficents for each set of formulas for each range of weekly earnings 
        private decimal CalculateTax(decimal grossAmount)
        {
            decimal tax = 0;
            for (int i = 0; i < _WeeklyPay.Length; i++)
            {
                if (grossAmount <= _WeeklyPay[i])
                {
                    tax = (grossAmount + 0.99m) * _TaxA[i] - _TaxB[i];
                    break;
                }
            }
            return Math.Round(tax);
        }

        private static decimal CalculateNet(decimal grossAmount, decimal tax)
        {
            return grossAmount - tax;
        }


        /// <summary>
        /// Generates an employee's pay slip
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <param name="hours">The employee's hours worked per week</param>
        /// <param name="withThreshold">The Pay Calculator that used to calculate tax when withholding tax</param>
        /// <param name="noThreshold">The Pay Calculator that used to calculate tax when not withholding the tax</param>
        /// <returns>The employee's pay slip</returns>
        public static PaySlip GeneratePaySlip(Employee employee,int hours,PayCalculator withThreshold,PayCalculator noThreshold)
        {
            decimal gross = CalculateGross(employee.PayRate,hours);
            decimal super = CalculateSuper(gross);
            decimal tax = employee.TaxThreshold? withThreshold.CalculateTax(gross): noThreshold.CalculateTax(gross);
            decimal net = CalculateNet(gross, tax);
            return new PaySlip(gross, super, tax, hours, net, employee);
        }
    }
}
