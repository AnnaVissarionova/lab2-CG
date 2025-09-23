using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab2
{
    public partial class Task1Form : Form
    {
        Bitmap original, gray1, gray2, diff;

        public Task1Form()
        {
            InitializeComponent();
            original = new Bitmap("C:\\Users\\HP\\Desktop\\lab2-CG\\lab2\\apple.jpg");
            this.Width = 1360;
            this.Height = 800;

            
            gray1 = ConvertToGray(original, 0.299, 0.587, 0.114);   //PAL/NTSC
            gray2 = ConvertToGray(original, 0.2126, 0.7152, 0.0722); // HDTV
            diff = Difference(gray1, gray2);

            
            Label lblOriginal = new Label(){Text = "Оригинал", AutoSize = true, Left = 10,  Top = 10 };
            PictureBox pb1 = new PictureBox() { Image = original, SizeMode = PictureBoxSizeMode.Zoom, Width = 300, Height = 250, Left = 10, Top = 20 };

            Label lblGray1 = new Label(){ Text = "NTSC/PAL (0.299R + 0.587G + 0.114B)", AutoSize = true,  Left = 350, Top = 10};
            PictureBox pb2 = new PictureBox() { Image = gray1, SizeMode = PictureBoxSizeMode.Zoom, Width = 300, Height = 250, Left = 350, Top = 20 };

            Label lblGray2 = new Label(){ Text = "HDTV (0.2126R + 0.7152G + 0.0722B)",  AutoSize = true, Left = 690, Top = 10 };
            PictureBox pb3 = new PictureBox() { Image = gray2, SizeMode = PictureBoxSizeMode.Zoom, Width = 300, Height = 250, Left = 690, Top = 20 };

            Label lblDiff = new Label() {Text = "Разность изображений", AutoSize = true, Left = 1030, Top = 10};
            PictureBox pb4 = new PictureBox() { Image = diff, SizeMode = PictureBoxSizeMode.Zoom, Width = 300, Height = 250, Left = 1030, Top = 20 };

            this.Controls.Add(lblOriginal);
            this.Controls.Add(pb1);

            this.Controls.Add(lblGray1);
            this.Controls.Add(pb2);

            this.Controls.Add(lblGray2);
            this.Controls.Add(pb3);

            this.Controls.Add(lblDiff);
            this.Controls.Add(pb4);


            Chart chart1 = CreateHistogram(gray1, "NTSC/PAL Grayscale", 160, 280);
            Chart chart2 = CreateHistogram(gray2, "HDTV Grayscale", 790, 280);

            this.Controls.Add(chart1);
            this.Controls.Add(chart2);

        }

        private Bitmap ConvertToGray(Bitmap img, double kr, double kg, double kb)
        {
            Bitmap result = new Bitmap(img.Width, img.Height);
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color c = img.GetPixel(x, y);
                    int gray = (int)(kr * c.R + kg * c.G + kb * c.B);
                    gray = Math.Min(255, Math.Max(0, gray));
                    result.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return result;
        }

        private Bitmap Difference(Bitmap img1, Bitmap img2)
        {
            Bitmap result = new Bitmap(img1.Width, img1.Height);
            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    int g1 = img1.GetPixel(x, y).R;
                    int g2 = img2.GetPixel(x, y).R;
                    int diff = Math.Abs(g1 - g2);
                    result.SetPixel(x, y, Color.FromArgb(diff, diff, diff));
                }
            }
            return result;
        }

        private Chart CreateHistogram(Bitmap img, string title, int left, int top)
        {
            int[] hist = new int[256];
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    int g = img.GetPixel(x, y).R;
                    hist[g]++;
                }
            }

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.Left = left; 
            chart.Top = top;

            ChartArea ca = new ChartArea();
            chart.ChartAreas.Add(ca);

            Series s = new Series();
            s.ChartType = SeriesChartType.Column;
            for (int i = 0; i < 256; i++)
                s.Points.AddXY(i, hist[i]);

            chart.Series.Add(s);
            chart.Titles.Add(title);

            return chart;
        }

        

        private void Task1Form_Load(object sender, EventArgs e)
        {

        }
    }
}
