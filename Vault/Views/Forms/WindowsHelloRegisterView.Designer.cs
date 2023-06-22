namespace Application.Views.Forms
{
    partial class WindowsHelloRegisterView
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
            ConfirmButton = new Button();
            EnterBackupPasswordBelowText = new Label();
            PasswordTextBox = new TextBox();
            ReEnterPasswordTextBox = new TextBox();
            SuspendLayout();
            // 
            // ConfirmButton
            // 
            ConfirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmButton.Location = new Point(180, 221);
            ConfirmButton.Margin = new Padding(7, 8, 7, 8);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new Size(182, 60);
            ConfirmButton.TabIndex = 0;
            ConfirmButton.Text = "Confirm";
            ConfirmButton.UseVisualStyleBackColor = true;
            // 
            // EnterBackupPasswordBelowText
            // 
            EnterBackupPasswordBelowText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EnterBackupPasswordBelowText.AutoSize = true;
            EnterBackupPasswordBelowText.Location = new Point(56, 25);
            EnterBackupPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            EnterBackupPasswordBelowText.Name = "EnterBackupPasswordBelowText";
            EnterBackupPasswordBelowText.Size = new Size(434, 41);
            EnterBackupPasswordBelowText.TabIndex = 1;
            EnterBackupPasswordBelowText.Text = "Enter a Backup Password Below";
            EnterBackupPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            PasswordTextBox.ForeColor = SystemColors.ControlDark;
            PasswordTextBox.Location = new Point(29, 74);
            PasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PlaceholderText = "Enter password here.";
            PasswordTextBox.Size = new Size(483, 47);
            PasswordTextBox.TabIndex = 2;
            PasswordTextBox.TabStop = false;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ReEnterPasswordTextBox
            // 
            ReEnterPasswordTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ReEnterPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            ReEnterPasswordTextBox.ForeColor = SystemColors.ControlDark;
            ReEnterPasswordTextBox.Location = new Point(29, 142);
            ReEnterPasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            ReEnterPasswordTextBox.Name = "ReEnterPasswordTextBox";
            ReEnterPasswordTextBox.PlaceholderText = "Repeat password here.";
            ReEnterPasswordTextBox.Size = new Size(483, 47);
            ReEnterPasswordTextBox.TabIndex = 3;
            ReEnterPasswordTextBox.TabStop = false;
            ReEnterPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // WindowsHelloRegisterView
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 309);
            Controls.Add(ReEnterPasswordTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(EnterBackupPasswordBelowText);
            Controls.Add(ConfirmButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "WindowsHelloRegisterView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Backup Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ConfirmButton;
        private Label EnterBackupPasswordBelowText;
        private TextBox PasswordTextBox;
        private TextBox ReEnterPasswordTextBox;
    }
}