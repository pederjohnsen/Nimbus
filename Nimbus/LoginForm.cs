using System;
using System.Collections.Generic;
using System.Text;
using Nimbus.Controls;
using Nimbus.Network;
using Nimbus.Theming;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nimbus
{
    [System.ComponentModel.DesignerCategory("form")]
    class LoginForm : NimbusForm
    {
        private System.Windows.Forms.TextBox txtEmailAddress;
        private System.Windows.Forms.TextBox txtPassword;
        private TextButton btnCancel;
        private TransparentLabel lblEmailAddress;
        private TransparentLabel lblPassword;
        private TextButton btnLogin;

        static LoginResult loginResult;

        private void InitializeComponent()
        {
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new Nimbus.Controls.TextButton();
            this.btnCancel = new Nimbus.Controls.TextButton();
            this.lblEmailAddress = new Nimbus.Controls.TransparentLabel();
            this.lblPassword = new Nimbus.Controls.TransparentLabel();
            this.SuspendLayout();
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(104, 54);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(229, 20);
            this.txtEmailAddress.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(104, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(229, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogin.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.HasShadow = false;
            this.btnLogin.IsToggle = false;
            this.btnLogin.Location = new System.Drawing.Point(58, 121);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(78, 31);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.Toggled = false;
            this.btnLogin.Tracking = 0;
            this.btnLogin.YOffset = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCancel.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.HasShadow = false;
            this.btnCancel.IsToggle = false;
            this.btnCancel.Location = new System.Drawing.Point(186, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 31);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Toggled = false;
            this.btnCancel.Tracking = 0;
            this.btnCancel.YOffset = 0;
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.ForeColor = System.Drawing.Color.White;
            this.lblEmailAddress.Location = new System.Drawing.Point(6, 54);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(92, 20);
            this.lblEmailAddress.TabIndex = 5;
            this.lblEmailAddress.Text = "Email Address:";
            this.lblEmailAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.ForeColor = System.Drawing.Color.White;
            this.lblPassword.Location = new System.Drawing.Point(9, 80);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(89, 20);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LoginForm
            // 
            this.AllowResize = false;
            this.ClientSize = new System.Drawing.Size(348, 192);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblEmailAddress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtEmailAddress);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public LoginForm(NimbusTheme theme)
        {
            this.Theme = theme;
            InitializeComponent();
            btnCancel.Clicked += new EventHandler(btnCancel_Clicked);
            btnLogin.Clicked += new EventHandler(btnLogin_Clicked);
        }

        void btnLogin_Clicked(object sender, EventArgs e)
        {
            LoginResult lr = Login();
            
            if (lr == null) return;

            if (lr.Passed)
            {
                loginResult = lr;
                Close();
            }
        }

        void btnCancel_Clicked(object sender, EventArgs e)
        {
            loginResult = new LoginResult(false);
            Close();
        }

        LoginResult Login()
        {
            if (txtEmailAddress.Text.Length > 0 && txtPassword.Text.Length > 0)
            {
                LoginManager lm = new LoginManager();
                LoginResult lr = lm.Login(txtEmailAddress.Text, txtPassword.Text);
                return lr;

            }
            else return null;
        }


        public static LoginResult ShowLogin(NimbusTheme theme)
        {

            LoginForm logfrm = new LoginForm(theme);

            logfrm.ShowDialog();
            return LoginForm.loginResult;

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Brush lgb2 = new LinearGradientBrush(new Point(Width / 2, Height), new Point(Width / 2, CaptionHeight), Color.FromArgb(44, 66, 69), Color.FromArgb(28, 47, 53));
            e.Graphics.FillRectangle(lgb2, new Rectangle(0, CaptionHeight, Width, Height - CaptionHeight));
            base.OnPaint(e);
        }

    }
}
