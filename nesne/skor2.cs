using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nesne
{
    public partial class skor2 : Form
    {
        private string connectionString = @"Data Source=BET\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True";
        private DataTable orijinalData;

        public skor2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            cmbFiltre.Items.AddRange(new string[] { "Günlük", "Haftalık", "Aylık", "Yıllık", "Tümü" });
            cmbFiltre.SelectedIndex = 0;
        }

        private void skor2_Load(object sender, EventArgs e)
        {
            SkorlariYukle("Tümü");
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            SkorlariYukle(cmbFiltre.SelectedItem.ToString());
        }

        private void SkorlariYukle(string filtre)
        {
            string query = "";

            switch (filtre)
            {
                case "Günlük":
                    query = "SELECT * FROM Skor WHERE CAST(Tarih AS DATE) = CAST(GETDATE() AS DATE)";
                    break;
                case "Haftalık":
                    query = "SELECT * FROM Skor WHERE DATEDIFF(WEEK, Tarih, GETDATE()) = 0";
                    break;
                case "Aylık":
                    query = "SELECT * FROM Skor WHERE MONTH(Tarih) = MONTH(GETDATE()) AND YEAR(Tarih) = YEAR(GETDATE())";
                    break;
                case "Yıllık":
                    query = "SELECT * FROM Skor WHERE YEAR(Tarih) = YEAR(GETDATE())";
                    break;
                case "Tümü":
                    query = "SELECT * FROM Skor";
                    break;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                orijinalData = dt.Copy(); // Temizle butonunda geri dönebilmek için
            }
        }

        private void btnTop5_Click(object sender, EventArgs e)
        {
            if (orijinalData == null) return;

            var top5 = orijinalData.AsEnumerable()
                        .OrderBy(r => Convert.ToInt32(r["Sure"]))
                        .Take(5);

            DataTable filtered = orijinalData.Clone();
            foreach (var row in top5)
                filtered.ImportRow(row);

            dataGridView1.DataSource = filtered;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (orijinalData == null) return;

            var maxRow = orijinalData.AsEnumerable()
                .OrderByDescending(r => Convert.ToInt32(r["Sure"]))
                .FirstOrDefault();

            if (maxRow != null)
            {
                DataTable dt = orijinalData.Clone();
                dt.ImportRow(maxRow);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (orijinalData == null) return;

            var minRow = orijinalData.AsEnumerable()
                .OrderBy(r => Convert.ToInt32(r["Sure"]))
                .FirstOrDefault();

            if (minRow != null)
            {
                DataTable dt = orijinalData.Clone();
                dt.ImportRow(minRow);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnAZ_Click(object sender, EventArgs e)
        {
            if (orijinalData == null) return;

            var sorted = orijinalData.AsEnumerable()
                .OrderBy(r => Convert.ToInt32(r["Sure"]));

            DataTable dt = orijinalData.Clone();
            foreach (var row in sorted)
                dt.ImportRow(row);

            dataGridView1.DataSource = dt;
        }

        private void btnZA_Click(object sender, EventArgs e)
        {
            if (orijinalData == null) return;

            var sorted = orijinalData.AsEnumerable()
                .OrderByDescending(r => Convert.ToInt32(r["Sure"]));

            DataTable dt = orijinalData.Clone();
            foreach (var row in sorted)
                dt.ImportRow(row);

            dataGridView1.DataSource = dt;
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = orijinalData;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

            skor frm = new skor();
            frm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnarama_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtArama.Text.Trim();

            if (string.IsNullOrEmpty(aramaMetni))
            {
                MessageBox.Show("Lütfen arama için tarih bilgisi girin (örnek: 2024 veya 06 veya 06.2024 veya 08.06.2024)", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                dataGridView1.DataSource = dt;
            }
        }

    }
}
