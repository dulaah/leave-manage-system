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

namespace Grifindo_Lanka_system
{

    public partial class adminDashboard_form : KryptonForm
    {
        int rid, emid;
        string s_time, e_time;
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        public adminDashboard_form()
        {
            InitializeComponent();
        }

        private void ShowEmployeeCount()
        {
            // Open the connection
            conn.Open();

            // SQL query to count the number of employees in empReg_tbl
            string query = "SELECT COUNT(*) FROM empReg_tbl";

            // Create and execute the command
            SqlCommand cmd = new SqlCommand(query, conn);

            // Execute the query and retrieve the result
            int employeeCount = (int)cmd.ExecuteScalar();

            // Close the connection
            conn.Close();

            // Set the result to label8
            label8.Text =  employeeCount.ToString();
        }

        private void adminDashboard_form_Load(object sender, EventArgs e)
        {
            ShowEmployeeCount();
            Showleavecount();
            LoadEmployeeIDs();

            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            {
                conn.Open();

                string query = "SELECT ISNULL(MAX(roaster_id), 0) FROM roaster_tbl";

                SqlCommand command = new SqlCommand(query, conn);

                object result = command.ExecuteScalar();
                int newId = 1;

                if (result != null)
                {
                    newId = Convert.ToInt32(result) + 1;
                }

                txtid.Text = newId.ToString();

            }
        }
        private void Showleavecount()
        {
            // Open the connection
            conn.Open();

            // SQL query to count the number of leaves in leave_Man_tbl
            string query = "SELECT COUNT(*) FROM leave_Man_tbl";

            // Create and execute the command
            SqlCommand cmd = new SqlCommand(query, conn);

            // Execute the query and retrieve the result
            int leavecount = (int)cmd.ExecuteScalar();

            // Close the connection
            conn.Close();

            // Set the result to label10
            label10.Text = leavecount.ToString();
        }

        private void tbemploye_Click(object sender, EventArgs e)
        {
            emp_manageForm emp_ManageForm = new emp_manageForm();
            this.Hide();
            emp_ManageForm.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            leaveMan_adminForm leaveMan_AdminForm = new leaveMan_adminForm();
            this.Hide();
            leaveMan_AdminForm.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            reports_form empReportsForm = new reports_form();
            this.Hide();
            empReportsForm.Show();
        }
        private void LoadEmployeeIDs()
        {
            // Clear any existing items in the ComboBox
            cmdempID.Items.Clear();

            try
            {
                // Open the connection
                conn.Open();

                // SQL query to select employee IDs
                string query = "SELECT id FROM empReg_tbl";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                // Loop through each row in the result
                while (reader.Read())
                {
                    // Add each employee ID to the ComboBox
                    cmdempID.Items.Add(reader["id"].ToString());
                }

                // Close the reader
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Close the connection
                conn.Close();
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                LoadData();
                string save = "insert into roaster_tbl values ('" + rid + "','" + emid + "','" + s_time + "','" + e_time+"')";
                SqlCommand cmd = new SqlCommand(save, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your save was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { conn.Close(); }
        }

        public void LoadData()
        {
            rid=int.Parse(txtid.Text);
           emid=int.Parse(cmdempID.Text);
            s_time=txts_time.Text;
            e_time=txte_time.Text;

        }
    }
}
