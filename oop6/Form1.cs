using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;

namespace oop6
{

    public partial class Form1 : Form
    {
        int points = 5;
        int middle_x = 640;
        int middle_y = 360;
        int radius = 200;        
        struct cords
        {
            public int _x;
            public int _y;
            public cords(int x, int y) {
                _x = x;
                _y = y;
            }
        }

        List<cords> conn = new List<cords>();

        public Form1()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(middle_x*2, middle_y*2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reloadBox(listBox1);
            reloadBox(listBox2);
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.DefaultExt = "matrix.txt";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {            
            // Делаем круг
            makeCircle(e);

            //Прямые
            connectLine(e);
        }
        private void makeCircle(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            for (int i = 0; i < points; ++i)
            {
                double rad = 2 * 3.14 * i / points;

                double x = radius * Math.Cos(rad);
                double y = radius * Math.Sin(rad);
                e.Graphics.DrawString(i.ToString(), Font, SystemBrushes.WindowText, middle_x + (float)x + 7, middle_y + (float)y + 7);
                e.Graphics.DrawEllipse(pen, middle_x + (float)x, middle_y + (float)y, 30, 30);
            }
        }
        private void reloadBox(ListBox comboBox)
        {
            comboBox.Items.Clear();
            for(int i = 0; i < points; i++)
            {

                comboBox.Items.Add(i.ToString());
            }
           
        }
        private void connectLine(PaintEventArgs e)
        {


            for (int i = 0; i < conn.Count(); i++)
            {
                if(conn[i]._y < points && conn[i]._x < points)
                {
                    double rad1 = 2 * 3.14 * conn[i]._x / points;

                    double x1 = (radius - 30) * Math.Cos(rad1);
                    double y1 = (radius - 30) * Math.Sin(rad1);
                    double rad2 = 2 * 3.14 * conn[i]._y / points;

                    double x2 = (radius - 30) * Math.Cos(rad2);
                    double y2 = (radius - 30) * Math.Sin(rad2);


                    using (Pen p = new Pen(Color.Black, 3))
                    using (GraphicsPath capPath = new GraphicsPath())
                    {
                        // A triangle
                        capPath.AddLine(-2, 0, 2, 0);
                        capPath.AddLine(-2, 0, 0, 2);
                        capPath.AddLine(0, 2, 2, 0);

                        p.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);

                        e.Graphics.DrawLine(p, middle_x + (float)x1 + 15, middle_y + (float)y1 + 15, middle_x + (float)x2 + 15, middle_y + (float)y2 + 15);
                    }
                }
                
                


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points++;
            Invalidate();
            reloadBox(listBox1);
            reloadBox(listBox2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points--;
            Invalidate();
            reloadBox(listBox1);
            reloadBox(listBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cords t = new cords();
            t._y = Int32.Parse(listBox1.SelectedItem.ToString());
            t._x = Int32.Parse(listBox2.SelectedItem.ToString());

            conn.Add(t);
            Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(filename);
           try
           {
                if (fileText.Contains("a"))
                {
                    string[] temp = fileText.Split(';');
                    points = Int32.Parse(temp[1]);
                    conn.Clear();
                    for (int i = 2; i < temp.Length - 1; i++)
                    {
                        cords cords = new cords(Int32.Parse(temp[i]), Int32.Parse(temp[i + 1]));
                        conn.Add(cords);
                        ++i;
                    }
                }
                else
                {
                    
                    string[] temp = fileText.Split(' ');
                    int t = Convert.ToInt32(Math.Sqrt(temp.Count()));
                    points = t;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        for (int j = 0; j < t; j++)
                        {
                        if (i < temp.Length)
                            {
                                if (temp[i].Trim('\n') == "1")
                                {
                                    cords cords = new cords(i/ Convert.ToInt32(Math.Sqrt(temp.Count())), j);
                                MessageBox.Show(i.ToString() + " " + j.ToString());
                                    conn.Add(cords);
                                }
                                ++i;
                            }
                        }
                        --i;
                    }
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Invalidate();



        }
        private void button5_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;


            // сохраняем текст в файл
            string temp = "a;";
            temp += points.ToString();
            temp += ";";
            foreach (var i in conn)
            {
                temp += i._x.ToString();
                temp += ";";
                temp += i._y.ToString();
                temp += ";";
            }

            System.IO.File.WriteAllText(filename, temp);
            MessageBox.Show("Файл сохранен");
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;


            // сохраняем текст в файл
            string temp = "";


            for(int i = 0; i < points; ++i)
            {
                for (int j = 0; j<points; ++j)
                {
                    cords t = new cords(i,j);
                    if (conn.Contains(t) == true)
                        temp += "1 ";
                    else temp += "0 ";
                }
                temp += "\n";
            }

            System.IO.File.WriteAllText(filename, temp);
            MessageBox.Show("Файл сохранен");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn.Clear();
            Invalidate();

        }
    }
}
