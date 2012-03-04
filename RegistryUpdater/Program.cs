using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace RegistryUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RegistryKey Key = Registry.ClassesRoot.CreateSubKey("Nimbus");
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\nimbus.exe";
                Key.CreateSubKey("DefaultIcon").SetValue("", String.Format("{0},1", path));
                Key.SetValue("", "Nimbus:Protocol");
                Key.SetValue("URL Protocol", "");
                Key.CreateSubKey(@"shell\open\command").SetValue("", String.Format("{0} %1", path));
                Console.WriteLine(1);
            }
            catch
            {

                Console.WriteLine(0);
            }

        }
    }
}
