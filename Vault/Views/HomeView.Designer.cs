﻿namespace Application.Views
{
    partial class HomeView
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
            this.UploadButton = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.Label();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.UploadFolderButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.SearchBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // UploadButton
            // 
            this.UploadButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UploadButton.Location = new System.Drawing.Point(29, 191);
            this.UploadButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(481, 213);
            this.UploadButton.TabIndex = 1;
            this.UploadButton.Text = "Add File To Vault";
            this.UploadButton.UseVisualStyleBackColor = true;
            // 
            // Logo
            // 
            this.Logo.AutoSize = true;
            this.Logo.Font = new System.Drawing.Font("Segoe UI Variable Text", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Logo.ForeColor = System.Drawing.Color.Transparent;
            this.Logo.Location = new System.Drawing.Point(17, 30);
            this.Logo.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(501, 99);
            this.Logo.TabIndex = 2;
            this.Logo.Text = "Personal Vault";
            // 
            // DownloadButton
            // 
            this.DownloadButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DownloadButton.Location = new System.Drawing.Point(29, 651);
            this.DownloadButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(481, 213);
            this.DownloadButton.TabIndex = 3;
            this.DownloadButton.Text = "Download Selected File";
            this.DownloadButton.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView.Location = new System.Drawing.Point(566, 191);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 102;
            this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(2157, 1896);
            this.dataGridView.TabIndex = 4;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DeleteButton.Location = new System.Drawing.Point(29, 1110);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(481, 213);
            this.DeleteButton.TabIndex = 5;
            this.DeleteButton.Text = "Delete Selected File";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OpenFileButton.Location = new System.Drawing.Point(29, 880);
            this.OpenFileButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(481, 213);
            this.OpenFileButton.TabIndex = 6;
            this.OpenFileButton.Text = "Open Selected File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            // 
            // UploadFolderButton
            // 
            this.UploadFolderButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UploadFolderButton.Location = new System.Drawing.Point(29, 421);
            this.UploadFolderButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.UploadFolderButton.Name = "UploadFolderButton";
            this.UploadFolderButton.Size = new System.Drawing.Size(481, 213);
            this.UploadFolderButton.TabIndex = 7;
            this.UploadFolderButton.Text = "Add Folder To Vault";
            this.UploadFolderButton.UseVisualStyleBackColor = true;
            // 
            // ImportButton
            // 
            this.ImportButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ImportButton.Location = new System.Drawing.Point(29, 1339);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(231, 213);
            this.ImportButton.TabIndex = 8;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            this.ExportButton.Font = new System.Drawing.Font("Segoe UI Variable Text", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExportButton.Location = new System.Drawing.Point(279, 1339);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(231, 213);
            this.ExportButton.TabIndex = 9;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            // 
            // SearchBox
            // 
            this.SearchBox.Location = new System.Drawing.Point(567, 126);
            this.SearchBox.MaxLength = 100;
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.PlaceholderText = "Search";
            this.SearchBox.Size = new System.Drawing.Size(723, 47);
            this.SearchBox.TabIndex = 10;
            // 
            // HomeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(2739, 2104);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.UploadFolderButton);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.UploadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.Name = "HomeView";
            this.Text = "Vault";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button UploadButton;
        private Label Logo;
        private Button DownloadButton;
        private DataGridView dataGridView;
        private Button DeleteButton;
        private Button OpenFileButton;
        private Button UploadFolderButton;
        private Button ImportButton;
        private Button ExportButton;
        private TextBox SearchBox;
    }
}