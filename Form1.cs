using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

struct Chart_Show
{
    public double[] xValues;
    public double[] yValues;
}
struct ZhiXian
{
    //起始终点坐标
    public double xs;
    public double ys;
    public double xe;
    public double ye;

    public double step;
    public int dir;
    public int count;

    public double[] x_chart;
    public double[] y_chart;

    //相对坐标
    public double xrs;
    public double yrs;

    public double xre;
    public double yre;

    public double x_chart_temp;
    public double y_chart_temp;
    /*
    public void InterpolationPoint()
    {
        double Fs;
        Fs = Math.Abs(xre) * Math.Abs(yrs) - Math.Abs(xrs) * Math.Abs(yre);
        x_chart_temp = 0;
        y_chart_temp = 0;
        if(dir == 1)
        {
            if (Fs >= 0)
            {
                x_chart_temp = xrs + step;
                y_chart_temp = yrs;
            }
            else
            {
                x_chart_temp = xrs;
                y_chart_temp = yrs + step;
            }
        }
        if (dir == 2)
        {
            if (Fs >= 0)
            {
                x_chart_temp = xrs - step;
                y_chart_temp = yrs;
            }
            else
            {
                x_chart_temp = xrs;
                y_chart_temp = yrs + step;
            }
        }
        if (dir == 3)
        {
            if (Fs >= 0)
            {
                x_chart_temp = xrs - step;
                y_chart_temp = yrs;
            }
            else
            {
                x_chart_temp = xrs;
                y_chart_temp = yrs - step;
            }
        }
        if (dir == 4)
        {
            if (Fs >= 0)
            {
                x_chart_temp = xrs + step;
                y_chart_temp = yrs;
            }
            else
            {
                x_chart_temp = xrs;
                y_chart_temp = yrs - step;
            }
        }
    }*/
}
struct YuanHu
{
    //圆心坐标
    public double cx;
    public double cy;
    public double R;

    public double theta0;       //起始角度
    public double detaltheta;   //转过角度
    public double alpha;

    public double step;
    public int dir;
    public int count;

    public double[] x_chart;
    public double[] y_chart;

    //起始终点坐标
    public double x1;
    public double y1;
    public double x2;
    public double y2;

    public double x_temp;
    public double y_temp;

}

namespace 直线圆弧插补
{
    public partial class Form1 : Form
    {
        Chart_Show ch;
        ZhiXian zx = new ZhiXian();
        YuanHu yh = new YuanHu();

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Series series = chart1.Series[0];
            // 画样条曲线（Spline）
            series.ChartType = SeriesChartType.Line;
            // 线宽2个像素
            series.BorderWidth = 2;
            // 线的颜色：红色
            series.Color = System.Drawing.Color.Red;
            // 图示上的文字
            series.LegendText = "插补曲线";

            double[] xv = { };
            double[] yv = { };

            // 准备数据
            ch.xValues = xv;
            ch.yValues = yv;
            //float[] xValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            //float[] yValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };// 10, 4, 5, 45, 34, 67, 23, 58, 99, 45 

            // 在chart中显示数据
            for (int i = 0; i < ch.xValues.Length; i++)
            {
                series.Points.AddXY(ch.xValues[i], ch.yValues[i]);
            }

            // 设置显示范围
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.AxisX.Minimum = -400;
            chartArea.AxisX.Maximum = 400;
            chartArea.AxisY.Minimum = -400;
            chartArea.AxisY.Maximum = 400;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zx.xs = 0;
            zx.ys = 0;
            zx.xe = 0;
            zx.ye = 0;
            zx.x_chart = new double[100000];
            zx.y_chart = new double[100000];
            zx.xrs = 0;
            zx.yrs = 0;
            zx.step = 1;
            zx.count = 1;

