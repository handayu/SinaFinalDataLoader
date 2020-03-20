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
            mPane.Legend.IsVisible = false;//不显示Lenged，因为当股票很多的时候，下面的图表会被压缩到一点点没有。

            //加载窗口
            this.Text = bankuaiDta.BanKuaiName;

            //图表标题
            mPane.Title.Text = bankuaiDta.BanKuaiName;//标题

            //加载数据
            StockListToChart(bankuaiDta.BanKuaiStockList);
        }

        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数
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

                if (maList.Count <= 0) continue;

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

            string title = string.Format("板块当前最强/最弱-Max:{0},Min:{1}", maxkey,minkey);
            this.Text = title;

            //板块显示强弱
            decimal qiangDuPercent = 0m;
            int iUpNum = 0;
            foreach(KeyValuePair<string,double> kvPair in stockDatDic)
            {
                if(kvPair.Value >= 0)
                {
                    iUpNum = iUpNum + 1;
                }
            }

            qiangDuPercent = iUpNum*100 / stockDatDic.Count;
            mPane.Title.Text = mPane.Title.Text + string.Format("[板块强度占比:{0}%]",qiangDuPercent.ToString());//标题

        }

        private string zGCDateChart_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            curve.IsSelected = true;


            PointPair pt = curve[iPt];
            return curve.Label.Text +":" + pt.X.ToString() +"/"+ pt.Y.ToString();

        }

        /// <summary>
        /// 在系统右键下拉菜单中加入隐藏和现实强势弱势标的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="menuStrip"></param>
        /// <param name="mousePt"></param>
        /// <param name="objState"></param>
        private void ContextMenuClick_Event(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            ToolStripSeparator tPera = new ToolStripSeparator();
            menuStrip.Items.Add(tPera);

            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "显示/隐藏弱势标的";
            menuItem.Checked = true;

            menuItem.Click += MenuItem_Click;
            menuStrip.Items.Add(menuItem);

        }

        /// <summary>
        /// 默认为显示强-弱标的
        /// </summary>
        private int m_isVisableDownStock = 1;

        /// <summary>
        /// 单击改变强势和弱势
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItem_Click(object sender, EventArgs e)
        {
            switch(m_isVisableDownStock)
            {
                case 1:

                    if ((sender as ToolStripMenuItem).Text == "显示/隐藏弱势标的")
                    {
                        (sender as ToolStripMenuItem).Checked = false;
                    }

                    this.progressBar1.Value = 50;

                    GraphPane mPane1 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
                    CurveList curList = mPane1.CurveList;
                    foreach (CurveItem curItem in curList)
                    {
                        if (curItem.Points[curItem.Points.Count - 1].Y < 0)
                        {
                            curItem.IsVisible = false;
                        }
                    }
                    zedGraphControl1.RestoreScale(mPane1);
                    this.progressBar1.Value = 100;
                    m_isVisableDownStock = 0;
                    break;

                case 0:

                    if ((sender as ToolStripMenuItem).Text == "显示/隐藏弱势标的")
                    {
                        (sender as ToolStripMenuItem).Checked = true;
                    }

                    this.progressBar1.Value = 50;

                    GraphPane mPane2 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
                    CurveList curList2 = mPane2.CurveList;
                    foreach (CurveItem curItem in curList2)
                    {
                        curItem.IsVisible = true;
                    }
                    zedGraphControl1.RestoreScale(mPane2);
                    this.progressBar1.Value = 100;
                    m_isVisableDownStock = 1;
                    break;            
            }
        }
    }
}




