using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Show_Course : Form

    {
        private DataAccess Da { get; set; }
        private string ID { get; set; }
        private string Name { get; set; }
        /*public Show_Course()
        {
            InitializeComponent();
            this.textBox1.ReadOnly = true;
        }*/
        public Show_Course(string id, string name)   //taking id name from login page
        {
            InitializeComponent();
            this.ID = id;
            this.Name = name;
            this.Da = new DataAccess();
            this.label5.Text = name;
            this.label3.Text = id;
            this.textBox1.ReadOnly = true;
            textBox1.Text = id;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Student1().Show(); // Show the form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string studentId = textBox1.Text; // Get student ID from TextBox

            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Please enter a Student ID.");
                return;
            }

            Student student = new Student(); // Create an instance of Student class
            DataTable dt = student.GetEnrollmentDetails(studentId); // Call method from Student class

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt; // Display results in DataGridView
            }
            else
            {
                MessageBox.Show("No records found for the given Student ID.");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new Student1(ID,Name).Show();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
