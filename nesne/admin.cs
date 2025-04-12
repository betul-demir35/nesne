using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel; 

namespace nesne
{    
    public partial class admin : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True;";
        private bool siralamaTers = false;
        private bool artan = true;

        public admin()
        {
            InitializeComponent();
            KullanicilariGetir();
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
        }
        private void KullanicilariGetir()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT * FROM Kullanici";
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Kullanici (Isim, Soyisim, Mail, Sifre, Avatar) " +
                               "VALUES (@Isim, @Soyisim, @Mail, @Sifre, @Avatar)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Isim", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Soyisim", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Mail", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Sifre", textBox4.Text);

                    if (pictureBox1.Tag != null)
                        cmd.Parameters.AddWithValue("@Avatar", pictureBox1.Tag.ToString());
                    else
                        cmd.Parameters.AddWithValue("@Avatar", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            KullanicilariGetir();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox5.Text = row.Cells["Id"].Value.ToString();
                textBox1.Text = row.Cells["Isim"].Value.ToString();
                textBox2.Text = row.Cells["Soyisim"].Value.ToString();
                textBox3.Text = row.Cells["Mail"].Value.ToString();
                textBox4.Text = row.Cells["Sifre"].Value.ToString();

                if (row.Cells["Avatar"].Value != DBNull.Value)
                {
                    string avatarPath = row.Cells["Avatar"].Value.ToString();
                    if (File.Exists(avatarPath))
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
                else
                {
                    pictureBox1.Image = null;
                    pictureBox1.Tag = null;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Silme işlemi için önce tablodan bir kayıt seçmelisiniz.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "DELETE FROM Kullanici WHERE Id=@ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(textBox5.Text));
                    cmd.ExecuteNonQuery();
                }
            }
            KullanicilariGetir();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Güncelleme için önce tablodan bir kayıt seçmelisiniz.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Kullanici SET " +
                               "Isim=@Isim, Soyisim=@Soyisim, Mail=@Mail, Sifre=@Sifre, Avatar=@Avatar " +
                               "WHERE Id=@ID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(textBox5.Text));
                    cmd.Parameters.AddWithValue("@Isim", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Soyisim", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Mail", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Sifre", textBox4.Text);

                    if (pictureBox1.Tag != null)
                        cmd.Parameters.AddWithValue("@Avatar", pictureBox1.Tag.ToString());
                    else
                        cmd.Parameters.AddWithValue("@Avatar", DBNull.Value);

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
                string query = "SELECT * FROM Kullanici ORDER BY Isim " + sort;
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bir resim dosyası seçiniz...";
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
                pictureBox1.Tag = ofd.FileName;
            }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Kullanici";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;
            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            worksheet.Name = "VeriAktarim";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Dosyası|*.xlsx";
            sfd.Title = "Excel Kaydet";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName);
            }

            workbook.Close();
            excelApp.Quit();
            MessageBox.Show("Excel'e aktarma işlemi tamamlandı.");
        }


        private void btnExportTxt_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Kullanici";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Metin Dosyası|*.txt";
            sfd.Title = "TXT Kaydet";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sw.Write(dt.Columns[i].ColumnName);
                        if (i < dt.Columns.Count - 1) sw.Write("\t");
                    }
                    sw.WriteLine();


                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(row[i].ToString());
                            if (i < dt.Columns.Count - 1) sw.Write("\t");
                        }
                        sw.WriteLine();
                    }
                }
                MessageBox.Show("TXT aktarımı tamamlandı!");
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Kullanici WHERE Id=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
                        cmd.ExecuteNonQuery();
                    }
                }
                KullanicilariGetir();
                MessageBox.Show("Kayıt silindi.");
            }
        }
    }
}
