using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

using System.ComponentModel;
using System.IO.Compression;
using Ionic.Zip;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;

namespace Nimbus
{
    public class Game : INotifyPropertyChanged, IComparable<Game>
    {
        private string status;
        private string executable;
        private string engine;
        private string title;
        private string version;
        private string description;
        private bool favourited;


        private TimeSpan playedFor;
        private bool needsDosBox;
        private Image iconImg;
        private long speed;
        private ZipFile zf;
        public event EventHandler IsReady;
        public event EventHandler StopGame;

        private GamesCollection collection;

        public TimeSpan PlayedFor
        {
            get { return playedFor; }
            set { playedFor = value; }

        }

        public bool Favourited
        {
            get { return favourited; }
            set 
            {
                bool oldval = favourited;
                favourited = value;
                if (oldval != favourited) NotifyPropertyChanged("favourited");
            
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }

        }

        public Image IconImg
        {
            get {
                if (iconImg == null)
                {
                    LoadImage();

                }
                return iconImg; 
            
            }
            set { iconImg = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public bool NeedsDosBox
        {
            get { return needsDosBox; }
            set { needsDosBox = value; }
        }
        private Uri _icon;

        public string Engine
        {
            get { return engine; }
            set { engine = value; }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                //Console.WriteLine("Setting Status of Game:{0} to {1}", GameId, _status);
                NotifyPropertyChanged("status");
            }
        }


        public bool Downloaded { get; set; }

        public string Executable
        {
            get { return executable; }
            set
            {
                executable = value;
                Console.WriteLine("Executable of Game {0} changed to {1}", GameId, value);
            }
        }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                NotifyPropertyChanged("title");
            }
        }
        public string Author { get; set; }
        public Uri Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                NotifyPropertyChanged("Icon");
            }
        }


        private long downloadedbytes;
        [field: NonSerialized]
        private Stopwatch watch;
        [field: NonSerialized]
        private WebClient client;
        private Uri Url { get; set; }
        public int GameId { get; set; }
        public string LocalPath { get; set; }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private bool justHadByteUpdate = false;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));

            }
        }

        public Game(GamesCollection collection)
        {
            client = new WebClient();
            title = "Not Populated Yet";
            status = "Not Ready";
            PlayedFor = new TimeSpan();
            this.collection = collection;
        }

        public Game(int _id, GamesCollection collection)
        {
            client = new WebClient();
            GameId = _id;
            PlayedFor = new TimeSpan();
            this.collection = collection;
        }

        private void LoadImage()
        {
            try
            {
                IconImg = Image.FromFile(String.Format("{0}\\{1}.png", Globals.IconCache, GameId));
            }
            catch { }
        }

        string BreaksForParas(string toProcess)
        {
            return toProcess.Replace("\n", "</p><p>");
        }

        public string GenerateGamecard()
        {
            StreamReader streamReader = new StreamReader(Globals.AppDir + "\\WebMedia\\GameCard.html");
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            text = text.Replace("$title", Title);
            text = text.Replace("$author", Author);
            text = text.Replace("$description", BreaksForParas(Description));
            text = text.Replace("$playlink", "nimbus://play:" + GameId);
            text = text.Replace("$configlink", "nimbus://config:" + GameId);
            text = text.Replace("$playedfor", FormatTimePlayed());
            text = text.Replace("$time", DateTime.Now.ToLongTimeString());

            StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(FullExecutablePath()) + "\\GameCard" + GameId + ".html");
            streamWriter.Write(text);
            streamWriter.Flush();
            streamWriter.Close();
            return Path.GetDirectoryName(FullExecutablePath()) + "\\GameCard" + GameId + ".html";
        }

        public bool Populate()
        {
            try
            {
                string temp = client.DownloadString(new Uri(String.Format("{1}getinfo.php?GameID={0}", GameId, Globals.Root)));
                string[] vals = temp.Split('#');
                Title = vals[0];
                Author = vals[1];
                Description = vals[2];
                Url = new Uri(vals[3]);
                Icon = new Uri(String.Format("{0}{1}{2}", Globals.Root, Globals.Iconlocal, vals[4]));
                Executable = vals[5];
                Engine = "AGS";
                System.Console.WriteLine("Got Info:{0}", temp);
            }
            catch
            {
                return false;
            }

            DownloadImage();
             return true;

        }

        private void DownloadImage()
        {
            if (!Directory.Exists(Globals.IconCache)) Directory.CreateDirectory(Globals.IconCache);
            Image i = null;
            try
            {
                i = Image.FromFile(Globals.IconCache + String.Format("\\{0}.png", this.GameId));
            }
            catch { }
            
            if (i == null)
            {
                try
                {
                    client.DownloadFile(Icon, Globals.IconCache + String.Format("\\{0}.png", this.GameId));
                }
                catch
                {
                    Console.WriteLine("Icon 404ed!");
                }
            }
        }



        public void Download()
        {
            if (!Directory.Exists(String.Format("{1}\\{0}", GameId, Globals.Gamecache))) Directory.CreateDirectory(String.Format("{1}\\{0}", GameId, Globals.Gamecache));
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadComplete);
            watch = new Stopwatch();
            watch.Start();
            Status = "Starting Download...";
            client.DownloadFileAsync(Url, LocalPath);
            


        }

        private void OnReady()
        {
            if (IsReady != null) IsReady(this, null);
        }

        void DownloadComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) return;
            Status = "Downloaded, Unzipping.";
            System.Console.WriteLine("Completed.");
            System.Console.WriteLine("Unzipping.");
            zf = new ZipFile(LocalPath);

            zf.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zf_ExtractProgress);
            zf.ExtractAll(String.Format("{1}\\{0}", GameId, Globals.Gamecache), ExtractExistingFileAction.OverwriteSilently);






        }

        void zf_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                if (justHadByteUpdate)
