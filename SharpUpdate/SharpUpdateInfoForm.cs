using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpUpdate
{
    public partial class SharpUpdateInfoForm : Form
    {
        public SharpUpdateInfoForm(SharpUpdateInterface info, SharpUpdateXml updateInfo)
        {
            InitializeComponent();

            if (info.ApplicationIcon != null)  this.Icon = info.ApplicationIcon;

            this.Text = info.ApplicationName + " - Update Info";
            this.lbVersion.Text = String.Format("Current Version: {0}\nUpdate Version: {1}", info.ApplicationAssembly.GetName().Version.ToString(), updateInfo.Version.ToString());
            this.txtDescription.Text = updateInfo.Description;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Control && e.KeyCode == Keys.C)) e.SuppressKeyPress= true;
        }
    }
}
