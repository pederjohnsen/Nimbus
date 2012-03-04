using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;


namespace Nimbus
{
    public class SlipManager
    {
        Queue<Slip> Slips = new Queue<Slip>();
        Timer timer = new Timer();
        Slip activeSlip;

        public SlipManager()
        {
            timer.Interval = 1000/80;

            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (activeSlip == null && Slips.Count > 0)
            {
                activeSlip = Slips.Dequeue();
              
                activeSlip.Show();
            }

            if (activeSlip != null) activeSlip.Tick();
        }

        public void Show(string text)
        {
            Slip temp = new Slip(Factory.CurrentTheme, text);
            temp.FormClosed += new FormClosedEventHandler(temp_FormClosed);
            Slips.Enqueue(temp);
            
        }

        void temp_FormClosed(object sender, FormClosedEventArgs e)
        {
            activeSlip.Dispose();
            activeSlip = null;
            
        }


        
    }
}
