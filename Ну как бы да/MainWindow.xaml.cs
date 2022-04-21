using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Ну_как_бы_да
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\AccInfo");
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            txbLogin.FontSize = Math.Round((this.Height / 28.125));
            txbLog.FontSize = Math.Round((this.Height / 22.5));
            txbBfB.FontSize = Math.Round((this.Height / 9.184) );
            txbPassword.FontSize = Math.Round((this.Height / 28.125));
            txtLogin.FontSize = Math.Round((this.Height / 32.143));
            pswPassword.FontSize = Math.Round((this.Height / 32.143));
            btnSignIn.FontSize = Math.Round((this.Height / 37.5));
            btnSignUp.FontSize = Math.Round((this.Height / 37.5));
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox) == txtName && (sender as TextBox).Text == "Put your name here: ex. Adam")
                (sender as TextBox).Text = "";
            if ((sender as TextBox) == txtCreateLogin && (sender as TextBox).Text == "Create login here: ex. Adam228")
                (sender as TextBox).Text = "";
            if ((sender as TextBox) == txtCreatePassword && (sender as TextBox).Text == "Create password here: ex. Adam1337")
                (sender as TextBox).Text = "";
            if ((sender as TextBox) == txtEmail && (sender as TextBox).Text == "Put your email here: Adam@mail.ru")
                (sender as TextBox).Text = "";
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox) == txtName && txtName.Text.Length == 0) 
                txtName.Text = "Put your name here: ex. Adam";
            if ((sender as TextBox) == txtCreateLogin && txtCreateLogin.Text.Length == 0) 
                txtCreateLogin.Text = "Create login here: ex. Adam228";
            if ((sender as TextBox) == txtCreatePassword && txtCreatePassword.Text.Length == 0) 
                txtCreatePassword.Text = "Create password here: ex. Adam1337";
            if ((sender as TextBox) == txtEmail && txtEmail.Text.Length == 0)
                txtEmail.Text = "Put your email here: Adam@mail.ru";
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {            
            Anymation.Animation(txtName);
            Anymation.Animation(txtCreateLogin);
            Anymation.Animation(txtCreatePassword);
            Anymation.Animation(txtEmail);
            Anymation.OpacityAnimationForButton(btnCreateAccount);
        }

        public int rnd;
        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $"\\AccInfo\\{txtCreateLogin.Text}"))
            {
                try
                {
                    Random random = new Random();
                    rnd = random.Next(1000, 9999);
                    MailAddress from = new MailAddress("petrash123321@mail.ru", "Bestie");
                    MailAddress to = new MailAddress(txtEmail.Text);
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Confirm your Bestie account";
                    message.Body = Convert.ToString(rnd);
                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                    smtp.Credentials = new NetworkCredential("petrash123321@mail.ru", "Qwerty20!5");
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                    Color color = new Color();
                    color.R = 67; color.G = 160; color.B = 71;
                    txbHelper.Foreground = Brushes.LightGreen;
                    txbHelper.Text = "We send a cod on your email write it down below";
                    txbHelper.Visibility = Visibility.Visible;
                    txtVerCode.Visibility = Visibility.Visible;
                }
                catch
                {
                    Color color = new Color();
                    color.R = 229; color.G = 157; color.B = 53;
                    txbHelper.Foreground = Brushes.OrangeRed;
                    txbHelper.Text = "Mail address doesn't exist";
                    txbHelper.Visibility = Visibility.Visible;

                }
            }
            else
            {
                txbHelper.Foreground = Brushes.Yellow;
                txbHelper.Text = "This login is already taken. Choose another one";
                txbHelper.Visibility = Visibility.Visible;
            }
        }

        private void txtVerCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtVerCode.Text == Convert.ToString(rnd))
            {
                string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{txtCreateLogin.Text}";
                Directory.CreateDirectory(path);
                using (StreamWriter writer = new StreamWriter(path +  $"\\{txtCreateLogin.Text}.txt", false))
                {
                    byte[] hashcode;
                    using(SHA256 sha256 = SHA256.Create())
                    {
                        hashcode = sha256.ComputeHash(Encoding.UTF8.GetBytes(txtCreatePassword.Text));
                        writer.WriteLine(Encoding.UTF8.GetString(hashcode));
                    }
                    writer.WriteLine(txtName.Text);
                    writer.WriteLine(txtEmail.Text);
                    writer.WriteLine(0);
                    writer.WriteLine("none");
                }
                accountData = txtCreateLogin.Text;
                Player player = new Player();
                this.Close();
                player.Show();
            }
        }

        public static string accountData;

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{txtLogin.Text}\\{txtLogin.Text}.txt";
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    byte[] hashcode0;
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        hashcode0 = sha256.ComputeHash(Encoding.UTF8.GetBytes(pswPassword.Password));
                        if (Encoding.UTF8.GetString(hashcode0) == reader.ReadLine())
                        {
                            accountData = txtLogin.Text;
                            Player player = new Player();
                            this.Close();
                            player.Show();
                        }
                        else
                        {
                            txbIncorectData.Text = "Incorext password";
                            txbIncorectData.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch
            {
                txbIncorectData.Text = "Incorext login";
                txbIncorectData.Visibility = Visibility.Visible;
            }
        }

        private void pswPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs ev = new RoutedEventArgs();
                btnSignIn_Click(btnSignIn, ev);
            }
        }
    }
}