            if (textBox1.Text != String.Empty && textBox2.Text != String.Empty && textBox3.Text != String.Empty && textBox4.Text != String.Empty && textBox11.Text != String.Empty) //判断数据输入框是否为空
            {
                zx.xs = double.Parse(textBox1.Text.ToString());
                zx.ys = double.Parse(textBox2.Text.ToString());
                zx.xe = double.Parse(textBox4.Text.ToString());
                zx.ye = double.Parse(textBox3.Text.ToString());
                zx.step = double.Parse(textBox11.Text.ToString());

                zx.xre = zx.xe - zx.xs;
                zx.yre = zx.ye - zx.ys;

                //textBox10.AppendText("zx.xs:" + zx.xs + "\r\n");
                //textBox10.AppendText("zx.ys:" + zx.ys + "\r\n");
                //textBox10.AppendText("zx.xe:" + zx.xe + "\r\n");
                //textBox10.AppendText("zx.ye:" + zx.ye + "\r\n");
                //textBox10.AppendText("zx.xre:" + zx.xre + "\r\n");
                //textBox10.AppendText("zx.yre:" + zx.yre + "\r\n");

                //象限判断
                if (zx.xre >= 0 && zx.yre >= 0)
                {
                    zx.dir = 1;
                }
                if (zx.xre < 0 && zx.yre >= 0)
                {
                    zx.dir = 2;
                }
                if (zx.xre < 0 && zx.yre < 0)
                {
                    zx.dir = 3;
                }
                if (zx.xre >= 0 && zx.yre < 0)
                {
                    zx.dir = 4;
                }
                //textBox10.AppendText("zx.dir:" + zx.dir + "\r\n");

                //
                double Tstep;
                Tstep = (Math.Abs(zx.xre) +Math.Abs(zx.yre)) / zx.step;
                //textBox10.AppendText("Tstep:" + Tstep + "\r\n");
                zx.x_chart[1] = zx.xs;
                zx.y_chart[1] = zx.ys;
                while (zx.count < Tstep)
                {
                    zx.count = zx.count + 1;
                    //zx.InterpolationPoint();
                    //textBox10.AppendText("count:" + zx.count + "\r\n");

                    double Fs;
                    Fs = Math.Abs(zx.xre) * Math.Abs(zx.yrs) - Math.Abs(zx.xrs) * Math.Abs(zx.yre);
                    //textBox10.AppendText("Fs:" + Fs + "\r\n");
                    zx.x_chart_temp = 0;
                    zx.y_chart_temp = 0;
                    if (zx.dir == 1)
                    {
                        if (Fs >= 0)
                        {
                            zx.x_chart_temp = zx.xrs + zx.step;
                            zx.y_chart_temp = zx.yrs;
                        }
                        else
                        {
                            zx.x_chart_temp = zx.xrs;
                            zx.y_chart_temp = zx.yrs + zx.step;
                        }
                    }
                    if (zx.dir == 2)
                    {
                        if (Fs >= 0)
                        {
                            zx.x_chart_temp = zx.xrs - zx.step;
                            zx.y_chart_temp = zx.yrs;
                        }
                        else
                        {
                            zx.x_chart_temp = zx.xrs;
                            zx.y_chart_temp = zx.yrs + zx.step;
                        }
                    }
                    if (zx.dir == 3)
                    {
                        if (Fs >= 0)
                        {
                            zx.x_chart_temp = zx.xrs - zx.step;
                            zx.y_chart_temp = zx.yrs;
                        }
                        else
                        {
                            zx.x_chart_temp = zx.xrs;
                            zx.y_chart_temp = zx.yrs - zx.step;
                        }
                    }
                    if (zx.dir == 4)
                    {
                        if (Fs >= 0)
                        {
                            zx.x_chart_temp = zx.xrs + zx.step;
                            zx.y_chart_temp = zx.yrs;
                        }
                        else
                        {
                            zx.x_chart_temp = zx.xrs;
                            zx.y_chart_temp = zx.yrs - zx.step;
                        }
                    }

                    zx.x_chart[zx.count] = zx.x_chart_temp + zx.xs;
                    zx.y_chart[zx.count] = zx.y_chart_temp + zx.ys;
                    zx.xrs = zx.x_chart_temp;
                    zx.yrs = zx.y_chart_temp;
                    //textBox10.AppendText("zx.x_chart_temp:" + zx.x_chart_temp + "\tzx.y_chart_temp:" + zx.y_chart_temp + "\r\n");

                    //textBox10.AppendText("zx.xrs:" + zx.xrs + "\tzx.yrs:" + zx.yrs + "\r\n");
                }

                //画图

                Series series = chart1.Series[0];
                // 画样条曲线（Spline）
                series.ChartType = SeriesChartType.Line;
                // 线宽2个像素
                series.BorderWidth = 1;
                // 线的颜色：红色
                series.Color = System.Drawing.Color.Red;
                // 图示上的文字
                series.LegendText = "插补曲线";

                // 准备数据
                ch.xValues = zx.x_chart;
                ch.yValues = zx.y_chart;
                //float[] xValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                //float[] yValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };// 10, 4, 5, 45, 34, 67, 23, 58, 99, 45 

                // 在chart中显示数据
                for (int i = 1; i <= zx.count; i++)
                {
                    series.Points.AddXY(ch.xValues[i], ch.yValues[i]);
                    //textBox10.AppendText(ch.xValues[i] + "\t" + ch.yValues[i] + "\r\n");
                }

                // 设置显示范围
                ChartArea chartArea = chart1.ChartAreas[0];
                chartArea.AxisX.Minimum = -400;
                chartArea.AxisX.Maximum = 400;
                chartArea.AxisY.Minimum = -400;
                chartArea.AxisY.Maximum = 400;

            }
            else
            {
                MessageBox.Show("校准数据不能为空");
            }

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            yh.cx = 0;
            yh.cy = 0;
            yh.R = 0;
            yh.theta0 = 0;
            yh.detaltheta = 0;
            yh.alpha = 0;
            yh.step = 1;
            yh.count = 1;
            yh.x_chart = new double[100000];
            yh.y_chart = new double[100000];
            yh.x1 = 0;
            yh.y1 = 0;
            yh.x2 = 0;
            yh.y2 = 0;
            yh.x_temp = 0;
            yh.y_temp = 0;

