using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SharpUpdate
{
    public class SharpUpdater
    {
        private SharpUpdateInterface info;
        private BackgroundWorker worker;

        public SharpUpdater(SharpUpdateInterface info)
        {
            this.info = info;
            this.worker = new BackgroundWorker();
            this.worker.DoWork += new DoWorkEventHandler(bg_DoWorkEventHandler);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompletedEventHandler);
        }

        public void DoUpdate()
        {
            if (!this.worker.IsBusy) this.worker.RunWorkerAsync(this.info);
        }

        private void bg_DoWorkEventHandler(object sender, DoWorkEventArgs e)
        {
            SharpUpdateInterface app = (SharpUpdateInterface) e.Argument;

            if (!SharpUpdateXml.ExitsOnServer(app.UpdateXmlLocation)) e.Cancel = true;
            else e.Result = SharpUpdateXml.Parse(app.UpdateXmlLocation, app.ApplicationId);
        }

        private void bg_RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                SharpUpdateXml update = (SharpUpdateXml)e.Result;

                if (update != null && update.InNewerThan(this.info.ApplicationAssembly.GetName().Version))
                {
                    if (new SharpUpdateAcceptForm(this.info, update).ShowDialog(this.info.Context) == DialogResult.Yes)
                        this.DownloadUpdate(update);
                }
            }
        }

        private void DownloadUpdate(SharpUpdateXml update)
        {
            SharpUpdateDownloadForm form = new SharpUpdateDownloadForm(update.Uri, update.MD5, this.info.ApplicationIcon);
            DialogResult result = form.ShowDialog(this.info.Context);

            if (result == DialogResult.OK)
            {
                string currentPath = this.info.ApplicationAssembly.Location;
                string newPath = Path.GetDirectoryName(currentPath) + "\\" + update.FileName;

                UpdateApplication(form.TempFilePath, currentPath, newPath, update.LaunchArgs);

                Application.Exit();
            }
            else if (result == DialogResult.Abort)
            {
                MessageBox.Show("The update download was cancelled.\nThis program has not been modified.", "Update Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                MessageBox.Show("There was a problem downloading the update.\nPlease try again latter.", "Update Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateApplication(string tempFilePath, string currentPath, string newPath, string launchArgs)
        {
            string argument = "/C Choice /C Y /N /D Y /T 4 & Del /F /Q \"{0}\" & /C Choice /C Y /N /D Y /T 2 & Move /Y \"{1}\" \"{2}\" & Start \"\" /D \"{3}\" \"{4}\" {5}";

            ProcessStartInfo inf = new ProcessStartInfo();
            inf.Arguments = string.Format(argument, currentPath, tempFilePath, newPath, Path.GetDirectoryName(newPath), Path.GetFileName(newPath), launchArgs;
            inf.WindowStyle = ProcessWindowStyle.Hidden;
            inf.CreateNoWindow = true;
            inf.FileName = "cmd.exe";
            Process.Start(inf);
        }
    }
}
