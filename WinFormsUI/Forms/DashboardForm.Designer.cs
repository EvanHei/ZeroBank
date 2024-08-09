namespace WinFormsUI
{
    partial class DashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            SidebarPanel = new Panel();
            AccountsLabel = new Label();
            DashboardLabel = new Label();
            AccountsPanel = new Panel();
            AccountsListBox = new ListBox();
            PasswordArrowPictureBox = new PictureBox();
            AccountsListPictureBox = new PictureBox();
            PasswordTextBox = new TextBox();
            PasswordPictureBox = new PictureBox();
            PasswordLabel = new Label();
            AccountsPictureBox = new PictureBox();
            AccountDetailsPanel = new Panel();
            ChartPanel = new Panel();
            SwitchViewPictureBox = new PictureBox();
            TransactionsNumberPictureBox = new PictureBox();
            TransactionsListPictureBox = new PictureBox();
            BalancePictureBox = new PictureBox();
            ChartPictureBox = new PictureBox();
            DashboardPanel = new Panel();
            MakeTransactionLabel = new Label();
            MakeTransactionIconPictureBox = new PictureBox();
            MakeTransactionPictureBox = new PictureBox();
            CreateAccountLabel = new Label();
            CreateAccountIconPictureBox = new PictureBox();
            CreateAccountPictureBox = new PictureBox();
            LogsLabel = new Label();
            LogsIconPictureBox = new PictureBox();
            LogsPictureBox = new PictureBox();
            UserGuideLabel = new Label();
            UserGuideIconPictureBox = new PictureBox();
            UserGuidePictureBox = new PictureBox();
            SidebarPanel.SuspendLayout();
            AccountsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PasswordArrowPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountsListPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPictureBox).BeginInit();
            AccountDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SwitchViewPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsNumberPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsListPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BalancePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ChartPictureBox).BeginInit();
            DashboardPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MakeTransactionIconPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MakeTransactionPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CreateAccountIconPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CreateAccountPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LogsIconPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LogsPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UserGuideIconPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UserGuidePictureBox).BeginInit();
            SuspendLayout();
            // 
            // SidebarPanel
            // 
            SidebarPanel.BorderStyle = BorderStyle.FixedSingle;
            SidebarPanel.Controls.Add(AccountsLabel);
            SidebarPanel.Controls.Add(DashboardLabel);
            SidebarPanel.Location = new Point(-3, -6);
            SidebarPanel.Name = "SidebarPanel";
            SidebarPanel.Size = new Size(196, 530);
            SidebarPanel.TabIndex = 0;
            // 
            // AccountsLabel
            // 
            AccountsLabel.AutoSize = true;
            AccountsLabel.ForeColor = Color.FromArgb(172, 172, 172);
            AccountsLabel.Location = new Point(30, 178);
            AccountsLabel.Name = "AccountsLabel";
            AccountsLabel.Size = new Size(73, 21);
            AccountsLabel.TabIndex = 2;
            AccountsLabel.Text = "Accounts";
            AccountsLabel.Click += AccountsLabel_Click;
            // 
            // DashboardLabel
            // 
            DashboardLabel.AutoSize = true;
            DashboardLabel.BackColor = Color.FromArgb(45, 45, 45);
            DashboardLabel.ForeColor = Color.FromArgb(172, 172, 172);
            DashboardLabel.Location = new Point(30, 130);
            DashboardLabel.Name = "DashboardLabel";
            DashboardLabel.Size = new Size(86, 21);
            DashboardLabel.TabIndex = 1;
            DashboardLabel.Text = "Dashboard";
            DashboardLabel.Click += DashboardLabel_Click;
            // 
            // AccountsPanel
            // 
            AccountsPanel.Controls.Add(AccountsListBox);
            AccountsPanel.Controls.Add(PasswordArrowPictureBox);
            AccountsPanel.Controls.Add(AccountsListPictureBox);
            AccountsPanel.Controls.Add(PasswordTextBox);
            AccountsPanel.Controls.Add(PasswordPictureBox);
            AccountsPanel.Controls.Add(PasswordLabel);
            AccountsPanel.Controls.Add(AccountsPictureBox);
            AccountsPanel.Location = new Point(195, -6);
            AccountsPanel.Name = "AccountsPanel";
            AccountsPanel.Size = new Size(987, 530);
            AccountsPanel.TabIndex = 10;
            // 
            // AccountsListBox
            // 
            AccountsListBox.BackColor = Color.FromArgb(79, 79, 79);
            AccountsListBox.BorderStyle = BorderStyle.None;
            AccountsListBox.DrawMode = DrawMode.OwnerDrawVariable;
            AccountsListBox.ForeColor = Color.Silver;
            AccountsListBox.FormattingEnabled = true;
            AccountsListBox.ItemHeight = 21;
            AccountsListBox.Location = new Point(223, 171);
            AccountsListBox.Name = "AccountsListBox";
            AccountsListBox.Size = new Size(520, 315);
            AccountsListBox.TabIndex = 14;
            AccountsListBox.DrawItem += AccountsListBox_DrawItem;
            AccountsListBox.MeasureItem += AccountsListBox_MeasureItem;
            // 
            // PasswordArrowPictureBox
            // 
            PasswordArrowPictureBox.Image = (Image)resources.GetObject("PasswordArrowPictureBox.Image");
            PasswordArrowPictureBox.Location = new Point(723, 104);
            PasswordArrowPictureBox.Name = "PasswordArrowPictureBox";
            PasswordArrowPictureBox.Size = new Size(20, 20);
            PasswordArrowPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PasswordArrowPictureBox.TabIndex = 13;
            PasswordArrowPictureBox.TabStop = false;
            PasswordArrowPictureBox.Click += PasswordArrowPictureBox_Click;
            // 
            // AccountsListPictureBox
            // 
            AccountsListPictureBox.Image = (Image)resources.GetObject("AccountsListPictureBox.Image");
            AccountsListPictureBox.Location = new Point(210, 160);
            AccountsListPictureBox.Name = "AccountsListPictureBox";
            AccountsListPictureBox.Size = new Size(544, 346);
            AccountsListPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            AccountsListPictureBox.TabIndex = 12;
            AccountsListPictureBox.TabStop = false;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.BackColor = Color.FromArgb(45, 45, 45);
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.ForeColor = Color.White;
            PasswordTextBox.Location = new Point(507, 103);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(239, 22);
            PasswordTextBox.TabIndex = 10;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // PasswordPictureBox
            // 
            PasswordPictureBox.Image = (Image)resources.GetObject("PasswordPictureBox.Image");
            PasswordPictureBox.Location = new Point(497, 88);
            PasswordPictureBox.Name = "PasswordPictureBox";
            PasswordPictureBox.Size = new Size(257, 55);
            PasswordPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PasswordPictureBox.TabIndex = 11;
            PasswordPictureBox.TabStop = false;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PasswordLabel.ForeColor = Color.Silver;
            PasswordLabel.Location = new Point(493, 65);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(76, 21);
            PasswordLabel.TabIndex = 9;
            PasswordLabel.Text = "Password";
            // 
            // AccountsPictureBox
            // 
            AccountsPictureBox.Image = (Image)resources.GetObject("AccountsPictureBox.Image");
            AccountsPictureBox.Location = new Point(210, 55);
            AccountsPictureBox.Name = "AccountsPictureBox";
            AccountsPictureBox.Size = new Size(257, 81);
            AccountsPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountsPictureBox.TabIndex = 2;
            AccountsPictureBox.TabStop = false;
            AccountsPictureBox.Paint += AccountsPictureBox_Paint;
            // 
            // AccountDetailsPanel
            // 
            AccountDetailsPanel.Controls.Add(ChartPanel);
            AccountDetailsPanel.Controls.Add(SwitchViewPictureBox);
            AccountDetailsPanel.Controls.Add(TransactionsNumberPictureBox);
            AccountDetailsPanel.Controls.Add(TransactionsListPictureBox);
            AccountDetailsPanel.Controls.Add(BalancePictureBox);
            AccountDetailsPanel.Controls.Add(ChartPictureBox);
            AccountDetailsPanel.Location = new Point(195, -6);
            AccountDetailsPanel.Name = "AccountDetailsPanel";
            AccountDetailsPanel.Size = new Size(987, 530);
            AccountDetailsPanel.TabIndex = 1;
            // 
            // ChartPanel
            // 
            ChartPanel.Location = new Point(58, 142);
            ChartPanel.Name = "ChartPanel";
            ChartPanel.Size = new Size(602, 300);
            ChartPanel.TabIndex = 7;
            // 
            // SwitchViewPictureBox
            // 
            SwitchViewPictureBox.Image = (Image)resources.GetObject("SwitchViewPictureBox.Image");
            SwitchViewPictureBox.Location = new Point(547, 29);
            SwitchViewPictureBox.Name = "SwitchViewPictureBox";
            SwitchViewPictureBox.Size = new Size(422, 61);
            SwitchViewPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            SwitchViewPictureBox.TabIndex = 6;
            SwitchViewPictureBox.TabStop = false;
            // 
            // TransactionsNumberPictureBox
            // 
            TransactionsNumberPictureBox.Image = (Image)resources.GetObject("TransactionsNumberPictureBox.Image");
            TransactionsNumberPictureBox.Location = new Point(334, 29);
            TransactionsNumberPictureBox.Name = "TransactionsNumberPictureBox";
            TransactionsNumberPictureBox.Size = new Size(184, 61);
            TransactionsNumberPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            TransactionsNumberPictureBox.TabIndex = 5;
            TransactionsNumberPictureBox.TabStop = false;
            // 
            // TransactionsListPictureBox
            // 
            TransactionsListPictureBox.Image = (Image)resources.GetObject("TransactionsListPictureBox.Image");
            TransactionsListPictureBox.Location = new Point(696, 108);
            TransactionsListPictureBox.Name = "TransactionsListPictureBox";
            TransactionsListPictureBox.Size = new Size(275, 406);
            TransactionsListPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            TransactionsListPictureBox.TabIndex = 4;
            TransactionsListPictureBox.TabStop = false;
            // 
            // BalancePictureBox
            // 
            BalancePictureBox.Image = (Image)resources.GetObject("BalancePictureBox.Image");
            BalancePictureBox.Location = new Point(38, 29);
            BalancePictureBox.Name = "BalancePictureBox";
            BalancePictureBox.Size = new Size(275, 61);
            BalancePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BalancePictureBox.TabIndex = 0;
            BalancePictureBox.TabStop = false;
            // 
            // ChartPictureBox
            // 
            ChartPictureBox.Image = (Image)resources.GetObject("ChartPictureBox.Image");
            ChartPictureBox.Location = new Point(38, 108);
            ChartPictureBox.Name = "ChartPictureBox";
            ChartPictureBox.Size = new Size(643, 369);
            ChartPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ChartPictureBox.TabIndex = 8;
            ChartPictureBox.TabStop = false;
            // 
            // DashboardPanel
            // 
            DashboardPanel.Controls.Add(MakeTransactionLabel);
            DashboardPanel.Controls.Add(MakeTransactionIconPictureBox);
            DashboardPanel.Controls.Add(MakeTransactionPictureBox);
            DashboardPanel.Controls.Add(CreateAccountLabel);
            DashboardPanel.Controls.Add(CreateAccountIconPictureBox);
            DashboardPanel.Controls.Add(CreateAccountPictureBox);
            DashboardPanel.Controls.Add(LogsLabel);
            DashboardPanel.Controls.Add(LogsIconPictureBox);
            DashboardPanel.Controls.Add(LogsPictureBox);
            DashboardPanel.Controls.Add(UserGuideLabel);
            DashboardPanel.Controls.Add(UserGuideIconPictureBox);
            DashboardPanel.Controls.Add(UserGuidePictureBox);
            DashboardPanel.Location = new Point(195, -6);
            DashboardPanel.Name = "DashboardPanel";
            DashboardPanel.Size = new Size(987, 530);
            DashboardPanel.TabIndex = 9;
            // 
            // MakeTransactionLabel
            // 
            MakeTransactionLabel.AutoSize = true;
            MakeTransactionLabel.ForeColor = Color.FromArgb(146, 146, 146);
            MakeTransactionLabel.Location = new Point(519, 380);
            MakeTransactionLabel.Name = "MakeTransactionLabel";
            MakeTransactionLabel.Size = new Size(129, 21);
            MakeTransactionLabel.TabIndex = 15;
            MakeTransactionLabel.Text = "Make transaction";
            // 
            // MakeTransactionIconPictureBox
            // 
            MakeTransactionIconPictureBox.Image = (Image)resources.GetObject("MakeTransactionIconPictureBox.Image");
            MakeTransactionIconPictureBox.Location = new Point(553, 311);
            MakeTransactionIconPictureBox.Name = "MakeTransactionIconPictureBox";
            MakeTransactionIconPictureBox.Size = new Size(61, 57);
            MakeTransactionIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            MakeTransactionIconPictureBox.TabIndex = 14;
            MakeTransactionIconPictureBox.TabStop = false;
            // 
            // MakeTransactionPictureBox
            // 
            MakeTransactionPictureBox.Image = (Image)resources.GetObject("MakeTransactionPictureBox.Image");
            MakeTransactionPictureBox.Location = new Point(507, 278);
            MakeTransactionPictureBox.Name = "MakeTransactionPictureBox";
            MakeTransactionPictureBox.Size = new Size(153, 153);
            MakeTransactionPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            MakeTransactionPictureBox.TabIndex = 13;
            MakeTransactionPictureBox.TabStop = false;
            // 
            // CreateAccountLabel
            // 
            CreateAccountLabel.AutoSize = true;
            CreateAccountLabel.ForeColor = Color.FromArgb(146, 146, 146);
            CreateAccountLabel.Location = new Point(334, 380);
            CreateAccountLabel.Name = "CreateAccountLabel";
            CreateAccountLabel.Size = new Size(113, 21);
            CreateAccountLabel.TabIndex = 12;
            CreateAccountLabel.Text = "Create account";
            // 
            // CreateAccountIconPictureBox
            // 
            CreateAccountIconPictureBox.Image = (Image)resources.GetObject("CreateAccountIconPictureBox.Image");
            CreateAccountIconPictureBox.Location = new Point(367, 320);
            CreateAccountIconPictureBox.Name = "CreateAccountIconPictureBox";
            CreateAccountIconPictureBox.Size = new Size(47, 39);
            CreateAccountIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CreateAccountIconPictureBox.TabIndex = 11;
            CreateAccountIconPictureBox.TabStop = false;
            // 
            // CreateAccountPictureBox
            // 
            CreateAccountPictureBox.Image = (Image)resources.GetObject("CreateAccountPictureBox.Image");
            CreateAccountPictureBox.Location = new Point(314, 278);
            CreateAccountPictureBox.Name = "CreateAccountPictureBox";
            CreateAccountPictureBox.Size = new Size(153, 153);
            CreateAccountPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CreateAccountPictureBox.TabIndex = 10;
            CreateAccountPictureBox.TabStop = false;
            // 
            // LogsLabel
            // 
            LogsLabel.AutoSize = true;
            LogsLabel.ForeColor = Color.FromArgb(146, 146, 146);
            LogsLabel.Location = new Point(562, 191);
            LogsLabel.Name = "LogsLabel";
            LogsLabel.Size = new Size(43, 21);
            LogsLabel.TabIndex = 9;
            LogsLabel.Text = "Logs";
            // 
            // LogsIconPictureBox
            // 
            LogsIconPictureBox.Image = (Image)resources.GetObject("LogsIconPictureBox.Image");
            LogsIconPictureBox.Location = new Point(560, 131);
            LogsIconPictureBox.Name = "LogsIconPictureBox";
            LogsIconPictureBox.Size = new Size(47, 39);
            LogsIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            LogsIconPictureBox.TabIndex = 8;
            LogsIconPictureBox.TabStop = false;
            // 
            // LogsPictureBox
            // 
            LogsPictureBox.Image = (Image)resources.GetObject("LogsPictureBox.Image");
            LogsPictureBox.Location = new Point(507, 89);
            LogsPictureBox.Name = "LogsPictureBox";
            LogsPictureBox.Size = new Size(153, 153);
            LogsPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            LogsPictureBox.TabIndex = 7;
            LogsPictureBox.TabStop = false;
            // 
            // UserGuideLabel
            // 
            UserGuideLabel.AutoSize = true;
            UserGuideLabel.ForeColor = Color.FromArgb(146, 146, 146);
            UserGuideLabel.Location = new Point(347, 191);
            UserGuideLabel.Name = "UserGuideLabel";
            UserGuideLabel.Size = new Size(87, 21);
            UserGuideLabel.TabIndex = 6;
            UserGuideLabel.Text = "User Guide";
            // 
            // UserGuideIconPictureBox
            // 
            UserGuideIconPictureBox.Image = (Image)resources.GetObject("UserGuideIconPictureBox.Image");
            UserGuideIconPictureBox.Location = new Point(367, 131);
            UserGuideIconPictureBox.Name = "UserGuideIconPictureBox";
            UserGuideIconPictureBox.Size = new Size(47, 39);
            UserGuideIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            UserGuideIconPictureBox.TabIndex = 5;
            UserGuideIconPictureBox.TabStop = false;
            // 
            // UserGuidePictureBox
            // 
            UserGuidePictureBox.Image = (Image)resources.GetObject("UserGuidePictureBox.Image");
            UserGuidePictureBox.Location = new Point(314, 89);
            UserGuidePictureBox.Name = "UserGuidePictureBox";
            UserGuidePictureBox.Size = new Size(153, 153);
            UserGuidePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            UserGuidePictureBox.TabIndex = 1;
            UserGuidePictureBox.TabStop = false;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(1176, 520);
            Controls.Add(SidebarPanel);
            Controls.Add(AccountsPanel);
            Controls.Add(DashboardPanel);
            Controls.Add(AccountDetailsPanel);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ZeroBank";
            Load += DashboardForm_Load;
            SidebarPanel.ResumeLayout(false);
            SidebarPanel.PerformLayout();
            AccountsPanel.ResumeLayout(false);
            AccountsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PasswordArrowPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountsListPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)PasswordPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPictureBox).EndInit();
            AccountDetailsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SwitchViewPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsNumberPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsListPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BalancePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ChartPictureBox).EndInit();
            DashboardPanel.ResumeLayout(false);
            DashboardPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MakeTransactionIconPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)MakeTransactionPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)CreateAccountIconPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)CreateAccountPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)LogsIconPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)LogsPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)UserGuideIconPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)UserGuidePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel SidebarPanel;
        private Label AccountsLabel;
        private Label DashboardLabel;
        private Panel AccountDetailsPanel;
        private PictureBox SwitchViewPictureBox;
        private PictureBox TransactionsNumberPictureBox;
        private PictureBox TransactionsListPictureBox;
        private PictureBox BalancePictureBox;
        private Panel ChartPanel;
        private PictureBox ChartPictureBox;
        private Panel DashboardPanel;
        private Panel AccountsPanel;
        private PictureBox UserGuidePictureBox;
        private Label UserGuideLabel;
        private PictureBox UserGuideIconPictureBox;
        private Label MakeTransactionLabel;
        private PictureBox MakeTransactionIconPictureBox;
        private PictureBox MakeTransactionPictureBox;
        private Label CreateAccountLabel;
        private PictureBox CreateAccountIconPictureBox;
        private PictureBox CreateAccountPictureBox;
        private Label LogsLabel;
        private PictureBox LogsIconPictureBox;
        private PictureBox LogsPictureBox;
        private PictureBox AccountsPictureBox;
        private TextBox PasswordTextBox;
        private PictureBox PasswordPictureBox;
        private Label PasswordLabel;
        private PictureBox AccountsListPictureBox;
        private PictureBox PasswordArrowPictureBox;
        private ListBox AccountsListBox;
    }
}
