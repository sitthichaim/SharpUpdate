using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace SharpUpdate
{
    internal partial class SharpUpdateDownloadForm : Form
    {
        private WebClient webClient;

        private BackgroundWorker bgWorker;

        private string tmpFile;

        private string md5;

        internal string TempFilePath
        {
            get { return tmpFile; }
        }


        internal SharpUpdateDownloadForm(Uri location, string md5, Icon programIcon)
        {
            InitializeComponent();

            if (programIcon != null) 
                this.Icon = programIcon;

            tmpFile = Path.GetTempFileName();

            this.md5 = md5;

            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressCharged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
        
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                string file = ((string[]) e.Argument)[0];
                string updateMd5 = ((string[]) e.Argument)[1];

                Debug.WriteLine("file -----------> " + file);
                Debug.WriteLine("updateMd5 -----------> " + updateMd5);

                if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
                {
                    Debug.WriteLine("----------- IF -----------");
                    e.Result = DialogResult.No;
                }
                else
                {
                    e.Result = DialogResult.OK;
                }

            });
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompletedEventHandler);
       
            try
            {
                webClient.DownloadFileAsync(location, this.tmpFile);  
            }
            catch
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
        
        }

        private void bgWorker_RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = (DialogResult)e.Result;
            this.Close();
        }

        private void bgWorker_DoWorkEventHandler(object sender, DoWorkEventArgs e)
        {
            string file = ((string[]) e.Argument)[0];
            string updateMd5 = ((string[]) e.Argument)[1];

            Debug.WriteLine("file -----------> " + file);
            Debug.WriteLine("updateMd5 -----------> " + updateMd5);

            if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
            {
                Debug.WriteLine("----------- IF -----------");
                e.Result = DialogResult.No;
            }
            else
            {
                e.Result = DialogResult.OK;
            }
        }

        private void webClient_DownloadProgressCharged(object sender, DownloadProgressChangedEventArgs e)
        {
            Debug.WriteLine("e.ProgressPercentage -----------> " + e.ProgressPercentage);
            Debug.WriteLine("e.BytesReceived -----------> " + e.BytesReceived);
            this.progressBar.Value = e.ProgressPercentage;
            this.lbProgress.Text = String.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived, 1, true), FormatBytes(e.TotalBytesToReceive, 1, true));

        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else if (e.Cancelled)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
            else
            {
                this.lbProgress.Text = "Verifying Download...";
                progressBar.Style = ProgressBarStyle.Marquee;

                bgWorker.RunWorkerAsync(new string[] { this.tmpFile, this.md5 });
            }
        }

        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType)
        {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            if (newBytes > 1024 && newBytes < 1048576)
            {
                newBytes /= 1024;
                byteType = "KB";
            }
            else if (newBytes > 1048576 && newBytes < 1073741824)
            {
                newBytes /= 1048576;
                byteType = "MB";
            }
            else
            {
                newBytes /= 1073741824;
                byteType = "GB";
            }

            if (decimalPlaces > 0) formatString += ":0.";

            for (int i = 0; i < decimalPlaces; i++) formatString += "0";

            formatString += "}";

            if (showByteType) formatString += byteType;
            return String.Format(formatString, newBytes);
        }

        private void SharpUpdateDownloadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
                this.DialogResult = DialogResult.Abort;
            }

            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
                this.DialogResult=DialogResult.Abort;
            }
        }
    }
}
