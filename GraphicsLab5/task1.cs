using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsLab5
{
    public partial class task1 : Form
    {


        string atom, startingPosition;
        double rotationAngle;
        Dictionary<char, string> rules;
        List<Tuple<PointF, PointF>> states;
        Graphics g;

        public task1()
        {
            InitializeComponent();
            rules = new Dictionary<char, string>();
            states = new List<Tuple<PointF, PointF>>();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.ShowDialog();
            var fname = openDialog.FileName;

            rules = new Dictionary<char, string>();
            var flines = File.ReadAllLines(fname).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            var props = flines[0].Split(' ');

            atom = props[0];

            rotationAngle = (float.Parse(props[1])) * Math.PI / 180;

            startingPosition = props[2];

            foreach (var rl in flines)
            {
                if (rl.Contains(">"))
                {
                    var rule = rl.Split('>');
                    rules[char.Parse(rule[0])] = rule[1];
                }
            }
        }

        int it;
        void drawImage()
        {
            g.Clear(Color.White);
            string prev, lines = atom;
            float x1 = 0, y1 = 0;
            float x2 = 0, y2 = 0;
            for (int i = 0; i < it; i++)
            {
                prev = lines;
                lines = "";
                foreach(var p in prev)
                    lines += rules.ContainsKey(p) ? rules[p] : p.ToString();
            }

            var res = new List<Tuple<PointF, PointF>>();

            if (startingPosition == "L")
            {
                x1 = pictureBox1.Width;
                y1 = pictureBox1.Height / 2;
                x2 = -pictureBox1.Width / 2;
            }
            else if (startingPosition == "U")
            {
                x1 = pictureBox1.Width / 2;
                y1 = pictureBox1.Height / 2;
                y2 = -pictureBox1.Height / 2;
            }
            else if (startingPosition == "R")
            {
                y1 = pictureBox1.Height / 2;
                x2 = pictureBox1.Width / 2;
            }
            else if (startingPosition == "D")
            {
                x1 = pictureBox1.Width / 2;
                y2 = pictureBox1.Height / 2;
            }

            var xx = new List<float>();
            xx.Add(x1);

            var yy = new List<float>();
            yy.Add(y1);
            foreach (var l in lines)
                if (l == '[')
                    states.Add(Tuple.Create(new PointF(x1, y1), new PointF(x2, y2)));
                else if (l == ']')
                {
                    var coords = states[states.Count - 1];
                    states.RemoveAt(states.Count - 1);
                    x1 = coords.Item1.X;
                    y1 = coords.Item1.Y;

                    x2 = coords.Item2.X;
                    y2 = coords.Item2.Y;
                }
                else if (l == '-')
                {
                    //https://en.wikipedia.org/wiki/Rotation_matrix
                    var temp = x2;
                    x2 = (float)(temp * Math.Cos(rotationAngle) + y2 * Math.Sin(rotationAngle));
                    y2 = (float)(-temp * Math.Sin(rotationAngle) + y2 * Math.Cos(rotationAngle));
                }
                else if (l == '+')
                {
                    var temp = x2;
                    x2 = (float)(temp * Math.Cos(rotationAngle) - y2 * Math.Sin(rotationAngle));
                    y2 = (float)(temp * Math.Sin(rotationAngle) + y2 * Math.Cos(rotationAngle));
                }
                else
                {
                    res.Add(Tuple.Create(new PointF(x1, y1), new PointF(x1 + x2, y1 + y2)));
                    x1 += x2;
                    y1 += y2;
                    xx.Add(x1);
                    yy.Add(y1);
                }

            var scale = Math.Max(xx.Max() - xx.Min(), yy.Max() - yy.Min());
            foreach (var r in res)
                    g.DrawLine(Pens.Black, (xx.Max() - r.Item1.X) / scale * pictureBox1.Width, (yy.Max() - r.Item1.Y) / scale * pictureBox1.Height,
                                          (xx.Max() - r.Item2.X) / scale * pictureBox1.Width, (yy.Max() - r.Item2.Y) / scale * pictureBox1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            it = int.Parse(textBox1.Text);
            drawImage();
            pictureBox1.Invalidate();
        }
    }
}
