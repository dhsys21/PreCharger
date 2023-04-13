namespace PreCharger
{
    partial class PLCForm
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
            this.dgvPLC4 = new System.Windows.Forms.DataGridView();
            this.dgvPC4 = new System.Windows.Forms.DataGridView();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvPLC1 = new System.Windows.Forms.DataGridView();
            this.dgvPC1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvPLC2 = new System.Windows.Forms.DataGridView();
            this.dgvPC2 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvPLC3 = new System.Windows.Forms.DataGridView();
            this.dgvPC3 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnWriteBit = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC4)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC2)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC3)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPLC4
            // 
            this.dgvPLC4.AllowUserToResizeColumns = false;
            this.dgvPLC4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC4.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPLC4.Location = new System.Drawing.Point(11, 12);
            this.dgvPLC4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPLC4.Name = "dgvPLC4";
            this.dgvPLC4.RowHeadersVisible = false;
            this.dgvPLC4.RowHeadersWidth = 51;
            this.dgvPLC4.RowTemplate.Height = 23;
            this.dgvPLC4.Size = new System.Drawing.Size(514, 216);
            this.dgvPLC4.TabIndex = 0;
            // 
            // dgvPC4
            // 
            this.dgvPC4.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC4.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC4.Location = new System.Drawing.Point(538, 12);
            this.dgvPC4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPC4.Name = "dgvPC4";
            this.dgvPC4.RowHeadersVisible = false;
            this.dgvPC4.RowHeadersWidth = 51;
            this.dgvPC4.RowTemplate.Height = 23;
            this.dgvPC4.Size = new System.Drawing.Size(514, 216);
            this.dgvPC4.TabIndex = 1;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1659, 13);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 41);
            this.button6.TabIndex = 6;
            this.button6.Text = "CLOSE";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvPLC4);
            this.panel1.Controls.Add(this.dgvPC4);
            this.panel1.Location = new System.Drawing.Point(136, 737);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.panel1.Size = new System.Drawing.Size(1063, 240);
            this.panel1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.PowderBlue;
            this.label2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(7, 749);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(1);
            this.label2.Size = new System.Drawing.Size(110, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "STAGE 4";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvPLC1);
            this.panel2.Controls.Add(this.dgvPC1);
            this.panel2.Location = new System.Drawing.Point(136, 8);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.panel2.Size = new System.Drawing.Size(1063, 240);
            this.panel2.TabIndex = 8;
            // 
            // dgvPLC1
            // 
            this.dgvPLC1.AllowUserToResizeColumns = false;
            this.dgvPLC1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPLC1.Location = new System.Drawing.Point(11, 12);
            this.dgvPLC1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPLC1.Name = "dgvPLC1";
            this.dgvPLC1.RowHeadersVisible = false;
            this.dgvPLC1.RowHeadersWidth = 51;
            this.dgvPLC1.RowTemplate.Height = 23;
            this.dgvPLC1.Size = new System.Drawing.Size(514, 216);
            this.dgvPLC1.TabIndex = 0;
            // 
            // dgvPC1
            // 
            this.dgvPC1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC1.Location = new System.Drawing.Point(538, 12);
            this.dgvPC1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dgvPC1.Name = "dgvPC1";
            this.dgvPC1.RowHeadersVisible = false;
            this.dgvPC1.RowHeadersWidth = 51;
            this.dgvPC1.RowTemplate.Height = 23;
            this.dgvPC1.Size = new System.Drawing.Size(514, 216);
            this.dgvPC1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.PowderBlue;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(1);
            this.label1.Size = new System.Drawing.Size(110, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "STAGE 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvPLC2);
            this.panel3.Controls.Add(this.dgvPC2);
            this.panel3.Location = new System.Drawing.Point(136, 250);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.panel3.Size = new System.Drawing.Size(1063, 240);
            this.panel3.TabIndex = 9;
            // 
            // dgvPLC2
            // 
            this.dgvPLC2.AllowUserToResizeColumns = false;
            this.dgvPLC2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC2.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPLC2.Location = new System.Drawing.Point(11, 12);
            this.dgvPLC2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPLC2.Name = "dgvPLC2";
            this.dgvPLC2.RowHeadersVisible = false;
            this.dgvPLC2.RowHeadersWidth = 51;
            this.dgvPLC2.RowTemplate.Height = 23;
            this.dgvPLC2.Size = new System.Drawing.Size(514, 216);
            this.dgvPLC2.TabIndex = 0;
            // 
            // dgvPC2
            // 
            this.dgvPC2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC2.Location = new System.Drawing.Point(538, 12);
            this.dgvPC2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPC2.Name = "dgvPC2";
            this.dgvPC2.RowHeadersVisible = false;
            this.dgvPC2.RowHeadersWidth = 51;
            this.dgvPC2.RowTemplate.Height = 23;
            this.dgvPC2.Size = new System.Drawing.Size(514, 216);
            this.dgvPC2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.PowderBlue;
            this.label3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 262);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(1);
            this.label3.Size = new System.Drawing.Size(110, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "STAGE 2";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvPLC3);
            this.panel4.Controls.Add(this.dgvPC3);
            this.panel4.Location = new System.Drawing.Point(136, 493);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.panel4.Size = new System.Drawing.Size(1063, 240);
            this.panel4.TabIndex = 10;
            // 
            // dgvPLC3
            // 
            this.dgvPLC3.AllowUserToResizeColumns = false;
            this.dgvPLC3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC3.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPLC3.Location = new System.Drawing.Point(11, 12);
            this.dgvPLC3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPLC3.Name = "dgvPLC3";
            this.dgvPLC3.RowHeadersVisible = false;
            this.dgvPLC3.RowHeadersWidth = 51;
            this.dgvPLC3.RowTemplate.Height = 23;
            this.dgvPLC3.Size = new System.Drawing.Size(514, 216);
            this.dgvPLC3.TabIndex = 0;
            // 
            // dgvPC3
            // 
            this.dgvPC3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC3.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC3.Location = new System.Drawing.Point(538, 12);
            this.dgvPC3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPC3.Name = "dgvPC3";
            this.dgvPC3.RowHeadersVisible = false;
            this.dgvPC3.RowHeadersWidth = 51;
            this.dgvPC3.RowTemplate.Height = 23;
            this.dgvPC3.Size = new System.Drawing.Size(514, 216);
            this.dgvPC3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.PowderBlue;
            this.label4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(7, 505);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(1);
            this.label4.Size = new System.Drawing.Size(110, 30);
            this.label4.TabIndex = 2;
            this.label4.Text = "STAGE 3";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.btnWriteBit);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(1205, 62);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(565, 583);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(20, 57);
            this.textBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(300, 60);
            this.textBox5.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(336, 57);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(114, 25);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "120";
            // 
            // btnWriteBit
            // 
            this.btnWriteBit.Location = new System.Drawing.Point(465, 57);
            this.btnWriteBit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnWriteBit.Name = "btnWriteBit";
            this.btnWriteBit.Size = new System.Drawing.Size(86, 29);
            this.btnWriteBit.TabIndex = 6;
            this.btnWriteBit.Text = "Write Bit";
            this.btnWriteBit.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(465, 90);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 29);
            this.button2.TabIndex = 4;
            this.button2.Text = "CONNECT";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(336, 90);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(114, 25);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "1";
            // 
            // PLCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 953);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button6);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PLCForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLCForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PLCForm_FormClosed);
            this.Load += new System.EventHandler(this.PLCForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC2)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPLC4;
        private System.Windows.Forms.DataGridView dgvPC4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPLC1;
        private System.Windows.Forms.DataGridView dgvPC1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvPLC2;
        private System.Windows.Forms.DataGridView dgvPC2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvPLC3;
        private System.Windows.Forms.DataGridView dgvPC3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnWriteBit;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
    }
}