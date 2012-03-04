using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nimbus.Theming;
using Nimbus.Controls;

namespace Nimbus
{
    [System.ComponentModel.DesignerCategory("form")]
    public partial class NimbusMessageBox : NimbusForm
    {

        static MessageBoxReturn toReturn;
        

        public NimbusMessageBox(NimbusTheme theme)
            :base(theme)
        {
            InitializeComponent();
            AllowResize = false;
            this.TopMost = true;
        }

        public static MessageBoxReturn AskQuestion(string question, string title)
        {
            NimbusMessageBox mb = new NimbusMessageBox(Factory.CurrentTheme);
            mb.lblText.Text = question;
            mb.Text = title;
            mb.ShowDialog();
            return toReturn;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            toReturn.clickedYes = false;
            toReturn.checkedBox = chkNeverShow.Checked;
            this.Dispose();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            toReturn.clickedYes = true;
            toReturn.checkedBox = chkNeverShow.Checked;
            this.Dispose();
        }

      
    }

    public struct MessageBoxReturn
    {

        public bool clickedYes;
        public bool checkedBox;

    }
}
