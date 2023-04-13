
namespace PreCharger
{
    partial class KeysightForm
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
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.tbCommand = new System.Windows.Forms.TextBox();
            this.lblCommand = new System.Windows.Forms.Label();
            this.lblCmd1 = new System.Windows.Forms.Label();
            this.lblCmd2 = new System.Windows.Forms.Label();
            this.gbCommands = new System.Windows.Forms.GroupBox();
            this.lblCmd24 = new System.Windows.Forms.Label();
            this.lblCmd12 = new System.Windows.Forms.Label();
            this.lblCmd10 = new System.Windows.Forms.Label();
            this.lblCmd11 = new System.Windows.Forms.Label();
            this.lblCmd23 = new System.Windows.Forms.Label();
            this.lblCmd22 = new System.Windows.Forms.Label();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.lblCmd21 = new System.Windows.Forms.Label();
            this.lblCmd5 = new System.Windows.Forms.Label();
            this.lblCmd9 = new System.Windows.Forms.Label();
            this.lblCmd8 = new System.Windows.Forms.Label();
            this.lblCmd4 = new System.Windows.Forms.Label();
            this.lblCmd7 = new System.Windows.Forms.Label();
            this.lblCmd6 = new System.Windows.Forms.Label();
            this.lblCmd3 = new System.Windows.Forms.Label();
            this.cbModule1 = new System.Windows.Forms.CheckBox();
            this.cbModule2 = new System.Windows.Forms.CheckBox();
            this.cbModule3 = new System.Windows.Forms.CheckBox();
            this.cbModule4 = new System.Windows.Forms.CheckBox();
            this.cbModule8 = new System.Windows.Forms.CheckBox();
            this.cbModule7 = new System.Windows.Forms.CheckBox();
            this.cbModule6 = new System.Windows.Forms.CheckBox();
            this.cbModule5 = new System.Windows.Forms.CheckBox();
            this.btnKeysightOpen = new System.Windows.Forms.Button();
            this.tbKeysightIPAddress = new System.Windows.Forms.TextBox();
            this.cbKeysigt = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbPrechargeTime = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbPrechargeCurrent = new System.Windows.Forms.TextBox();
            this.tbPrechargeVoltage = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbChargeTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbChargeCurrent = new System.Windows.Forms.TextBox();
            this.tbChargeVoltage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tbDischargeTime = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tbDischargeCurrent = new System.Windows.Forms.TextBox();
            this.tbDischargeVoltage = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.tabSetting = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gbCommands.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(12, 309);
            this.tbMsg.Multiline = true;
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbMsg.Size = new System.Drawing.Size(1758, 632);
            this.tbMsg.TabIndex = 0;
            // 
            // tbCommand
            // 
            this.tbCommand.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbCommand.Location = new System.Drawing.Point(148, 234);
            this.tbCommand.Multiline = true;
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.Size = new System.Drawing.Size(637, 38);
            this.tbCommand.TabIndex = 1;
            this.tbCommand.Text = "*IDN?";
            // 
            // lblCommand
            // 
            this.lblCommand.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCommand.Location = new System.Drawing.Point(24, 234);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(118, 38);
            this.lblCommand.TabIndex = 2;
            this.lblCommand.Text = "COMMAND";
            this.lblCommand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCmd1
            // 
            this.lblCmd1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd1.Location = new System.Drawing.Point(53, 30);
            this.lblCmd1.Name = "lblCmd1";
            this.lblCmd1.Size = new System.Drawing.Size(165, 42);
            this.lblCmd1.TabIndex = 3;
            this.lblCmd1.Tag = "1";
            this.lblCmd1.Text = "1. *IDN?";
            this.lblCmd1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd1.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd2
            // 
            this.lblCmd2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd2.Location = new System.Drawing.Point(224, 30);
            this.lblCmd2.Name = "lblCmd2";
            this.lblCmd2.Size = new System.Drawing.Size(165, 42);
            this.lblCmd2.TabIndex = 4;
            this.lblCmd2.Tag = "2";
            this.lblCmd2.Text = "2. *RST";
            this.lblCmd2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd2.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // gbCommands
            // 
            this.gbCommands.BackColor = System.Drawing.Color.White;
            this.gbCommands.Controls.Add(this.lblCmd24);
            this.gbCommands.Controls.Add(this.lblCmd12);
            this.gbCommands.Controls.Add(this.lblCmd10);
            this.gbCommands.Controls.Add(this.lblCmd11);
            this.gbCommands.Controls.Add(this.lblCmd23);
            this.gbCommands.Controls.Add(this.lblCmd22);
            this.gbCommands.Controls.Add(this.btnSendCommand);
            this.gbCommands.Controls.Add(this.lblCmd21);
            this.gbCommands.Controls.Add(this.lblCmd5);
            this.gbCommands.Controls.Add(this.lblCmd9);
            this.gbCommands.Controls.Add(this.lblCmd8);
            this.gbCommands.Controls.Add(this.lblCmd4);
            this.gbCommands.Controls.Add(this.lblCmd7);
            this.gbCommands.Controls.Add(this.lblCmd6);
            this.gbCommands.Controls.Add(this.lblCmd3);
            this.gbCommands.Controls.Add(this.lblCmd1);
            this.gbCommands.Controls.Add(this.lblCmd2);
            this.gbCommands.Controls.Add(this.lblCommand);
            this.gbCommands.Controls.Add(this.tbCommand);
            this.gbCommands.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbCommands.Location = new System.Drawing.Point(833, 22);
            this.gbCommands.Name = "gbCommands";
            this.gbCommands.Size = new System.Drawing.Size(937, 281);
            this.gbCommands.TabIndex = 6;
            this.gbCommands.TabStop = false;
            this.gbCommands.Text = "COMMANDS";
            // 
            // lblCmd24
            // 
            this.lblCmd24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd24.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd24.Location = new System.Drawing.Point(566, 180);
            this.lblCmd24.Name = "lblCmd24";
            this.lblCmd24.Size = new System.Drawing.Size(165, 42);
            this.lblCmd24.TabIndex = 25;
            this.lblCmd24.Tag = "24";
            this.lblCmd24.Text = "24. ABORT (STOP)";
            this.lblCmd24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd24.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd12
            // 
            this.lblCmd12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd12.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd12.Location = new System.Drawing.Point(657, 130);
            this.lblCmd12.Name = "lblCmd12";
            this.lblCmd12.Size = new System.Drawing.Size(247, 42);
            this.lblCmd12.TabIndex = 24;
            this.lblCmd12.Tag = "12";
            this.lblCmd12.Text = "12. SEQ:STEP:DEF? 1,1";
            this.lblCmd12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd12.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd10
            // 
            this.lblCmd10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd10.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd10.Location = new System.Drawing.Point(53, 130);
            this.lblCmd10.Name = "lblCmd10";
            this.lblCmd10.Size = new System.Drawing.Size(165, 42);
            this.lblCmd10.TabIndex = 23;
            this.lblCmd10.Tag = "19";
            this.lblCmd10.Text = "10. SYST:ERR?";
            this.lblCmd10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd10.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd11
            // 
            this.lblCmd11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd11.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd11.Location = new System.Drawing.Point(395, 130);
            this.lblCmd11.Name = "lblCmd11";
            this.lblCmd11.Size = new System.Drawing.Size(256, 42);
            this.lblCmd11.TabIndex = 22;
            this.lblCmd11.Tag = "11";
            this.lblCmd11.Text = "11. SYST:PROB:LIM 2,0";
            this.lblCmd11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd11.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd23
            // 
            this.lblCmd23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd23.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd23.Location = new System.Drawing.Point(395, 180);
            this.lblCmd23.Name = "lblCmd23";
            this.lblCmd23.Size = new System.Drawing.Size(165, 42);
            this.lblCmd23.TabIndex = 21;
            this.lblCmd23.Tag = "23";
            this.lblCmd23.Text = "23. DISCHARGING";
            this.lblCmd23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd23.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd22
            // 
            this.lblCmd22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd22.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd22.Location = new System.Drawing.Point(224, 180);
            this.lblCmd22.Name = "lblCmd22";
            this.lblCmd22.Size = new System.Drawing.Size(165, 42);
            this.lblCmd22.TabIndex = 20;
            this.lblCmd22.Tag = "22";
            this.lblCmd22.Text = "22. CHARGING";
            this.lblCmd22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd22.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSendCommand.Location = new System.Drawing.Point(792, 212);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(112, 60);
            this.btnSendCommand.TabIndex = 19;
            this.btnSendCommand.Text = "SEND";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // lblCmd21
            // 
            this.lblCmd21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd21.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd21.Location = new System.Drawing.Point(53, 180);
            this.lblCmd21.Name = "lblCmd21";
            this.lblCmd21.Size = new System.Drawing.Size(165, 42);
            this.lblCmd21.TabIndex = 18;
            this.lblCmd21.Tag = "21";
            this.lblCmd21.Text = "21. SET";
            this.lblCmd21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd21.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd5
            // 
            this.lblCmd5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd5.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd5.Location = new System.Drawing.Point(737, 30);
            this.lblCmd5.Name = "lblCmd5";
            this.lblCmd5.Size = new System.Drawing.Size(165, 42);
            this.lblCmd5.TabIndex = 17;
            this.lblCmd5.Tag = "5";
            this.lblCmd5.Text = "5. DATA:LOG:CLE";
            this.lblCmd5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd5.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd9
            // 
            this.lblCmd9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd9.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd9.Location = new System.Drawing.Point(657, 80);
            this.lblCmd9.Name = "lblCmd9";
            this.lblCmd9.Size = new System.Drawing.Size(245, 42);
            this.lblCmd9.TabIndex = 16;
            this.lblCmd9.Tag = "9";
            this.lblCmd9.Text = "9. STAT:CELL:VERB? 1001";
            this.lblCmd9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd9.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd8
            // 
            this.lblCmd8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd8.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd8.Location = new System.Drawing.Point(395, 80);
            this.lblCmd8.Name = "lblCmd8";
            this.lblCmd8.Size = new System.Drawing.Size(256, 42);
            this.lblCmd8.TabIndex = 15;
            this.lblCmd8.Tag = "8";
            this.lblCmd8.Text = "8. STAT:CELL:REP? (@1001:1032)";
            this.lblCmd8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd8.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd4
            // 
            this.lblCmd4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd4.Location = new System.Drawing.Point(566, 30);
            this.lblCmd4.Name = "lblCmd4";
            this.lblCmd4.Size = new System.Drawing.Size(165, 42);
            this.lblCmd4.TabIndex = 14;
            this.lblCmd4.Tag = "4";
            this.lblCmd4.Text = "4. DATA:LOG?";
            this.lblCmd4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd4.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd7
            // 
            this.lblCmd7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd7.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd7.Location = new System.Drawing.Point(224, 80);
            this.lblCmd7.Name = "lblCmd7";
            this.lblCmd7.Size = new System.Drawing.Size(165, 42);
            this.lblCmd7.TabIndex = 13;
            this.lblCmd7.Tag = "7";
            this.lblCmd7.Text = "7. MEAS:CURR? 0000";
            this.lblCmd7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd7.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd6
            // 
            this.lblCmd6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd6.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd6.Location = new System.Drawing.Point(53, 80);
            this.lblCmd6.Name = "lblCmd6";
            this.lblCmd6.Size = new System.Drawing.Size(165, 42);
            this.lblCmd6.TabIndex = 12;
            this.lblCmd6.Tag = "6";
            this.lblCmd6.Text = "6. MEAS:VOLT? 0000";
            this.lblCmd6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd6.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // lblCmd3
            // 
            this.lblCmd3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCmd3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCmd3.Location = new System.Drawing.Point(395, 30);
            this.lblCmd3.Name = "lblCmd3";
            this.lblCmd3.Size = new System.Drawing.Size(165, 42);
            this.lblCmd3.TabIndex = 5;
            this.lblCmd3.Tag = "3";
            this.lblCmd3.Text = "3. *CLS";
            this.lblCmd3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCmd3.Click += new System.EventHandler(this.lblCmd_Click);
            // 
            // cbModule1
            // 
            this.cbModule1.AutoSize = true;
            this.cbModule1.Checked = true;
            this.cbModule1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule1.Location = new System.Drawing.Point(69, 55);
            this.cbModule1.Name = "cbModule1";
            this.cbModule1.Size = new System.Drawing.Size(116, 21);
            this.cbModule1.TabIndex = 0;
            this.cbModule1.Text = "MODULE 1";
            this.cbModule1.UseVisualStyleBackColor = true;
            // 
            // cbModule2
            // 
            this.cbModule2.AutoSize = true;
            this.cbModule2.Checked = true;
            this.cbModule2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule2.Location = new System.Drawing.Point(228, 55);
            this.cbModule2.Name = "cbModule2";
            this.cbModule2.Size = new System.Drawing.Size(116, 21);
            this.cbModule2.TabIndex = 1;
            this.cbModule2.Text = "MODULE 2";
            this.cbModule2.UseVisualStyleBackColor = true;
            // 
            // cbModule3
            // 
            this.cbModule3.AutoSize = true;
            this.cbModule3.Checked = true;
            this.cbModule3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule3.Location = new System.Drawing.Point(390, 55);
            this.cbModule3.Name = "cbModule3";
            this.cbModule3.Size = new System.Drawing.Size(116, 21);
            this.cbModule3.TabIndex = 2;
            this.cbModule3.Text = "MODULE 3";
            this.cbModule3.UseVisualStyleBackColor = true;
            // 
            // cbModule4
            // 
            this.cbModule4.AutoSize = true;
            this.cbModule4.Checked = true;
            this.cbModule4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule4.Location = new System.Drawing.Point(539, 55);
            this.cbModule4.Name = "cbModule4";
            this.cbModule4.Size = new System.Drawing.Size(117, 21);
            this.cbModule4.TabIndex = 3;
            this.cbModule4.Text = "MODULE 4";
            this.cbModule4.UseVisualStyleBackColor = true;
            // 
            // cbModule8
            // 
            this.cbModule8.AutoSize = true;
            this.cbModule8.Checked = true;
            this.cbModule8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule8.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule8.Location = new System.Drawing.Point(539, 135);
            this.cbModule8.Name = "cbModule8";
            this.cbModule8.Size = new System.Drawing.Size(116, 21);
            this.cbModule8.TabIndex = 7;
            this.cbModule8.Text = "MODULE 8";
            this.cbModule8.UseVisualStyleBackColor = true;
            // 
            // cbModule7
            // 
            this.cbModule7.AutoSize = true;
            this.cbModule7.Checked = true;
            this.cbModule7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule7.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule7.Location = new System.Drawing.Point(390, 135);
            this.cbModule7.Name = "cbModule7";
            this.cbModule7.Size = new System.Drawing.Size(116, 21);
            this.cbModule7.TabIndex = 6;
            this.cbModule7.Text = "MODULE 7";
            this.cbModule7.UseVisualStyleBackColor = true;
            // 
            // cbModule6
            // 
            this.cbModule6.AutoSize = true;
            this.cbModule6.Checked = true;
            this.cbModule6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule6.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule6.Location = new System.Drawing.Point(228, 135);
            this.cbModule6.Name = "cbModule6";
            this.cbModule6.Size = new System.Drawing.Size(116, 21);
            this.cbModule6.TabIndex = 5;
            this.cbModule6.Text = "MODULE 6";
            this.cbModule6.UseVisualStyleBackColor = true;
            // 
            // cbModule5
            // 
            this.cbModule5.AutoSize = true;
            this.cbModule5.Checked = true;
            this.cbModule5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbModule5.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbModule5.Location = new System.Drawing.Point(69, 135);
            this.cbModule5.Name = "cbModule5";
            this.cbModule5.Size = new System.Drawing.Size(116, 21);
            this.cbModule5.TabIndex = 4;
            this.cbModule5.Text = "MODULE 5";
            this.cbModule5.UseVisualStyleBackColor = true;
            // 
            // btnKeysightOpen
            // 
            this.btnKeysightOpen.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnKeysightOpen.Location = new System.Drawing.Point(624, 69);
            this.btnKeysightOpen.Name = "btnKeysightOpen";
            this.btnKeysightOpen.Size = new System.Drawing.Size(129, 111);
            this.btnKeysightOpen.TabIndex = 2;
            this.btnKeysightOpen.Text = "OPEN";
            this.btnKeysightOpen.UseVisualStyleBackColor = true;
            // 
            // tbKeysightIPAddress
            // 
            this.tbKeysightIPAddress.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbKeysightIPAddress.Location = new System.Drawing.Point(342, 69);
            this.tbKeysightIPAddress.Name = "tbKeysightIPAddress";
            this.tbKeysightIPAddress.Size = new System.Drawing.Size(234, 30);
            this.tbKeysightIPAddress.TabIndex = 3;
            this.tbKeysightIPAddress.Text = "192.168.250.211";
            this.tbKeysightIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbKeysigt
            // 
            this.cbKeysigt.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbKeysigt.FormattingEnabled = true;
            this.cbKeysigt.Items.AddRange(new object[] {
            "KEYSIGHT 001",
            "KEYSIGHT 002",
            "KEYSIGHT 003",
            "KEYSIGHT 004",
            "KEYSIGHT 005",
            "KEYSIGHT 006",
            "KEYSIGHT 007",
            "KEYSIGHT 008",
            "KEYSIGHT 009",
            "KEYSIGHT 010",
            "KEYSIGHT 011",
            "KEYSIGHT 012",
            "KEYSIGHT 013",
            "KEYSIGHT 014",
            "KEYSIGHT 015"});
            this.cbKeysigt.Location = new System.Drawing.Point(70, 69);
            this.cbKeysigt.Name = "cbKeysigt";
            this.cbKeysigt.Size = new System.Drawing.Size(231, 28);
            this.cbKeysigt.TabIndex = 4;
            this.cbKeysigt.Text = "Select Keysight";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(70, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "KEYSIGHT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(342, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(234, 25);
            this.label5.TabIndex = 6;
            this.label5.Text = "IP ADDRESS";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tbPrechargeTime);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.tbPrechargeCurrent);
            this.groupBox5.Controls.Add(this.tbPrechargeVoltage);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox5.Location = new System.Drawing.Point(15, 29);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(240, 193);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PRECHARGE SETTING";
            // 
            // tbPrechargeTime
            // 
            this.tbPrechargeTime.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbPrechargeTime.Location = new System.Drawing.Point(111, 45);
            this.tbPrechargeTime.Name = "tbPrechargeTime";
            this.tbPrechargeTime.Size = new System.Drawing.Size(90, 27);
            this.tbPrechargeTime.TabIndex = 10;
            this.tbPrechargeTime.Text = "30";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(27, 128);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(74, 25);
            this.label20.TabIndex = 7;
            this.label20.Text = "VOLT :";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPrechargeCurrent
            // 
            this.tbPrechargeCurrent.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbPrechargeCurrent.Location = new System.Drawing.Point(111, 85);
            this.tbPrechargeCurrent.Name = "tbPrechargeCurrent";
            this.tbPrechargeCurrent.Size = new System.Drawing.Size(90, 27);
            this.tbPrechargeCurrent.TabIndex = 8;
            this.tbPrechargeCurrent.Text = "1000";
            // 
            // tbPrechargeVoltage
            // 
            this.tbPrechargeVoltage.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbPrechargeVoltage.Location = new System.Drawing.Point(111, 126);
            this.tbPrechargeVoltage.Name = "tbPrechargeVoltage";
            this.tbPrechargeVoltage.Size = new System.Drawing.Size(90, 27);
            this.tbPrechargeVoltage.TabIndex = 6;
            this.tbPrechargeVoltage.Text = "1000";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(27, 44);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(74, 25);
            this.label21.TabIndex = 11;
            this.label21.Text = "TIME :";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.Location = new System.Drawing.Point(27, 88);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(74, 25);
            this.label22.TabIndex = 9;
            this.label22.Text = "CURR :";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbChargeTime);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.tbChargeCurrent);
            this.groupBox4.Controls.Add(this.tbChargeVoltage);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox4.Location = new System.Drawing.Point(281, 29);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(240, 193);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CHARGE SETTING";
            // 
            // tbChargeTime
            // 
            this.tbChargeTime.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbChargeTime.Location = new System.Drawing.Point(123, 44);
            this.tbChargeTime.Name = "tbChargeTime";
            this.tbChargeTime.Size = new System.Drawing.Size(90, 27);
            this.tbChargeTime.TabIndex = 10;
            this.tbChargeTime.Text = "180";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(39, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 25);
            this.label7.TabIndex = 7;
            this.label7.Text = "VOLT :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbChargeCurrent
            // 
            this.tbChargeCurrent.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbChargeCurrent.Location = new System.Drawing.Point(123, 84);
            this.tbChargeCurrent.Name = "tbChargeCurrent";
            this.tbChargeCurrent.Size = new System.Drawing.Size(90, 27);
            this.tbChargeCurrent.TabIndex = 8;
            this.tbChargeCurrent.Text = "1600";
            // 
            // tbChargeVoltage
            // 
            this.tbChargeVoltage.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbChargeVoltage.Location = new System.Drawing.Point(123, 125);
            this.tbChargeVoltage.Name = "tbChargeVoltage";
            this.tbChargeVoltage.Size = new System.Drawing.Size(90, 27);
            this.tbChargeVoltage.TabIndex = 6;
            this.tbChargeVoltage.Text = "4200";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(39, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 25);
            this.label8.TabIndex = 11;
            this.label8.Text = "TIME :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(39, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 25);
            this.label9.TabIndex = 9;
            this.label9.Text = "CURR :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tbDischargeTime);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.tbDischargeCurrent);
            this.groupBox6.Controls.Add(this.tbDischargeVoltage);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox6.Location = new System.Drawing.Point(542, 29);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(240, 193);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "DISCHARGE SETTING";
            // 
            // tbDischargeTime
            // 
            this.tbDischargeTime.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbDischargeTime.Location = new System.Drawing.Point(117, 45);
            this.tbDischargeTime.Name = "tbDischargeTime";
            this.tbDischargeTime.Size = new System.Drawing.Size(90, 27);
            this.tbDischargeTime.TabIndex = 10;
            this.tbDischargeTime.Text = "1800";
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(33, 128);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(74, 25);
            this.label23.TabIndex = 7;
            this.label23.Text = "VOLT :";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDischargeCurrent
            // 
            this.tbDischargeCurrent.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbDischargeCurrent.Location = new System.Drawing.Point(117, 85);
            this.tbDischargeCurrent.Name = "tbDischargeCurrent";
            this.tbDischargeCurrent.Size = new System.Drawing.Size(90, 27);
            this.tbDischargeCurrent.TabIndex = 8;
            this.tbDischargeCurrent.Text = "20000";
            // 
            // tbDischargeVoltage
            // 
            this.tbDischargeVoltage.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbDischargeVoltage.Location = new System.Drawing.Point(117, 126);
            this.tbDischargeVoltage.Name = "tbDischargeVoltage";
            this.tbDischargeVoltage.Size = new System.Drawing.Size(90, 27);
            this.tbDischargeVoltage.TabIndex = 6;
            this.tbDischargeVoltage.Text = "2000";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(33, 44);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(74, 25);
            this.label24.TabIndex = 11;
            this.label24.Text = "TIME :";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(33, 88);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(74, 25);
            this.label25.TabIndex = 9;
            this.label25.Text = "CURR :";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.tabPage1);
            this.tabSetting.Controls.Add(this.tabPage2);
            this.tabSetting.Controls.Add(this.tabPage3);
            this.tabSetting.Location = new System.Drawing.Point(12, 22);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.SelectedIndex = 0;
            this.tabSetting.Size = new System.Drawing.Size(805, 272);
            this.tabSetting.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblConnectionStatus);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.btnKeysightOpen);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.tbKeysightIPAddress);
            this.tabPage1.Controls.Add(this.cbKeysigt);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(797, 243);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "CONNECTION";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblConnectionStatus.Location = new System.Drawing.Point(70, 155);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(506, 25);
            this.lblConnectionStatus.TabIndex = 7;
            this.lblConnectionStatus.Text = "KEYSIGHT 192.168.250.211 IS CONNECTED";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(797, 243);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SETTING";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cbModule8);
            this.tabPage3.Controls.Add(this.cbModule5);
            this.tabPage3.Controls.Add(this.cbModule7);
            this.tabPage3.Controls.Add(this.cbModule1);
            this.tabPage3.Controls.Add(this.cbModule6);
            this.tabPage3.Controls.Add(this.cbModule2);
            this.tabPage3.Controls.Add(this.cbModule3);
            this.tabPage3.Controls.Add(this.cbModule4);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(797, 243);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SELECT MODULE";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // KeysightForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 953);
            this.Controls.Add(this.tabSetting);
            this.Controls.Add(this.gbCommands);
            this.Controls.Add(this.tbMsg);
            this.Name = "KeysightForm";
            this.Text = "KEYSIGHT COMMAND";
            this.gbCommands.ResumeLayout(false);
            this.gbCommands.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.TextBox tbCommand;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.Label lblCmd1;
        private System.Windows.Forms.Label lblCmd2;
        private System.Windows.Forms.CheckBox cbModule8;
        private System.Windows.Forms.CheckBox cbModule7;
        private System.Windows.Forms.CheckBox cbModule6;
        private System.Windows.Forms.CheckBox cbModule5;
        private System.Windows.Forms.CheckBox cbModule4;
        private System.Windows.Forms.CheckBox cbModule3;
        private System.Windows.Forms.CheckBox cbModule2;
        private System.Windows.Forms.CheckBox cbModule1;
        private System.Windows.Forms.GroupBox gbCommands;
        private System.Windows.Forms.ComboBox cbKeysigt;
        private System.Windows.Forms.TextBox tbKeysightIPAddress;
        private System.Windows.Forms.Button btnKeysightOpen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCmd3;
        private System.Windows.Forms.Label lblCmd7;
        private System.Windows.Forms.Label lblCmd6;
        private System.Windows.Forms.Label lblCmd4;
        private System.Windows.Forms.Label lblCmd9;
        private System.Windows.Forms.Label lblCmd8;
        private System.Windows.Forms.Label lblCmd5;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.Label lblCmd21;
        private System.Windows.Forms.Label lblCmd23;
        private System.Windows.Forms.Label lblCmd22;
        private System.Windows.Forms.Label lblCmd11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tbPrechargeTime;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbPrechargeCurrent;
        private System.Windows.Forms.TextBox tbPrechargeVoltage;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbChargeTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbChargeCurrent;
        private System.Windows.Forms.TextBox tbChargeVoltage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tbDischargeTime;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbDischargeCurrent;
        private System.Windows.Forms.TextBox tbDischargeVoltage;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblCmd10;
        private System.Windows.Forms.Label lblCmd12;
        private System.Windows.Forms.Label lblCmd24;
        private System.Windows.Forms.TabControl tabSetting;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblConnectionStatus;
    }
}