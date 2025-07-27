using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using nesne;

namespace nesne
{
    public partial class level2 : Form
    {
        Oyuncu sirin = new Oyuncu();
        dusman gargamel = new dusman();
        Random rnd = new Random();
        int sure = 600;
        int hareketYon = 1;
        int gargamelHizi = 5;
        int bombaAtmaSikligi = 5;
        private string kullaniciAdi;
        private string avatarYolu;
        private string? avatarPath;
        private string kullaniciYetki;

        public level2(string avatarYolu)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.avatarYolu = avatarYolu;
        }

        private void level2_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(avatarYolu) && File.Exists(avatarYolu))
            {
                pictureBox1.Image = Image.FromFile(avatarYolu);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                pictureBox5.Image = Image.FromFile(avatarYolu);
                pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            }

            sirin.Can = 3;
            sirin.Hiz = 10;

            gargamel.Can = 3;
            gargamel.X = pictureBox2.Left;
            gargamel.Y = pictureBox2.Top;

            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox11.Visible = false;

            label1.Text = "Süre: 60";

            timer1.Interval = 100;
            timer1.Start();

            KalpGuncelle();
            GargamelKalpGuncelle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Left += hareketYon * gargamelHizi;
            if (pictureBox2.Left <= 0 || pictureBox2.Right >= this.ClientSize.Width)
                hareketYon *= -1;

            if (!pictureBox3.Visible && rnd.Next(0, 100) < bombaAtmaSikligi)
                gargamel.BombaAt(pictureBox3, pictureBox2);

            if (pictureBox3.Visible)
            {
                pictureBox3.Top += 15;

                if (pictureBox3.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    sirin.Can--;
                    pictureBox3.Visible = false;
                    KalpGuncelle();

                    if (sirin.Can <= 0)
                    {
                        timer1.Stop();
                        MessageBox.Show("Şirin bayıldı, oyun bitti!");
                    }
                }
                else if (pictureBox3.Top > this.Height)
                {
                    pictureBox3.Visible = false;
                }
            }

            sure--;
            label1.Text = "Süre: " + (sure / 10);

            if (sure <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre doldu!");
                return;
            }

            if (pictureBox4.Visible)
            {
                pictureBox4.Top -= 10;

                if (pictureBox4.Bounds.IntersectsWith(pictureBox2.Bounds))
                {
                    gargamel.Can--;
                    pictureBox4.Visible = false;
                    GargamelKalpGuncelle();

                    if (gargamel.Can <= 0)
                    {
                        timer1.Stop();
                        MessageBox.Show("Tebrikler! Gargamel'i yendin!");
                    }

                }
                else if (pictureBox4.Bounds.IntersectsWith(pictureBox11.Bounds) && pictureBox11.Visible)
                {
                    if (sirin.Can < 3)
                        sirin.Can++;

                    pictureBox11.Visible = false;
                    pictureBox4.Visible = false;
                    KalpGuncelle();
                }
                else if (pictureBox4.Top < 0)
                {
                    pictureBox4.Visible = false;
                }
            }

            if (!pictureBox11.Visible && rnd.Next(0, 100) < 2)
            {
                pictureBox11.Left = rnd.Next(50, this.ClientSize.Width - pictureBox11.Width - 50);
                pictureBox11.Top = rnd.Next(50, this.ClientSize.Height / 2);
                pictureBox11.Visible = true;
            }
        }

        private void level2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                sirin.SagaGit(pictureBox1, this);
            else if (e.KeyCode == Keys.Left)
                sirin.SolaGit(pictureBox1);
            else if (e.KeyCode == Keys.Space && !pictureBox4.Visible)
            {
                pictureBox4.Left = pictureBox1.Left + pictureBox1.Width / 2 - pictureBox4.Width / 2;
                pictureBox4.Top = pictureBox1.Top - pictureBox4.Height;
                pictureBox4.Visible = true;
            }
        }

        private void KalpGuncelle()
        {
            kalp1.Image = sirin.Can >= 1 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
            kalp2.Image = sirin.Can >= 2 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
            kalp3.Image = sirin.Can >= 3 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
        }

        private void GargamelKalpGuncelle()
        {
            pictureBox10.Image = gargamel.Can >= 1 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
            pictureBox9.Image = gargamel.Can >= 2 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
            pictureBox8.Image = gargamel.Can >= 3 ? Properties.Resources.kalpDolu : Properties.Resources.kalpBos;
        }

        private void SkorKaydet(int sure)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=nesne;Integrated Security=True;"))
            {
                con.Open();
                string sorgu = "INSERT INTO Skor (KullaniciAdi, Sure) VALUES (@ad, @sure)";
                using (SqlCommand cmd = new SqlCommand(sorgu, con))
                {
                    cmd.Parameters.AddWithValue("@ad", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@sure", sure);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
