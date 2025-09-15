using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Grifindo_Lanka_system
{
    public partial class toyMan_form : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-Q7BFFGN;Initial Catalog=Grifindo_Payroll_System;Integrated Security=True;Encrypt=False");
        int id, m_year;
        string name, brand, price, catagory, m_country;

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                LoadData();
                string save = "insert into toyMan_tbl values ('" + id + "','" + name + "','" + brand + "','" + price + "','" + catagory + "','" + m_year + "','" + m_country + "')";
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
            LoadGridView();
        }

        private void toyMan_form_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-Q7BFFGN;Initial Catalog=Grifindo_Payroll_System;Integrated Security=True;Encrypt=False"))
            {
                conn.Open();

                string query = "SELECT ISNULL(MAX(toy_id), 0) FROM toyMan_tbl";

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

        public toyMan_form()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            id=int.Parse(txtid.Text);
            name=txtname.Text;
            brand=txtbrand.Text;
            price=txtprice.Text;
          
            m_year=int.Parse(txtyear.Text);
            m_country=txtcountry.Text;
            catagory=cmdcatagory.Text;

        }

        private void txtidsearch_TextChanged(object sender, EventArgs e)
        {
            TextboxFilter();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Cleardata();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            conn.Open();
            LoadData();
            String update = "update toyMan_tbl set toy_id = '" + id + "',name = '" + name + "',brand ='" + brand + "',price = '" + price + "',catagory='" + catagory + "',m_year='" + m_year + "',m_country='"+m_country+"' where toy_id = '" + id + "' ";
            SqlCommand cmd = new SqlCommand(update, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Your update was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            LoadGridView();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            int toyid = int.Parse(txtid.Text);
            conn.Open();
            string delete = "delete from toyMan_tbl where toy_id= '" + toyid + "'";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Your Delete was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            LoadGridView();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            leaveMan_adminForm lm = new leaveMan_adminForm();
            this.Hide();
            lm.Show();

        }

        private void tbemploye_Click(object sender, EventArgs e)
        {
            emp_manageForm frm = new emp_manageForm();
            this.Hide();
            frm.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadGridView()
        {
            conn.Open();
            string query = "select * from toyMan_tbl";
            SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dgvtoys.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }

        private void TextboxFilter()
        {
            try
            {
                conn.Open();

                string query = "SELECT * FROM toyMan_tbl WHERE toy_id = @toy_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@toy_id", txtidsearch.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtid.Text = reader["toy_id"].ToString();
                    txtname.Text = reader["name"].ToString();
                    txtbrand.Text = reader["brand"].ToString();
                    txtprice.Text = reader["price"].ToString();
                    cmdcatagory.Text = reader["catagory"].ToString();
                    txtyear.Text = reader["m_year"].ToString();
                    txtcountry.Text = reader["m_country"].ToString();
                    

                }
                else
                {
                    Cleardata();
                    toyMan_form pt = new toyMan_form();
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
            txtname.Text = "";
            txtbrand.Text = "";
            txtprice.Text = "";
            cmdcatagory.SelectedIndex = -1; // Clear the combo box selection
            txtyear.Text = "";
            txtcountry.Text = "";
        }

    }
}
