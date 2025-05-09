using AdminLibrary;
using ServerLibrary;
using SharedLibrary.Models;

namespace AdminUI.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private bool ValidateFields()
        {
            bool output = true;

            if (string.IsNullOrEmpty(UsernameTextBox.Text) ||
                string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                output = false;
            }

            return output;
        }

        private async void LoginPictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                Credentials adminCredentials = new(UsernameTextBox.Text, PasswordTextBox.Text);
                await AdminConfig.AdminLogin(adminCredentials);

                this.Hide();
                DashboardForm dashboardForm = new();
                dashboardForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Log in failed";
                ErrorLabel.Visible = true;
            }
        }

        private void BackArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        private void ShowLogin()
        {
            LoginPictureBox.Visible = true;
            BackArrowPictureBox.Visible = false;
            ErrorLabel.Visible = false;

            HeaderLabel.Text = "ZeroBank Admin";
            CenterLabelHorizontally(HeaderLabel);
        }

        private void CenterLabelHorizontally(Label label)
        {
            label.Left = (this.ClientSize.Width - label.Width) / 2;
        }

        private void LoginPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Log In";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (LoginPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (LoginPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }
    }
}
