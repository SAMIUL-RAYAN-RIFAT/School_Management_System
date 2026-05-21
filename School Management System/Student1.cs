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
    public partial class Student1 : Form
    {
        public Student1()
        {
            InitializeComponent();
        }

        private string ID { get; set; }
        private string Name { get; set; }
        public Student1(string id, string name) : this()
        {
            this.lbl_profile.Text = name;
            this.label3.Text = id;
            this.ID = id;
            this.Name = name;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Show_Course addCourseForm = new Show_Course(ID,Name);
            addCourseForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = "D:\\demo\\noti.txt";

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the content of the file and display it in the RichTextBox
                richTextBox1.Text = File.ReadAllText(filePath);
            }
            else
            {
                // If the file doesn't exist, show a message
                MessageBox.Show("No notification found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the content of the file and display it in the RichTextBox
                richTextBox1.Text = File.ReadAllText(filePath);
            }
            else
            {
                // If the file doesn't exist, show a message
                MessageBox.Show("No notification found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
          /*  OpenFileDialog op = new OpenFileDialog();

            if ( op.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                axAcroPDF1.src = op.FileName;

            }  */
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            Payment payment= new Payment(ID,Name);
            payment.Show();
        }
    }
}
