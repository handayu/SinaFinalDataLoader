﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}

