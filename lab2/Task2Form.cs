using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Task2Form : Form
    {
        private Bitmap originalImage;
        private PictureBox pictureBoxOriginal;
        private PictureBox pictureBoxRed;
        private PictureBox pictureBoxGreen;
        private PictureBox pictureBoxBlue;
        private Button btnLoadImage;
        private Button btnExtractChannels;
        private Button btnShowHistograms;
        private Label labelOriginal;
        private Label labelRed;
        private Label labelGreen;
        private Label labelBlue;

        public Task2Form()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(1000, 450);
            this.Text = "Задание 2 - Выделение каналов RGB и гистограммы";

            btnLoadImage = new Button
            {
                Text = "Загрузить изображение",
                Location = new Point(10, 10),
                Size = new Size(150, 30)
            };
            btnLoadImage.Click += BtnLoadImage_Click;

            btnExtractChannels = new Button
            {
                Text = "Выделить каналы",
                Location = new Point(170, 10),
                Size = new Size(150, 30),
                Enabled = false
            };
            btnExtractChannels.Click += BtnExtractChannels_Click;

            btnShowHistograms = new Button
            {
                Text = "Показать гистограммы",
                Location = new Point(330, 10),
                Size = new Size(150, 30),
                Enabled = false
            };
            btnShowHistograms.Click += BtnShowHistograms_Click;

            labelOriginal = new Label
            {
                Text = "Оригинал",
                Location = new Point(10, 360),
                AutoSize = true,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            labelRed = new Label
            {
                Text = "Красный канал",
                Location = new Point(320, 260),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };
            labelGreen = new Label
            {
                Text = "Зеленый канал",
                Location = new Point(530, 260),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };
            labelBlue = new Label
            {
                Text = "Синий канал",
                Location = new Point(740, 260),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Blue
            };

            // области для оригинального изображения, красного, зеленого и синего
            pictureBoxOriginal = new PictureBox
            {
                Location = new Point(10, 50),
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            pictureBoxRed = new PictureBox
            {
                Location = new Point(320, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            pictureBoxGreen = new PictureBox
            {
                Location = new Point(530, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            pictureBoxBlue = new PictureBox
            {
                Location = new Point(740, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            this.Controls.AddRange(new Control[] {
                btnLoadImage, btnExtractChannels, btnShowHistograms,
                pictureBoxOriginal, pictureBoxRed, pictureBoxGreen, pictureBoxBlue,
                labelOriginal, labelRed, labelGreen, labelBlue
            });
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        originalImage = new Bitmap(openFileDialog.FileName);
                        pictureBoxOriginal.Image = originalImage;
                        btnExtractChannels.Enabled = true;
                        btnShowHistograms.Enabled = false;

                        pictureBoxRed.Image = null;
                        pictureBoxGreen.Image = null;
                        pictureBoxBlue.Image = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                    }
                }
            }
        }

        private void BtnExtractChannels_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;

            try
            {
                Bitmap redChannel = ExtractChannel(originalImage, Channel.Red);
                Bitmap greenChannel = ExtractChannel(originalImage, Channel.Green);
                Bitmap blueChannel = ExtractChannel(originalImage, Channel.Blue);

                pictureBoxRed.Image = redChannel;
                pictureBoxGreen.Image = greenChannel;
                pictureBoxBlue.Image = blueChannel;

                btnShowHistograms.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выделения каналов: {ex.Message}");
            }
        }

        private void BtnShowHistograms_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;

            try
            {
                ShowHistograms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка построения гистограмм: {ex.Message}");
            }
        }

        private Bitmap ExtractChannel(Bitmap source, Channel channel)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    Color pixel = source.GetPixel(x, y);
                    Color resultColor;

                    switch (channel)
                    {
                        case Channel.Red:
                            resultColor = Color.FromArgb(pixel.R, 0, 0); // Только красный
                            break;
                        case Channel.Green:
                            resultColor = Color.FromArgb(0, pixel.G, 0); // Только зеленый
                            break;
                        case Channel.Blue:
                            resultColor = Color.FromArgb(0, 0, pixel.B); // Только синий
                            break;
                        default:
                            resultColor = Color.Black;
                            break;
                    }

                    result.SetPixel(x, y, resultColor);
                }
            }

            return result;
        }

        private void ShowHistograms()
        {
            int[] redHistogram = new int[256];
            int[] greenHistogram = new int[256];
            int[] blueHistogram = new int[256];

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixel = originalImage.GetPixel(x, y);
                    redHistogram[pixel.R]++;
                    greenHistogram[pixel.G]++;
                    blueHistogram[pixel.B]++;
                }
            }

            HistogramForm histogramForm = new HistogramForm(redHistogram, greenHistogram, blueHistogram);
            histogramForm.Show();
        }

        private enum Channel { Red, Green, Blue }
    }

    public class HistogramForm : Form
    {
        private int[] redHistogram;
        private int[] greenHistogram;
        private int[] blueHistogram;

        public HistogramForm(int[] red, int[] green, int[] blue)
        {
            redHistogram = red;
            greenHistogram = green;
            blueHistogram = blue;

            this.Text = "Гистограммы RGB";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Paint += HistogramForm_Paint;
        }

        private void HistogramForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = this.ClientSize.Width - 60;
            int height = (this.ClientSize.Height - 180) / 3;
            int margin = 30;

            int maxRed = redHistogram.Max();
            int maxGreen = greenHistogram.Max();
            int maxBlue = blueHistogram.Max();
            int maxOverall = Math.Max(maxRed, Math.Max(maxGreen, maxBlue));

            DrawHistogram(g, redHistogram, Color.Red, maxOverall,
                new Rectangle(30, margin + 40, width, height), "Красный канал");

            DrawHistogram(g, greenHistogram, Color.Green, maxOverall,
                new Rectangle(30, margin + height + margin + 40, width, height), "Зеленый канал");

            DrawHistogram(g, blueHistogram, Color.Blue, maxOverall,
                new Rectangle(30, margin + (height + margin) * 2 + 40, width, height), "Синий канал");
        }

        private void DrawHistogram(Graphics g, int[] histogram, Color color, int maxValue,
                                 Rectangle area, string title)
        {
            using (Pen pen = new Pen(color))
            using (Brush brush = new SolidBrush(color))
            using (Font font = new Font("Arial", 10))
            {
                g.DrawString(title, font, Brushes.Black, area.Left, area.Top - 20);

                float barWidth = (float)area.Width / 256;
                for (int i = 0; i < 256; i++)
                {
                    float barHeight = (float)histogram[i] / maxValue * area.Height;
                    float x = area.Left + i * barWidth;
                    float y = area.Bottom - barHeight;

                    g.DrawRectangle(pen, x, y, barWidth, barHeight);
                    g.FillRectangle(brush, x, y, barWidth, barHeight);
                }

                g.DrawLine(Pens.Black, area.Left, area.Bottom, area.Right, area.Bottom);
                g.DrawLine(Pens.Black, area.Left, area.Bottom, area.Left, area.Top);
            }
        }
    }
}