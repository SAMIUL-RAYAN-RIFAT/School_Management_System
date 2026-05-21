using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management_System
{
    internal class Student
    {
        private DataAccess Da { get; set; }



        public Student()
        {

            Da = new DataAccess();
        }



        public DataTable GetEnrollmentDetails(string student_id)
        {
            Teacher1 teacher = new Teacher1();
            return teacher.searchStudentId(student_id);
        }












    }
}
