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
    public partial class BatchManagement : UserControl
    {
        private DataAccess Da { get; set; }
        public BatchManagement()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGidView();
            this.AutoIdGenerate();
        }

        private void AutoIdGenerate()
        {
            var sql = "select batchId from batchInfo order by batchId desc;";
            var dt = this.Da.ExecuteQueryTable(sql);
            var oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            string newId = "B-" + (++num).ToString("d3");
            this.txtBatchID.Text = newId;
        }

        public void PopulateGidView(string sql = "SELECT * from batchInfo;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvBatch.AutoGenerateColumns = true;
            this.dgvBatch.DataSource = ds.Tables[0];
        }

        private void ClearAll()
        {
            this.txtBatchID.Clear();
            this.txtBatchName.Text = "";
            this.txtCapacity.Text = "";
            this.dtpOpeningdate.Text = "";
            this.txtClassID.Text = string.Empty;
            this.txtAutoSearch.Clear();

            this.dgvBatch.ClearSelection();
            this.AutoIdGenerate();
        }

        private bool IsValidToSave()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtBatchID.Text) || String.IsNullOrEmpty(this.txtBatchName.Text)
                || String.IsNullOrEmpty(this.txtCapacity.Text) 
                || String.IsNullOrEmpty(this.dtpOpeningdate.Text) 
                || String.IsNullOrEmpty(this.txtClassID.Text))
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
            var sql = "select * from batchInfo where batchName like '" + this.txtAutoSearch.Text + "%';";
            this.PopulateGidView(sql);
        }

        private void dgvUser_DoubleClick_1(object sender, EventArgs e)
        {
            this.txtBatchID.Text = this.dgvBatch.CurrentRow.Cells[0].Value.ToString();
            this.txtBatchName.Text = this.dgvBatch.CurrentRow.Cells[1].Value.ToString();
            this.txtCapacity.Text = this.dgvBatch.CurrentRow.Cells[4].Value.ToString();
            this.dtpOpeningdate.Text = this.dgvBatch.CurrentRow.Cells[3].Value.ToString();
            this.txtClassID.Text = this.dgvBatch.CurrentRow.Cells[2].Value.ToString();
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
                var sql = "select * from batchInfo where batchId = '" + this.txtBatchID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    query = @"INSERT INTO batchInfo(batchId,batchName,capacity,openingDate,classId) 
                                VALUES('" + this.txtBatchID.Text + @"', 
                                '" + this.txtBatchName.Text + @"', 
                                '" + this.txtCapacity.Text + @"', 
                                '" + this.dtpOpeningdate.Text + @"', 
                                '" + this.txtClassID.Text + "');";
                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Batch data has been added properly");
                    else
                        MessageBox.Show("Batch data saving failed");
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
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information Correctly");
                    return;
                }

                string query = null;
                var sql = "select * from batchInfo where batchId = '" + this.txtBatchID.Text + "';";
                var ds = this.Da.ExecuteQuery(sql);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    query = @"UPDATE batchInfo
                            SET batchName = '" + this.txtBatchName.Text + @"',
                            capacity = '" + this.txtCapacity.Text + @"',
                            openingDate = '" + this.dtpOpeningdate.Text + @"',
                            classId = '" + this.txtClassID.Text + @"'
                            WHERE batchId = '" + this.txtBatchID.Text + "'; ";

                    var count = this.Da.ExecuteDMLQuery(query);

                    if (count == 1)
                        MessageBox.Show("Batch data has been updated properly");
                    else
                        MessageBox.Show("Batch data upgradation failed");
                }
                this.PopulateGidView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error has been found:\n" + exc.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtBatchID.Text))
                {
                    MessageBox.Show("Please enter a Batch ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string batchId = this.txtBatchID.Text.Trim();

                // Check if the user exists
                string checkQuery = $"SELECT * FROM batchInfo WHERE batchId = '{batchId}';";
                var ds = this.Da.ExecuteQuery(checkQuery);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("User not found. Please enter a valid Batch ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Delete query
                string deleteQuery = $"DELETE FROM batchInfo WHERE batchId = '{batchId}';";
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
