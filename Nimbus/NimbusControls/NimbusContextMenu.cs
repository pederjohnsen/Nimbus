using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Nimbus.Theming;

namespace Nimbus.NimbusControls
{
    [System.ComponentModel.DesignerCategory("code")]
    public class NimbusContextMenu : Form
    {
        public List<NMenuItem> MenuItems;
        NimbusTheme theme;

        int selectedIndex = -1;

        int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                int oldval = selectedIndex;
                selectedIndex = value;
                if (oldval != selectedIndex) OnSelectedChange();

            }
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

        private void OnSelectedChange()
        {
            Console.WriteLine(SelectedIndex);
            Invalidate();
        }

        public NimbusContextMenu(NimbusTheme theme)
        {
            this.theme = theme;
            MenuItems = new List<NMenuItem>();
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            TopMost = true;
            Font = new Font(theme.CaptionFontFamily, theme.CaptionFontSize);
            DoubleBuffered = true;
            ShowInTaskbar = false;
            this.Deactivate += new EventHandler(NimbusContextMenu_Deactivate);

        }

        public void Add(NMenuItem item)
        {
            MenuItems.Add(item);
            item.MouseClick += new MouseEventHandler(item_MouseClick);
            Controls.Add(item);
        }

        void item_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
        }

        void NimbusContextMenu_Deactivate(object sender, EventArgs e)
        {

            this.Hide();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {

                case (int)NativeMethods.WindowMessages.WM_ACTIVATEAPP:
                    {
                        if (m.WParam.ToInt32() == 0) this.Hide();
                        break;
                    }
       

                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //   /* if (e.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        if (SelectedIndex > -1 && SelectedIndex < MenuItems.Count)
        //        {
        //            MenuItems[SelectedIndex].PerformClick();
        //            this.Hide();
        //        }
        //    }
        //    * */
        //    base.OnMouseClick(e);
        //}

        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    SelectedIndex = -1;
        //    base.OnMouseLeave(e);
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    SelectedIndex = (int)Math.Floor((double)(e.Y / (Height / MenuItems.Count)));
        //    base.OnMouseMove(e);
        //}

        protected override void OnShown(EventArgs e)
        {
            Focus();
            base.OnShown(e);
        }

        void SetSizeAndPos()
        {
            Size = new Size(200, MenuItems.Count * (Font.Height + 4));
            int y;
            if (Cursor.Position.Y < Screen.FromControl(this).Bounds.Height - Size.Height) y = Cursor.Position.Y;
            else y = Cursor.Position.Y - Size.Height;
            Location = new Point(Cursor.Position.X, y);
            if (Location.X + Width > Screen.FromControl(this).Bounds.Width) Location = new Point(Screen.FromControl(this).Bounds.Width - Size.Width, Location.Y);
            int totalheight = 0;
            for (int i = 0; i < MenuItems.Count; i++)
            {
                if (i == 0) MenuItems[i].Location = new Point(0, 0);
                else MenuItems[i].Location = new Point(0, MenuItems[i - 1].Location.Y + MenuItems[i - 1].Height); 
                if (MenuItems[i].IsSeparator) MenuItems[i].Size = new Size(Width, 5);
                else MenuItems[i].Size = new Size(Width, (Font.Height + 6));
                totalheight += MenuItems[i].Height;
            }

            this.Height = totalheight;
            

        }

        protected override void OnLoad(EventArgs e)
        {
            SetSizeAndPos();
            base.OnLoad(e);
        }

        public void ShowMenu()
        {
            SetSizeAndPos();
            Show();
            //MenuItems[0].Focus();
            Activate();
        }
        


        protected override void OnLostFocus(EventArgs e)
        {
            
           // this.Hide();
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(theme.BackgroundColor);
            //e.Graphics.DrawRectangle(new Pen(theme.CaptionBarColor), new Rectangle(0, 0, Width - 1, Height - 1));
           
            
            base.OnPaint(e);
        }

    }
}
