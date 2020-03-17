using System;
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

    }
}
