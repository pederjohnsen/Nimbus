using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using Nimbus.Controls;
using Nimbus.Network;
using Nimbus.Theming;
using System.IO;
using System.Net;
using System.Threading;
using WebKit.DOM;
using Nimbus.NimbusControls;
using System.Drawing.Drawing2D;


namespace Nimbus
{
    [System.ComponentModel.DesignerCategory("form")]
    public partial class NimbusMain : NimbusForm
    {

        string updateServer = "http://nimbus.agsarchives.com/NimbusFiles/";
        delegate void UpdateConnectionDelegate();
        delegate void CheckForUpdatesDelegate();
        delegate void GameContextClick(Game g);
        NotifyIcon notifyIcon;
        NimbusContextMenu notifyIconContext;
        NimbusContextMenu gameContext;
        Splash splash;
        
            

        public NimbusMain(NimbusTheme theme)
            :base(theme)
        {
            InitializeComponent();
            splash = new Splash(theme);
            splash.Show();
#if DEBUG
            ConsoleManager.Show();
#endif

            SetSettings();
            RenewRegistry();
            InitialiseFactory();
            SetEvents();
            CreateContextMenu();
            ConfigureCaptionButtons();
            ConfigureNotifyIcon();

            
            
  
            ConfigureControls();
            ShowCollection();
            brwLibrary.Navigate(Globals.Homepage);
            //brwLibrary.Navigate("http://svn.thethoughtradar.com/test.html");
            //brwLibrary.Navigate("http://trac.webkit.org/export/41842/trunk/LayoutTests/scrollbars/overflow-scrollbar-combinations.html");
            CheckForUpdatesDelegate chk = new CheckForUpdatesDelegate(CheckForUpdates);
            chk.BeginInvoke(null, null);
            
            
        }

