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
    public partial class ClassManagement : UserControl
    {
        private DataAccess Da { get; set; }
        public ClassManagement()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGidView();
            this.AutoIdGenerate();
        }
        private void AutoIdGenerate()
        {
            var sql = "select classId from classInfo order by classId desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "C-" + (++num).ToString("d3");
            this.txtClassID.Text = newId;
        }

        public void PopulateGidView(string sql = "SELECT * FROM classInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvClass.AutoGenerateColumns = true;
            this.dgvClass.DataSource = ds.Tables[0];
        }

        private void ClearAll()
        {
            this.txtClassID.Clear();
            this.txtClassName.Text = "";
            this.txtAutoSearch.Clear();
            this.dgvClass.ClearSelection();
            this.AutoIdGenerate();
        }

        private bool IsValidToSave()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtClassID.Text) || String.IsNullOrEmpty(this.txtClassName.Text))
              
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
            var sql = "select * from classInfo where className like '" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);
        }

        private void dgvUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtClassID.Text = this.dgvClass.CurrentRow.Cells[0].Value.ToString();
            this.txtClassName.Text = this.dgvClass.CurrentRow.Cells[1].Value.ToString();
       
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
                var sql = "select * from classInfo where classId = '" + this.txtClassID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    query = @"INSERT INTO classInfo(classId,className) 
                                VALUES( 
                                '" + this.txtClassID.Text + @"',  
                                '" + this.txtClassName.Text + "');";
                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Class data has been added properly");
                    else
                        MessageBox.Show("Class data saving failed");
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
                if (string.IsNullOrEmpty(this.txtClassID.Text))
                {
                    MessageBox.Show("Please enter a Class ID to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string classId = this.txtClassID.Text.Trim();

                // Check if the class exists
                string checkQuery = $"SELECT * FROM classInfo WHERE classId = '{classId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Class not found. Please enter a valid Class ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information correctly before updating.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update query
                string updateQuery = $@"UPDATE classInfo 
                SET className = '{this.txtClassName.Text}'
                WHERE classId = '{classId}';";

                int count = this.Da.ExecuteDMLQuery(updateQuery);

                if (count == 1)
                {
                    MessageBox.Show("Class data has been updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Class data update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtClassID.Text))
                {
                    MessageBox.Show("Please enter a Class ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string classId = this.txtClassID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM classInfo WHERE classId = '{classId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid Class ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Delete query
                string deleteQuery = $"DELETE FROM classInfo WHERE classId = '{classId}';";
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
    }
}
