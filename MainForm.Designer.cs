namespace PreCharger
{
    partial class BaseForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLineNo = new System.Windows.Forms.Label();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnPLC = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BasePanel = new System.Windows.Forms.Panel();
            this.btnKeysight = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblLineNo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2187, 91);
            this.panel1.TabIndex = 0;
            // 
            // lblLineNo
            // 
            this.lblLineNo.AutoSize = true;
            this.lblLineNo.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLineNo.ForeColor = System.Drawing.Color.White;
            this.lblLineNo.Location = new System.Drawing.Point(337, 11);
            this.lblLineNo.Name = "lblLineNo";
            this.lblLineNo.Size = new System.Drawing.Size(70, 60);
            this.lblLineNo.TabIndex = 11;
            this.lblLineNo.Text = "#";
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Ivory;
            this.btnConfig.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConfig.Location = new System.Drawing.Point(125, 5);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Padding = new System.Windows.Forms.Padding(5);
            this.btnConfig.Size = new System.Drawing.Size(110, 75);
            this.btnConfig.TabIndex = 9;
            this.btnConfig.Text = "Config";
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnPLC
            // 
            this.btnPLC.BackColor = System.Drawing.Color.Red;
            this.btnPLC.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPLC.Location = new System.Drawing.Point(245, 5);
            this.btnPLC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPLC.Name = "btnPLC";
            this.btnPLC.Padding = new System.Windows.Forms.Padding(5);
            this.btnPLC.Size = new System.Drawing.Size(110, 75);
            this.btnPLC.TabIndex = 5;
            this.btnPLC.Text = "PLC";
            this.btnPLC.UseVisualStyleBackColor = false;
            this.btnPLC.Click += new System.EventHandler(this.btnPLC_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "PreCharger";
            // 
            // BasePanel
            // 
            this.BasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BasePanel.Location = new System.Drawing.Point(0, 91);
            this.BasePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BasePanel.Name = "BasePanel";
            this.BasePanel.Size = new System.Drawing.Size(2187, 1160);
            this.BasePanel.TabIndex = 1;
            // 
            // btnKeysight
            // 
            this.btnKeysight.BackColor = System.Drawing.Color.Ivory;
            this.btnKeysight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnKeysight.Location = new System.Drawing.Point(5, 5);
            this.btnKeysight.Margin = new System.Windows.Forms.Padding(5);
            this.btnKeysight.Name = "btnKeysight";
            this.btnKeysight.Padding = new System.Windows.Forms.Padding(5);
            this.btnKeysight.Size = new System.Drawing.Size(110, 75);
            this.btnKeysight.TabIndex = 12;
            this.btnKeysight.Text = "KEYSIGHT";
            this.btnKeysight.UseVisualStyleBackColor = false;
            this.btnKeysight.Click += new System.EventHandler(this.btnKeysight_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnPLC);
            this.panel2.Controls.Add(this.btnConfig);
            this.panel2.Controls.Add(this.btnKeysight);
            this.panel2.Location = new System.Drawing.Point(1820, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.panel2.Size = new System.Drawing.Size(360, 85);
            this.panel2.TabIndex = 14;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2187, 1251);
            this.Controls.Add(this.BasePanel);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BaseForm";
            this.Text = "PreCharger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BaseForm_FormClosed);
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPLC;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Label lblLineNo;
        public System.Windows.Forms.Panel BasePanel;
        private System.Windows.Forms.Button btnKeysight;
        private System.Windows.Forms.Panel panel2;
    }
}

