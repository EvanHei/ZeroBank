namespace WinFormsUI.Forms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            HeaderLabel = new Label();
            UsernameLabel = new Label();
            UsernameTextBox = new TextBox();
            UsernamePictureBox = new PictureBox();
            PasswordTextBox = new TextBox();
            PasswordPictureBox = new PictureBox();
            PasswordLabel = new Label();
            ErrorLabel = new Label();
            EnrollPictureBox = new PictureBox();
            SignUpPictureBox = new PictureBox();
            LoginPictureBox = new PictureBox();
            BackArrowPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)UsernamePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)EnrollPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SignUpPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LoginPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BackArrowPictureBox).BeginInit();
            SuspendLayout();
            // 
            // HeaderLabel
            // 
            HeaderLabel.AutoSize = true;
            HeaderLabel.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HeaderLabel.ForeColor = Color.White;
            HeaderLabel.Location = new Point(337, 63);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new Size(153, 45);
            HeaderLabel.TabIndex = 0;
            HeaderLabel.Text = "ZeroBank";
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            UsernameLabel.ForeColor = Color.Silver;
            UsernameLabel.Location = new Point(284, 157);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(81, 21);
            UsernameLabel.TabIndex = 2;
            UsernameLabel.Text = "Username";
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.BackColor = Color.FromArgb(45, 45, 45);
            UsernameTextBox.BorderStyle = BorderStyle.None;
            UsernameTextBox.ForeColor = Color.White;
            UsernameTextBox.Location = new Point(298, 194);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(239, 22);
            UsernameTextBox.TabIndex = 1;
            // 
            // UsernamePictureBox
            // 
            UsernamePictureBox.Image = (Image)resources.GetObject("UsernamePictureBox.Image");
            UsernamePictureBox.Location = new Point(288, 178);
            UsernamePictureBox.Name = "UsernamePictureBox";
            UsernamePictureBox.Size = new Size(258, 55);
            UsernamePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            UsernamePictureBox.TabIndex = 5;
            UsernamePictureBox.TabStop = false;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.BackColor = Color.FromArgb(45, 45, 45);
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.ForeColor = Color.White;
            PasswordTextBox.Location = new Point(298, 273);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(239, 22);
            PasswordTextBox.TabIndex = 2;
            PasswordTextBox.UseSystemPasswordChar = true;
            PasswordTextBox.KeyDown += PasswordTextBox_KeyDown;
            // 
            // PasswordPictureBox
            // 
            PasswordPictureBox.Image = (Image)resources.GetObject("PasswordPictureBox.Image");
            PasswordPictureBox.Location = new Point(288, 257);
            PasswordPictureBox.Name = "PasswordPictureBox";
            PasswordPictureBox.Size = new Size(258, 55);
            PasswordPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PasswordPictureBox.TabIndex = 8;
            PasswordPictureBox.TabStop = false;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordLabel.ForeColor = Color.Silver;
            PasswordLabel.Location = new Point(284, 236);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(76, 21);
            PasswordLabel.TabIndex = 6;
            PasswordLabel.Text = "Password";
            // 
            // ErrorLabel
            // 
            ErrorLabel.AutoSize = true;
            ErrorLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ErrorLabel.ForeColor = Color.Red;
            ErrorLabel.Location = new Point(380, 371);
            ErrorLabel.Name = "ErrorLabel";
            ErrorLabel.Size = new Size(67, 21);
            ErrorLabel.TabIndex = 11;
            ErrorLabel.Text = "<error>";
            ErrorLabel.Visible = false;
            // 
            // EnrollPictureBox
            // 
            EnrollPictureBox.Image = (Image)resources.GetObject("EnrollPictureBox.Image");
            EnrollPictureBox.Location = new Point(352, 323);
            EnrollPictureBox.Name = "EnrollPictureBox";
            EnrollPictureBox.Size = new Size(123, 36);
            EnrollPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            EnrollPictureBox.TabIndex = 11;
            EnrollPictureBox.TabStop = false;
            EnrollPictureBox.Visible = false;
            EnrollPictureBox.Click += EnrollPictureBox_Click;
            EnrollPictureBox.Paint += EnrollPictureBox_Paint;
            // 
            // SignUpPictureBox
            // 
            SignUpPictureBox.Image = (Image)resources.GetObject("SignUpPictureBox.Image");
            SignUpPictureBox.Location = new Point(423, 323);
            SignUpPictureBox.Name = "SignUpPictureBox";
            SignUpPictureBox.Size = new Size(123, 36);
            SignUpPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            SignUpPictureBox.TabIndex = 10;
            SignUpPictureBox.TabStop = false;
            SignUpPictureBox.Click += SignUpPictureBox_Click;
            SignUpPictureBox.Paint += SignUpPictureBox_Paint;
            // 
            // LoginPictureBox
            // 
            LoginPictureBox.Image = (Image)resources.GetObject("LoginPictureBox.Image");
            LoginPictureBox.Location = new Point(288, 323);
            LoginPictureBox.Name = "LoginPictureBox";
            LoginPictureBox.Size = new Size(123, 36);
            LoginPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            LoginPictureBox.TabIndex = 9;
            LoginPictureBox.TabStop = false;
            LoginPictureBox.Click += LoginPictureBox_Click;
            LoginPictureBox.Paint += LoginPictureBox_Paint;
            // 
            // BackArrowPictureBox
            // 
            BackArrowPictureBox.Image = (Image)resources.GetObject("BackArrowPictureBox.Image");
            BackArrowPictureBox.Location = new Point(12, 12);
            BackArrowPictureBox.Name = "BackArrowPictureBox";
            BackArrowPictureBox.Size = new Size(38, 36);
            BackArrowPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            BackArrowPictureBox.TabIndex = 12;
            BackArrowPictureBox.TabStop = false;
            BackArrowPictureBox.Visible = false;
            BackArrowPictureBox.Click += BackArrowPictureBox_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(841, 491);
            Controls.Add(BackArrowPictureBox);
            Controls.Add(LoginPictureBox);
            Controls.Add(SignUpPictureBox);
            Controls.Add(EnrollPictureBox);
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
            ((System.ComponentModel.ISupportInitialize)UsernamePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)EnrollPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)SignUpPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)LoginPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BackArrowPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label HeaderLabel;
        private Label UsernameLabel;
        private TextBox UsernameTextBox;
        private PictureBox UsernamePictureBox;
        private TextBox PasswordTextBox;
        private PictureBox PasswordPictureBox;
        private Label PasswordLabel;
        private Label ErrorLabel;
        private PictureBox EnrollPictureBox;
        private PictureBox SignUpPictureBox;
        private PictureBox LoginPictureBox;
        private PictureBox BackArrowPictureBox;
    }
}