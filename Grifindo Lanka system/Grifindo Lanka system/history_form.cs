using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ComponentFactory.Krypton.Toolkit;
namespace Grifindo_Lanka_system
{

    public partial class history_form : KryptonForm
    {
        public static int jj;
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        public history_form()
        {
            InitializeComponent();
        }

        private void history_form_Load(object sender, EventArgs e)
        {
            LoadGridView();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            leaveReq_form lq = new leaveReq_form();
            this.Hide();
            lq.Show();
        }




        private void LoadGridView(int? leaveID = null)
        {
            conn.Open();
            string query = "SELECT * FROM Leave_Man_tbl where id='"+jj+"'";

            if (leaveID.HasValue)
            {
                query += " WHERE leaveID = @leaveID";
            }

            using (SqlDataAdapter adapt = new SqlDataAdapter(query, conn))
            {
                if (leaveID.HasValue)
                {
                    adapt.SelectCommand.Parameters.AddWithValue("@leaveID", leaveID.Value);
                }

                DataSet ds = new DataSet();
                adapt.Fill(ds);
                dgvhistory.DataSource = ds.Tables[0].DefaultView;
            }

            conn.Close();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {

            int trip_id = int.Parse(txtid.Text);
            conn.Open();
            string delete = "delete from leave_Man_tbl where leaveID= '" + trip_id + "'";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery(); MessageBox.Show("Your Delete was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            LoadGridView();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            employeDashboard_form form = new employeDashboard_form();
            this.Hide();
            form.Show();
        }
    }



}
