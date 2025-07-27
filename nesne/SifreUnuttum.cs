using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace nesne
{
    public partial class SifreUnuttum : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True;";
        private string dogrulamaKodu = "";  
        private string girilenAd = "";
        private string girilenSoyad = "";
        private string girilenMail = "";

        public SifreUnuttum()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            txtKod.Visible = false;
            txtSifre.Visible = false;
            txtSifreTekrar.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtAd.Visible && txtSoyad.Visible && txtMail.Visible && !txtKod.Visible && !txtSifre.Visible)
            {
                girilenAd = txtAd.Text.Trim();
                girilenSoyad = txtSoyad.Text.Trim();
                girilenMail = txtMail.Text.Trim();

                if (girilenAd.Length < 1 || girilenSoyad.Length < 1 || girilenMail.Length < 5)
                {
                    MessageBox.Show("Lütfen geçerli bilgileri giriniz.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sorgu = "SELECT COUNT(*) FROM dbo.kullanici WHERE Isim = @ad AND Soyisim = @soyad AND Mail = @mail";
                    using (SqlCommand cmd = new SqlCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", girilenAd);
                        cmd.Parameters.AddWithValue("@soyad", girilenSoyad);
                        cmd.Parameters.AddWithValue("@mail", girilenMail);

                        int sayi = (int)cmd.ExecuteScalar();
                        if (sayi == 0)
                        {
                            MessageBox.Show("Bu bilgilere sahip bir kullanıcı bulunamadı!");
                            return;
                        }
                    }
                }
                dogrulamaKodu = KodUret(6); 

                try
                {
                    MailMessage mesaj = new MailMessage();
                    mesaj.From = new MailAddress("sumeyyebdemir@gmail.com");
                    mesaj.To.Add(girilenMail);
                    mesaj.Subject = "Şifremi Unuttum Kodunuz";
                    mesaj.Body = "Kodunuz: " + dogrulamaKodu;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("sumeyyebdemir@gmail.com", "htww dysw almw jomr");
                    smtp.EnableSsl = true;
                    smtp.Send(mesaj);

                    MessageBox.Show("Kod mail adresinize gönderildi.");

                    txtAd.Visible = false;
                    txtSoyad.Visible = false;
                    txtMail.Visible = false;

                    txtKod.Visible = true;
                    txtKod.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mail gönderilemedi: " + ex.Message);
                }
            }
            else if (txtKod.Visible && !txtSifre.Visible)
            {
                if (txtKod.Text.Trim() == dogrulamaKodu)
                {
                    MessageBox.Show("Kodunuz doğru!");

                    txtKod.Visible = false;

                    txtSifre.Visible = true;
                    txtSifreTekrar.Visible = true;
                }
                else
                {
                    MessageBox.Show("Kod hatalı!");
                }
            }
            else if (txtSifre.Visible && txtSifreTekrar.Visible)
            {
                string yeniSifre = txtSifre.Text.Trim();
                string yeniSifreTekrar = txtSifreTekrar.Text.Trim();

                if (yeniSifre.Length < 8 ||
                    !Regex.IsMatch(yeniSifre, @"[A-Z]") ||
                    !Regex.IsMatch(yeniSifre, @"[a-z]") ||
                    !Regex.IsMatch(yeniSifre, @"[\W_]"))
                {
                    MessageBox.Show("Şifre en az 8 karakter olmalı ve büyük, küçük harf ile özel karakter içermelidir!");
                    return;
                }
                if (yeniSifre != yeniSifreTekrar)
                {
                    MessageBox.Show("Şifreler eşleşmiyor!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string updateQuery = "UPDATE dbo.kullanici SET Sifre = @sifre WHERE Isim = @ad AND Soyisim = @soyad AND Mail = @mail";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@sifre", yeniSifre);
                            cmd.Parameters.AddWithValue("@ad", girilenAd);
                            cmd.Parameters.AddWithValue("@soyad", girilenSoyad);
                            cmd.Parameters.AddWithValue("@mail", girilenMail);

                            cmd.ExecuteNonQuery();
                            // Yeni şifre sonrası mail gönderme
                            try
                            {
                                MailMessage basariMesaj = new MailMessage();
                                basariMesaj.From = new MailAddress("sumeyyebdemir@gmail.com");
                                basariMesaj.To.Add(girilenMail);
                                basariMesaj.Subject = "Şifreniz Başarıyla Değiştirildi!";
                                basariMesaj.Body = "Şifreniz başarılı bir şekilde değiştirilmiştir. Sisteme yeni şifreniz ile giriş yapabilirsiniz.";

                                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                                smtp.Credentials = new System.Net.NetworkCredential("sumeyyebdemir@gmail.com", "ficg pdjh bxzs frdo");
                                smtp.EnableSsl = true;
                                smtp.Send(basariMesaj);

                                MessageBox.Show("Şifre değişiklik maili gönderildi!");
                            }
                            catch (Exception exMail)
                            {
                                MessageBox.Show("Şifre değişiklik maili gönderilemedi: " + exMail.Message);
                            }

                            üye üye = new üye();
                            üye.Show();
                            this.Hide();
                        }
                    }
                }
                catch (Exception ex)
                { MessageBox.Show("Kayıt sırasında hata: " + ex.Message); }

                MessageBox.Show("Güncelleme başarılı! Yeni şifreniz kaydedildi.");

            }
        }
        private string KodUret(int uzunluk)
        {
            Random rnd = new Random();
            int sayi = rnd.Next(100000, 999999);
            return sayi.ToString();
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
    }
}
