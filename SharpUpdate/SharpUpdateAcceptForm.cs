using System;
using System.Windows.Forms;

namespace SharpUpdate
{
    internal partial class SharpUpdateAcceptForm : Form
    {

        private SharpUpdateInterface info;

        private SharpUpdateXml updateInfo;

        private SharpUpdateInfoForm updateInfoForm;

        internal SharpUpdateAcceptForm(SharpUpdateInterface applicationInfo, SharpUpdateXml updateInfo)
        {
            InitializeComponent();

            this.info = applicationInfo;
            this.updateInfo = updateInfo;

            this.Text = this.info.ApplicationName + " - Update Available";

            if (this.info.ApplicationIcon != null) this.Icon = this.info.ApplicationIcon;

            this.lbNewVersion.Text = string.Format("New version: {0}", this.updateInfo.Version.ToString());
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (this.updateInfo == null) this.updateInfoForm = new SharpUpdateInfoForm(this.info, this.updateInfo);
        
            this.updateInfoForm.ShowDialog(this);
        }
    }
}
