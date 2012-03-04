using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nimbus.Theming;

namespace Nimbus
{
    [System.ComponentModel.DesignerCategory("form")]
    public partial class Slip : Form
    {
        
        
        private int waitTimer = 300;
        private string text = "This is example text";        
        
        public Slip(NimbusTheme theme)
            //:base(theme)
        {

            InitializeComponent();
        }

        public Slip(NimbusTheme theme, string text)
            //:base(theme)
        {
            InitializeComponent();
            this.ForeColor = Color.FromArgb(57, 116, 145);
            this.BackColor = theme.BackgroundColor;
            this.TopMost = true;
            this.text = text;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            
        }

        protected override void OnClick(EventArgs e)
        {
            waitTimer = 0;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            Height = 100;
            
            Location = new Point(Screen.FromControl(this).WorkingArea.Width - Width, Screen.FromControl(this).WorkingArea.Height - Height);
            NativeMethods.AnimateWindow(this.Handle, 500, (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE | (int)NativeMethods.AnimateWindowFlags.AW_VER_NEGATIVE | (int)NativeMethods.AnimateWindowFlags.AW_SLIDE);
            base.OnLoad(e);
        }

        int Clamp(int val, int min, int max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public void Tick()
        {
            waitTimer--;
            Opacity = Clamp((int)Opacity + 1, 0, 100);
            if (waitTimer <= 0) Close();

        }

        protected override void OnClosed(EventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 500, (int)NativeMethods.AnimateWindowFlags.AW_HIDE | (int)NativeMethods.AnimateWindowFlags.AW_VER_POSITIVE | (int)NativeMethods.AnimateWindowFlags.AW_SLIDE);
            base.OnClosed(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
           
            Pen p = new Pen(Color.FromArgb(57, 116, 145), 1.0f);
            e.Graphics.Clear(BackColor);
            e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            
            e.Graphics.DrawString(text, Font, new SolidBrush(ForeColor), new RectangleF(new PointF(10, 10), new SizeF(Width - 20, Height - 20)), sf);
            
            //base.OnPaint(e);
        }


        

       
    }
}
