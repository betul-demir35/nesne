namespace nesne
{
    partial class SifreUnuttum
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SifreUnuttum));
            label1 = new Label();
            txtAd = new TextBox();
            txtSoyad = new TextBox();
            txtMail = new TextBox();
            txtSifre = new TextBox();
            txtSifreTekrar = new TextBox();
            txtKod = new TextBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            toolTip1 = new ToolTip(components);
            toolTip2 = new ToolTip(components);
            toolTip3 = new ToolTip(components);
            toolTip4 = new ToolTip(components);
            toolTip5 = new ToolTip(components);
            toolTip6 = new ToolTip(components);
            toolTip7 = new ToolTip(components);
            toolTip8 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 18F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 162);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(221, 2);
            label1.Name = "label1";
            label1.Size = new Size(328, 34);
            label1.TabIndex = 1;
            label1.Text = "UNUTKAN ŞİRİN SENİ";
            // 
            // txtAd
            // 
            txtAd.BorderStyle = BorderStyle.FixedSingle;
            txtAd.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtAd.Location = new Point(257, 129);
            txtAd.Multiline = true;
            txtAd.Name = "txtAd";
            txtAd.Size = new Size(205, 38);
            txtAd.TabIndex = 5;
            txtAd.Text = "isim";
            toolTip1.SetToolTip(txtAd, "isim giriniz");
            // 
            // txtSoyad
            // 
            txtSoyad.BorderStyle = BorderStyle.FixedSingle;
            txtSoyad.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSoyad.Location = new Point(257, 173);
            txtSoyad.Multiline = true;
            txtSoyad.Name = "txtSoyad";
            txtSoyad.Size = new Size(205, 38);
            txtSoyad.TabIndex = 6;
            txtSoyad.Text = "soyisim";
            toolTip2.SetToolTip(txtSoyad, "soyisim giriniz");
            // 
            // txtMail
            // 
            txtMail.BorderStyle = BorderStyle.FixedSingle;
            txtMail.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtMail.Location = new Point(257, 217);
            txtMail.Multiline = true;
            txtMail.Name = "txtMail";
            txtMail.Size = new Size(205, 38);
            txtMail.TabIndex = 7;
            txtMail.Text = "g-mail";
            // 
            // txtSifre
            // 
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSifre.Location = new Point(257, 173);
            txtSifre.Multiline = true;
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(205, 38);
            txtSifre.TabIndex = 8;
            txtSifre.Text = "şifre";
            toolTip4.SetToolTip(txtSifre, "şifre giriniz");
            // 
            // txtSifreTekrar
            // 
            txtSifreTekrar.BorderStyle = BorderStyle.FixedSingle;
            txtSifreTekrar.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSifreTekrar.Location = new Point(257, 217);
            txtSifreTekrar.Multiline = true;
            txtSifreTekrar.Name = "txtSifreTekrar";
            txtSifreTekrar.Size = new Size(205, 38);
            txtSifreTekrar.TabIndex = 9;
            txtSifreTekrar.Text = "şifre tekrarı";
            toolTip5.SetToolTip(txtSifreTekrar, "tekrar şifre giriniz");
            // 
            // txtKod
            // 
            txtKod.BorderStyle = BorderStyle.FixedSingle;
            txtKod.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtKod.Location = new Point(257, 173);
            txtKod.Multiline = true;
            txtKod.Name = "txtKod";
            txtKod.Size = new Size(205, 38);
            txtKod.TabIndex = 12;
            txtKod.Text = "kodu giriniz";
            toolTip3.SetToolTip(txtKod, "gelen kod");
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(46, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 34);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 23;
            pictureBox2.TabStop = false;
            toolTip8.SetToolTip(pictureBox2, "geri dön");
            pictureBox2.Click += pictureBox2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.switch1;
            pictureBox1.Location = new Point(-1, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 34);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 22;
            pictureBox1.TabStop = false;
            toolTip7.SetToolTip(pictureBox1, "çıkış");
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Image = Properties.Resources.user1;
            pictureBox3.Location = new Point(274, 262);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(169, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 24;
            pictureBox3.TabStop = false;
            toolTip6.SetToolTip(pictureBox3, "şifre yenile");
            pictureBox3.Click += button1_Click;
            // 
            // SifreUnuttum
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImage = Properties.Resources.First_Look_at_3D__The_Smurfs__Logo_Unveiled___Official_Zoom_Backgrounds;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(710, 398);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(txtSifreTekrar);
            Controls.Add(txtSifre);
            Controls.Add(txtMail);
            Controls.Add(txtSoyad);
            Controls.Add(txtAd);
            Controls.Add(label1);
            Controls.Add(txtKod);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SifreUnuttum";
            Text = "SifreUnuttum";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtAd;
        private TextBox txtSoyad;
        private TextBox txtMail;
        private TextBox txtSifre;
        private TextBox txtSifreTekrar;
        private TextBox txtKod;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip3;
        private ToolTip toolTip8;
        private ToolTip toolTip7;
        private ToolTip toolTip6;
    }
}