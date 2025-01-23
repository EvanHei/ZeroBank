namespace AdminUI.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void AddPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void AddAdminPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void BackArrowPictureBox_Click(object sender, EventArgs e)
        {

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

        private void AddPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Add";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (LoginPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (LoginPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }

        private void AddAdminPictureBox_Paint(object sender, PaintEventArgs e)
        {
            string text = "Add Admin";

            using Font font = new("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Point);
            using Brush brush = new SolidBrush(Color.White);
            SizeF textSize = e.Graphics.MeasureString(text, font);

            float x = (AddAdminPictureBox.ClientSize.Width - textSize.Width) / 2;
            float y = (AddAdminPictureBox.ClientSize.Height - textSize.Height) / 2;

            e.Graphics.DrawString(text, font, brush, new PointF(x, y));
        }
    }
}
