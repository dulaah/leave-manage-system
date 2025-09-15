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
    public partial class employeDashboard_form : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-Q7BFFGN;Initial Catalog=Grifindo_System;Integrated Security=True;Encrypt=False");
        public static int emp;
        public employeDashboard_form()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            history_form history_Form = new history_form();
            history_form.jj = emp;
            this.Hide();
            history_Form.Show();    
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            leaveReq_form leaveReq_Form = new leaveReq_form();
            leaveReq_form.gg = emp;
            this.Hide();
            leaveReq_Form.Show();
        }

        private void employeDashboard_form_Load(object sender, EventArgs e)
        {
            txtempID.Text = emp.ToString();
            ShowLeaveCount();
            ShowRemainingAnnualLeaves();
            ShowRemainingCasuallLeaves();
            ShowRemainingshortlLeaves();
        }

        private void ShowLeaveCount()
        {
            // SQL query to count the number of leave records for the employee
            string query = "SELECT COUNT(*) FROM leave_Man_tbl WHERE id = '" + emp + "'";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-KM70SOPK\\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int leaveCount = (int)command.ExecuteScalar(); // Get the count result
                label13.Text =  leaveCount.ToString(); // Display in label13
            }
        }

        private void ShowRemainingAnnualLeaves()
        {
            int annualLeaveAllowance = 14; // Total annual leaves per employee
            string query = "SELECT COUNT(*) FROM leave_Man_tbl WHERE id = '" + emp + "' AND leave_type = 'Annual'";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-KM70SOPK\\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int usedAnnualLeaves = (int)command.ExecuteScalar(); // Count of used annual leaves
                int remainingAnnualLeaves = annualLeaveAllowance - usedAnnualLeaves; // Calculate remaining leaves
                label9.Text = remainingAnnualLeaves.ToString(); // Display in label9
            }
        }

        private void ShowRemainingCasuallLeaves()
        {
            int annualLeaveAllowance = 7; // Total casual leaves per employee
            string query = "SELECT COUNT(*) FROM leave_Man_tbl WHERE id = '" + emp + "' AND leave_type = 'Casual'";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-KM70SOPK\\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int usedAnnualLeaves = (int)command.ExecuteScalar(); // Count of used casual leaves
                int remainingAnnualLeaves = annualLeaveAllowance - usedAnnualLeaves; // Calculate remaining leaves
                label10.Text = remainingAnnualLeaves.ToString(); // Display in label10
            }
        }
        private void ShowRemainingshortlLeaves()
        {
            int annualLeaveAllowance = 24; // Total short leaves per employee
            string query = "SELECT COUNT(*) FROM leave_Man_tbl WHERE id = '" + emp + "' AND leave_type = 'Short'";

            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-KM70SOPK\\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int usedAnnualLeaves = (int)command.ExecuteScalar(); // Count of used short leaves
                int remainingAnnualLeaves = annualLeaveAllowance - usedAnnualLeaves; // Calculate remaining leaves
                label11.Text = remainingAnnualLeaves.ToString(); // Display in label11
            }
        }

    }
}
