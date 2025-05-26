using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace NetsIsWPFv2
{
    public partial class Form1 : Form
    {
        public class Graphw
        {
            private int V;

            public Graphw(int v)
            {
                V = v;
            }

            private int FindMinDistance(int[] distance, bool[] shortestPathSet)
            {
                int minDistance = int.MaxValue;
                int minIndex = -1;

                for (int v = 0; v < V; ++v)
                {
                    if (shortestPathSet[v] == false && distance[v] <= minDistance)
                    {
                        minDistance = distance[v];
                        minIndex = v;
                    }
                }

                return minIndex;
            }

            private void PrintPath(int[] parent, int j, System.Windows.Forms.TextBox text)
            {
                if (parent[j] == -1)
                {
                    return;
                }
                
                PrintPath(parent, parent[j],  text);
                text.Text +=(j + 1) + " ";
                Data.put.Add(j);
            }

            // Метод для печати результатов нахождения кратчайшего пути
            private void PrintSolution(int[] distance, int[] parent, int src, int dest, System.Windows.Forms.TextBox text)
            {
                Data.put.Clear();
                text.Text="";
                text.Text += "Кратчайший путь от канала" + (src + 1) + " к каналу " + (dest + 1) + " состоит из каналов:\n";
                text.Text += (src + 1) + " ";
                Data.put.Add(src);
                PrintPath(parent, dest, text);
                text.Text += "Минимальное значение: " + distance[dest];
                Graph();
            }


            public void Dijkstra(int[,] graph, int src, int dest, System.Windows.Forms.TextBox text)
            {
                int[] distance = new int[V]; // Массив для хранения расстояний между вершинами
                bool[] shortestPathSet = new bool[V]; // Массив для отметки посещенных вершин
                int[] parent = new int[V]; // Массив для хранения предыдущих вершин в кратчайшем пути

                // Инициализация массивов
                for (int i = 0; i < V; ++i)
                {
                    distance[i] = int.MaxValue;
                    shortestPathSet[i] = false;
                    parent[i] = -1;
                }

                distance[src] = 0;

                // Нахождение кратчайшего пути для всех вершин
                for (int count = 0; count < V - 1; ++count)
                {
                    int u = FindMinDistance(distance, shortestPathSet);

                    shortestPathSet[u] = true;

                    for (int v = 0; v < V; ++v)
                    {
                        if (!shortestPathSet[v] && graph[u, v] != 0 && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                        {
                            parent[v] = u;
                            distance[v] = distance[u] + graph[u, v];
                        }
                    }
                }

                // Печать результатов
                PrintSolution(distance, parent, src, dest, text);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(textBox1.Text))>0 & (Convert.ToInt32(textBox2.Text))>0 & (Convert.ToInt32(textBox1.Text) - 1)<= (Data.points.Count-1) & (Convert.ToInt32(textBox2.Text) - 1) <= (Data.points.Count - 1)) {
                int[,] graphData = new int[Data.points.Count, Data.points.Count];
                for (int k = 0; k < Data.points.Count; k++)
                {
                    for (int j = 0; j < Data.points.Count; j++)
                    {
                        graphData[k, j] = Data.Con[k][j];
                    }
                }
                Graphw graph = new Graphw(Data.points.Count);
                graph.Dijkstra(graphData, Convert.ToInt32(textBox1.Text) - 1, Convert.ToInt32(textBox2.Text) - 1, textBox3);
                int m = 0;
                while (Data.put.Count>m)
                {
                    m++;
                }
            }
        }
        
        public void Bulder()
        {
            DataTable table = new DataTable();
            table.Columns.Add(" ");
            for (int j = 1; j < Data.points.Count+1; j++)
            {
                table.Columns.Add("Узел " + (j));

            }
            table.Rows.Add();
            for (int k = 0; k < Data.points.Count; k++)
            {
                
                table.Rows.Add();
                table.Rows[k][0] = "Узел " + (k+1);
                for (int j = 1; j < Data.points.Count+1; j++)
                {
                    table.Rows[k][j] = Data.Trafic[k][j-1];

                }
            }
            dataGridView1.DataSource = table;

            DataTable table1 = new DataTable();
            table1.Columns.Add(" ");
            for (int j = 1; j < Data.points.Count+1; j++)
            {
                table1.Columns.Add("Узел " + (j));

            }
            table1.Rows.Add();
            for (int k = 0; k < Data.points.Count; k++)
            {
                table1.Rows.Add();
                table1.Rows[k][0] = "Узел " + (k + 1);
                for (int j = 1; j < Data.points.Count+1; j++)
                {
                    table1.Rows[k][j] = Data.Con[k][j-1];

                }
            }
            dataGridView2.DataSource = table1;

            DataTable table2 = new DataTable();
            table2.Columns.Add("Num", typeof(string));
            table2.Columns.Add("X", typeof(int));
            table2.Columns.Add("Y", typeof(int));
            for (int j = 0; j < Data.points.Count; j++)
            {
                table2.Rows.Add("Узел " + (j + 1), Data.points[j].X, Data.points[j].Y);

            }
            dataGridView3.DataSource = table2;

            DataTable table3 = new DataTable();
            table3.Columns.Add("Num", typeof(string));
            table3.Columns.Add("Channels", typeof(int));
            table3.Columns.Add("Сost in rub", typeof(int));
            for (int j = 0; j < Data.Channels.Count; j++)
            {
                table3.Rows.Add("Канал " + (j + 1), Data.Channels[j], Data.Channel_Cost[j]);

            }
            dataGridView4.DataSource = table3;

        }

        public void load()
        {
            double Y = 0;
            double T = 0;
            for (int j = 0; j < Data.nodeNumber; j++)
            {
                for (int k = j + 1; k < Data.nodeNumber; k++)
                {
                    Y += (double)Data.Trafic[j][k];
                }
            }
            for (int j = 0; j < Data.nodeNumber; j++)
            {
                for (int k = j + 1; k < Data.nodeNumber; k++)
                {
                    if (Data.Con[j][k] > 0)
                    {
                        T += (((double)Data.Trafic[j][k]/(Data.Con[j][k]- Data.Trafic[j][k]))/Y);
                    }
                }
            }
            textBox5.Text = T.ToString();
        }

        int i = 0;
        string path;
        public static Graphics graphics;
        public Form1()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics();
        }

        public static void Graph()
        {
            graphics.Clear(Color.DimGray);
            for (int i = 0; i < Data.points.Count(); i++)
            {
                graphics.DrawRectangle(Pens.Black, new Rectangle(Data.points[i], new Size(30, 30)));
                if (Data.put.Count > 0 && Data.put.IndexOf(i) >= 0)
                {
                    graphics.DrawRectangle(Pens.Green, new Rectangle(Data.points[i], new Size(30, 30)));
                } 
                graphics.DrawString(i + 1 + "\n" + Data.points[i].ToString(), new Font("Arial", 8), Brushes.Black, Data.points[i]);

            }

            for (int i = 0; i < Data.nodeNumber; i++)
            {
                for (int j = i + 1; j < Data.nodeNumber; j++)
                {
                    if (Data.Con[i][j] != 0)
                    {
                        graphics.DrawLine(Pens.Black, new Point(Data.points[i].X + 15, Data.points[i].Y + 15), new Point(Data.points[j].X + 15, Data.points[j].Y + 15));
                        if (Data.put.Count > 0 && Data.put.IndexOf(i) >= 0 && Data.put.IndexOf(j) >= 0 && ( Data.put.IndexOf(i) == (Data.put.IndexOf(j)-1)) | (Data.put.IndexOf(j) == (Data.put.IndexOf(i) - 1)))
                        {
                            graphics.DrawLine(Pens.Green, new Point(Data.points[i].X + 15, Data.points[i].Y + 15), new Point(Data.points[j].X + 15, Data.points[j].Y + 15));
                        }
                        graphics.DrawString(Data.Con[i][j].ToString(), new Font("Arial", 8), Brushes.Black, new Point((Data.points[i].X + Data.points[j].X + 30) / 2, (Data.points[i].Y + Data.points[j].Y + 30) / 2));
                    }
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e) { }

        private void ToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            OpenFileDialog sf = new OpenFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                path = sf.FileName;
                Data.File_Read(path);
                Data.put.Clear();
                textBox3.Clear();
                Graph();
                Bulder();
                label3.BackColor = Color.Aquamarine;
                label1.BackColor = Color.White;
                label2.BackColor = Color.White;
                label4.BackColor = Color.White;
                dataGridView3.Visible = true;
                dataGridView1.Visible = false;
                dataGridView2.Visible = false;
                dataGridView4.Visible = false;
            }
        }

        private void ToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Data.File_Write(path);

        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sf = new OpenFileDialog();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                path = sf.FileName;
                Data.File_Read(path);


            }
            Form2 form2 = new Form2();
            form2.Show();

        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            label3.BackColor = Color.Aquamarine;
            label1.BackColor = Color.White;
            label2.BackColor = Color.White;
            label4.BackColor = Color.White;
            dataGridView3.Visible = true;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView4.Visible = false;
            Bulder();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = String.Format("{0:C}", Data.summa());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Data.put.Clear();
            Graph();
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            load();
        }

        // Матрица нагрузок
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int k = e.ColumnIndex-1;
            int j = e.RowIndex;
            object cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            if (int.TryParse(cellValue.ToString(), out int newValue))
            {
                if (j == k)
                {
                    if (newValue != 0)
                    {
                        MessageBox.Show("Нельзя изменять значения на главной диагонали. Значение должно быть 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridView1.Rows[j].Cells[k+1].Value = 0;
                    }
                }
                else if (k+1 == 0)
                {
                    Bulder();
                }
                else
                if (newValue <= 0)
                {
                    Bulder();
                    MessageBox.Show("Значение не может быть отрицательным или равным 0", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    Data.Trafic[j][k] = newValue;
                    Data.Trafic[k][j] = newValue;
                    if (j != k)
                    {
                        Bulder();
                    }

                }
            }
            else
            {
                Bulder();
                MessageBox.Show("Некорректное значение. Пожалуйста, введите целое число.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Матрица смежности
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int k = e.ColumnIndex-1;
            int j = e.RowIndex;
            object cellValue = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (int.TryParse(cellValue.ToString(), out int newValue))
            {
                if (j == k)
                {
                    if (newValue != 0)
                    {
                        MessageBox.Show("Нельзя изменять значения на главной диагонали. Значение должно быть 0.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dataGridView2.Rows[j].Cells[k+1].Value = 0;
                    }
                }
                else if (k + 1 == 0)
                {
                    Bulder();
                }
                else
                {
                    if (newValue < 0)
                    {
                        Bulder();
                        MessageBox.Show("Значение не может быть отрицательным", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    if (newValue == 0)
                    {
                        Data.Con[k][j] = 0;
                        Data.Con[j][k] = 0;
                        Bulder();
                        Graph();
                    }
                    else
                    if (newValue < Data.Trafic[k][j])
                    {
                        Bulder();
                    }
                    else
                    {
                        Data.Con[j][k] = newValue;
                        Data.Con[k][j] = newValue;
                        if (j != k)
                        {
                            Bulder();
                            Graph();
                        }
                    }
                }
            }
            else
            {
                Bulder();
                MessageBox.Show("Некорректное значение. Пожалуйста, введите целое число.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Матрица координат
        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int k = e.ColumnIndex;
            int j = e.RowIndex;
            object cellValue = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (int.TryParse(cellValue.ToString(), out int newValue))
            {
                if (newValue < 0)
                {
                    Bulder();
                    MessageBox.Show("Значение не может быть отрицательным", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int result;
                    if (k == 1)
                    {
                        result = (int)dataGridView3.Rows[j].Cells[1].Value;
                        Data.points[j] = new Point(result, Data.points[j].Y);
                    }
                    if (k == 2)
                    {
                        result = (int)dataGridView3.Rows[j].Cells[2].Value;
                        Data.points[j] = new Point(Data.points[j].X, result);
                    }
                    Bulder();
                    Graph();
                }
            }
            else
            {
                Bulder();
                MessageBox.Show("Некорректное значение. Пожалуйста, введите целое число.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Матрица каналов
        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int k = e.ColumnIndex;
            int j = e.RowIndex;
            object cellValue = dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (int.TryParse(cellValue.ToString(), out int newValue))
            {

                if (newValue < 0)
                {
                    Bulder();
                    MessageBox.Show("Значение не может быть отрицательным", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                if ((j + 1 > Data.channelsNumber))
                {
                    if (k == 1) {
                        Data.Channels.Add(newValue);
                        Data.Channel_Cost.Add(1000);
                        Data.channelsNumber++;
                        Bulder();
                    }
                    else
                    if (k == 2)
                    {
                        Data.Channels.Add(1000);
                        Data.Channel_Cost.Add(newValue);
                        Data.channelsNumber++;
                        Bulder();
                    }
                    else
                    {
                        Bulder();
                    }
                }
                else
                if (k == 1)
                {
                    if (newValue == 0)
                    {
                        for (int i = 0; i < Data.nodeNumber; i++)
                        {
                            for (int h = i; h < Data.nodeNumber; h++)
                            {
                                if (Data.Con[i][h] == Data.Channels[j] || Data.Con[h][i] == Data.Channels[j])
                                {
                                    Data.Con[i][h] = 0;
                                    Data.Con[h][i] = 0;
                                }
                            }
                        }
                        Data.Channels.RemoveAt(j);
                        Data.Channel_Cost.RemoveAt(j);
                        Data.channelsNumber--;
                        Bulder();
                        Graph();
                    }
                    else
                    {
                        Data.Channels[j] = newValue;
                        Bulder();
                    }
                }
                else
                if (k == 2)
                {
                    if (newValue == 0)
                    {
                        Bulder();
                        MessageBox.Show("Значение не может быть 0", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Data.Channel_Cost[j] = newValue;
                        Bulder();
                    }
                }
                else
                {
                    Bulder();
                }
            }
            else
            {
                Bulder();
                MessageBox.Show("Некорректное значение. Пожалуйста, введите целое число.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            label3.BackColor = Color.Aquamarine;
            label1.BackColor = Color.White;
            label2.BackColor = Color.White;
            label4.BackColor = Color.White;
            dataGridView3.Visible = true;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView4.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.BackColor = Color.Aquamarine;
            label3.BackColor = Color.White;
            label2.BackColor = Color.White;
            label4.BackColor = Color.White;
            dataGridView1.Visible = true;
            dataGridView3.Visible = false;
            dataGridView2.Visible = false;
            dataGridView4.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label2.BackColor = Color.Aquamarine;
            label1.BackColor = Color.White;
            label3.BackColor = Color.White;
            label4.BackColor = Color.White;
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            label4.BackColor = Color.Aquamarine;
            label1.BackColor = Color.White;
            label3.BackColor = Color.White;
            label2.BackColor = Color.White;
            dataGridView4.Visible = true;
            dataGridView2.Visible = false;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
        }

        #region добавление канала
        int first_Node;
        int second_Node;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int j = 0; j < Data.nodeNumber; j++)
                {
                    Point p = Data.points[j];
                    Rectangle R = new Rectangle(new Point(p.X, p.Y), new Size(30, 30));
                    if (R.Contains(e.Location))
                    {
                        if (i == 0)
                        {
                            first_Node = j;
                            i++;
                            graphics.DrawRectangle(Pens.Yellow, R);
                        }
                        else
                        {
                            i = 0;

                            graphics.DrawRectangle(Pens.Black, new Rectangle(new Point(Data.points[first_Node].X, Data.points[first_Node].Y), new Size(30, 30)));
                            if (first_Node != j)
                            {
                                second_Node = j;
                                GroupBox groupBox = new GroupBox();
                                groupBox.Location = new Point(200, 100);
                                groupBox.AutoSize = true;
                                groupBox.Text = "Введите пропускную способность";
                                int v = 0;

                                System.Windows.Forms.Button[] buttons = new System.Windows.Forms.Button[Data.Channels.Count() + 1];
                                for (int k = 0; k <= Data.Channels.Count(); k++)
                                {
                                    if (k != Data.Channels.Count())
                                    {

                                        if (Data.Trafic[first_Node][second_Node] < Data.Channels[k])
                                        {
                                            buttons[k] = new System.Windows.Forms.Button();
                                            groupBox.Controls.Add(buttons[k]);
                                            buttons[k].Location = new Point(10, 20 + v * 30);
                                            buttons[k].AutoSize = true;
                                            buttons[k].Text = Convert.ToString(Data.Channels[k]);
                                            buttons[k].Click += new EventHandler(button_Click);
                                            v++;
                                        }
                                    }
                                    else
                                    {
                                        buttons[k] = new System.Windows.Forms.Button();
                                        groupBox.Controls.Add(buttons[k]);
                                        buttons[k].Location = new Point(10, 20 + v * 30);
                                        buttons[k].AutoSize = true;
                                        buttons[k].Text = "Отмена";
                                        buttons[k].Click += new EventHandler(button_Click);
                                        v++;
                                    }
                                }
                                Controls.Add(groupBox);
                                groupBox.BringToFront();
                            }
                        }
                        return;
                    }
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (((sender as System.Windows.Forms.Button).Text) != "Отмена")
            {
                foreach (int ch in Data.Channels)
                    if (Convert.ToInt32((sender as System.Windows.Forms.Button).Text) == ch)
                    {
                        Data.Con[Math.Min(first_Node, second_Node)][Math.Max(first_Node, second_Node)] = ch;
                        Data.Con[Math.Max(first_Node, second_Node)][Math.Min(first_Node, second_Node)] = ch;
                        break;
                    }
            }
            first_Node = 0;
            second_Node = 0;
            for (i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is GroupBox)
                {
                    Controls.RemoveAt(i);
                    GC.Collect();
                    break;
                }
            }
            Graph();
            Bulder();
        }
        #endregion

        #region удаление
        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int j = 0; j < Data.nodeNumber; j++)
                {
                    Point p = Data.points[j];
                    Rectangle R = new Rectangle(new Point(p.X, p.Y), new Size(30, 30));
                    if (R.Contains(e.Location))//удаление узла
                    {

                        Data.nodeNumber--;
                        Data.points.RemoveAt(j);
                        Data.Trafic.RemoveAt(j);
                        Data.Con.RemoveAt(j);
                        for (int k = 0; k < Data.nodeNumber; k++)
                        {
                            Data.Con[k].RemoveAt(j);
                            Data.Trafic[k].RemoveAt(j);
                        }
                        Graph();
                        Bulder() ;
                        return;
                    }
                    for (int k = j + 1; k < Data.nodeNumber; k++)
                    {
                    if (DeleteCanal.InRectangle(Data.points[j].X + 15, Data.points[j].Y + 10, Data.points[j].X + 15, Data.points[j].Y + 20, Data.points[k].X + 15, Data.points[k].Y + 10, Data.points[k].X + 15, Data.points[k].Y + 20, e.X, e.Y) == true && Data.Con[j][k] != 0)
                        {//удаление канала
                            Data.Con[j][k] = 0;
                            Graph();
                            Bulder();
                            return;
                        }
                    }
                }
            }
        }
    }
    class DeleteCanal//класс для удаления канала
    {
        static double Len(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        static double Area(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double a = Len(x1, y1, x2, y2);
            double b = Len(x1, y1, x3, y3);
            double c = Len(x3, y3, x2, y2);
            double p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
        static bool InTriangle(double x1, double y1, double x2, double y2, double x3, double y3, double x, double y)
        {
            double s = Area(x1, y1, x2, y2, x3, y3);
            double s1 = Area(x1, y1, x2, y2, x, y);
            double s2 = Area(x1, y1, x, y, x3, y3);
            double s3 = Area(x, y, x2, y2, x3, y3);
            // так как квадратный корень не может быть вычислен точно, нужно учесть возможность накопления погрешности
            // принимается, что точка, принадлежит треугольнику, если сумма площадей подтреугольников почти совпадает с площадью основного треугольника
            if (Math.Abs(s1 + s2 + s3 - s) < 0.000001)
                return true;
            else return false;
        }
        public static bool InRectangle(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, double x, double y)
        {
            if (InTriangle(x1, y1, x2, y2, x3, y3, x, y) || InTriangle(x1, y1, x4, y4, x3, y3, x, y))
                return true;
            return false;
        }
    }
    #endregion

    public static class Data
    {

        public static List<Point> points = new List<Point>(); // список значений узлов
        public static int nodeNumber = 0;
        public static int channelsNumber = 0;
        public static List<List<int>> Trafic = new List<List<int>>();// матрица нагрузки        
        public static List<List<int>> Con = new List<List<int>>();// матрица связности
        public static List<int> Channels = new List<int>();
        public static List<int> Channel_Cost = new List<int>();
        public static List<int> put = new List<int>();
        public static void File_Write(string path)
        {
            using (var fstream = new StreamWriter(path, false))
            {
                fstream.WriteLine(nodeNumber);
                foreach (var point in points)
                    fstream.WriteLine(point.ToShortPointString());
                foreach (var list in Trafic)
                    foreach (var tr in list)
                        fstream.WriteLine(tr);
                foreach (var list in Con)
                    foreach (var cn in list)
                        fstream.WriteLine(cn);
                fstream.WriteLine(Channels.Count);
                foreach (var channel in Channels)
                    fstream.WriteLine(channel);
                foreach (var cost in Channel_Cost)
                    fstream.WriteLine(cost);
            }
        }

        public static void File_Read(string path)
        {
            nodeNumber = 0;
            points = new List<Point>(); // список значений узлов
            Trafic = new List<List<int>>();// матрица нагрузки        
            Con = new List<List<int>>();// матрица связности
            Channels = new List<int>();
            Channel_Cost = new List<int>();
            using (var istream = new StreamReader(path))
            {
                if (istream.Peek() != -1)
                {
                    nodeNumber = Convert.ToInt32(istream.ReadLine());
                    for (int i = 0; i < nodeNumber; i++)
                    {
                        points.Add(istream.ReadLine().FromShortPointString());
                    }
                    for (int i = 0; i < nodeNumber; i++)
                    {
                        var list = new List<int>();
                        for (int j = 0; j < nodeNumber; j++)
                        {
                            list.Add(Convert.ToInt32(istream.ReadLine()));
                        }
                        Trafic.Add(list);
                    }
                    for (int i = 0; i < nodeNumber; i++)
                    {
                        var list = new List<int>();
                        for (int j = 0; j < nodeNumber; j++)
                        {
                            list.Add(Convert.ToInt32(istream.ReadLine()));
                        }
                        Con.Add(list);
                    }
                    int temp = Convert.ToInt32(istream.ReadLine());
                    channelsNumber = temp;
                    for (int i = 0; i < temp; i++)
                    {
                        Channels.Add(Convert.ToInt32(istream.ReadLine()));
                    }
                    for (int i = 0; i < temp; i++)
                    {
                        Channel_Cost.Add(Convert.ToInt32(istream.ReadLine()));
                    }
                }
            }
        }

        public static double summa()
        {
            double sum = 0;
            double dist;
            double channelcost = 0;
            for (int j = 0; j < nodeNumber; j++)
            {
                for (int k = j + 1; k < nodeNumber; k++)
                {
                    if (Con[j][k] != 0)
                    {
                        for (int c = 0; c < channelsNumber; c++)
                        {
                            if (Channels[c] == Con[j][k])
                            {
                                channelcost = Channel_Cost[c];
                                break;
                            }
                        }
                        dist = Math.Sqrt(Math.Pow(points[j].X - points[k].X, 2) + Math.Pow(points[j].Y - points[k].Y, 2));
                        sum += channelcost * dist;
                    }
                }
            }
            return sum;
        }

    }

    public static class PointExtension
    {
        public static string ToShortPointString(this Point pt)
        {
            return string.Format("{0} {1}", pt.X, pt.Y);
        }
    }

    public static class StringExtension
    {
        public static Point FromShortPointString(this string str)
        {
            string[] parts = str.Split();
            return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}