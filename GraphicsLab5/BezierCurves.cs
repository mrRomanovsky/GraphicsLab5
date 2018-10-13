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
    public partial class BezierCurves : Form
    {
        public BezierCurves()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private PointF point1 = new PointF();
        private PointF point2 = new PointF();
        private PointF point3 = new PointF();
        private List<PointF> points = new List<PointF>();

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            point1.X = Int16.Parse(x1TextBox.Text);
            point1.Y = Int16.Parse(y1TextBox.Text);
            point2.X = Int16.Parse(x2TextBox.Text);
            point2.Y = Int16.Parse(y2TextBox.Text);
            point3.X = Int16.Parse(x3TextBox.Text);
            point3.Y = Int16.Parse(y3TextBox.Text);
        }

        private static Func<float, PointF> BezierLine(PointF p1, PointF p2) => t =>
            new PointF(p1.X * (1 - t) + p2.X * t, p1.Y * (1 - t) + p2.Y * t);

        private static Func<float, PointF> HigherOrderBezierLine(Func<float, PointF> p1, Func<float, PointF> p2) =>
            t =>
            {
                var p1Point = p1(t);
                var p2Point = p2(t);
                return new PointF(p1Point.X * (1 - t) + p2Point.X * t, p1Point.Y * (1 - t) + p2Point.Y * t);
            };

        private Func<float, PointF> CubicalBezierCurve(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            var q0 = BezierLine(p0, p1);
            var q1 = BezierLine(p1, p2);
            var q2 = BezierLine(p2, p3);
            var r0 = HigherOrderBezierLine(q0, q1);
            var r1 = HigherOrderBezierLine(q1, q2);
            var b = HigherOrderBezierLine(r0, r1);
            b = t =>
            {
                var oneMint3 = (1 - t) * (1 - t) * (1 - t);
                var oneMint2t = (1 - t) * (1 - t) * t;
                var oneMintt2 = (1 - t) * t * t;
                var t3 = t * t * t;
                return new PointF(p0.X * oneMint3 + 3 * p1.X * oneMint2t + 3 * p2.X * oneMintt2 + p3.X * t3,
                    p0.Y * oneMint3 + 3 * p1.Y * oneMint2t + 3 * p2.Y * oneMintt2 + p3.Y * t3);
            };
            return b;
        }

        private void DrawBezierCurve(Func<float, PointF> curve)
        {
            float x = 0;
            //PointF[] points = new PointF[10];
            var points = new List<PointF>();
            while (x < 1)
            {
                points.Add(curve(x));
                x += (float)0.01;
            }
            /*for (int i = 0; i < 10; ++i)
            {
                points[i] = curve(x);
                x += (float)0.1;
            }*/

            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {
                g.DrawCurve(new Pen(Color.Red), points.ToArray());
            }

            pictureBox1.Invalidate();
        }

        //точки всего 4? - нарисовать кубическую кривую
        //точек больше?
        //1)отметить начало и конец первых четырёх
        //2)отметить начало и конец следующих четырёх, при необходимости, добавив точки
        //3)нарисовать первую часть кривой Безье
        //4)сместить индекс на четвёртую точку из первых четырёх (чтобы "сфокусироваться" на следующих четырёх)
        //?????
        //6)PROFIT

        private void CompositeCubicalBezierCurve(List<PointF> points)
        {
            int first4Start = 0;
            int second4Start;
            int distToEnd;
            Func<float, PointF> bezierCurve;
            while (first4Start < points.Count - 1)
            {
                if (points.Count - first4Start == 4)
                {
                    bezierCurve = CubicalBezierCurve(points[first4Start], points[first4Start + 1],
                        points[first4Start + 2], points[first4Start + 3]);
                    DrawBezierCurve(bezierCurve);
                    break;
                }
                second4Start = first4Start + 3;
                distToEnd = points.Count - second4Start;
                distToEnd = points.Count - second4Start;
                if (distToEnd < 4)
                {
                    if (distToEnd == 2)
                        points.Insert(second4Start + 1, MiddlePoint(points[second4Start], points[second4Start + 1]));
                    points.Insert(second4Start, MiddlePoint(points[second4Start - 1], points[second4Start]));
                }
                bezierCurve = CubicalBezierCurve(points[first4Start], points[first4Start + 1],
                    points[first4Start + 2], points[first4Start + 3]);
                DrawBezierCurve(bezierCurve);
                first4Start = second4Start;
            }
        }

        private static PointF MiddlePoint(PointF p1, PointF p2)
        {
            float newX = (p1.X + p2.X) / 2;
            float newY = (p1.Y + p2.Y) / 2;
            return new PointF(newX, newY);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
            points.Add(e.Location);
            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                foreach (var p in points)
                  g.FillRectangle(new SolidBrush(Color.Blue), p.X - 1, p.Y - 1, 3, 3);
            if (points.Count >= 2)
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                    g.DrawLine(new Pen(Color.Blue), points[points.Count - 1], points.Last());
            pictureBox1.Invalidate();
            if (points.Count >= 4)
                CompositeCubicalBezierCurve(points);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            points.Clear();
            pictureBox1.Image = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
        }
    }
}
