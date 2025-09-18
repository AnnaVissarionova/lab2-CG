using System;
using System.Windows.Forms;
using lab2;

public class MainMenu : Form
{
    public MainMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Лабораторная #2";
        Size = new Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;

        var btnTask1 = new Button { Text = "Задание 1", Location = new Point(100, 50), Size = new Size(200, 50) };
        var btnTask2 = new Button { Text = "Задание 2", Location = new Point(100, 120), Size = new Size(200, 50) };
        var btnTask3 = new Button { Text = "Задание 3", Location = new Point(100, 190), Size = new Size(200, 50) };

        btnTask1.Click += (s, e) => new Task1Form().Show();
        btnTask2.Click += (s, e) => new Task2Form().Show();
        btnTask3.Click += (s, e) => new Task3Form().Show();

        Controls.AddRange(new Control[] { btnTask1, btnTask2, btnTask3 });
    }
}