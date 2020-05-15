using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace VM_LAB__num1
{
    public partial class Form1 : Form
    {
        double  dx, h, delta, b, a;
        int n;
        double[] x;
        double[] z;
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void RunRungeKutt()
        {
            x = new double[1000000];
            z = new double[1000000];
         
            x[0] = a;
            z[0] = 0;
            if (checkBox1.Checked == true)
            {
                h = (b - a) / 5000;
            }
            n = Convert.ToInt32((b - a) / h);
            double T1, T2, T3, T4, U1, U2, U3, U4;
            for (int i = 0; i < n; i++)
            {
                U1 = h * z[i];
                U2 = h * (z[i] + U1 / 2);
                U3 = h * (z[i] + U2 / 2);
                U4 = h * (z[i] + U3);

                T1 = h * f1(x[i], z[i]);
                T2 = h * f1(x[i] + h / 2, z[i] + T1 / 2);
                T3 = h * f1(x[i] + h / 2, z[i] + T2 / 2);
                T4 = h * f1(x[i] + h, z[i] + T3);

            
                x[i+1] = x[i] + (U1 + 2 * U2 + 2 * U3 + U4) / 6;
                z[i+1] = z[i] + (T1 + 2 * T2 + 2 * T3 + T4) / 6;
            }
            ChartGraph(x, z, n);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            MessageBox.Show("График успешно очищен!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                textBox3.Enabled = false;
            }
            else
                textBox3.Enabled = true;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            textBox1.Text = label10.Text;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            textBox2.Text = label9.Text;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            textBox4.Text = label7.Text;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            textBox5.Text = label6.Text;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            textBox3.Text = label8.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ReadCheckParams() == 1)
            {
                MessageBox.Show("Введены некорректные параметры! Проверьте правильность введенных параметров и попробуйте снова.",
                    "Предупреждение",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            else
            {
                RunRungeKutt();
                RememberParams();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(n == 0)
            {
                MessageBox.Show("Нечего сохранять!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;       
            string filename = saveFileDialog1.FileName;
            string filetext = GetXYvalues(x,z,n);
            System.IO.File.WriteAllText(filename, filetext);
            MessageBox.Show("Файл успешно сохранен!","Информация", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private int ReadCheckParams()
        {
            try
            {
                a = double.Parse(textBox1.Text, CultureInfo.InvariantCulture.NumberFormat);
                dx = double.Parse(textBox2.Text, CultureInfo.InvariantCulture.NumberFormat);
                if(checkBox1.Checked == false)
                    h = double.Parse(textBox3.Text, CultureInfo.InvariantCulture.NumberFormat);
                delta = double.Parse(textBox4.Text, CultureInfo.InvariantCulture.NumberFormat);
                b = double.Parse(textBox5.Text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return 1;
            }
            return 0;
        }
        private double f1(double _x, double _z)
        {
            double value;
            value = -delta * _z - Math.Sin(_x);     
            return value;
        }  
        private void ChartGraph(double[] x, double[] z,int  n)
        {
            for(int i = 0; i < n - 1; i++)
            {  
                chart1.Series[0].Points.AddXY(Math.Round(x[i],3), z[i]);
            }
            
        }
        private void RememberParams()
        {
            label10.Text = textBox1.Text;
            label9.Text = textBox2.Text;
            label7.Text = textBox4.Text;
            label6.Text = textBox5.Text;
            label8.Text = textBox3.Text;
        }
        private string GetXYvalues(double[] x, double[] y, int n)
        {
            string result = "";
            for(int i = 0; i< n; i++)
            {
                result = result.Insert(result.Length, Convert.ToString(Math.Round(x[i],5)));
                result = result.Insert(result.Length, " ; ");
                result = result.Insert(result.Length, Convert.ToString(Math.Round(y[i],5)));
                result = result.Insert(result.Length, System.Environment.NewLine);
            }
            return result;
        }
    }
}
