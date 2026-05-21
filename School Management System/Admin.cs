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
    public partial class Admin : Form
    {
        private DataAccess Da { get; set; }
        private string ID { get; set; }
        private string Name { get; set; }
        public Admin()
        {
            InitializeComponent();
        }
        public Admin(string id, string name) : this()
        {
            this.lbl_profile.Text = name;
            this.label3.Text = id;
            this.ID = id;
            this.Name = name;
        }

        public void AddUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            pnl_usercontrol.Controls.Clear();
            pnl_usercontrol.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void btn_UserManagement_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement();
            AddUserControl(userManagement);
        }

        private void btn_dashBoard_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            AddUserControl(dashboard);
        }

        private void btn_CourseManagement_Click(object sender, EventArgs e)
        {
            CourseManagement courseManagement = new CourseManagement();
            AddUserControl(courseManagement);
        }

        private void btn_ClassManagement_Click(object sender, EventArgs e)
        {
            ClassManagement classManagement = new ClassManagement();
            AddUserControl(classManagement);
        }

        private void btn_batchManagement_Click(object sender, EventArgs e)
        {
            BatchManagement batchManagement = new BatchManagement();
            AddUserControl(batchManagement);
        }

        private void btn_StudentEnrollement_Click(object sender, EventArgs e)
        {
            StudentEnrollment studentEnrollment = new StudentEnrollment();
            AddUserControl(studentEnrollment);
        }

        private void btn_Singout_Click(object sender, EventArgs e)
        {
           //Application.Exit(); 
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void lbl_profile_Click(object sender, EventArgs e)
        {

        }

        private void butAssignTeacher_Click(object sender, EventArgs e)
        {

            Assign_Teacher batchManagement = new Assign_Teacher();
            AddUserControl(batchManagement);
        }

        private void pnl_usercontrol_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
