using System.Windows.Forms;
namespace Nimbus
{
    partial class NimbusMessageBox
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
            this.lblText = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.chkNeverShow = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(12, 49);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(298, 49);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "This is example text for this label. Omg I lovethis text so god damned, freaking " +
                "much!";
            this.lblText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnYes
            // 
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.Location = new System.Drawing.Point(164, 138);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(146, 51);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "Yes";
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Location = new System.Drawing.Point(15, 138);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(143, 50);
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "No";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // chkNeverShow
            // 
            this.chkNeverShow.Location = new System.Drawing.Point(87, 101);
            this.chkNeverShow.Name = "chkNeverShow";
            this.chkNeverShow.Size = new System.Drawing.Size(151, 22);
            this.chkNeverShow.TabIndex = 3;
            this.chkNeverShow.Text = "Never show this again";
            this.chkNeverShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkNeverShow.UseVisualStyleBackColor = true;
            // 
            // NexusMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(322, 201);
            this.Controls.Add(this.chkNeverShow);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblText);
            this.HasCaptionBar = true;
            this.Name = "NexusMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NexusMessageBox";
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblText;
        private Button btnYes;
        private Button btnNo;
        private System.Windows.Forms.CheckBox chkNeverShow;

    }
}