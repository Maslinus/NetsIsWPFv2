using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NetsIsWPFv2.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace NetsIsWPFv2
{
    public partial class Form2 : Form
    {
        public int i = 0, j = 1; // переменные для корректного ввода данных
        public Form2()
        {
            InitializeComponent();
            Data.nodeNumber = 0;
            Data.points = new List<Point>(); // список значений узлов
            Data.Trafic = new List<List<int>>();// матрица нагрузки        
            Data.Con = new List<List<int>>();// матрица связности
            Data.Channels = new List<int>();
            Data.Channel_Cost = new List<int>();
        }
        #region Ввод XY 
        public void InputXY() // ввод местоположений узлов
        {

            Data.points.Add(new Point(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text)));
            textBox1.Clear();
            textBox2.Clear();
            if (i != (Data.nodeNumber))
            {
                groupBox1.Text = "Введите координаты " + (i + 1) + "-го узла";
            }
            else
            {
                i = 0;
                groupBox3.Visible = true;
                groupBox1.Visible = false;
                groupBox3.Text = "Введите трафик между " + (i + 1) + " и " + (j + 1) + " узлом";
                textBox4.Focus();
                return;

            }
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                i++;
                InputXY();
            }
        }
        #endregion
        private void textBox3_KeyDown(object sender, KeyEventArgs e) // ввод кол-ва узлов
        {
            if (e.KeyCode == Keys.Enter)
            {
                Data.nodeNumber = Convert.ToInt32(textBox3.Text);
                for (int c = 0; c < Data.nodeNumber; c++) // создаем матрицу свзяности
                {
                    List<int> myList2 = new List<int>();
                    for (int k = 0; k < Data.nodeNumber; k++)
                        myList2.Add(0);
                    Data.Con.Add(myList2);
                }
                for (int c = 0; c < Data.nodeNumber; c++) // создаем матрицу нагрузок
                {
                    List<int> myList2 = new List<int>();
                    for (int k = 0; k < Data.nodeNumber; k++)
                        myList2.Add(0);
                    Data.Trafic.Add(myList2);
                }
                label4.Dispose();
                textBox3.Dispose();
                groupBox1.Visible = true;
                groupBox1.Text = "Введите координаты " + (i + 1) + "-го узла";
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)//переход к отрисовке
        {
            Close();
            Form1.Graph();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox6.Focus();
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (i == 0)
                    button1.Visible = true;
                Data.Channels.Add(Convert.ToInt32(textBox5.Text));
                Data.Channel_Cost.Add(Convert.ToInt32(textBox6.Text));
                textBox5.Clear();
                textBox6.Clear();
                i++;
                groupBox2.Text = "Введите данные " + (i + 1) + "-го канала или нажмите кнопку отрисовки";
                textBox5.Focus();

            }
        }

        #region Ввод матрицы нагрузок
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Data.Trafic[i][j] = Convert.ToInt32(textBox4.Text);
                if (j == (Data.nodeNumber - 1))
                {
                    i++;
                    j = i;
                }
                if (i == (Data.nodeNumber - 1))
                {
                    groupBox3.Visible = false;
                    groupBox2.Visible = true;
                    i = 0;
                    groupBox2.Text = "Введите данные " + (i + 1) + "-го канала";
                    textBox5.Focus();
                    return;
                }
                j++;
                textBox4.Clear();
                groupBox3.Text = "Введите трафик между " + (i + 1) + " и " + (j + 1) + " узлом";
                textBox4.Focus();
            }
        }
        #endregion
    }
}