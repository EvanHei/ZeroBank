﻿namespace AdminUI.Forms
{
    partial class DashboardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            AccountsPanel = new Panel();
            AccountsPanelClosedRadioButton = new RadioButton();
            AccountsPanelOpenRadioButton = new RadioButton();
            AccountsPanelAllRadioButton = new RadioButton();
            AccountsPanelErrorLabel = new Label();
            AccountsPanelViewDetailsPictureBox = new PictureBox();
            AccountsPanelListBox = new ListBox();
            AccountsPanelListPictureBox = new PictureBox();
            AccountsPanelAccountsPictureBox = new PictureBox();
            SidebarListBox = new ListBox();
            AccountDetailsPanel = new Panel();
            AccountDetailsPanelClosePictureBox = new PictureBox();
            AccountDetailsPanelTransactionsListBox = new ListBox();
            AccountDetailsPanelTranasctionsListPictureBox = new PictureBox();
            AccountDetailsPanelMaxTransactionSizePictureBox = new PictureBox();
            AccountDetailsPanelBackArrowPictureBox = new PictureBox();
            AccountDetailsPanelRangePictureBox = new PictureBox();
            AccountDetailsPanelTransactionsPictureBox = new PictureBox();
            AccountDetailsPanelErrorLabel = new Label();
            AccountDetailsPanelStatusPictureBox = new PictureBox();
            AccountDetailsPanelDateCreatedPictureBox = new PictureBox();
            AccountDetailsPanelNamePictureBox = new PictureBox();
            CreateAdminPanel = new Panel();
            CreateAdminPanelErrorLabel = new Label();
            CreateAdminPanelUsernameTextBox = new TextBox();
            CreateAdminPanelUsernamePictureBox = new PictureBox();
            CreateAdminPanelUsernameLabel = new Label();
            CreateAdminPanelPasswordTextBox = new TextBox();
            CreateAdminPanelPasswordPictureBox = new PictureBox();
            CreateAdminPanelPasswordLabel = new Label();
            CreateAdminPanelCreatePictureBox = new PictureBox();
            CreateAdminPanelHeaderLabel = new Label();
            SidebarRefreshLabel = new Label();
            RootPanel = new Panel();
            AccountsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelViewDetailsPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelListPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelAccountsPictureBox).BeginInit();
            AccountDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelClosePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelTranasctionsListPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelMaxTransactionSizePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelBackArrowPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelRangePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelTransactionsPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelStatusPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelDateCreatedPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelNamePictureBox).BeginInit();
            CreateAdminPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelUsernamePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelPasswordPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelCreatePictureBox).BeginInit();
            RootPanel.SuspendLayout();
            SuspendLayout();
            // 
            // AccountsPanel
            // 
            AccountsPanel.Controls.Add(AccountsPanelClosedRadioButton);
            AccountsPanel.Controls.Add(AccountsPanelOpenRadioButton);
            AccountsPanel.Controls.Add(AccountsPanelAllRadioButton);
            AccountsPanel.Controls.Add(AccountsPanelErrorLabel);
            AccountsPanel.Controls.Add(AccountsPanelViewDetailsPictureBox);
            AccountsPanel.Controls.Add(AccountsPanelListBox);
            AccountsPanel.Controls.Add(AccountsPanelListPictureBox);
            AccountsPanel.Controls.Add(AccountsPanelAccountsPictureBox);
            AccountsPanel.Location = new Point(195, -6);
            AccountsPanel.Name = "AccountsPanel";
            AccountsPanel.Size = new Size(987, 732);
            AccountsPanel.TabIndex = 11;
            // 
            // AccountsPanelClosedRadioButton
            // 
            AccountsPanelClosedRadioButton.AutoSize = true;
            AccountsPanelClosedRadioButton.ForeColor = Color.Silver;
            AccountsPanelClosedRadioButton.Location = new Point(720, 199);
            AccountsPanelClosedRadioButton.Name = "AccountsPanelClosedRadioButton";
            AccountsPanelClosedRadioButton.Size = new Size(75, 25);
            AccountsPanelClosedRadioButton.TabIndex = 38;
            AccountsPanelClosedRadioButton.Text = "Closed";
            AccountsPanelClosedRadioButton.UseVisualStyleBackColor = true;
            AccountsPanelClosedRadioButton.CheckedChanged += AccountsPanelRadioButton_CheckedChanged;
            // 
            // AccountsPanelOpenRadioButton
            // 
            AccountsPanelOpenRadioButton.AutoSize = true;
            AccountsPanelOpenRadioButton.ForeColor = Color.Silver;
            AccountsPanelOpenRadioButton.Location = new Point(720, 173);
            AccountsPanelOpenRadioButton.Name = "AccountsPanelOpenRadioButton";
            AccountsPanelOpenRadioButton.Size = new Size(66, 25);
            AccountsPanelOpenRadioButton.TabIndex = 37;
            AccountsPanelOpenRadioButton.Text = "Open";
            AccountsPanelOpenRadioButton.UseVisualStyleBackColor = true;
            AccountsPanelOpenRadioButton.CheckedChanged += AccountsPanelRadioButton_CheckedChanged;
            // 
            // AccountsPanelAllRadioButton
            // 
            AccountsPanelAllRadioButton.AutoSize = true;
            AccountsPanelAllRadioButton.Checked = true;
            AccountsPanelAllRadioButton.ForeColor = Color.Silver;
            AccountsPanelAllRadioButton.Location = new Point(720, 147);
            AccountsPanelAllRadioButton.Name = "AccountsPanelAllRadioButton";
            AccountsPanelAllRadioButton.Size = new Size(46, 25);
            AccountsPanelAllRadioButton.TabIndex = 36;
            AccountsPanelAllRadioButton.TabStop = true;
            AccountsPanelAllRadioButton.Text = "All";
            AccountsPanelAllRadioButton.UseVisualStyleBackColor = true;
            AccountsPanelAllRadioButton.CheckedChanged += AccountsPanelRadioButton_CheckedChanged;
            // 
            // AccountsPanelErrorLabel
            // 
            AccountsPanelErrorLabel.Dock = DockStyle.Bottom;
            AccountsPanelErrorLabel.Font = new Font("Segoe UI Emoji", 12F);
            AccountsPanelErrorLabel.ForeColor = Color.Red;
            AccountsPanelErrorLabel.Location = new Point(0, 706);
            AccountsPanelErrorLabel.Name = "AccountsPanelErrorLabel";
            AccountsPanelErrorLabel.Padding = new Padding(0, 0, 3, 0);
            AccountsPanelErrorLabel.Size = new Size(987, 26);
            AccountsPanelErrorLabel.TabIndex = 35;
            AccountsPanelErrorLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AccountsPanelViewDetailsPictureBox
            // 
            AccountsPanelViewDetailsPictureBox.Image = (Image)resources.GetObject("AccountsPanelViewDetailsPictureBox.Image");
            AccountsPanelViewDetailsPictureBox.Location = new Point(359, 666);
            AccountsPanelViewDetailsPictureBox.Name = "AccountsPanelViewDetailsPictureBox";
            AccountsPanelViewDetailsPictureBox.Size = new Size(123, 36);
            AccountsPanelViewDetailsPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountsPanelViewDetailsPictureBox.TabIndex = 18;
            AccountsPanelViewDetailsPictureBox.TabStop = false;
            AccountsPanelViewDetailsPictureBox.Click += AccountsPanelViewDetailsPictureBox_Click;
            AccountsPanelViewDetailsPictureBox.Paint += AccountsPanelViewDetailsPictureBox_Paint;
            // 
            // AccountsPanelListBox
            // 
            AccountsPanelListBox.BackColor = Color.FromArgb(79, 79, 79);
            AccountsPanelListBox.BorderStyle = BorderStyle.None;
            AccountsPanelListBox.DrawMode = DrawMode.OwnerDrawVariable;
            AccountsPanelListBox.ForeColor = Color.Silver;
            AccountsPanelListBox.FormattingEnabled = true;
            AccountsPanelListBox.ItemHeight = 21;
            AccountsPanelListBox.Location = new Point(160, 158);
            AccountsPanelListBox.Name = "AccountsPanelListBox";
            AccountsPanelListBox.Size = new Size(520, 483);
            AccountsPanelListBox.TabIndex = 14;
            AccountsPanelListBox.DrawItem += AccountsPanelListBox_DrawItem;
            AccountsPanelListBox.MeasureItem += AccountsPanelListBox_MeasureItem;
            // 
            // AccountsPanelListPictureBox
            // 
            AccountsPanelListPictureBox.Image = (Image)resources.GetObject("AccountsPanelListPictureBox.Image");
            AccountsPanelListPictureBox.Location = new Point(147, 147);
            AccountsPanelListPictureBox.Name = "AccountsPanelListPictureBox";
            AccountsPanelListPictureBox.Size = new Size(551, 507);
            AccountsPanelListPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            AccountsPanelListPictureBox.TabIndex = 12;
            AccountsPanelListPictureBox.TabStop = false;
            // 
            // AccountsPanelAccountsPictureBox
            // 
            AccountsPanelAccountsPictureBox.Image = (Image)resources.GetObject("AccountsPanelAccountsPictureBox.Image");
            AccountsPanelAccountsPictureBox.Location = new Point(147, 42);
            AccountsPanelAccountsPictureBox.Name = "AccountsPanelAccountsPictureBox";
            AccountsPanelAccountsPictureBox.Size = new Size(257, 81);
            AccountsPanelAccountsPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountsPanelAccountsPictureBox.TabIndex = 2;
            AccountsPanelAccountsPictureBox.TabStop = false;
            AccountsPanelAccountsPictureBox.Paint += AccountsPanelAccountsPictureBox_Paint;
            // 
            // SidebarListBox
            // 
            SidebarListBox.BackColor = Color.FromArgb(45, 45, 45);
            SidebarListBox.BorderStyle = BorderStyle.None;
            SidebarListBox.DrawMode = DrawMode.OwnerDrawFixed;
            SidebarListBox.ForeColor = Color.FromArgb(172, 172, 172);
            SidebarListBox.FormattingEnabled = true;
            SidebarListBox.ItemHeight = 35;
            SidebarListBox.Items.AddRange(new object[] { "💳 Accounts", "👥 Add Admin" });
            SidebarListBox.Location = new Point(0, 168);
            SidebarListBox.Name = "SidebarListBox";
            SidebarListBox.Size = new Size(194, 140);
            SidebarListBox.TabIndex = 39;
            SidebarListBox.DrawItem += SidebarListBox_DrawItem;
            SidebarListBox.SelectedIndexChanged += SidebarListBox_SelectedIndexChanged;
            // 
            // AccountDetailsPanel
            // 
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelClosePictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelTransactionsListBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelTranasctionsListPictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelMaxTransactionSizePictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelBackArrowPictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelRangePictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelTransactionsPictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelErrorLabel);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelStatusPictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelDateCreatedPictureBox);
            AccountDetailsPanel.Controls.Add(AccountDetailsPanelNamePictureBox);
            AccountDetailsPanel.ForeColor = Color.Black;
            AccountDetailsPanel.Location = new Point(195, -6);
            AccountDetailsPanel.Name = "AccountDetailsPanel";
            AccountDetailsPanel.Size = new Size(987, 735);
            AccountDetailsPanel.TabIndex = 40;
            // 
            // AccountDetailsPanelClosePictureBox
            // 
            AccountDetailsPanelClosePictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelClosePictureBox.Image");
            AccountDetailsPanelClosePictureBox.Location = new Point(738, 642);
            AccountDetailsPanelClosePictureBox.Name = "AccountDetailsPanelClosePictureBox";
            AccountDetailsPanelClosePictureBox.Size = new Size(223, 61);
            AccountDetailsPanelClosePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelClosePictureBox.TabIndex = 43;
            AccountDetailsPanelClosePictureBox.TabStop = false;
            AccountDetailsPanelClosePictureBox.Click += AccountDetailsPanelClosePictureBox_Click;
            AccountDetailsPanelClosePictureBox.Paint += AccountDetailsPanelClosePictureBox_Paint;
            // 
            // AccountDetailsPanelTransactionsListBox
            // 
            AccountDetailsPanelTransactionsListBox.BackColor = Color.FromArgb(79, 79, 79);
            AccountDetailsPanelTransactionsListBox.BorderStyle = BorderStyle.None;
            AccountDetailsPanelTransactionsListBox.DrawMode = DrawMode.OwnerDrawVariable;
            AccountDetailsPanelTransactionsListBox.ForeColor = Color.Silver;
            AccountDetailsPanelTransactionsListBox.FormattingEnabled = true;
            AccountDetailsPanelTransactionsListBox.ItemHeight = 21;
            AccountDetailsPanelTransactionsListBox.Location = new Point(48, 143);
            AccountDetailsPanelTransactionsListBox.Name = "AccountDetailsPanelTransactionsListBox";
            AccountDetailsPanelTransactionsListBox.Size = new Size(654, 546);
            AccountDetailsPanelTransactionsListBox.TabIndex = 42;
            AccountDetailsPanelTransactionsListBox.DrawItem += AccountDetailsPanelTransactionsListBox_DrawItem;
            AccountDetailsPanelTransactionsListBox.MeasureItem += AccountDetailsPanelTransactionsListBox_MeasureItem;
            // 
            // AccountDetailsPanelTranasctionsListPictureBox
            // 
            AccountDetailsPanelTranasctionsListPictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelTranasctionsListPictureBox.Image");
            AccountDetailsPanelTranasctionsListPictureBox.Location = new Point(35, 101);
            AccountDetailsPanelTranasctionsListPictureBox.Name = "AccountDetailsPanelTranasctionsListPictureBox";
            AccountDetailsPanelTranasctionsListPictureBox.Size = new Size(686, 606);
            AccountDetailsPanelTranasctionsListPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            AccountDetailsPanelTranasctionsListPictureBox.TabIndex = 41;
            AccountDetailsPanelTranasctionsListPictureBox.TabStop = false;
            AccountDetailsPanelTranasctionsListPictureBox.Paint += AccountDetailsPanelTranasctionsListPictureBox_Paint;
            // 
            // AccountDetailsPanelMaxTransactionSizePictureBox
            // 
            AccountDetailsPanelMaxTransactionSizePictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelMaxTransactionSizePictureBox.Image");
            AccountDetailsPanelMaxTransactionSizePictureBox.Location = new Point(738, 103);
            AccountDetailsPanelMaxTransactionSizePictureBox.Name = "AccountDetailsPanelMaxTransactionSizePictureBox";
            AccountDetailsPanelMaxTransactionSizePictureBox.Size = new Size(223, 61);
            AccountDetailsPanelMaxTransactionSizePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelMaxTransactionSizePictureBox.TabIndex = 40;
            AccountDetailsPanelMaxTransactionSizePictureBox.TabStop = false;
            AccountDetailsPanelMaxTransactionSizePictureBox.Paint += AccountDetailsPanelMaxTransactionSizePictureBox_Paint;
            // 
            // AccountDetailsPanelBackArrowPictureBox
            // 
            AccountDetailsPanelBackArrowPictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelBackArrowPictureBox.Image");
            AccountDetailsPanelBackArrowPictureBox.Location = new Point(12, 12);
            AccountDetailsPanelBackArrowPictureBox.Name = "AccountDetailsPanelBackArrowPictureBox";
            AccountDetailsPanelBackArrowPictureBox.Size = new Size(38, 36);
            AccountDetailsPanelBackArrowPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelBackArrowPictureBox.TabIndex = 39;
            AccountDetailsPanelBackArrowPictureBox.TabStop = false;
            AccountDetailsPanelBackArrowPictureBox.Click += AccountDetailsPanelBackArrowPictureBox_Click;
            // 
            // AccountDetailsPanelRangePictureBox
            // 
            AccountDetailsPanelRangePictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelRangePictureBox.Image");
            AccountDetailsPanelRangePictureBox.Location = new Point(738, 274);
            AccountDetailsPanelRangePictureBox.Name = "AccountDetailsPanelRangePictureBox";
            AccountDetailsPanelRangePictureBox.Size = new Size(223, 61);
            AccountDetailsPanelRangePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelRangePictureBox.TabIndex = 38;
            AccountDetailsPanelRangePictureBox.TabStop = false;
            AccountDetailsPanelRangePictureBox.Paint += AccountDetailsPanelRangeOrBalancePictureBox_Paint;
            // 
            // AccountDetailsPanelTransactionsPictureBox
            // 
            AccountDetailsPanelTransactionsPictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelTransactionsPictureBox.Image");
            AccountDetailsPanelTransactionsPictureBox.Location = new Point(738, 190);
            AccountDetailsPanelTransactionsPictureBox.Name = "AccountDetailsPanelTransactionsPictureBox";
            AccountDetailsPanelTransactionsPictureBox.Size = new Size(223, 61);
            AccountDetailsPanelTransactionsPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelTransactionsPictureBox.TabIndex = 37;
            AccountDetailsPanelTransactionsPictureBox.TabStop = false;
            AccountDetailsPanelTransactionsPictureBox.Paint += AccountDetailsPanelTransactionsPictureBox_Paint;
            // 
            // AccountDetailsPanelErrorLabel
            // 
            AccountDetailsPanelErrorLabel.Dock = DockStyle.Bottom;
            AccountDetailsPanelErrorLabel.Font = new Font("Segoe UI Emoji", 12F);
            AccountDetailsPanelErrorLabel.ForeColor = Color.Red;
            AccountDetailsPanelErrorLabel.Location = new Point(0, 709);
            AccountDetailsPanelErrorLabel.Name = "AccountDetailsPanelErrorLabel";
            AccountDetailsPanelErrorLabel.Padding = new Padding(0, 0, 3, 0);
            AccountDetailsPanelErrorLabel.Size = new Size(987, 26);
            AccountDetailsPanelErrorLabel.TabIndex = 34;
            AccountDetailsPanelErrorLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AccountDetailsPanelStatusPictureBox
            // 
            AccountDetailsPanelStatusPictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelStatusPictureBox.Image");
            AccountDetailsPanelStatusPictureBox.Location = new Point(738, 20);
            AccountDetailsPanelStatusPictureBox.Name = "AccountDetailsPanelStatusPictureBox";
            AccountDetailsPanelStatusPictureBox.Size = new Size(223, 61);
            AccountDetailsPanelStatusPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelStatusPictureBox.TabIndex = 10;
            AccountDetailsPanelStatusPictureBox.TabStop = false;
            AccountDetailsPanelStatusPictureBox.Paint += AccountDetailsPanelStatusPictureBox_Paint;
            // 
            // AccountDetailsPanelDateCreatedPictureBox
            // 
            AccountDetailsPanelDateCreatedPictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelDateCreatedPictureBox.Image");
            AccountDetailsPanelDateCreatedPictureBox.Location = new Point(494, 20);
            AccountDetailsPanelDateCreatedPictureBox.Name = "AccountDetailsPanelDateCreatedPictureBox";
            AccountDetailsPanelDateCreatedPictureBox.Size = new Size(223, 61);
            AccountDetailsPanelDateCreatedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelDateCreatedPictureBox.TabIndex = 9;
            AccountDetailsPanelDateCreatedPictureBox.TabStop = false;
            AccountDetailsPanelDateCreatedPictureBox.Paint += AccountDetailsPanelDateCreatedPictureBox_Paint;
            // 
            // AccountDetailsPanelNamePictureBox
            // 
            AccountDetailsPanelNamePictureBox.Image = (Image)resources.GetObject("AccountDetailsPanelNamePictureBox.Image");
            AccountDetailsPanelNamePictureBox.Location = new Point(244, 20);
            AccountDetailsPanelNamePictureBox.Name = "AccountDetailsPanelNamePictureBox";
            AccountDetailsPanelNamePictureBox.Size = new Size(223, 61);
            AccountDetailsPanelNamePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AccountDetailsPanelNamePictureBox.TabIndex = 0;
            AccountDetailsPanelNamePictureBox.TabStop = false;
            AccountDetailsPanelNamePictureBox.Paint += AccountDetailsPanelNamePictureBox_Paint;
            // 
            // CreateAdminPanel
            // 
            CreateAdminPanel.Controls.Add(CreateAdminPanelErrorLabel);
            CreateAdminPanel.Controls.Add(CreateAdminPanelUsernameTextBox);
            CreateAdminPanel.Controls.Add(CreateAdminPanelUsernamePictureBox);
            CreateAdminPanel.Controls.Add(CreateAdminPanelUsernameLabel);
            CreateAdminPanel.Controls.Add(CreateAdminPanelPasswordTextBox);
            CreateAdminPanel.Controls.Add(CreateAdminPanelPasswordPictureBox);
            CreateAdminPanel.Controls.Add(CreateAdminPanelPasswordLabel);
            CreateAdminPanel.Controls.Add(CreateAdminPanelCreatePictureBox);
            CreateAdminPanel.Controls.Add(CreateAdminPanelHeaderLabel);
            CreateAdminPanel.Location = new Point(195, -6);
            CreateAdminPanel.Name = "CreateAdminPanel";
            CreateAdminPanel.Size = new Size(987, 735);
            CreateAdminPanel.TabIndex = 41;
            // 
            // CreateAdminPanelErrorLabel
            // 
            CreateAdminPanelErrorLabel.Dock = DockStyle.Bottom;
            CreateAdminPanelErrorLabel.Font = new Font("Segoe UI Emoji", 12F);
            CreateAdminPanelErrorLabel.ForeColor = Color.Red;
            CreateAdminPanelErrorLabel.Location = new Point(0, 709);
            CreateAdminPanelErrorLabel.Name = "CreateAdminPanelErrorLabel";
            CreateAdminPanelErrorLabel.Padding = new Padding(0, 0, 3, 0);
            CreateAdminPanelErrorLabel.Size = new Size(987, 26);
            CreateAdminPanelErrorLabel.TabIndex = 36;
            CreateAdminPanelErrorLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CreateAdminPanelUsernameTextBox
            // 
            CreateAdminPanelUsernameTextBox.BackColor = Color.FromArgb(45, 45, 45);
            CreateAdminPanelUsernameTextBox.BorderStyle = BorderStyle.None;
            CreateAdminPanelUsernameTextBox.ForeColor = Color.White;
            CreateAdminPanelUsernameTextBox.Location = new Point(356, 240);
            CreateAdminPanelUsernameTextBox.Name = "CreateAdminPanelUsernameTextBox";
            CreateAdminPanelUsernameTextBox.Size = new Size(239, 22);
            CreateAdminPanelUsernameTextBox.TabIndex = 1;
            // 
            // CreateAdminPanelUsernamePictureBox
            // 
            CreateAdminPanelUsernamePictureBox.Image = (Image)resources.GetObject("CreateAdminPanelUsernamePictureBox.Image");
            CreateAdminPanelUsernamePictureBox.Location = new Point(346, 225);
            CreateAdminPanelUsernamePictureBox.Name = "CreateAdminPanelUsernamePictureBox";
            CreateAdminPanelUsernamePictureBox.Size = new Size(257, 55);
            CreateAdminPanelUsernamePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CreateAdminPanelUsernamePictureBox.TabIndex = 27;
            CreateAdminPanelUsernamePictureBox.TabStop = false;
            // 
            // CreateAdminPanelUsernameLabel
            // 
            CreateAdminPanelUsernameLabel.AutoSize = true;
            CreateAdminPanelUsernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CreateAdminPanelUsernameLabel.ForeColor = Color.Silver;
            CreateAdminPanelUsernameLabel.Location = new Point(342, 202);
            CreateAdminPanelUsernameLabel.Name = "CreateAdminPanelUsernameLabel";
            CreateAdminPanelUsernameLabel.Size = new Size(81, 21);
            CreateAdminPanelUsernameLabel.TabIndex = 25;
            CreateAdminPanelUsernameLabel.Text = "Username";
            // 
            // CreateAdminPanelPasswordTextBox
            // 
            CreateAdminPanelPasswordTextBox.BackColor = Color.FromArgb(45, 45, 45);
            CreateAdminPanelPasswordTextBox.BorderStyle = BorderStyle.None;
            CreateAdminPanelPasswordTextBox.ForeColor = Color.White;
            CreateAdminPanelPasswordTextBox.Location = new Point(356, 321);
            CreateAdminPanelPasswordTextBox.Name = "CreateAdminPanelPasswordTextBox";
            CreateAdminPanelPasswordTextBox.Size = new Size(239, 22);
            CreateAdminPanelPasswordTextBox.TabIndex = 2;
            CreateAdminPanelPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // CreateAdminPanelPasswordPictureBox
            // 
            CreateAdminPanelPasswordPictureBox.Image = (Image)resources.GetObject("CreateAdminPanelPasswordPictureBox.Image");
            CreateAdminPanelPasswordPictureBox.Location = new Point(346, 306);
            CreateAdminPanelPasswordPictureBox.Name = "CreateAdminPanelPasswordPictureBox";
            CreateAdminPanelPasswordPictureBox.Size = new Size(257, 55);
            CreateAdminPanelPasswordPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CreateAdminPanelPasswordPictureBox.TabIndex = 20;
            CreateAdminPanelPasswordPictureBox.TabStop = false;
            // 
            // CreateAdminPanelPasswordLabel
            // 
            CreateAdminPanelPasswordLabel.AutoSize = true;
            CreateAdminPanelPasswordLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CreateAdminPanelPasswordLabel.ForeColor = Color.Silver;
            CreateAdminPanelPasswordLabel.Location = new Point(342, 283);
            CreateAdminPanelPasswordLabel.Name = "CreateAdminPanelPasswordLabel";
            CreateAdminPanelPasswordLabel.Size = new Size(76, 21);
            CreateAdminPanelPasswordLabel.TabIndex = 18;
            CreateAdminPanelPasswordLabel.Text = "Password";
            // 
            // CreateAdminPanelCreatePictureBox
            // 
            CreateAdminPanelCreatePictureBox.Image = (Image)resources.GetObject("CreateAdminPanelCreatePictureBox.Image");
            CreateAdminPanelCreatePictureBox.Location = new Point(414, 385);
            CreateAdminPanelCreatePictureBox.Name = "CreateAdminPanelCreatePictureBox";
            CreateAdminPanelCreatePictureBox.Size = new Size(123, 36);
            CreateAdminPanelCreatePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CreateAdminPanelCreatePictureBox.TabIndex = 17;
            CreateAdminPanelCreatePictureBox.TabStop = false;
            CreateAdminPanelCreatePictureBox.Click += CreateAdminPanelCreatePictureBox_Click;
            CreateAdminPanelCreatePictureBox.Paint += CreateAdminPanelCreatePictureBox_Paint;
            // 
            // CreateAdminPanelHeaderLabel
            // 
            CreateAdminPanelHeaderLabel.Anchor = AnchorStyles.None;
            CreateAdminPanelHeaderLabel.AutoSize = true;
            CreateAdminPanelHeaderLabel.Font = new Font("Segoe UI Emoji", 24F);
            CreateAdminPanelHeaderLabel.ForeColor = Color.White;
            CreateAdminPanelHeaderLabel.Location = new Point(368, 91);
            CreateAdminPanelHeaderLabel.Name = "CreateAdminPanelHeaderLabel";
            CreateAdminPanelHeaderLabel.Size = new Size(214, 43);
            CreateAdminPanelHeaderLabel.TabIndex = 1;
            CreateAdminPanelHeaderLabel.Text = "Create Admin";
            // 
            // SidebarRefreshLabel
            // 
            SidebarRefreshLabel.AutoSize = true;
            SidebarRefreshLabel.ForeColor = Color.FromArgb(172, 172, 172);
            SidebarRefreshLabel.Location = new Point(0, 3);
            SidebarRefreshLabel.Name = "SidebarRefreshLabel";
            SidebarRefreshLabel.Size = new Size(32, 21);
            SidebarRefreshLabel.TabIndex = 42;
            SidebarRefreshLabel.Text = "🔄";
            SidebarRefreshLabel.Click += SidebarRefreshLabel_Click;
            // 
            // RootPanel
            // 
            RootPanel.AutoScroll = true;
            RootPanel.Controls.Add(AccountDetailsPanel);
            RootPanel.Controls.Add(CreateAdminPanel);
            RootPanel.Controls.Add(AccountsPanel);
            RootPanel.Controls.Add(SidebarListBox);
            RootPanel.Controls.Add(SidebarRefreshLabel);
            RootPanel.Dock = DockStyle.Fill;
            RootPanel.Location = new Point(0, 0);
            RootPanel.Name = "RootPanel";
            RootPanel.Size = new Size(1182, 742);
            RootPanel.TabIndex = 43;
            RootPanel.Paint += RootPanel_Paint;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 45);
            ClientSize = new Size(1182, 742);
            Controls.Add(RootPanel);
            Font = new Font("Segoe UI Emoji", 12F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ZeroBank Admin";
            AccountsPanel.ResumeLayout(false);
            AccountsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelViewDetailsPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelListPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountsPanelAccountsPictureBox).EndInit();
            AccountDetailsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelClosePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelTranasctionsListPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelMaxTransactionSizePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelBackArrowPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelRangePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelTransactionsPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelStatusPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelDateCreatedPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)AccountDetailsPanelNamePictureBox).EndInit();
            CreateAdminPanel.ResumeLayout(false);
            CreateAdminPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelUsernamePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelPasswordPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)CreateAdminPanelCreatePictureBox).EndInit();
            RootPanel.ResumeLayout(false);
            RootPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel AccountsPanel;
        private RadioButton AccountsPanelClosedRadioButton;
        private RadioButton AccountsPanelOpenRadioButton;
        private RadioButton AccountsPanelAllRadioButton;
        private Label AccountsPanelErrorLabel;
        private PictureBox AccountsPanelViewDetailsPictureBox;
        private ListBox AccountsPanelListBox;
        private PictureBox AccountsPanelListPictureBox;
        private PictureBox AccountsPanelAccountsPictureBox;
        private ListBox SidebarListBox;
        private Panel AccountDetailsPanel;
        private PictureBox AccountDetailsPanelBackArrowPictureBox;
        private PictureBox AccountDetailsPanelRangePictureBox;
        private PictureBox AccountDetailsPanelTransactionsPictureBox;
        private Label AccountDetailsPanelErrorLabel;
        private PictureBox AccountDetailsPanelStatusPictureBox;
        private PictureBox AccountDetailsPanelDateCreatedPictureBox;
        private PictureBox AccountDetailsPanelNamePictureBox;
        private PictureBox AccountDetailsPanelMaxTransactionSizePictureBox;
        private ListBox AccountDetailsPanelTransactionsListBox;
        private PictureBox AccountDetailsPanelTranasctionsListPictureBox;
        private PictureBox AccountDetailsPanelClosePictureBox;
        private Panel CreateAdminPanel;
        private Label CreateAdminPanelErrorLabel;
        private TextBox CreateAdminPanelUsernameTextBox;
        private PictureBox CreateAdminPanelUsernamePictureBox;
        private Label CreateAdminPanelUsernameLabel;
        private TextBox CreateAdminPanelPasswordTextBox;
        private PictureBox CreateAdminPanelPasswordPictureBox;
        private Label CreateAdminPanelPasswordLabel;
        private PictureBox CreateAdminPanelCreatePictureBox;
        private Label CreateAdminPanelHeaderLabel;
        private Label SidebarRefreshLabel;
        private Panel RootPanel;
    }
}