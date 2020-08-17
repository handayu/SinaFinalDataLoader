namespace TuShareLoader
{
    partial class FormWenHuaManger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox_Level2 = new System.Windows.Forms.ListBox();
            this.listBox_Level1 = new System.Windows.Forms.ListBox();
            this.richTextBox_Data = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox_ChengfenData = new System.Windows.Forms.ListBox();
            this.listBox_SelfBanKuai = new System.Windows.Forms.ListBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Remove = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Ok);
            this.groupBox1.Controls.Add(this.textBox_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-2, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(915, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文华安装路径";
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(356, 13);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(246, 23);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "确定";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.Button_Ok_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(87, 15);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(245, 21);
            this.textBox_path.TabIndex = 1;
            this.textBox_path.Text = "D:\\wh6上海中期\\Data\\\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文华路径:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox_Level2);
            this.groupBox2.Controls.Add(this.listBox_Level1);
            this.groupBox2.Controls.Add(this.richTextBox_Data);
            this.groupBox2.Location = new System.Drawing.Point(-2, 59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(653, 487);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文华数据汇总";
            // 
            // listBox_Level2
            // 
            this.listBox_Level2.BackColor = System.Drawing.SystemColors.Menu;
            this.listBox_Level2.FormattingEnabled = true;
            this.listBox_Level2.ItemHeight = 12;
            this.listBox_Level2.Location = new System.Drawing.Point(141, 20);
            this.listBox_Level2.Name = "listBox_Level2";
            this.listBox_Level2.Size = new System.Drawing.Size(128, 460);
            this.listBox_Level2.TabIndex = 4;
            // 
            // listBox_Level1
            // 
            this.listBox_Level1.BackColor = System.Drawing.SystemColors.Menu;
            this.listBox_Level1.FormattingEnabled = true;
            this.listBox_Level1.ItemHeight = 12;
            this.listBox_Level1.Location = new System.Drawing.Point(7, 21);
            this.listBox_Level1.Name = "listBox_Level1";
            this.listBox_Level1.Size = new System.Drawing.Size(128, 460);
            this.listBox_Level1.TabIndex = 3;
            this.listBox_Level1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Level1_MouseClick);
            // 
            // richTextBox_Data
            // 
            this.richTextBox_Data.BackColor = System.Drawing.SystemColors.Menu;
            this.richTextBox_Data.Location = new System.Drawing.Point(275, 20);
            this.richTextBox_Data.Name = "richTextBox_Data";
            this.richTextBox_Data.Size = new System.Drawing.Size(372, 461);
            this.richTextBox_Data.TabIndex = 2;
            this.richTextBox_Data.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox_ChengfenData);
            this.groupBox3.Controls.Add(this.listBox_SelfBanKuai);
            this.groupBox3.Controls.Add(this.button_Save);
            this.groupBox3.Controls.Add(this.button_Remove);
            this.groupBox3.Controls.Add(this.button_Add);
            this.groupBox3.Location = new System.Drawing.Point(658, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 486);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "自定义版块与成分(截面)";
            // 
            // listBox_ChengfenData
            // 
            this.listBox_ChengfenData.BackColor = System.Drawing.SystemColors.Menu;
            this.listBox_ChengfenData.FormattingEnabled = true;
            this.listBox_ChengfenData.ItemHeight = 12;
            this.listBox_ChengfenData.Location = new System.Drawing.Point(11, 198);
            this.listBox_ChengfenData.Name = "listBox_ChengfenData";
            this.listBox_ChengfenData.Size = new System.Drawing.Size(221, 232);
            this.listBox_ChengfenData.TabIndex = 6;
            // 
            // listBox_SelfBanKuai
            // 
            this.listBox_SelfBanKuai.BackColor = System.Drawing.SystemColors.Menu;
            this.listBox_SelfBanKuai.FormattingEnabled = true;
            this.listBox_SelfBanKuai.ItemHeight = 12;
            this.listBox_SelfBanKuai.Location = new System.Drawing.Point(9, 20);
            this.listBox_SelfBanKuai.Name = "listBox_SelfBanKuai";
            this.listBox_SelfBanKuai.Size = new System.Drawing.Size(221, 172);
            this.listBox_SelfBanKuai.TabIndex = 5;
            this.listBox_SelfBanKuai.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelBanKuai_MouseUp);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(122, 447);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(108, 32);
            this.button_Save.TabIndex = 4;
            this.button_Save.Text = "保存配置退出";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(64, 448);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(52, 32);
            this.button_Remove.TabIndex = 3;
            this.button_Remove.Text = "移除";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.Button_Remove_Click);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(9, 448);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(49, 32);
            this.button_Add.TabIndex = 2;
            this.button_Add.Text = "添加";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.Button_Add_Click);
            // 
            // FormWenHuaManger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 558);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormWenHuaManger";
            this.Text = "文华数据与自定义版块管理";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBox_Data;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.ListBox listBox_Level1;
        private System.Windows.Forms.ListBox listBox_Level2;
        private System.Windows.Forms.ListBox listBox_ChengfenData;
        private System.Windows.Forms.ListBox listBox_SelfBanKuai;
    }
}