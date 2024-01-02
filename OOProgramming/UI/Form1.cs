using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OOProgramming.Domain;
using OOProgramming.Persistence;
using CsvHelper.Configuration.Attributes;
using Microsoft.VisualBasic.Devices;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace OOProgramming.UI
{
    public partial class Form1 : Form
    {
        private readonly BindingSource _employees;
        private readonly BindingSource _paySummary;
        private readonly PayCalculator _noThreshold;
        private readonly PayCalculator _withThreshold;

        private PaySlip paySlip;
        /// <summary>
        /// Initialisation of the UI
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // Add code below to complete the implementation to populate the listBox
            // by reading the employee.csv file into a List of Employee objects, then binding this to the ListBox.
            // CSV file format: <employee ID>, <first name>, <last name>, <hourly rate>,<taxthreshold>
            try
            {
                //pay slip stores nothing right now.
                _paySummary = new BindingSource();
                _paySummary.DataSource = new List<Employee>();
                string path = CsvHandler.GetDirectory("employee.csv");
                _employees = new BindingSource();
                _employees.DataSource = CsvHandler.ReadEmployeeCsvFile(path);
                
                _noThreshold = CsvHandler.ReadTaxCsvFile(CsvHandler.GetDirectory("taxrate-nothreshold.csv"));
                _withThreshold = CsvHandler.ReadTaxCsvFile(CsvHandler.GetDirectory("taxrate-withthreshold.csv"));
                listBox1.DataSource = _employees;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show($"{e.Message}\nClick okay to close application","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (CsvHelper.TypeConversion.TypeConverterException e)
            {
                MessageBox.Show($"{e.Message}\nClick okay to close application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show($"{e.Message}\nClick okay to close application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (IOException e)
            {
                MessageBox.Show($"{e.Message}\nClick okay to close application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // The calculate button
            // Add code below to complete the implementation to populate the
            // payment summary (textBox2) using the Employee and PayCalculatorNoThreshold
            // and PayCalculatorWithThresholds classes object and methods.
            
            object item = listBox1.SelectedItem;
            object hours = textBox1.Text;
            int hourParsed;
            if (item == null)
            {
                return;
            }

            if (int.TryParse(hours.ToString(), out hourParsed) == false) 
            {
                MessageBox.Show("Please enter an integer for hours","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBox1.Text = "0";
                return;
            }
            if (hourParsed <0 || hourParsed >168)
            {
                MessageBox.Show("Hours must be between 0 or 168", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "0";
                return;
            }

            Employee emp = item as Employee;
            if (emp != null)
            { 
                paySlip = PayCalculator.GeneratePaySlip(emp, hourParsed, _withThreshold, _noThreshold);
                textBox2.Text = paySlip.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // The save button
            // Add code below to complete the implementation for saving the
            // calculated payment data into a csv file.
            // File naming convention: Pay_<full name>_<datetimenow>.csv
            // Data fields expected - EmployeeId, Full Name, Hours Worked, Hourly Rate, Tax Threshold, Gross Pay, Tax, Net Pay, Superannuation

            try {
                //Generate file path
                DateTime date = DateTime.Now;
                string fileName = string.Format("Pay_{0} {1}_{2}.csv", paySlip.Employee.LastName, paySlip.Employee.FirstName, date.ToString("dd-MM-yyyy"));
                string path = CsvHandler.GetDirectory(fileName);
                //Writes csv file
                CsvHandler.WritePaySlipToCsv(paySlip, path);
                
                //reset paySlip, button and Pay Slip summary textbox. and show success message.
                paySlip = null;
                textBox2.Text = string.Empty;
                button2.Visible = false;
                MessageBox.Show($"Created {fileName} at {path}","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (IOException er) 
            {
                MessageBox.Show($"{er.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //If textbox2(Pay summary box) text changes, the save button appears
            button2.Visible = true;
        }
    }
}