#if DEBUG
                    Console.SetCursorPosition(0, Console.CursorTop);
#endif

                Console.Write("   {0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer,
                              e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));
                justHadByteUpdate = true;
            }
            else if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                if (justHadByteUpdate)
                    Console.WriteLine();
                Console.WriteLine("Extracting: {0}", e.CurrentEntry.FileName);
                justHadByteUpdate = false;
            }
            else if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
            {
                if (Executable != "") Status = "Ready";
                try
                {
                    zf.Dispose();
                    File.Delete(LocalPath);
                }
                catch
                {
                    Console.WriteLine("Failed to delete zipfile: {0}", LocalPath);
                }
                SetVersion();
                OnReady();
                SaveRegistryFile();
                //Status = "Processing";
                //ProcessIni();
            }



        }



        private void ProcessIniLine(string read)
        {
            string[] temp = read.Split('=');

            switch (temp[0].ToLower())
            {
                case "description":
                    Description = temp[1];
                    break;

                case "executable":
                    Executable = temp[1];
                    break;

                case "engine":
                    Engine = temp[1];
                    break;

                case "title":
                    Title = temp[1];
                    break;

                case "author":
                    Author = temp[1];
                    break;

                case "needsdosbox":
                    if (temp[1] == "1") NeedsDosBox = true;
                    break;

                case "version":
                    Version = temp[1];
                    break;

                case "icon":
                    Icon = new Uri(temp[1]);
                    DownloadImage();
                    break;

                case "url":
                    Url = new Uri(temp[1]);
                    break;

                case "playedfor":
                    int i = Int32.Parse(temp[1]);
                    PlayedFor = new TimeSpan((int)(i / 60), i % 60, 0);
                    break;
                default:

                    break;

            }

            
            

        }

        public string BoolToString(bool val)
        {
            if (val) return "1";
            else return "0";

        }

        public void SaveRegistryFile()
        {

            GameSave.Save(this, GetRegistryXMLFile());

        }

        public string Pluralise(string text, int mod)
        {
            if (mod != 1) return text + "s";
            else return text;
        }

        public string FormatTimePlayed()
        {
            StringBuilder sb = new StringBuilder();
            if (playedFor.Days > 0) sb.Append(playedFor.Days.ToString() + Pluralise(" Day", playedFor.Days) + " ");
            if (sb.Length > 0 || playedFor.Hours > 0) sb.Append(playedFor.Hours.ToString() + Pluralise(" Hour", playedFor.Hours) + " ");
            if (sb.Length > 0 || playedFor.Minutes > 0) sb.Append(playedFor.Minutes.ToString() + Pluralise(" Minute", playedFor.Minutes) + " ");
            if (sb.Length == 0) sb.Append("Never Played");
            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Title, Status);
        }


        void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            if (watch.Elapsed.Seconds > 1)
            {
                speed = (e.BytesReceived - downloadedbytes) / ((long)watch.Elapsed.TotalMilliseconds + 1);
                downloadedbytes = e.BytesReceived;
                watch.Reset();
                watch.Start();

            }

            Status = String.Format("Downloading: {0}%, {1}KB/s", e.ProgressPercentage, speed);
            
            
            // System.Console.WriteLine(Status, e.ProgressPercentage, speed);

        }

        public void Configure()
        {

            if (Engine == "AGS")
            {
                if (NeedsDosBox)
                {
                    Factory.SlipManager.Show("Dos games cannot be configured in this version.");
                    return;
                }
                try
                {
                    if (Status == "Ready")
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        if (NeedsDosBox)
                        {
                            Proc.StartInfo.FileName = Globals.DosboxLocation;
                            Proc.StartInfo.Arguments = GenerateDosboxArguments(true);
                            Console.WriteLine("Running: {0} {1}", Proc.StartInfo.FileName, Proc.StartInfo.Arguments);
                        }
                        else
                        {
                            Proc.StartInfo.FileName = FullExecutablePath();
                            Proc.StartInfo.Arguments = "--setup";
                        }

                        Proc.EnableRaisingEvents = true;
                        Proc.Exited += new EventHandler(Proc_Exited);
                        Status = "Configuring";
                        Proc.Start();

                    }
                    else
                    {
                        Factory.SlipManager.Show("The game is not ready yet!");
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }
            else MessageBox.Show("This is not an AGS Game.");



        }

        private string GenerateDosboxArguments(bool setup = false)
        {
            if (setup) return String.Format("\"{0}\\ac.exe\" -conf -exit \"{1}\"", GamePath(), Globals.DosboxConf);
            return String.Format("\"{0}\" -scaler normal2x -exit -conf \"{1}\"", FullExecutablePath(), Globals.DosboxConf);
        }

        public void Play()
        {

            try
            {
                if (Status == "Ready")
                {
                    System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                    if (NeedsDosBox)
                    {
                        if (!File.Exists(Path.GetDirectoryName(FullExecutablePath()) + "\\CWSDPMI.EXE"))
                        {
                            try
                            {
                                File.Copy(Globals.AppDir + "\\CWSDPMI.EXE", Path.GetDirectoryName(FullExecutablePath()) + "\\CWSDPMI.EXE");
                            }
                            catch
                            {

                            }
                        }
                        Proc.StartInfo.FileName = Globals.DosboxLocation;
                        Proc.StartInfo.Arguments = GenerateDosboxArguments();

                    }
                    else
                    {
                        Proc.StartInfo.WorkingDirectory = String.Format("{1}\\{0}", GameId, Globals.Gamecache);
                        Proc.StartInfo.FileName = String.Format("{2}\\{0}\\{1}", GameId, Executable, Globals.Gamecache);
                    }
                    Proc.StartInfo.UseShellExecute = true;
                    Proc.EnableRaisingEvents = true;
                    Proc.Exited += new EventHandler(Proc_Exited);
                    Status = "In Game";
                    collection.AddGameToLastFive(this);
                    watch = new Stopwatch();
                    watch.Start();
                    Proc.Start();

                }
                else
                {
                    Factory.SlipManager.Show("The game is not ready yet!");
                }
            }
            catch
            {
                throw new Exception();
            }
        }



        void Proc_Exited(object sender, EventArgs e)
        {
            if (Status == "In Game")
            {
                watch.Stop();
                playedFor += watch.Elapsed;
                if (playedFor.TotalMinutes < 1)
                {
                    playedFor = playedFor.Add(new TimeSpan(0, 1, 0));
                }
                SaveRegistryFile();
                if (StopGame != null) StopGame(this, null);
            }
            Status = "Ready";
        }

        public string GetRegistryXMLFile()
        {

            return String.Format("{1}\\{0}\\Registry.xml", GameId, Globals.Gamecache);
        }

        public string GetRegistryFile()
        {

            return String.Format("{1}\\{0}\\Registry.ini", GameId, Globals.Gamecache);
        }

        private string GamePath()
        {
            return String.Format("{0}\\{1}", Globals.Gamecache, GameId);
        }

        private string FullExecutablePath()
        {
            return String.Format("{2}\\{0}\\{1}", GameId, Executable, Globals.Gamecache);
        }

        public void SetVersion()
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FullExecutablePath());
            version = fvi.FileVersion;
            if (version == null)
            {
                version = "2";
                needsDosBox = true;
            }
            Console.WriteLine("Version for {0} is {1}", title, version);
            SaveRegistryFile();
        }

        public bool ProcessRegistry()
        {
            if (File.Exists(GetRegistryXMLFile()))
            {
                GameSave gSave;
                try
                {
                    gSave = GameSave.CreateFromFile(GetRegistryXMLFile());
                    Description = gSave.Description;
                    Title = gSave.Title;
                    Executable = gSave.Executable;
                    NeedsDosBox = gSave.NeedsDosBox;
                    Engine = gSave.Engine;
                    PlayedFor = TimeSpan.FromMinutes(gSave.PlayedFor);
                    Version = gSave.Version;
                    Author = gSave.Author;
                    Icon = new Uri(gSave.Icon);
                    Favourited = gSave.Favourited;
                    Status = "Ready";
                    if (version == null || version == "") SetVersion();
                }
                catch
                {
                    Console.WriteLine("Failed to load game: " + GameId.ToString());
                    return false;
                }

                try
                {
                    if (File.Exists(GetRegistryFile())) File.Delete(GetRegistryFile());
                }
                catch { return false; }
                return true;
            }
            else if (File.Exists(GetRegistryFile()))
            {
                Console.WriteLine("Reading the contents from the Registry for {0}", title);
                using (StreamReader s = File.OpenText(GetRegistryFile()))
                {
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        try
                        {
                            ProcessIniLine(read);
                        }
                        catch { }
                    }
                    s.Close();
                    s.Dispose();
                }
                if (version == null || version == "") SetVersion();
                Status = "Ready";
                GameSave.Save(this, GetRegistryXMLFile());
                return true;
            }
            else return false;
        }

        public int CompareTo(Game other)
        {
            if (this.favourited && !other.favourited) return -1;
            else if (other.favourited && !this.favourited) return 1;
            else return this.Title.CompareTo(other.Title);
        }

        public bool DeleteFiles()
        {
            if (Status.Contains("Download"))
            {
                client.CancelAsync();

            }
            if (Status == "Downloaded, Unzipping.")
            {
                Factory.SlipManager.Show("Games cannot be deleted during the unzip procedure.");
                return false;
            }
            if (Directory.Exists(String.Format("{1}\\{0}", GameId, Globals.Gamecache)))
            {
                Directory.Delete(String.Format("{1}\\{0}", GameId, Globals.Gamecache), true);
            }
            return true;
        }
    }

    public class GameSave
    {

        public static void Save(Game game, string filename)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(GameSave));

            GameSave gSave = new GameSave();
            gSave.Description = game.Description;
            gSave.Title = game.Title;
            gSave.Executable = game.Executable;
            gSave.NeedsDosBox = game.NeedsDosBox;
            gSave.Engine = game.Engine;
            gSave.PlayedFor = (int)game.PlayedFor.TotalMinutes;
            gSave.Version = game.Version;
            gSave.Author = game.Author;
            gSave.Icon = game.Icon.ToString();
            gSave.Favourited = game.Favourited;

            using (Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, gSave);
                fStream.Close();
            }
        }

        public static GameSave CreateFromFile(string filename)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(GameSave));
            GameSave toReturn;
            using (Stream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                try
                {
                    toReturn = (GameSave)xmlFormat.Deserialize(fStream);
                }
                catch
                {
                    throw new Exception();
                }
                finally
                {
                    fStream.Close();
                }
            }
            return toReturn;
        }

        public string Author;
        public string Executable;
        public string Engine;
        public string Title;
        public string Version;
        public string Description;
        public string Icon;
        public int PlayedFor;
        public bool NeedsDosBox;
        public bool Favourited;
    }
}
