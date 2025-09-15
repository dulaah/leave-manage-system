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
    public partial class leaveMan_adminForm : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        string time, reson, type;
        int leaveid, id;
        DateTime s_date,e_date;

        public leaveMan_adminForm()
        {
            InitializeComponent();
        }

        private void leaveMan_adminForm_Load(object sender, EventArgs e)
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
            LoadGridView();
        }

        private void txtidsearch_TextChanged(object sender, EventArgs e)
        {
            TextboxFilter();
        }

        private void tbemploye_Click(object sender, EventArgs e)
        {
            emp_manageForm em=new emp_manageForm();
            this.Hide();
            em.Show();
        }

        private void btninsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                LoadData();
 
                string save = "INSERT INTO leave_Man_tbl (leaveID, id, leave_type, Start_date, End_date, time, reason, status) " +
              "VALUES ('" + leaveid + "', '" + id + "', '" + type + "', '" + s_date + "', '" + e_date + "', '" + time + "', '" + reson + "', 'Approved')";

                SqlCommand cmd = new SqlCommand(save, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Leave  was successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadGridView();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            
            try
            {
                conn.Open();
                LoadData();

                // Corrected SQL command, with "Approved" directly in the values
                string save = "UPDATE leave_Man_tbl SET status = 'Approved' WHERE leaveID = '" + leaveid + "'";


                SqlCommand cmd = new SqlCommand(save, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Approve was successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            LoadGridView();
        }

        private void btnreject_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                LoadData();

                // Corrected SQL command, with "Approved" directly in the values
                string save = "UPDATE leave_Man_tbl SET status = 'Rejected' WHERE leaveID = '" + leaveid + "'";


                SqlCommand cmd = new SqlCommand(save, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Rejected !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            LoadGridView();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            reports_form rp= new reports_form();
            this.Hide();
            rp.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminDashboard_form adminDashboard_Form = new adminDashboard_form();
            adminDashboard_Form.Show();
            this.Hide();
        }

        private void LoadData()
        {
            leaveid=int.Parse(txtid.Text);
            id=int.Parse(txtempID.Text);
            type=cmdcatagory.Text;
            s_date=dtps_date.Value;
            e_date=dtps_date.Value;
            time=txtperiod.Text;
            reson=txtperiod.Text;
           
        }
        private void TextboxFilter()
        {
            try
            {
                conn.Open();

                string query = "SELECT * FROM leave_Man_tbl WHERE leaveID = @leaveID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@leaveID", txtidsearch.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtid.Text = reader["leaveID"].ToString();
                    txtempID.Text = reader["id"].ToString();
                    cmdcatagory.Text = reader["leave_type"].ToString();
                    dtps_date.Text = reader["Start_date"].ToString();
                    dtpe_date.Text = reader["End_date"].ToString();
                    txtperiod.Text = reader["time"].ToString();
                    txtreason.Text = reader["reason"].ToString();

                }
                else
                {
                    Cleardata();
                    leaveMan_adminForm pt = new leaveMan_adminForm();
                    pt.Show();
                    this.Hide();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

        }
        private void Cleardata()
        {
            txtid.Text = "";
            txtempID.Text = "";
            cmdcatagory.Text = "";
            dtps_date.Text = "";
            dtpe_date.Text = "";
            txtperiod.Text = "";
            txtreason.Text = "";

        }
        private void LoadGridView()
        {
            conn.Open();
            string query = "select * from leave_Man_tbl";
            SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dgvleavs.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
            // Change the column header for 'id' to 'Employee ID'
            dgvleavs.Columns["id"].HeaderText = "Employee ID";

            // Highlight the 'status' column to indicate it's "Approved"
            dgvleavs.Columns["status"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvleavs.Columns["status"].DefaultCellStyle.Font = new Font(dgvleavs.DefaultCellStyle.Font, FontStyle.Bold);
        }


    }
}
