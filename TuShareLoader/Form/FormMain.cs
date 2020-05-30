using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TuShareLoader
{
    public partial class FormMain : DockContent
    {

        private MarketDataUserForm m_marketDataForm = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form_load(object sender, EventArgs e)
        {

            foreach (Control c in this.Controls)
            {
                if (c is MdiClient)
                {
                    c.BackColor = Color.White; //颜色 
                }
            }


            //生成所有的有业务对象-然后再在这里管理所有的事件通知
            m_marketDataForm = new MarketDataUserForm();
            m_marketDataForm.Show(dockPanel1, DockState.DockLeft);

            //行情点击事件订阅-生成窗口展示
            m_marketDataForm.MarketDataUserControlSelf.BanKuaiEvent += MarketDataUserControlSelf_BanKuaiEvent; ;
            m_marketDataForm.MarketDataUserControlSelf.BanKuaiSycEvent += MarketDataUserControlSelf_BanKuaiSycEvent;

        }

        private void MarketDataUserControlSelf_BanKuaiSycEvent(string bankuaiName)
        {
            foreach (Form f in m_BanKuaiFormList)
            {
                if ((f as Form1) != null && (f as Form1).Text == bankuaiName)
                {
                    (f as Form1).Activate();
                    break;
                }
            }
        }

        private int m_formNum = 0;
        private List<Form1> m_BanKuaiFormList = new List<Form1>();

        private void MarketDataUserControlSelf_BanKuaiEvent(BanKuaiDta data)
        {
            Form1 form = new Form1(data);
            form.TopLevel = false;//设置为非顶级控件
            form.MdiParent = this;
            form.Show();

            m_BanKuaiFormList.Add(form);

            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        /// <summary>
        /// K线窗口关闭删除事件--清除List的存在的KLine
        /// </summary>
        /// <param name="t"></param>
        private void DeleteKLineFormSubEvent(string formName)
        {
            //if (formName == null || formName == "") return;

            //Form reForm = null;
            //foreach (Form f in m_OpenKLineFormList)
            //{
            //    if ((f as KLineFormTest) != null && (f as KLineFormTest).FORMNAME.CompareTo(formName) == 0)
            //    {
            //        reForm = f;
            //        break;
            //    }
            //}

            //if (reForm == null) return;
            //m_OpenKLineFormList.Remove(reForm);
        }

        #region VH布局管理
        private void toolStripButton_VLayout_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);

        }

        private void toolStripButton_SLayOut_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void toolStripButton_HLayout_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        #endregion

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            if (m_marketDataForm != null && m_marketDataForm.MarketDataUserControlSelf != null)
            {
                m_marketDataForm.MarketDataUserControlSelf.BanKuaiEvent -= MarketDataUserControlSelf_BanKuaiEvent;
            }

            m_marketDataForm = null;

        }

        private void HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        /// <summary>
        /// 默认为显示强-弱标的
        /// </summary>
        private int m_isAllVisableDownStock = 1;

        /// <summary>
        /// 全部显示或隐藏弱势品种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_AllVisualClick(object sender, EventArgs e)
        {
            switch (m_isAllVisableDownStock)
            {
                case 1:
                    foreach(Form1 f in m_BanKuaiFormList)
                    {
                        f.NoVisualPartStocks();
                    }

                    this.ToolStripMenuItem_AllVisual.Checked = false;
                    m_isAllVisableDownStock = 0;
                    break;

                case 0:
                    foreach (Form1 f in m_BanKuaiFormList)
                    {
                        f.YesVisualAllStocks();
                    }

                    this.ToolStripMenuItem_AllVisual.Checked = true;
                    m_isAllVisableDownStock = 1;
                    break;
            }
        }

        private void ToolStripMenuItem_HoldAllClick(object sender, EventArgs e)
        {
            this.m_marketDataForm.MarketDataUserControlSelf.CalAllBanKuaiData();
        }

        /// <summary>
        /// 显示最强和最弱的五只进行龙头跟踪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_ViewMaxMinFiveStockClick(object sender, EventArgs e)
        {
            foreach (Form1 f in m_BanKuaiFormList)
            {
                f.VisualJustMaxMinStrongStocks();
            }
        }

        private void ToolStripMenuItem_StanderdClick(object sender, EventArgs e)
        {
            FormStandard s = new FormStandard();
            s.Show();
        }

        /// <summary>
        /// 将toShare-Api查询回来的数据全部输入到Dic-Files中
        /// 每一个板块名称是一个文件夹 - 每个文件夹下每只个股有自己的独立的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void apiToFileDisToolStripMenuItemClick(object sender, EventArgs e)
        {
            List<BanKuaiDta>  banKuaiAndStockinfos = DataHandler.HandleBanKuaiAndStocks();
            foreach(BanKuaiDta dataInfo in banKuaiAndStockinfos)
            {
                string bankuaiName = dataInfo.BanKuaiName;
                List<string> stockCodeList = dataInfo.BanKuaiStockList;

                string dir = AppDomain.CurrentDomain.BaseDirectory + "A-StockData" + "/" + bankuaiName;
                //每个板块创建一个文件夹
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                //每个板块的文件夹下每个个股有一个txt数据
                foreach(string code in stockCodeList)
                {
                    //剪切掉.SH .SZ因为文件不允许这样命名
                    string[] codeInfoArray = code.Split('.');
                    string codeRl = codeInfoArray[0];
                    string stockCodeDir = AppDomain.CurrentDomain.BaseDirectory + "A-StockData" + "/" + bankuaiName + "/" + codeRl + ".txt";
                    if (!File.Exists(stockCodeDir)) File.Create(stockCodeDir);
                }
            }

            //写入txt每只个股的数据
            //数据结构：板块 - 个股 - 个股数据
            Dictionary<string, Dictionary<string, List<List<string>>>> banKuaiStockDataDic = DataHandler.BanKuaiStockDataDic;


        }
    }
}



	

