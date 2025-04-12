using System;
using System.Drawing;
using System.Drawing.Text;
using System.Media;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
namespace nesne
{ public partial class girisloading : Form
    {
        private System.Windows.Forms.Timer timer;
        private SoundPlayer player;
        private bool isSoundOn = true;
        public girisloading()
        {
            InitializeComponent();
            PlaySound(); 
         
            this.StartPosition = FormStartPosition.CenterScreen;

            PictureBox loadingGif = new PictureBox
            {
                Size = new Size(690, 485),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            try
            {
                loadingGif.Image = Image.FromFile(@"C:\Users\EXCALİBUR\Source\Repos\sirinler\sirinler\loading-thinking.gif");
            }catch { }
      
            this.Controls.Add(loadingGif);

            timer = new System.Windows.Forms.Timer
            {
                Interval = 5000
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.Hide();
                üye üye = new üye();
                üye.Show();

            };
            timer.Start();
        }

        private void PlaySound()
        {
            try
            {
                string soundPath = @"C:\Users\EXCALİBUR\Desktop\ödevler\nesne\nesne\load.wav";

                player = new SoundPlayer(soundPath);
                player.Play();
                isSoundOn = true;
            }  catch { }
            
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.Stop();
            isSoundOn = false;

            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            player.Play();
            isSoundOn = true;

            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }
    }
}