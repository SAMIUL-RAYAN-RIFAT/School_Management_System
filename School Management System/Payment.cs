using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School_Management_System
{
    public partial class Payment : Form
    {

        private DataAccess Da { get; set; }
        private  string ID { get; set; }
        private  string Name { get; set; }

        public Payment(string id, string name)   //taking id name from login page
        {
            InitializeComponent();
           this. ID = id;
            this.Name = name;
            this.Da = new DataAccess();
            this.label5.Text = Name;
            this.label3.Text = ID;
            this.textBox1.ReadOnly = true;
            textBox1.Text = id;
         //  DataAccess dataAccess = new DataAccess();  // Initialize it in the constructor

        }




        private void CalculateTotalFees()
        {
            try
            {
                decimal totalFees = 0;

                // Iterate through each row in the DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Ensure the row is not a new row (if DataGridView allows adding new rows)
                    if (!row.IsNewRow)
                    {
                        // Get the value from the "Fees" column (assuming it's the 4th column, index 3)
                        if (row.Cells["Fees"].Value != null && decimal.TryParse(row.Cells["Fees"].Value.ToString(), out decimal fee))
                        {
                            totalFees += fee;
                        }
                    }
                }

                // Update label7 with the total fees
                label7.Text = totalFees.ToString("C"); // Format as currency
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total fees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






















        private void SearchEnrollment()
        {
            try
            {
                // Ensure the dataAccess object is properly initialized
                if (Da == null)
                {
                    Da = new DataAccess(); // Initialize if not already initialized
                }

                // Ensure the textBox1 has a valid student_id input
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Please enter a student ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string studentId = textBox1.Text.Trim(); // Get the student_id from textBox1

                // Define the SQL query with a WHERE clause to filter by student_id
                string query = @"
        SELECT 
            e.student_id,
            e.course_id,
            c.Timing,
            c.Subject,
            c.Fees
        FROM 
            enrollments e
        JOIN 
            courseInfo c ON e.course_id = c.CourseID
        WHERE
            e.student_id = @studentId";  // Filter by student_id

                // Define the parameter to prevent SQL injection
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@studentId", SqlDbType.VarChar) { Value = studentId }
                };

                // Execute the query and get the result as a DataTable
                DataTable dataTable = Da.ExecuteQueryTable(query, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;

                    CalculateTotalFees();//============================================================================calculatefee
                }
                else
                {
                    MessageBox.Show("No records found for the specified student ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Payment_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchEnrollment();
        }

        private void button2_Click(object sender, EventArgs e)

        {
            this.Hide();
           Student1 student= new Student1(ID, Name);
            student.Show();
        }
    }
}
