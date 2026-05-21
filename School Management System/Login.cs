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
    public partial class Login : Form
    {
        private DataAccess Da {  get; set; }

        public Login()
        {
            InitializeComponent();
            Da = new DataAccess();    //create a instance of DataAccess for database operations

        }
          
        private bool IsValidToSave()    //Ensures both UserId and Password fields are not empty before allowing login.
        {
            if (String.IsNullOrEmpty(this.txt_UserId.Text) || String.IsNullOrEmpty(this.txt_UserPass.Text))
            return false;
            else 
            return true;
        }


        private void login_button_Click(object sender, EventArgs e)
        {
            if (!IsValidToSave())
            {
                MessageBox.Show("Fill all the information ");
                return;
            }
            String sql = "Select * from userInfo where userId = '" + this.txt_UserId.Text + "'  and userPass ='" + this.txt_UserPass.Text + "';";
            var ds = this.Da.ExecuteQuery(sql);

            if (ds.Tables[0].Rows.Count == 1)
            {
                var name = ds.Tables[0].Rows[0][2].ToString();
                var id = ds.Tables[0].Rows[0][0].ToString();
                var role = ds.Tables[0].Rows[0][3].ToString();
                var status = ds.Tables[0].Rows[0][8].ToString(); 

                if (status == "1") // Check if user is active
                {
                    if (role == "Admin")
                    {
                        this.Hide();
                        new Admin(id, name).Show();///*********send the login id,name======================================================
                    }
                    else if (role == "Teacher")
                    {
                        //new Update_Grade();
                        new Update_Grade(id,name);//teacher login korle update grade e value jabe
                        new Add_Course(id, name);
                        this.Hide();
                        new Teacher(id,name).Show();
                        
                    }
                    else if (role == "Student")
                    {
                        new  Show_Course(id,name);
                        new Payment(id, name);
                        this.Hide();
                        new Student1(id,name).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Your account is inactive. Please contact the administrator.");
                }
            }
            else
            {
                MessageBox.Show("The UserName or Password you entered is incorrect, Try Again");
                txt_UserId.Clear();
                txt_UserPass.Clear();
                txt_UserId.Focus();
            }


        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            txt_UserId.Clear();
            txt_UserPass.Clear();
            txt_UserId.Focus();
        }

        private void checkBox1_show_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbox_showpass.Checked)
            {
                txt_UserPass.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                txt_UserPass.UseSystemPasswordChar = true ; // Hide password
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registation registation = new Registation(); // Create an instance of the Add_course form
            registation.Show(); // Show the form
        }
    }
}
