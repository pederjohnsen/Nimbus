using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Text;

namespace Nimbus.Theming
{
    
    public class NimbusTheme
    {

        #region Form Things

        private PrivateFontCollection fontCollection;

        public string LocalPath { get; set; }
        public Color BackgroundColor { get; set; }
        public Color ToolbarColor { get; set; }
        public Color CaptionBarColor { get; set; }
        public Color WindowBackground { get; set; }
        public Color CaptionTextColor { get; set; }
        public Color PanelColor { get; set; }
        public FontFamily CaptionFontFamily { get; set; }
        public FontFamily ButtonFont { get; set; }
        public int CaptionFontSize { get; set; }
        public int CaptionOffsetX { get; set; }
        public int CaptionOffsetY { get; set; }
        public ImageSet CloseButton { get; set; }
        public ImageSet MinimizeButton { get; set; }
        public ImageSet MaximizeButton { get; set; }
        public ImageSet LibraryButton { get; set; }
        public ImageSet GamesButton { get; set; }
        public ImageSet ConfigButton { get; set; }
        public ImageSet FriendsButton { get; set; }
        public ImageSet BackButton { get; set; }
        public ImageSet ForwardButton { get; set; }
        public Image NexusWatermark { get; set; }
        public Image ToolbarLeft { get; set; }
        public Image ToolbarRight { get; set; }
        public Image ToolbarStretch { get; set; }
        public Image ShadowTopRight { get; set; }
        public Image ShadowBottomLeft { get; set; }
        public Image ShadowCorner { get; set; }
        public Image ShadowHorizontal { get; set; }
        public Image ShadowVertical { get; set; }
        public string SplashFile { get; set; }
        public Icon Icon { get; set; }


        #endregion

        public NimbusTheme()
        {
            CloseButton = new ImageSet();
            MinimizeButton = new ImageSet();
            MaximizeButton = new ImageSet();
            LibraryButton = new ImageSet();
            GamesButton = new ImageSet();
            ConfigButton = new ImageSet();
            FriendsButton = new ImageSet();
            BackButton = new ImageSet();
            ForwardButton = new ImageSet();
            
            fontCollection = new PrivateFontCollection();
        }

        public static Color ParseColor(string val)
        {
            try
            {
                string[] rgb = val.Split(',');
                int r = Int32.Parse(rgb[0].Trim());
                int g = Int32.Parse(rgb[1].Trim());
                int b = Int32.Parse(rgb[2].Trim());
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                throw new ThemingError(null, "Color Parse Error");
            }
           
        }

        public FontFamily LoadFontFamily(string fileName)
        {
            
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fileName);
            return fontCollection.Families[fontCollection.Families.Length - 1];
        }

