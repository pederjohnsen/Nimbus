using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Nimbus.NimbusControls
{
    public class NMenuItem : Control
    {
        public Game ForGame;
        EventHandler Clicked;
        bool selected = false;

        bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                Invalidate();
            }
        }

        public bool IsSeparator
        {
            get { return (Text == "-"); }
        }

        public NMenuItem(string text)
        {
            Text = text;
            ForeColor = Factory.CurrentTheme.CaptionBarColor;
            BackColor = Factory.CurrentTheme.BackgroundColor;
        }

        public NMenuItem(string text, EventHandler handler)
            
        {
            Text = text;
            Clicked = handler;
            ForeColor = Factory.CurrentTheme.CaptionBarColor;
            BackColor = Factory.CurrentTheme.BackgroundColor;
        }

        public NMenuItem(string text, EventHandler handler, Game game)
            
        {
            Text = text;
            Clicked = handler;
            ForGame = game;
            ForeColor = Factory.CurrentTheme.CaptionBarColor;
            BackColor = Factory.CurrentTheme.BackgroundColor;
            
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Selected = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Selected = false;
            base.OnLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
           if (Clicked != null) Clicked(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            

            if (IsSeparator)
            {
                e.Graphics.Clear(BackColor);
                e.Graphics.DrawLine(new Pen(ForeColor), new Point(10, 2), new Point(Width - 10, 2));

            }
            else
            {
                if (selected)
                {
                    ForeColor = Factory.CurrentTheme.BackgroundColor;
                    BackColor = Factory.CurrentTheme.CaptionBarColor;
                }
                else
                {
                    ForeColor = Factory.CurrentTheme.CaptionBarColor;
                    BackColor = Factory.CurrentTheme.BackgroundColor;
                }


                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;

                e.Graphics.Clear(BackColor);
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new RectangleF(new PointF(5, 0), new SizeF(Width - 5, Height)), sf);
            }
            base.OnPaint(e);

        }

        internal void PerformClick()
        {
            if (Clicked != null) Clicked(this, null);   
        }
    }
}
