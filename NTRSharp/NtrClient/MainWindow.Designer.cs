namespace NewNtrClient
{
	partial class MainWindow
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
			this.ContentTable = new System.Windows.Forms.TableLayoutPanel();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.ContentPanel = new System.Windows.Forms.Panel();
			this.cmbMemlayout = new System.Windows.Forms.ComboBox();
			this.buttonMemlayout = new System.Windows.Forms.Button();
			this.cmbProcesses = new System.Windows.Forms.ComboBox();
			this.buttonProcesses = new System.Windows.Forms.Button();
			this.txtIpAddress = new System.Windows.Forms.TextBox();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.pgbProgress = new System.Windows.Forms.ProgressBar();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.ContentTable.SuspendLayout();
			this.ContentPanel.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ContentTable
			// 
			this.ContentTable.ColumnCount = 1;
			this.ContentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.ContentTable.Controls.Add(this.txtOutput, 0, 0);
			this.ContentTable.Controls.Add(this.ContentPanel, 0, 1);
			this.ContentTable.Controls.Add(this.pgbProgress, 0, 2);
			this.ContentTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ContentTable.Location = new System.Drawing.Point(0, 24);
			this.ContentTable.Name = "ContentTable";
			this.ContentTable.RowCount = 3;
			this.ContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.34829F));
			this.ContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.65171F));
			this.ContentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.ContentTable.Size = new System.Drawing.Size(746, 578);
			this.ContentTable.TabIndex = 0;
			// 
			// txtOutput
			// 
			this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOutput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtOutput.Location = new System.Drawing.Point(3, 3);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOutput.Size = new System.Drawing.Size(740, 319);
			this.txtOutput.TabIndex = 0;
			// 
			// ContentPanel
			// 
			this.ContentPanel.Controls.Add(this.tabControl1);
			this.ContentPanel.Controls.Add(this.cmbMemlayout);
			this.ContentPanel.Controls.Add(this.buttonMemlayout);
			this.ContentPanel.Controls.Add(this.cmbProcesses);
			this.ContentPanel.Controls.Add(this.buttonProcesses);
			this.ContentPanel.Controls.Add(this.txtIpAddress);
			this.ContentPanel.Controls.Add(this.buttonConnect);
			this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ContentPanel.Location = new System.Drawing.Point(3, 325);
			this.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.ContentPanel.Name = "ContentPanel";
			this.ContentPanel.Size = new System.Drawing.Size(740, 232);
			this.ContentPanel.TabIndex = 1;
			// 
			// cmbMemlayout
			// 
			this.cmbMemlayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMemlayout.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbMemlayout.FormattingEnabled = true;
			this.cmbMemlayout.Items.AddRange(new object[] {
            "Memory Layout",
            "00000000 | 00000000 | 00000000"});
			this.cmbMemlayout.Location = new System.Drawing.Point(501, 7);
			this.cmbMemlayout.Name = "cmbMemlayout";
			this.cmbMemlayout.Size = new System.Drawing.Size(234, 23);
			this.cmbMemlayout.TabIndex = 6;
			// 
			// buttonMemlayout
			// 
			this.buttonMemlayout.Location = new System.Drawing.Point(420, 7);
			this.buttonMemlayout.Name = "buttonMemlayout";
			this.buttonMemlayout.Size = new System.Drawing.Size(75, 23);
			this.buttonMemlayout.TabIndex = 5;
			this.buttonMemlayout.Text = "Memlayout";
			this.buttonMemlayout.UseVisualStyleBackColor = true;
			this.buttonMemlayout.Click += new System.EventHandler(this.buttonMemlayout_Click);
			// 
			// cmbProcesses
			// 
			this.cmbProcesses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProcesses.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbProcesses.FormattingEnabled = true;
			this.cmbProcesses.Items.AddRange(new object[] {
            "Processes"});
			this.cmbProcesses.Location = new System.Drawing.Point(293, 7);
			this.cmbProcesses.Name = "cmbProcesses";
			this.cmbProcesses.Size = new System.Drawing.Size(121, 23);
			this.cmbProcesses.TabIndex = 4;
			// 
			// buttonProcesses
			// 
			this.buttonProcesses.Location = new System.Drawing.Point(212, 7);
			this.buttonProcesses.Name = "buttonProcesses";
			this.buttonProcesses.Size = new System.Drawing.Size(75, 23);
			this.buttonProcesses.TabIndex = 3;
			this.buttonProcesses.Text = "Processes";
			this.buttonProcesses.UseVisualStyleBackColor = true;
			this.buttonProcesses.Click += new System.EventHandler(this.buttonProcesses_Click);
			// 
			// txtIpAddress
			// 
			this.txtIpAddress.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtIpAddress.Location = new System.Drawing.Point(12, 8);
			this.txtIpAddress.Name = "txtIpAddress";
			this.txtIpAddress.Size = new System.Drawing.Size(113, 23);
			this.txtIpAddress.TabIndex = 2;
			this.txtIpAddress.Text = "192.168.0.17";
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(131, 7);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(75, 23);
			this.buttonConnect.TabIndex = 0;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// pgbProgress
			// 
			this.pgbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgbProgress.Location = new System.Drawing.Point(3, 560);
			this.pgbProgress.Name = "pgbProgress";
			this.pgbProgress.Size = new System.Drawing.Size(740, 15);
			this.pgbProgress.TabIndex = 2;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(746, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test1ToolStripMenuItem});
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.testToolStripMenuItem.Text = "Test";
			// 
			// test1ToolStripMenuItem
			// 
			this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
			this.test1ToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
			this.test1ToolStripMenuItem.Text = "Test1";
			// 
			// tabControl1
			// 
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabControl1.Location = new System.Drawing.Point(0, 37);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(740, 195);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(732, 166);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(738, 166);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(746, 602);
			this.Controls.Add(this.ContentTable);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainWindow";
			this.Text = "NTRClient";
			this.ContentTable.ResumeLayout(false);
			this.ContentTable.PerformLayout();
			this.ContentPanel.ResumeLayout(false);
			this.ContentPanel.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel ContentTable;
		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
		private System.Windows.Forms.Panel ContentPanel;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.ProgressBar pgbProgress;
		private System.Windows.Forms.TextBox txtIpAddress;
		private System.Windows.Forms.Button buttonProcesses;
		private System.Windows.Forms.ComboBox cmbProcesses;
		private System.Windows.Forms.Button buttonMemlayout;
		private System.Windows.Forms.ComboBox cmbMemlayout;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
	}
}

