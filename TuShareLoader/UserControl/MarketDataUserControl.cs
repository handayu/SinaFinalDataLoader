using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace TuShareLoader
{
    public partial class MarketDataUserControl : UserControl
    {
        /// <summary>
        /// 板块名称-板块个股列表
        /// </summary>
        private BindingList<BanKuaiDta> m_banKuaiList = new BindingList<BanKuaiDta>();

        /// <summary>
        /// 板块点击事件
        /// </summary>
        /// <param name="data"></param>
        public delegate void BanKuaiHandle(BanKuaiDta data);
        public event BanKuaiHandle BanKuaiEvent;

        /// <summary>
        /// 板块强度列表
        /// </summary>
        private BindingList<BanKuaiQiangdu> m_banKuaiQiangDuList = new BindingList<BanKuaiQiangdu>();

        /// <summary>
        /// 抛出，显示我正在看哪一个板块
        /// </summary>
        /// <param name="data"></param>
        public delegate void BanKuaiSycHandle(string bankuaiName);
        public event BanKuaiSycHandle BanKuaiSycEvent;

        public MarketDataUserControl()
        {
            InitializeComponent();
            this.dataGridView1.DataSource = m_banKuaiList;
            //
            this.dataGridView_BankuaiList.DataSource = m_banKuaiQiangDuList;
            //
            GraphPane mPane = zedGraphControl2.GraphPane;//获取索引到GraphPane面板上
            mPane.XAxis.Title.Text = "Daily";//X轴标题
            mPane.YAxis.Title.Text = "Trend%";//Y轴标题
            //
            mPane.Title.Text = "板块强弱[上涨过60的股票占比]";

            mPane.XAxis.Type = ZedGraph.AxisType.LinearAsOrdinal;//出现图表右侧出现空白的情况....
            mPane.XAxis.CrossAuto = true;//容许x轴的自动放大或缩小
            mPane.YAxis.MajorGrid.IsVisible = true;//设置虚线.
            zedGraphControl2.IsShowHScrollBar = true;//不显示水平滚动条
            zedGraphControl2.PanModifierKeys = Keys.None;//鼠标拖拽可移动
            mPane.Legend.IsVisible = false;//不显示Lenged，因为当股票很多的时候，下面的图表会被压缩到一点点没有。

            DataGridView gridView = this.dataGridView_BankuaiList;
            gridView.Columns[1].SortMode = DataGridViewColumnSortMode.Automatic;
        }

        public void SubScribe()
        {
            //1.获取所有的板块和股票数据
            List<BanKuaiDta> handleBankuaiStocksDic = DataHandler.HandleBanKuaiAndStocks();

            //2.加载到datagrid上等待被Click
            foreach (BanKuaiDta kvP in handleBankuaiStocksDic)
            {
                BanKuaiDta dta = new BanKuaiDta()
                {
                    BanKuaiName = kvP.BanKuaiName,
                    BanKuaiStockList = kvP.BanKuaiStockList
                };
                m_banKuaiList.Add(dta);
            }
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.dataGridView1.Rows[e.RowIndex].DataBoundItem == null) return;

            BanKuaiDta dta = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as BanKuaiDta;

            if (BanKuaiEvent != null && dta != null)
            {
                //点击之后调用一下DataHandler完成数据整理和加入总Handle中
                DataHandler.BKStockDatList(dta);
                BanKuaiEvent(dta);

                //计算板块占比
                CalBanKuaiQaingRuoTrend(dta);

            }
        }

        /// <summary>
        /// 外部调用，直接打开所有板块数据
        /// </summary>
        public void CalAllBanKuaiData()
        {
            List<BanKuaiDta> dtaList = DataHandler.HoldBanKuaiAndStocks;
            foreach (BanKuaiDta dta in dtaList)
            {
                try
                {
                    if (BanKuaiEvent != null && dta != null)
                    {
                        //点击之后调用一下DataHandler完成数据整理和加入总Handle中
                        DataHandler.BKStockDatList(dta);
                        BanKuaiEvent(dta);

                        //计算板块占比
                        CalBanKuaiQaingRuoTrend(dta);

                        System.Threading.Thread.Sleep(1000 * 60);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 计算板块强弱[同一个板块内部，站上60均的个股数量占板块内所有股票数量的比例作为板块的强度,可以一直追踪]
        /// </summary>
        private void CalBanKuaiQaingRuoTrend(BanKuaiDta dta)
        {
            //板块1 --- 股票1  (1)--开
            //                --高
            //                --低
            //                --收
            //                (2)--开
            //                 --高
            //                 --低
            //                 --收
            //     --- 股票2
            //     --- 股票3
            //板块2....................
            //板块3....................

            GraphPane mPane = zedGraphControl2.GraphPane;//获取索引到GraphPane面板上

            //板块 - 股票 - 数据
            Dictionary<string, Dictionary<string, List<List<string>>>> banKuaiStockDataDic = DataHandler.BanKuaiStockDataDic;

            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> kv in banKuaiStockDataDic)
            {
                if (kv.Key != dta.BanKuaiName) continue;

                //如果是这个我选入的板块
                Dictionary<string, List<List<string>>> stockDataDic = kv.Value;

                PointPairList datalist = new PointPairList();

                //由于可能不能的票数据长度不一致(停牌等)，所以一律从最后的往前推100根K验证
                for (int pp = 0; pp < 100; pp++)
                {
                    double UpCreaseStocksNum = 0;
                    double x = 0.00;

                    foreach (KeyValuePair<string, List<List<string>>> kp in stockDataDic)
                    {
                        string stockCode = kp.Key;
                        List<List<string>> dataCountList = kp.Value;

                        if (dataCountList.Count < 100) continue;

                        double radio = 0.00;
                        double.TryParse(dataCountList[dataCountList.Count - 1 - pp][4], out radio);
                        if (radio >= 0)
                        {
                            UpCreaseStocksNum = UpCreaseStocksNum + 1;
                        }

                        double.TryParse(dataCountList[dataCountList.Count - 1 - pp][1], out x);
                    }

                    double Y = UpCreaseStocksNum / stockDataDic.Count * 100;

                    PointPair pairData = new PointPair();
                    pairData.X = x;
                    pairData.Y = Y;

                    datalist.Add(pairData);
                }

                datalist.Reverse();

                LineItem mCure = mPane.AddCurve(kv.Key, datalist, Common.GetRandomColor(), SymbolType.None);

                BanKuaiQiangdu qiangDu = new BanKuaiQiangdu()
                {
                    BanKuaiQiangduNum = datalist[datalist.Count - 1].Y,
                    BanKuaiName = kv.Key
                };

                //====================================//
                    m_banKuaiQiangDuList.Add(qiangDu);               
                //====================================//

                zedGraphControl2.AxisChange();//画到zedGraphControl1控件中，此句必加  
                zedGraphControl2.Refresh();
            }
        }

        /// <summary>
        /// 图表点击事件，查看曲线对应的板块图，激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pane"></param>
        /// <param name="curve"></param>
        /// <param name="iPt"></param>
        /// <returns></returns>
        private string Control_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            curve.IsSelectable = true;
            curve.IsSelected = true;
            PointPair pt = curve[iPt];

            if (BanKuaiSycEvent != null)
            {
                BanKuaiSycEvent(curve.Label.Text);
            }

            return curve.Label.Text + ":" + pt.X.ToString() + "/" + pt.Y.ToString();
        }

        private void ContextMenu_Click(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            ToolStripSeparator tPera = new ToolStripSeparator();
            menuStrip.Items.Add(tPera);

            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "显示/隐藏当前板块强弱列表";
            menuItem.Checked = true;

            menuItem.Click += MenuItem_Click;
            menuStrip.Items.Add(menuItem);
        }


        /// <summary>
        /// 默认为显示强-弱标的
        /// </summary>
        private int m_isVisableGrid = 1;
        private void MenuItem_Click(object sender, EventArgs e)
        {
            switch (m_isVisableGrid)
            {
                case 1:

                    //this.dataGridView_BankuaiList.Visible = false;
                    m_isVisableGrid = 0;
                    break;

                case 0:

                    //this.dataGridView_BankuaiList.Visible = true;
                    m_isVisableGrid = 1;
                    break;
            }
        }

        /// <summary>
        /// 板块管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WenHua_Click(object sender, EventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;

            //左右键切换
            if (MouseButtons.Right == args.Button)
            {
                //右键弹出文华板块管理终端
                FormWenHuaManger FManager = new FormWenHuaManger();
                FManager.Show();
            }
            else
            {
                //左键点击显示并打开KForm显示


            }
        }
    }
}

