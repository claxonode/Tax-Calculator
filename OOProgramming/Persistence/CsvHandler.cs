using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using OOProgramming.Domain;
using System.Security.Cryptography;
using System.Linq.Expressions;

namespace OOProgramming.Persistence
{
    /// <summary>
    /// This class is used to read and write csv data
    /// </summary>
    public class CsvHandler
    {

        /// <summary>
        /// Gets the directory of a file
        /// </summary>
        /// <param name="file">The file name</param>
        /// <returns>The path of the file</returns>
        public static string GetDirectory(string file)
        {
            string path = Directory.GetCurrentDirectory();
            path = Path.Combine(path, "..", "..", "..", "..", file);

            return path;
        }

        /// <summary>
        /// Reads a csv file that contains employee data
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>A collection of employees from the file</returns>
        /// <exception cref="ArgumentOutOfRangeException">Out of range values in csv</exception>
        public static List<Employee> ReadEmployeeCsvFile(string path)
        {
            List<Employee> list = new List<Employee>();
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while (csv.Read())
                {
                    int id = csv.GetField<int>(0);
                    string firstName = csv.GetField<string>(1);
                    string lastName = csv.GetField<string>(2);
                    decimal payRate = csv.GetField<decimal>(3);
                    if (payRate<0)
                    {
                        throw new ArgumentOutOfRangeException($"Invalid pay rate {payRate:C2} at employee id {id}");
                    }
                    string taxThreshold = csv.GetField<string>(4);
                    if (!CheckValidTaxThreshold(taxThreshold))
                    {
                        throw new ArgumentOutOfRangeException("Could not cover tax threshold");
                    }
                    Employee employee = new Employee(id, firstName, lastName, payRate, taxThreshold);
                    list.Add(employee);
                }
            }
            if (list.Count == 0)
            {
                throw new ArgumentOutOfRangeException("No values in list");
            }
            if (!CheckUniqueEmployeeIDs(list))
            {
                throw new ArgumentOutOfRangeException("IDs are not unique");
            }
            
            return list;


        }
        private static Boolean CheckUniqueEmployeeIDs(List<Employee> list)
        {
            HashSet<int> unique = new HashSet<int>();
            list.ForEach(e => { unique.Add(e.Id); });
            return unique.Count == list.Count ? true : false;
        }
        private static Boolean CheckValidTaxThreshold(string taxThreshold)
        {
            string taxThresholdLowerCase = taxThreshold.ToLower();
            Boolean result = (taxThresholdLowerCase == "y"
                || taxThresholdLowerCase =="n"
                || taxThresholdLowerCase =="yes"
                || taxThresholdLowerCase =="no")? true:false;
            return result;
        }
        /// <summary>
        /// Reads either the taxrate-nothreshold.csv or taxrate-withthreshold.csv and creates a PayCalculator
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>A pay calculator that calculates tax,grosspay,netpay,superannuation</returns>
        /// <exception cref="ArgumentOutOfRangeException">Out of range values in csv</exception>
        public static PayCalculator ReadTaxCsvFile(string path)
        {
            PayCalculator payCalculator;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<decimal> weeklyPay = new List<decimal>();
                List<decimal> taxA = new List<decimal>();
                List<decimal> taxB = new List<decimal>();
                while (csv.Read())
                {
                    weeklyPay.Add(csv.GetField<decimal>(1));
                    taxA.Add(csv.GetField<decimal>(2));
                    taxB.Add(csv.GetField<decimal>(3));
                }
                if (weeklyPay.Count == 0 || taxA.Count == 0 || taxB.Count == 0)
                {
                    throw new ArgumentOutOfRangeException($"No values in {path}");
                }
                payCalculator = new PayCalculator(weeklyPay.ToArray(), taxA.ToArray(), taxB.ToArray());
                
                return payCalculator;
            }
        }
        /// <summary>
        /// Writes the csv file contain pay slip data
        /// </summary>
        /// <param name="paySlip">The payslip we want to write</param>
        /// <param name="path">The file path that we want the file</param>
        public static void WritePaySlipToCsv(PaySlip paySlip, string path)
        {
            var records = new List<object>
            {
                new {paySlip.Employee.Id,
                    paySlip.Employee.FirstName,
                    paySlip.Employee.LastName,
                    HoursWorked = paySlip.Hours,
                    HourlyRate = string.Format("{0:C2}",paySlip.Employee.PayRate),
                    TaxThreshold = string.Format("{0:C2}",paySlip.Employee.TaxThresholdToString()),
                    GrossPay = string.Format("{0:C2}",paySlip.GrossPay),
                    Superannuation = string.Format("{0:C2}",paySlip.Super),
                    Tax = string.Format("{0:C2}",paySlip.Tax),
                    NetPay = string.Format("{0:C2}",paySlip.Net)
                }
            };
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
