namespace FBXJapaneseConverter
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tboxFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridConv = new System.Windows.Forms.DataGridView();
            this.ColumnS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConvSave = new System.Windows.Forms.Button();
            this.btnTranslate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridConv)).BeginInit();
            this.SuspendLayout();
            // 
            // tboxFile
            // 
            this.tboxFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxFile.Location = new System.Drawing.Point(69, 17);
            this.tboxFile.Name = "tboxFile";
            this.tboxFile.ReadOnly = true;
            this.tboxFile.Size = new System.Drawing.Size(716, 19);
            this.tboxFile.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ファイル名";
            // 
            // gridConv
            // 
            this.gridConv.AllowUserToAddRows = false;
            this.gridConv.AllowUserToDeleteRows = false;
            this.gridConv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridConv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridConv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnS,
            this.ColumnD});
            this.gridConv.Location = new System.Drawing.Point(14, 113);
            this.gridConv.Name = "gridConv";
            this.gridConv.RowHeadersVisible = false;
            this.gridConv.RowTemplate.Height = 21;
            this.gridConv.Size = new System.Drawing.Size(771, 553);
            this.gridConv.TabIndex = 2;
            // 
            // ColumnS
            // 
            this.ColumnS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnS.HeaderText = "元文字列";
            this.ColumnS.Name = "ColumnS";
            this.ColumnS.ReadOnly = true;
            this.ColumnS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnD
            // 
            this.ColumnD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnD.HeaderText = "変換後文字列";
            this.ColumnD.Name = "ColumnD";
            // 
            // btnConvSave
            // 
            this.btnConvSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvSave.Location = new System.Drawing.Point(710, 42);
            this.btnConvSave.Name = "btnConvSave";
            this.btnConvSave.Size = new System.Drawing.Size(75, 23);
            this.btnConvSave.TabIndex = 3;
            this.btnConvSave.Text = "変換＆保存";
            this.btnConvSave.UseVisualStyleBackColor = true;
            this.btnConvSave.Click += new System.EventHandler(this.btnConvSave_Click);
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(710, 72);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(75, 23);
            this.btnTranslate.TabIndex = 4;
            this.btnTranslate.Text = "翻訳";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 678);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnConvSave);
            this.Controls.Add(this.gridConv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxFile);
            this.Name = "Main";
            this.Text = "FBX Japanese Converter";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.gridConv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tboxFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gridConv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnD;
        private System.Windows.Forms.Button btnConvSave;
        private System.Windows.Forms.Button btnTranslate;
    }
}

