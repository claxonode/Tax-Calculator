using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProgramming.Domain
{
    /// <summary>
    /// Represents an employee
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Stores the employee's id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Stores the employee's first name.
        /// </summary>
        public string FirstName { get; private set; }
        /// <summary>
        /// Stores the employee's last name.
        /// </summary>
        public string LastName { get; private set; }
        /// <summary>
        /// Stores the employee's pay rate
        /// </summary>
        public decimal PayRate { get; private set; }
        /// <summary>
        /// Store a boolean that determines if the employee want to withhold tax or not, True if they do,
        /// false if they don't.
        /// </summary>
        public bool TaxThreshold { get; private set; }
        /// <summary>
        /// Creates a employee
        /// </summary>
        /// <param name="id">The employee's id</param>
        /// <param name="firstName">The employee's first name</param>
        /// <param name="lastName">The employee's last name</param>
        /// <param name="payRate">The employee's pay rate</param>
        /// <param name="taxThreshold">The employee want to withhold tax or not for their payroll</param>
        public Employee(int id, string firstName, string lastName, decimal payRate, string taxThreshold)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PayRate = payRate;
            if (taxThreshold.ToLower()[0] == 'y')
            {
                TaxThreshold = true;
            }
            else if (taxThreshold.ToLower()[0] == 'n')
            {
                TaxThreshold = false;
            }
        }
        /// <summary>
        /// Returns employee details to a string.
        /// </summary>
        public override string ToString()
        {
            return "ID:" + Id + " " + FirstName + " " + LastName;
        }
        /// <summary>
        /// Returns the boolean of tax threshold to a string
        /// </summary>
        public string TaxThresholdToString()
        {
            return TaxThreshold ? "Yes" : "No";
        }
    }
}
