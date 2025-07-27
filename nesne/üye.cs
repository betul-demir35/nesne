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
            if (Properties.Settings.Default.BeniHatirla)
            {
                txtMail.Text = Properties.Settings.Default.Mail;
                txtSifre.Text = Properties.Settings.Default.Sifre;
                chkBeniHatirla.Checked = Properties.Settings.Default.BeniHatirla;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.AvatarPath) &&
                    File.Exists(Properties.Settings.Default.AvatarPath))
                {
                    pictureBoxAavacatar.Image = Image.FromFile(Properties.Settings.Default.AvatarPath);
                    pictureBoxAavacatar.SizeMode = PictureBoxSizeMode.Zoom;
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
                string soundPath = @"C:\Users\betul\OneDrive\Desktop\ödevler\nesne\nesne\bin\Debug\net8.0-windows\load.wav";

                player = new SoundPlayer("load.wav");
                player.Play();
                isSoundOn = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
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
            string secilenAvatarYolu = pictureBoxAavacatar.ImageLocation;
            string isim = "";
            string yetki = "kullanici";
            bool engelliMi = false;

            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen mail ve şifre alanlarını doldurun.");
                return;
            }

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT TOP 1 Isim, Sifre, Avatar, Yetki, Engel FROM dbo.Kullanici WHERE Mail = @mail";
                    using (SqlCommand komut = new SqlCommand(sorgu, baglanti))
                    {
                        komut.Parameters.AddWithValue("@mail", mail);

                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isim = reader["Isim"].ToString();
                                string dbSifre = reader["Sifre"].ToString();
                                mevcutAvatarYolu = reader["Avatar"].ToString();

                                yetki = reader["Yetki"] != DBNull.Value ? reader["Yetki"].ToString() : "kullanici";
                                engelliMi = reader["Engel"] != DBNull.Value && Convert.ToBoolean(reader["Engel"]);
                                if (dbSifre != sifre)
                                {
                                    MessageBox.Show("Yetki: " + yetki + "\nEngel: " + engelliMi.ToString());

                                    return;
                                }

                                if (engelliMi)
                                {
                                    MessageBox.Show("Bu kullanıcı engellenmiştir. Giriş yapamazsınız.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı bulunamadı!");
                                return;
                            }
                        }

                        // Avatar kontrolü
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
                                    pictureBoxAavacatar.Image = Image.FromFile(mevcutAvatarYolu);
                                    pictureBoxAavacatar.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                                return;
                            }
                        }

                        // Beni hatırla ayarları
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

                        
                        MessageBox.Show("Giriş başarılı!");

                        ekran frm = new ekran(isim, mevcutAvatarYolu, yetki);
                        frm.Show();
                        this.Hide();



                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }




        private void btnAvatarSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxAavacatar.ImageLocation = ofd.FileName;
                pictureBoxAavacatar.SizeMode = PictureBoxSizeMode.Zoom;
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