        public static void ProcessIniLine(string read, NimbusTheme theme)
        {
            if (read.StartsWith("//")) return; //is a comment
            string trigger = read.Split('=')[0].ToLower();
            string val = read.Split('=')[1];
            switch (trigger)
            {
                case "backgroundcolor":
                    {
                        try
                        {
                            theme.BackgroundColor = ParseColor(val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "panelcolor":
                    {
                        try
                        {
                            theme.PanelColor = ParseColor(val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "icon":
                    {
                        try
                        {
                            theme.Icon = new Icon(theme.LocalPath + "\\" + val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captionbarcolor":
                    {
                        try
                        {
                            theme.CaptionBarColor = ParseColor(val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "toolbarcolor":
                    {
                        try
                        {
                            theme.ToolbarColor = ParseColor(val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captionfont":
                    {
                        try
                        {
                            theme.CaptionFontFamily = theme.LoadFontFamily(theme.LocalPath + "\\" + val.Trim());
                            
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "buttonfont":
                    {
                        try
                        {
                            theme.ButtonFont = theme.LoadFontFamily(theme.LocalPath + "\\" + val.Trim());

                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captionfontsize":
                    {
                        try
                        {
                            theme.CaptionFontSize = Int32.Parse(val.Trim());

                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captiontextcolor":
                    {
                        try
                        {
                            theme.CaptionTextColor = ParseColor(val);
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captiontitleoffsetx":
                    {
                        try
                        {
                            theme.CaptionOffsetX = Int32.Parse(val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "captiontitleoffsety":
                    {
                        try
                        {
                            theme.CaptionOffsetY = Int32.Parse(val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "nexuswatermark":
                    {
                        try
                        {
                            theme.NexusWatermark = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "closenormal":
                    {
                        try
                        {
                            theme.CloseButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "closehover":
                    {
                        try
                        {
                            theme.CloseButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "closepressed":
                    {
                        try
                        {
                            theme.CloseButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "minimizenormal":
                    {
                        try
                        {
                            theme.MinimizeButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "minimizehover":
                    {
                        try
                        {
                            theme.MinimizeButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "minimizepressed":
                    {
                        try
                        {
                            theme.MinimizeButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "maximizenormal":
                    {
                        try
                        {
                            theme.MaximizeButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "maximizehover":
                    {
                        try
                        {
                            theme.MaximizeButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "maximizepressed":
                    {
                        try
                        {
                            theme.MaximizeButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "librarynormal":
                    {
                        try
                        {
                            theme.LibraryButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "libraryhover":
                    {
                        try
                        {
                            theme.LibraryButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "librarypressed":
                    {
                        try
                        {
                            theme.LibraryButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "gamesnormal":
                    {
                        try
                        {
                            theme.GamesButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "gameshover":
                    {
                        try
                        {
                            theme.GamesButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "gamespressed":
                    {
                        try
                        {
                            theme.GamesButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "confignormal":
                    {
                        try
                        {
                            theme.ConfigButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "confighover":
                    {
                        try
                        {
                            theme.ConfigButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "configpressed":
                    {
                        try
                        {
                            theme.ConfigButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "friendsnormal":
                    {
                        try
                        {
                            theme.FriendsButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "friendshover":
                    {
                        try
                        {
                            theme.FriendsButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "friendspressed":
                    {
                        try
                        {
                            theme.FriendsButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "backbuttonnormal":
                    {
                        try
                        {
                            theme.BackButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "backbuttonhover":
                    {
                        try
                        {
                            theme.BackButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "backbuttonpressed":
                    {
                        try
                        {
                            theme.BackButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "forwardbuttonnormal":
                    {
                        try
                        {
                            theme.ForwardButton.Normal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "forwardbuttonhover":
                    {
                        try
                        {
                            theme.ForwardButton.Hover = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "forwardbuttonpressed":
                    {
                        try
                        {
                            theme.ForwardButton.Pressed = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "splash":
                    {
                        try
                        {
                            theme.SplashFile = theme.LocalPath + "\\" + val.Trim();
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "toolbarleft":
                    {
                        try
                        {
                            theme.ToolbarLeft = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "toolbarright":
                    {
                        try
                        {
                            theme.ToolbarRight = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "toolbarstretch":
                    {
                        try
                        {
                            theme.ToolbarStretch = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "shadowtopright":
                    {
                        try
                        {
                            theme.ShadowTopRight = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "shadowbottomleft":
                    {
                        try
                        {
                            theme.ShadowBottomLeft = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "shadowhorizontal":
                    {
                        try
                        {
                            theme.ShadowHorizontal = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "shadowvertical":
                    {
                        try
                        {
                            theme.ShadowVertical = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }
                case "shadowcorner":
                    {
                        try
                        {
                            theme.ShadowCorner = Image.FromFile(theme.LocalPath + "\\" + val.Trim());
                        }
                        catch
                        {
                            throw new ThemingError(null, String.Format("Error parsing {0}", read));
                        }
                        break;
                    }


            }
        }

        public static NimbusTheme FromFile(string filename)
        {
            NimbusTheme toReturn = new NimbusTheme();
            toReturn.LocalPath = Path.GetDirectoryName(filename);
            if (File.Exists(filename))
            {
                Console.WriteLine("Reading theme");
                using (StreamReader s = File.OpenText(filename))
                {
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        ProcessIniLine(read, toReturn);
                    }
                    s.Close();
                    s.Dispose();
                }
                
            }
            else throw new ThemingError(null, String.Format("Theme file {0} not found", filename));

            return toReturn;
        }
    }

    #region ImageSet class

    public class ImageSet
    {
        public bool IsNinePatch;
        public int NinePatchBorder = 4;
        public Image Normal;
        public Image Hover;
        public Image Pressed;

        public bool AnyNull()
        {
            return (Normal == null || Hover == null || Pressed == null);
        }

        public void Draw(Graphics g, Rectangle rect, DrawType dt)
        {
            Image toDraw;
            switch (dt)
            {
                case DrawType.eNormal:
                    {
                        toDraw = Normal;
                        break;
                    }
                case DrawType.eHover:
                    {
                        toDraw = Hover;
                        break;
                    }
                case DrawType.ePressed:
                    {
                        toDraw = Pressed;
                        break;
                    }
                default:
                    {
                        throw new ThemingError(this, "DrawType not set");
                    }
            }

            if (toDraw == null) throw new ThemingError(this, "Image null");

            if (!IsNinePatch)
            {
                g.DrawImage(toDraw, rect);
            }
            else
            {
                int b = NinePatchBorder;
                int b2 = b * 2;
                g.DrawImage(toDraw, new Rectangle(0, 0, b, b), new Rectangle(0, 0, b, b), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(rect.Width - b, 0, b, b), new Rectangle(toDraw.Width - b, 0, b, b), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(0, rect.Height - b, b, b), new Rectangle(0, toDraw.Height - b, b, b), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(rect.Width - b, rect.Height - b, b, b), new Rectangle(toDraw.Width - b, toDraw.Height - b, b, b), System.Drawing.GraphicsUnit.Pixel);
                //sides
                g.DrawImage(toDraw, new Rectangle(b, 0, rect.Width - b2, b), new Rectangle(b, 0, toDraw.Width - b2, 4), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(b, rect.Height - b, rect.Width - 7, 5), new Rectangle(b, toDraw.Height - b, toDraw.Width - b2, b), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(0, b, b, rect.Height - b2), new Rectangle(0, b, b, toDraw.Height - b2), System.Drawing.GraphicsUnit.Pixel);
                g.DrawImage(toDraw, new Rectangle(rect.Width - b, b, b, rect.Height - b2), new Rectangle(toDraw.Width - b, b, b, toDraw.Height - b2), System.Drawing.GraphicsUnit.Pixel);
                //centre
                g.DrawImage(toDraw, new Rectangle(b, b, rect.Width - b2, rect.Height - b2), new Rectangle(b, b, toDraw.Width - b2, toDraw.Height - b2), System.Drawing.GraphicsUnit.Pixel);

            }

        }
    }

    #endregion

    public enum DrawType { eNormal, eHover, ePressed };
}
