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
    public partial class loginRole_form : KryptonForm
    {
        public loginRole_form()
        {
            InitializeComponent();
        }

        private void pbadmin_Click(object sender, EventArgs e)
        {
            admin_login admin_Login = new admin_login();
            this.Hide();
            admin_Login.Show();
        }

        private void pbemp_Click(object sender, EventArgs e)
        {
            empLogin_form el=new empLogin_form();
            el.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
