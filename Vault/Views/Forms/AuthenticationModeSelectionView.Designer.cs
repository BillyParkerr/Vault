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
            EnterPasswordBelowText = new Label();
            PasswordModeButton = new Button();
            WindowsHelloModeButton = new Button();
            SuspendLayout();
            // 
            // EnterPasswordBelowText
            // 
            EnterPasswordBelowText.Anchor = AnchorStyles.Top;
            EnterPasswordBelowText.AutoSize = true;
            EnterPasswordBelowText.Location = new Point(15, 26);
            EnterPasswordBelowText.Margin = new Padding(6, 0, 6, 0);
            EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            EnterPasswordBelowText.Size = new Size(498, 41);
            EnterPasswordBelowText.TabIndex = 1;
            EnterPasswordBelowText.Text = "Choose your authentication method";
            EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordModeButton
            // 
            PasswordModeButton.Location = new Point(28, 98);
            PasswordModeButton.Margin = new Padding(6, 8, 6, 8);
            PasswordModeButton.Name = "PasswordModeButton";
            PasswordModeButton.Size = new Size(224, 170);
            PasswordModeButton.TabIndex = 2;
            PasswordModeButton.Text = "Password";
            PasswordModeButton.UseVisualStyleBackColor = true;
            // 
            // WindowsHelloModeButton
            // 
            WindowsHelloModeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            WindowsHelloModeButton.Location = new Point(278, 98);
            WindowsHelloModeButton.Margin = new Padding(6, 8, 6, 8);
            WindowsHelloModeButton.Name = "WindowsHelloModeButton";
            WindowsHelloModeButton.Size = new Size(224, 170);
            WindowsHelloModeButton.TabIndex = 3;
            WindowsHelloModeButton.Text = "Windows Hello";
            WindowsHelloModeButton.UseVisualStyleBackColor = true;
            // 
            // AuthenticationModeSelectionView
            // 
            AutoScaleDimensions = new SizeF(240F, 240F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(526, 302);
            Controls.Add(WindowsHelloModeButton);
            Controls.Add(PasswordModeButton);
            Controls.Add(EnterPasswordBelowText);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6, 8, 6, 8);
            MaximizeBox = false;
            Name = "AuthenticationModeSelectionView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auth Method";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label EnterPasswordBelowText;
        private Button PasswordModeButton;
        private Button WindowsHelloModeButton;
    }
}