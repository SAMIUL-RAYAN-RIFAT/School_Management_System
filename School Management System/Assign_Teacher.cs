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
    public partial class Assign_Teacher : UserControl
    {
       
        public Assign_Teacher()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.txtCourseID.ReadOnly = true;
            

            this.PopulateGidView();
            this.PopulateGidView2();
            // this.AutoIdGenerate();
        }
        private void LoadTeacherIds()
        {
            string sql = "SELECT teacherId FROM teacherInfo";

            DataAccess dataAccess = new DataAccess(); // Create an instance of DataAccess
            DataTable dt = dataAccess.ExecuteQueryTable(sql); // Fetch data using DataAccess class

            if (dt != null)
            {
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "teacherId"; // Column to display
                comboBox2.ValueMember = "teacherId";   // Column to use as value
            }
        }
        private void Assign_Teacher_Load(object sender, EventArgs e)
        {
            LoadTeacherIds();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private DataAccess Da { get; set; }
       

      /*  private void AutoIdGenerate()
        {
            var sql = "select CourseID from courseInfo order by CourseID desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "CRs-" + (++num).ToString("d3");
            this.txtCourseID.Text = newId;
        }
      */
        public void PopulateGidView(string sql = @"SELECT ci.CourseID, ci.Timing, ci.Subject, act.teacherId, ti.teacherName 
FROM courseInfo ci
LEFT JOIN addCourseTeacher act ON ci.CourseID = act.courseId
LEFT JOIN teacherInfo ti ON act.teacherId = ti.teacherId;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvCourse.AutoGenerateColumns = true;
            this.dgvCourse.DataSource = ds.Tables[0];
        }
        public void PopulateGidView2(string sql = @"SELECT teacherId, teacherName, gender FROM teacherInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dataGridView1.AutoGenerateColumns = true;
            this.dataGridView1.DataSource = ds.Tables[0];
        }


        /*   private bool IsValidToSave()
           {
               try
               {
                   if (String.IsNullOrEmpty(this.txtCourseID.Text) || String.IsNullOrEmpty(this.comboBox1.Text)
                   || String.IsNullOrEmpty(this.comboBox2.Text))
                   {
                       return false;
                   }
                   else
                   {
                       return true;

                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("" + ex);
                   return false;
               }



           private void txtAutoSearch_TextChanged_1(object sender, EventArgs e)
           {
             //  var sql = "SELECT * FROM courseInfo WHERE CAST(CourseID AS VARCHAR) LIKE '%" + this.txtAutoSearch.Text + "%';";
              // this.PopulateGidView(sql);
               // Get the search text from the TextBox
               string searchText = this.txtAutoSearch.Text.Trim();

               // Construct the SQL query for partial search across multiple columns
               var sql = $@"SELECT * FROM courseInfo 
                    WHERE CAST(CourseID AS VARCHAR) LIKE '%{searchText}%'
                       OR CourseName LIKE '%{searchText}%'
                       OR Instructor LIKE '%{searchText}%'
                       OR Department LIKE '%{searchText}%';";

               // Populate the DataGridView with the search results
               this.PopulateGidView(sql);

           }
        }
        }*/


        private void panelUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtCourseID.Text = this.dgvCourse.CurrentRow.Cells[0].Value.ToString();
           // this.comboBox1.Text = this.dgvCourse.CurrentRow.Cells[1].Value.ToString();
           // this.comboBox2.Text = this.dgvCourse.CurrentRow.Cells[2].Value.ToString();
            //this.txtTeacherId.Text = this.dgvCourse.CurrentRow.Cells[3].Value.ToString();
            // this.txtClassID.Text = this.dgvCourse.CurrentRow.Cells[5].Value.ToString();
        }

      
        private void btnClear_Click(object sender, EventArgs e)
        {
           // this.ClearAll();
            //this.AutoIdGenerate();
        }

        private void txtCourseID_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAutoSearch_TextChanged(object sender, EventArgs e)
        {
            var sql = "SELECT * FROM courseInfo WHERE CAST(CourseID AS VARCHAR) LIKE '%" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);
        }

        private void dgvCourse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.txtCourseID.Text = this.dgvCourse.CurrentRow.Cells[0].Value.ToString();
        }

        private void dgvCourse_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.txtCourseID.Text = this.dgvCourse.CurrentRow.Cells[0].Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public bool InsertEnrollment(string teacher_id, string course_id)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(teacher_id) || string.IsNullOrEmpty(course_id))
                {
                    Console.WriteLine("Error: teacher_id or course_id is null or empty.");
                    return false;
                }

                // Step 1: Get the timing and subject of the course
                string getCourseDetailsQuery = @"
            SELECT Timing, Subject 
            FROM courseInfo
            WHERE CourseID = @course_id";

                var courseDetailsParams = new SqlParameter[]
                {
            new SqlParameter("@course_id", SqlDbType.VarChar) { Value = course_id }
                };

                DataTable courseDetailsTable = this.Da.ExecuteQueryTable(getCourseDetailsQuery, courseDetailsParams);

                if (courseDetailsTable == null || courseDetailsTable.Rows.Count == 0)
                {
                    Console.WriteLine($"Error: Course with ID {course_id} not found.");
                    return false; // Course does not exist
                }

                string timing = courseDetailsTable.Rows[0]["Timing"].ToString();
                string subject = courseDetailsTable.Rows[0]["Subject"].ToString();

                // Step 2: Check if the teacher is already assigned to another course with the SAME Timing
                string checkTimingQuery = @"
            SELECT act.teacherId, act.courseId 
            FROM addCourseTeacher act
            JOIN courseInfo c ON act.courseId = c.CourseID
            WHERE act.teacherId = @teacher_id 
            AND c.Timing = @timing";

                var checkTimingParams = new SqlParameter[]
                {
            new SqlParameter("@teacher_id", SqlDbType.VarChar) { Value = teacher_id },
            new SqlParameter("@timing", SqlDbType.VarChar) { Value = timing }
                };

                DataTable timingCheckTable = this.Da.ExecuteQueryTable(checkTimingQuery, checkTimingParams);

                if (timingCheckTable != null && timingCheckTable.Rows.Count > 0)
                {
                    Console.WriteLine($"Error: Teacher {teacher_id} is already assigned to a course at the same timing {timing}.");
                    return false; // Teacher already assigned at the same time
                }

                // Step 3: Check if the teacher is assigned to any course
                string checkTeacherAssignmentQuery = @"
            SELECT COUNT(*) 
            FROM addCourseTeacher 
            WHERE teacherId = @teacher_id";

                var checkTeacherAssignmentParams = new SqlParameter[]
                {
            new SqlParameter("@teacher_id", SqlDbType.VarChar) { Value = teacher_id }
                };

                int teacherAssignmentCount = (int)this.Da.ExecuteDMLQuery(checkTeacherAssignmentQuery, checkTeacherAssignmentParams);

                // Step 4: If the teacher is assigned to at least one course, check for subject conflict
                if (teacherAssignmentCount > 0)
                {
                    string checkCourseSubjectQuery = @"
                SELECT act.teacherId, act.courseId 
                FROM addCourseTeacher act
                JOIN courseInfo c ON act.courseId = c.CourseID
                WHERE act.teacherId = @teacher_id 
                AND c.Subject != @subject"; // Enforce subject conflict check

                    var checkCourseSubjectParams = new SqlParameter[]
                    {
                new SqlParameter("@teacher_id", SqlDbType.VarChar) { Value = teacher_id },
                new SqlParameter("@subject", SqlDbType.VarChar) { Value = subject }
                    };

                    DataTable courseSubjectCheckTable = this.Da.ExecuteQueryTable(checkCourseSubjectQuery, checkCourseSubjectParams);

                    if (courseSubjectCheckTable != null && courseSubjectCheckTable.Rows.Count > 0)
                    {
                        Console.WriteLine($"Error: Teacher {teacher_id} is already assigned to a course with a different subject.");
                        return false; // Teacher already assigned to a course with a different subject
                    }
                }

                // Step 5: Insert the new assignment if no conflicts exist
                string insertQuery = @"
            INSERT INTO addCourseTeacher (teacherId, courseId) 
            VALUES (@teacher_id, @course_id)";

                var insertParams = new SqlParameter[]
                {
            new SqlParameter("@teacher_id", SqlDbType.VarChar) { Value = teacher_id },
            new SqlParameter("@course_id", SqlDbType.VarChar) { Value = course_id }
                };

                int rowsAffected = this.Da.ExecuteDMLQuery(insertQuery, insertParams);
                Console.WriteLine($"Rows affected: {rowsAffected}");

                return rowsAffected > 0; // Return true if insertion was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in InsertEnrollment: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

            string courseId = txtCourseID.Text;
            string teacher_id = comboBox2.Text;

            
            bool success = InsertEnrollment(teacher_id, courseId);

            if (success)
            {
                MessageBox.Show("Enrollment successful!");
                PopulateGidView();
            }
            else
            {
                MessageBox.Show("Enrollment failed. ");
            }
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
