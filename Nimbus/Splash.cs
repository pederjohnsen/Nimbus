using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nimbus.Theming;
using System.Diagnostics;

namespace Nimbus
{
    public partial class Splash : Form
    {
        NimbusTheme Theme;
        Image bg;
        string versionText = "Version: 0.5.0.0";
        public Splash(NimbusTheme Theme)
        {
            this.ShowInTaskbar = false;
            this.Theme = Theme;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            bg = Image.FromFile(Theme.SplashFile);

            versionText = String.Format("Version: {0}", FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion);
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (bg != null)
            {
                Size = bg.Size;
            }
            NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE);
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {

            base.OnShown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_HIDE);
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            bg.Dispose();
            base.OnClosed(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
             base.OnPaint(e);
             if (!DesignMode) e.Graphics.DrawImage(bg,new Rectangle(0,0,Width,Height));
             int x = DrawingUtils.MeasureDisplayStringWidth(e.Graphics, versionText, this.Font);
             e.Graphics.DrawString(versionText, this.Font, Brushes.White, new PointF(Width - x - 10, Height - this.Font.Height - 10));
        }
    }
}
