using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace nesne
{
    public partial class yeniuye : Form
    {
        private string dogrulamaKodu = "";
        private string secilenAvatarYolu = "";
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True;";
        private bool kayitTamamlandi = false;
        private bool kodGonderildi = false; 
        public yeniuye()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            txtSifre.Visible = false;
            txtSifreTekrar.Visible = false;
            txtKod.Visible = false;
            label3.Visible = false;
            label2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox3.Image = Properties.Resources.gizle;

        }

        private void btnKodGonder_Click(object sender, EventArgs e)
        {
            if (kayitTamamlandi)
            {
                MessageBox.Show("Kayıt zaten tamamlandı.");
                return;
            }
            if (!kodGonderildi && txtKod.Visible == false)
            {
                string ad = txtAd.Text.Trim();
                string soyad = txtSoyad.Text.Trim();
                string mail = txtMail.Text.Trim();

                if (ad.Length < 3 || soyad.Length < 2)
                {
                    MessageBox.Show("Lütfen geçerli bir ad ve soyad girin.");
                    return;
                }
                dogrulamaKodu = ad.Substring(0, 3).ToLower() + soyad.Substring(soyad.Length - 2).ToLower() + "2025";

                try
                {
                    MailMessage mesaj = new MailMessage();
                    mesaj.From = new MailAddress("sumeyyebdemir@gmail.com");
                    mesaj.To.Add(mail);
                    mesaj.Subject = "Doğrulama Kodunuz";
                    mesaj.Body = "Doğrulama Kodunuz: " + dogrulamaKodu;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("sumeyyebdemir@gmail.com", "htww dysw almw jomr");
                    smtp.EnableSsl = true;
                    smtp.Send(mesaj);

                    txtKod.Visible = true;
                    kodGonderildi = true; 
                    MessageBox.Show("Doğrulama kodu mail adresinize gönderildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mail gönderilemedi: " + ex.Message);
                }
            }
            else if (txtSifre.Visible == false)
            {
                if (txtKod.Text == dogrulamaKodu)
                {
                    txtKod.Visible = false;
                    txtSifre.Visible = true;
                    txtSifreTekrar.Visible = true;
                    label3.Visible = true;
                    label2.Visible = true;
                    pictureBox3.Visible = true;
                    MessageBox.Show("Kod doğrulandı! Şifre oluşturabilirsiniz.");
                }
                else
                {
                    MessageBox.Show("Kod yanlış!");
                }
            }
            else
            {
                string sifre = txtSifre.Text;
                string tekrar = txtSifreTekrar.Text;

                if (sifre != tekrar)
                {
                    MessageBox.Show("Şifreler eşleşmiyor.");
                    return;
                }

                if (sifre.Length < 8 || !Regex.IsMatch(sifre, @"[A-Z]") ||
                    !Regex.IsMatch(sifre, @"[a-z]") || !Regex.IsMatch(sifre, @"[\W_]"))
                {
                    MessageBox.Show("Şifre en az 8 karakter olmalı ve büyük, küçük harf ile özel karakter içermelidir.");
                    return;
                }

                try
                {
                    using (SqlConnection baglanti = new SqlConnection(connectionString))
                    {
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand("INSERT INTO dbo.kullanici (Isim, Soyisim, Mail, Sifre, Avatar) VALUES (@ad, @soyad, @mail, @sifre, @avatar)", baglanti);
                        komut.Parameters.AddWithValue("@ad", txtAd.Text.Trim());
                        komut.Parameters.AddWithValue("@soyad", txtSoyad.Text.Trim());
                        komut.Parameters.AddWithValue("@mail", txtMail.Text.Trim());
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        komut.Parameters.AddWithValue("@avatar", secilenAvatarYolu);

                        komut.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarıyla tamamlandı!");

                        kayitTamamlandi = true;

                        üye uyeForm = new üye();
                        uyeForm.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt sırasında hata: " + ex.Message);
                }
            }
        }

        private void btnAvatarSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                secilenAvatarYolu = ofd.FileName;
                pictureBoxAvatar.Image = Image.FromFile(secilenAvatarYolu);
                pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            üye üye = new üye();
            üye.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
             if (txtSifre.UseSystemPasswordChar)
    {
        txtSifre.UseSystemPasswordChar = false;
        txtSifreTekrar.UseSystemPasswordChar = false;
        pictureBox3.Image = Properties.Resources.gizle;
    }
    else
    {
        txtSifre.UseSystemPasswordChar = true;
                txtSifreTekrar.UseSystemPasswordChar = true;
                pictureBox3.Image = Properties.Resources.goster;
    }

        }
    }
}
