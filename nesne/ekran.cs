
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace nesne
{
    public partial class ekran : Form
    {
        string kullaniciAdi;
        string avatarYolu;
        string kullaniciYetki;

        private string connectionString = @"Data Source=BET\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True";
        private ToolTip tooltip = new ToolTip();

        public ekran(string ad, string avatarPath, string yetki)
        {
            InitializeComponent();
            this.kullaniciAdi = ad ?? "";
            this.avatarYolu = avatarPath ?? "";
            this.kullaniciYetki = yetki ?? "";
            this.StartPosition = FormStartPosition.CenterScreen;
            panel1.Visible = false;
            this.Load += ekran_Load;
        }


        private void ekran_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(kullaniciYetki) && kullaniciYetki.ToLower() == "admin")
            {
                btnAdminPanel.Visible = true;
            }
            else
            {
                btnAdminPanel.Visible = false;
            }

            textBox1.Text = "Bir dilek yaz...";
            textBox2.Text = "Sitem etmekten çekinme...";
            textBox3.Text = "Bir önerin var mı?";
            textBox1.ForeColor = textBox2.ForeColor = textBox3.ForeColor = Color.Gray;
            textBox1.BackColor = textBox2.BackColor = textBox3.BackColor = Color.FromArgb(255, 255, 230);

            textBox1.GotFocus += (s, ev) => { if (textBox1.Text.StartsWith("Bir dilek")) { textBox1.Text = ""; textBox1.ForeColor = Color.Black; } };
            textBox2.GotFocus += (s, ev) => { if (textBox2.Text.StartsWith("Sitem")) { textBox2.Text = ""; textBox2.ForeColor = Color.Black; } };
            textBox3.GotFocus += (s, ev) => { if (textBox3.Text.StartsWith("Bir öner")) { textBox3.Text = ""; textBox3.ForeColor = Color.Black; } };


            tooltip.SetToolTip(textBox1, "Dileklerini yaz. Belki gerçekleşir 🎁");
            tooltip.SetToolTip(textBox2, "Rahatsız olduğun bir şeyi bize bildir 💢");
            tooltip.SetToolTip(textBox3, "Her öneri bizim için değerli ⭐");
            tooltip.SetToolTip(pictureBox7, "Tıklayınca mesajın uçacak 💌");

            this.tabControl1.Selected += (s, ev) => { new SoundPlayer("sekme.wav").Play(); };

            label1.Text = kullaniciAdi;
            if (!string.IsNullOrEmpty(avatarYolu) && System.IO.File.Exists(avatarYolu))
            {
                pictureBox3.Image = Image.FromFile(avatarYolu);
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT TOP 1 Id, Dilek, Sikayet, Oneri FROM Gorusler WHERE KullaniciAdi = @ad ORDER BY Id DESC", con);
                    cmd1.Parameters.AddWithValue("@ad", kullaniciAdi);
                    int sonGorusId = -1;
                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sonGorusId = Convert.ToInt32(reader["Id"]);
                            label8.Text = reader["Dilek"].ToString();
                            label9.Text = reader["Sikayet"].ToString();
                            label10.Text = reader["Oneri"].ToString();
                        }
                        else
                        {
                            label8.Text = label9.Text = label10.Text = "Yok";
                        }
                    }

                    string adminMesaj = "";
                    SqlCommand cmd2 = new SqlCommand("SELECT Mesaj FROM Donusler WHERE KullaniciAdi = @ad AND GorusId = @gid", con);
                    cmd2.Parameters.AddWithValue("@ad", kullaniciAdi);
                    cmd2.Parameters.AddWithValue("@gid", sonGorusId);
                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    if (reader2.Read())
                    {
                        adminMesaj = reader2["Mesaj"].ToString();
                        label12.Text = adminMesaj;
                    }
                    else
                    {
                        label12.Text = "Bu iletinize henüz geri dönüş yapılmamış. Cevap bekleniyor...";
                    }
                    reader2.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken hata: " + ex.Message);
            }

            EskiIletileriYukle("Tarih DESC");
        }

        private void EskiIletileriYukle(string siralama)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = $@"
                        SELECT TOP 5 G.Id, G.Dilek, G.Sikayet, G.Oneri, G.Tarih,
                                     (SELECT TOP 1 Mesaj FROM Donusler D WHERE D.GorusId = G.Id) as AdminMesaji
                        FROM Gorusler G
                        WHERE G.KullaniciAdi = @ad
                        ORDER BY {siralama}";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ad", kullaniciAdi);
                    SqlDataReader reader = cmd.ExecuteReader();

                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.AppendLine("==============================");
                        sb.AppendLine($"🕒 {Convert.ToDateTime(reader["Tarih"]).ToShortDateString()} | ID: {reader["Id"]}");
                        sb.AppendLine($"Dilek: {reader["Dilek"]}");
                        sb.AppendLine($"Şikayet: {reader["Sikayet"]}");
                        sb.AppendLine($"Öneri: {reader["Oneri"]}");
                        sb.AppendLine("---");
                        string adminMesaji = reader["AdminMesaji"] != DBNull.Value ? reader["AdminMesaji"].ToString() : "(Henüz admin mesajı yok.)";
                        sb.AppendLine($"👑 Admin Mesajı: {adminMesaji}");
                        sb.AppendLine();
                    }

                    richTextBox1.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eski iletiler yüklenirken hata: " + ex.Message);
            }

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string dilek = textBox1.Text.Trim();
            string sikayet = textBox2.Text.Trim();
            string oner = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(dilek) && string.IsNullOrWhiteSpace(sikayet) && string.IsNullOrWhiteSpace(oner))
            {
                MessageBox.Show("Lütfen en az bir alanı doldurun.");
                return;
            }

            string kullaniciMail = "";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Kullanıcının e-posta adresini çek
                    SqlCommand mailCmd = new SqlCommand("SELECT Mail FROM Kullanici WHERE Isim = @ad", con);
                    mailCmd.Parameters.AddWithValue("@ad", kullaniciAdi);
                    object result = mailCmd.ExecuteScalar();
                    if (result != null)
                        kullaniciMail = result.ToString();

                    // Görüşü veritabanına kaydet
                    SqlCommand cmd = new SqlCommand("INSERT INTO Gorusler (KullaniciAdi, Dilek, Sikayet, Oneri) VALUES (@ad, @d, @s, @o)", con);
                    cmd.Parameters.AddWithValue("@ad", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@d", dilek);
                    cmd.Parameters.AddWithValue("@s", sikayet);
                    cmd.Parameters.AddWithValue("@o", oner);
                    cmd.ExecuteNonQuery();
                }

                string body = $"Merhaba {kullaniciAdi},\n\nGörüşleriniz başarıyla gönderilmiştir.\n\n---\nDilek: {dilek}\nŞikayet: {sikayet}\nÖneri: {oner}\n\nİyi günler dileriz.";

                MailMessage mailToAdmin = new MailMessage();
                mailToAdmin.From = new MailAddress("sumeyyebdemir@gmail.com");
                mailToAdmin.To.Add("sumeyyebdemir@gmail.com");
                mailToAdmin.Subject = "Yeni Kullanıcı Görüşü";
                mailToAdmin.Body = body;

                MailMessage mailToUser = new MailMessage();
                mailToUser.From = new MailAddress("sumeyyebdemir@gmail.com");
                mailToUser.To.Add(kullaniciMail);
                mailToUser.Subject = "Görüşünüz Ulaştı!";
                mailToUser.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("sumeyyebdemir@gmail.com", "auhr cgkw buti kypp");
                smtp.EnableSsl = true;
                smtp.Send(mailToAdmin);
                smtp.Send(mailToUser);

                MessageBox.Show("💌 Görüşleriniz başarıyla gönderildi ve size e-posta da gönderildi!", "Tebrikler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new SoundPlayer("gonderildi.wav").Play();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gönderim hatası: " + ex.Message);
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e) => panel1.Visible = true;
        private void pictureBox10_Click(object sender, EventArgs e) => panel1.Visible = false;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            VideoForm frm = new VideoForm(this.avatarYolu);
            frm.Show();
            this.Hide();
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            üye frm = new üye();
            frm.Show();
            this.Hide();
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnZAZ_Click(object sender, EventArgs e)
        {
            EskiIletileriYukle("Dilek DESC");
        }

        private void btnAZ_Click(object sender, EventArgs e)
        {
            EskiIletileriYukle("Dilek ASC");
        }

        private void btnTarih_Click(object sender, EventArgs e)
        {
            EskiIletileriYukle("Tarih DESC");
        }

        private void btnAdminPaneli_Click(object sender, EventArgs e)
        {
            admin admin = new admin();
            admin.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            skor frm = new skor(this.kullaniciAdi);
            frm.Show();
        }
    }
}
