namespace BoxApiSample.Forms
{
    partial class FileUploadSampleForm
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
            this.button1 = new System.Windows.Forms.Button();
            this._textBoxRootDirectoryId = new System.Windows.Forms.TextBox();
            this._labelRootDirectoryId = new System.Windows.Forms.Label();
            this._pictureBoxRootDirectoryId = new System.Windows.Forms.PictureBox();
            this._labelRootDirectoryIdSummary = new System.Windows.Forms.Label();
            this._labelUploadDirectorySummary = new System.Windows.Forms.Label();
            this._textBoxUploadDirectory = new System.Windows.Forms.TextBox();
            this._buttonUploadDirectoryRef = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._buttonUpload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxRootDirectoryId)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(580, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _textBoxRootDirectoryId
            // 
            this._textBoxRootDirectoryId.Location = new System.Drawing.Point(73, 84);
            this._textBoxRootDirectoryId.Name = "_textBoxRootDirectoryId";
            this._textBoxRootDirectoryId.Size = new System.Drawing.Size(128, 22);
            this._textBoxRootDirectoryId.TabIndex = 1;
            this._textBoxRootDirectoryId.Text = "163142489157";
            // 
            // _labelRootDirectoryId
            // 
            this._labelRootDirectoryId.AutoSize = true;
            this._labelRootDirectoryId.Location = new System.Drawing.Point(46, 87);
            this._labelRootDirectoryId.Name = "_labelRootDirectoryId";
            this._labelRootDirectoryId.Size = new System.Drawing.Size(21, 15);
            this._labelRootDirectoryId.TabIndex = 2;
            this._labelRootDirectoryId.Text = "ID";
            // 
            // _pictureBoxRootDirectoryId
            // 
            this._pictureBoxRootDirectoryId.Image = global::BoxApiSample.Properties.Resources.directory_id;
            this._pictureBoxRootDirectoryId.Location = new System.Drawing.Point(30, 122);
            this._pictureBoxRootDirectoryId.Name = "_pictureBoxRootDirectoryId";
            this._pictureBoxRootDirectoryId.Size = new System.Drawing.Size(674, 170);
            this._pictureBoxRootDirectoryId.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBoxRootDirectoryId.TabIndex = 3;
            this._pictureBoxRootDirectoryId.TabStop = false;
            // 
            // _labelRootDirectoryIdSummary
            // 
            this._labelRootDirectoryIdSummary.AutoSize = true;
            this._labelRootDirectoryIdSummary.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._labelRootDirectoryIdSummary.Location = new System.Drawing.Point(25, 26);
            this._labelRootDirectoryIdSummary.Name = "_labelRootDirectoryIdSummary";
            this._labelRootDirectoryIdSummary.Size = new System.Drawing.Size(385, 28);
            this._labelRootDirectoryIdSummary.TabIndex = 4;
            this._labelRootDirectoryIdSummary.Text = "1. フォルダのIDを入力してください";
            // 
            // _labelUploadDirectorySummary
            // 
            this._labelUploadDirectorySummary.AutoSize = true;
            this._labelUploadDirectorySummary.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._labelUploadDirectorySummary.Location = new System.Drawing.Point(25, 330);
            this._labelUploadDirectorySummary.Name = "_labelUploadDirectorySummary";
            this._labelUploadDirectorySummary.Size = new System.Drawing.Size(483, 28);
            this._labelUploadDirectorySummary.TabIndex = 5;
            this._labelUploadDirectorySummary.Text = "2. アップロードするフォルダを選んでください";
            // 
            // _textBoxUploadDirectory
            // 
            this._textBoxUploadDirectory.Location = new System.Drawing.Point(73, 373);
            this._textBoxUploadDirectory.Name = "_textBoxUploadDirectory";
            this._textBoxUploadDirectory.Size = new System.Drawing.Size(294, 22);
            this._textBoxUploadDirectory.TabIndex = 6;
            // 
            // _buttonUploadDirectoryRef
            // 
            this._buttonUploadDirectoryRef.Location = new System.Drawing.Point(373, 368);
            this._buttonUploadDirectoryRef.Name = "_buttonUploadDirectoryRef";
            this._buttonUploadDirectoryRef.Size = new System.Drawing.Size(74, 30);
            this._buttonUploadDirectoryRef.TabIndex = 7;
            this._buttonUploadDirectoryRef.Text = "...";
            this._buttonUploadDirectoryRef.UseVisualStyleBackColor = true;
            this._buttonUploadDirectoryRef.Click += new System.EventHandler(this._buttonUploadDirectoryRef_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(25, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "3. アップロードします";
            // 
            // _buttonUpload
            // 
            this._buttonUpload.Location = new System.Drawing.Point(77, 469);
            this._buttonUpload.Name = "_buttonUpload";
            this._buttonUpload.Size = new System.Drawing.Size(310, 40);
            this._buttonUpload.TabIndex = 9;
            this._buttonUpload.Text = "アップロード！！";
            this._buttonUpload.UseVisualStyleBackColor = true;
            // 
            // FileUploadSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 529);
            this.Controls.Add(this._buttonUpload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._buttonUploadDirectoryRef);
            this.Controls.Add(this._textBoxUploadDirectory);
            this.Controls.Add(this._labelUploadDirectorySummary);
            this.Controls.Add(this._labelRootDirectoryIdSummary);
            this.Controls.Add(this._pictureBoxRootDirectoryId);
            this.Controls.Add(this._labelRootDirectoryId);
            this.Controls.Add(this._textBoxRootDirectoryId);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FileUploadSampleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOX にファイルをアップします";
            ((System.ComponentModel.ISupportInitialize)(this._pictureBoxRootDirectoryId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _textBoxRootDirectoryId;
        private System.Windows.Forms.Label _labelRootDirectoryId;
        private System.Windows.Forms.PictureBox _pictureBoxRootDirectoryId;
        private System.Windows.Forms.Label _labelRootDirectoryIdSummary;
        private System.Windows.Forms.Label _labelUploadDirectorySummary;
        private System.Windows.Forms.TextBox _textBoxUploadDirectory;
        private System.Windows.Forms.Button _buttonUploadDirectoryRef;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _buttonUpload;
    }
}

