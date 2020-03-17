using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;

namespace TuShareLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GraphPane mPane = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
            mPane.XAxis.Title.Text = "DaySeries";//X轴标题
            mPane.YAxis.Title.Text = "PercentUP%";//Y轴标题
            mPane.Title.Text = "板块测试";//标题
            //mPane.XAxis.Scale.MaxAuto = true;
            mPane.XAxis.Type = ZedGraph.AxisType.LinearAsOrdinal;//出现图表右侧出现空白的情况....
            mPane.XAxis.CrossAuto = true;//容许x轴的自动放大或缩小
            mPane.YAxis.MajorGrid.IsVisible = true;//设置虚线.
            zedGraphControl1.IsShowHScrollBar = true;//不显示水平滚动条
            zedGraphControl1.PanModifierKeys = Keys.None;//鼠标拖拽可移动

        }

        private void Form_Load(object sender, EventArgs e)
        {
            List<string> stockList = new List<string>();
            stockList.Add("002627.SZ");
            stockList.Add("600561.SH");
            stockList.Add("600611.SH");
            stockList.Add("600650.SH");
            stockList.Add("600662.SH");
            stockList.Add("600676.SH");
            stockList.Add("600834.SH");
            stockList.Add("603776.SH");

            StockListToChart(stockList);
        }

        public void StockListToChart(List<string> stockList)
        {
            GraphPane mPane = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上

            for (int j = 0;j< stockList.Count; j++)
            {
                //内部把token和Url，日期，数据周期，等参数封包了，只暴露了股票代码参数，默认从20190601开始的数据
                StockData sData1 = HttpHelper.PostUrl(stockList[j]);
                List<List<string>> maList = sData1.HoladMaList();
                maList.Reverse();

                PointPairList dataList = new PointPairList();

                for (int i = 0; i < maList.Count; i++)
                {
                    PointPair pairData = new PointPair();

                    double x = 0.00;
                    double.TryParse(maList[i][1], out x);

                    double y = 0.00;
                    double.TryParse(maList[i][4], out y);

                    pairData.X = x;
                    pairData.Y = y;

                    dataList.Add(pairData);
                }

                LineItem mCure = mPane.AddCurve(stockList[j], dataList, Color.Blue, SymbolType.None);
                zedGraphControl1.AxisChange();//画到zedGraphControl1控件中，此句必加
            }
        }
    }
}
