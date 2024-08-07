namespace WinFormsUI
{
    partial class Dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            SidebarPanel = new Panel();
            AccountsLabel = new Label();
            DashboardLabel = new Label();
            SidebarBubble = new PictureBox();
            AccountsPanel = new Panel();
            label3 = new Label();
            AccountDetailsPanel = new Panel();
            ChartPanel = new Panel();
            SwitchViewPictureBox = new PictureBox();
            TransactionsNumberPictureBox = new PictureBox();
            TransactionsListPictureBox = new PictureBox();
            BalancePictureBox = new PictureBox();
            ChartPictureBox = new PictureBox();
            DashboardPanel = new Panel();
            NumberOfTransactionsLabel = new Label();
            NumberOfAccountsLabel = new Label();
            TotalBalanceLabel = new Label();
            SidebarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SidebarBubble).BeginInit();
            AccountsPanel.SuspendLayout();
            AccountDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SwitchViewPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsNumberPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsListPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BalancePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ChartPictureBox).BeginInit();
            DashboardPanel.SuspendLayout();
            SuspendLayout();
            // 
            // SidebarPanel
            // 
            SidebarPanel.BorderStyle = BorderStyle.FixedSingle;
            SidebarPanel.Controls.Add(AccountsLabel);
            SidebarPanel.Controls.Add(DashboardLabel);
            SidebarPanel.Controls.Add(SidebarBubble);
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
            DashboardLabel.BackColor = Color.FromArgb(79, 79, 79);
            DashboardLabel.ForeColor = Color.FromArgb(172, 172, 172);
            DashboardLabel.Location = new Point(30, 130);
            DashboardLabel.Name = "DashboardLabel";
            DashboardLabel.Size = new Size(86, 21);
            DashboardLabel.TabIndex = 1;
            DashboardLabel.Text = "Dashboard";
            DashboardLabel.Click += DashboardLabel_Click;
            // 
            // SidebarBubble
            // 
            SidebarBubble.Image = (Image)resources.GetObject("SidebarBubble.Image");
            SidebarBubble.Location = new Point(14, 124);
            SidebarBubble.Name = "SidebarBubble";
            SidebarBubble.Size = new Size(177, 35);
            SidebarBubble.SizeMode = PictureBoxSizeMode.StretchImage;
            SidebarBubble.TabIndex = 1;
            SidebarBubble.TabStop = false;
            // 
            // AccountsPanel
            // 
            AccountsPanel.Controls.Add(label3);
            AccountsPanel.Location = new Point(195, -6);
            AccountsPanel.Name = "AccountsPanel";
            AccountsPanel.Size = new Size(987, 530);
            AccountsPanel.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(367, 300);
            label3.Name = "label3";
            label3.Size = new Size(266, 45);
            label3.TabIndex = 0;
            label3.Text = "<accountsPanel>";
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
            DashboardPanel.Controls.Add(NumberOfTransactionsLabel);
            DashboardPanel.Controls.Add(NumberOfAccountsLabel);
            DashboardPanel.Controls.Add(TotalBalanceLabel);
            DashboardPanel.Location = new Point(195, -6);
            DashboardPanel.Name = "DashboardPanel";
            DashboardPanel.Size = new Size(987, 530);
            DashboardPanel.TabIndex = 9;
            // 
            // NumberOfTransactionsLabel
            // 
            NumberOfTransactionsLabel.AutoSize = true;
            NumberOfTransactionsLabel.ForeColor = Color.FromArgb(172, 172, 172);
            NumberOfTransactionsLabel.Location = new Point(436, 416);
            NumberOfTransactionsLabel.Name = "NumberOfTransactionsLabel";
            NumberOfTransactionsLabel.Size = new Size(109, 21);
            NumberOfTransactionsLabel.TabIndex = 4;
            NumberOfTransactionsLabel.Text = "# Transactions";
            // 
            // NumberOfAccountsLabel
            // 
            NumberOfAccountsLabel.AutoSize = true;
            NumberOfAccountsLabel.ForeColor = Color.FromArgb(172, 172, 172);
            NumberOfAccountsLabel.Location = new Point(447, 386);
            NumberOfAccountsLabel.Name = "NumberOfAccountsLabel";
            NumberOfAccountsLabel.Size = new Size(86, 21);
            NumberOfAccountsLabel.TabIndex = 3;
            NumberOfAccountsLabel.Text = "# Accounts";
            // 
            // TotalBalanceLabel
            // 
            TotalBalanceLabel.AutoSize = true;
            TotalBalanceLabel.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TotalBalanceLabel.ForeColor = Color.White;
            TotalBalanceLabel.Location = new Point(367, 300);
            TotalBalanceLabel.Name = "TotalBalanceLabel";
            TotalBalanceLabel.Size = new Size(247, 45);
            TotalBalanceLabel.TabIndex = 0;
            TotalBalanceLabel.Text = "<total balance>";
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(1176, 520);
            Controls.Add(DashboardPanel);
            Controls.Add(AccountDetailsPanel);
            Controls.Add(SidebarPanel);
            Controls.Add(AccountsPanel);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            Name = "Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ZeroBank";
            SidebarPanel.ResumeLayout(false);
            SidebarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SidebarBubble).EndInit();
            AccountsPanel.ResumeLayout(false);
            AccountsPanel.PerformLayout();
            AccountDetailsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SwitchViewPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsNumberPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)TransactionsListPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BalancePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ChartPictureBox).EndInit();
            DashboardPanel.ResumeLayout(false);
            DashboardPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel SidebarPanel;
        private Label AccountsLabel;
        private Label DashboardLabel;
        private PictureBox SidebarBubble;
        private Panel AccountDetailsPanel;
        private PictureBox SwitchViewPictureBox;
        private PictureBox TransactionsNumberPictureBox;
        private PictureBox TransactionsListPictureBox;
        private PictureBox BalancePictureBox;
        private Panel ChartPanel;
        private PictureBox ChartPictureBox;
        private Panel DashboardPanel;
        private Label TotalBalanceLabel;
        private Label NumberOfAccountsLabel;
        private Label NumberOfTransactionsLabel;
        private Panel AccountsPanel;
        private Label label3;
    }
}
