using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Nimbus.Controls;
using Nimbus.Theming;

namespace Nimbus.Controls
{

       
    struct MinMaxInfo 
    {
        public Point ptReserved;
        public Point ptMaxSize;
        public Point ptMaxPosition;
        public Point ptMinTrackSize;
        public Point ptMaxTrackSize;
    }
    

    [System.ComponentModel.DesignerCategory("code")]
    public class NimbusForm : Form
    {
        public NimbusTheme Theme;
        private bool hasCaptionBar = true;
        private Font captionFont; 
        private int captionHeight = 25;
        private int border = 3;
        private bool allowResize = true;

        public int CaptionHeight
        {
            get { return captionHeight; }

        }

        public bool HasCaptionBar
        {
            get { return hasCaptionBar; }
            set { hasCaptionBar = value; }
        }


        public bool AllowResize
        {
            get { return allowResize; }
            set { allowResize = value; }
        }

        public Font CaptionFont
        {
            get { return captionFont; }
            set { captionFont = value; }
        }

        public NimbusForm()
            :base()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.DoubleBuffered = true;
        }

        public NimbusForm(NimbusTheme theme)
        {
            this.Theme = theme;
            this.Icon = theme.Icon;
            this.BackColor = theme.BackgroundColor;
            this.CaptionFont = new Font(theme.CaptionFontFamily, theme.CaptionFontSize);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           this.DoubleBuffered = true;
        }


        private Rectangle GetIconRectangle()
        {
            return new Rectangle(3, 3, 16, 16);
        }

        public Point PointToWindow(Point screenPoint)
        {
            return new Point(screenPoint.X - Location.X, screenPoint.Y - Location.Y);
        }
        protected override void OnResize(EventArgs e)
        {
            
            Invalidate();
            base.OnResize(e);
        }

        private const int CS_DROPSHADOW = 0x00020000;

        // Override the CreateParams property
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

      
        private void WmNCHitTest(ref Message m)
        {
            
            Point screenPoint = new Point(m.LParam.ToInt32());
            
            // convert to local coordinates
            Point clientPoint = PointToWindow(screenPoint);
           
            //m.Result = new System.IntPtr(OnNonClientAreaHitTest(clientPoint));
            m.Result = new IntPtr(OnNonClientAreaHitTest(clientPoint));
        }

        protected virtual int OnNonClientAreaHitTest(Point p)
        {
            //Console.WriteLine(clientPoint.Y);

            if (AllowResize && WindowState != FormWindowState.Maximized)
            {
                #region Handle sizable window borders
                if (p.X <= border) // left border
                {
                    if (p.Y <= border)
                        return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
                    else if (p.Y >= this.Height - border)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTLEFT;
                }
                else if (p.X >= this.Width - border) // right border
                {
                    if (p.Y <= border)
                        return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
                    else if (p.Y >= this.Height - border)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTRIGHT;
                }
                else if (p.Y <= border) // top border
                {
                    if (p.X <= border)
                        return (int)NativeMethods.NCHITTEST.HTTOPLEFT;
                    if (p.X >= this.Width - border)
                        return (int)NativeMethods.NCHITTEST.HTTOPRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTTOP;
                }
                else if (p.Y >= this.Height - border) // bottom border
                {
                    if (p.X <= border)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMLEFT;
                    if (p.X >= this.Width - border)
                        return (int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT;
                    else
                        return (int)NativeMethods.NCHITTEST.HTBOTTOM;
                }
                #endregion
            }
            if (GetIconRectangle().Contains(p)) return (int)NativeMethods.NCHITTEST.HTSYSMENU;

            if (p.Y < captionHeight) return (int)NativeMethods.NCHITTEST.HTCAPTION;
           
            return (int)NativeMethods.NCHITTEST.HTCLIENT;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
			{
                
				case (int)NativeMethods.WindowMessages.WM_NCHITTEST:
                    {
                    WmNCHitTest(ref m);
                    break;
                    }
                case (int)NativeMethods.WindowMessages.WM_GETMINMAXINFO:
                    {
                    base.WndProc(ref m);
                    MinMaxInfo mmi = (MinMaxInfo)Marshal.PtrToStructure(m.LParam, typeof(MinMaxInfo));
                    mmi.ptMaxPosition = Screen.FromControl(this).WorkingArea.Location;
                    mmi.ptMaxSize = (Point)Screen.FromControl(this).WorkingArea.Size;
                    
                    Marshal.StructureToPtr(mmi, m.LParam, false);
                    return;
                    }

                default:
                    {
                    base.WndProc(ref m);
                    break;
                    }
            }
    
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                
                e.Graphics.Clear(Color.LightGray);
                if (hasCaptionBar) e.Graphics.FillRectangle(Brushes.DarkGray,new Rectangle(0,0,Width, CaptionHeight));
                return;
            }

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            
            Pen p = new Pen(Theme.CaptionTextColor);
            if (HasCaptionBar)
            {
                SolidBrush b = new SolidBrush(Theme.CaptionBarColor);
                e.Graphics.FillRectangle(b, new Rectangle(0, 0, Width, captionHeight));
                e.Graphics.DrawIcon(this.Icon, new Rectangle(4, 4, 16, 16));
                int stringWidth = DrawingUtils.MeasureDisplayStringWidth(e.Graphics, Text, captionFont);
                e.Graphics.DrawString(this.Text, captionFont, p.Brush, new PointF(Theme.CaptionOffsetX, Theme.CaptionOffsetY));
            }
                                 

            base.OnPaint(e);
        }

       


    }
}