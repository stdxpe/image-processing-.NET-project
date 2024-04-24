using image_process_demo5.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography.X509Certificates;

namespace image_process_demo5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //emptyPanel.BringToFront();

            //Dosya Aç butonuna tıklanırsa seçilen görüntü tempImage'e yüklenecek.
            //Ama dosya seçilmemesi halinde standart background olarak burada tanımladım.
            tempImage1 = Resources.bg;
            tempImage2 = Resources.bg;


            //Kamera açma eylemi için gerekli instance alma gereksinimleri
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cboCamera.Items.Add(filterInfo.Name);
                cboCamera2.Items.Add(filterInfo.Name);
            }

            cboCamera.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();
        }


        //Global Değişkenler
        Bitmap girisResmi, cikisResmi;
        Color okunanRenk, donusenRenk;
        int R = 0, G = 0, B = 0;
        Image tempImage1 = null;
        Image tempImage2 = null;


        //Windows Barı Yerine X ve - Buttonları
        private void x_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void min_Button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        //Windows Barı Yerine Sürükleme Özelliği
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        private void dragBaby_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void dragBaby_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void dragBaby_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }


        //FullScreen Buttonu
        public static Image pbxTasima = null;
        private void fullScreenButton_Click(object sender, EventArgs e)
        {
            pbxTasima = pbxNew.Image;

            Form2 form2 = new Form2();
            form2.Show();

            Form3 form3 = new Form3();
            form3.ShowDialog();
        }


        //--------------------------------------------MAIN BUTTONLAR--------------------------------------------\\
        //Dosya Açma Buttonu
        private void dosyaAcButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (videoCaptureDevice.IsRunning) videoCaptureDevice.Stop();

            try
            {
                openFileDialog1.DefaultExt = ".jpg";
                openFileDialog1.Filter = "Image Files(*.bmp;*.jpg;*.gif)|*.bmp;*.jpg;*.gif|All files (*.*)|*.*";
                openFileDialog1.ShowDialog();
                String resimLink = openFileDialog1.FileName;
                pbxOld.Image = System.Drawing.Image.FromFile(resimLink);
            }
            catch
            {
            }

            tempImage1 = pbxOld.Image;
        }


        //Dosya Açma Buttonu
        private void dosyaKaydetButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Jpeg Resmi|*.jpg|Bitmap Resmi|*.bmp|Gif Resmi|*.gif";
            saveFileDialog1.Title = "Resmi Kaydet";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "") //Dosya adı boş değilse kaydedecek.
            {
                // FileStream nesnesi ile kayıtı gerçekleştirecek.
                FileStream dosyaAkisi = (FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pbxNew.Image.Save(dosyaAkisi, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        pbxNew.Image.Save(dosyaAkisi, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        pbxNew.Image.Save(dosyaAkisi, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                dosyaAkisi.Close();
            }
        }


        //Main Button - Main Panel Saklama
        bool p1 = true;
        private void panelSaklaButton_Click(object sender, EventArgs e)
        {
            if (p1 == false)
            {
                flowLayoutPanel1.Visible = true;
                upButton.Visible = true;
                downButton.Visible = true;
                p1 = true;
            }
            else
            {
                flowLayoutPanel1.Visible = false;
                upButton.Visible = false;
                downButton.Visible = false;
                p1 = false;
            }
        }

        //Sola Aktar Buttonu
        private void solaAktarButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxNew.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R;
                    G = okunanRenk.G;
                    B = okunanRenk.B;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }

            pbxOld.Image = cikisResmi;
        }


        //Saga Aktar Buttonu
        private void sagaAktarButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R;
                    G = okunanRenk.G;
                    B = okunanRenk.B;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }

            pbxNew.Image = cikisResmi;
        }

        //MENU UP & DOWN Buttonları
        private void upButton_Click(object sender, EventArgs e)
        {
            int change = flowLayoutPanel1.VerticalScroll.Value - flowLayoutPanel1.VerticalScroll.SmallChange * 15;
            flowLayoutPanel1.AutoScrollPosition = new Point(0, change);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int change = flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.VerticalScroll.SmallChange * 15;
            flowLayoutPanel1.AutoScrollPosition = new Point(0, change);

            upButton.Visible = true;
        }


        //--------------------------------------------MAIN BUTTONLAR SON--------------------------------------------\\


        //-----------------------------------------PANEL HIDE'LAMALAR-----------------------------------------------\\


        //Parlaklık Button Hide
        private void parlaklikButton_Click(object sender, EventArgs e)
        {
            parlaklikPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Kontrast Button Hide

        private void kontrastButton_Click(object sender, EventArgs e)
        {
            kontrastPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Greyscale Button Hide
        private void griButton_Click(object sender, EventArgs e)
        {
            grayscalePanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Threshold Button Hide
        private void esikButton_Click(object sender, EventArgs e)
        {
            thresholdPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Negatif Button Hide
        private void negatifButton_Click(object sender, EventArgs e)
        {
            negativePanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Rotate Button Hide
        private void dondurButton_Click(object sender, EventArgs e)
        {
            donmePanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Histogram Button Hide
        private void histogramButton_Click(object sender, EventArgs e)
        {
            histogramPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //RGB Button Hide
        private void rgbButton_Click(object sender, EventArgs e)
        {
            rgbPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Night Vision Button Hide
        private void nightVisionButton_Click(object sender, EventArgs e)
        {
            geceGorusuPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Motion Focus Button Hide
        private void motionFocusButton_Click(object sender, EventArgs e)
        {
            hareketPanel.BringToFront();
            fullScreenButton.BringToFront();
        }

        //Background Button Hide
        private void backgroundButton_Click(object sender, EventArgs e)
        {
            backgroundPanel.BringToFront();
            fullScreenButton.BringToFront();
        }



        //Perspektif Button Hide
        bool p10 = false;
        private void perspectiveButton_Click(object sender, EventArgs e)
        {
            //tempImage1 = pbxOld.Image;
            //tempImage2 = pbxNew.Image;
            tempImage2 = Properties.Resources.bg;

            //perspektifPanel.BringToFront();
            //fullScreenButton.BringToFront();

            if (p10 == false)
            {
                perspektifPanel.Visible = true;
                perspektifPanel.BringToFront();
                fullScreenButton.BringToFront();
                p10 = true;
            }
            else
            {
                perspektifPanel.Visible = false;
                perspektifPanel.SendToBack();
                emptyPanel.BringToFront();
                p10 = false;
            }
        }


        //-----------------------------------------PANEL HIDE'LAMALAR SON-----------------------------------------------\\


        //-----------------------------------------BUTTON CLICK EVENTLERİ-----------------------------------------------\\


        //Bütün panellerdeki Sıfırla Buttonları (ortak event)
        private void mainSifirlaBtn_Click(object sender, EventArgs e)
        {
            pbxOld.Image = tempImage1;
            pbxNew.Image = tempImage1;

            histogramTbx.Clear();
            histogramLbx.Items.Clear();
        }

        //Parlaklık Button
        private void parlaklikUygulaBtn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R + parlaklikTrackBar.Value;
                    G = okunanRenk.G + parlaklikTrackBar.Value;
                    B = okunanRenk.B + parlaklikTrackBar.Value;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }

            pbxNew.Image = cikisResmi;
        }



        //Kontrast Button
        private void kontrastUygulaBtn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            double C_KontrastSeviyesi = kontrastTrackBar.Value;
            double F_KontrastFaktoru = (259 * (C_KontrastSeviyesi + 255)) / (255 * (259 - C_KontrastSeviyesi));

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R;
                    G = okunanRenk.G;
                    B = okunanRenk.B;

                    R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                    G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                    B = (int)((F_KontrastFaktoru * (B - 128)) + 128);

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }

            pbxNew.Image = cikisResmi;
        }

        //Gri Tonlama Button
        private void gritonlamaUygulaBtn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    int griDegeri = 0;

                    if (griRb1.Checked == true)
                    {
                        griDegeri = Convert.ToInt16(okunanRenk.R * 0.21 + okunanRenk.G * 0.71 + okunanRenk.B * 0.071);
                    }
                    else if (griRb2.Checked == true)
                    {
                        griDegeri = Convert.ToInt16(okunanRenk.R * 0.299 + okunanRenk.G * 0.587 + okunanRenk.B * 0.114);
                    }
                    else if (griRb3.Checked == true)
                    {
                        griDegeri = Convert.ToInt16(okunanRenk.R + okunanRenk.G + okunanRenk.B) / 3;
                    }

                    R = griDegeri;
                    G = griDegeri;
                    B = griDegeri;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }


        //Eşikleme Button
        private void esiklemeUygulaBtn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            int esiklemeDegeri = esiklemeTrackBar.Value;

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    if (okunanRenk.R >= esiklemeDegeri)
                        R = 255;
                    else
                        R = 0;

                    if (okunanRenk.G >= esiklemeDegeri)
                        G = 255;
                    else
                        G = 0;

                    if (okunanRenk.B >= esiklemeDegeri)
                        B = 255;
                    else
                        B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }

            pbxNew.Image = cikisResmi;
        }

        //Negatif Button
        private void negatifUygulaBtn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = 255 - okunanRenk.R;
                    G = 255 - okunanRenk.G;
                    B = 255 - okunanRenk.B;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }

        //Dönme Rotate Buttonu / timer click eventi / donmeTrackBar_Scroll / Döndürme Metodu
        bool play = false;
        private void donmeBaslatBtn_Click(object sender, EventArgs e)
        {
            //false durur. true çalışır.
            if (play == false)
            {
                timer1.Enabled = true;
                donmeBaslatBtn.Text = "Durdur";
                play = true;
            }
            else
            {
                timer1.Enabled = false;
                donmeBaslatBtn.Text = "Başlat";
                play = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbxNew.Refresh();
            DondurmeMethodu(donmeTrackBar.Value);
            donmeTrackBar.Value = donmeTrackBar.Value + 1;
            Invalidate();
        }

        private void donmeTrackBar_Scroll(object sender, EventArgs e)
        {
            pbxNew.Refresh();
            DondurmeMethodu(donmeTrackBar.Value);
        }

        private void DondurmeMethodu(double aci)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            double radyanAci = aci * 2 * Math.PI / 360;

            double x2 = 0;
            double y2 = 0;

            //hata burada. double olmalı bunlar
            int x0 = resimGenisligi / 2;
            int y0 = resimYuksekligi / 2;


            for (int x1 = 0; x1 < resimGenisligi; x1++)
            {
                for (int y1 = 0; y1 < resimYuksekligi; y1++)
                {
                    okunanRenk = girisResmi.GetPixel(x1, y1);

                    //Aliaslı Döndürme -Sağa Kaydırma
                    x2 = (x1 - x0) - Math.Tan(radyanAci / 2) * (y1 - y0) + x0;
                    y2 = (y1 - y0) + y0;

                    x2 = Convert.ToInt16(x2);
                    y2 = Convert.ToInt16(y2);

                    //Aliaslı Döndürme -Aşağı kaydırma
                    x2 = (x2 - x0) + x0;
                    y2 = Math.Sin(radyanAci) * (x2 - x0) + (y2 - y0) + y0;

                    x2 = Convert.ToInt16(x2);
                    y2 = Convert.ToInt16(y2);

                    //Aliaslı Döndürme -Sağa Kaydırma
                    x2 = (x2 - x0) - Math.Tan(radyanAci / 2) * (y2 - y0) + x0;
                    y2 = (y2 - y0) + y0;

                    x2 = Convert.ToInt16(x2);
                    y2 = Convert.ToInt16(y2);

                    if (x2 > 0 && x2 < resimGenisligi && y2 > 0 && y2 < resimYuksekligi)
                        cikisResmi.SetPixel((int)x2, (int)y2, okunanRenk);
                }
            }

            pbxNew.Image = cikisResmi;
        }

        //Histogram Buttonu / Histogram Metodu
        private void histogramBaslatBtn_Click(object sender, EventArgs e)
        {
            histogramMethodu();
        }

        private void histogramMethodu()
        {
            pbxNew.Image = null;
            ArrayList pikselDizisi = new ArrayList();
            int[] pikselSayilariDizisi = new int[256];
            int ortalamaRenk = 0;

            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    ortalamaRenk = (int)(okunanRenk.R + okunanRenk.G + okunanRenk.B) / 3;

                    pikselDizisi.Add(ortalamaRenk);
                }
            }

            for (int r = 0; r < 255; r++)
            {
                int pikselSayisi = 0;
                for (int s = 0; s < pikselDizisi.Count; s++)
                {
                    if (r == Convert.ToInt16(pikselDizisi[s]))
                        pikselSayisi++;
                }

                pikselSayilariDizisi[r] = pikselSayisi;
            }

            //listbox işlemi
            int renkMaxPikselSayisi = 0;
            for (int k = 0; k <= 255; k++)
            {
                histogramLbx.Items.Add("Renk Kodu: " + k + "  -  " + pikselSayilariDizisi[k] + " adet");
                if (pikselSayilariDizisi[k] > renkMaxPikselSayisi)
                {
                    renkMaxPikselSayisi = pikselSayilariDizisi[k];
                }
            }

            //histogram grafik çizme işlemi
            Graphics cizimAlani;
            Pen kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
            Pen kalem2 = new Pen(System.Drawing.Color.Red, 1);
            cizimAlani = pbxNew.CreateGraphics();
            pbxNew.Refresh();

            int grafikYuksekligi = 400; ///////////////////?????? ne bu rakam?
            double olcekY = renkMaxPikselSayisi / grafikYuksekligi;
            double olcekX = 1.6;
            for (int x = 0; x <= 255; x++)
            {
                cizimAlani.DrawLine(kalem1, (int)(20 + x * olcekX), grafikYuksekligi, (int)(20 + x * olcekX),
                    (grafikYuksekligi - (int)(pikselSayilariDizisi[x] / olcekY)));
                if (x % 50 == 0)
                {
                    cizimAlani.DrawLine(kalem2, (int)(20 + x * olcekX), grafikYuksekligi, (int)(20 + x * olcekX), 0);
                }
            }
            histogramTbx.Text = "Max.Pixel: " + renkMaxPikselSayisi.ToString();
        }

        //RGB Buttonları

        //R Button
        private void R_RGB_Btn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    R = okunanRenk.R + RtrackBar.Value;
                    G = BtrackBar.Value;
                    B = GtrackBar.Value;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }

        //G Button
        private void G_RGB_Btn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    R = RtrackBar.Value;
                    G = okunanRenk.G + GtrackBar.Value;
                    B = BtrackBar.Value;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }

        //B Button
        private void B_RGB_Btn_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    R = RtrackBar.Value;
                    G = GtrackBar.Value;
                    B = okunanRenk.B + BtrackBar.Value;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }



        //Perspektif Button

        private void sifirlaButton_Click(object sender, EventArgs e)
        {
            XCord.Clear();
            YCord.Clear();

            pickXCord.Clear();
            pickYCord.Clear();

            boyutDonusturX(XCord).Clear();
            boyutDonusturY(YCord).Clear();

            boyutDonusturX(pickXCord).Clear();
            boyutDonusturY(pickYCord).Clear();



            pbxOld.Image = tempImage1;
            pbxNew.Image = tempImage2;
            //pbxNew.Image = Properties.Resources.bg;
            //pbxNew.Image = Resources.bg;

            //Application.Restart();
            //Environment.Exit(0);
        }

        private void cizgiButton_Click(object sender, EventArgs e)
        {
            //0-1-2-3
            for (int i = 0; i <= XCord.Count - 1; i++)
            {
                if (i < XCord.Count - 1)
                {
                    cizimAlani2.DrawLine(kalem3, XCord[i], YCord[i], XCord[i + 1], YCord[i + 1]);
                }
                else
                {
                    cizimAlani2.DrawLine(kalem3, XCord[i], YCord[i], XCord[i - i], YCord[i - i]);
                }
            }

            //kordinatları labellere yazdırmak

            //cordX0Lbl.Text = XCord[0].ToString();
            //cordX1Lbl.Text = XCord[1].ToString();
            //cordX2Lbl.Text = XCord[2].ToString();
            //cordX3Lbl.Text = XCord[3].ToString();

            //cordY0Lbl.Text = YCord[0].ToString();
            //cordY1Lbl.Text = YCord[1].ToString();
            //cordY2Lbl.Text = YCord[2].ToString();
            //cordY3Lbl.Text = YCord[3].ToString();
        }


        private void pbxOld_MouseEnter(object sender, EventArgs e)
        {
            if (doesMouseEnter)
            {
                pbxOld.Cursor = Cursors.Cross;
                pbxRedBorder.Visible = true;
            }
        }

        private void pbxOld_MouseLeave(object sender, EventArgs e)
        {
            if (doesMouseEnter)
            {
                pbxOld.Cursor = Cursors.Arrow;
                pbxRedBorder.Visible = false;
            }
        }

        //Plaka Analiz ve Çizim Safhası

        List<int> XCord = new List<int>();
        List<int> YCord = new List<int>();

        List<int> pickXCord = new List<int>();
        List<int> pickYCord = new List<int>();
        private void pbxOld_MouseClick(object sender, MouseEventArgs e)
        {
            if (noktaSecimi == true)
            {
                XCord.Add(e.X);
                YCord.Add(e.Y);
                ArtiCiz(e.X, e.Y);
            }

            if (dikdortgenSec == true)
            {
                pickXCord.Add(e.X);
                pickYCord.Add(e.Y);
                MaviArtiCiz(e.X, e.Y);
                if (pickXCord.Count >= 2 && pickYCord.Count >= 2)
                {
                    dikdortgenYaratMethod();
                }
            }
        }


        bool doesMouseEnter;
        bool noktaSecimi = false; //false ise bitir yazar. true ise seç yazar. false seçim aktif demek. true aktif değil demek.
        private void selectPointButton_Click(object sender, EventArgs e)
        {
            if (noktaSecimi == false)
            {
                selectPointButton.Text = "Seçimi Bitir";
                noktaSecimi = true;
                doesMouseEnter = true;
            }
            else //secim == true)
            {
                selectPointButton.Text = "Noktaları Seç";
                doesMouseEnter = false;
                noktaSecimi = false;
            }
        }

        bool dikdortgenSec = false; //false bitir yazar, true ise seç yazar. false seçim aktif demek. true aktif değil demek.
        private void olusmaNoktalariButton_Click(object sender, EventArgs e)
        {
            if (dikdortgenSec == false)
            {
                olusmaNoktalariButton.Text = "Seçimi Bitir";
                dikdortgenSec = true;
                doesMouseEnter = true;
            }
            else //(dikdortgenSec == true)
            {
                olusmaNoktalariButton.Text = "Oluşacak Noktaları Seç";
                dikdortgenSec = false;
                doesMouseEnter = false;
            }
        }


        //İkisi de true iken (buttonlarda seç yazıyor iken) mavi ve kırmızı artı çiziliyor.
        //İkisi de false iken (buttonlarda bitir yazıyor iken) hiçbir şey çizilmiyor.

        Graphics cizimAlani2;
        Pen kalem3 = new Pen(Color.Yellow, 4);
        Pen kalem4 = new Pen(Color.Red, 5);
        Pen kalem5 = new Pen(Color.Blue, 3);

        private void pbxOld_Paint(object sender, PaintEventArgs e)
        {
            if (noktaSecimi == false || dikdortgenSec == false)
            {
                cizimAlani2 = pbxOld.CreateGraphics();
            }

            //if (dikdortgenSec == false)
            //{
            //    cizimAlani2 = pbxOld.CreateGraphics();
            //    //pbxOld.Refresh();
            //}
        }


        //Perpektif Düzeltme Buttonu
        private void goruntuIsleButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            List<int> fileCordX = new List<int>();
            fileCordX = boyutDonusturX(XCord);

            List<int> fileCordY = new List<int>();
            fileCordY = boyutDonusturY(YCord);

            double x1 = fileCordX[0], y1 = fileCordY[0];
            double x2 = fileCordX[1], y2 = fileCordY[1];
            double x3 = fileCordX[3], y3 = fileCordY[3];
            double x4 = fileCordX[2], y4 = fileCordY[2];


            //////plaka_analiz2.png'deki formül için minimum boyutu olan x ve y kenarını seçmeye çalışıyorum.
            //////işlenen yeni görüntü X ve Y ekseninde bulunan X ve Y noktasına kadar oluşacak.
            //////Y2-Y1 mi küçük? Y3-Y0 mı sorgusu

            ////double olusacakYDegeri;
            ////if ((fileCordY[2] - fileCordY[1]) < (fileCordY[3] - fileCordY[0]))
            ////{
            ////    olusacakYDegeri = fileCordY[2] - fileCordY[1];
            ////}
            ////else
            ////{
            ////    olusacakYDegeri = fileCordY[3] - fileCordY[0];
            ////}

            ////double olusacakXDegeri;
            ////if ((fileCordX[1] - fileCordX[0]) < (fileCordX[2] - fileCordX[3]))
            ////{
            ////    olusacakXDegeri = fileCordX[1] - fileCordX[0];
            ////}
            ////else
            ////{
            ////    olusacakXDegeri = fileCordX[2] - fileCordX[3];
            ////}


            List<int> tempCordX = new List<int>();
            List<int> tempCordY = new List<int>();

            tempCordX = boyutDonusturX(pickXCord);
            tempCordY = boyutDonusturY(pickYCord);

            double X1 = tempCordX[0], Y1 = tempCordY[0];
            double X2 = tempCordX[1], Y2 = tempCordY[0];
            double X3 = tempCordX[0], Y3 = tempCordY[1];
            double X4 = tempCordX[1], Y4 = tempCordY[1];



            //double X1 = 0, Y1 = 0;
            //double X2 = girisResmi.Width, Y2 = 0;
            //double X3 = 0, Y3 = olusacakYDegeri; 
            //double X4 = girisResmi.Width, Y4 = olusacakYDegeri; ;

            double[,] GirisMatrisi = new double[8, 8];

            // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }
            GirisMatrisi[0, 0] = x1;
            GirisMatrisi[0, 1] = y1;
            GirisMatrisi[0, 2] = 1;
            GirisMatrisi[0, 3] = 0;
            GirisMatrisi[0, 4] = 0;
            GirisMatrisi[0, 5] = 0;
            GirisMatrisi[0, 6] = -x1 * X1;
            GirisMatrisi[0, 7] = -y1 * X1;

            //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
            GirisMatrisi[1, 0] = 0;
            GirisMatrisi[1, 1] = 0;
            GirisMatrisi[1, 2] = 0;
            GirisMatrisi[1, 3] = x1;
            GirisMatrisi[1, 4] = y1;
            GirisMatrisi[1, 5] = 1;
            GirisMatrisi[1, 6] = -x1 * Y1;
            GirisMatrisi[1, 7] = -y1 * Y1;

            //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 }
            GirisMatrisi[2, 0] = x2;
            GirisMatrisi[2, 1] = y2;
            GirisMatrisi[2, 2] = 1;
            GirisMatrisi[2, 3] = 0;
            GirisMatrisi[2, 4] = 0;
            GirisMatrisi[2, 5] = 0;
            GirisMatrisi[2, 6] = -x2 * X2;
            GirisMatrisi[2, 7] = -y2 * X2;

            //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
            GirisMatrisi[3, 0] = 0;
            GirisMatrisi[3, 1] = 0;
            GirisMatrisi[3, 2] = 0;
            GirisMatrisi[3, 3] = x2;
            GirisMatrisi[3, 4] = y2;
            GirisMatrisi[3, 5] = 1;
            GirisMatrisi[3, 6] = -x2 * Y2;
            GirisMatrisi[3, 7] = -y2 * Y2;

            //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
            GirisMatrisi[4, 0] = x3;
            GirisMatrisi[4, 1] = y3;
            GirisMatrisi[4, 2] = 1;
            GirisMatrisi[4, 3] = 0;
            GirisMatrisi[4, 4] = 0;
            GirisMatrisi[4, 5] = 0;
            GirisMatrisi[4, 6] = -x3 * X3;
            GirisMatrisi[4, 7] = -y3 * X3;

            //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
            GirisMatrisi[5, 0] = 0;
            GirisMatrisi[5, 1] = 0;
            GirisMatrisi[5, 2] = 0;
            GirisMatrisi[5, 3] = x3;
            GirisMatrisi[5, 4] = y3;
            GirisMatrisi[5, 5] = 1;
            GirisMatrisi[5, 6] = -x3 * Y3;
            GirisMatrisi[5, 7] = -y3 * Y3;

            //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
            GirisMatrisi[6, 0] = x4;
            GirisMatrisi[6, 1] = y4;
            GirisMatrisi[6, 2] = 1;
            GirisMatrisi[6, 3] = 0;
            GirisMatrisi[6, 4] = 0;
            GirisMatrisi[6, 5] = 0;
            GirisMatrisi[6, 6] = -x4 * X4;
            GirisMatrisi[6, 7] = -y4 * X4;

            //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 }
            GirisMatrisi[7, 0] = 0;
            GirisMatrisi[7, 1] = 0;
            GirisMatrisi[7, 2] = 0;
            GirisMatrisi[7, 3] = x4;
            GirisMatrisi[7, 4] = y4;
            GirisMatrisi[7, 5] = 1;
            GirisMatrisi[7, 6] = -x4 * Y4;
            GirisMatrisi[7, 7] = -y4 * Y4;

            //---------------------------------------------------------------------------
            double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);
            //---------------------------------------------------------------------------


            //----------------------------------- A Dönüşüm Matrisi (3x3) ------------------
            double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0, a21 = 0, a22 = 0;
            for (int i = 0; i < 8; i++)
            {
                a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 + matrisBTersi[0, 2] * X2 +
                      matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 + matrisBTersi[0, 5] * Y3 +
                      matrisBTersi[0, 6] * X4 +
                      matrisBTersi[0, 7] * Y4;
                a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 + matrisBTersi[1, 2] * X2 +
                      matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 + matrisBTersi[1, 5] * Y3 +
                      matrisBTersi[1, 6] * X4 +
                      matrisBTersi[1, 7] * Y4;
                a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 + matrisBTersi[2, 2] * X2 +
                      matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 + matrisBTersi[2, 5] * Y3 +
                      matrisBTersi[2, 6] * X4 +
                      matrisBTersi[2, 7] * Y4;
                a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 + matrisBTersi[3, 2] * X2 +
                      matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 + matrisBTersi[3, 5] * Y3 +
                      matrisBTersi[3, 6] * X4 +
                      matrisBTersi[3, 7] * Y4;
                a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 + matrisBTersi[4, 2] * X2 +
                      matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 + matrisBTersi[4, 5] * Y3 +
                      matrisBTersi[4, 6] * X4 +
                      matrisBTersi[4, 7] * Y4;
                a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 + matrisBTersi[5, 2] * X2 +
                      matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 + matrisBTersi[5, 5] * Y3 +
                      matrisBTersi[5, 6] * X4 +
                      matrisBTersi[5, 7] * Y4;
                a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 + matrisBTersi[6, 2] * X2 +
                      matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 + matrisBTersi[6, 5] * Y3 +
                      matrisBTersi[6, 6] * X4 +
                      matrisBTersi[6, 7] * Y4;
                a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 + matrisBTersi[7, 2] * X2 +
                      matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 + matrisBTersi[7, 5] * Y3 +
                      matrisBTersi[7, 6] * X4 +
                      matrisBTersi[7, 7] * Y4;
                a22 = 1;
            }

            //------------------------- Perspektif düzeltme işlemi --------------------------------
            PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22, X1, Y1, X4, Y4);
        }

        //================== Perspektif düzeltme işlemi =================
        private void PerspektifDuzelt(double a00, double a01, double a02, double a10, double a11, double a12,
            double a20, double a21, double a22, double X1, double Y1, double X4, double Y4)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            //girisKordLbl.Text = "W=" + resimGenisligi + "; " + "H" + resimYuksekligi;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            double X, Y, Z;
            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);

                    Z = (a20 * x + a21 * y + 1);  //neden 1?
                    X = (a00 * x + a01 * y + a02) / Z;
                    Y = (a10 * x + a11 * y + a12) / Z;

                    if (X > X1 && X < X4 && Y > Y1 && Y < Y4) //Çapraz iki köşe kullanılarak dikdörtgen sınır belirlendi(X1, Y1) - (X4, Y4).Bu sınırın dışına çıkan pikseller resme kaydedilmeyecek.
                        cikisResmi.SetPixel((int)X, (int)Y, okunanRenk);
                }
                pbxNew.Image = cikisResmi;
                //cikisKordLbl.Text = "W=" + X4 + "; " + "H" + cikisResmi.Height;
            }
        }

        // MATRİS TERSİNİ ALMA---------------------
        private double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length)); //matris boyutu içindeki eleman sayısı olduğu için kare matrisde karekökü matris boyutu olur.
            double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın tersi alındığında bu matris içinde tutulacak.

            //--I Birim matrisin içeriğini dolduruyor
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }

            //--Matris Tersini alma işlemi---------

            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu.
                    }

                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }

                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }


        public void ArtiCiz(int X, int Y)
        {
            cizimAlani2.DrawLine(kalem4, X - 10, Y, X + 10, Y);
            cizimAlani2.DrawLine(kalem4, X, Y - 10, X, Y + 10);
        }

        public void MaviArtiCiz(int X, int Y)
        {
            cizimAlani2.DrawLine(kalem5, X - 3, Y, X + 3, Y);
            cizimAlani2.DrawLine(kalem5, X, Y - 3, X, Y + 3);
        }

        private void dikdortgenYaratMethod()
        {
            int olusacakGenislik = Convert.ToInt32(pickXCord[1] - pickXCord[0]);
            int olusacakYukseklik = Convert.ToInt32(pickYCord[1] - pickYCord[0]);

            cizimAlani2.DrawRectangle(kalem5, pickXCord[0], pickYCord[0], olusacakGenislik, olusacakYukseklik);
        }

        private List<int> boyutDonusturX(List<int> gelenListX)
        {
            List<int> gidenListX = new List<int>();
            foreach (int gelenX in gelenListX)
            {
                gidenListX.Add((girisResmi.Width * gelenX) / pbxOld.Width);
            }
            return gidenListX;
        }

        private List<int> boyutDonusturY(List<int> gelenListY)
        {
            List<int> gidenListY = new List<int>();
            foreach (int gelenY in gelenListY)
            {
                gidenListY.Add((girisResmi.Height * gelenY) / pbxOld.Height);
            }
            return gidenListY;
        }

        private void medianFilterButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            int sablonBoyutu = 3; //şablon boyutu 3 den büyük tek rakam olmalıdır (3,5,7 gibi).
            int elemanSayisi = sablonBoyutu * sablonBoyutu;
            int[] R = new int[elemanSayisi];
            int[] G = new int[elemanSayisi];
            int[] B = new int[elemanSayisi];
            int[] Gri = new int[elemanSayisi];
            int x, y, i, j;
            for (x = (sablonBoyutu - 1) / 2; x < resimGenisligi - (sablonBoyutu - 1) / 2; x++)
            {
                for (y = (sablonBoyutu - 1) / 2; y < resimYuksekligi - (sablonBoyutu - 1) / 2; y++)
                {
                    //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                    int k = 0;
                    for (i = -((sablonBoyutu - 1) / 2); i <= (sablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((sablonBoyutu - 1) / 2); j <= (sablonBoyutu - 1) / 2; j++)
                        {
                            okunanRenk = girisResmi.GetPixel(x + i, y + j);
                            R[k] = okunanRenk.R;
                            G[k] = okunanRenk.G;
                            B[k] = okunanRenk.B;
                            Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); //Gri ton formülü
                            k++;
                        }
                    }
                    //Gri tona göre sıralama yapıyor. Aynı anda üç rengide değiştiriyor.
                    int geciciSayi = 0;
                    for (i = 0; i < elemanSayisi; i++)
                    {
                        for (j = i + 1; j < elemanSayisi; j++)
                        {
                            if (Gri[j] < Gri[i])
                            {
                                geciciSayi = Gri[i];
                                Gri[i] = Gri[j];
                                Gri[j] = geciciSayi;
                                geciciSayi = R[i];
                                R[i] = R[j];
                                R[j] = geciciSayi;
                                geciciSayi = G[i];
                                G[i] = G[j];
                                G[j] = geciciSayi;
                                geciciSayi = B[i];
                                B[i] = B[j];
                                B[j] = geciciSayi;
                            }
                        }
                    }//Sıralama sonrası ortadaki değeri çıkış resminin piksel değeri olarak atıyor.
                    cikisResmi.SetPixel(x, y, Color.FromArgb(R[(elemanSayisi - 1) / 2], G[(elemanSayisi - 1) / 2], B[(elemanSayisi - 1) / 2]));
                }
            }
            pbxNew.Image = cikisResmi;
        }


        ////////////Kamera Açma\\\\\\\\\\\\


        VideoCaptureDevice videoCaptureDevice;
        FilterInfoCollection filterInfoCollection;
        bool cam = false; // cam false kapalı, cam true açık olacak.

        private void kameraAcButton_Click(object sender, EventArgs e)
        {
            //videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            //videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;

            //videoCaptureDevice.Start();

            //kameraAcMethod();

            if (cam == false)
            {
                kameraAcKapatMethod();

                kameraAcGeceBtn.Text = "Kamera Kapat";
                kameraAcHareketButton.Text = "Kamera Kapat";
                cam = true;

            }
            else if (cam == true)
            {
                kameraAcKapatMethod();

                kameraAcGeceBtn.Text = "Kamera Aç";
                kameraAcHareketButton.Text = "Kamera Aç";
                cam = false;

            }
        }

        private void kameraAcKapatMethod()
        {
            if (!videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;

                videoCaptureDevice.Start();
            }
            else if (videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }


        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pbxOld.Image = (Bitmap)eventArgs.Frame.Clone();
            //pbxNew.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }



        ///// Gece Görüş Button
        private void geceGorusUygulaButton_Click(object sender, EventArgs e)
        {
            timer3.Enabled = true;
            //geceGorusMethod();
        }

        int sayac2;
        Bitmap sonResim2;
        List<Bitmap> fotoList2 = new List<Bitmap>();

        private void timer3_Tick(object sender, EventArgs e)
        {
            pbxNew.Image = geceGorusMethod();
        }

        private Bitmap geceGorusMethod()
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x += 1)
            {
                for (int y = 0; y < resimYuksekligi; y += 1)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R + geceGorusTrackBar.Value;
                    //G = okunanRenk.G + 50 + geceGorusTrackBar.Value;
                    G = okunanRenk.G + geceGorusTrackBar.Value;
                    B = okunanRenk.B + geceGorusTrackBar.Value;

                    //R = okunanRenk.R;
                    //G = okunanRenk.G;
                    //B = okunanRenk.B;

                    //b) Aynı uygulamayı şu şekide değiştirin.Karanlık ortamda çıkan en büyük renk değerini 255 gelecek şekilde ve en
                    //küçük renk değerini de 100 gelecek şekilde Normalize ederek resmi yeniden düzenleyin.Buradaki 100 sayısını
                    //Sürgü(Trackbar) ile ayarlanabilir yapın.



                    //if (R > 255) R = 255;
                    //if (G > 255) G = 255;
                    //if (B > 255) B = 255;

                    //Sınırı aşan değerleri 255 ayarlama. Gri resim üzerinde işlem yapıldığı için Sadece R ye bakıldı.

                    int enBuyukDeger = 0, enKucukDeger = 0;
                    int ustSinir = 0, altSinir = 0;

                    if (R > enBuyukDeger)
                        enBuyukDeger = R;
                    if (G > enBuyukDeger)
                        enBuyukDeger = G;
                    if (B > enBuyukDeger)
                        enBuyukDeger = B;

                    if (R < enKucukDeger)
                        enKucukDeger = R;
                    if (G < enKucukDeger)
                        enKucukDeger = G;
                    if (B < enKucukDeger)
                        enKucukDeger = G;


                    if (enBuyukDeger > 255)
                        ustSinir = 255;
                    else
                        ustSinir = enBuyukDeger;


                    if (enKucukDeger < geceGorusTrackBar2.Value)
                        altSinir = geceGorusTrackBar2.Value;
                    else
                        altSinir = enKucukDeger;
                    //altSinir = geceGorusTrackBar2.Value;



                    int normalDegerR = (((ustSinir - altSinir) * (R - enKucukDeger)) / (enBuyukDeger - enKucukDeger)) + altSinir;
                    int normalDegerG = (((ustSinir - altSinir) * (G - enKucukDeger)) / (enBuyukDeger - enKucukDeger)) + altSinir;
                    int normalDegerB = (((ustSinir - altSinir) * (B - enKucukDeger)) / (enBuyukDeger - enKucukDeger)) + altSinir;

                    cikisResmi.SetPixel(x, y, Color.FromArgb(normalDegerR, normalDegerG, normalDegerB));

                }
            }
            return cikisResmi;

        }



        /////// Hareket Sensörü Button
        private void hareketSensoruUygulaBtn_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }

        int sayac = 0;
        List<Bitmap> fotoList = new List<Bitmap>();
        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac++;
            if (sayac <= 2)
            {
                fotoList.Add(pbxOld.Image as Bitmap);

                if (sayac % 2 != 0)
                {
                    Bitmap sonResim = fotoList[sayac - 1];
                }
                else
                {
                    Bitmap sonResim = fotoList[sayac - 2];

                    HareketAlgila(sonResim);
                    pbxNew.Image = pbxOld.Image;

                    Bitmap image = pbxOld.Image as Bitmap;

                    Bitmap tBitmap = pbxOld.Image as Bitmap;
                    Graphics g3 = Graphics.FromImage(tBitmap);

                    //buraya kadar ortak adımlar. pikselList'te farklı olan (hareket eden) pikseller var.

                    Graphics g = Graphics.FromImage(image);

                    if (hareketRb1.Checked == true)
                    {
                        foreach (Point point in pixelList)
                        {
                            g.DrawEllipse(new Pen(Color.Yellow), new Rectangle(point, new Size(5, 5)));
                        }
                    }
                    else if (hareketRb2.Checked == true)
                    {
                        int toplamXNoktalari = 0;
                        int toplamYNoktalari = 0;
                        foreach (Point point in pixelList)
                        {
                            toplamXNoktalari = toplamXNoktalari + point.X;
                            toplamYNoktalari = toplamYNoktalari + point.Y;
                        }

                        int ortaXNoktasi = toplamXNoktalari / (pixelList.Count + 1);
                        int ortaYNoktasi = toplamYNoktalari / (pixelList.Count + 1);

                        g.DrawEllipse(new Pen(Color.Red, 3f), ortaXNoktasi, ortaYNoktasi, 50, 50);
                    }
                    else if (hareketRb3.Checked == true)
                    {
                        pixelList.Add(new Point(-100, -100));
                        Point ilkEleman = pixelList[0];
                        Point sonEleman = pixelList[pixelList.Count - 1];

                        int width = Math.Abs(sonEleman.X - ilkEleman.X);
                        int height = Math.Abs(sonEleman.Y - ilkEleman.Y);


                        Rectangle dikdortgen = new Rectangle(ilkEleman, new Size(width, height));
                        Pen kalem = new Pen(Color.Red, 3f);
                        g.DrawRectangle(kalem, dikdortgen);



                        //Bitmap tBitmap = new Bitmap(zoomX, zoomY, PixelFormat.Format24bppRgb);  //temp bitmap
                        //Graphics g3 = Graphics.FromImage(tBitmap);

                        //tBitmap = new Bitmap(zoomX, zoomY, PixelFormat.Format24bppRgb);  //temp bitmap
                        //g3 = Graphics.FromImage(tBitmap);
                        //g3.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        //Draw the zoomed image using the DrawImage method(using the original image from the PictureBox). 
                        //g3.DrawImage(pbxNew.Image, picboxDikdortgen, dikdortgen, GraphicsUnit.Pixel);

                        //pbxNew.Image = tBitmap;
                        //g3.Dispose();
                        //pbxNew.Refresh();





                        //g.DrawImage(image, 0, 0, 100, 100);
                        //g.DrawImage(image, picboxDikdortgen, dikdortgen,units);

                        //kucult(3, image);

                        //Bitmap bitmap = new Bitmap(475, 300, g);


                        //g.DrawImage(image, 50, 50, dikdortgen, units);
                        //g.DrawImage(b

                        //g2.DrawImage(image, 0, 0);

                    }
                    else if (hareketRb4.Checked == true)
                    {
                        pixelList.Add(new Point(-100, -100));
                        Point ilkEleman = pixelList[0];
                        Point sonEleman = pixelList[pixelList.Count - 1];

                        int width = Math.Abs(sonEleman.X - ilkEleman.X);
                        int height = Math.Abs(sonEleman.Y - ilkEleman.Y);


                        Rectangle dikdortgen = new Rectangle(ilkEleman, new Size(width, height));
                        Pen kalem = new Pen(Color.Transparent, 3f);
                        g.DrawRectangle(kalem, dikdortgen);


                        Rectangle picboxDikdortgen = new Rectangle(100, 100, pbxNew.Width, pbxNew.Height);

                        GraphicsUnit units = GraphicsUnit.Pixel;

                        //Bitmap tBitmap = new Bitmap(zoomX, zoomY, PixelFormat.Format24bppRgb);  //temp bitmap
                        //Graphics g3 = Graphics.FromImage(tBitmap);

                        //tBitmap = new Bitmap(zoomX, zoomY, PixelFormat.Format24bppRgb);  //temp bitmap
                        //g3 = Graphics.FromImage(tBitmap);
                        //g3.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        //Draw the zoomed image using the DrawImage method(using the original image from the PictureBox). 
                        g3.DrawImage(pbxNew.Image, picboxDikdortgen, dikdortgen, GraphicsUnit.Pixel);

                        //pbxNew.Image = tBitmap;
                        //g3.Dispose();
                        //pbxNew.Refresh();





                        //g.DrawImage(image, 0, 0, 100, 100);
                        //g.DrawImage(image, picboxDikdortgen, dikdortgen,units);

                        //kucult(3, image);

                        //Bitmap bitmap = new Bitmap(475, 300, g);


                        //g.DrawImage(image, 50, 50, dikdortgen, units);
                        //g.DrawImage(b

                        //g2.DrawImage(image, 0, 0);
                    }
                    pbxNew.Image = image;
                }
            }
            else
            {
                sayac = 0;
                fotoList.Clear();
                pixelList.Clear();
            }
        }



        List<Point> pixelList = new List<Point>();

        private void HareketAlgila(Bitmap sonKare)
        {
            pixelList.Clear();
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;


            for (int x = 0; x < resimGenisligi; x += 5)
            {
                for (int y = 0; y < resimYuksekligi; y += 5)
                {
                    Color okunanRenk1 = girisResmi.GetPixel(x, y);
                    Color okunanRenk2 = sonKare.GetPixel(x, y);

                    R = Math.Abs(okunanRenk1.R - okunanRenk2.R);
                    G = Math.Abs(okunanRenk1.G - okunanRenk2.G);
                    B = Math.Abs(okunanRenk1.B - okunanRenk2.B);

                    if (R > 25 && G > 25 && B > 25)
                    {
                        pixelList.Add(new Point(x, y));
                    }
                }
            }
        }



        ///////Arkaplan Değiştir Button
        List<Point> farkliPixeller = new List<Point>();
        
        private void bgUygulaButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);
            Bitmap sonKare = pbxNew.Image as Bitmap;

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    Color okunanRenk1 = girisResmi.GetPixel(x, y);
                    Color okunanRenk2 = sonKare.GetPixel(x, y);

                    R = Math.Abs(okunanRenk1.R - okunanRenk2.R);
                    G = Math.Abs(okunanRenk1.G - okunanRenk2.G);
                    B = Math.Abs(okunanRenk1.B - okunanRenk2.B);

                    if (R > 25 && G > 25 && B > 25)
                    {
                        farkliPixeller.Add(new Point(x, y));   //farklı piksellerin konumunu kaydettik list'e
                    }

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;  //yaratılan görüntü önemsiz, sadece konumları aldık çünkü.
        }

        private void bgYerlestirButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            int resimGenisligi = girisResmi.Width;
            int resimYuksekligi = girisResmi.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk = girisResmi.GetPixel(x, y);
                    R = okunanRenk.R;
                    G = okunanRenk.G;
                    B = okunanRenk.B;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);

                    int X1 = farkliPixeller[0].X;
                    int Y1 = farkliPixeller[0].Y;

                    int X4 = farkliPixeller[farkliPixeller.Count - 1].X;
                    int Y4 = farkliPixeller[farkliPixeller.Count - 1].Y;

                    if (x > X1 && x < X4 && y > Y1 && y < Y4)
                        cikisResmi.SetPixel(x, y, donusenRenk);
                }
                pbxNew.Image = cikisResmi;
            }
        }

        Color okunanRenk1, okunanRenk2;
        private void bgToplaButton_Click(object sender, EventArgs e)
        {
            girisResmi = new Bitmap(pbxOld.Image);

            Bitmap resim1, resim2;
            resim1 = new Bitmap(pbxOld.Image);
            resim2 =new Bitmap(pbxNew.Image);

            int resimGenisligi = resim1.Width;
            int resimYuksekligi = resim1.Height;

            cikisResmi = new Bitmap(resimGenisligi, resimYuksekligi);

            for (int x = 0; x < resimGenisligi; x++)
            {
                for (int y = 0; y < resimYuksekligi; y++)
                {
                    okunanRenk1 = resim1.GetPixel(x, y);
                    okunanRenk2= resim2.GetPixel(x, y);

                    if (okunanRenk2.R > 5 && okunanRenk2.G > 5 && okunanRenk2.B > 5)
                    {
                        R = Convert.ToInt16(okunanRenk1.R * 0 + okunanRenk2.R * 1);
                        G = Convert.ToInt16(okunanRenk1.G * 0 + okunanRenk2.G * 1);
                        B = Convert.ToInt16(okunanRenk1.B * 0 + okunanRenk2.B * 1);
                    }
                    else
                    {
                        R = okunanRenk1.R;
                        G = okunanRenk1.G;
                        B = okunanRenk1.B;
                    }

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    donusenRenk = Color.FromArgb(R, G, B);
                    cikisResmi.SetPixel(x, y, donusenRenk);
                }
            }
            pbxNew.Image = cikisResmi;
        }
    }
}



