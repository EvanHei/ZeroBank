using ClientLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUI.Forms
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
                await ClientConfig.Login(UsernameTextBox.Text, PasswordTextBox.Text);

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

        private void SignUpPictureBox_Click(object sender, EventArgs e)
        {
            ShowSignUp();
        }

        private void BackArrowPictureBox_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        private async void EnrollPictureBox_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            try
            {
                await ClientConfig.SignUp(UsernameTextBox.Text, PasswordTextBox.Text);

                this.Hide();
                DashboardForm dashboardForm = new();
                dashboardForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Sign up failed";
                ErrorLabel.Visible = true;
            }
        }

        private void ShowSignUp()
        {
            LoginPictureBox.Visible = false;
            SignUpPictureBox.Visible = false;
            EnrollPictureBox.Visible = true;
            BackArrowPictureBox.Visible = true;
            ErrorLabel.Visible = false;

            HeaderLabel.Text = "Sign Up";
            CenterLabelHorizontally(HeaderLabel);
        }

        private void ShowLogin()
        {
            LoginPictureBox.Visible = true;
            SignUpPictureBox.Visible = true;
            EnrollPictureBox.Visible = false;
            BackArrowPictureBox.Visible = false;
            ErrorLabel.Visible = false;

            HeaderLabel.Text = "ZeroBank";
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

        private void SignUpPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Sign Up";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (LoginPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (LoginPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private void EnrollPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Enroll";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (EnrollPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (EnrollPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            LoginPictureBox_Click(null, null);
        }
    }
}
