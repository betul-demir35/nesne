using nesne;

    public class Oyuncu : Karakter
    {
        public int Hiz { get; set; }

        public Oyuncu() : base() { }

        public Oyuncu(int can, int hiz) : base(can, 0, 0)
        {
            this.Hiz = hiz;
        }

        public void SagaGit(PictureBox p, Form f)
        {
            if (p.Left + Hiz + p.Width <= f.ClientSize.Width)
                p.Left += Hiz;
        }

        public void SolaGit(PictureBox p)
        {
            if (p.Left - Hiz >= 0)
                p.Left -= Hiz;
        }
    }

