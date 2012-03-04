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
    abstract class HoverButton : Control
    {

        public event EventHandler Clicked;
        protected DrawType drawType;
        
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



        void SetStatus(DrawType type)
        {
            Console.WriteLine(type);
            DrawType oldstat = drawType;
            drawType = type;
            if (type != oldstat)
            {
                Invalidate();
               // Update();
            }

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
            
            base.OnPaint(e);
        }
        

    }
}
