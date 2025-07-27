using System;
using System.Media;
using System.Windows.Forms;

namespace nesne
{
    public partial class VideoForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private SoundPlayer player;
        string avatarYolu;
        private string? avatarPath;

        public VideoForm(string avatarYolu)
        {
            InitializeComponent();
            this.avatarYolu = avatarPath;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += VideoForm_Load;
            this.avatarYolu = avatarYolu;
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            PlaySound();

            timer = new System.Windows.Forms.Timer
            {
                Interval = 10000 // 10 saniye
            };

            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                this.Hide();
                level1 level = new level1(this.avatarYolu);
                level.Show();
                this.Hide();
            };

            timer.Start();
        }

        private void PlaySound()
        {
            try
            {
                string soundPath = @"C:\Users\betul\OneDrive\Desktop\ödevler\nesne\nesne\bin\Debug\net8.0-windows\level1ses.wav";
                player = new SoundPlayer(soundPath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ses çalma hatası: " + ex.Message);
            }
        }
    }
}
