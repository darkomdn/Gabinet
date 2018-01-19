namespace Logowanie001
{
    partial class Form1
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.btZamknijLogowanie = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnZaloguj2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(214, 178);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(71, 32);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Zaloguj";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.zaloguj_Click);
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(108, 77);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(177, 20);
            this.tbUser.TabIndex = 1;
            this.tbUser.Text = "admin";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(108, 127);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(177, 20);
            this.tbPass.TabIndex = 2;
            this.tbPass.Text = "admin";
            // 
            // btZamknijLogowanie
            // 
            this.btZamknijLogowanie.Location = new System.Drawing.Point(108, 178);
            this.btZamknijLogowanie.Name = "btZamknijLogowanie";
            this.btZamknijLogowanie.Size = new System.Drawing.Size(71, 32);
            this.btZamknijLogowanie.TabIndex = 3;
            this.btZamknijLogowanie.Text = "Zamknij";
            this.btZamknijLogowanie.UseVisualStyleBackColor = true;
            this.btZamknijLogowanie.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Login:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hasło:";
            // 
            // btnZaloguj2
            // 
            this.btnZaloguj2.Location = new System.Drawing.Point(13, 237);
            this.btnZaloguj2.Name = "btnZaloguj2";
            this.btnZaloguj2.Size = new System.Drawing.Size(75, 23);
            this.btnZaloguj2.TabIndex = 6;
            this.btnZaloguj2.Text = "Zaloguj2";
            this.btnZaloguj2.UseVisualStyleBackColor = true;
            this.btnZaloguj2.Visible = false;
            this.btnZaloguj2.Click += new System.EventHandler(this.btnZaloguj2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 282);
            this.Controls.Add(this.btnZaloguj2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btZamknijLogowanie);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.btnLogin);
            this.Name = "Form1";
            this.Text = "Logowanie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Button btZamknijLogowanie;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnZaloguj2;
    }
}

