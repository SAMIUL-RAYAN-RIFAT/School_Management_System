using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management_System
{
    internal class DataAccess
    {
        private SqlConnection sqlcon;      //manage the connection to the data set
        public SqlConnection Sqlcon
        {
            get { return this.sqlcon; }
            set { this.sqlcon = value; }
        }

        private SqlCommand sqlcom;       //SQL query or stored procedure
        public SqlCommand Sqlcom
        {
            get { return this.sqlcom; }
            set { this.sqlcom = value; }
        }

        private SqlDataAdapter sda;       //fetches data from database
        public SqlDataAdapter Sda
        {
            get { return this.sda; }
            set { this.sda = value; }
        }

        private DataSet ds;               //store datatable in memory
        public DataSet Ds
        {
            get { return this.ds; }
            set { this.ds = value; }
        }

        public DataAccess()            //constructor
        {
            this.Sqlcon = new SqlConnection(@"Data Source=LAPTOP-BU2I51DL\SQLEXPRESS;Initial Catalog=student_managment1;Integrated Security=True");
            Sqlcon.Open();
        }

        private void QueryText(string query)     //A helper method that initializes the SqlCommand object with the provided query.
        {
            this.Sqlcom = new SqlCommand(query, this.Sqlcon);
        }

        public DataSet ExecuteQuery(string sql)    //used for select query//for data-grid-view value refresh after update
        {
            try
            {
                this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
                this.Sda = new SqlDataAdapter(this.Sqlcom);
                this.Ds = new DataSet();
                this.Sda.Fill(this.Ds);
                return Ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
        public DataTable ExecuteQueryTable(string sql)       //Similar to ExecuteQuery, but instead of returning the entire DataSet, it returns only the first table.
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
            this.Sda = new SqlDataAdapter(this.Sqlcom);
            this.Ds = new DataSet();
            this.Sda.Fill(this.Ds);
            return Ds.Tables[0];
        }

        public int ExecuteDMLQuery(string sql)          //Used for INSERT, UPDATE, and DELETE queries.
        {
            this.Sqlcom = new SqlCommand(sql, this.Sqlcon);
            int u = this.Sqlcom.ExecuteNonQuery();
            return u;
        }
        //---------------------------------------------------------------------//==================================
        public DataTable ExecuteQueryTable(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                this.Sqlcom = new SqlCommand(sql, this.Sqlcon);

                if (parameters != null)
                {
                    this.Sqlcom.Parameters.AddRange(parameters);
                }

                this.Sda = new SqlDataAdapter(this.Sqlcom);
                this.Ds = new DataSet();
                this.Sda.Fill(this.Ds);

                if (Ds.Tables.Count > 0)
                    return Ds.Tables[0];

                return null; // Return null if no data is found
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public int ExecuteDMLQuery(string sql, SqlParameter[] parameters = null) // Modified to accept parameters
        {
            try
            {
                this.Sqlcom = new SqlCommand(sql, this.Sqlcon);

                // Add parameters if any
                if (parameters != null)
                {
                    this.Sqlcom.Parameters.AddRange(parameters);
                }

                // Execute the query and return the number of affected rows
                int rowsAffected = this.Sqlcom.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                // Log the error message to help with debugging
                Console.WriteLine("Error during DML operation: " + ex.Message);
                return -1; // Indicate failure if any error occurs
            }
        }



    }
}
