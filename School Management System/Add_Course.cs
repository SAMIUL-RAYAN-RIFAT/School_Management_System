using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School_Management_System
{
    public partial class Add_Course : Form

    {
        private DataAccess Da { get; set; }
        public string GetStudentId()
        {
            return comboBox1.Text;
        }


      
        private string ID { get; set; }
        private string Name { get; set; }

        public Add_Course(string id, string name)   //taking id name from login page
        {
            InitializeComponent();
            this.Da = new DataAccess();
            PopulateGidView2();

            this.ID = id;
            this.Name = name;
           
            


        }
        private void LoadStudentIds()        //dynamic combobox
        {
            string sql = "SELECT studentId FROM studentInfo";

            DataAccess dataAccess = new DataAccess(); // Create an instance of DataAccess
            DataTable dt = dataAccess.ExecuteQueryTable(sql); // Fetch data using DataAccess class

            if (dt != null)
            {
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "studentId"; // Column to display
                comboBox1.ValueMember = "studentId";   // Column to use as value
            }
        }
       /* private void AutoIdGenerate()
        {
            var sql = "select student_id from enrollments order by student_id desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "S-" + (++num).ToString("d3");
            this.comboBox1.Text = newId;
        }*/
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
       /* public void PopulateGidView1(string sql = @"SELECT CourseId, Timing, Subject FROM courseInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dataGridView1.AutoGenerateColumns = true;
            this.dataGridView1.DataSource = ds.Tables[0];
        }*/
        public void PopulateGidView2(string sql = @"SELECT CourseId, Timing, Subject FROM courseInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dataGridView2.AutoGenerateColumns = true;
            this.dataGridView2.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Teacher().Show(); // Show the form
        }

        private void Add_Course_Load(object sender, EventArgs e)
        {
            LoadStudentIds();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {

           
        }
        private void ClearTextBoxes()
        {
           // comboBox1.Clear();  // Clears comboBox1
            textBox2.Clear();  // Clears TextBox2
            textBox3.Clear();  // Clears TextBox3
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Teacher1 teacher = new Teacher1(); // Create a single instance
            DataTable dt = teacher.searchStudentId(comboBox1.Text);

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("student does not has any CourseAssign.");
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string studentId = comboBox1.Text;
            string courseId = textBox2.Text;

            Teacher1 teacher = new Teacher1();
            bool success = teacher.InsertEnrollment(studentId, courseId);

            if (success)
            {
                MessageBox.Show("Enrollment successful!");
            }
            else
            {
                MessageBox.Show("Enrollment failed. ");
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            ClearTextBoxes();  // Clears all three text boxes
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string studentId = comboBox1.Text;
            string courseId = textBox2.Text;

            Teacher1 teacher = new Teacher1();

            if (teacher.DeleteEnrollment(studentId, courseId))
            {
                MessageBox.Show("Enrollment deleted successfully!");
                // Refresh DataGridView or UI after deletion
            }
            else
            {
                MessageBox.Show("Failed to delete enrollment. Please check the data.");
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string studentId = comboBox1.Text;
            string oldCourseId = textBox2.Text;
            string newCourseId = textBox3.Text;

            Teacher1 teacher = new Teacher1();

            if (teacher.UpdateEnrollment(studentId, oldCourseId, newCourseId))
            {
                MessageBox.Show("Enrollment updated successfully!");
                // Refresh DataGridView or UI after update
            }
            else
            {
                MessageBox.Show("Failed to update enrollment. Please check the data.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Teacher teacher = new Teacher(ID,Name);
            teacher.Show();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
            //double click korle value datagrid theke textBox e jabe
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            this.textBox2.Text = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
        }




        /* private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
          {
              // Ensure that a valid row is double-clicked (i.e., not the header row)
              if (e.RowIndex >= 0)
              {
                  // Get the values from both columns in the selected row
                  string column1Data = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();  // First column data
                  string column2Data = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();  // Second column data

                  // Fill the TextBox or TextButton controls with the data
                  comboBox1.Text = column1Data;  // Fill first textbox 
                  textBox2.Text = column2Data;  // Fill second textbox 
              }
          }*/
    }
}
