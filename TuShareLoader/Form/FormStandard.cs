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
    public partial class FormStandard : Form
    {
        public FormStandard()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //加载Debug-txt说明-编制便准和筛选标准思想
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Standard.txt";

            List<string> infoList = new List<string>();

            //设置文件共享方式为读写，FileShare.ReadWrite，这样的话，就可以打开了
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                if (line != "")
                    this.richTextBox1.AppendText(line.ToString() + "\n");
            }
        }
    }
}

