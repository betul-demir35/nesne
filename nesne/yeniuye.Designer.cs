namespace nesne
{
    partial class yeniuye
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(yeniuye));
            label1 = new Label();
            txtAd = new TextBox();
            txtSoyad = new TextBox();
            txtMail = new TextBox();
            txtSifre = new TextBox();
            pictureBoxAvatar = new PictureBox();
            txtSifreTekrar = new TextBox();
            txtKod = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            btnKodGonder = new PictureBox();
            label2 = new Label();
            label3 = new Label();
            label9 = new Label();
            label4 = new Label();
            label5 = new Label();
            pictureBox3 = new PictureBox();
            toolTip1 = new ToolTip(components);
            toolTip2 = new ToolTip(components);
            toolTip3 = new ToolTip(components);
            toolTip4 = new ToolTip(components);
            toolTip5 = new ToolTip(components);
            toolTip6 = new ToolTip(components);
            toolTip7 = new ToolTip(components);
            toolTip8 = new ToolTip(components);
            toolTip9 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnKodGonder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 18F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 162);
            label1.Location = new Point(276, 0);
            label1.Name = "label1";
            label1.Size = new Size(434, 34);
            label1.TabIndex = 1;
            label1.Text = "AİLEMİZE HOŞGELDİN ŞİRİN";
            // 
            // txtAd
            // 
            txtAd.BorderStyle = BorderStyle.FixedSingle;
            txtAd.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtAd.Location = new Point(428, 171);
            txtAd.Multiline = true;
            txtAd.Name = "txtAd";
            txtAd.Size = new Size(205, 38);
            txtAd.TabIndex = 5;
            txtAd.Text = "isim giriniz";
            // 
            // txtSoyad
            // 
            txtSoyad.BorderStyle = BorderStyle.FixedSingle;
            txtSoyad.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSoyad.Location = new Point(428, 215);
            txtSoyad.Multiline = true;
            txtSoyad.Name = "txtSoyad";
            txtSoyad.Size = new Size(205, 38);
            txtSoyad.TabIndex = 6;
            toolTip3.SetToolTip(txtSoyad, "soyisim giriniz");
            // 
            // txtMail
            // 
            txtMail.BorderStyle = BorderStyle.FixedSingle;
            txtMail.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtMail.Location = new Point(428, 259);
            txtMail.Multiline = true;
            txtMail.Name = "txtMail";
            txtMail.Size = new Size(205, 38);
            txtMail.TabIndex = 7;
            toolTip4.SetToolTip(txtMail, "g-mail giriniz");
            // 
            // txtSifre
            // 
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSifre.Location = new Point(287, 303);
            txtSifre.Multiline = true;
            txtSifre.Name = "txtSifre";
            txtSifre.PasswordChar = '*';
            txtSifre.Size = new Size(205, 38);
            txtSifre.TabIndex = 8;
            toolTip5.SetToolTip(txtSifre, "şifre giriniz");
            // 
            // pictureBoxAvatar
            // 
            pictureBoxAvatar.Location = new Point(457, 72);
            pictureBoxAvatar.Name = "pictureBoxAvatar";
            pictureBoxAvatar.Size = new Size(142, 82);
            pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAvatar.TabIndex = 11;
            pictureBoxAvatar.TabStop = false;
            toolTip1.SetToolTip(pictureBoxAvatar, "avatar seçiniz");
            pictureBoxAvatar.Click += btnAvatarSec_Click;
            // 
            // txtSifreTekrar
            // 
            txtSifreTekrar.BorderStyle = BorderStyle.FixedSingle;
            txtSifreTekrar.Font = new Font("Tahoma", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtSifreTekrar.Location = new Point(590, 303);
            txtSifreTekrar.Multiline = true;
            txtSifreTekrar.Name = "txtSifreTekrar";
            txtSifreTekrar.PasswordChar = '*';
            txtSifreTekrar.Size = new Size(205, 38);
            txtSifreTekrar.TabIndex = 14;
            toolTip6.SetToolTip(txtSifreTekrar, "tekrar şifre giriniz");
            // 
            // txtKod
            // 
            txtKod.Location = new Point(447, 304);
            txtKod.Multiline = true;
            txtKod.Name = "txtKod";
            txtKod.Size = new Size(137, 37);
            txtKod.TabIndex = 16;
            txtKod.Text = "kodu giriniz";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.switch1;
            pictureBox1.Location = new Point(-5, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(41, 34);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 20;
            pictureBox1.TabStop = false;
            toolTip7.SetToolTip(pictureBox1, "çıkış");
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(42, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(38, 34);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 21;
            pictureBox2.TabStop = false;
            toolTip8.SetToolTip(pictureBox2, "geri dön");
            pictureBox2.Click += pictureBox2_Click;
            // 
            // btnKodGonder
            // 
            btnKodGonder.BackColor = Color.Transparent;
            btnKodGonder.Image = Properties.Resources.sign_up;
            btnKodGonder.Location = new Point(408, 324);
            btnKodGonder.Name = "btnKodGonder";
            btnKodGonder.Size = new Size(235, 109);
            btnKodGonder.SizeMode = PictureBoxSizeMode.Zoom;
            btnKodGonder.TabIndex = 22;
            btnKodGonder.TabStop = false;
            toolTip9.SetToolTip(btnKodGonder, "üye ol");
            btnKodGonder.Click += btnKodGonder_Click;
            // 
            // label2
            // 
            label2.BackColor = Color.WhiteSmoke;
            label2.Font = new Font("Times New Roman", 10F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label2.Location = new Point(219, 311);
            label2.Name = "label2";
            label2.Size = new Size(65, 26);
            label2.TabIndex = 23;
            label2.Text = "ŞİFRE";
            // 
            // label3
            // 
            label3.BackColor = Color.WhiteSmoke;
            label3.Font = new Font("Times New Roman", 10F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label3.Location = new Point(503, 302);
            label3.Name = "label3";
            label3.Size = new Size(85, 42);
            label3.TabIndex = 24;
            label3.Text = "ŞİFRE\r\nTEKRARI";
            // 
            // label9
            // 
            label9.BackColor = Color.WhiteSmoke;
            label9.Font = new Font("Times New Roman", 10F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label9.Location = new Point(348, 223);
            label9.Name = "label9";
            label9.Size = new Size(78, 24);
            label9.TabIndex = 25;
            label9.Text = "SOYİSİM";
            // 
            // label4
            // 
            label4.BackColor = Color.WhiteSmoke;
            label4.Font = new Font("Times New Roman", 10F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label4.Location = new Point(380, 182);
            label4.Name = "label4";
            label4.Size = new Size(46, 21);
            label4.TabIndex = 26;
            label4.Text = "İSİM";
            // 
            // label5
            // 
            label5.BackColor = Color.WhiteSmoke;
            label5.Font = new Font("Times New Roman", 10F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label5.Location = new Point(348, 265);
            label5.Name = "label5";
            label5.Size = new Size(78, 24);
            label5.TabIndex = 27;
            label5.Text = "G-MAİL";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = SystemColors.ButtonHighlight;
            pictureBox3.Image = Properties.Resources.gizle;
            pictureBox3.Location = new Point(812, 303);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(48, 32);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 28;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // yeniuye
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImage = Properties.Resources.yeniuye;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(917, 424);
            Controls.Add(pictureBox3);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label9);
            Controls.Add(txtSifre);
            Controls.Add(txtSifreTekrar);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(txtKod);
            Controls.Add(pictureBoxAvatar);
            Controls.Add(txtMail);
            Controls.Add(txtSoyad);
            Controls.Add(txtAd);
            Controls.Add(label1);
            Controls.Add(btnKodGonder);
            FormBorderStyle = FormBorderStyle.None;
            Name = "yeniuye";
            Text = "yeniuye";
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnKodGonder).EndInit();
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
        private PictureBox pictureBoxAvatar;
        private TextBox txtSifreTekrar;
        private TextBox txtKod;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox btnKodGonder;
        private Label label2;
        private Label label3;
        private Label label9;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox3;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip1;
        private ToolTip toolTip6;
        private ToolTip toolTip2;
        private ToolTip toolTip7;
        private ToolTip toolTip8;
        private ToolTip toolTip9;
    }
}