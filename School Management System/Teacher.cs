using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace School_Management_System
{
    public partial class Teacher : Form
    {
        public Teacher()
        {
            InitializeComponent();
        }
        private string ID { get; set; }
        private string Name { get; set; }
        public Teacher(string id, string name) : this()
        {
            this.lbl_profile.Text = name;
            this.label2.Text = id;
            this.ID = id;
            this.Name = name;
        }
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Add_Course addCourseForm = new Add_Course(ID,Name); // Create an instance of the Add_course form
            addCourseForm.Show(); // Show the form
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void sendbtn_Click(object sender, EventArgs e)
        {
            // Specify the path to the text file
            string filePath = "D:\\demo\\noti.txt";

            // Get the text from the RichTextBox (or any other control)
            string notificationText = notitxt.Text;

            // Check if the text is not empty
            if (!string.IsNullOrEmpty(notificationText))
            {
                // Write the text to the file
                File.WriteAllText(filePath, notificationText);

                // Show a success message
                MessageBox.Show("Notification sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // If the text is empty, show an error message
                MessageBox.Show("Please write a notification before sending!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            // Specify the path to the text file
            string filePath = "D:\\demo\\noti.txt";

            // Clear the text in the RichTextBox (or any other control)
            notitxt.Clear();

            // Clear the content of the file
            File.WriteAllText(filePath, string.Empty);

            // Show a success message
            MessageBox.Show("Notification cleared successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Update_Grade updategrade = new Update_Grade(ID, Name); // Create an instance of the Add_course form
            updategrade.Show(); // Show the form
        }
    }
}
