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
    public partial class StudentEnrollment : UserControl
    {
        private DataAccess Da { get; set; }
        public StudentEnrollment()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGidView();
            this.AutoIdGenerate();
        }

        private void AutoIdGenerate()
        {
            var sql = "select studentId from studentInfo order by studentId desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "S-" + (++num).ToString("d3");
            this.txtStudentID.Text = newId;
        }

        public void PopulateGidView(string sql = "SELECT * from studentInfo ;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvStudent.AutoGenerateColumns = true;
            this.dgvStudent.DataSource = ds.Tables[0];
        }

        private void ClearAll()
        {
            this.txtStudentID.Clear();
            this.txtStudentName.Text = "";
            this.txtBatchID.Text = "";
            this.txtClassID.Text = string.Empty;
            this.txtAutoSearch.Clear();

            this.dgvStudent.ClearSelection();
            this.AutoIdGenerate();
        }

        private bool IsValidToSave()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtStudentID.Text) || String.IsNullOrEmpty(this.txtStudentName.Text)
                || String.IsNullOrEmpty(this.txtBatchID.Text)
                || String.IsNullOrEmpty(this.txtClassID.Text))
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
            var sql = "select * from studentInfo where studentName like '" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);
        }

        private void dgvUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtStudentID.Text = this.dgvStudent.CurrentRow.Cells[0].Value.ToString();
            this.txtStudentName.Text = this.dgvStudent.CurrentRow.Cells[1].Value.ToString();
            this.txtBatchID.Text = this.dgvStudent.CurrentRow.Cells[3].Value.ToString();
            this.txtClassID.Text = this.dgvStudent.CurrentRow.Cells[2].Value.ToString();
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
                var sql = "select * from studentInfo where studentId = '" + this.txtStudentID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    query = @"INSERT INTO studentInfo(studentId,studentName,classId,batchId) 
                                VALUES('" + this.txtStudentID.Text + @"', 
                                '" + this.txtStudentName.Text + @"', 
                                '" + this.txtClassID.Text + @"',
                                '" + this.txtBatchID.Text + "');";
                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Student data has been added properly");
                    else
                        MessageBox.Show("Student data saving failed");
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
                var sql = "select * from studentInfo where studentId = '" + this.txtStudentID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    query = @"UPDATE studentInfo
                            SET studentName = '" + this.txtStudentName.Text + @"',
                            classId = '" + this.txtClassID.Text + @"',
                            batchId = '" + this.txtBatchID.Text + @"'
                            WHERE studentId = '" + this.txtStudentID.Text + "'; ";

                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Student data has been updated properly");
                    else
                        MessageBox.Show("Student data upgradation failed");
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
                if (string.IsNullOrEmpty(this.txtStudentID.Text))
                {
                    MessageBox.Show("Please enter a Student ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string studentId = this.txtStudentID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM studentInfo WHERE studentId = '{studentId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid Batch ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Delete query
                string deleteQuery = $"DELETE FROM studentInfo WHERE studentId = '{studentId}';";
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

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
