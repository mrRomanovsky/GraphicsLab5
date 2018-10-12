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
        }

        private PointF point1 = new PointF();
        private PointF point2 = new PointF();
        private PointF point3 = new PointF();

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

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

        private static Func<float, PointF> BezierLine(PointF p1, PointF p2) => p => new PointF(p1.X + p2.X * p, p1.Y + p2.Y * p);

        private static Func<float, PointF> HigherOrderBezierLine(Func<float, PointF> p1, Func<float, PointF> p2) =>
            p =>
            {
                var p1Point = p1(p);
                var p2Point = p2(p);
                return new PointF(p1Point.X + p2Point.X * p, p1Point.Y + p2Point.Y * p);
            };

        private void BezierCurve(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            var q0 = BezierLine(p0, p1);
            var q1 = BezierLine(p1, p2);
            var q2 = BezierLine(p2, p3);
            var r0 = HigherOrderBezierLine(q0, q1);
            var r1 = HigherOrderBezierLine(q1, q2);
            var b = HigherOrderBezierLine(r0, r1);
        }
    }
}
