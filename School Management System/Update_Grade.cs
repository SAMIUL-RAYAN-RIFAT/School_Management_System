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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School_Management_System
{
    public partial class Update_Grade : Form
    {
        private DataAccess Da { get; set; }


        private string ID { get; set; }
        private string Name { get; set; }
        public Update_Grade(string id, string name)   //taking id name from login page
        {
            InitializeComponent();
            this.ID = id;
            this.Name = name;
            this.Da = new DataAccess();
            this.label2.Text = name;
            this.label3.Text = id;
            this.textBox1.ReadOnly = true;


        }

      
        


        //========================Dynamic keyword(works only one time))================================================
       private void LoadCourseIds()
{
    if (string.IsNullOrEmpty(this.ID))
    {
        MessageBox.Show("Teacher ID is not set.");
        return;
    }

    string sql = "SELECT courseId FROM addCourseTeacher WHERE teacherId = @teacherId;";
    DataAccess dataAccess = new DataAccess();

    var parameters = new SqlParameter[]
    {
        new SqlParameter("@teacherId", SqlDbType.VarChar) { Value = this.ID }
    };

    DataTable dt = dataAccess.ExecuteQueryTable(sql, parameters);

    if (dt != null && dt.Rows.Count > 0)
    {
        comboBox1.DataSource = dt;
        comboBox1.DisplayMember = "courseId";
        comboBox1.ValueMember = "courseId";
    }
    else
    {
        comboBox1.DataSource = null;
        MessageBox.Show("No courses assigned to this teacher.");
    }
}


        private void Update_Grade_Load(object sender, EventArgs e)
        {
            LoadCourseIds();
        }





        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Teacher teacher = new Teacher(ID,Name); // Create an instance of the Add_course form
            teacher.Show(); // Show the form
        }




        //=============================search for student for perticular course=======================================
        private void SearchEnrollments()
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Please select a course.");
                return;
            }

            string courseId = comboBox1.Text;  // Get selected CourseId

            string sql = "SELECT student_id, grade FROM enrollments WHERE course_id = @courseId;";
            var parameters = new SqlParameter[]
            {
        new SqlParameter("@courseId", SqlDbType.VarChar) { Value = courseId }
            };

            DataTable dt = this.Da.ExecuteQueryTable(sql, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;  // Display in DataGridView
            }
            else
            {
                dataGridView1.DataSource = null;
                MessageBox.Show("No students found for the selected course.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SearchEnrollments();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        //=====================================grade add update=======================================================================
        private void UpdateEnrollment()
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(comboBox2.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            string courseId = comboBox1.Text;    // Get selected CourseId
            string studentId = textBox1.Text; // Get StudentId from TextBox       txtStudentId.Text;
            string grade = comboBox2.Text;         // Get Grade from TextBox       txtGrade

            string sql = "UPDATE enrollments SET grade = @grade WHERE student_id = @studentId AND course_id = @courseId;";
            var parameters = new SqlParameter[]
            {
        new SqlParameter("@studentId", SqlDbType.VarChar) { Value = studentId },
        new SqlParameter("@courseId", SqlDbType.VarChar) { Value = courseId },
        new SqlParameter("@grade", SqlDbType.VarChar) { Value = grade }
            };

            int rowsAffected = this.Da.ExecuteDMLQuery(sql, parameters);

            if (rowsAffected > 0)
            {
                MessageBox.Show("Enrollment added successfully!");
                SearchEnrollments();  // Refresh DataGridView totally search operation ekane diyai data grid refresh hobe
            }
            else
            {
                MessageBox.Show("Failed to add enrollment.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateEnrollment();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.textBox1.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            this.comboBox2.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
