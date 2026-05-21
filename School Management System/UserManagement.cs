using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class UserManagement : UserControl
    {
       
        private DataAccess Da { get; set; }
        public UserManagement()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGidView();
            this.AutoIdGenerate();
            
                 this.txtUserID.ReadOnly = true;
        }
        private void AutoIdGenerate()
        {
            var sql = "select userId from userInfo order by userId desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "U-" + (++num).ToString("d3");
            this.txtUserID.Text = newId;
        }

        public void PopulateGidView(string sql = "select * from userInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvUser.AutoGenerateColumns = true;
            this.dgvUser.DataSource = ds.Tables[0];
        }

        private void ClearAll()
        {
            this.txtUserID.Clear();
            this.txtUserName.Clear();
            this.txtPassword.Clear();
            this.txtNumber.Clear();
            this.txtAddress.Clear();
            this.dtpDateOfBirth.Text = "";
            this.cmbStatus.SelectedIndex = -1;
            this.cmbRole.SelectedIndex = -1;
            this.cmbGender.SelectedIndex = -1;

            this.txtAutoSearch.Clear();

            this.dgvUser.ClearSelection();
            this.AutoIdGenerate();
        }

        private bool IsValidToSave()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtUserID.Text) || String.IsNullOrEmpty(this.txtUserName.Text)
                || String.IsNullOrEmpty(this.txtPassword.Text) || String.IsNullOrEmpty(this.txtNumber.Text)
                || String.IsNullOrEmpty(this.txtAddress.Text) || String.IsNullOrEmpty(this.cmbGender.Text)
                || String.IsNullOrEmpty(this.cmbRole.Text) || String.IsNullOrEmpty(this.cmbStatus.Text))
                {
                    return false;
                }
                else
                {
                    if (IsValidBangladeshiPhoneNumber(this.txtNumber.Text))
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Bangladeshi phone number.\n" + "Example : 01782641610");
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return false;
            }

        }
        private bool IsValidBangladeshiPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^(?:\+?88)?01[0-9]\d{8}$");

            return regex.IsMatch(phoneNumber);
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information Correctly");
                    return;
                }

                string query = null;
                var sql = "select * from userInfo where userId = '" + this.txtUserID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    query = @"INSERT INTO userInfo(userId, userPass, userName, role, gender, address, phoneNumber, dateOfBirth,status) 
                                VALUES('" + this.txtUserID.Text + @"', 
                                '" + this.txtPassword.Text + @"', 
                                '" + this.txtUserName.Text + @"', 
                                '" + this.cmbRole.Text + @"', 
                                '" + this.cmbGender.Text + @"', 
                                '" + this.txtAddress.Text + @"', 
                                '" + this.txtNumber.Text + @"',  
                                '" + this.dtpDateOfBirth.Text + @"' ,
                                '" + this.cmbStatus.Text + "');";
                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("User data has been added properly");
                    else
                        MessageBox.Show("User data saving failed");
                }
                this.PopulateGidView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error has been found:\n" + exc.Message);
            }

        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtUserID.Text))
                {
                    MessageBox.Show("Please enter a User ID to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userId = this.txtUserID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM userInfo WHERE userId = '{userId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information correctly before updating.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update query
                string updateQuery = $@"UPDATE userInfo 
                    SET userPass = '{this.txtPassword.Text}',
                        userName = '{this.txtUserName.Text}',
                        role = '{this.cmbRole.Text}',
                        gender = '{this.cmbGender.Text}',
                        address = '{this.txtAddress.Text}',
                        phoneNumber = '{this.txtNumber.Text}',
                        dateOfBirth = '{this.dtpDateOfBirth.Text}', 
                        status = '{this.cmbStatus.Text}'
                    WHERE userId = '{userId}';";

                int count = this.Da.ExecuteDMLQuery(updateQuery);

                if (count >= 1)
                {
                    MessageBox.Show("User information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Check the role and insert into the respective table
                    string role = this.cmbRole.Text.ToLower();
                    if (role == "teacher")
                    {
                        // Insert into teacherInfo table (silently, no notification)
                        string insertTeacherQuery = $@"INSERT INTO teacherInfo (teacherId, teacherName, password,gender) 
                                       VALUES ('{userId}', '{this.txtUserName.Text}', '{this.txtPassword.Text}','{this.cmbGender.Text}');";
                        this.Da.ExecuteDMLQuery(insertTeacherQuery);
                    }
                    else if (role == "student")
                    {
                        // Insert into studentInfo table (silently, no notification)
                        string insertStudentQuery = $@"INSERT INTO studentInfo (studentId, studentName, password,gender) 
                                       VALUES ('{userId}', '{this.txtUserName.Text}', '{this.txtPassword.Text}','{this.cmbGender.Text}');";
                        this.Da.ExecuteDMLQuery(insertStudentQuery);
                    }
                }
                else
                {
                    MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtUserID.Text))
                {
                    MessageBox.Show("Please enter a User ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string userId = this.txtUserID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM userInfo WHERE userId = '{userId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Delete query
                string deleteQuery = $"DELETE FROM userInfo WHERE userId = '{userId}';";
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

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            this.ClearAll();
            this.AutoIdGenerate();
        }

        private void txtAutoSearch_TextChanged_1(object sender, EventArgs e)
        {
            var sql = "SELECT * FROM userInfo WHERE userName LIKE '%" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);

        }

        private void dgvUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtUserID.Text = this.dgvUser.CurrentRow.Cells[0].Value.ToString();
            this.txtUserName.Text = this.dgvUser.CurrentRow.Cells[2].Value.ToString();
            this.txtPassword.Text = this.dgvUser.CurrentRow.Cells[1].Value.ToString();
            this.txtNumber.Text = this.dgvUser.CurrentRow.Cells[6].Value.ToString();
            this.txtAddress.Text = this.dgvUser.CurrentRow.Cells[5].Value.ToString();
            this.dtpDateOfBirth.Text = this.dgvUser.CurrentRow.Cells[7].Value.ToString();
            this.cmbStatus.Text = this.dgvUser.CurrentRow.Cells[8].Value.ToString();
            this.cmbRole.Text = this.dgvUser.CurrentRow.Cells[3].Value.ToString();
            this.cmbGender.Text = this.dgvUser.CurrentRow.Cells[4].Value.ToString();
        }

        private void txtNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
    
}
