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

namespace Triangle
{
    public partial class Form1 : Form
    {
        private List<string> fileText;
        public Form1()
        {
            fileText = new List<string>();
            InitializeComponent();
            pictureBox1.BackColor = Color.White;

        }




        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox1.Text);
                int b = int.Parse(textBox2.Text);
                int c = int.Parse(textBox3.Text);
                
                label4.BackColor = Color.White;
                if (a == 0 | b == 0 | c == 0)
                {
                    MessageBox.Show("Диапазон значений от 1 до 99999.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    label4.Text = "";
                    pictureBox1.BackgroundImage = new Bitmap(600, 400);
                    pictureBox1.Invalidate();
                }
                else

                    if (a >= b + c | a <= b - c | b >= a + c | b <= a - c | c >= a + b | c <= a - b)
                    {
                        MessageBox.Show("Сумма длин двух сторон должна быть больше длины третьей стороны.");
                    }
                    else
                    {
                        if (a.Equals(b) & a.Equals(c))
                        {
                            label4.Text = "Равносторонний треугольник";
                        }
                        else if (a.Equals(b) | a.Equals(c) | b.Equals(c))
                        {
                            label4.Text = "Равнобедренный треугольник";
                        }
                        else { label4.Text = "Неравносторонний треугольник"; }
                        int[] parameters = GetParameters(a, b, c);
                        Triangle triangle = new Triangle(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                        DrawTriangle(triangle);
                        this.Invalidate();
                    MessageBox.Show("Треугольник нарисован.");
                    }
                
            }
            catch (FormatException error)
            {
                MessageBox.Show("Диапазон значений от 1 до 99999.");
                pictureBox1.BackgroundImage = new Bitmap(600, 400);
                pictureBox1.Invalidate();
            }
        }


        private List<int> ChangeScale(List<int> values)
        {
            List<double> newValues = new List<double>();
            foreach (var v in values)
            {
                var n = Convert.ToDouble(v);
                newValues.Add(n * 300 / values.Max());
            }

            List<int> newValues1 = new List<int>();
            foreach (var v in newValues)
            {
                newValues1.Add(Convert.ToInt32(v));
            }
            return newValues1;
        }

        private int[] GetParameters(int a, int b, int c)
        {
            List<int> values = new List<int>();
            values.Add(a);
            values.Add(b);
            values.Add(c);
            values.Sort();
            values.Reverse();
            values = ChangeScale(values);
            int[] parameters = new int[6];
            parameters[0] = 50;
            parameters[1] = 50;
            parameters[2] = parameters[0] + values.ElementAt(0);
            parameters[3] = parameters[1];
            int[] xy = GetThirdCoordinate(values);
            parameters[4] = xy[0]+ parameters[0];
            parameters[5] = xy[1]+ parameters[1];
            return parameters;
        }

        private int[] GetThirdCoordinate(List<int> values)
        {
            int[] xy = new int[2];
            values.Sort();
            values.Reverse();
            var p = values.Sum() / 2;
            var s = Math.Sqrt(p * (p - values[0]) * (p - values[1]) * (p - values[2]));
            var sinA = 2 * s / (values[0] * values[1]);
            var alpha = Math.Asin(sinA);
            int x = Convert.ToInt32(values[1] * Math.Cos(alpha));
            int y = Convert.ToInt32(values[1] * Math.Sin(alpha));
            xy[0] = x;
            xy[1] = y;
            return xy;
        }

        private void DrawTriangle(Triangle triangle)
        {
            pictureBox1.BackgroundImage = new Bitmap(600, 400);
            DrawLine(triangle.x1, triangle.y1, triangle.x2, triangle.y2);
            DrawLine(triangle.x2, triangle.y2, triangle.x3, triangle.y3);
            DrawLine(triangle.x3, triangle.y3, triangle.x1, triangle.y1);
            pictureBox1.Invalidate();
        }

        private void DrawLine(int x1,int y1, int x2, int y2)
        {
            Pen pen = new Pen(Color.Red, 1);
            Graphics.FromImage(pictureBox1.BackgroundImage).DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label4.Text = "";
            pictureBox1.BackgroundImage = new Bitmap(600, 400);
            pictureBox1.Invalidate();
            var filePath = string.Empty;
            fileText.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                filePath = openFileDialog.FileName;
                using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        fileText.Add(line);
                    }
                }
            }

            if (fileText.Count <= 3 & fileText.Count >0)
            {
                int[] nums = new int[3];
                bool error = false;
                for(int i=0; i<fileText.Count;i++)
                {
                        if (fileText[i].Length >= 5)
                        {
                            fileText[i]=fileText[i].Substring(0, fileText[i].Length - 5);
                        }

                        if(!int.TryParse(fileText[i], out nums[i]))
                        {
                            error = true;
                        }
            
                }
                

                if (error)
                {
                    MessageBox.Show("Ошибка данных в файле.");
                }else
                {
                    textBox1.Text = Math.Abs(nums[0]).ToString();
                    textBox2.Text = Math.Abs(nums[1]).ToString();
                    textBox3.Text = Math.Abs(nums[2]).ToString();
                }

            } 


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheck(e, textBox1.Text.Length);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheck(e, textBox2.Text.Length);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheck(e, textBox3.Text.Length);
        }

        private void InputCheck(KeyPressEventArgs e, int length)
        {
            char number = e.KeyChar;

            if (length >= 5 && number != 8)
            {
                e.Handled = true;
            }
            //if (length == 0)
            //{
            //    if ((e.KeyChar <= 48 || e.KeyChar >= 58))
            //    {
            //        e.Handled = true;
            //    }
            //}
            //else if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            //{
            //    e.Handled = true;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label4.Text = "";
            pictureBox1.BackgroundImage = new Bitmap(600, 400);
            pictureBox1.Invalidate();
            MessageBox.Show("Данная программа создана для определения вида треугольника. Треугольник - геометрическая фигура,"+
                " образованная тремя отрезками, которые соединяют три точки, не лежащие на одной прямой. У треугольника сумма"+
                " любых двух сторон должна быть больше третьей. Равнобедренный треугольник -" +
                " это треугольник, в котором две стороны равны между собой по длине. Равносторонний треугольник - треугольник,"+
                " у которого все стороны равны.");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = Change(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = Change(textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = Change(textBox3.Text);
        }

        public string Change(string str)
        {
            if (str.StartsWith("0"))
            {
                str = str.Remove(0, 1);
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsDigit(str[i]))
                {
                    str = str.Remove(i, 1);
                }
            }
            return str;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    }

