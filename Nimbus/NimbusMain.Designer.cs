using Nimbus.Controls;
namespace Nimbus
{
    partial class NimbusMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnForward = new Nimbus.Controls.ImageButton();
            this.btnBack = new Nimbus.Controls.ImageButton();
            this.btnGames = new Nimbus.Controls.TextButton();
            this.btnLibrary = new Nimbus.Controls.TextButton();
            this.minimizeButton = new Nimbus.Controls.ImageButton();
            this.maximizeButton = new Nimbus.Controls.ImageButton();
            this.closeButton = new Nimbus.Controls.ImageButton();
            this.pnlCollection = new Nimbus.Controls.TransparentPanel();
            this.brwCollection = new Nimbus.Controls.BorderlessWebkit();
            this.lst = new Nimbus.Controls.GamesListBox();
            this.pnlLibrary = new Nimbus.Controls.TransparentPanel();
            this.brwLibrary = new Nimbus.Controls.BorderlessWebkit();
            this.pnlCollection.SuspendLayout();
            this.pnlLibrary.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnForward.IsToggle = false;
            this.btnForward.Location = new System.Drawing.Point(97, 314);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(63, 35);
            this.btnForward.TabIndex = 24;
            this.btnForward.Text = "captionButton2";
            this.btnForward.Toggled = false;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBack.IsToggle = false;
            this.btnBack.Location = new System.Drawing.Point(28, 314);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(63, 35);
            this.btnBack.TabIndex = 23;
            this.btnBack.Text = "captionButton1";
            this.btnBack.Toggled = false;
            // 
            // btnGames
            // 
            this.btnGames.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnGames.Font = new System.Drawing.Font("AkzidenzGrotesk", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGames.ForeColor = System.Drawing.Color.White;
            this.btnGames.HasShadow = true;
            this.btnGames.IsToggle = false;
            this.btnGames.Location = new System.Drawing.Point(2, 111);
            this.btnGames.Name = "btnGames";
            this.btnGames.Size = new System.Drawing.Size(169, 45);
            this.btnGames.TabIndex = 20;
            this.btnGames.Text = "games.";
            this.btnGames.Toggled = false;
            this.btnGames.Tracking = -12;
            this.btnGames.YOffset = 15;
            // 
            // btnLibrary
            // 
            this.btnLibrary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLibrary.Font = new System.Drawing.Font("AkzidenzGrotesk", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLibrary.ForeColor = System.Drawing.Color.White;
            this.btnLibrary.HasShadow = true;
            this.btnLibrary.IsToggle = false;
            this.btnLibrary.Location = new System.Drawing.Point(2, 51);
            this.btnLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.btnLibrary.Name = "btnLibrary";
            this.btnLibrary.Size = new System.Drawing.Size(154, 48);
            this.btnLibrary.TabIndex = 19;
            this.btnLibrary.Text = "library.";
            this.btnLibrary.Toggled = false;
            this.btnLibrary.Tracking = -12;
            this.btnLibrary.YOffset = 10;
            // 
            // minimizeButton
            // 
            this.minimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.minimizeButton.IsToggle = false;
            this.minimizeButton.Location = new System.Drawing.Point(975, 7);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(10, 10);
            this.minimizeButton.TabIndex = 11;
            this.minimizeButton.Text = "captionButton2";
            this.minimizeButton.Toggled = false;
            // 
            // maximizeButton
            // 
            this.maximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maximizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.maximizeButton.IsToggle = false;
            this.maximizeButton.Location = new System.Drawing.Point(991, 7);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(10, 10);
            this.maximizeButton.TabIndex = 10;
            this.maximizeButton.Text = "captionButton1";
            this.maximizeButton.Toggled = false;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.IsToggle = false;
            this.closeButton.Location = new System.Drawing.Point(1007, 7);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(10, 10);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "captionButton1";
            this.closeButton.Toggled = false;
            // 
            // pnlCollection
            // 
            this.pnlCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCollection.BackColor = System.Drawing.Color.Transparent;
            this.pnlCollection.Controls.Add(this.brwCollection);
            this.pnlCollection.Controls.Add(this.lst);
            this.pnlCollection.Location = new System.Drawing.Point(177, 51);
            this.pnlCollection.Name = "pnlCollection";
            this.pnlCollection.Size = new System.Drawing.Size(840, 407);
            this.pnlCollection.TabIndex = 6;
            this.pnlCollection.Visible = false;
            // 
            // brwCollection
            // 
            this.brwCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.brwCollection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brwCollection.BackColor = System.Drawing.Color.White;
            this.brwCollection.BorderColor = System.Drawing.Color.Red;
            this.brwCollection.Location = new System.Drawing.Point(293, 0);
            this.brwCollection.Margin = new System.Windows.Forms.Padding(0);
            this.brwCollection.Name = "brwCollection";
            this.brwCollection.Size = new System.Drawing.Size(547, 407);
            this.brwCollection.TabIndex = 1;
            this.brwCollection.Url = null;
            // 
            // lst
            // 
            this.lst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(61)))), ((int)(((byte)(90)))));
            this.lst.Dock = System.Windows.Forms.DockStyle.Left;
            this.lst.ForeColor = System.Drawing.Color.White;
            this.lst.InternalMargin = 2;
            this.lst.Location = new System.Drawing.Point(0, 0);
            this.lst.Name = "lst";
            this.lst.SelectedIndex = -1;
            this.lst.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(60)))), ((int)(((byte)(114)))));
            this.lst.Size = new System.Drawing.Size(288, 407);
            this.lst.StatusFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lst.TabIndex = 4;
            this.lst.TitleFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            // 
            // pnlLibrary
            // 
            this.pnlLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLibrary.BackColor = System.Drawing.Color.Transparent;
            this.pnlLibrary.Controls.Add(this.brwLibrary);
            this.pnlLibrary.Location = new System.Drawing.Point(174, 51);
            this.pnlLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLibrary.Name = "pnlLibrary";
            this.pnlLibrary.Size = new System.Drawing.Size(843, 409);
            this.pnlLibrary.TabIndex = 5;
            // 
            // brwLibrary
            // 
            this.brwLibrary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brwLibrary.BackColor = System.Drawing.Color.White;
            this.brwLibrary.BorderColor = System.Drawing.Color.Red;
            this.brwLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brwLibrary.Location = new System.Drawing.Point(0, 0);
            this.brwLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.brwLibrary.Name = "brwLibrary";
            this.brwLibrary.Size = new System.Drawing.Size(843, 409);
            this.brwLibrary.TabIndex = 2;
            this.brwLibrary.Url = null;
            // 
            // NimbusMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 608);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnGames);
            this.Controls.Add(this.btnLibrary);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.maximizeButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.pnlCollection);
            this.Controls.Add(this.pnlLibrary);
            this.MinimumSize = new System.Drawing.Size(850, 300);
            this.Name = "NimbusMain";
            this.Text = "Nimbus";
            this.pnlCollection.ResumeLayout(false);
            this.pnlLibrary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BorderlessWebkit brwCollection;
        private BorderlessWebkit brwLibrary;
        private Controls.GamesListBox lst;
        private TransparentPanel pnlLibrary;
        private TransparentPanel pnlCollection;
        private ImageButton closeButton;
        private ImageButton maximizeButton;
        private ImageButton minimizeButton;
        private TextButton btnLibrary;
        private TextButton btnGames;
        private ImageButton btnBack;
        private ImageButton btnForward;
        

    }
}

