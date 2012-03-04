using System;
using System.Collections.Generic;
using System.Text;
using Nimbus.Controls;
using System.Windows.Forms;
using System.Drawing;
using Nimbus.Theming;

namespace Nimbus.Controls
{
    class ImageButton : HoverButton
    {

        private ImageSet imageSet;

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
                    else
                    {
                        Console.WriteLine("Drawing {0}", drawType);
                        imageSet.Draw(e.Graphics, new Rectangle(Point.Empty, Size), drawType);
                    }

                }
            }
            base.OnPaint(e);
        }

    }
}
