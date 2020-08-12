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

        private List<LineItem> m_LineItemList = new List<LineItem>();

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
            StockListToChart(bankuaiDta);
        }

        public void StockListToChart(BanKuaiDta dta)
        {
            GraphPane mPane = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上

            //
            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> kv in DataHandler.BanKuaiStockDataDic)
            {
                if (kv.Key != dta.BanKuaiName) continue;

                Dictionary<string, List<List<string>>> stockDataDic = kv.Value;

                foreach (KeyValuePair<string, List<List<string>>> kp in stockDataDic)
                {
                    List<List<string>> dataCountList = kp.Value;

                    PointPairList dataList = new PointPairList();

                    for (int j = 0; j < dataCountList.Count; j++)
                    {
                        PointPair pairData = new PointPair();

                        double x = 0.00;
                        double.TryParse(kp.Value[j][1], out x);

                        double y = 0.00;
                        double.TryParse(kp.Value[j][4], out y);

                        pairData.X = x;
                        pairData.Y = y;

                        dataList.Add(pairData);
                    }

                    LineItem mCure = mPane.AddCurve(kp.Key, dataList, Common.GetRandomColor(), SymbolType.None);
                    m_LineItemList.Add(mCure);
                    //LineItem line = (LineItem)mCure;
                    //line.Line.Width = 200;
                    //line.Line.IsSmooth = true;
                    zedGraphControl1.AxisChange();//画到zedGraphControl1控件中，此句必加        

                }
            }
        }

        private string zGCDateChart_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            GraphPane mPane = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上

            curve.IsSelectable = true;
            curve.IsSelected = true;
            PointPair pt = curve[iPt];

            foreach(LineItem item in m_LineItemList)
            {
                if(item.Label.Text != curve.Label.Text)
                {
                    item.Line.Width = 1;
                    item.Line.IsSmooth = true;
                }

                if (item.Label.Text == curve.Label.Text)
                {
                    item.Line.Width = 5;
                    item.Line.IsSmooth = true;
                }
            }

            zedGraphControl1.Invalidate();

            return curve.Label.Text + ":" + pt.X.ToString() + "/" + pt.Y.ToString();

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
            //ToolStripSeparator tPera = new ToolStripSeparator();
            //menuStrip.Items.Add(tPera);

            //ToolStripMenuItem menuItem = new ToolStripMenuItem();
            //menuItem.Text = "显示/隐藏弱势标的";
            //menuItem.Checked = true;

            //menuItem.Click += MenuItem_Click;
            //menuStrip.Items.Add(menuItem);

        }

        /// <summary>
        /// 显示板块所有的品种 
        /// </summary>
        public void YesVisualAllStocks()
        {
            this.progressBar1.Value = 50;

            GraphPane mPane2 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
            CurveList curList2 = mPane2.CurveList;
            foreach (CurveItem curItem in curList2)
            {
                curItem.IsVisible = true;
            }
            zedGraphControl1.Refresh();
        }

        /// <summary>
        /// 只显示部分强势的品种 
        /// </summary>
        public void NoVisualPartStocks()
        {
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
            zedGraphControl1.Refresh();
            this.progressBar1.Value = 100;
        }

        /// <summary>
        /// 只显示强前五的品种
        /// </summary>
        public void VisualJustMaxMinStrongStocks()
        {
            this.progressBar1.Value = 50;

            //先把所有的曲线隐藏
            GraphPane mPane2 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
            CurveList curList2 = mPane2.CurveList;
            foreach (CurveItem curItem in curList2)
            {
                curItem.IsVisible = false;
            }

            //把所有的最后的板块内部的股票数据都加入Dic中便于之后排序取前五
            Dictionary<string, double> curveNameValueList = new Dictionary<string, double>();
            GraphPane mPane1 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
            CurveList curList = mPane1.CurveList;
            foreach (CurveItem curItem in curList)
            {
                if (curItem.Points[curItem.Points.Count - 1].Y >= 0)
                {
                    curveNameValueList.Add(curItem.Label.Text, curItem.Points[curItem.Points.Count - 1].Y);
                }
            }

            //排序-LinQ按照Dic的Value值进行排序-OrderByDescending降序
            Dictionary<string, double> sortedByValueList = curveNameValueList.OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);

            //小于五个的全部显示，大于五个的，只显示五个
            if (sortedByValueList.Count >= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    string cULabel = sortedByValueList.ElementAt(i).Key;

                    GraphPane mPane3 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
                    CurveList curList3 = mPane3.CurveList;
                    foreach (CurveItem curItem in curList3)
                    {
                        if (curItem.Label.Text.CompareTo(cULabel) == 0)
                        {
                            curItem.IsVisible = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, double> kpiar in sortedByValueList)
                {
                    string cULabel = kpiar.Key;

                    GraphPane mPane3 = zedGraphControl1.GraphPane;//获取索引到GraphPane面板上
                    CurveList curList3 = mPane3.CurveList;
                    foreach (CurveItem curItem in curList3)
                    {
                        if (curItem.Label.Text.CompareTo(cULabel) == 0)
                        {
                            curItem.IsVisible = true;
                            break;
                        }
                    }
                }

            }


            zedGraphControl1.Refresh();
            this.progressBar1.Value = 100;
        }
    }
}







