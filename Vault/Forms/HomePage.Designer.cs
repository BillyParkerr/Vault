namespace Application.Forms
{
    partial class HomePage
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
            this.HomepageList = new System.Windows.Forms.ListView();
            this.FileName = new System.Windows.Forms.ColumnHeader();
            this.FileType = new System.Windows.Forms.ColumnHeader();
            this.FileSize = new System.Windows.Forms.ColumnHeader();
            this.UploadButton = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.Label();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HomepageList
            // 
            this.HomepageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.FileType,
            this.FileSize});
            this.HomepageList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.HomepageList.Location = new System.Drawing.Point(216, 12);
            this.HomepageList.Name = "HomepageList";
            this.HomepageList.Size = new System.Drawing.Size(836, 601);
            this.HomepageList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.HomepageList.TabIndex = 0;
            this.HomepageList.UseCompatibleStateImageBehavior = false;
            this.HomepageList.View = System.Windows.Forms.View.Details;
            this.HomepageList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.HomepageList_ColumnWidthChanging);
            this.HomepageList.SelectedIndexChanged += new System.EventHandler(this.HomepageList_SelectedIndexChanged);
            // 
            // FileName
            // 
            this.FileName.Text = "File Name";
            this.FileName.Width = 650;
            // 
            // FileType
            // 
            this.FileType.Text = "Type";
            this.FileType.Width = 80;
            // 
            // FileSize
            // 
            this.FileSize.Text = "Size";
            this.FileSize.Width = 80;
            // 
            // UploadButton
            // 
            this.UploadButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UploadButton.Location = new System.Drawing.Point(12, 75);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(198, 83);
            this.UploadButton.TabIndex = 1;
            this.UploadButton.Text = "Add File To Vault";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // Logo
            // 
            this.Logo.AutoSize = true;
            this.Logo.Font = new System.Drawing.Font("Segoe UI Variable Text", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Logo.ForeColor = System.Drawing.Color.Transparent;
            this.Logo.Location = new System.Drawing.Point(7, 12);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(203, 40);
            this.Logo.TabIndex = 2;
            this.Logo.Text = "Personal Vault";
            this.Logo.Click += new System.EventHandler(this.label1_Click);
            // 
            // DownloadButton
            // 
            this.DownloadButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DownloadButton.Location = new System.Drawing.Point(12, 164);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(198, 83);
            this.DownloadButton.TabIndex = 3;
            this.DownloadButton.Text = "Download Selected File";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1064, 625);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.HomepageList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "HomePage";
            this.Text = "Vault";
            this.Load += new System.EventHandler(this.HomePage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView HomepageList;
        private ColumnHeader FileName;
        private ColumnHeader FileType;
        private ColumnHeader FileSize;
        private Button UploadButton;
        private Label Logo;
        private Button DownloadButton;
    }
}