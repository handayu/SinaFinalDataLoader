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
                foreach(KeyValuePair<string,Dictionary<string,string>> kv in DatDataManager.Instance.BankuaiGeguPathDic)
                {
                    if (kv.Key != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;

                    Level2Info infos = this.listBox_Level2.SelectedItem as Level2Info;

                    Dictionary<string, string> geguNamePathStr = kv.Value;
                    geguNamePathStr.Add(infos.Instrument.Replace("\0","").Trim(),
                                    this.textBox_path.Text.Replace("\r\n", "") + this.listBox_Level1.SelectedItem.ToString()
                                    + "\\" + "day\\" + infos.DataCodeStr + ".dat");

                    this.listBox_ChengfenData.Items.Add(infos.Instrument + " " +
                                    this.textBox_path.Text.Replace("\r\n", "")  + this.listBox_Level1.SelectedItem.ToString()
                                    + "\\" + "day\\" + infos.DataCodeStr + ".dat");
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

            //if (this.listBox_ChengfenData.SelectedItem == null ||
            //this.listBox_ChengfenData.SelectedItem.ToString() == "" ||
            //this.listBox_SelfBanKuai.SelectedItem == null ||
            //this.listBox_SelfBanKuai.SelectedItem.ToString() == "")
            //{
            //    MessageBox.Show("请选择要移除的品种以及品种所属的板块！");
            //    return;
            //}

            //foreach (KeyValuePair<string, Dictionary<string,string>> kv in DatDataManager.Instance.BankuaiGeguPathDic)
            //{
            //    if (kv.Key != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;
            //    Dictionary<string, string> gegupair = kv.Value;
            //    object itemSel = null;
            //    foreach(KeyValuePair<string,string> kv2 in gegupair)
            //    {
            //        if(this.listBox_ChengfenData.SelectedItem.ToString().Contains(kv2.Key))
            //        {
            //            itemSel = this.listBox_ChengfenData.SelectedItem;
            //            gegupair.Remove(kv2.Key);
            //        }
            //    }
            //    if(itemSel != null) this.listBox_ChengfenData.Items.Remove(itemSel);
            //}
        }

        /// <summary>
        /// 保存退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, EventArgs e)
        {
            this.Close();
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
                List<Level2Info> level2List = WenHuaDataHandle.GetConDatData(pathData);
                foreach (Level2Info info in level2List)
                {
                    this.listBox_Level2.Items.Add(info);
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

                Dictionary<string, string> dic = new Dictionary<string, string>();
                DatDataManager.Instance.BankuaiGeguPathDic.Add(bF.BanKuaiName, dic);

            }

            if (e.Button == MouseButtons.Left)
            {
                //选中显示个股
                this.listBox_ChengfenData.Items.Clear();
                foreach (KeyValuePair<string, Dictionary<string,string>> kv in DatDataManager.Instance.BankuaiGeguPathDic)
                {
                    if (this.listBox_SelfBanKuai.SelectedItem != null && kv.Key != this.listBox_SelfBanKuai.SelectedItem.ToString()) continue;
                    Dictionary<string, string> geguList = kv.Value;
                    foreach (KeyValuePair<string,string> gegu in geguList)
                    {
                        this.listBox_ChengfenData.Items.Add(gegu.Key + " " + gegu.Value);
                    }
                }
            }
        }

        private void Level2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //选中显示个股
                this.richTextBox_Data.Clear();

                if (this.listBox_Level1.SelectedItem == null ||
                    this.listBox_Level1.SelectedItem.ToString() == "" ||
                    this.listBox_Level2.SelectedItem == null ||
                    this.listBox_Level2.SelectedItem.ToString() == "") return;

                Level2Info infos = this.listBox_Level2.SelectedItem as Level2Info;

                string fileInfos = this.textBox_path.Text.Replace("\r\n", "") + this.listBox_Level1.SelectedItem.ToString()
                                + "\\" + "day\\" + infos.DataCodeStr + ".dat";

                //获取数据
                if(!File.Exists(fileInfos))
                {
                    MessageBox.Show("该行情文件不存在,请刷新文华相关合约的日线数据！");
                }
                else
                {
                    List<MarketData> wDList = WenHuaDataHandle.GetHQDatData(fileInfos);
                    //因为数据可能很多，所以只展示100条
                    for(int i = wDList.Count - 1;i >= wDList.Count - 50;i--)
                    {
                        this.richTextBox_Data.AppendText(wDList[i].DateTimeNum + "|" + wDList[i].Close + "\n");
                    }
                }
            }
        }
    }
}
