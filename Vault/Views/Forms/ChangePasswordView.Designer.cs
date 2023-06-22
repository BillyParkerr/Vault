namespace Application.Views.Forms
{
    partial class ChangePasswordView
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
            ReEnterPasswordTextBox = new TextBox();
            label1 = new Label();
            OldPasswordTextBox = new TextBox();
            SuspendLayout();
            // 
            // ConfirmButton
            // 
            ConfirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmButton.Location = new Point(175, 396);
            ConfirmButton.Margin = new Padding(7, 8, 7, 8);
            ConfirmButton.Name = "ConfirmButton";
            ConfirmButton.Size = new Size(182, 60);
            ConfirmButton.TabIndex = 0;
            ConfirmButton.Text = "Confirm";
            ConfirmButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            EnterPasswordBelowText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EnterPasswordBelowText.AutoSize = true;
            EnterPasswordBelowText.Location = new Point(-2, 200);
            EnterPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            EnterPasswordBelowText.Size = new Size(550, 41);
            EnterPasswordBelowText.TabIndex = 1;
            EnterPasswordBelowText.Text = "Enter Your New Chosen Password Below";
            EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            PasswordTextBox.ForeColor = SystemColors.ControlDark;
            PasswordTextBox.Location = new Point(29, 249);
            PasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PlaceholderText = "Enter new password here.";
            PasswordTextBox.Size = new Size(483, 47);
            PasswordTextBox.TabIndex = 2;
            PasswordTextBox.TabStop = false;
            PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ReEnterPasswordTextBox
            // 
            ReEnterPasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ReEnterPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            ReEnterPasswordTextBox.ForeColor = SystemColors.ControlDark;
            ReEnterPasswordTextBox.Location = new Point(29, 317);
            ReEnterPasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            ReEnterPasswordTextBox.Name = "ReEnterPasswordTextBox";
            ReEnterPasswordTextBox.PlaceholderText = "Repeat new password here.";
            ReEnterPasswordTextBox.Size = new Size(483, 47);
            ReEnterPasswordTextBox.TabIndex = 3;
            ReEnterPasswordTextBox.TabStop = false;
            ReEnterPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(61, 25);
            label1.Margin = new Padding(7, 0, 7, 0);
            label1.Name = "label1";
            label1.Size = new Size(430, 41);
            label1.TabIndex = 4;
            label1.Text = "Enter Your Old Password Below";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // OldPasswordTextBox
            // 
            OldPasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OldPasswordTextBox.BorderStyle = BorderStyle.FixedSingle;
            OldPasswordTextBox.ForeColor = SystemColors.ControlDark;
            OldPasswordTextBox.Location = new Point(29, 74);
            OldPasswordTextBox.Margin = new Padding(7, 8, 7, 8);
            OldPasswordTextBox.Name = "OldPasswordTextBox";
            OldPasswordTextBox.PlaceholderText = "Enter old password here.";
            OldPasswordTextBox.Size = new Size(483, 47);
            OldPasswordTextBox.TabIndex = 5;
            OldPasswordTextBox.TabStop = false;
            OldPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ChangePasswordView
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 489);
            Controls.Add(OldPasswordTextBox);
            Controls.Add(label1);
            Controls.Add(ReEnterPasswordTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(EnterPasswordBelowText);
            Controls.Add(ConfirmButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "ChangePasswordView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Change Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ConfirmButton;
        private Label EnterPasswordBelowText;
        private TextBox PasswordTextBox;
        private TextBox ReEnterPasswordTextBox;
        private Label label1;
        private TextBox OldPasswordTextBox;
    }
}