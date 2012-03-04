using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nimbus
{

    public class Settings
    {

        private bool neverTryRegKey = false;
        private bool dontShowMinimizeTip = false;

        public bool DontShowMinimizeTip
        {
            get { return dontShowMinimizeTip; }
            set { dontShowMinimizeTip = value; }

        }

        public bool NeverTryRegKey
        {
            get { return neverTryRegKey; }
            set { neverTryRegKey = value; }
        }

        public void Load()
        {
            if (File.Exists(Globals.SettingsFile))
            {
                Console.WriteLine("Loading Settings");
                using (StreamReader s = File.OpenText(Globals.SettingsFile))
                {
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        ProcessLine(read);
                    }
                    s.Close();
                }



            }
            else CreateDefault();




        }

        private void CreateDefault()
        {
            

            Save();
        }

        private void ProcessLine(string read)
        {
            string[] temp = read.Split('=');

            switch (temp[0].ToLower())
            {
                case "nevertryregkey":
                    if (temp[1].ToLower() == "1") NeverTryRegKey = true;
                    else NeverTryRegKey = false;
                    break;
                case "dontshowminimizetip":
                    if (temp[1].ToLower() == "1") DontShowMinimizeTip = true;
                    else DontShowMinimizeTip = false;
                    break;

                default:

                    break;

            }

        }

        public void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Globals.SettingsFile))) Directory.CreateDirectory(Path.GetDirectoryName(Globals.SettingsFile));
            using (StreamWriter s = new StreamWriter(Globals.SettingsFile,false))
            {
                s.WriteLine("///Nexus Settings File///");
                s.WriteLine("///Do Not Edit///");
                if (NeverTryRegKey) s.WriteLine("nevertryregkey=1");
                if (DontShowMinimizeTip) s.WriteLine("dontshowminimizetip=1");

                s.Close();
            }


        }
    }
}
