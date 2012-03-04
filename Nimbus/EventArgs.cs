using System;
using System.Collections.Generic;
using System.Text;

namespace Nimbus
{
    public class GameClickEventArgs : EventArgs
    {
        public GameClickEventArgs(Game g)
        {
            GameClicked = g;
        }

        public Game GameClicked;
    }
}
