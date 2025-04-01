namespace AdminUI.Forms
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            BackArrowPictureBox = new PictureBox();
            LoginPictureBox = new PictureBox();
            ErrorLabel = new Label();
            PasswordTextBox = new TextBox();
            PasswordPictureBox = new PictureBox();
            PasswordLabel = new Label();
            UsernameTextBox = new TextBox();
            UsernamePictureBox = new PictureBox();
            UsernameLabel = new Label();
            HeaderLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)BackArrowPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LoginPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UsernamePictureBox).BeginInit();
            SuspendLayout();
            // 
            // BackArrowPictureBox
            // 
            BackArrowPictureBox.Image = (Image)resources.GetObject("BackArrowPictureBox.Image");
            BackArrowPictureBox.Location = new Point(14, 13);
            BackArrowPictureBox.Name = "BackArrowPictureBox";
            BackArrowPictureBox.Size = new Size(38, 36);
            BackArrowPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            BackArrowPictureBox.TabIndex = 24;
            BackArrowPictureBox.TabStop = false;
            BackArrowPictureBox.Visible = false;
            BackArrowPictureBox.Click += BackArrowPictureBox_Click;
            // 
            // LoginPictureBox
            // 
            LoginPictureBox.Image = (Image)resources.GetObject("LoginPictureBox.Image");
            LoginPictureBox.Location = new Point(358, 324);
            LoginPictureBox.Name = "LoginPictureBox";
            LoginPictureBox.Size = new Size(123, 36);
            LoginPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            LoginPictureBox.TabIndex = 20;
            LoginPictureBox.TabStop = false;
            LoginPictureBox.Click += LoginPictureBox_Click;
            LoginPictureBox.Paint += LoginPictureBox_Paint;
            // 
            // ErrorLabel
            // 
            ErrorLabel.Dock = DockStyle.Bottom;
            ErrorLabel.Font = new Font("Segoe UI Emoji", 12F);
            ErrorLabel.ForeColor = Color.Red;
            ErrorLabel.Location = new Point(0, 470);
            ErrorLabel.Name = "ErrorLabel";
            ErrorLabel.Size = new Size(841, 21);
            ErrorLabel.TabIndex = 23;
            ErrorLabel.Text = "<error>";
            ErrorLabel.TextAlign = ContentAlignment.MiddleRight;
            ErrorLabel.Visible = false;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.BackColor = Color.FromArgb(45, 45, 45);
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.ForeColor = Color.White;
            PasswordTextBox.Location = new Point(300, 274);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(239, 22);
            PasswordTextBox.TabIndex = 15;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // PasswordPictureBox
            // 
            PasswordPictureBox.Image = (Image)resources.GetObject("PasswordPictureBox.Image");
            PasswordPictureBox.Location = new Point(290, 258);
            PasswordPictureBox.Name = "PasswordPictureBox";
            PasswordPictureBox.Size = new Size(258, 55);
            PasswordPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PasswordPictureBox.TabIndex = 19;
            PasswordPictureBox.TabStop = false;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordLabel.ForeColor = Color.Silver;
            PasswordLabel.Location = new Point(286, 237);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(76, 21);
            PasswordLabel.TabIndex = 18;
            PasswordLabel.Text = "Password";
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.BackColor = Color.FromArgb(45, 45, 45);
            UsernameTextBox.BorderStyle = BorderStyle.None;
            UsernameTextBox.ForeColor = Color.White;
            UsernameTextBox.Location = new Point(300, 195);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(239, 22);
            UsernameTextBox.TabIndex = 14;
            // 
            // UsernamePictureBox
            // 
            UsernamePictureBox.Image = (Image)resources.GetObject("UsernamePictureBox.Image");
            UsernamePictureBox.Location = new Point(290, 179);
            UsernamePictureBox.Name = "UsernamePictureBox";
            UsernamePictureBox.Size = new Size(258, 55);
            UsernamePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            UsernamePictureBox.TabIndex = 17;
            UsernamePictureBox.TabStop = false;
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            UsernameLabel.ForeColor = Color.Silver;
            UsernameLabel.Location = new Point(286, 158);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(81, 21);
            UsernameLabel.TabIndex = 16;
            UsernameLabel.Text = "Username";
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HeaderLabel.ForeColor = Color.White;
            HeaderLabel.Location = new Point(291, 64);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new Size(256, 45);
            HeaderLabel.TabIndex = 13;
            HeaderLabel.Text = "ZeroBank Admin";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(841, 491);
            Controls.Add(BackArrowPictureBox);
            Controls.Add(LoginPictureBox);
            Controls.Add(ErrorLabel);
            Controls.Add(PasswordTextBox);
            Controls.Add(PasswordPictureBox);
            Controls.Add(PasswordLabel);
            Controls.Add(UsernameTextBox);
            Controls.Add(UsernamePictureBox);
            Controls.Add(UsernameLabel);
            Controls.Add(HeaderLabel);
            Font = new Font("Segoe UI Emoji", 12F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)BackArrowPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)LoginPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)UsernamePictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox BackArrowPictureBox;
        private PictureBox LoginPictureBox;
        private Label ErrorLabel;
        private TextBox PasswordTextBox;
        private PictureBox PasswordPictureBox;
        private Label PasswordLabel;
        private TextBox UsernameTextBox;
        private PictureBox UsernamePictureBox;
        private Label UsernameLabel;
        private Label HeaderLabel;
    }
}
