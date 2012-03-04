using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace Nimbus
{
    public static class Globals
    {

        private static string _gamecache;
        private static string _root;
        private static string _homepage;
        private static string _iconlocal;
        private static string _appDir;
        private static string _settingsFile;
        private static string _iconCache;
        private static string _dosboxLocation;
        private static string _dosboxConf;
        private static string _lastFiveFile;

        public static string LastFiveFile
        {
            get { return Globals._lastFiveFile; }
            set { Globals._lastFiveFile = value; }
        }

        

        public static string DosboxConf
        {
            get { return Globals._dosboxConf; }
            set { Globals._dosboxConf = value; }
        }

        public static string DosboxLocation
        {
            get { return Globals._dosboxLocation; }
            set { Globals._dosboxLocation = value; }
        }

        public static string IconCache
        {
            get { return Globals._iconCache; }
            set { Globals._iconCache = value; }
        }

        public static string SettingsFile
        {
            get { return _settingsFile; }
            set { _settingsFile = value; }
        }
        
        public static string AppDir
        {
            get { return _appDir; }
            set { _appDir = value; }
        }

//        public static NexoidSession Session;

        public static string Iconlocal
        {
            get { return _iconlocal; }
            set { _iconlocal = value; }
        }

        public static string Homepage
        {
            get { return _homepage; }
            set { _homepage = value; }
        }

        public static string Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public static string Gamecache
        {
            get { return _gamecache; }
            set { _gamecache = value; }
        }




        public static bool IsAdmin()
        {

            AppDomain ad = Thread.GetDomain();

            ad.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            WindowsPrincipal user = (WindowsPrincipal)Thread.CurrentPrincipal;

            // Decorate Next button with the BCM_SETSHIELD method if the user is an non admin



            if (!user.IsInRole(WindowsBuiltInRole.Administrator)) return true;
            else return false;


        }




    }
}
