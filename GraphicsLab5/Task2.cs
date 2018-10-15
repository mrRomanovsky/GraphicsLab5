using System;
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
    public partial class Task2 : Form
    {
        Point firstPoint;
        Point endPoint;
        SortedList<int, Point> listPoints;
        double roughness;
        bool firstIteration = true;
        bool lastIteration = false;
        Graphics g;
        Random rand;

        public Task2()
        {
            InitializeComponent();
            listPoints = new SortedList<int, Point>();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            rand = new Random();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lastIteration)
                return;
            roughness = double.Parse(textBox5.Text);
            if (firstIteration)
            {
                firstPoint = new Point(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                endPoint = new Point(int.Parse(textBox3.Text), int.Parse(textBox4.Text));
                listPoints.Add(firstPoint.X, firstPoint);
                listPoints.Add(endPoint.X, endPoint);
                firstIteration = false;
                g.DrawLine(new Pen(Color.Black), firstPoint, endPoint);
                pictureBox1.Refresh();
            }
            else
            {
                MidpointDisplacement();
                g.Clear(pictureBox1.BackColor);
                g.DrawLines(new Pen(Color.Black), listPoints.Select(x => x.Value).ToArray());
                pictureBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            firstIteration = true;
            lastIteration = false;
            listPoints.Clear();
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           if(firstIteration)
            {
                firstPoint = new Point(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                endPoint = new Point(int.Parse(textBox3.Text), int.Parse(textBox4.Text));
                listPoints.Add(firstPoint.X, firstPoint);
                listPoints.Add(endPoint.X, endPoint);
                firstIteration = false;
                roughness = double.Parse(textBox5.Text);
            }
            LastMidpointDisplacement();
            g.Clear(pictureBox1.BackColor);
            g.DrawLines(new Pen(Color.Black), listPoints.Select(x => x.Value).ToArray());
            pictureBox1.Refresh();
        }

        private void MidpointDisplacement()
        {
            lastIteration = true;
            var newPoints = new SortedList<int, Point>(listPoints);
            for (int i = 0; i < listPoints.Count()-1; i++)
            {
                if (Math.Abs(listPoints.ElementAt(i).Value.X - listPoints.ElementAt(i + 1).Value.X) == 1)
                    break;
                lastIteration = false;
                var l = Math.Abs(listPoints.ElementAt(i).Value.X + listPoints.ElementAt(i + 1).Value.X) / 2;
                var h = (listPoints.ElementAt(i).Value.Y + listPoints.ElementAt(i + 1).Value.Y) / 2 + rand.NextDouble() * (roughness * l - (-roughness * l)) + (-roughness * l);
                if (h >= pictureBox1.Height)
                    h = pictureBox1.Height - 1;
                if (h <= 0)
                    h = 1;
                newPoints.Add(l, new Point(l, (int)h));
            }
            listPoints = newPoints;
        }

        private void LastMidpointDisplacement()
        {
            while(!lastIteration)
            {
                MidpointDisplacement();
            }
        }
    }
}
