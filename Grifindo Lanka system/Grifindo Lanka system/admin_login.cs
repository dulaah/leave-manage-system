using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
namespace Grifindo_Lanka_system

{
    public partial class admin_login : KryptonForm

    {
        public admin_login()
        {
            InitializeComponent();
        }

        private void admin_login_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string user_Name = txtus.Text.Trim();
            string password = txtpw.Text;

            string query = "SELECT COUNT(*) FROM adminlogin_tbl WHERE username = @user_Name AND password = @password";
            using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            {

                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {

                    command.Parameters.AddWithValue("@user_Name", user_Name);
                    command.Parameters.AddWithValue("@password", password);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {


                        MessageBox.Show("Login successful!!!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        adminDashboard_form admin_Dashboard = new adminDashboard_form();
                        this.Hide();
                        admin_Dashboard.Show();
                    }

                    else
                    {
                        MessageBox.Show("Invalid username or password. Please try again.");
                    }
                }

            }
        }

        private void cbshowhide_CheckedChanged(object sender, EventArgs e)
        {
            if (cbshowhide.Checked)
            {

                txtpw.PasswordChar = '\0';
            }
            else
            {

                txtpw.PasswordChar = '●';
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            txtpw.Text = "";
            txtus.Text = "";
        }
    }
}
