using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuShareLoader
{
    public partial class FormAddSelBanKuai : Form
    {
        private string m_bankuaiName = string.Empty;

        public string BanKuaiName
        {
            get
            {
                return m_bankuaiName;
            }
        }


        public FormAddSelBanKuai()
        {
            InitializeComponent();
        }

        private void Button_ok_Click(object sender, EventArgs e)
        {
            this.m_bankuaiName = this.textBox1.Text.Replace("\0", "").Trim();
            this.Close();
        }
    }
}
