namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pb_Promgram = new ProgressBar();
            label3 = new Label();
            comboBox_device = new ComboBox();
            label4 = new Label();
            comboBox_index = new ComboBox();
            label5 = new Label();
            comboBox_channel = new ComboBox();
            btn_Program = new Button();
            btn_OpenDevice = new Button();
            btn_Connect = new Button();
            btn_CloseDevice = new Button();
            tb_PID = new TextBox();
            label6 = new Label();
            label7 = new Label();
            tb_FID = new TextBox();
            label8 = new Label();
            tb_RECID = new TextBox();
            label9 = new Label();
            tb_FilePath = new TextBox();
            groupBox1 = new GroupBox();
            bt_OpenFile = new Button();
            label10 = new Label();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            textBox1 = new TextBox();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // pb_Promgram
            // 
            pb_Promgram.Location = new Point(106, 86);
            pb_Promgram.Margin = new Padding(2);
            pb_Promgram.Name = "pb_Promgram";
            pb_Promgram.Size = new Size(805, 34);
            pb_Promgram.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 36);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 9;
            label3.Text = "设备类型";
            // 
            // comboBox_device
            // 
            comboBox_device.FormattingEnabled = true;
            comboBox_device.Location = new Point(108, 30);
            comboBox_device.Margin = new Padding(2);
            comboBox_device.Name = "comboBox_device";
            comboBox_device.Size = new Size(189, 32);
            comboBox_device.TabIndex = 8;
            comboBox_device.SelectedIndexChanged += comboBox_device_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(334, 36);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(82, 24);
            label4.TabIndex = 11;
            label4.Text = "设备索引";
            // 
            // comboBox_index
            // 
            comboBox_index.FormattingEnabled = true;
            comboBox_index.Location = new Point(423, 30);
            comboBox_index.Margin = new Padding(2);
            comboBox_index.Name = "comboBox_index";
            comboBox_index.Size = new Size(189, 32);
            comboBox_index.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(643, 36);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 13;
            label5.Text = "通道";
            // 
            // comboBox_channel
            // 
            comboBox_channel.FormattingEnabled = true;
            comboBox_channel.Location = new Point(721, 32);
            comboBox_channel.Margin = new Padding(2);
            comboBox_channel.Name = "comboBox_channel";
            comboBox_channel.Size = new Size(189, 32);
            comboBox_channel.TabIndex = 12;
            // 
            // btn_Program
            // 
            btn_Program.Location = new Point(491, 30);
            btn_Program.Margin = new Padding(2);
            btn_Program.Name = "btn_Program";
            btn_Program.Size = new Size(189, 53);
            btn_Program.TabIndex = 14;
            btn_Program.Text = "编程";
            btn_Program.UseVisualStyleBackColor = true;
            btn_Program.Click += btn_Program_Click;
            // 
            // btn_OpenDevice
            // 
            btn_OpenDevice.Location = new Point(22, 30);
            btn_OpenDevice.Margin = new Padding(2);
            btn_OpenDevice.Name = "btn_OpenDevice";
            btn_OpenDevice.Size = new Size(189, 53);
            btn_OpenDevice.TabIndex = 15;
            btn_OpenDevice.Text = "打开";
            btn_OpenDevice.UseVisualStyleBackColor = true;
            btn_OpenDevice.Click += btn_OpenDevice_Click;
            // 
            // btn_Connect
            // 
            btn_Connect.Location = new Point(257, 30);
            btn_Connect.Margin = new Padding(2);
            btn_Connect.Name = "btn_Connect";
            btn_Connect.Size = new Size(189, 53);
            btn_Connect.TabIndex = 16;
            btn_Connect.Text = "连接";
            btn_Connect.UseVisualStyleBackColor = true;
            btn_Connect.Click += btn_Connect_Click;
            // 
            // btn_CloseDevice
            // 
            btn_CloseDevice.Location = new Point(726, 30);
            btn_CloseDevice.Margin = new Padding(2);
            btn_CloseDevice.Name = "btn_CloseDevice";
            btn_CloseDevice.Size = new Size(189, 53);
            btn_CloseDevice.TabIndex = 17;
            btn_CloseDevice.Text = "复位";
            btn_CloseDevice.UseVisualStyleBackColor = true;
            btn_CloseDevice.Click += btn_CloseDevice_Click;
            // 
            // tb_PID
            // 
            tb_PID.Location = new Point(108, 80);
            tb_PID.Margin = new Padding(4);
            tb_PID.Name = "tb_PID";
            tb_PID.Size = new Size(189, 30);
            tb_PID.TabIndex = 18;
            tb_PID.Text = " ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 84);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(40, 24);
            label6.TabIndex = 19;
            label6.Text = "PID";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(336, 84);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(39, 24);
            label7.TabIndex = 21;
            label7.Text = "FID";
            // 
            // tb_FID
            // 
            tb_FID.Location = new Point(423, 80);
            tb_FID.Margin = new Padding(4);
            tb_FID.Name = "tb_FID";
            tb_FID.Size = new Size(189, 30);
            tb_FID.TabIndex = 20;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(643, 84);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(68, 24);
            label8.TabIndex = 23;
            label8.Text = "REC ID";
            // 
            // tb_RECID
            // 
            tb_RECID.Location = new Point(721, 83);
            tb_RECID.Margin = new Padding(4);
            tb_RECID.Name = "tb_RECID";
            tb_RECID.Size = new Size(189, 30);
            tb_RECID.TabIndex = 22;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(20, 35);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(46, 24);
            label9.TabIndex = 24;
            label9.Text = "程序";
            // 
            // tb_FilePath
            // 
            tb_FilePath.Location = new Point(106, 31);
            tb_FilePath.Margin = new Padding(4);
            tb_FilePath.Name = "tb_FilePath";
            tb_FilePath.Size = new Size(729, 30);
            tb_FilePath.TabIndex = 25;
            // 
            // groupBox1
            // 
            groupBox1.Location = new Point(957, 14);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(306, 377);
            groupBox1.TabIndex = 26;
            groupBox1.TabStop = false;
            groupBox1.Text = "说明";
            // 
            // bt_OpenFile
            // 
            bt_OpenFile.Location = new Point(842, 31);
            bt_OpenFile.Margin = new Padding(2);
            bt_OpenFile.Name = "bt_OpenFile";
            bt_OpenFile.Size = new Size(68, 32);
            bt_OpenFile.TabIndex = 27;
            bt_OpenFile.Text = "······";
            bt_OpenFile.UseVisualStyleBackColor = true;
            bt_OpenFile.Click += bt_OpenFile_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 91);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(46, 24);
            label10.TabIndex = 28;
            label10.Text = "进度";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btn_Connect);
            groupBox2.Controls.Add(btn_Program);
            groupBox2.Controls.Add(btn_OpenDevice);
            groupBox2.Controls.Add(btn_CloseDevice);
            groupBox2.Location = new Point(15, 14);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(935, 95);
            groupBox2.TabIndex = 27;
            groupBox2.TabStop = false;
            groupBox2.Text = "命令";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBox_device);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(comboBox_index);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(comboBox_channel);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(tb_PID);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(tb_RECID);
            groupBox3.Controls.Add(tb_FID);
            groupBox3.Controls.Add(label7);
            groupBox3.Location = new Point(16, 110);
            groupBox3.Margin = new Padding(4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4);
            groupBox3.Size = new Size(935, 133);
            groupBox3.TabIndex = 27;
            groupBox3.TabStop = false;
            groupBox3.Text = "设备信息";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tb_FilePath);
            groupBox4.Controls.Add(pb_Promgram);
            groupBox4.Controls.Add(label9);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(bt_OpenFile);
            groupBox4.Location = new Point(16, 246);
            groupBox4.Margin = new Padding(4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(4);
            groupBox4.Size = new Size(934, 145);
            groupBox4.TabIndex = 27;
            groupBox4.TabStop = false;
            groupBox4.Text = "编程信息";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(15, 415);
            textBox1.Margin = new Padding(4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1247, 249);
            textBox1.TabIndex = 28;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 679);
            Controls.Add(textBox1);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Form1";
            Text = "下载器";
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ProgressBar pb_Promgram;
        private Label label3;
        private ComboBox comboBox_device;
        private Label label4;
        private ComboBox comboBox_index;
        private Label label5;
        private ComboBox comboBox_channel;
        private Button btn_Program;
        private Button btn_OpenDevice;
        private Button btn_Connect;
        private Button btn_CloseDevice;
        private TextBox tb_PID;
        private Label label6;
        private Label label7;
        private TextBox tb_FID;
        private Label label8;
        private TextBox tb_RECID;
        private Label label9;
        private TextBox tb_FilePath;
        private GroupBox groupBox1;
        private Button bt_OpenFile;
        private Label label10;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private TextBox textBox1;
    }
}
