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
namespace Grifindo_Lanka_system

{
    public partial class emp_manageForm : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        int id;
        string name, nic, gen, num, address, sal, role, us, pw;
        

        private void btndelete_Click(object sender, EventArgs e)
        {
            int emid = int.Parse(txtid.Text);
            conn.Open();
            string delete = "delete from empReg_tbl where id= '" + emid + "'";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show(" Delete was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            LoadGridView();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
          
                conn.Open();
                LoadData();
                String update = "update empReg_tbl set id = '" + id + "',emp_name = '" + name + "',nic ='" + nic + "',date_of_birth = '" + dob + "',con_num='" + num + "',addrres='" + address + "',basic_salary='"+sal+"',role='"+role+"',username='"+us+"',passwrod='"+pw+"' where id = '" + id + "' ";
                SqlCommand cmd = new SqlCommand(update, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Your update was successful! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
                LoadGridView();
            
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                LoadData();
                string save = "insert into empReg_tbl values ('" + id + "','" + name + "','" + nic + "','"+gen+"','" + dob + "','" + num + "','" + address + "','"+sal+"','"+role+"','"+us+"','"+pw+"')";
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

        private void txtidsearch_TextChanged(object sender, EventArgs e)
        {
            TextboxFilter();
        }

        private void LoadData()
        {
            id=int.Parse(txtid.Text);
            name=txtname.Text;
            nic=txtnic.Text;
            gen=txtgender.Text;
            num=txtnum.Text;
            address=txtaddress.Text;
            sal=txtsal.Text;
            role=txtrole.Text;
            us=txtus.Text;
            pw=txtpw.Text;
            dob = dtpdob.Value.Date;

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            leaveMan_adminForm lm=new leaveMan_adminForm();
            this.Hide();
            lm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminDashboard_form ad=new adminDashboard_form();
            this.Hide();
            ad.Show();
        }

        private void emp_manageForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True"))
            {
                conn.Open();

                string query = "SELECT ISNULL(MAX(id), 0) FROM empReg_tbl";

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

        DateTime dob;
        public emp_manageForm()
        {
            InitializeComponent();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pbtoys_Click(object sender, EventArgs e)
        {
            toyMan_form tm=new toyMan_form();
            this.Hide();
            tm.Show();
        }
        private void TextboxFilter()
        {
            try
            {
                conn.Open();

                string query = "SELECT * FROM empReg_tbl WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", txtidsearch.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtid.Text = reader["id"].ToString();
                    txtname.Text = reader["emp_name"].ToString();
                    txtnic.Text = reader["nic"].ToString();
                    txtgender.Text = reader["gender"].ToString();
                    dtpdob.Text = reader["date_of_birth"].ToString();
                    txtnum.Text = reader["con_num"].ToString();
                    txtaddress.Text = reader["addrres"].ToString();
                    txtsal.Text = reader["basic_salary"].ToString();
                    txtrole.Text = reader["role"].ToString();
                    txtus.Text = reader["username"].ToString();
                    txtpw.Text = reader["passwrod"].ToString();
                }
                else
                {
                    Cleardata();
                    emp_manageForm pt = new emp_manageForm();
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
            txtnic.Text = "";
            txtgender.Text = "";
            dtpdob.Text = "";
            txtnum.Text = "";
            txtaddress.Text = "";
            txtsal.Text = "";
            txtrole.Text = "";
            txtus.Text = "";
            txtpw.Text = "";

        }

        private void LoadGridView()
        {
            conn.Open();
            string query = "select * from empReg_tbl";
            SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dgvemp.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
    }
}
