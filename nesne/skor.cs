using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace nesne
{
    public partial class skor : Form
    {
        private string connectionString = @"Data Source=BET\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True";
        private string kullaniciAdi;
        private string avatarYolu;
        private string kullaniciYetki;
        private string? avatarPath;

        public skor(string kullaniciAdi)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.StartPosition = FormStartPosition.CenterScreen;

            cmbFiltre.Items.AddRange(new string[] { "Günlük", "Haftalık", "Aylık", "Yıllık", "Top5", "Tümü" });
            cmbFiltre.SelectedIndex = 0;
        }

        public skor()
        {
        }

        private void skor_Load(object sender, EventArgs e)
        {

        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            string filtre = cmbFiltre.SelectedItem.ToString();
            string query = "SELECT Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi";

            if (filtre == "Günlük")
                query += " AND CAST(Tarih AS DATE) = CAST(GETDATE() AS DATE)";
            else if (filtre == "Haftalık")
                query += " AND DATEPART(WEEK, Tarih) = DATEPART(WEEK, GETDATE()) AND YEAR(Tarih) = YEAR(GETDATE())";
            else if (filtre == "Aylık")
                query += " AND MONTH(Tarih) = MONTH(GETDATE()) AND YEAR(Tarih) = YEAR(GETDATE())";
            else if (filtre == "Yıllık")
                query += " AND YEAR(Tarih) = YEAR(GETDATE())";
            else if (filtre == "Top5")
                query += " ORDER BY Sure ASC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnArama_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtArama.Text.Trim();

            if (string.IsNullOrEmpty(aramaMetni))
            {
                MessageBox.Show("Lütfen arama için tarih bilgisi girin (örnek: 2024 veya 06 veya 06.2024 veya 08.06.2024)", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi";
            bool paramEklendi = false;

            string[] parts = aramaMetni.Split('.');

            if (parts.Length == 1) // Yıl veya Ay olabilir
            {
                if (parts[0].Length == 4 && parts[0].All(char.IsDigit)) // Yıl
                {
                    query += " AND YEAR(Tarih) = @yil";
                    paramEklendi = true;
                }
                else if (parts[0].Length <= 2 && parts[0].All(char.IsDigit)) // Ay
                {
                    query += " AND MONTH(Tarih) = @ay";
                    paramEklendi = true;
                }
            }
            else if (parts.Length == 2)
            {
                if (parts[0].Length <= 2 && parts[1].Length == 4 && parts.All(p => p.All(char.IsDigit))) // Ay + Yıl
                {
                    query += " AND MONTH(Tarih) = @ay AND YEAR(Tarih) = @yil";
                    paramEklendi = true;
                }
                else if (parts[0].Length <= 2 && parts[1].Length <= 2 && parts.All(p => p.All(char.IsDigit))) // Gün + Ay
                {
                    query += " AND DAY(Tarih) = @gun AND MONTH(Tarih) = @ay";
                    paramEklendi = true;
                }
            }
            else if (parts.Length == 3) // Gün + Ay + Yıl
            {
                if (parts.All(p => p.All(char.IsDigit)))
                {
                    query += " AND DAY(Tarih) = @gun AND MONTH(Tarih) = @ay AND YEAR(Tarih) = @yil";
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
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

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
                    else // Gün + Ay
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
                dataGridView1.DataSource = dt;
            }
        }


        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ekran frm = new ekran(kullaniciAdi, avatarYolu, kullaniciYetki);
            frm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            skor2 frm = new skor2();
            frm.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // MIN
            string query = "SELECT TOP 1 Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi ORDER BY Sure ASC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // MAX
            string query = "SELECT TOP 1 Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi ORDER BY Sure DESC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // TOP 5
            string query = "SELECT TOP 5 Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi ORDER BY Sure ASC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            // ARTAN
            string query = "SELECT Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi ORDER BY Sure ASC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            // AZALAN
            string query = "SELECT Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi ORDER BY Sure DESC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            // TEMIZLE 
            string query = "SELECT Sure, Tarih FROM Skor WHERE KullaniciAdi = @kullaniciAdi";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
