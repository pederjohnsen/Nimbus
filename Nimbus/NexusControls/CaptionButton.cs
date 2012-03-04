using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Nimbus.Theming;
using Nimbus.Controls;

namespace Nimbus.Controls
{
    [System.ComponentModel.DesignerCategory("code")]
    class HoverButton : Control
    {

        public event EventHandler Clicked;
        private DrawType drawType;
        private ImageSet imageSet;
        private bool toggled;
        
        private List<HoverButton> toggleSet;
        public bool IsToggle { get; set; }
        public bool Toggled { get{
            return toggled;
        }
            set
            {
                bool oldval = toggled;
                toggled = value;
                
                if (toggled && toggleSet != null)
                {
                    foreach (HoverButton b in toggleSet)
                    {
                        if (b == this) continue;
                        b.Toggled = false;
                    }
                }
                if (oldval != toggled) Invalidate();
            }
        }

   
        public HoverButton()
        {
            
            DoubleBuffered = true;
                        
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.FromArgb(0, 0, 0, 0);
        }

        public void SetToggleSet(List<HoverButton> toggleSet)
        {
            this.toggleSet = toggleSet;
        }

        public void SetImageSet(ImageSet imageSet)
        {

            try
            {
                this.imageSet = imageSet;
                this.Width = this.imageSet.Normal.Width;
                this.Height = this.imageSet.Normal.Height;
            }
            catch
            {
                throw new ThemingError(this, "Imageset cannot be assigned");
            }
        }

        void SetStatus(DrawType type)
        {
            DrawType oldstat = drawType;
            drawType = type;
            if (type != oldstat) Invalidate();

        }

        void OnClicked(object sender, EventArgs e)
        {
            if (Clicked != null) Clicked(this, null);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetStatus(DrawType.eHover);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetStatus(DrawType.eNormal);
            Capture = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            SetStatus(DrawType.eHover);
            OnClicked(this, null);
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Toggled = true;
            SetStatus(DrawType.ePressed);
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                Pen p = new Pen(Color.Black);
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            }
            else
            {
                if (IsToggle && Toggled) imageSet.Draw(e.Graphics, new Rectangle(Point.Empty, Size), DrawType.ePressed);
                else
                {
                    if (imageSet == null)
                    {
                        Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
                        e.Graphics.DrawRectangle(Pens.Black, r);
                        e.Graphics.DrawLine(Pens.Black, r.Location, (Point)r.Size);
                        

                    }
                    else imageSet.Draw(e.Graphics, new Rectangle(Point.Empty, Size), drawType);

                }
            }
            base.OnPaint(e);
        }

    }
}
