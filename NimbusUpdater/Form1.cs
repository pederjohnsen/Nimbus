using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;


namespace NimbusUpdater
{
    public partial class Form1 : Form
    {
        string updateServer = "http://svn.thethoughtradar.com/nimbusupdate/";
        int progress;

        public Form1()
        {
            InitializeComponent();
            UpdateFiles();
        }

        private void UpdateUI()
        {
            progressBar1.Value = progress;
            if (progress == 100) label1.Text = "Extracting Files";
        }

        private bool ValidateUpdate()
        {
            return true; //TODO!
        }

        private void Finish()
        {
            label1.Text = "Validating";
            if (ValidateUpdate())
            {
                Process proc = new Process();
                proc.StartInfo.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\nimbus.exe";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                Close();
            }
        }

        private void UpdateFiles()
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadFileAsync(new Uri(updateServer + "latest.zip"), Path.GetDirectoryName(Application.ExecutablePath) + "\\latest.zip");

        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ZipFile zf = new ZipFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\latest.zip");
            zf.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zf_ExtractProgress);
            zf.ExtractAll(Path.GetDirectoryName(Application.ExecutablePath), ExtractExistingFileAction.OverwriteSilently);
        }

        void zf_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
            {
                this.BeginInvoke(new MethodInvoker(Finish));
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progress = e.ProgressPercentage;
            this.BeginInvoke(new MethodInvoker(UpdateUI));
        }
    }
}
