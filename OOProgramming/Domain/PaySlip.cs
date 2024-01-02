using Microsoft.VisualBasic.ApplicationServices;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OOProgramming.Domain
{
    /// <summary>
    /// Class a capture details accociated with an employee's pay slip record
    /// A pay slip has 1 employee, and stores the details of the payment.
    /// This includes the gross pay, super amount, net pay, hours, tax, and net pay.
    /// </summary>
    public class PaySlip
    {
        /// <summary>
        /// The employee's gross pay per week for the current pay slip
        /// </summary>
        public decimal GrossPay { get; private set; }
        /// <summary>
        /// The employee's superannuation per week for the current pay slip
        /// </summary>
        public decimal Super { get; private set; }
        /// <summary>
        /// The employee's hours worked per week for the current pay slip
        /// </summary>
        public int Hours { get; private set; }
        /// <summary>
        /// The employee's tax amount per week for the current pay slip
        /// </summary>
        public decimal Tax { get; private set; }
        /// <summary>
        /// The employee's net pay per week for the current pay slip
        /// </summary>
        public decimal Net { get; private set; }
        /// <summary>
        /// The employee that the payslip is linked to
        /// </summary>
        public Employee Employee { get; private set; }
        /// <summary>
        /// Creates a pay slip for an employee
        /// </summary>
        /// <param name="gross">Employee's gross pay per week</param>
        /// <param name="super">Employee's superannuation per week</param>
        /// <param name="tax">Employee's tax amount per week</param>
        /// <param name="hours">Employee's hours worked per week</param>
        /// <param name="net">Employee's net pay per week</param>
        /// <param name="employee">Employee that the payslip is linked to</param>
        public PaySlip(decimal gross, decimal super, decimal tax, int hours,decimal net, Employee employee)
        {
            GrossPay = gross;
            Super = super;
            Tax = tax;
            Hours = hours;
            Net = net;
            Employee = employee;
        }
        /// <summary>
        /// Return string of pay slip details
        /// </summary>
        /// <returns>Pay slip details</returns>
        public override string ToString()
        {
            return string.Format("Id: {1}{0}" +
                    "Name: {2} {3}{0}" +
                    "Hours worked: {4}{0}" +
                    "Hourly rate: {5:C2}{0}" +
                    "Tax Threshold: {6}{0}" +
                    "Gross pay: {7:C2}{0}" +
                    "Super: {8:C2}{0}" +
                    "Tax: {9:C2}{0}" +
                    "Net pay: {10:C2}"
                    , Environment.NewLine, Employee.Id, Employee.FirstName, Employee.LastName, Hours, Employee.PayRate,
                    Employee.TaxThresholdToString(), GrossPay, Super, Tax, Net);
        }


    }
}
