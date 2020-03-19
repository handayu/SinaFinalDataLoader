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

namespace TuShareLoader
{
    public partial class MarketDataUserControl : UserControl
    {
        private BindingList<BanKuaiDta> m_banKuaiList = new BindingList<BanKuaiDta>();

        private Dictionary<string, List<string>> m_stockDic = new Dictionary<string, List<string>>();

        public delegate void BanKuaiHandle(BanKuaiDta data);
        public event BanKuaiHandle BanKuaiEvent;

        public MarketDataUserControl()
        {
            InitializeComponent();

            this.dataGridView1.DataSource = m_banKuaiList;

        }

        public void SubScribe()
        {
            //1.获取所有的板块和股票数据
            string filePath = @"C:\Users\Administrator\Desktop\SinaFinalDataLoader\TuShareLoader\bin\Debug\A股板块";
            string[] files = Directory.GetFiles(filePath, "*.txt");

            foreach (string file in files)
            {

                string fileName = Path.GetFileNameWithoutExtension(file);

                List<string> stockList = new List<string>();

                //设置文件共享方式为读写，FileShare.ReadWrite，这样的话，就可以打开了
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    stockList.Add(line.ToString());
                }

                m_stockDic.Add(fileName, stockList);
            }

            //2.加载到datagrid上等待被Click
            foreach(KeyValuePair<string,List<string>> kvP in m_stockDic)
            {
                BanKuaiDta dta = new BanKuaiDta()
                {
                    BanKuaiName = kvP.Key,
                    BanKuaiStockList = kvP.Value
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
                BanKuaiEvent(dta);
            }

        }
    }
}
