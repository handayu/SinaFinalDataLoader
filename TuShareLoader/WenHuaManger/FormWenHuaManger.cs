using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuShareLoader
{
    public partial class FormWenHuaManger : Form
    {
        private Dictionary<string, List<string>> m_bankuaiAndSelGegu = new Dictionary<string, List<string>>();

        public FormWenHuaManger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存路径到配置，然后清空各个richBox，拉取各个数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Ok_Click(object sender, EventArgs e)
        {

            //保存配置


            //清空所有
            ClearBox();

            //添加数据
            string path = this.textBox_path.Text.Replace("\r\n", "");
            string[] diArr = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string str in diArr)
            {
                string fileNames = System.IO.Path.GetFileName(str);
                this.listBox_Level1.Items.Add(fileNames);
            }
        }

        private void ClearBox()
        {
            this.listBox_Level1.Items.Clear();
            this.listBox_Level2.Items.Clear();
            this.richTextBox_Data.Clear();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, EventArgs e)
        {
            if (this.listBox_Level1.SelectedItem == null ||
                this.listBox_Level1.SelectedItem.ToString() == "" ||
                this.listBox_Level2.SelectedItem == null ||
                this.listBox_Level2.SelectedItem.ToString() == "" ||
                this.listBox_SelfBanKuai.SelectedItem == null ||
                this.listBox_SelfBanKuai.SelectedItem.ToString() == ""
                )
            {
                MessageBox.Show("请选中一级市场板块，二级品种，以及自定义版块，再添加！");
                return;
            }


            if (this.listBox_Level1.SelectedItem != null &&
            this.listBox_Level1.SelectedItem.ToString() != "" &&
            this.listBox_Level2.SelectedItem != null &&
            this.listBox_Level2.SelectedItem.ToString() != "" &&
            this.listBox_SelfBanKuai.SelectedItem != null &&
            this.listBox_SelfBanKuai.SelectedItem.ToString() != ""
            )
            {
                foreach (KeyValuePair<string, List<string>> kv in m_bankuaiAndSelGegu)
                {
                    string bankuai = kv.Key;
                    if (bankuai != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;
                    List<string> geguList = kv.Value;
                    string path = this.textBox_path.Text.Replace("\r\n", "");
                    string pathData = path + this.listBox_Level1.SelectedItem.ToString() + "\\" + this.listBox_Level2.SelectedItem.ToString();

                    geguList.Add(pathData);

                    this.listBox_ChengfenData.Items.Add(pathData);
                }
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Remove_Click(object sender, EventArgs e)
        {

            if (this.listBox_ChengfenData.SelectedItem == null ||
            this.listBox_ChengfenData.SelectedItem.ToString() == "" ||
            this.listBox_SelfBanKuai.SelectedItem == null ||
            this.listBox_SelfBanKuai.SelectedItem.ToString() == "")
            {
                MessageBox.Show("请选择要移除的品种以及品种所属的板块！");
                return;
            }

            foreach (KeyValuePair<string, List<string>> kv in m_bankuaiAndSelGegu)
            {
                if (kv.Key != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;
                List<string> geguList = kv.Value;
                int index = 0;
                for (int i = 0;i<geguList.Count;i++)
                {
                    if (this.listBox_ChengfenData.SelectedItem!=null && geguList[i] != this.listBox_ChengfenData.SelectedItem.ToString()) continue;
                    this.listBox_ChengfenData.Items.Remove(this.listBox_ChengfenData.SelectedItem);
                    //从内存表中也要移除；
                    index = i;
                }
                geguList.RemoveAt(index);
            }
        }

        /// <summary>
        /// 保存退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, EventArgs e)
        {
            WenHuaSelConfig.Instance.BanKuaiAndGegu = m_bankuaiAndSelGegu;
            WenHuaSelConfig.Instance.Path = this.textBox_path.Text.Replace("\r\n", "");
        }

        /// <summary>
        /// 一级目录点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Level1_MouseClick(object sender, MouseEventArgs e)
        {
            //进去该目录下的cont.dat进行解析放入Level2
            try
            {
                this.listBox_Level2.Items.Clear();

                string path = this.textBox_path.Text.Replace("\r\n", "");
                string pathData = path + this.listBox_Level1.SelectedItem.ToString() + "\\cont.dat";
                List<string> level2List = WenHuaDataHandle.GetConDatData(pathData);
                foreach (string str in level2List)
                {
                    this.listBox_Level2.Items.Add(str);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SelBanKuai_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //右键弹出文华板块管理终端
                FormAddSelBanKuai bF = new FormAddSelBanKuai();
                bF.ShowDialog();

                if (bF.BanKuaiName == "") return;
                this.listBox_SelfBanKuai.Items.Add(bF.BanKuaiName);

                List<string> tempList = new List<string>();
                m_bankuaiAndSelGegu.Add(bF.BanKuaiName, tempList);

            }

            if (e.Button == MouseButtons.Left)
            {
                //选中显示个股
                this.listBox_ChengfenData.Items.Clear();
                foreach (KeyValuePair<string, List<string>> kv in m_bankuaiAndSelGegu)
                {
                    if (kv.Key != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;
                    List<string> geguList = kv.Value;
                    foreach(string gegu in geguList)
                    {
                        this.listBox_ChengfenData.Items.Add(gegu);
                    }
                }
            }
        }
    }
}
