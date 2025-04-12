using System;   
using System.Data;      
using System.Data.SqlClient;        
using System.Drawing;   
using System.IO;
using System.Media;
using System.Net;                  
using System.Net.Mail;            
using System.Runtime.InteropServices; 
using System.Text.RegularExpressions;    
using System.Windows.Forms;              
using Microsoft.VisualBasic;

namespace nesne
{

    public partial class üye : Form
    {
        private SoundPlayer player;
        private bool isSoundOn = true;

        [DllImport("winmm.dll")] 
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True;";

        private string mevcutAvatarYolu = "";

        public üye()
        {

            PlaySound();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtSifre.PasswordChar = '*';
            this.hakkımızdaToolStripMenuItem.Click += new EventHandler(hakkımızdaToolStripMenuItem_Click);

            if (Properties.Settings.Default.BeniHatirla)
            {
                txtMail.Text = Properties.Settings.Default.Mail;
                txtSifre.Text = Properties.Settings.Default.Sifre;
                chkBeniHatirla.Checked = Properties.Settings.Default.BeniHatirla;// Uygulamanın kalıcı ayarlarını (uygulama kapansa bile saklanan veriler) temsil eder.

                if (!string.IsNullOrEmpty(Properties.Settings.Default.AvatarPath) &&
                    File.Exists(Properties.Settings.Default.AvatarPath))
                {
                    pictureBoxAvatar.Image = Image.FromFile(Properties.Settings.Default.AvatarPath);
                    pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                txtMail.Text = "";
                txtSifre.Text = "";
                chkBeniHatirla.Checked = false;
            }
        }


        private void PlaySound()
        {
            try
            {
                string soundPath = @"C:\Users\EXCALİBUR\Desktop\ödevler\nesne\nesne\game.wav";
                player = new SoundPlayer(soundPath);
                player.Play();
                isSoundOn = true;
            }
            catch { }
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            player.Stop();
            isSoundOn = true;

            pictureBox3.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.Play();
            isSoundOn = false;

            pictureBox2.Visible = false;
            pictureBox3.Visible = true;

        }

        private void müzikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Ses seviyesini girin (0-100):", "Müzik seviyesi", "");
            if (int.TryParse(input, out int level))
            {
                if (level >= 0 && level <= 100)
                {
                    SetVolume(level);
                }
            }
        }
        public void SetVolume(int level)
        {
            int newVolume = (ushort.MaxValue / 100) * level;
            uint volumeAllChannels = ((uint)newVolume & 0x0000ffff) | ((uint)newVolume << 16);
            waveOutSetVolume(IntPtr.Zero, volumeAllChannels);
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string mail = txtMail.Text.Trim();
            string sifre = txtSifre.Text.Trim();
            string secilenAvatarYolu = "";

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                baglanti.Open();
                string sorgu = "SELECT TOP 1 Sifre, Avatar FROM dbo.Kullanici WHERE Mail = @mail";
                using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@mail", mail);
                    SqlDataReader reader = komut.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbSifre = reader["Sifre"].ToString();
                        mevcutAvatarYolu = reader["Avatar"].ToString();

                        reader.Close();

                        if (dbSifre != sifre)
                        {
                            MessageBox.Show("Şifre hatalı!");
                            return;
                        }
                        secilenAvatarYolu = pictureBoxAvatar.ImageLocation;
                        if (!string.IsNullOrEmpty(secilenAvatarYolu) && secilenAvatarYolu != mevcutAvatarYolu)
                        {
                            DialogResult dr = MessageBox.Show(
                                "Seçtiğiniz avatar kayıtlı olandan farklı. Değiştirmek istiyor musunuz?",
                                "Avatar Farklı",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (dr == DialogResult.Yes)
                            {

                                string updateQuery = "UPDATE dbo.Kullanici SET Avatar = @yeniAvatar WHERE Mail = @mail AND Sifre = @sifre";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti))
                                {
                                    updateCmd.Parameters.AddWithValue("@yeniAvatar", secilenAvatarYolu);
                                    updateCmd.Parameters.AddWithValue("@mail", mail);
                                    updateCmd.Parameters.AddWithValue("@sifre", sifre);
                                    updateCmd.ExecuteNonQuery();
                                }
                                MessageBox.Show("Avatar güncellendi.");
                            }
                            else
                            {
                                MessageBox.Show("Kayıtlı avatarı seçiniz!");
                                if (File.Exists(mevcutAvatarYolu))
                                {
                                    pictureBoxAvatar.Image = Image.FromFile(mevcutAvatarYolu);
                                    pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                                return;
                            }
                        }

                        MessageBox.Show("Giriş başarılı!");


                        if (chkBeniHatirla.Checked)
                        {
                            Properties.Settings.Default.Mail = mail;
                            Properties.Settings.Default.Sifre = sifre;
                            Properties.Settings.Default.AvatarPath = !string.IsNullOrEmpty(secilenAvatarYolu)
                                ? secilenAvatarYolu
                                : mevcutAvatarYolu;
                            Properties.Settings.Default.BeniHatirla = true;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.Mail = "";
                            Properties.Settings.Default.Sifre = "";
                            Properties.Settings.Default.AvatarPath = "";
                            Properties.Settings.Default.BeniHatirla = false;
                            Properties.Settings.Default.Save();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı bulunamadı!");
                    }
                }
            }
        }

        private void btnAvatarSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxAvatar.ImageLocation = ofd.FileName;
                pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void linkSifremiUnuttum_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SifreUnuttum sifreUnuttum = new SifreUnuttum();
            sifreUnuttum.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            yeniuye yeniuye = new yeniuye();
            yeniuye.ShowDialog();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string dilek = textBox1.Text.Trim();
            string sikayet = textBox2.Text.Trim();
            string oner = textBox3.Text.Trim();

            string emailBody = "DİLEK:\n" + dilek + "\n\n" +
                               "ŞİKAYET:\n" + sikayet + "\n\n" +
                               "ÖNERİ:\n" + oner;

            // 3. E-postayı gönder
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("sumeyyebdemir@gmail.com");

                mail.To.Add("sumeyyebdemir@gmail.com");

                mail.Subject = "oyun görüşü";
                mail.Body = emailBody;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("sumeyyebdemir@gmail.com", "tcii neht mnje rgeh");
                smtp.EnableSsl = true;

                smtp.Send(mail);

                MessageBox.Show(
         "Görüşleriniz için teşekkürler. Mesajınız başarıyla iletildi 💕",
         "Bilgi",
         MessageBoxButtons.OK,
         MessageBoxIcon.Information
     );
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken hata oluştu: " + ex.Message);
            }

        }

        private void hakkımızdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            tabControl1.SelectedTab = tabPage1;


        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (txtSifre.UseSystemPasswordChar)
            {
                txtSifre.UseSystemPasswordChar = false;
                pictureBox13.Image = Properties.Resources.gizle;
            }
            else
            {

                txtSifre.UseSystemPasswordChar = true;
                pictureBox13.Image = Properties.Resources.goster;
            }

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin admin = new admin();
            admin.Show();
             this.Hide();
        }
    }


}



