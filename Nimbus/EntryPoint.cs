using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;
using Nimbus.Theming;
using System.IO;

namespace Nimbus
{
    class EntryPoint
    {

        [STAThread]
        public static void Main(string[] args)
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SingleInstanceManager manager = new SingleInstanceManager();
            
            manager.Run(args);
        }

    }

    public class SingleInstanceManager : WindowsFormsApplicationBase
    {

        NimbusMain mainForm;

        public SingleInstanceManager()
        {
            this.IsSingleInstance = true;
            this.StartupNextInstance += new StartupNextInstanceEventHandler(SingleInstanceManager_StartupNextInstance);
            
        }

        protected override void OnCreateMainForm()
        {
            try
            {
                Factory.CurrentTheme = NimbusTheme.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\\themes\\default\\default.theme");
            }
            catch (ThemingError e)
            {
                MessageBox.Show(e.Error);
                Application.Exit();
            }
            
            mainForm = new NimbusMain(Factory.CurrentTheme);
            this.MainForm = mainForm;
            string[] args = new string[this.CommandLineArgs.Count];
            this.CommandLineArgs.CopyTo(args, 0);
            base.OnCreateMainForm();
            mainForm.ProcessArgs(args);
            
        }

        void SingleInstanceManager_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            string[] args = new string[e.CommandLine.Count];
            e.CommandLine.CopyTo(args, 0);
            mainForm.ProcessArgs(args);
            mainForm.Show();
            
        }
                   
       
    }

}
