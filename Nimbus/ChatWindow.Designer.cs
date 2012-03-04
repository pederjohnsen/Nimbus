using System.Windows.Forms;
namespace Nimbus
{
    partial class ChatWindow
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
            this.txtChat = new TextBox();
            this.nexusText1 = new TextBox();
            this.SuspendLayout();
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(12, 40);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(469, 220);
            this.txtChat.TabIndex = 0;
            // 
            // nexusText1
            // 
            this.nexusText1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nexusText1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nexusText1.Location = new System.Drawing.Point(12, 263);
            this.nexusText1.Margin = new System.Windows.Forms.Padding(0);
            this.nexusText1.Name = "nexusText1";
            this.nexusText1.Size = new System.Drawing.Size(469, 28);
            this.nexusText1.TabIndex = 1;
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 295);
            this.Controls.Add(this.nexusText1);
            this.Controls.Add(this.txtChat);
            this.Name = "ChatWindow";
            this.Text = "ChatWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtChat;
        private TextBox nexusText1;
    }
}