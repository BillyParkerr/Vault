namespace Application.Views
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
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.EnterBackupPasswordBelowText = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ReEnterPasswordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(74, 81);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(75, 22);
            this.ConfirmButton.TabIndex = 0;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            // 
            // EnterBackupPasswordBelowText
            // 
            this.EnterBackupPasswordBelowText.AutoSize = true;
            this.EnterBackupPasswordBelowText.Location = new System.Drawing.Point(23, 9);
            this.EnterBackupPasswordBelowText.Name = "EnterBackupPasswordBelowText";
            this.EnterBackupPasswordBelowText.Size = new System.Drawing.Size(173, 15);
            this.EnterBackupPasswordBelowText.TabIndex = 1;
            this.EnterBackupPasswordBelowText.Text = "Enter a Backup Password Below";
            this.EnterBackupPasswordBelowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.PasswordTextBox.Location = new System.Drawing.Point(12, 27);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PlaceholderText = "Enter password here.";
            this.PasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.PasswordTextBox.TabIndex = 2;
            this.PasswordTextBox.TabStop = false;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ReEnterPasswordTextBox
            // 
            this.ReEnterPasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReEnterPasswordTextBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ReEnterPasswordTextBox.Location = new System.Drawing.Point(12, 52);
            this.ReEnterPasswordTextBox.Name = "ReEnterPasswordTextBox";
            this.ReEnterPasswordTextBox.PlaceholderText = "Repeat password here.";
            this.ReEnterPasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.ReEnterPasswordTextBox.TabIndex = 3;
            this.ReEnterPasswordTextBox.TabStop = false;
            this.ReEnterPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // WindowsHelloRegisterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 113);
            this.Controls.Add(this.ReEnterPasswordTextBox);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.EnterBackupPasswordBelowText);
            this.Controls.Add(this.ConfirmButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WindowsHelloRegisterView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ConfirmButton;
        private Label EnterBackupPasswordBelowText;
        private TextBox PasswordTextBox;
        private TextBox ReEnterPasswordTextBox;
    }
}