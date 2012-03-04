using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace Nimbus.Controls
{
    [System.ComponentModel.DesignerCategory("code")]
    class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            this.BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)NativeMethods.WindowMessages.WM_XBUTTONDOWN:
                    MessageBox.Show("Clicked!");
                    break;
               
                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }
    }
    
}
