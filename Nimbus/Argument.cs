using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Nimbus
{
    public class Argument
    {
        public string Activator { get; set; }
        public int Code { get; set; }

        public Argument(string raw)
        {
            Console.WriteLine("Processing Argument:{0}", raw);
            raw = raw.ToLower();
            if (raw.Contains("game:"))
            {
                Activator = "Game";
                string temp = raw.Substring(raw.LastIndexOf(":") + 1);
                Code = Convert.ToInt32(temp.Substring(0, temp.Length));
            }
            if (raw.Contains("install:"))
            {
                Activator = "Install";
                string temp = raw.Substring(raw.LastIndexOf(":") + 1);
                string temp2 = "";
                foreach (char c in temp)
                {
                    if (char.IsDigit(c)) temp2 = temp2 + c;

                }
                Code = Convert.ToInt32(temp2);
            }
            if (raw.Contains("play:"))
            {
                Activator = "Play";
                string temp = raw.Substring(raw.LastIndexOf(":") + 1);
                string temp2 = "";
                foreach (char c in temp)
                {
                    if (char.IsDigit(c)) temp2 = temp2 + c;

                }
                Code = Convert.ToInt32(temp2);
            }
            if (raw.Contains("config:"))
            {
                Activator = "Config";
                string temp = raw.Substring(raw.LastIndexOf(":") + 1);
                string temp2 = "";
                foreach (char c in temp)
                {
                    if (char.IsDigit(c)) temp2 = temp2 + c;

                }
                Code = Convert.ToInt32(temp2);
            }
        }

        public Argument(string act, int code)
        {
            Activator = act;
            Code = code;

        }

        public void RunArgument(NimbusMain window)
        {
            
            //MessageBox.Show(this.Activator + this.Code);

            if (Activator == "Game")
            {

                
            }
            if (Activator == "Install")
            {

                Factory.Games.AddGame(Code);

            }
            if (Activator == "Play")
            {
                Game g = Factory.Games.GetGameByCode(Code);
                if (g != null) g.Play();
            }
            if (Activator == "Config")
            {
                Game g = Factory.Games.GetGameByCode(Code);
                if (g != null) g.Configure();

            }
            

        }
    }
}
