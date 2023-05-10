using Google.Authenticator;
using OtpNet;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Authenticator2FA
{
    public partial class Form1 : Form
    {
        byte[] key = new byte[10];
        public Form1()
        {
            InitializeComponent();

        }

        public void AutAsync()
        {
            // Create a new instance of the GoogleAuthenticator library
            var authenticator = new TwoFactorAuthenticator();

            // Generate a secret key for the user
            var secret = GenerateSecretKey();

            // Get the QR code URL for the user to scan
            var setupInfo = authenticator.GenerateSetupCode("Farmacia Indiana", "Gerência", secret, true);

            // Decode the QR code URL and create an image from the bytes
            string qrCodeUrl = setupInfo.QrCodeSetupImageUrl;
            byte[] imageBytes = Convert.FromBase64String(qrCodeUrl.Replace("data:image/png;base64,", ""));
            using (var ms = new MemoryStream(imageBytes))
            {
                pictureBox.Image = Image.FromStream(ms);
            }

            // Add the PictureBox control to the form
            this.Controls.Add(pictureBox);
        }



        public string GenerateSecretKey()
        {
            byte[] keyS = new byte[10];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyS);
            }

            key = keyS;
            return Google.Authenticator.Base32Encoding.ToString(keyS);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AutAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a new instance of the GoogleAuthenticator library
            var authenticator = new TwoFactorAuthenticator();

            // Get the secret key and the code entered by the user
            string code = txtCode.Text;

            var totp = new Totp(key);
            long timeStep = 0;
            bool isCodeValid = totp.VerifyTotp(code, out timeStep, null);
            if (isCodeValid)
            {
                MessageBox.Show("Code é valido!");
            }
            else
            {
                MessageBox.Show("Code é invalido!");
            }

        }
    }
}