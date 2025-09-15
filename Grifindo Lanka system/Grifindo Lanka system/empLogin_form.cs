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
    public partial class empLogin_form : KryptonForm
    {
        
        public empLogin_form()
        {
            InitializeComponent();
        }
        



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void btnlogin_Click(object sender, EventArgs e)
        {
                       
            string user_Name = txtus.Text.Trim();
            string password = txtpw.Text;

            // Corrected SQL query with the proper column name for password
            string query = "SELECT id FROM empReg_tbl WHERE username = @user_Name AND passwrod = @password";

            using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Using parameterized query to prevent SQL injection
                    command.Parameters.AddWithValue("@user_Name", user_Name);
                    command.Parameters.AddWithValue("@password", password);

                    // Execute the query to fetch the user ID if login is successful
                    object result = command.ExecuteScalar();

                    if (result != null) // Login successful if result is not null
                    {
                        int id = Convert.ToInt32(result);

                        // Show success message
                        MessageBox.Show("Login successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Store the logged-in employee ID
                        employeDashboard_form.emp = id;

                        // Show the admin dashboard and hide the login form
                        employeDashboard_form em = new employeDashboard_form();
                        this.Hide();
                        em.Show();
                    }
                    else
                    {
                        // Show an error message if login fails
                        MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
        }
        }
    }
