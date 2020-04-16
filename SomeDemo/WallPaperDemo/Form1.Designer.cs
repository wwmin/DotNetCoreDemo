using System.Windows.Forms;

namespace WallPaperDemo
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_setwall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mainNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.oolStripMenuItemMinimize = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox_copyToClipBoard = new System.Windows.Forms.CheckBox();
            this.textBox_imageUrl = new WallPaperDemo.TextBoxExt();
            this.textBox_imageText = new WallPaperDemo.TextBoxExt();
            this.mainContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_setwall
            // 
            this.button_setwall.Location = new System.Drawing.Point(114, 99);
            this.button_setwall.Name = "button_setwall";
            this.button_setwall.Size = new System.Drawing.Size(482, 33);
            this.button_setwall.TabIndex = 2;
            this.button_setwall.Text = "设置桌面背景";
            this.button_setwall.UseVisualStyleBackColor = true;
            this.button_setwall.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "图片路径";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "图片上文字";
            // 
            // mainNotifyIcon
            // 
            this.mainNotifyIcon.ContextMenuStrip = this.mainContextMenuStrip;
            this.mainNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("mainNotifyIcon.Icon")));
            this.mainNotifyIcon.Text = "桌面背景";
            this.mainNotifyIcon.Visible = true;
            this.mainNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mainNotifyIcon_MouseDoubleClick);
            // 
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oolStripMenuItemMinimize,
            this.ToolStripMenuItemNormal,
            this.ToolStripMenuItemQuit});
            this.mainContextMenuStrip.Name = "mainContextMenuStrip";
            this.mainContextMenuStrip.Size = new System.Drawing.Size(124, 76);
            // 
            // oolStripMenuItemMinimize
            // 
            this.oolStripMenuItemMinimize.Name = "oolStripMenuItemMinimize";
            this.oolStripMenuItemMinimize.Size = new System.Drawing.Size(123, 24);
            this.oolStripMenuItemMinimize.Text = "最小化";
            this.oolStripMenuItemMinimize.Click += new System.EventHandler(this.ToolStripMenuItemMinimize_Click);
            // 
            // ToolStripMenuItemNormal
            // 
            this.ToolStripMenuItemNormal.Name = "ToolStripMenuItemNormal";
            this.ToolStripMenuItemNormal.Size = new System.Drawing.Size(123, 24);
            this.ToolStripMenuItemNormal.Text = "还原";
            this.ToolStripMenuItemNormal.Click += new System.EventHandler(this.ToolStripMenuItemNormal_Click);
            // 
            // ToolStripMenuItemQuit
            // 
            this.ToolStripMenuItemQuit.Name = "ToolStripMenuItemQuit";
            this.ToolStripMenuItemQuit.Size = new System.Drawing.Size(123, 24);
            this.ToolStripMenuItemQuit.Text = "退出";
            this.ToolStripMenuItemQuit.Click += new System.EventHandler(this.ToolStripMenuItemQuit_Click);
            // 
            // checkBox_copyToClipBoard
            // 
            this.checkBox_copyToClipBoard.AutoSize = true;
            this.checkBox_copyToClipBoard.Location = new System.Drawing.Point(657, 107);
            this.checkBox_copyToClipBoard.Name = "checkBox_copyToClipBoard";
            this.checkBox_copyToClipBoard.Size = new System.Drawing.Size(119, 19);
            this.checkBox_copyToClipBoard.TabIndex = 6;
            this.checkBox_copyToClipBoard.Text = "复制到剪切板";
            this.checkBox_copyToClipBoard.UseVisualStyleBackColor = true;
            this.checkBox_copyToClipBoard.CheckedChanged += new System.EventHandler(this.checkBox_copyToClipBoard_CheckedChanged);
            // 
            // textBox_imageUrl
            // 
            this.textBox_imageUrl.AllowDrop = true;
            this.textBox_imageUrl.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_imageUrl.Location = new System.Drawing.Point(114, 6);
            this.textBox_imageUrl.Name = "textBox_imageUrl";
            this.textBox_imageUrl.PlaceHolder = null;
            this.textBox_imageUrl.Size = new System.Drawing.Size(674, 30);
            this.textBox_imageUrl.TabIndex = 1;
            this.textBox_imageUrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxExt1_DragDrop);
            this.textBox_imageUrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxExt1_DragEnter);
            // 
            // textBox_imageText
            // 
            this.textBox_imageText.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_imageText.Location = new System.Drawing.Point(114, 54);
            this.textBox_imageText.Name = "textBox_imageText";
            this.textBox_imageText.PlaceHolder = null;
            this.textBox_imageText.Size = new System.Drawing.Size(674, 30);
            this.textBox_imageText.TabIndex = 0;
            this.textBox_imageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 144);
            this.Controls.Add(this.checkBox_copyToClipBoard);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_imageUrl);
            this.Controls.Add(this.textBox_imageText);
            this.Controls.Add(this.button_setwall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "下载图片并设置背景图";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_setwall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TextBoxExt textBox_imageText;
        private TextBoxExt textBox_imageUrl;
        private NotifyIcon mainNotifyIcon;
        private ContextMenuStrip mainContextMenuStrip;
        private ToolStripMenuItem ToolStripMenuItemQuit;
        private ToolStripMenuItem oolStripMenuItemMinimize;
        private ToolStripMenuItem ToolStripMenuItemNormal;
        private CheckBox checkBox_copyToClipBoard;
    }


  
}

