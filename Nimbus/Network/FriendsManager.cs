using System;
using System.Collections.Generic;
using System.Text;

namespace Nimbus.Network
{
    public class FriendsManager
    {

        public delegate void PopulateDelegate(string friends);

        public List<Friend> Friends;
        public FriendsList FriendsList;

        public FriendsManager()
        {
            FriendsList = new FriendsList(Factory.CurrentTheme);
            Friends = new List<Friend>();
            Factory.Nexoid.ConnectionChange += new EventHandler(Nexoid_ConnectionChange);
        }

        void Nexoid_ConnectionChange(object sender, EventArgs e)
        {
            if (Factory.Nexoid.Connected) Factory.Nexoid.SendToServer("FRIENDS");
        }

        public void StartPopulate(string friends)
        {
            PopulateDelegate p = new PopulateDelegate(Populate);
            p.BeginInvoke(friends, new AsyncCallback(PopulateComplete),null);
        }

        private void Populate(string friends)
        {
            string[] split = friends.Split('#');
            Friends.Clear();
            foreach (string s in split)
            {
                if (s.Length == 0) continue;
                Friend f = new Friend();
                f.Username = s;
                Friends.Add(f);

            }
        }

        public void SetStatus(string Username, string Status)
        {
            foreach (Friend f in Friends)
            {
                if (f.Username == Username) f.Status = Status;
            }
        }

        void PopulateComplete(IAsyncResult itfAR)
        {
            FriendsList.UpdateList();
        }
    }
}
