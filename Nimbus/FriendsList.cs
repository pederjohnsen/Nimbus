using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nimbus.Network;
using Nimbus.Theming;
using Nimbus.Controls;

namespace Nimbus
{
    [System.ComponentModel.DesignerCategory("form")]
    public partial class FriendsList : NimbusForm
    {
        delegate void UIUpdateDelegate();
        public FriendsList(NimbusTheme theme)
            :base(theme)
        {
            InitializeComponent();
            //this.Show();
            //events
            lstFriends.MouseDoubleClick += new MouseEventHandler(lstFriends_MouseDoubleClick);
        }

        void lstFriends_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Factory.Nexoid.ShowChatSync((string)lstFriends.SelectedItem);
        }

        public void UpdateUI()
        {
            if (!this.Visible) return;
            lstFriends.Items.Clear();
            foreach (Friend f in Factory.FriendsMan.Friends)
            {
                lstFriends.Items.Add(f.Username);

            }
        }

        public void UpdateList()
        {
            UIUpdateDelegate UpdateHandler = new UIUpdateDelegate(UpdateUI);
            this.BeginInvoke(UpdateHandler);
        }
    }
}
