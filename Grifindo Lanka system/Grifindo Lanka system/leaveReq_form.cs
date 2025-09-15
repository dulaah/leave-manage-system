using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Data.SqlClient;
namespace Grifindo_Lanka_system
{
    public partial class leaveReq_form : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        string time, reson, status,type;
        int leaveid, id;
        public static int gg;

        private void leaveReq_form_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            {
                conn.Open();

                string query = "SELECT ISNULL(MAX(leaveID), 0) FROM leave_Man_tbl";

                SqlCommand command = new SqlCommand(query, conn);

                object result = command.ExecuteScalar();
                int newId = 1;

                if (result != null)
                {
                    newId = Convert.ToInt32(result) + 1;
                }

                txtid.Text = newId.ToString();

            }
            txtempID.Text=gg.ToString();


        }

        private void btnsave_Click_1(object sender, EventArgs e)
        {
                     
            DateTime selectedDate = dtps_date.Value;
            DateTime currentDate = DateTime.Now;

            // Check if the selected category is "Annual" and if the selected date is less than 7 days from now
            if (cmdcatagory.SelectedItem?.ToString() == "Annual" && (selectedDate - currentDate).TotalDays < 7)
            {
                MessageBox.Show("Annual leaves must be applied for at least 7 days in advance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    conn.Open();
                    LoadData();

                    // Define limits for each leave type
                    int annualLeaveLimit = 14;
                    int casualLeaveLimit = 7;
                    int shortLeaveLimit = 24;

                    // Retrieve the selected leave type from the ComboBox
                    string leaveType = cmdcatagory.SelectedItem?.ToString();

                    // Check leave counts based on the selected leave type
                    string checkLeaveCountQuery = $"SELECT COUNT(*) FROM leave_Man_tbl WHERE id = '{id}' AND leave_type = '{leaveType}'";
                    SqlCommand countCmd = new SqlCommand(checkLeaveCountQuery, conn);
                    int leaveCount = (int)countCmd.ExecuteScalar();

                    // Check if the leave count exceeds the limit for each leave type
                    if (leaveType == "Annual" && leaveCount >= annualLeaveLimit)
                    {
                        MessageBox.Show("You cannot apply for more than 14 annual leave days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (leaveType == "Casual" && leaveCount >= casualLeaveLimit)
                    {
                        MessageBox.Show("You cannot apply for more than 7 casual leave days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (leaveType == "Short leaves" && leaveCount >= shortLeaveLimit)
                    {
                        MessageBox.Show("You cannot apply for more than 2 short leave in this month.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Insert new leave record if within limits
                        string save = "INSERT INTO leave_Man_tbl (leaveID, id, leave_type, Start_date, End_date, time, reason, status) " +
                                      "VALUES ('" + leaveid + "', '" + id + "', '" + type + "', '" + s_date + "', '" + e_date + "', '" + time + "', '" + reson + "', 'Pending')";

                        SqlCommand cmd = new SqlCommand(save, conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Your save was successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            autoid();

            //DateTime selectedDate = dtps_date.Value;
            //DateTime currentDate = DateTime.Now;

            //// Check if the selected category is "Annual" and if the selected date is less than 7 days from now
            //if (cmdcatagory.SelectedItem?.ToString() == "Annual" && (selectedDate - currentDate).TotalDays < 7)
            //{
            //    MessageBox.Show("Annual leaves must be applied for at least 7 days in advance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            //{
            //    try
            //    {
            //        conn.Open();
            //        LoadData();

            //        string save = "INSERT INTO leave_Man_tbl (leaveID, id, leave_type, Start_date, End_date, time, reason, status) " +
            //                      "VALUES ('" + leaveid + "', '" + id + "', '" + type + "', '" + s_date + "', '" + e_date + "', '" + time + "', '" + reson + "', 'Pending')";

            //        SqlCommand cmd = new SqlCommand(save, conn);
            //        cmd.ExecuteNonQuery();

            //        MessageBox.Show("Your save was successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //        if (conn.State == ConnectionState.Open)
            //            conn.Close();
            //    }
            //}
            //autoid();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            history_form hf=new history_form();
            history_form.jj = gg;
            this.Hide();
            hf.Show();
        }

        private void cmdcatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmdcatagory.SelectedItem?.ToString() == "Annual")
            {
                txttime.Visible = false;
                label6.Visible = false;
            }
            if (cmdcatagory.SelectedItem?.ToString() == "Short")
            {
                dtpe_date.Visible = false;
                label7.Visible = false;
            }
            if (cmdcatagory.SelectedItem?.ToString() == "Casual")
            {
                txttime.Visible = true;
                label6.Visible = true;
                dtpe_date.Visible = false;
                label7.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            employeDashboard_form form = new employeDashboard_form();
            this.Hide();
            form.Show();
        }

        DateTime s_date, e_date;
        public leaveReq_form()
        {
            InitializeComponent();
        }


        //private void btnsave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        conn.Open();
        //        LoadData();
        //        string save = "insert into leave_Man_tbl values ('" + leaveid + "','" + id + "','"+type+"','" + s_date + "','" + e_date + "','" + time + "','" + reson + "')";
        //        SqlCommand cmd = new SqlCommand(save, conn);
        //        cmd.ExecuteNonQuery();
        //        MessageBox.Show("Your save was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);

        //    }
        //    finally { conn.Close(); }
            
        //}
        private void LoadData()
        {
            leaveid = int.Parse(txtid.Text);
            id = int.Parse(txtempID.Text);
            s_date = dtps_date.Value;
            e_date = dtps_date.Value;
            time = txttime.Text;
            reson = txtreason.Text;
            type=cmdcatagory.Text;
            
        }
        public void autoid() {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-Q7BFFGN;Initial Catalog=Grifindo_System;Integrated Security=True;Encrypt=False"))
            {
                conn.Open();

                string query = "SELECT ISNULL(MAX(leaveID), 0) FROM leave_Man_tbl";

                SqlCommand command = new SqlCommand(query, conn);

                object result = command.ExecuteScalar();
                int newId = 1;

                if (result != null)
                {
                    newId = Convert.ToInt32(result) + 1;
                }

                txtid.Text = newId.ToString();

            }
            txtempID.Text = gg.ToString();
        }
    }

}

