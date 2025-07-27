using System;
using System.Drawing;
using System.Windows.Forms;

namespace nesne
{
    public partial class level1 : Form
    {
        Oyuncu sirin = new Oyuncu();
        dusman gargamel = new dusman();
        Random rnd = new Random();
        System.Windows.Forms.Timer bombaTimer = new System.Windows.Forms.Timer();

        int bombaGosterimSuresi = 10;
        int bombaBeklemeSuresi = 0;
        int sure = 600;

        private string kullaniciAdi;
        private string avatarYolu;
        private string kullaniciYetki;
        private string? avatarPath;

        public level1(string avatarYolu)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.avatarYolu = avatarYolu;
        }

        private void level1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += level1_KeyDown;

            if (!string.IsNullOrEmpty(avatarYolu) && File.Exists(avatarYolu))
            {
                pictureBox1.Image = Image.FromFile(avatarYolu);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                pictureBox7.Image = Image.FromFile(avatarYolu);
                pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            }

            sirin.Can = 3;
            sirin.Hiz = 10;

            gargamel.Can = 3;
            gargamel.X = pictureBox2.Left;
            gargamel.Y = pictureBox2.Top;

            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox6.Visible = false;

            bombaTimer.Interval = 100;
            bombaTimer.Tick += BombaTimer_Tick;
            bombaTimer.Start();

            this.ActiveControl = null;
            this.Focus();

            timer1.Interval = 100;
            timer1.Start();
            this.ActiveControl = null;
            this.Focus();

            KalpGuncelle();
            GargamelKalpGuncelle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sure--;
            label1.Text = "Süre: " + (sure / 10);

            if (sure <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre doldu!");
                return;
            }

            // Gargamel bomba atma
            if (!pictureBox3.Visible && rnd.Next(0, 100) < 15)
            {
                gargamel.BombaAt(pictureBox3, pictureBox2);
            }

            if (pictureBox3.Visible)
            {
                pictureBox3.Top += 10;

                if (pictureBox3.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    sirin.Can--;
                    pictureBox3.Visible = false;
                    KalpGuncelle();

                    if (sirin.Can <= 0)
                    {
                        timer1.Stop();
                        MessageBox.Show("Game Over! Şirin bayıldı.");

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            ekran frm = new ekran(kullaniciAdi, avatarYolu, kullaniciYetki);
                            frm.Show();
                            this.Hide();
                        });

                        return;
                    }
                }
                else if (pictureBox3.Top > this.Height)
                {
                    pictureBox3.Visible = false;
                }
            }

            // Şirin iksir hareketi
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
                        MessageBox.Show("Tebrikler! Gargamel’i yendin!");

                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            level2 level = new level2(this.avatarYolu);
                            level.Show();
                            this.Hide();
                        });

                        return;
                    }
                }
                else if (pictureBox4.Top < 0)
                {
                    pictureBox4.Visible = false;
                }
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ekran frm = new ekran(kullaniciAdi, avatarYolu, kullaniciYetki);
            frm.Show();
            this.Hide();
        }

        private void BombaTimer_Tick(object sender, EventArgs e)
        {
            // Eğer şu an bomba çıkmadı ve bekleme süresi bitti ise → yeni bomba çıkarma şansı
            if (!pictureBox6.Visible && bombaBeklemeSuresi <= 0 && rnd.Next(0, 100) < 5)
            {
                int maxX = this.ClientSize.Width - pictureBox6.Width;
                int maxY = this.ClientSize.Height - pictureBox6.Height;

                pictureBox6.Left = rnd.Next(0, maxX);
                pictureBox6.Top = rnd.Next(0, maxY);
                pictureBox6.Visible = true;

                bombaGosterimSuresi = 30; // bomba 3 saniye görünsün
            }

            // Eğer bomba görünüyorsa:
            if (pictureBox6.Visible)
            {
                bombaGosterimSuresi--;

                // Bomba Şirin'e çarptı mı?
                if (pictureBox6.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    sure -= 200; // 200 / 10 = 20 saniye azalt
                    if (sure < 0) sure = 0;

                    label1.Text = "Süre: " + (sure / 10);

                    pictureBox6.Visible = false;
                    bombaBeklemeSuresi = 10; // bomba çarptıktan sonra 2 saniye bekleme

                    MessageBox.Show("💣 Bomba patladı! Süreden 20 saniye kaybettin!");
                }

                // Bomba süresi bitti mi? Otomatik kaybolsun
                if (bombaGosterimSuresi <= 0)
                {
                    pictureBox6.Visible = false;
                    bombaBeklemeSuresi = 10; // bomba kaybolduktan sonra da biraz beklesin
                }
            }
            else
            {
                if (bombaBeklemeSuresi > 0)
                    bombaBeklemeSuresi--;
            }
        }

        private void level1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Right)
            {
                sirin.SagaGit(pictureBox1, this);
            }
            else if (e.KeyCode == Keys.Left)
            {
                sirin.SolaGit(pictureBox1);
            }
            else if (e.KeyCode == Keys.Space && !pictureBox4.Visible)
            {
                pictureBox4.Left = pictureBox1.Left + pictureBox1.Width / 2 - pictureBox4.Width / 2;
                pictureBox4.Top = pictureBox1.Top - pictureBox4.Height;
                pictureBox4.Visible = true;
            }
        }
    }
}
