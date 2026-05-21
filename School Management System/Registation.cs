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
 
    public partial class Registation : Form
    {

        
        private DataAccess Da { get; set; }
        public Registation()
        {
            InitializeComponent();
            Da = new DataAccess();
            this.textBox1.ReadOnly = true;
            this.AutoIdGenerate();
        }
        
        // Auto generate User ID for a new user
        private void AutoIdGenerate()
        {
            var sql = "select userId from userInfo order by userId desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "U-000";
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "U-" + (++num).ToString("d3");
            this.textBox1.Text = newId;
        }

        // Method to validate form fields before adding a new user
        private bool IsValidToSave()
        {
            if (string.IsNullOrEmpty(this.textBox1.Text) || string.IsNullOrEmpty(this.txtUserName.Text)
                || string.IsNullOrEmpty(this.txtPassword.Text) || string.IsNullOrEmpty(this.textBox2.Text)
                || string.IsNullOrEmpty(this.txtAddress.Text) || string.IsNullOrEmpty(this.cmbGender.Text)
                || string.IsNullOrEmpty(this.cmbRole.Text)) // Status field removed
            {
                MessageBox.Show("Please fill all the information correctly.");
                return false;
            }

            if (!IsValidBangladeshiPhoneNumber(this.textBox2.Text))
            {
                MessageBox.Show("Invalid Bangladeshi phone number.\nExample: 01782641610");
                return false;
            }

            return true;
        }

        // Validate the phone number format for Bangladesh
        private bool IsValidBangladeshiPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^(?:\+?88)?01[0-9]\d{8}$");
            return regex.IsMatch(phoneNumber);
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    return; // Don't proceed if validation fails
                }

                // SQL to check if the user already exists
                var sql = "select * from userInfo where userId = '" + this.textBox1.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("User already exists.");
                    return;
                }

                // SQL to insert a new user (Default status is set to 'Active')
                string query = @"INSERT INTO userInfo(userId, userPass, userName, role, gender, address, phoneNumber, dateOfBirth, status) 
                                VALUES('" + this.textBox1.Text + "', '" + this.txtPassword.Text + "', '" + this.txtUserName.Text + "', '" + this.cmbRole.Text + "', '" + this.cmbGender.Text + "', '" + this.txtAddress.Text + "', '" + this.textBox2.Text + "', '" + this.dtpDateOfBirth.Text + "', 0);";
                int count = this.Da.ExecuteDMLQuery(query);

                if (count == 1)
                {
                    MessageBox.Show("User data has been added successfully.");
                }
                else
                {
                    MessageBox.Show("User data saving failed.");
                }

                this.ClearAll(); // Clear all fields after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll(); // Reset the form
        }
        private void ClearAll()
        {
            this.textBox1.Clear();
            this.txtUserName.Clear();
            this.txtPassword.Clear();
            this.textBox2.Clear();
            this.txtAddress.Clear();
            this.dtpDateOfBirth.ResetText();
            this.cmbRole.SelectedIndex = -1;
            this.cmbGender.SelectedIndex = -1;

            this.AutoIdGenerate(); // Auto-generate a new user ID after clearing
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login(); // Create an instance of the Add_course form
            login.Show(); // Show the form
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
