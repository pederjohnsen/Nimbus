using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WebKit;

namespace Nimbus.Controls
{
    class MouseArgs : EventArgs
    {
        public int Button;

        public MouseArgs(int Button)
        {
            this.Button = Button;
        }
       
    }

    [System.ComponentModel.DesignerCategory("code")]
    class BorderlessWebkit : WebKit.WebKitBrowser
    {
        private Color borderColor = Color.Red;

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }


        public BorderlessWebkit()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
             
        }
       
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                
                case (int)NativeMethods.WindowMessages.WM_PAINT:

                    base.WndProc(ref m);

                    OnPaint();

                    break;

                default:

                    base.WndProc(ref m);

                    break;

            }
            
        }

        private void OnPaint()
        {

            Rectangle rcItem = new Rectangle(0, 0, this.Bounds.Width - 1, this.Bounds.Height - 1);

            IntPtr hDC = NativeMethods.GetWindowDC(this.Handle);

            Graphics g = Graphics.FromHdc(hDC);

            g.DrawRectangle(new Pen(BorderColor), rcItem);

            g.Dispose();

        }

    }
}
