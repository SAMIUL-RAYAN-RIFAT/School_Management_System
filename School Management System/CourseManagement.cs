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
   

    public partial class CourseManagement : UserControl
    {
        private DataAccess Da { get; set; }
        public CourseManagement()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.txtCourseID.ReadOnly = true;
           
            this.PopulateGidView();
            this.AutoIdGenerate();
        }

        private void AutoIdGenerate()
        {
            var sql = "select CourseID from courseInfo order by CourseID desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "CRs-" + (++num).ToString("d3");
            this.txtCourseID.Text = newId;
        }

        public void PopulateGidView(string sql = "SELECT * from courseInfo")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvCourse.AutoGenerateColumns = true;
            this.dgvCourse.DataSource = ds.Tables[0];
        }

        private void ClearAll()
        {
            this.txtCourseID.Clear();
            //this.comboBox1.Clear();
            // this.comboBox2.Clear();
            this.comboBox1.SelectedIndex = -1;
            this.comboBox2.SelectedIndex = -1;
            this.txtCourseID.Text = string.Empty;
            this.txtAutoSearch.Clear();

            this.dgvCourse.ClearSelection();
            this.AutoIdGenerate();
        }

        private bool IsValidToSave()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtCourseID.Text) || String.IsNullOrEmpty(this.comboBox1.Text)
                || String.IsNullOrEmpty(this.comboBox2.Text) )
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

        }
        

        private void txtAutoSearch_TextChanged_1(object sender, EventArgs e)
        {
            var sql = "SELECT * FROM courseInfo WHERE CourseID LIKE '%" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);
        }

        private void dgvUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtCourseID.Text = this.dgvCourse.CurrentRow.Cells[0].Value.ToString();
            this.comboBox1.Text = this.dgvCourse.CurrentRow.Cells[1].Value.ToString();
            this.comboBox2.Text = this.dgvCourse.CurrentRow.Cells[2].Value.ToString();
            //this.txtTeacherId.Text = this.dgvCourse.CurrentRow.Cells[3].Value.ToString();
           // this.txtClassID.Text = this.dgvCourse.CurrentRow.Cells[5].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information Correctly");
                    return;
                }

                string query = null;
                var sql = "select * from courseInfo where CourseID = '" + this.txtCourseID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    query = @"INSERT INTO courseInfo(CourseID,Timing,Subject) 
                                VALUES('" + this.txtCourseID.Text + @"', 
                                '" + this.comboBox1.Text + @"', 
                                '" + this.comboBox2.Text + "');"; 
                                
                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Course data has been added properly");
                    else
                        MessageBox.Show("Course data saving failed");
                }
                this.PopulateGidView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error has been found:\n" + exc.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information Correctly");
                    return;
                }

                string query = null;
                var sql = "select * from courseInfo WHERE CourseID = '" + this.txtCourseID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    query = @"UPDATE courseInfo
                            SET Timing = '" + this.comboBox1.Text + @"',
                            Subject = '" + this.comboBox2.Text + @"'
                            WHERE CourseID = '" + this.txtCourseID.Text + "'; ";

                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Course data has been updated properly");
                    else
                        MessageBox.Show("Course data upgradation failed");
                }
                this.PopulateGidView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error has been found:\n" + exc.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCourseID.Text))
                {
                    MessageBox.Show("Please enter a Course ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string courseId = this.txtCourseID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM courseInfo WHERE CourseID = '{courseId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid Course ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Delete query
                string deleteQuery = $"DELETE FROM courseInfo WHERE CourseID = '{courseId}';";
                int count = this.Da.ExecuteDMLQuery(deleteQuery);

                if (count == 1)
                {
                    MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Delete failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Refresh DataGridView
                this.PopulateGidView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred:\n" + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
            this.AutoIdGenerate();
        }

       private void txtCourseID_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvCourse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
