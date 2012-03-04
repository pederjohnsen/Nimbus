using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nimbus
{
    public partial class ChatWindow : Form
    {

        private string _user;
        

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }


        protected override void OnClosed(EventArgs e)
        {
            Factory.Nexoid.Chats.Remove(this);
        }

        public ChatWindow()
        {
            InitializeComponent();
        }

        public ChatWindow(string user)
        {
            InitializeComponent();
            this.User = user;
         
        }

        public void Flash()
        {


        }

        public ChatWindow(string user, string message)
        {
            InitializeComponent();
            this.User = user;
            AddLine(user, message);
        }

        public void AddLine(String From, String Message)
        {
            txtChat.Text = txtChat.Text + String.Format("{0}: {1}", From, Message) + Environment.NewLine;

        }

        private void nexusText1_PressedEnter(object sender, EventArgs e)
        {
            String format = String.Format("CHAT {0} {1}", this.User, nexusText1.Text);
            AddLine(Factory.Nexoid.User, nexusText1.Text);
            Factory.Nexoid.SendToServer(format);
            nexusText1.Text = "";
        }
    }
}
