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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonDumpMemoryConsole = new System.Windows.Forms.Button();
			this.buttonDumpMemoryFile = new System.Windows.Forms.Button();
			this.txtDumpMemFilename = new System.Windows.Forms.TextBox();
			this.txtDumpMemAddrLength = new System.Windows.Forms.TextBox();
			this.txtDumpMemAddrStart = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.buttonBaseClipboardPaste = new System.Windows.Forms.Button();
			this.buttonBaseClipboardCopy = new System.Windows.Forms.Button();
			this.buttonUseBaseCode = new System.Windows.Forms.Button();
			this.txtBaseCode = new System.Windows.Forms.RichTextBox();
			this.buttonCreateBaseCode = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtBaseLength = new System.Windows.Forms.TextBox();
			this.txtBaseAddress = new System.Windows.Forms.TextBox();
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
			this.ContentTable.SuspendLayout();
			this.ContentPanel.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
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
			this.ContentTable.Size = new System.Drawing.Size(758, 578);
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
			this.txtOutput.Size = new System.Drawing.Size(752, 319);
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
			this.ContentPanel.Size = new System.Drawing.Size(752, 232);
			this.ContentPanel.TabIndex = 1;
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
			this.tabControl1.Size = new System.Drawing.Size(752, 195);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.buttonDumpMemoryConsole);
			this.tabPage1.Controls.Add(this.buttonDumpMemoryFile);
			this.tabPage1.Controls.Add(this.txtDumpMemFilename);
			this.tabPage1.Controls.Add(this.txtDumpMemAddrLength);
			this.tabPage1.Controls.Add(this.txtDumpMemAddrStart);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(744, 166);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(5, 36);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(91, 15);
			this.label2.TabIndex = 7;
			this.label2.Text = "Write Memory";
			// 
			// buttonDumpMemoryConsole
			// 
			this.buttonDumpMemoryConsole.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonDumpMemoryConsole.Location = new System.Drawing.Point(582, 8);
			this.buttonDumpMemoryConsole.Name = "buttonDumpMemoryConsole";
			this.buttonDumpMemoryConsole.Size = new System.Drawing.Size(156, 23);
			this.buttonDumpMemoryConsole.TabIndex = 6;
			this.buttonDumpMemoryConsole.Text = "Dump to Console";
			this.buttonDumpMemoryConsole.UseVisualStyleBackColor = true;
			this.buttonDumpMemoryConsole.Click += new System.EventHandler(this.buttonDumpMemoryConsole_Click);
			// 
			// buttonDumpMemoryFile
			// 
			this.buttonDumpMemoryFile.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonDumpMemoryFile.Location = new System.Drawing.Point(423, 8);
			this.buttonDumpMemoryFile.Name = "buttonDumpMemoryFile";
			this.buttonDumpMemoryFile.Size = new System.Drawing.Size(156, 23);
			this.buttonDumpMemoryFile.TabIndex = 5;
			this.buttonDumpMemoryFile.Text = "Dump as File";
			this.buttonDumpMemoryFile.UseVisualStyleBackColor = true;
			this.buttonDumpMemoryFile.Click += new System.EventHandler(this.buttonDumpMemoryFile_Click);
			// 
			// txtDumpMemFilename
			// 
			this.txtDumpMemFilename.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDumpMemFilename.Location = new System.Drawing.Point(238, 8);
			this.txtDumpMemFilename.Name = "txtDumpMemFilename";
			this.txtDumpMemFilename.Size = new System.Drawing.Size(179, 23);
			this.txtDumpMemFilename.TabIndex = 4;
			this.txtDumpMemFilename.Validating += new System.ComponentModel.CancelEventHandler(this.txtDumpMemFilename_Validating);
			// 
			// txtDumpMemAddrLength
			// 
			this.txtDumpMemAddrLength.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDumpMemAddrLength.Location = new System.Drawing.Point(170, 8);
			this.txtDumpMemAddrLength.MaxLength = 8;
			this.txtDumpMemAddrLength.Name = "txtDumpMemAddrLength";
			this.txtDumpMemAddrLength.Size = new System.Drawing.Size(62, 23);
			this.txtDumpMemAddrLength.TabIndex = 3;
			this.txtDumpMemAddrLength.Text = "00000000";
			this.txtDumpMemAddrLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtDumpMemAddrLength.Validating += new System.ComponentModel.CancelEventHandler(this.txtDumpMemAddrLength_Validating);
			// 
			// txtDumpMemAddrStart
			// 
			this.txtDumpMemAddrStart.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDumpMemAddrStart.Location = new System.Drawing.Point(102, 8);
			this.txtDumpMemAddrStart.MaxLength = 8;
			this.txtDumpMemAddrStart.Name = "txtDumpMemAddrStart";
			this.txtDumpMemAddrStart.Size = new System.Drawing.Size(62, 23);
			this.txtDumpMemAddrStart.TabIndex = 1;
			this.txtDumpMemAddrStart.Text = "00000000";
			this.txtDumpMemAddrStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtDumpMemAddrStart.Validating += new System.ComponentModel.CancelEventHandler(this.txtDumpMemAddrStart_Validating);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 11);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(84, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Dump Memory";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.buttonBaseClipboardPaste);
			this.tabPage2.Controls.Add(this.buttonBaseClipboardCopy);
			this.tabPage2.Controls.Add(this.buttonUseBaseCode);
			this.tabPage2.Controls.Add(this.txtBaseCode);
			this.tabPage2.Controls.Add(this.buttonCreateBaseCode);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.txtBaseLength);
			this.tabPage2.Controls.Add(this.txtBaseAddress);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(744, 166);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Base64 Code";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// buttonBaseClipboardPaste
			// 
			this.buttonBaseClipboardPaste.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonBaseClipboardPaste.Location = new System.Drawing.Point(643, 6);
			this.buttonBaseClipboardPaste.Name = "buttonBaseClipboardPaste";
			this.buttonBaseClipboardPaste.Size = new System.Drawing.Size(88, 23);
			this.buttonBaseClipboardPaste.TabIndex = 11;
			this.buttonBaseClipboardPaste.Text = "Paste";
			this.buttonBaseClipboardPaste.UseVisualStyleBackColor = true;
			this.buttonBaseClipboardPaste.Click += new System.EventHandler(this.buttonBaseClipboardPaste_Click);
			// 
			// buttonBaseClipboardCopy
			// 
			this.buttonBaseClipboardCopy.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonBaseClipboardCopy.Location = new System.Drawing.Point(549, 6);
			this.buttonBaseClipboardCopy.Name = "buttonBaseClipboardCopy";
			this.buttonBaseClipboardCopy.Size = new System.Drawing.Size(88, 23);
			this.buttonBaseClipboardCopy.TabIndex = 10;
			this.buttonBaseClipboardCopy.Text = "Copy";
			this.buttonBaseClipboardCopy.UseVisualStyleBackColor = true;
			this.buttonBaseClipboardCopy.Click += new System.EventHandler(this.buttonBaseClipboardCopy_Click);
			// 
			// buttonUseBaseCode
			// 
			this.buttonUseBaseCode.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonUseBaseCode.Location = new System.Drawing.Point(362, 6);
			this.buttonUseBaseCode.Name = "buttonUseBaseCode";
			this.buttonUseBaseCode.Size = new System.Drawing.Size(93, 23);
			this.buttonUseBaseCode.TabIndex = 9;
			this.buttonUseBaseCode.Text = "Use Code";
			this.buttonUseBaseCode.UseVisualStyleBackColor = true;
			this.buttonUseBaseCode.Click += new System.EventHandler(this.buttonUseBaseCode_Click);
			// 
			// txtBaseCode
			// 
			this.txtBaseCode.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.txtBaseCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBaseCode.Location = new System.Drawing.Point(3, 35);
			this.txtBaseCode.Name = "txtBaseCode";
			this.txtBaseCode.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.txtBaseCode.Size = new System.Drawing.Size(738, 128);
			this.txtBaseCode.TabIndex = 8;
			this.txtBaseCode.Text = "";
			// 
			// buttonCreateBaseCode
			// 
			this.buttonCreateBaseCode.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCreateBaseCode.Location = new System.Drawing.Point(263, 6);
			this.buttonCreateBaseCode.Name = "buttonCreateBaseCode";
			this.buttonCreateBaseCode.Size = new System.Drawing.Size(93, 23);
			this.buttonCreateBaseCode.TabIndex = 7;
			this.buttonCreateBaseCode.Text = "Create Code";
			this.buttonCreateBaseCode.UseVisualStyleBackColor = true;
			this.buttonCreateBaseCode.Click += new System.EventHandler(this.buttonCreateBaseCode_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(37, 9);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label3.Size = new System.Drawing.Size(84, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "Create Code";
			// 
			// txtBaseLength
			// 
			this.txtBaseLength.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBaseLength.Location = new System.Drawing.Point(195, 6);
			this.txtBaseLength.MaxLength = 8;
			this.txtBaseLength.Name = "txtBaseLength";
			this.txtBaseLength.Size = new System.Drawing.Size(62, 23);
			this.txtBaseLength.TabIndex = 5;
			this.txtBaseLength.Text = "00000000";
			this.txtBaseLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtBaseLength.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxBaseLength_Validating);
			// 
			// txtBaseAddress
			// 
			this.txtBaseAddress.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBaseAddress.Location = new System.Drawing.Point(127, 6);
			this.txtBaseAddress.MaxLength = 8;
			this.txtBaseAddress.Name = "txtBaseAddress";
			this.txtBaseAddress.Size = new System.Drawing.Size(62, 23);
			this.txtBaseAddress.TabIndex = 4;
			this.txtBaseAddress.Text = "00000000";
			this.txtBaseAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtBaseAddress.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxBaseAddress_Validating);
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
			this.buttonMemlayout.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.cmbProcesses.SelectedIndexChanged += new System.EventHandler(this.cmbProcesses_SelectedIndexChanged);
			// 
			// buttonProcesses
			// 
			this.buttonProcesses.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.buttonConnect.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.pgbProgress.Size = new System.Drawing.Size(752, 15);
			this.pgbProgress.TabIndex = 2;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(758, 24);
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
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(758, 602);
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
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
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
		private System.Windows.Forms.TextBox txtDumpMemAddrStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDumpMemAddrLength;
		private System.Windows.Forms.TextBox txtDumpMemFilename;
		private System.Windows.Forms.Button buttonDumpMemoryConsole;
		private System.Windows.Forms.Button buttonDumpMemoryFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCreateBaseCode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtBaseLength;
		private System.Windows.Forms.TextBox txtBaseAddress;
		private System.Windows.Forms.RichTextBox txtBaseCode;
		private System.Windows.Forms.Button buttonUseBaseCode;
		private System.Windows.Forms.Button buttonBaseClipboardPaste;
		private System.Windows.Forms.Button buttonBaseClipboardCopy;
	}
}

