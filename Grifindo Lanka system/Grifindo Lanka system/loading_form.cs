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

namespace Grifindo_Lanka_system
{
    public partial class loading_form : KryptonForm
    {
        public loading_form()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pbload.Value < 100)
            {
                pbload.Value += 2;
                label2.Text = pbload.Value.ToString() + "%";
            }
            else
            {
                timer1.Stop();
            }
            if (pbload.Value >= 100)
            {
                timer1.Stop();
                loginRole_form login_Role = new loginRole_form();
                this.Hide();
                login_Role.Show();
            }
        }

        private void loading_form_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void pbload_Paint(object sender, PaintEventArgs e)
        {
            ProgressBar progressBar = sender as ProgressBar;
            if (progressBar != null)
            {
                // Calculate the width of the filled section based on the current progress
                int progressWidth = (int)(progressBar.Width * ((double)progressBar.Value / progressBar.Maximum));

                // Define the orange color and fill the rectangle to represent progress
                e.Graphics.FillRectangle(Brushes.Orange, 0, 0, progressWidth, progressBar.Height);
            }
        }


    }
}
