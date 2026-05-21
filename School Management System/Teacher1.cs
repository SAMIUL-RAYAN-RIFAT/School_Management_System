using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School_Management_System
{
    internal class Teacher1
    {

        private DataAccess Da { get; set; }


        public Teacher1()
        {

            Da = new DataAccess();
        }




        public DataTable searchStudentId(string student_id)
        {
            string sql = "SELECT * FROM enrollments WHERE student_id = @student_id";

            var parameters = new SqlParameter[]
            {
        new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id }
            };

            return this.Da.ExecuteQueryTable(sql, parameters);
        }






        public bool InsertEnrollment(string student_id, string course_id)
        {
            try
            {
                // no 1: Geting the timing and subject 
                string getCourseDetailsQuery = @"
            SELECT Timing, Subject 
            FROM courseInfo
            WHERE CourseID = @course_id";

                var courseDetailsParams = new SqlParameter[]
                {
            new SqlParameter("@course_id", SqlDbType.VarChar) { Value = course_id }
                };

                DataTable courseDetailsTable = this.Da.ExecuteQueryTable(getCourseDetailsQuery, courseDetailsParams);

                if (courseDetailsTable == null || courseDetailsTable.Rows.Count == 0)
                {
                    Console.WriteLine("Error: Course not found.");
                    return false; // Cours not exist
                }

                string timing = courseDetailsTable.Rows[0]["Timing"].ToString();
                string subject = courseDetailsTable.Rows[0]["Subject"].ToString();

                // No 2: Check if the student is already enrolled in another course with the SAME Subject
                string checkSubjectQuery = @"
            SELECT e.student_id, e.course_id 
            FROM enrollments e
            JOIN courseInfo c ON e.course_id = c.CourseID
            WHERE e.student_id = @student_id 
            AND c.Subject = @subject";

                var checkSubjectParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@subject", SqlDbType.VarChar) { Value = subject }
                };

                DataTable subjectCheckTable = this.Da.ExecuteQueryTable(checkSubjectQuery, checkSubjectParams);

                if (subjectCheckTable != null && subjectCheckTable.Rows.Count > 0)
                {
                    Console.WriteLine("Error: The student is already enrolled in course with the same subject.");
                    return false; // Student already  in the same subject there fore showing fls
                }

                // no 3:--- See the student is already enrolled with the SAME Timing
                string checkTimingQuery = @"
            SELECT e.student_id, e.course_id 
            FROM enrollments e
            JOIN courseInfo c ON e.course_id = c.CourseID
            WHERE e.student_id = @student_id 
            AND c.Timing = @timing";

                var checkTimingParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@timing", SqlDbType.VarChar) { Value = timing }
                };

                DataTable timingCheckTable = this.Da.ExecuteQueryTable(checkTimingQuery, checkTimingParams);

                if (timingCheckTable != null && timingCheckTable.Rows.Count > 0)
                {
                    Console.WriteLine("Error: The student is already enrolled in course at the same time.");
                    return false; // Student already enrolled at the same time
                }

                // Num 4: Insert the new enrollment no conflicts exist
                string insertQuery = @"
            INSERT INTO enrollments (student_id, course_id) 
            VALUES (@student_id, @course_id)";

                var insertParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@course_id", SqlDbType.VarChar) { Value = course_id }
                };

                int rowsAffected = this.Da.ExecuteDMLQuery(insertQuery, insertParams);

                return rowsAffected > 0; // Return true if insertion was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }



        public bool DeleteEnrollment(string student_id, string course_id)
        {
            try
            {
                string deleteQuery = @"
        DELETE FROM enrollments 
        WHERE student_id = @student_id AND course_id = @course_id";

                var deleteParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@course_id", SqlDbType.VarChar) { Value = course_id }
                };

                int rowsAffected = this.Da.ExecuteDMLQuery(deleteQuery, deleteParams);

                return rowsAffected > 0; // Return true if a row was deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public bool UpdateEnrollment(string student_id, string old_course_id, string new_course_id)
        {
            try
            {
                // No 1  Get the timing and subject for the new course
                string getCourseDetailsQuery = @"
        SELECT Timing, Subject 
        FROM courseInfo
        WHERE CourseID = @new_course_id";

                var courseDetailsParams = new SqlParameter[]
                {
            new SqlParameter("@new_course_id", SqlDbType.VarChar) { Value = new_course_id }
                };

                DataTable courseDetailsTable = this.Da.ExecuteQueryTable(getCourseDetailsQuery, courseDetailsParams);

                if (courseDetailsTable == null || courseDetailsTable.Rows.Count == 0)
                {
                    Console.WriteLine("Error: New course not found.");
                    return false; // Course does not exist
                }

                string newTiming = courseDetailsTable.Rows[0]["Timing"].ToString();
                string newSubject = courseDetailsTable.Rows[0]["Subject"].ToString();

                // no 2 Check  student is already enroll in a course with the same subject
                string checkSubjectQuery = @"
        SELECT e.student_id, e.course_id 
        FROM enrollments e
        JOIN courseInfo c ON e.course_id = c.CourseID
        WHERE e.student_id = @student_id 
        AND c.Subject = @new_subject
        AND e.course_id != @old_course_id";  // Exclude the old course

                var checkSubjectParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@new_subject", SqlDbType.VarChar) { Value = newSubject },
            new SqlParameter("@old_course_id", SqlDbType.VarChar) { Value = old_course_id }
                };

                DataTable subjectCheckTable = this.Da.ExecuteQueryTable(checkSubjectQuery, checkSubjectParams);

                if (subjectCheckTable != null && subjectCheckTable.Rows.Count > 0)
                {
                    Console.WriteLine("Error: The student is already enrolled in another course with the same subject.");
                    return false;
                }

                // no 3 see the student is already enrolled in another course at the same time
                string checkTimingQuery = @"
        SELECT e.student_id, e.course_id 
        FROM enrollments e
        JOIN courseInfo c ON e.course_id = c.CourseID
        WHERE e.student_id = @student_id 
        AND c.Timing = @new_timing
        AND e.course_id != @old_course_id"; // Exclude the old course

                var checkTimingParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@new_timing", SqlDbType.VarChar) { Value = newTiming },
            new SqlParameter("@old_course_id", SqlDbType.VarChar) { Value = old_course_id }
                };

                DataTable timingCheckTable = this.Da.ExecuteQueryTable(checkTimingQuery, checkTimingParams);

                if (timingCheckTable != null && timingCheckTable.Rows.Count > 0)
                {
                    Console.WriteLine("Error: The student is already enrolled in another course at the same time.");
                    return false;
                }

                // no 4--- Update the enrollment if no conflicts exist
                string updateQuery = @"
        UPDATE enrollments 
        SET course_id = @new_course_id 
        WHERE student_id = @student_id AND course_id = @old_course_id";

                var updateParams = new SqlParameter[]
                {
            new SqlParameter("@student_id", SqlDbType.VarChar) { Value = student_id },
            new SqlParameter("@old_course_id", SqlDbType.VarChar) { Value = old_course_id },
            new SqlParameter("@new_course_id", SqlDbType.VarChar) { Value = new_course_id }
                };

                int rowsAffected = this.Da.ExecuteDMLQuery(updateQuery, updateParams);

                return rowsAffected > 0; // Return true if update was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }








    }
}


















