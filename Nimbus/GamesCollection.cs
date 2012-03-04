using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Nimbus.Controls;

namespace Nimbus
{
    public class GamesCollection
    {

        private GameList<Game> games;

        public GameList<Game> Games
        {
            get { return games; }
        }

        public event EventHandler CollectionChanged;

        public GamesCollection()
        {
            games = new GameList<Game>();

        }

        public event EventHandler StopGame;

        public List<Game> LastFive = new List<Game>();

        public void LoadGames()
        {
            if (!Directory.Exists(Globals.Gamecache)) Directory.CreateDirectory(Globals.Gamecache);
            string[] directories = Directory.GetDirectories(Globals.Gamecache);
            foreach (string dir in directories)
            {
                string dirtemp = dir.Substring(Globals.Gamecache.Length + 1);
                int gameid;
                try
                {
                    gameid = Convert.ToInt32(dirtemp);
                }
                catch
                {
                    continue;
                }
                
                Game tempgame = new Game(gameid, this);
                if (tempgame.ProcessRegistry())
                {
                    games.Add(tempgame);
                    tempgame.StopGame +=new EventHandler(temp_StopGame);
                    tempgame.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(temp_PropertyChanged);
                }
                
                

            }

            games.Sort();
            LoadLastFive();
            if (CollectionChanged != null) CollectionChanged(this, null);


        }

        private void OnStopGame(Game g)
        {
            if (StopGame != null) StopGame(g, null);
        }

        public bool AddGame(int ID)
        {

            if (HasGame(ID))
            {
                Console.WriteLine("Game exists.");
                Factory.SlipManager.Show("You already have this game installed!");
                return false;

            }

            Console.WriteLine("Adding Game {0}", ID);
                Game temp = new Game(ID, this);
                temp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(temp_PropertyChanged);
                temp.LocalPath = String.Format("{1}\\{0}.zip", ID, Globals.Gamecache);
                temp.IsReady += new EventHandler(temp_IsReady);
                temp.StopGame += new EventHandler(temp_StopGame);
                Action<string> action = s =>
                {
                    if (temp.Populate())
                    {
                       lock (this) games.Add(temp);
                       games.Sort();
                       temp.Download();
                    }
                    else
                    {
                        Factory.SlipManager.Show("Failed to get game info from server. Try again later");

                    }
                };

                action.BeginInvoke(null, null, null);
                
                Factory.MainForm.ShowCollection();
                //if (CollectionChanged != null) CollectionChanged(this, null);

                return true;
          

        }

        void temp_StopGame(object sender, EventArgs e)
        {
            OnStopGame((Game)sender);
        }

        public void LoadLastFive()
        {
            if (File.Exists(Globals.LastFiveFile))
            {
                Console.WriteLine("Loading Last Five");
                using (StreamReader s = File.OpenText(Globals.LastFiveFile))
                {
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        try
                        {
                            int id = Int32.Parse(read);
                            Game g = GetGameByCode(id);
                            if (g != null) LastFive.Add(g);

                        }
                        catch { continue; }
                    }
                    s.Close();
                }



            }

        }

        public void SaveLastFive()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(Globals.LastFiveFile))) Directory.CreateDirectory(Path.GetDirectoryName(Globals.LastFiveFile));
                using (StreamWriter s = new StreamWriter(Globals.LastFiveFile, false))
                {
                    foreach (Game g in LastFive)
                    {
                        s.WriteLine(g.GameId);
                    }

                    s.Close();
                }
            }
            catch { }
        }

        public void AddGameToLastFive(Game g)
        {
            if (LastFive.Contains(g)) LastFive.Remove(g);
            LastFive.Insert(0, g);
            if (LastFive.Count > 5) LastFive.RemoveAt(5);
            SaveLastFive();
        }

        void temp_IsReady(object sender, EventArgs e)
        {
            Game g = (Game)sender;
            Factory.SlipManager.Show(String.Format("{0} has finished downloading!", g.Title));
        }

        public void RemoveGame(Game g)
        {
            if (g.DeleteFiles())
            {
                games.Remove(g);
                if (CollectionChanged != null) CollectionChanged(this, null);
            }

        }

        void temp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "title" || e.PropertyName == "favourited") games.Sort();
            CollectionChanged(sender, null);
        }

        public bool HasGame(int ID)
        {
            bool found = false;
            foreach (Game _game in games)
            {
                if (_game.GameId == ID) found = true;

            }
            return found;

        }



        public Game GetGameByCode(int Code)
        {
            foreach (Game g in Games)
            {
                if (g.GameId == Code) return g;

            }
            return null;
        }

        internal bool IsDownloading()
        {
            foreach (Game g in games)
            {
                if (g.Status != "Ready") return true;
            }
            return false;
        }
    }
}
