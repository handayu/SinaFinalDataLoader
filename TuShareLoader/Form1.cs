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

        public Form1(BanKuaiDta bankuaiDta)
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

            //加载窗口
            this.Text = bankuaiDta.BanKuaiName;
            mPane.Title.Text = bankuaiDta.BanKuaiName;//标题

            //加载数据
            StockListToChart(bankuaiDta.BanKuaiStockList);
        }

        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            Color color = Color.FromArgb(int_Red, int_Green, int_Blue);
            return color;
        }

        public void StockListToChart(List<string> stockList)
        {
            GraphPane mPane = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上

            //每一只股票的最新的一个值做比较
            Dictionary<string, double> stockDatDic = new Dictionary<string, double>();

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

                double dataLast = 0.00;
                double.TryParse(maList[maList.Count - 1][4], out dataLast);

                stockDatDic.Add(stockList[j], dataLast);

                LineItem mCure = mPane.AddCurve(stockList[j], dataList, GetRandomColor(), SymbolType.None);
                zedGraphControl1.AxisChange();//画到zedGraphControl1控件中，此句必加                
            }

            //把最大的值作为最强的，最小的最为最弱的，作为标题方便查看
            //按照Value降序排列选出最大和最小的值的那只股票
            string minkey = stockDatDic.Keys.Select(x => new { x, y = stockDatDic[x] }).OrderBy(x => x.y).First().x;
            string maxkey = stockDatDic.Keys.Select(x => new { x, y = stockDatDic[x] }).OrderByDescending(x => x.y).First().x;

            string title = string.Format("Max:{0},Min:{1}", maxkey,minkey);
            this.Text = title;
        }

        private string zGCDateChart_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            return curve.Label.Text +":" + pt.X.ToString() +"/"+ pt.Y.ToString();
        }
    }
}
