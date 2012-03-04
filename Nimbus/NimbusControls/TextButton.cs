using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Nimbus.Theming;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Nimbus.Controls
{
    class TextButton : HoverButton
    {

        public int YOffset { get; set; }
        public bool HasShadow { get; set; }
        public int Tracking { get; set; }

        public TextButton()
            : base()
        {
            

        }

        public void SetTheme(NimbusTheme theme)
        {
            Font = new Font(theme.ButtonFont, 34f, FontStyle.Bold);
        }

        void DrawStringWithSpacing(string text, Graphics g, float spacing, Brush brush, float x, float y)
        {
            //SizeF size = g.MeasureString("A", font);
            char[] chars = text.ToCharArray();
            float pos = 0.0f;
            for (int i = 0; i < text.Length; ++i)
            {
                string charToDraw = new string(chars[i], 1);
                g.DrawString(charToDraw, Font, brush, pos + x, y);
                SizeF sizeChar = g.MeasureString(charToDraw, Font);
                int wid = DrawingUtils.MeasureDisplayStringWidth(g, charToDraw, Font);
                pos += wid + spacing;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;

            
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Color toUse = Color.White;

            if (drawType == DrawType.eHover) toUse = Color.FromArgb(178, 204, 217);
            if (drawType == DrawType.eNormal) toUse = Color.White;
            if (drawType == DrawType.ePressed) toUse = Color.FromArgb(182, 130, 99);
            if (IsToggle && Toggled) toUse = Color.FromArgb(182, 130, 99);

            

            Brush b = new SolidBrush(Color.Black);
            Brush f = new SolidBrush(toUse);
            
           /* IntPtr hDC = e.Graphics.GetHdc();

            SetTextCharacterExtra(hDC, -2);

            IDeviceContext HDC = System.Drawing.Graphics.FromHdc(hDC);
            Graphics g = System.Drawing.Graphics.FromHdc(hDC);
            g.DrawString(Text, Font, b, new System.Drawing.PointF(3, 3));
            //TextRenderer.DrawText(HDC, Text, Font, new Point(3, 3), Color.Black);
            //TextRenderer.DrawText(HDC, Text, Font, new Point(0, 0), ForeColor);

            e.Graphics.ReleaseHdc(hDC);

            //e.Graphics.DrawString(Text, Font, b, new System.Drawing.PointF(3, 3));
            //e.Graphics.DrawString(Text, Font, f, new System.Drawing.PointF(0, 0));
            */


            if (HasShadow) DrawStringWithSpacing(Text, g, Tracking, b, 3.0f, 3.0f - YOffset);
            DrawStringWithSpacing(Text, g, Tracking, f, 0.0f, 0.0f - YOffset);
            f.Dispose();
            b.Dispose();
            base.OnPaint(e);
        }

    }
}