        protected override void OnLoad(EventArgs e)
        {
            NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE);
            base.OnLoad(e);
        }

        
        protected override void OnClosing(CancelEventArgs e)
        {
            if (Factory.Games.IsDownloading())
            {
                MessageBoxReturn mbr = NimbusMessageBox.AskQuestion("Nimbus is currently downloading, are you sure you wish to exit?", "Are you sure");
                if (!mbr.clickedYes) e.Cancel = true;
            }
            if (!e.Cancel) NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_HIDE);
            base.OnClosing(e);
        }
        protected override void OnShown(EventArgs e)
        {
            if (splash != null)
            {
                splash.Close();
                splash.Dispose();
            }
            base.OnShown(e);
            //Login();
        }

        private void UpdateRequired()
        {
            DialogResult result = MessageBox.Show("An update for nimbus is available. Would you like to update now?","Update Available", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = Globals.AppDir + "\\nimbusupdater.exe";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                Close();
            }

        }

        private void CheckForUpdates()
        {
            Thread.Sleep(5000);
            WebClient wc = new WebClient();
            try
            {
                string ver = wc.DownloadString(updateServer + "version");
                Console.WriteLine("Server version '{0}'", ver.Trim());

                string thisver = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
                Console.WriteLine("This version '{0}'", thisver);

                if (ver.Trim() != thisver) this.BeginInvoke(new MethodInvoker(UpdateRequired));
            }
            catch
            {
                Console.WriteLine("Couldn't get update version");
            }
            

            
        }

        private void ConfigureCaptionButtons()
        {
            
            if (!Theme.CloseButton.AnyNull()) closeButton.SetImageSet(Theme.CloseButton);
            if (!Theme.MinimizeButton.AnyNull()) minimizeButton.SetImageSet(Theme.MinimizeButton);
            if (!Theme.MaximizeButton.AnyNull()) maximizeButton.SetImageSet(Theme.MaximizeButton);
            if (!Theme.BackButton.AnyNull()) btnBack.SetImageSet(Theme.BackButton);
            if (!Theme.ForwardButton.AnyNull()) btnForward.SetImageSet(Theme.ForwardButton);
            btnBack.Visible = false;
            btnForward.Visible = false;
            btnLibrary.IsToggle = true;
            btnGames.IsToggle = true;
          
            
            List<HoverButton> toggleSet = new List<HoverButton>();
            toggleSet.Add(btnLibrary);
            toggleSet.Add(btnGames);
  
            
            btnLibrary.SetToggleSet(toggleSet);
            btnGames.SetToggleSet(toggleSet);

          

            btnLibrary.SetTheme(Theme);
            btnGames.SetTheme(Theme);

            btnLibrary.Toggled = true;
            btnLibrary.Clicked += new EventHandler(btnLibrary_Clicked);
            btnGames.Clicked += new EventHandler(btnGames_Clicked);
           closeButton.Clicked += new EventHandler(closeButton_Clicked);
            minimizeButton.Clicked += new EventHandler(minimizeButton_Clicked);
            maximizeButton.Clicked += new EventHandler(maximizeButton_Clicked);
            btnBack.Clicked += new EventHandler(btnBack_Clicked);
            btnForward.Clicked += new EventHandler(btnForward_Clicked);

           

        }

        void btnForward_Clicked(object sender, EventArgs e)
        {
            brwLibrary.GoForward();
        }

        void btnBack_Clicked(object sender, EventArgs e)
        {
            brwLibrary.GoBack();
        }

        void btnFriends_Clicked(object sender, EventArgs e)
        {
            NimbusMessageBox.AskQuestion("Sup dawg", "hell yes");
        }

        void btnConfig_Clicked(object sender, EventArgs e)
        {
            Factory.SlipManager.Show("I love this message!");
        }

        void btnGames_Clicked(object sender, EventArgs e)
        {
            ShowCollection();
        }

        void btnLibrary_Clicked(object sender, EventArgs e)
        {
            ShowLibrary();
        }

        void btnFavourite_Click(object sender, EventArgs e)
        {
            Game g = lst.GetSelectedGame();
            if (g != null) g.Favourited = !g.Favourited;
            g.SaveRegistryFile();
            lst.SelectGame(g);
        }

        #region Configures

        void ConfigureControls()
        {

            //pnlCollection.Size = new Size(Width - 12, Height - (CaptionHeight + 105 + 12));
            //pnlLibrary.Size = pnlCollection.Size;
            //pnlCollection.Location = new Point(6, CaptionHeight + 105 + 6);
            //pnlLibrary.Location = pnlCollection.Location;
            
            pnlCollection.Location = pnlLibrary.Location;
            pnlCollection.Size = pnlLibrary.Size;
            pnlCollection.BackColor = Theme.PanelColor;
            pnlLibrary.BackColor = Theme.PanelColor;
            brwLibrary.Size = new Size(pnlLibrary.Size.Width, pnlLibrary.Size.Height);
            brwLibrary.Location = new Point(0, 0);
            lst.Location = new Point(0, 0);
            lst.Height = pnlCollection.Height;
            brwCollection.Height = pnlCollection.Height;
            brwCollection.Location = new Point(lst.Width + 6, 0);
            brwCollection.Width = pnlCollection.Width - (lst.Width + 6);
            brwLibrary.UserAgent += " Nimbus Version: " + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            brwCollection.AllowDownloads = false;
            brwLibrary.AllowDownloads = false;
            brwCollection.BorderColor = Theme.PanelColor;
            
            brwLibrary.BorderColor = Theme.PanelColor;
            lst.Height = pnlCollection.Height;
            lst.Location = new Point(0, 0);
            lst.Games = Factory.Games.Games;
            lst.SetTheme(Theme);
            brwLibrary.ObjectForScripting = new BrowserInterop();
        }

        void brwCollection_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Theme.BackgroundColor);
            e.Graphics.DrawRectangle(p, new Rectangle(0,0,brwCollection.Width, brwCollection.Height));
        }

        void CreateContextMenu()
        {
            gameContext = new NimbusContextMenu(this.Theme);
            gameContext.Add(new NMenuItem("Play", new EventHandler(btnPlay_Click)));
            gameContext.Add(new NMenuItem("Configure", new EventHandler(btnSetup_Click)));
            gameContext.Add(new NMenuItem("Favourite", new EventHandler(btnFavourite_Click)));
            gameContext.Add(new NMenuItem("-"));
            gameContext.Add(new NMenuItem("Delete", new EventHandler(btnDelete_Click)));
            lst.ContextMenu = gameContext;
        }

        void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //WebKit.WebKitBrowser brow = (WebKit.WebKitBrowser)sender;
            Console.WriteLine("Navigating to: {0}", e.Url.ToString());
            if (e.Url.ToString().Contains("nimbus://"))
            {
                e.Cancel = true;
                Argument temparg = new Argument(e.Url.ToString());
                temparg.RunArgument(this);
            }
        }

        void SetEvents()
        {
            lst.SelectedItemChanged += new EventHandler(lst_SelectedItemChanged);
            brwLibrary.Navigating +=new WebBrowserNavigatingEventHandler(Browser_Navigating);
            brwLibrary.Navigated += new WebBrowserNavigatedEventHandler(brwLibrary_Navigated);
            brwCollection.Navigating += new WebBrowserNavigatingEventHandler(Browser_Navigating);
            this.Disposed += new EventHandler(NexusMain_Disposed);
        }

        void SetNavButtonsVis()
        {
            btnBack.Visible = (brwLibrary.CanGoBack && btnLibrary.Toggled);
            btnForward.Visible = (brwLibrary.CanGoForward && btnLibrary.Toggled);
        }

        void brwLibrary_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            SetNavButtonsVis();
        }



        void InitialiseFactory()
        {
            Factory.Games = new GamesCollection();
            Factory.Games.CollectionChanged += new EventHandler(Games_CollectionChanged);
            Factory.Games.StopGame += new EventHandler(Games_StopGame);
            Factory.Games.LoadGames();
            Factory.Nexoid = new NimboidSession();
            Factory.FriendsMan = new FriendsManager();
            Factory.MainForm = this;
            Factory.Pipe = new Interop.NexusPipe();
            Factory.Pipe.StartListening();
            Factory.Pipe.MessageReceived += new Interop.NexusPipe.MessageReceivedHandler(Pipe_MessageReceived);
            Factory.SlipManager = new SlipManager();

        }

        

        #endregion

        void Games_StopGame(object sender, EventArgs e)
        {
           // brwCollection.Reload();
        }

        void Pipe_MessageReceived(string message)
        {
            MessageBox.Show(message);
        }

      

        void NexusMain_Disposed(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
        }



        private void ConfigureNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = true;
            notifyIcon.Text = "Nimbus";
            notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);

            //notifyIconContext = new NimbusContextMenu(this.Theme);
            
            //notifyIconContext.MenuItems.Add(new MenuItem("Online Library", new EventHandler(btnLibrary_Clicked)));
            //notifyIconContext.MenuItems.Add(new MenuItem("Collection", new EventHandler(btnGames_Clicked)));
            //notifyIconContext.MenuItems.Add(new MenuItem("-"));
            //notifyIconContext.MenuItems.Add(new MenuItem("Exit", new EventHandler(Exit_Click)));
            

        }

        void Game_Click(object sender, EventArgs e)
        {
            NMenuItem m = (NMenuItem)sender;

            Game g = m.ForGame;
            if (g != null) g.Play();
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                WindowState = FormWindowState.Normal;
                if (!Visible)
                {
                    NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE);
                    Show();
                }
                this.Focus();

            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                notifyIconContext = new NimbusContextMenu(Theme);
                foreach (Game g in Factory.Games.LastFive)
                {
                                        
                    notifyIconContext.Add(new NMenuItem(g.Title, new EventHandler(Game_Click),g));
                }
                if (Factory.Games.LastFive.Count > 0) notifyIconContext.Add(new NMenuItem("-"));
                notifyIconContext.Add(new NMenuItem("Online Library", new EventHandler(btnLibrary_Clicked)));
                notifyIconContext.Add(new NMenuItem("Collection", new EventHandler(btnGames_Clicked)));
                notifyIconContext.Add(new NMenuItem("-"));
                notifyIconContext.Add(new NMenuItem("Exit", new EventHandler(Exit_Click)));
                notifyIconContext.ShowMenu();
                

            }
        }

        


        protected void btnDelete_Click(Object sender, System.EventArgs e)
        {
            Game g = lst.GetSelectedGame();
            if (g != null) Factory.Games.RemoveGame(g);
        }
       

        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Application.Exit();
        }
       


        void maximizeButton_Clicked(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal;
            else this.WindowState = FormWindowState.Maximized;

        }

        void minimizeButton_Clicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        
        void closeButton_Clicked(object sender, EventArgs e)
        {
#if DEBUG
            Close();
#else
            NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_HIDE);
            this.Hide();
            if (!Factory.Settings.DontShowMinimizeTip)
            {
                notifyIcon.ShowBalloonTip(15, "Nimbus", "Nimbus is only hidden!" + Environment.NewLine + "(Click here to never see this notification again)", ToolTipIcon.Info);
                notifyIcon.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
            }
