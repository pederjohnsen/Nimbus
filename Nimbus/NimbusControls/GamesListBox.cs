using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Nimbus.Theming;
using Nimbus.NimbusControls;

namespace Nimbus.Controls
{
    [System.ComponentModel.DesignerCategory("code")]
    public partial class GamesListBox : UserControl
    {

        private GameList<Game> games;
        private int selectedIndex  = -1;
        private Color selectionColor = Color.FromArgb(60,60,60);
        private Font statusFont;
        private Font titleFont;
        private float offset = 0;
        private float targetOffset = 0;
        private int margin = 0;

        private Timer timer;
        private NimbusTheme theme;
        public NimbusContextMenu ContextMenu;
        
     
        public event EventHandler SelectedItemChanged;

        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        public Font StatusFont
        {
            get { return statusFont; }
            set { statusFont = value; }
        }

        public Color SelectionColor
        {
            get { return selectionColor; }
            set { selectionColor = value; }
        }

        public int InternalMargin
        {
            get { return margin; }
            set { margin = value; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameList<Game> Games
        {
            get { return games; }
            set { games = value; }
        }
        

        public GamesListBox()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            timer = new Timer();
            timer.Interval = 1000 / 40;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            games = new GameList<Game>();
            //Paint += new PaintEventHandler(GamesListBox_Paint);
            
            MouseDown += new MouseEventHandler(GamesListBox_MouseDown);
            vScrollBar1.ValueChanged += new EventHandler(vScrollBar1_ValueChanged);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(targetOffset - offset) > 1)
            {
                float dy = targetOffset - offset;
                offset += dy / 8;
                if (Math.Abs(dy) < 1) offset = targetOffset;
                Invalidate();
                Console.WriteLine("Games List Invalidate {0}", DateTime.Now.Second);
            }
        }

        public Game GetSelectedGame()
        {
            if (selectedIndex == -1) return null;
            else return games[selectedIndex];

        }

        public void SetTheme(NimbusTheme theme)
        {
            this.theme = theme;
        }

        void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (vScrollBar1.Value < 0) targetOffset = 0;
            else targetOffset = vScrollBar1.Value * 56;
            Invalidate();
        }

        void GamesListBox_MouseDown(object sender, MouseEventArgs e)
        {

       
            
            
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int index = (e.Y + (int)offset) / 56;
            if (index + 1 > games.Count) return;
            int oldindex = selectedIndex;
            //if (index == oldindex) return;
            selectedIndex = index;
            OnItemChange();
            Invalidate();
            Update();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu.ShowMenu();
            }
            base.OnMouseClick(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();

        }

        private void OnItemChange()
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this,null);
        }

        private void HandleOffset()
        {
            bool needScrollBars = (games.Count * 56 > Height);
            vScrollBar1.Visible = needScrollBars;
            if (vScrollBar1.Visible)
            {
                vScrollBar1.Height = Height;
                vScrollBar1.Maximum = games.Count - (Height / 56);
                vScrollBar1.Minimum = 0;
                vScrollBar1.SmallChange = 1;
                vScrollBar1.LargeChange = 1;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (selectedIndex > games.Count - 1) selectedIndex = -1;
            if (DesignMode)
            {
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(2 + margin, 0 * 52 + 1 - (int)offset + margin, 49, 49));
                Brush b = new SolidBrush(this.ForeColor);
                e.Graphics.DrawString("Unselected Title", titleFont, b, new PointF(55.0f + margin, 0 * 52.0f + 10.0f + margin));
                e.Graphics.DrawString("Ready", statusFont, b, new PointF(55.0f + margin, 0 * 52.0f + 30.0f + margin));

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(57,116,145)), new Rectangle(margin + 2, 1 * 52 + 1 - (int)offset + margin, Width - 1, 50));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(2 + margin, 1 * 52 + 1 - (int)offset + margin, 49, 49));
                e.Graphics.DrawString("Selected Title", titleFont, b, new PointF(55.0f + margin, 1 * 52.0f + 10.0f + margin));
                e.Graphics.DrawString("Ready", statusFont, b, new PointF(55.0f + margin, 1 * 52.0f + 30.0f + margin));
                return;
            }
            HandleOffset();

            Pen p = new Pen(this.ForeColor);
            e.Graphics.Clear(theme.PanelColor);
           
            int count = 0;
            
            foreach (Game g in games)
            {
                if (count == selectedIndex)
                {
                    p.Color = Color.FromArgb(57, 116, 145);
                    e.Graphics.FillRectangle(p.Brush, new Rectangle(0, count * 56 - (int)offset + margin, Width - 1, 56));
                }

                p.Color = this.ForeColor;

                if (g.Favourited) e.Graphics.DrawString("(*) " + g.Title, titleFont, p.Brush, new PointF(55.0f + margin, count * 56.0f + 2f - offset + margin));
                else e.Graphics.DrawString(g.Title, titleFont, p.Brush, new PointF(55.0f + margin, count * 56.0f + 2f - offset + margin));
                e.Graphics.DrawString(g.Status, statusFont, p.Brush, new PointF(55.0f + margin, count * 56.0f + 20.0f - offset + margin));

                p.Color = Color.Black;

                if (g.IconImg != null)
                {
                    //e.Graphics.DrawRectangle(p, new Rectangle(3 + margin, count * 54 + 2 - offset + margin, 49, 49));
                    
                    e.Graphics.DrawImage(g.IconImg, new Rectangle(4 + margin, count * 56 + 4 - (int)offset + margin, 48, 48));

                }

                count++;

            }
        
            
            //e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            base.OnPaint(e);
        }

        internal void SelectGame(Game g)
        {
            selectedIndex = -1;
            for (int i = 0; i < games.Count; i++)
            {
                if (g == games[i])
                {
                    selectedIndex = i;
                    break;
                }
            }
        }
    }
}
