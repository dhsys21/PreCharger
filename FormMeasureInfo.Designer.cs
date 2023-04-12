namespace PreCharger
{
    partial class FormMeasureInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pBase = new System.Windows.Forms.Panel();
            this.gridView = new System.Windows.Forms.DataGridView();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnProbeOpen = new System.Windows.Forms.Button();
            this.btnProbeClose = new System.Windows.Forms.Button();
            this.btnRunStart = new System.Windows.Forms.Button();
            this.btnRunStop = new System.Windows.Forms.Button();
            this.gbManualMode = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnDischargingStart = new System.Windows.Forms.Button();
            this.btnDischargingStop = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnInit = new System.Windows.Forms.Button();
            this.lblTestTime = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnMeasureStart = new System.Windows.Forms.Button();
            this.btnMeasureStop = new System.Windows.Forms.Button();
            this.pBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbManualMode.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.BackColor = System.Drawing.Color.Black;
            this.pBase.Controls.Add(this.gridView);
            this.pBase.Controls.Add(this.tbTime);
            this.pBase.Location = new System.Drawing.Point(23, 100);
            this.pBase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pBase.Name = "pBase";
            this.pBase.Padding = new System.Windows.Forms.Padding(2);
            this.pBase.Size = new System.Drawing.Size(1198, 1018);
            this.pBase.TabIndex = 9;
            // 
            // gridView
            // 
            this.gridView.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.gridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridView.BackgroundColor = System.Drawing.Color.White;
            this.gridView.ColumnHeadersHeight = 29;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView.Location = new System.Drawing.Point(2, 2);
            this.gridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridView.Name = "gridView";
            this.gridView.RowHeadersWidth = 51;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.gridView.RowTemplate.Height = 23;
            this.gridView.Size = new System.Drawing.Size(1194, 1014);
            this.gridView.TabIndex = 0;
            // 
            // tbTime
            // 
            this.tbTime.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbTime.Location = new System.Drawing.Point(1006, 494);
            this.tbTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTime.Name = "tbTime";
            this.tbTime.Size = new System.Drawing.Size(85, 42);
            this.tbTime.TabIndex = 10;
            this.tbTime.Text = "30";
            this.tbTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.LightBlue;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(23, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(221, 50);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Text = "STAGE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Location = new System.Drawing.Point(1361, 22);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 50);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 22);
            this.label1.TabIndex = 26;
            this.label1.Text = "voltage";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightPink;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(74, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 22);
            this.label2.TabIndex = 27;
            this.label2.Text = "current";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Silver;
            this.label17.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(145, 3);
            this.label17.Margin = new System.Windows.Forms.Padding(1);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 22);
            this.label17.TabIndex = 35;
            this.label17.Text = "No Cell";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Red;
            this.label18.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(216, 3);
            this.label18.Margin = new System.Windows.Forms.Padding(1);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 22);
            this.label18.TabIndex = 34;
            this.label18.Text = "ERROR";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Yellow;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(287, 3);
            this.label13.Margin = new System.Windows.Forms.Padding(1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 22);
            this.label13.TabIndex = 36;
            this.label13.Text = "Cell Flow";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label17);
            this.flowLayoutPanel1.Controls.Add(this.label18);
            this.flowLayoutPanel1.Controls.Add(this.label13);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(23, 68);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(361, 30);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(2, 112);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 22);
            this.label3.TabIndex = 26;
            this.label3.Text = "0x45 : Sensing Over Range";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(2, 200);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(250, 22);
            this.label4.TabIndex = 27;
            this.label4.Text = "0xD0 : Limit Voltage";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(2, 178);
            this.label5.Margin = new System.Windows.Forms.Padding(1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(250, 22);
            this.label5.TabIndex = 35;
            this.label5.Text = "0x48 : DCUN Error";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(2, 156);
            this.label6.Margin = new System.Windows.Forms.Padding(1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 22);
            this.label6.TabIndex = 34;
            this.label6.Text = "0x47 : Sensing Time Out";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(2, 134);
            this.label7.Margin = new System.Windows.Forms.Padding(1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 22);
            this.label7.TabIndex = 36;
            this.label7.Text = "0x46 : Sensing No Ref";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(2, 90);
            this.label9.Margin = new System.Windows.Forms.Padding(1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(250, 22);
            this.label9.TabIndex = 38;
            this.label9.Text = "0x42 : OVP ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(2, 24);
            this.label10.Margin = new System.Windows.Forms.Padding(1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(250, 22);
            this.label10.TabIndex = 39;
            this.label10.Text = "0x06 : Disable";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(2, 2);
            this.label11.Margin = new System.Windows.Forms.Padding(1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(250, 22);
            this.label11.TabIndex = 40;
            this.label11.Text = "0x05 : NO CELL (Contact Err)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(2, 46);
            this.label12.Margin = new System.Windows.Forms.Padding(1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(250, 22);
            this.label12.TabIndex = 41;
            this.label12.Text = "0x40 : COMM Error (No Status)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(2, 68);
            this.label8.Margin = new System.Windows.Forms.Padding(1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(250, 22);
            this.label8.TabIndex = 37;
            this.label8.Text = "0x41 : OCP 0x42 : OVP ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(1227, 864);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(258, 254);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ERROR CODE";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 21);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(254, 231);
            this.panel1.TabIndex = 0;
            // 
            // btnProbeOpen
            // 
            this.btnProbeOpen.Location = new System.Drawing.Point(3, 44);
            this.btnProbeOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnProbeOpen.Name = "btnProbeOpen";
            this.btnProbeOpen.Size = new System.Drawing.Size(114, 75);
            this.btnProbeOpen.TabIndex = 2;
            this.btnProbeOpen.Text = "OPEN";
            this.btnProbeOpen.UseVisualStyleBackColor = true;
            this.btnProbeOpen.Click += new System.EventHandler(this.btnProbeOpen_Click);
            // 
            // btnProbeClose
            // 
            this.btnProbeClose.Location = new System.Drawing.Point(124, 44);
            this.btnProbeClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnProbeClose.Name = "btnProbeClose";
            this.btnProbeClose.Size = new System.Drawing.Size(114, 75);
            this.btnProbeClose.TabIndex = 1;
            this.btnProbeClose.Text = "CLOSE";
            this.btnProbeClose.UseVisualStyleBackColor = true;
            this.btnProbeClose.Click += new System.EventHandler(this.btnProbeClose_Click);
            // 
            // btnRunStart
            // 
            this.btnRunStart.Location = new System.Drawing.Point(3, 46);
            this.btnRunStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunStart.Name = "btnRunStart";
            this.btnRunStart.Size = new System.Drawing.Size(114, 75);
            this.btnRunStart.TabIndex = 1;
            this.btnRunStart.Text = "START";
            this.btnRunStart.UseVisualStyleBackColor = true;
            this.btnRunStart.Click += new System.EventHandler(this.btnRunStart_Click);
            // 
            // btnRunStop
            // 
            this.btnRunStop.Location = new System.Drawing.Point(124, 46);
            this.btnRunStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunStop.Name = "btnRunStop";
            this.btnRunStop.Size = new System.Drawing.Size(114, 75);
            this.btnRunStop.TabIndex = 2;
            this.btnRunStop.Text = "STOP";
            this.btnRunStop.UseVisualStyleBackColor = true;
            this.btnRunStop.Click += new System.EventHandler(this.btnRunStop_Click);
            // 
            // gbManualMode
            // 
            this.gbManualMode.Controls.Add(this.groupBox6);
            this.gbManualMode.Controls.Add(this.groupBox5);
            this.gbManualMode.Controls.Add(this.groupBox3);
            this.gbManualMode.Controls.Add(this.groupBox2);
            this.gbManualMode.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbManualMode.Location = new System.Drawing.Point(1231, 103);
            this.gbManualMode.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.gbManualMode.Name = "gbManualMode";
            this.gbManualMode.Padding = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.gbManualMode.Size = new System.Drawing.Size(256, 676);
            this.gbManualMode.TabIndex = 43;
            this.gbManualMode.TabStop = false;
            this.gbManualMode.Text = "Manual Mode";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnDischargingStart);
            this.groupBox5.Controls.Add(this.btnDischargingStop);
            this.groupBox5.Location = new System.Drawing.Point(7, 366);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(241, 138);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Discharging";
            // 
            // btnDischargingStart
            // 
            this.btnDischargingStart.Location = new System.Drawing.Point(3, 46);
            this.btnDischargingStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDischargingStart.Name = "btnDischargingStart";
            this.btnDischargingStart.Size = new System.Drawing.Size(114, 75);
            this.btnDischargingStart.TabIndex = 1;
            this.btnDischargingStart.Text = "START";
            this.btnDischargingStart.UseVisualStyleBackColor = true;
            this.btnDischargingStart.Click += new System.EventHandler(this.btnDischargingStart_Click);
            // 
            // btnDischargingStop
            // 
            this.btnDischargingStop.Location = new System.Drawing.Point(124, 46);
            this.btnDischargingStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDischargingStop.Name = "btnDischargingStop";
            this.btnDischargingStop.Size = new System.Drawing.Size(114, 75);
            this.btnDischargingStop.TabIndex = 2;
            this.btnDischargingStop.Text = "STOP";
            this.btnDischargingStop.UseVisualStyleBackColor = true;
            this.btnDischargingStop.Click += new System.EventHandler(this.btnDischargingStop_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRunStart);
            this.groupBox3.Controls.Add(this.btnRunStop);
            this.groupBox3.Location = new System.Drawing.Point(7, 206);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(241, 138);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Charging";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnProbeOpen);
            this.groupBox2.Controls.Add(this.btnProbeClose);
            this.groupBox2.Location = new System.Drawing.Point(7, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 136);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PROBE";
            // 
            // btnInit
            // 
            this.btnInit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInit.Location = new System.Drawing.Point(1228, 22);
            this.btnInit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(126, 50);
            this.btnInit.TabIndex = 44;
            this.btnInit.Text = "Initialization";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // lblTestTime
            // 
            this.lblTestTime.BackColor = System.Drawing.Color.White;
            this.lblTestTime.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTestTime.Location = new System.Drawing.Point(175, 25);
            this.lblTestTime.Name = "lblTestTime";
            this.lblTestTime.Size = new System.Drawing.Size(94, 43);
            this.lblTestTime.TabIndex = 45;
            this.lblTestTime.Text = "-";
            this.lblTestTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.lblTestTime);
            this.groupBox4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox4.Location = new System.Drawing.Point(945, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(276, 81);
            this.groupBox4.TabIndex = 46;
            this.groupBox4.TabStop = false;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(13, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(150, 43);
            this.label14.TabIndex = 46;
            this.label14.Text = "Charging Time :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnMeasureStart);
            this.groupBox6.Controls.Add(this.btnMeasureStop);
            this.groupBox6.Location = new System.Drawing.Point(7, 523);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(241, 138);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Measuring";
            // 
            // btnMeasureStart
            // 
            this.btnMeasureStart.Location = new System.Drawing.Point(3, 46);
            this.btnMeasureStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMeasureStart.Name = "btnMeasureStart";
            this.btnMeasureStart.Size = new System.Drawing.Size(114, 75);
            this.btnMeasureStart.TabIndex = 1;
            this.btnMeasureStart.Text = "START";
            this.btnMeasureStart.UseVisualStyleBackColor = true;
            this.btnMeasureStart.Click += new System.EventHandler(this.btnMeasureStart_Click);
            // 
            // btnMeasureStop
            // 
            this.btnMeasureStop.Location = new System.Drawing.Point(124, 46);
            this.btnMeasureStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMeasureStop.Name = "btnMeasureStop";
            this.btnMeasureStop.Size = new System.Drawing.Size(114, 75);
            this.btnMeasureStop.TabIndex = 2;
            this.btnMeasureStop.Text = "STOP";
            this.btnMeasureStop.UseVisualStyleBackColor = true;
            // 
            // FormMeasureInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1496, 1158);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.gbManualMode);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pBase);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMeasureInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMeasureInfo";
            this.pBase.ResumeLayout(false);
            this.pBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbManualMode.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pBase;
        private System.Windows.Forms.DataGridView gridView;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnProbeOpen;
        private System.Windows.Forms.Button btnProbeClose;
        private System.Windows.Forms.Button btnRunStop;
        private System.Windows.Forms.Button btnRunStart;
        private System.Windows.Forms.GroupBox gbManualMode;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Label lblTestTime;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnDischargingStart;
        private System.Windows.Forms.Button btnDischargingStop;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnMeasureStart;
        private System.Windows.Forms.Button btnMeasureStop;
    }
}