#endif
        }

        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Factory.Settings.DontShowMinimizeTip = true;
            Factory.Settings.Save();
        }


        void lst_SelectedItemChanged(object sender, EventArgs e)
        {
            Game g = Factory.Games.Games[lst.SelectedIndex];
            ShowGameCard(g);
        }

        void ShowGameCard(Game g)
        {
            
            if (g != null)
            {
                string filename = g.GenerateGamecard();
                string toBrowse = "file:///" + filename.Replace("\\", "/");
                toBrowse = toBrowse.Replace(" ", "%20");
                //if (brwCollection.Url.AbsoluteUri.ToString() == toBrowse) brwCollection.Reload();
             
                    brwCollection.Navigate(toBrowse);
                

            }
        }


  
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            base.OnPaint(e);
            if (DesignMode) return;

            if (Theme.ToolbarLeft == null)
            {
                Brush b = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(b, new Rectangle(0, CaptionHeight, Width, 105));
                b.Dispose();
            }
            else
            {
                //for (int i = Theme.ToolbarLeft.Width; i < Width - Theme.ToolbarRight.Width; i++)
                //{
                //    e.Graphics.DrawImage(Theme.ToolbarStretch, new Rectangle(i, CaptionHeight, 1, 105));
                //}

                Brush lgb = new LinearGradientBrush(new Point(Width / 2, Height), new Point(Width / 2, Height - 120), Color.FromArgb(106, 145, 126), Color.FromArgb(44, 66, 69));
                Brush lgb2 = new LinearGradientBrush(new Point(Width / 2, Height - 119), new Point(Width / 2, CaptionHeight), Color.FromArgb(44, 66, 69), Color.FromArgb(28, 47, 53));
                
                e.Graphics.FillRectangle(lgb, new Rectangle(0, Height - 120, Width, 120));
                e.Graphics.FillRectangle(lgb2, new Rectangle(0, CaptionHeight, Width, Height - CaptionHeight - 119));
                lgb.Dispose();
                lgb2.Dispose();
                e.Graphics.DrawImage(Theme.ToolbarLeft, new Rectangle(0, Height - Theme.ToolbarLeft.Height, Theme.ToolbarLeft.Width, Theme.ToolbarLeft.Height));
                e.Graphics.DrawImage(Theme.ToolbarRight, new Rectangle(Width - Theme.ToolbarRight.Width, Height - Theme.ToolbarRight.Height, Theme.ToolbarRight.Width, Theme.ToolbarRight.Height));

                DrawShadow(pnlLibrary, e.Graphics);
            }

        }

        private void DrawShadow(Control ctrl, Graphics graphics)
        {
            if (Theme.ShadowBottomLeft != null)
            {
                graphics.DrawImage(Theme.ShadowBottomLeft, new Point(ctrl.Left, ctrl.Top + ctrl.Height));
                for (int i = ctrl.Left + Theme.ShadowBottomLeft.Width; i < ctrl.Left + ctrl.Width; i++)
                {
                   graphics.DrawImage(Theme.ShadowHorizontal, new Point(i, ctrl.Top + ctrl.Height));
                }
                graphics.DrawImage(Theme.ShadowCorner, new Point(ctrl.Left + ctrl.Width, ctrl.Top + ctrl.Height));
                graphics.DrawImage(Theme.ShadowTopRight, new Point(ctrl.Left + ctrl.Width, ctrl.Top));
                for (int i = ctrl.Top + Theme.ShadowTopRight.Height; i < ctrl.Top + ctrl.Height; i++)
                {
                    graphics.DrawImage(Theme.ShadowVertical, new Point(ctrl.Left + ctrl.Width, i));
                }
            }
        }

        void Games_CollectionChanged(object sender, EventArgs e)
        {
            lst.Invalidate();
         
        }

        internal void ProcessArgs(string[] args)
        {
            
            if (args.Length > 0)
            {
                try
                {
                    Argument arg = new Argument(args[0]);
                    arg.RunArgument(this);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void SetSettings()
        {
            Globals.SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Nimbus\\Settings.ini";
            Globals.LastFiveFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Nimbus\\LastFive.ini";
            Globals.Root = "http://www.agsarchives.com/";
            Globals.Gamecache = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Nimbus\\Gamecache";
            Globals.Homepage = "http://nimbus.agsarchives.com/";
            Globals.Iconlocal = "upload/images/games/icons/";
            Globals.AppDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Globals.IconCache = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Nimbus\\IconCache";
            Globals.DosboxLocation = Globals.AppDir + "\\Dosbox\\dosbox.exe";
            Globals.DosboxConf = Globals.AppDir + "\\Dosbox\\dosbox.conf";
            Factory.Settings = new Settings();
            Factory.Settings.Load();

        }

        private void RenewRegistry()
        {

            if (Factory.Settings.NeverTryRegKey) return;

            RegistryKey TestKey = Registry.ClassesRoot.OpenSubKey("Nimbus");
            if (TestKey != null)
            {
                Console.WriteLine("Registry Key exists");
                return;
            }
            else
            {
                Console.WriteLine("Registry Key doesn't exist! Attempting to set.");
                MessageBoxReturn mb = NimbusMessageBox.AskQuestion("The Nimbus registry key does not seem to be set. Would you like to set it now?", "Nimbus Registry Key");
                if (mb.clickedYes)
                {
                    Process myProcess = new Process();
                    ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\RegistryUpdater.exe");
                    //myProcessStartInfo.UseShellExecute = true;
                    //myProcessStartInfo.Verb = "runas";
                    myProcess.StartInfo = myProcessStartInfo;
                    myProcess.Start();

                    myProcess.WaitForExit();
                }

                Factory.Settings.NeverTryRegKey = mb.checkedBox;
                Factory.Settings.Save();

                    TestKey = Registry.ClassesRoot.OpenSubKey("Nexus");
                    if (TestKey != null) Console.WriteLine("Success!");
                    else Console.WriteLine("FAILED!");
                
            }

        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            Game g = lst.GetSelectedGame();
            if (g != null) g.Play();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            Game g = lst.GetSelectedGame();
            if (g != null) g.Configure();
        }

        public void ShowLibrary()
        {
            if (!Visible)
            {
                NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE);
                Show();
            }
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
            Focus();
            //if (Factory.Session == null || !Factory.Session.LoggedIn)
            //{
            //    Factory.SlipManager.Show("You need to be logged in to access the library");
            //    return;
            //}
            if (!btnLibrary.Toggled) btnLibrary.Toggled = true;
            pnlLibrary.Visible = true;
            pnlCollection.Visible = false;
            SetNavButtonsVis();
            Invalidate();
            Update();
        }

        public void ShowCollection()
        {
            if (!Visible)
            {
                NativeMethods.AnimateWindow(this.Handle, 200, (int)NativeMethods.AnimateWindowFlags.AW_BLEND | (int)NativeMethods.AnimateWindowFlags.AW_ACTIVATE);
                Show();
            }
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
            Focus();
            pnlCollection.Visible = true;
            pnlLibrary.Visible = false;
            if (!btnGames.Toggled) btnGames.Toggled = true;
            SetNavButtonsVis();
            Game g = null;
            if (lst.SelectedIndex > -1) g = Factory.Games.Games[lst.SelectedIndex];
            if (g == null)
            {
                string toBrowse = "file:///" + Globals.AppDir.Replace("\\", "/") + "/WebMedia/Default.html";
                toBrowse = toBrowse.Replace(" ", "%20");
                brwCollection.Navigate(toBrowse);
            }
            Invalidate();
            Update();
        }

        public void Login()
        {
            Factory.Session = new Session();
            LoginResult lr = Factory.Session.Login();
            if (lr.Passed)
            {
                string loginlander = String.Format("{0}index.php?sessionid={1}", Globals.Homepage, lr.Hash);
                ShowLibrary();
                brwLibrary.Navigate(loginlander);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowLibrary();
        }

        private void btnCollection_Click(object sender, EventArgs e)
        {
            ShowCollection();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            brwLibrary.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            brwLibrary.GoForward();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           /* Element scriptElm = brwLibrary.Document.CreateElement("script");
            scriptElm.AppendChild(brwLibrary.Document.CreateTextNode("Test(['ABC','EFG'],{'name':'First Name','surname':'LastName'})"));

            NodeList headElm = brwLibrary.Document.GetElementsByTagName("head");
            headElm[0].AppendChild(scriptElm);
            */
            BrowserInterop.RunScript(brwLibrary, "Test", "hi", "lol");
        }


       

        
      

     
      
     
     

     

        

    }
}
