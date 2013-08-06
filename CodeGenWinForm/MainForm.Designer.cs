namespace CodeGenWinForm
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtConnStr = new System.Windows.Forms.TextBox();
			this.btnConn = new System.Windows.Forms.Button();
			this.cmbTables = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbCodeTypes = new System.Windows.Forms.ComboBox();
			this.btnGen = new System.Windows.Forms.Button();
			this.txtCode = new System.Windows.Forms.RichTextBox();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnGen);
			this.panel1.Controls.Add(this.cmbCodeTypes);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.cmbTables);
			this.panel1.Controls.Add(this.btnConn);
			this.panel1.Controls.Add(this.txtConnStr);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(5, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(574, 75);
			this.panel1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtCode);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(5, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(574, 476);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "代码输出";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "链接字符串";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(50, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "表";
			// 
			// txtConnStr
			// 
			this.txtConnStr.Location = new System.Drawing.Point(74, 7);
			this.txtConnStr.Name = "txtConnStr";
			this.txtConnStr.Size = new System.Drawing.Size(415, 21);
			this.txtConnStr.TabIndex = 2;
			this.txtConnStr.TextChanged += new System.EventHandler(this.txtConnStr_TextChanged);
			// 
			// btnConn
			// 
			this.btnConn.Location = new System.Drawing.Point(495, 5);
			this.btnConn.Name = "btnConn";
			this.btnConn.Size = new System.Drawing.Size(75, 23);
			this.btnConn.TabIndex = 3;
			this.btnConn.Text = "链接";
			this.btnConn.UseVisualStyleBackColor = true;
			this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
			// 
			// cmbTables
			// 
			this.cmbTables.Enabled = false;
			this.cmbTables.FormattingEnabled = true;
			this.cmbTables.Location = new System.Drawing.Point(73, 40);
			this.cmbTables.Name = "cmbTables";
			this.cmbTables.Size = new System.Drawing.Size(157, 20);
			this.cmbTables.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(236, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "代码类型";
			// 
			// cmbCodeTypes
			// 
			this.cmbCodeTypes.Enabled = false;
			this.cmbCodeTypes.FormattingEnabled = true;
			this.cmbCodeTypes.Location = new System.Drawing.Point(295, 40);
			this.cmbCodeTypes.Name = "cmbCodeTypes";
			this.cmbCodeTypes.Size = new System.Drawing.Size(92, 20);
			this.cmbCodeTypes.TabIndex = 6;
			// 
			// btnGen
			// 
			this.btnGen.Enabled = false;
			this.btnGen.Location = new System.Drawing.Point(414, 37);
			this.btnGen.Name = "btnGen";
			this.btnGen.Size = new System.Drawing.Size(75, 23);
			this.btnGen.TabIndex = 7;
			this.btnGen.Text = "生成代码";
			this.btnGen.UseVisualStyleBackColor = true;
			this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
			// 
			// txtCode
			// 
			this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCode.Location = new System.Drawing.Point(5, 19);
			this.txtCode.Name = "txtCode";
			this.txtCode.ReadOnly = true;
			this.txtCode.Size = new System.Drawing.Size(564, 452);
			this.txtCode.TabIndex = 1;
			this.txtCode.Text = "";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 561);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Text = "SQLServer代码生成器";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnGen;
		private System.Windows.Forms.ComboBox cmbCodeTypes;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbTables;
		private System.Windows.Forms.Button btnConn;
		private System.Windows.Forms.TextBox txtConnStr;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox txtCode;
	}
}

