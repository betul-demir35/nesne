using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace nesne
{
    public partial class admin : Form
    {
        private string kullaniciAdi;
        private string connectionString = @"Data Source=BET\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True";
        private bool siralamaTers = false;
        private bool artan = true;
        private string secilenKullaniciAdi = "";
        private string secilenMail = "";
        private int secilenDonusId = -1;
        private DataTable skorOrijinalData;
        public admin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            KullanicilariGetir();
            GorusleriGetir();
            DonusleriGetir();
            SkorlariGetir();


        }
        private void KullanicilariGetir()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kullanici", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }
        private void kullanıcılar_Load(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Admin Yap", null, AdminYap);
            menu.Items.Add("Kullanıcı Yap", null, KullaniciYap);
            menu.Items.Add("Engelle", null, Engelle);
            menu.Items.Add("Engeli Kaldır", null, EngelKaldir);
            dataGridView1.ContextMenuStrip = menu;
            KullaniciListesiniYukle(); // Başlangıç verisi
        }
        private void KullaniciListesiniYukle()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kullanici", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void AdminYap(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string mail = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); // ✅ Mail kolonu doğru
                YetkiGuncelle(mail, "admin");
            }
        }

        private void KullaniciYap(object sender, EventArgs e)
        {
            string mail = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); // ✅
            YetkiGuncelle(mail, "kullanici");
        }

        private void Engelle(object sender, EventArgs e)
        {
            string mail = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); // ✅
            EngelDurumuGuncelle(mail, true);
        }

        private void EngelKaldir(object sender, EventArgs e)
        {
            string mail = dataGridView1.CurrentRow.Cells["Mail"].Value.ToString(); // ✅
            EngelDurumuGuncelle(mail, false);
        }

        private void YetkiGuncelle(string mail, string yetki)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE kullanici SET Yetki = @yetki WHERE Mail = @mail", con);
                cmd.Parameters.AddWithValue("@yetki", yetki);
                cmd.Parameters.AddWithValue("@mail", mail);

                int sonuc = cmd.ExecuteNonQuery();

                if (sonuc > 0)
                    MessageBox.Show("Yetki başarıyla güncellendi.");
                else
                    MessageBox.Show("Yetki güncellenemedi! Mail doğru olmayabilir.");

                KullaniciListesiniYukle();
            }
        }


        private void EngelDurumuGuncelle(string mail, bool engelliMi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // ENGEL sütunu var mı ve düzgün yazıldı mı?
                SqlCommand cmd = new SqlCommand("UPDATE kullanici SET Engel = @engel WHERE Mail = @mail", con);
                cmd.Parameters.AddWithValue("@engel", engelliMi ? 1 : 0);
                cmd.Parameters.AddWithValue("@mail", mail);

                int etkilenen = cmd.ExecuteNonQuery();

                if (etkilenen > 0)
                    MessageBox.Show("Engel durumu güncellendi.");
                else
                    MessageBox.Show("Engel güncellenemedi! Kullanıcı maili hatalı olabilir.");

                KullaniciListesiniYukle(); // DataGridView yenile
            }
        }
        private void GorusleriGetir()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Id, KullaniciAdi, Dilek, Sikayet, Oneri, Tarih FROM Gorusler ORDER BY Tarih DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
        }
        private void DonusleriGetir()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Id, KullaniciAdi, Mail, Mesaj, Tarih FROM Donusler ORDER BY Tarih DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt;
                }
            }
        }


        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView2.Rows[e.RowIndex];
                secilenKullaniciAdi = row.Cells["KullaniciAdi"].Value.ToString();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Mail FROM Kullanici WHERE Isim = @Isim", con))
                    {
                        cmd.Parameters.AddWithValue("@Isim", secilenKullaniciAdi);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            secilenMail = result.ToString();
                    }
                }
            }
            DonusleriGetir();
        }

        private void buttonGeriDonus_Click(object sender, EventArgs e)
        {
            string mesaj = richTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(secilenMail) || string.IsNullOrEmpty(secilenKullaniciAdi))
            {
                MessageBox.Show("Lütfen önce bir kullanıcı seçin.");
                return;
            }

            if (string.IsNullOrEmpty(mesaj))
            {
                MessageBox.Show("Mesaj alanı boş olamaz.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Donusler (KullaniciAdi, Mail, Mesaj) VALUES (@ad, @mail, @mesaj)", con))
                    {
                        cmd.Parameters.AddWithValue("@ad", secilenKullaniciAdi);
                        cmd.Parameters.AddWithValue("@mail", secilenMail);
                        cmd.Parameters.AddWithValue("@mesaj", mesaj);
                        cmd.ExecuteNonQuery();
                    }
                }

                MailMessage mail = new MailMessage("sumeyyebdemir@gmail.com", secilenMail)
                {
                    Subject = "Görüşünüze Cevap Geldi!",
                    Body = $"Merhaba {secilenKullaniciAdi},\n\nYönetici tarafından size gelen cevap:\n\n\"{mesaj}\"\n\nİyi günler dileriz."
                };

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("sumeyyebdemir@gmail.com", "ymql paya drac ehuk");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                MessageBox.Show("Geri dönüş yapıldı ve mail gönderildi.");
                richTextBox1.Clear();
                DonusleriGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView3.Rows[e.RowIndex];
                secilenDonusId = Convert.ToInt32(row.Cells["Id"].Value);
                richTextBox1.Text = row.Cells["Mesaj"].Value.ToString();
                secilenKullaniciAdi = row.Cells["KullaniciAdi"].Value.ToString();
                secilenMail = row.Cells["Mail"].Value.ToString();
                MessageBox.Show("Geri dönüş düzenleme modunda.");
            }
            GorusleriGetir();
        }

        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            if (secilenDonusId == -1 || string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Lütfen geçerli bir geri dönüş seçin ve mesaj yazın.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Donusler SET Mesaj=@Mesaj WHERE Id=@Id", con))
                {
                    cmd.Parameters.AddWithValue("@Mesaj", richTextBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Id", secilenDonusId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Güncellendi.");
            DonusleriGetir();
            richTextBox1.Clear();
            secilenDonusId = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Kullanici (Isim, Soyisim, Mail, Sifre, Avatar) VALUES (@Isim, @Soyisim, @Mail, @Sifre, @Avatar)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Isim", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Soyisim", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Mail", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Sifre", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Avatar", pictureBox1.Tag ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            KullanicilariGetir();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                textBox5.Text = row.Cells["Id"].Value.ToString();
                textBox1.Text = row.Cells["Isim"].Value.ToString();
                textBox2.Text = row.Cells["Soyisim"].Value.ToString();
                textBox3.Text = row.Cells["Mail"].Value.ToString();
                textBox4.Text = row.Cells["Sifre"].Value.ToString();

                string avatarPath = row.Cells["Avatar"].Value?.ToString();
                if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                {
                    pictureBox1.Image = Image.FromFile(avatarPath);
                    pictureBox1.Tag = avatarPath;
                }
                else
                {
                    pictureBox1.Image = null;
                    pictureBox1.Tag = null;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox5.Text, out int id))
            {
                MessageBox.Show("Geçerli bir kullanıcı seçmelisiniz.");
                return;
            }

            DialogResult result = MessageBox.Show("Seçilen kullanıcıyı silmek istediğinizden emin misiniz?",
                                                  "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Kullanici WHERE Id=@ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            int affected = cmd.ExecuteNonQuery();
                            if (affected > 0)
                                MessageBox.Show("Kullanıcı başarıyla silindi.");
                            else
                                MessageBox.Show("Kullanıcı bulunamadı. Silinemedi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message);
                }

                KullanicilariGetir();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Güncellemek için bir kullanıcı seçmelisiniz.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Kullanici SET Isim=@Isim, Soyisim=@Soyisim, Mail=@Mail, Sifre=@Sifre, Avatar=@Avatar WHERE Id=@ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(textBox5.Text));
                    cmd.Parameters.AddWithValue("@Isim", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Soyisim", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Mail", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Sifre", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Avatar", pictureBox1.Tag ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            KullanicilariGetir();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sort = artan ? "ASC" : "DESC";
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kullanici ORDER BY Isim " + sort, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            artan = !artan;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Bir resim dosyası seçiniz...";
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    pictureBox1.Tag = ofd.FileName;
                }
            }
        }

        private void btnExportTxt_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kullanici", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Metin Dosyası|*.txt";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter sw = new StreamWriter(sfd.FileName))
                            {
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    sw.Write(dt.Columns[i].ColumnName + (i < dt.Columns.Count - 1 ? "\t" : ""));
                                }
                                sw.WriteLine();
                                foreach (DataRow row in dt.Rows)
                                {
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        sw.Write(row[i].ToString() + (i < dt.Columns.Count - 1 ? "\t" : ""));
                                    }
                                    sw.WriteLine();
                                }
                            }
                            MessageBox.Show("TXT aktarımı tamamlandı!");
                        }
                    }
                }
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Kullanici WHERE Id=@ID", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
                        cmd.ExecuteNonQuery();
                    }
                }
                KullanicilariGetir();
                MessageBox.Show("Kayıt silindi.");
            }
        }
        private void btnMax_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is not DataTable dt)
            {
                MessageBox.Show("DataTable bağlı değil.");
                return;
            }

            if (!dt.Columns.Contains("Isim"))
            {
                MessageBox.Show("Isim sütunu bulunamadı.");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Veri yok.");
                return;
            }

            var maxRow = dt.AsEnumerable()
                .Where(r => !string.IsNullOrEmpty(r["Isim"]?.ToString()))
                .OrderByDescending(r => r["Isim"].ToString().Length)
                .FirstOrDefault();

            if (maxRow != null)
            {
                DataTable filtered = dt.Clone();
                filtered.ImportRow(maxRow);
                dataGridView1.DataSource = filtered;

              
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is DataTable dt && dt.Columns.Contains("Isim"))
            {
                if (dt.Rows.Count > 0)
                {
                    var minRow = dt.AsEnumerable()
                        .OrderBy(r => r["Isim"]?.ToString().Length ?? 0)
                        .FirstOrDefault();

                    if (minRow != null)
                    {
                        DataTable filtered = dt.Clone();
                        filtered.ImportRow(minRow);
                        dataGridView1.DataSource = filtered;
                    }
                }
            }
            else
            {
                MessageBox.Show("Isim sütunu bulunamadı!");
            }
        }
        private void btnTop5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is not DataTable dt)
            {
                MessageBox.Show("DataTable bağlı değil.");
                return;
            }

            if (!dt.Columns.Contains("Id"))
            {
                MessageBox.Show("Id sütunu bulunamadı!");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Veri yok.");
                return;
            }

            try
            {
                // Null değerleri filtreleyip sıralama yapalım
                var top5 = dt.AsEnumerable()
                    .Where(r => r["Id"] != DBNull.Value)
                    .OrderByDescending(r => Convert.ToInt32(r["Id"]))
                    .Take(5);

                DataTable filtered = dt.Clone();
                foreach (var row in top5)
                    filtered.ImportRow(row);

                dataGridView1.DataSource = filtered;

             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            KullaniciListesiniYukle();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            üye frm = new üye();
            frm.Show();
            this.Hide();
        }
        private void MailGonder(string aliciMail, string aliciIsim, string mesaj)
        {
            MailMessage mail = new MailMessage("sumeyyebdemir@gmail.com", aliciMail)
            {
                Subject = "Görüşünüze Yanıt Geldi!",
                Body = $"Merhaba {aliciIsim},\n\nYönetici yanıtı:\n\"{mesaj}\"\n\nTeşekkür ederiz.",
                IsBodyHtml = false
            };

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("sumeyyebdemir@gmail.com", "omne jqnr pbkv adrs");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
        private void SkorlariGetir()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Skor", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView4.DataSource = dt;
                skorOrijinalData = dt.Copy();
            }
        }
        private void btnSkorEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSkorAd.Text) || string.IsNullOrEmpty(txtSure.Text)) return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Skor (KullaniciAdi, Sure, Tarih) VALUES (@ad, @sure, @tarih)", con);
                cmd.Parameters.AddWithValue("@ad", txtSkorAd.Text);
                cmd.Parameters.AddWithValue("@sure", Convert.ToInt32(txtSure.Text));
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            SkorlariGetir();
        }

        private void btnSkorSil_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Skor WHERE Id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            SkorlariGetir();
        }

        private void btnSkorGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count == 0) return;
            int id = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["Id"].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Skor SET KullaniciAdi=@ad, Sure=@sure WHERE Id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ad", txtSkorAd.Text);
                cmd.Parameters.AddWithValue("@sure", Convert.ToInt32(txtSure.Text));
                cmd.ExecuteNonQuery();
            }
            SkorlariGetir();
        }

        private void btnSkorTop5_Click(object sender, EventArgs e)
        {
            if (skorOrijinalData == null) return;
            var top5 = skorOrijinalData.AsEnumerable()
                .OrderBy(r => Convert.ToInt32(r["Sure"])).Take(5);

            DataTable dt = skorOrijinalData.Clone();
            foreach (var row in top5) dt.ImportRow(row);
            dataGridView4.DataSource = dt;
        }

        private void btnSkorMin_Click(object sender, EventArgs e)
        {
            if (skorOrijinalData == null) return;
            var row = skorOrijinalData.AsEnumerable().OrderBy(r => Convert.ToInt32(r["Sure"])).FirstOrDefault();
            if (row != null)
            {
                DataTable dt = skorOrijinalData.Clone();
                dt.ImportRow(row);
                dataGridView4.DataSource = dt;
            }
        }

        private void btnSkorMax_Click(object sender, EventArgs e)
        {
            if (skorOrijinalData == null) return;
            var row = skorOrijinalData.AsEnumerable().OrderByDescending(r => Convert.ToInt32(r["Sure"])).FirstOrDefault();
            if (row != null)
            {
                DataTable dt = skorOrijinalData.Clone();
                dt.ImportRow(row);
                dataGridView4.DataSource = dt;
            }
        }

        private void btnSkorAZ_Click(object sender, EventArgs e)
        {
            if (skorOrijinalData == null) return;
            var sorted = skorOrijinalData.AsEnumerable().OrderBy(r => Convert.ToInt32(r["Sure"]));
            DataTable dt = skorOrijinalData.Clone();
            foreach (var row in sorted) dt.ImportRow(row);
            dataGridView4.DataSource = dt;
        }

        private void btnSkorZA_Click(object sender, EventArgs e)
        {
            if (skorOrijinalData == null) return;
            var sorted = skorOrijinalData.AsEnumerable().OrderByDescending(r => Convert.ToInt32(r["Sure"]));
            DataTable dt = skorOrijinalData.Clone();
            foreach (var row in sorted) dt.ImportRow(row);
            dataGridView4.DataSource = dt;
        }

        private void btnSkorTemizle_Click(object sender, EventArgs e)
        {
            dataGridView4.DataSource = skorOrijinalData;
        }
        private void btnara_Click(object sender, EventArgs e)
        {
            string arama = txtara.Text.Trim();
            if (string.IsNullOrEmpty(arama))
            {
                KullaniciListesiniYukle();
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Kullanici WHERE Isim LIKE @ara OR Soyisim LIKE @ara OR Mail LIKE @ara", con);
                da.SelectCommand.Parameters.AddWithValue("@ara", "%" + arama + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void btnara2_Click(object sender, EventArgs e)
        {
            string arama = txtara2.Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Görüşler için
                SqlDataAdapter daGorus = new SqlDataAdapter(
                    "SELECT Id, KullaniciAdi, Dilek, Sikayet, Oneri, Tarih FROM Gorusler " +
                    "WHERE KullaniciAdi LIKE @ara OR Dilek LIKE @ara OR Sikayet LIKE @ara OR Oneri LIKE @ara", con);
                daGorus.SelectCommand.Parameters.AddWithValue("@ara", "%" + arama + "%");

                DataTable dtGorus = new DataTable();
                daGorus.Fill(dtGorus);
                dataGridView2.DataSource = dtGorus;

                // Donusler için
                SqlDataAdapter daDonus = new SqlDataAdapter(
                    "SELECT Id, KullaniciAdi, Mail, Mesaj, Tarih FROM Donusler " +
                    "WHERE KullaniciAdi LIKE @ara OR Mail LIKE @ara OR Mesaj LIKE @ara", con);
                daDonus.SelectCommand.Parameters.AddWithValue("@ara", "%" + arama + "%");

                DataTable dtDonus = new DataTable();
                daDonus.Fill(dtDonus);
                dataGridView3.DataSource = dtDonus;
            }


        }

        private void btnara3_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtara3.Text.Trim();

            if (string.IsNullOrEmpty(aramaMetni))
            {
                SkorlariGetir(); // Boşsa tüm skorları getir
                return;
            }

            string query = "SELECT * FROM Skor";
            bool paramEklendi = false;

            string[] parts = aramaMetni.Split('.');

            if (parts.Length == 1)
            {
                if (parts[0].Length == 4 && parts[0].All(char.IsDigit))
                {
                    query += " WHERE YEAR(Tarih) = @yil";
                    paramEklendi = true;
                }
                else if (parts[0].Length <= 2 && parts[0].All(char.IsDigit))
                {
                    query += " WHERE MONTH(Tarih) = @ay";
                    paramEklendi = true;
                }
            }
            else if (parts.Length == 2)
            {
                if (parts[0].Length <= 2 && parts[1].Length == 4 && parts.All(p => p.All(char.IsDigit)))
                {
                    query += " WHERE MONTH(Tarih) = @ay AND YEAR(Tarih) = @yil";
                    paramEklendi = true;
                }
                else if (parts[0].Length <= 2 && parts[1].Length <= 2 && parts.All(p => p.All(char.IsDigit)))
                {
                    query += " WHERE DAY(Tarih) = @gun AND MONTH(Tarih) = @ay";
                    paramEklendi = true;
                }
            }
            else if (parts.Length == 3)
            {
                if (parts.All(p => p.All(char.IsDigit)))
                {
                    query += " WHERE DAY(Tarih) = @gun AND MONTH(Tarih) = @ay AND YEAR(Tarih) = @yil";
                    paramEklendi = true;
                }
            }

            if (!paramEklendi)
            {
                MessageBox.Show("Geçersiz format! Örnek: 2024 veya 06 veya 06.2024 veya 08.06.2024", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                if (parts.Length == 1)
                {
                    if (query.Contains("@yil"))
                        cmd.Parameters.AddWithValue("@yil", int.Parse(parts[0]));
                    else if (query.Contains("@ay"))
                        cmd.Parameters.AddWithValue("@ay", int.Parse(parts[0]));
                }
                else if (parts.Length == 2)
                {
                    if (query.Contains("@yil"))
                    {
                        cmd.Parameters.AddWithValue("@ay", int.Parse(parts[0]));
                        cmd.Parameters.AddWithValue("@yil", int.Parse(parts[1]));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@gun", int.Parse(parts[0]));
                        cmd.Parameters.AddWithValue("@ay", int.Parse(parts[1]));
                    }
                }
                else if (parts.Length == 3)
                {
                    cmd.Parameters.AddWithValue("@gun", int.Parse(parts[0]));
                    cmd.Parameters.AddWithValue("@ay", int.Parse(parts[1]));
                    cmd.Parameters.AddWithValue("@yil", int.Parse(parts[2]));
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void btnTemizle23_Click(object sender, EventArgs e)
        {
            GorusleriGetir();
            DonusleriGetir();
            txtara2.Clear();

        }

        private void btnara4_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtara4.Text.Trim();

            if (string.IsNullOrEmpty(aramaMetni))
            {
                SkorlariGetir(); // Boşsa tüm skorları getir
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Skor WHERE KullaniciAdi LIKE @ara", con);
                da.SelectCommand.Parameters.AddWithValue("@ara", "%" + aramaMetni + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            üye frm = new üye();
            frm.Show();
            this.Hide();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            üye frm = new üye();
            frm.Show();
            this.Hide();
        }
    }
}

