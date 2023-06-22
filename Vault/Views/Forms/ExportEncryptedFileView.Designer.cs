namespace Application.Views.Forms
{
    partial class ExportEncryptedFileView
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
            EnterPasswordBelowText = new Label();
            PasswordTextBox = new TextBox();
            SuspendLayout();
            // 
            // ConfirmButton
            // 
            ConfirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmButton.Location = new Point(180, 221);
            ConfirmButton.Margin = new Padding(7, 8, 7, 8);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new Size(182, 77);
            ConfirmButton.TabIndex = 0;
            ConfirmButton.Text = "Confirm";
            ConfirmButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            EnterPasswordBelowText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EnterPasswordBelowText.Location = new Point(29, 25);
            EnterPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            EnterPasswordBelowText.Size = new Size(486, 109);
            EnterPasswordBelowText.TabIndex = 1;
            EnterPasswordBelowText.Text = "Please enter a password for this file to be encrypted with.";
            EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            PasswordTextBox.ForeColor = SystemColors.ControlDark;
            PasswordTextBox.Location = new Point(32, 142);
            PasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PlaceholderText = "Enter password here";
            PasswordTextBox.Size = new Size(483, 47);
            PasswordTextBox.TabIndex = 3;
            PasswordTextBox.TabStop = false;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ExportEncryptedFileView
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 309);
            Controls.Add(PasswordTextBox);
            Controls.Add(EnterPasswordBelowText);
            Controls.Add(ConfirmButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "ExportEncryptedFileView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Export File";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ConfirmButton;
        private Label EnterPasswordBelowText;
        private TextBox PasswordTextBox;
    }
}