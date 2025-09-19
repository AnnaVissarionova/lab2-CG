using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace lab2
{
    public partial class Task3Form : Form
    {
        private Bitmap original_img;
        private Bitmap converted_img;

        private TrackBar hue_trackbar;
        private TrackBar saturation_trackbar;
        private TrackBar value_trackbar;

        private Label hue_label;
        private Label saturation_label;
        private Label value_label;
        private Label original_label;
        private Label modified_label;

        private Button load_btn;
        private Button save_btn;
        private Button reset_btn;

        private PictureBox original_picturebox;
        private PictureBox converted_picturebox;

     

        public Task3Form()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Task №3. RGB to HSV Converter";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateControls();
            ArrangeControls();
        }

        private void CreateControls()
        {

            load_btn = new Button
            {
                Text = "Загрузить изображение",
                Size = new Size(150, 30),
                Location = new Point(20, 20)
            };
            load_btn.Click += LoadButton_Click;

            save_btn = new Button
            {
                Text = "Сохранить результат",
                Size = new Size(150, 30),
                Location = new Point(20, 60),
                Enabled = false
            };
            save_btn.Click += SaveButton_Click;

            reset_btn = new Button
            {
                Text = "Сбросить настройки",
                Size = new Size(150, 30),
                Location = new Point(20, 100),
                Enabled = false
            };
            reset_btn.Click += ResetButton_Click;


            //hue
            hue_trackbar = new TrackBar
            {
                Minimum = -180,
                Maximum = 180,
                Value = 0,
                Size = new Size(200, 50),
                Location = new Point(20, 160),
                TickFrequency = 20
            };
            hue_trackbar.Scroll += TrackBar_Scroll;

            hue_label = new Label
            {
                Text = "Оттенок (Hue): 0",
                Location = new Point(20, 140),
                AutoSize = true
            };


            //saturation
            saturation_trackbar = new TrackBar
            {
                Minimum = -100,
                Maximum = 100,
                Value = 0,
                Size = new Size(200, 50),
                Location = new Point(20, 240),
                TickFrequency = 10
            };
            saturation_trackbar.Scroll += TrackBar_Scroll;

            saturation_label = new Label
            {
                Text = "Насыщенность (Saturation): 0%",
                Location = new Point(20, 220),
                AutoSize = true
            };


            //value
            value_trackbar = new TrackBar
            {
                Minimum = -100,
                Maximum = 100,
                Value = 0,
                Size = new Size(200, 50),
                Location = new Point(20, 320),
                TickFrequency = 10
            };
            value_trackbar.Scroll += TrackBar_Scroll;

            value_label = new Label
            {
                Text = "Яркость (Value): 0%",
                Location = new Point(20, 300),
                AutoSize = true
            };



            //обласи для изображений
            original_picturebox = new PictureBox
            {
                Size = new Size(300, 250),
                Location = new Point(250, 50),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            converted_picturebox = new PictureBox
            {
                Size = new Size(300, 250),
                Location = new Point(600, 50),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // метки для изображений
            original_label = new Label
            {
                Text = "Оригинальное изображение",
                Location = new Point(250, 20),
                AutoSize = true
            };

            modified_label = new Label
            {
                Text = "Результат преобразования",
                Location = new Point(600, 20),
                AutoSize = true
            };
        }

        private void ArrangeControls()
        {
            // добавление элементов на форму
            this.Controls.Add(load_btn);
            this.Controls.Add(save_btn);
            this.Controls.Add(reset_btn);

            this.Controls.Add(hue_trackbar);
            this.Controls.Add(hue_label);

            this.Controls.Add(saturation_trackbar);
            this.Controls.Add(saturation_label);

            this.Controls.Add(value_trackbar);
            this.Controls.Add(value_label);

            this.Controls.Add(original_picturebox);
            this.Controls.Add(converted_picturebox);

            this.Controls.Add(original_label);
            this.Controls.Add(modified_label);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.bmp";
                openFileDialog.Title = "Выберите изображение";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        original_img = new Bitmap(openFileDialog.FileName);
                        converted_img = new Bitmap(original_img);

                        original_picturebox.Image = original_img;
                        converted_picturebox.Image = converted_img;

                        save_btn.Enabled = true;
                        reset_btn.Enabled = true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (converted_img == null)
            {
                MessageBox.Show("Нет изображения для сохранения", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JPEG Image|*.jpg|BMP Image|*.bmp";
                saveFileDialog.Title = "Сохранить изображение";
                saveFileDialog.DefaultExt = "jpg";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ImageFormat format = ImageFormat.Jpeg;
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                format = ImageFormat.Jpeg;
                                break;
                            case 2:
                                format = ImageFormat.Bmp;
                                break;
                        }

                        converted_img.Save(saveFileDialog.FileName, format);
                        MessageBox.Show("Изображение успешно сохранено!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            hue_trackbar.Value = 0;
            saturation_trackbar.Value = 0;
            value_trackbar.Value = 0;

        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {

        }

        

    }
}