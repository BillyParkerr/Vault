namespace Application.Views.Forms
{
    partial class SettingsView
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
            Logo = new Label();
            BasicModeButton = new Button();
            AdvancedModeButton = new Button();
            WindowsHelloModeButton = new Button();
            PasswordModeButton = new Button();
            DeleteUploadedFilesButton = new Button();
            ChangeDefaultDownloadLocationButton = new Button();
            ConfirmSettingsButton = new Button();
            ChangePasswordButton = new Button();
            SuspendLayout();
            // 
            // Logo
            // 
            Logo.AutoSize = true;
            Logo.BackColor = Color.FromArgb(64, 64, 64);
            Logo.Font = new Font("Segoe UI Variable Text", 22F, FontStyle.Regular, GraphicsUnit.Point);
            Logo.ForeColor = Color.Transparent;
            Logo.Location = new Point(29, 25);
            Logo.Margin = new Padding(7, 0, 7, 0);
            Logo.Name = "Logo";
            Logo.Size = new Size(784, 99);
            Logo.TabIndex = 3;
            Logo.Text = "Personal Vault Settings";
            // 
            // BasicModeButton
            // 
            BasicModeButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            BasicModeButton.Location = new Point(29, 189);
            BasicModeButton.Margin = new Padding(7, 8, 7, 8);
            BasicModeButton.Name = "BasicModeButton";
            BasicModeButton.Size = new Size(481, 213);
            BasicModeButton.TabIndex = 4;
            BasicModeButton.Text = "Basic Mode";
            BasicModeButton.UseVisualStyleBackColor = true;
            // 
            // AdvancedModeButton
            // 
            AdvancedModeButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            AdvancedModeButton.Location = new Point(571, 189);
            AdvancedModeButton.Margin = new Padding(7, 8, 7, 8);
            AdvancedModeButton.Name = "AdvancedModeButton";
            AdvancedModeButton.Size = new Size(481, 213);
            AdvancedModeButton.TabIndex = 5;
            AdvancedModeButton.Text = "Advanced Mode";
            AdvancedModeButton.UseVisualStyleBackColor = true;
            // 
            // WindowsHelloModeButton
            // 
            WindowsHelloModeButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            WindowsHelloModeButton.Location = new Point(29, 418);
            WindowsHelloModeButton.Margin = new Padding(7, 8, 7, 8);
            WindowsHelloModeButton.Name = "WindowsHelloModeButton";
            WindowsHelloModeButton.Size = new Size(481, 213);
            WindowsHelloModeButton.TabIndex = 6;
            WindowsHelloModeButton.Text = "Windows Hello Mode";
            WindowsHelloModeButton.UseVisualStyleBackColor = true;
            // 
            // PasswordModeButton
            // 
            PasswordModeButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            PasswordModeButton.Location = new Point(571, 418);
            PasswordModeButton.Margin = new Padding(7, 8, 7, 8);
            PasswordModeButton.Name = "PasswordModeButton";
            PasswordModeButton.Size = new Size(481, 213);
            PasswordModeButton.TabIndex = 7;
            PasswordModeButton.Text = "Password Mode";
            PasswordModeButton.UseVisualStyleBackColor = true;
            // 
            // DeleteUploadedFilesButton
            // 
            DeleteUploadedFilesButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            DeleteUploadedFilesButton.Location = new Point(29, 648);
            DeleteUploadedFilesButton.Margin = new Padding(7, 8, 7, 8);
            DeleteUploadedFilesButton.Name = "DeleteUploadedFilesButton";
            DeleteUploadedFilesButton.Size = new Size(481, 213);
            DeleteUploadedFilesButton.TabIndex = 8;
            DeleteUploadedFilesButton.Text = "Deletion of uploaded files [No]";
            DeleteUploadedFilesButton.UseVisualStyleBackColor = true;
            // 
            // ChangeDefaultDownloadLocationButton
            // 
            ChangeDefaultDownloadLocationButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            ChangeDefaultDownloadLocationButton.Location = new Point(29, 877);
            ChangeDefaultDownloadLocationButton.Margin = new Padding(7, 8, 7, 8);
            ChangeDefaultDownloadLocationButton.Name = "ChangeDefaultDownloadLocationButton";
            ChangeDefaultDownloadLocationButton.Size = new Size(481, 213);
            ChangeDefaultDownloadLocationButton.TabIndex = 9;
            ChangeDefaultDownloadLocationButton.Text = "Change Default Download Location";
            ChangeDefaultDownloadLocationButton.UseVisualStyleBackColor = true;
            // 
            // ConfirmSettingsButton
            // 
            ConfirmSettingsButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            ConfirmSettingsButton.Location = new Point(29, 1391);
            ConfirmSettingsButton.Margin = new Padding(7, 8, 7, 8);
            ConfirmSettingsButton.Name = "ConfirmSettingsButton";
            ConfirmSettingsButton.Size = new Size(1022, 213);
            ConfirmSettingsButton.TabIndex = 11;
            ConfirmSettingsButton.Text = "Confirm Settings";
            ConfirmSettingsButton.UseVisualStyleBackColor = true;
            // 
            // ChangePasswordButton
            // 
            ChangePasswordButton.Font = new Font("Segoe UI Variable Text", 15F, FontStyle.Regular, GraphicsUnit.Point);
            ChangePasswordButton.Location = new Point(29, 1107);
            ChangePasswordButton.Margin = new Padding(7, 8, 7, 8);
            ChangePasswordButton.Name = "ChangePasswordButton";
            ChangePasswordButton.Size = new Size(481, 213);
            ChangePasswordButton.TabIndex = 12;
            ChangePasswordButton.Text = "Change Password";
            ChangePasswordButton.UseVisualStyleBackColor = true;
            // 
            // SettingsView
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            ClientSize = new Size(1083, 1624);
            Controls.Add(ChangePasswordButton);
            Controls.Add(ConfirmSettingsButton);
            Controls.Add(ChangeDefaultDownloadLocationButton);
            Controls.Add(DeleteUploadedFilesButton);
            Controls.Add(PasswordModeButton);
            Controls.Add(WindowsHelloModeButton);
            Controls.Add(AdvancedModeButton);
            Controls.Add(BasicModeButton);
            Controls.Add(Logo);
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "SettingsView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SettingsView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Logo;
        private Button BasicModeButton;
        private Button AdvancedModeButton;
        private Button WindowsHelloModeButton;
        private Button PasswordModeButton;
        private Button DeleteUploadedFilesButton;
        private Button ChangeDefaultDownloadLocationButton;
        private Button ConfirmSettingsButton;
        private Button ChangePasswordButton;
    }
}