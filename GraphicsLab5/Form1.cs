﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsLab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void task1Button_Click(object sender, EventArgs e)
        {
            var task1 = new task1();
            task1.ShowDialog();
        }

        private void task3Button_Click(object sender, EventArgs e)
        {
            var task3Form = new BezierCurves();
            task3Form.ShowDialog();
        }

        private void task2Button_Click(object sender, EventArgs e)
        {
            var task2 = new Task2();
            task2.ShowDialog();
        }
    }
}