            double xt = 0, yt = 0, alphaStep = 0, x_pre = 0,y_pre = 0;

            if (textBox5.Text != String.Empty && textBox6.Text != String.Empty && textBox7.Text != String.Empty && textBox8.Text != String.Empty && textBox9.Text != String.Empty && textBox11.Text != String.Empty) //判断数据输入框是否为空
            {
                yh.cx = double.Parse(textBox5.Text.ToString());
                yh.cy = double.Parse(textBox6.Text.ToString());
                yh.R = double.Parse(textBox7.Text.ToString());
                yh.theta0 = double.Parse(textBox8.Text.ToString());
                yh.detaltheta = double.Parse(textBox9.Text.ToString());
                yh.step = double.Parse(textBox11.Text.ToString());

                //textBox10.AppendText("detaltheta:" + yh.detaltheta + "\r\n");

                //计算起始角及增量角（弧度）
                yh.theta0 = Math.PI * (yh.theta0 % 360) / 180;
                if (yh.detaltheta < 0)
                {
                    yh.detaltheta = - Math.PI * (Math.Abs(yh.detaltheta) % 360) / 180;
                }
                else
                {
                    yh.detaltheta = Math.PI * (Math.Abs(yh.detaltheta) % 360) / 180;
                }

                //textBox10.AppendText("detaltheta:" + yh.detaltheta + "\r\n");

                //计算插补起点与终点坐标
                yh.x1 = yh.R * Math.Cos(yh.theta0);
                yh.y1 = yh.R * Math.Sin(yh.theta0);
                yh.x2 = yh.R * Math.Cos(yh.theta0 + yh.detaltheta);
                yh.y2 = yh.R * Math.Sin(yh.theta0 + yh.detaltheta);

                yh.x_temp = yh.x1;
                yh.y_temp = yh.y1;

                //textBox10.AppendText("yh.x1:" + yh.x1 + "\r\n");
                //textBox10.AppendText("yh.y1:" + yh.y1 + "\r\n");
                //textBox10.AppendText("yh.x2:" + yh.x2 + "\r\n");
                //textBox10.AppendText("yh.y2:" + yh.y2 + "\r\n");
                //textBox10.AppendText("yh.x_temp:" + yh.x_temp + "\r\n");
                //textBox10.AppendText("yh.y_temp:" + yh.y_temp + "\r\n");

                yh.x_chart[1] = yh.x1 + yh.cx;
                yh.y_chart[1] = yh.y1 + yh.cy;

                while (Math.Abs(yh.alpha) < Math.Abs(yh.detaltheta))
                {
                    yh.count = yh.count + 1;
                    //textBox10.AppendText("count:" + yh.count + "\r\n");

                    x_pre = yh.x_temp;
                    y_pre = yh.y_temp;

                    //象限判断
                    if ((yh.detaltheta >= 0 && yh.x_temp >= 0 && yh.y_temp < 0) || (yh.detaltheta < 0 && yh.x_temp < 0 && yh.y_temp >= 0))
                    {
                        yh.dir = 1;
                    }
                    if ((yh.detaltheta >= 0 && yh.x_temp >= 0 && yh.y_temp >= 0) || (yh.detaltheta < 0 && yh.x_temp < 0 && yh.y_temp < 0))
                    {
                        yh.dir = 2;
                    }
                    if ((yh.detaltheta >= 0 && yh.x_temp < 0 && yh.y_temp >= 0) || (yh.detaltheta < 0 && yh.x_temp >= 0 && yh.y_temp < 0))
                    {
                        yh.dir = 3;
                    }
                    if ((yh.detaltheta >= 0 && yh.x_temp < 0 && yh.y_temp < 0) || (yh.detaltheta < 0 && yh.x_temp >= 0 && yh.y_temp >= 0))
                    {
                        yh.dir = 4;
                    }

                    //textBox10.AppendText("yh.dir:" + yh.dir + "\r\n");

                    double DevVal;
                    DevVal = yh.x_temp * yh.x_temp + yh.y_temp * yh.y_temp - yh.R * yh.R;
                    //textBox10.AppendText("DevVal:" + DevVal + "\r\n");

                    //逆时针
                    if (yh.detaltheta >= 0)
                    {
                        if (yh.dir == 1)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp + yh.step;
                            }
                            else
                            {
                                xt = yh.x_temp + yh.step;
                                yt = yh.y_temp;
                            }
                        }
                        if (yh.dir == 2)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp - yh.step;
                                yt = yh.y_temp;
                            }
                            else
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp + yh.step;
                            }
                        }
                        if (yh.dir == 3)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp - yh.step;
                            }
                            else
                            {
                                xt = yh.x_temp - yh.step;
                                yt = yh.y_temp;
                            }
                        }
                        if (yh.dir == 4)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp + yh.step;
                                yt = yh.y_temp;
                            }
                            else
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp - yh.step;
                            }
                        }
                    }
                    //顺时针
                    else
                    {
                        if (yh.dir == 1)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp + yh.step;
                                yt = yh.y_temp;
                            }
                            else
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp + yh.step;
                            }
                        }
                        if (yh.dir == 2)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp + yh.step;
                            }
                            else
                            {
                                xt = yh.x_temp - yh.step;
                                yt = yh.y_temp;
                            }
                        }
                        if (yh.dir == 3)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp - yh.step;
                                yt = yh.y_temp;
                            }
                            else
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp - yh.step;
                            }
                        }
                        if (yh.dir == 4)
                        {
                            if (DevVal >= 0)
                            {
                                xt = yh.x_temp;
                                yt = yh.y_temp - yh.step;
                            }
                            else
                            {
                                xt = yh.x_temp + yh.step;
                                yt = yh.y_temp;
                            }
                        }
                    }

                    yh.x_chart[yh.count] = xt + yh.cx;
                    yh.y_chart[yh.count] = yt + yh.cy;
                    yh.x_temp = xt;
                    yh.y_temp = yt;
                    //textBox10.AppendText("yh.x_temp:" + yh.x_temp + "\tyh.y_temp:" + yh.y_temp + "\r\n");

                    alphaStep = Math.Abs(Math.Atan(yh.y_temp/yh.x_temp)) - Math.Abs(Math.Atan(y_pre / x_pre));
                    yh.alpha = yh.alpha + Math.Abs(alphaStep);
                }

                //画图

                Series series = chart1.Series[0];
                // 画样条曲线（Spline）
                series.ChartType = SeriesChartType.Line;
                // 线宽2个像素
                series.BorderWidth = 1;
                // 线的颜色：红色
                series.Color = System.Drawing.Color.Red;
                // 图示上的文字
                series.LegendText = "插补曲线";

                // 准备数据
                ch.xValues = yh.x_chart;
                ch.yValues = yh.y_chart;
                //float[] xValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                //float[] yValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };// 10, 4, 5, 45, 34, 67, 23, 58, 99, 45 

                // 在chart中显示数据
                for (int i = 1; i < yh.count; i++)
                {
                    series.Points.AddXY(ch.xValues[i], ch.yValues[i]);
                    //textBox10.AppendText(ch.xValues[i] + "\t" + ch.yValues[i] + "\r\n");
                }

                // 设置显示范围
                ChartArea chartArea = chart1.ChartAreas[0];
                chartArea.AxisX.Minimum = -400;
                chartArea.AxisX.Maximum = 400;
                chartArea.AxisY.Minimum = -400;
                chartArea.AxisY.Maximum = 400;

            }
            else
            {
                MessageBox.Show("校准数据不能为空");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Series series = chart1.Series[0];
            series.Points.Clear();
            //textBox10.Clear();
        }
    }
}
