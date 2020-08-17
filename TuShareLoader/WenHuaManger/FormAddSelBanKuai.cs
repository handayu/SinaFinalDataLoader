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
        private string m_selBanKuaiName = string.Empty;

        public string BanKuaiName
        {
            get
            {
                return m_selBanKuaiName;
            }
        }

        public FormAddSelBanKuai()
        {
            InitializeComponent();
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            this.m_selBanKuaiName = this.textBox_SelName.Text;
            this.Close();
        }
    }
}
