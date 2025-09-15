using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Grifindo_Lanka_system
{
    public partial class reports_form : KryptonForm
    {
        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-KM70SOPK\SQLEXPRESS;Initial Catalog=Grifindo_System;Integrated Security=True");
        public reports_form()
        {
            InitializeComponent();
        }


        private void LoadGridView()
        {
            //conn.Open();
            //string query = "SELECT id, leave_type,status FROM Leave_Man_tbl WHERE status = 'Approved'";
            //SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            //DataSet ds = new DataSet();
            //adapt.Fill(ds);
            //dgvreport.DataSource = ds.Tables[0].DefaultView;
            //conn.Close();


            conn.Open();
            string query = @"
    SELECT id,
           leave_type,
           status,
           Start_date,
           CASE 
               WHEN leave_type IN ('Annual', 'Casual') THEN End_date
               ELSE NULL
           END AS End_date,
           CASE 
               WHEN leave_type = 'Short' THEN time
               ELSE NULL
           END AS time
    FROM Leave_Man_tbl
    WHERE status = 'Approved'";

            SqlDataAdapter adapt = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            dgvreport.DataSource = ds.Tables[0].DefaultView;
            conn.Close();

            // Change the column header for 'id' to 'Employee ID'
            dgvreport.Columns["id"].HeaderText = "Employee ID";

            // Highlight the 'status' column to indicate it's "Approved"
            dgvreport.Columns["status"].DefaultCellStyle.BackColor = Color.LightGreen;
            //dgvreport.Columns["status"].DefaultCellStyle.Font = new Font(dgvreport.DefaultCellStyle.Font, FontStyle.Bold);


        }

        private void reports_form_Load(object sender, EventArgs e)
        {
            LoadGridView();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            leaveMan_adminForm lv = new leaveMan_adminForm();
            this.Hide();
            lv.Show();
        }

        private void tbemploye_Click(object sender, EventArgs e)
        {
            emp_manageForm emp = new emp_manageForm();
            this.Hide();
            emp.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminDashboard_form emp = new adminDashboard_form();
            this.Hide();
            emp.Show();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (dgvreport.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Filter = "PDF (*.pdf)|*.pdf",
                    FileName = "Result.pdf"
                };

                if (save.ShowDialog() == DialogResult.OK)
                {
                    bool errorOccurred = false;

                    try
                    {
                        if (File.Exists(save.FileName))
                        {
                            File.Delete(save.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorOccurred = true;
                        MessageBox.Show($"Unable to write data to disk: {ex.Message}");
                    }

                    if (!errorOccurred)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dgvreport.Columns.Count)
                            {
                                DefaultCell = { Padding = 2 },
                                WidthPercentage = 100,
                                HorizontalAlignment = Element.ALIGN_LEFT
                            };

                            // Add Header Row
                            foreach (DataGridViewColumn col in dgvreport.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pCell.BackgroundColor = BaseColor.LIGHT_GRAY; // Highlight headers
                                pTable.AddCell(pCell);
                            }

                            // Add Data Rows
                            foreach (DataGridViewRow row in dgvreport.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    PdfPCell pCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty));

                                    // Check if the column is "Status" and apply background color
                                    if (dgvreport.Columns[cell.ColumnIndex].HeaderText.Equals("Status", StringComparison.OrdinalIgnoreCase))
                                    {
                                        pCell.BackgroundColor = BaseColor.YELLOW; // Highlight color for the "Status" column
                                    }

                                    pTable.AddCell(pCell);
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            using (Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f))
                            {
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                            }

                            MessageBox.Show("Data exported successfully.", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error while exporting data: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No records found.", "Info");
            }
        }
    }
}
