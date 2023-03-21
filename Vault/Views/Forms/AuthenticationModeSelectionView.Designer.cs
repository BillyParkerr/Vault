namespace Application.Views.Forms
{
    partial class AuthenticationModeSelectionView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EnterPasswordBelowText = new System.Windows.Forms.Label();
            this.PasswordModeButton = new System.Windows.Forms.Button();
            this.WindowsHelloModeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnterPasswordBelowText
            // 
            this.EnterPasswordBelowText.AutoSize = true;
            this.EnterPasswordBelowText.Location = new System.Drawing.Point(15, 9);
            this.EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            this.EnterPasswordBelowText.Size = new System.Drawing.Size(199, 15);
            this.EnterPasswordBelowText.TabIndex = 1;
            this.EnterPasswordBelowText.Text = "Choose your authentication method";
            this.EnterPasswordBelowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PasswordModeButton
            // 
            this.PasswordModeButton.Location = new System.Drawing.Point(12, 37);
            this.PasswordModeButton.Name = "PasswordModeButton";
            this.PasswordModeButton.Size = new System.Drawing.Size(98, 64);
            this.PasswordModeButton.TabIndex = 2;
            this.PasswordModeButton.Text = "Password";
            this.PasswordModeButton.UseVisualStyleBackColor = true;
            // 
            // WindowsHelloModeButton
            // 
            this.WindowsHelloModeButton.Location = new System.Drawing.Point(116, 37);
            this.WindowsHelloModeButton.Name = "WindowsHelloModeButton";
            this.WindowsHelloModeButton.Size = new System.Drawing.Size(98, 64);
            this.WindowsHelloModeButton.TabIndex = 3;
            this.WindowsHelloModeButton.Text = "Windows Hello";
            this.WindowsHelloModeButton.UseVisualStyleBackColor = true;
            // 
            // AuthenticationModeSelectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 113);
            this.Controls.Add(this.WindowsHelloModeButton);
            this.Controls.Add(this.PasswordModeButton);
            this.Controls.Add(this.EnterPasswordBelowText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AuthenticationModeSelectionView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auth Method";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label EnterPasswordBelowText;
        private Button PasswordModeButton;
        private Button WindowsHelloModeButton;
    }
}