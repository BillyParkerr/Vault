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
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.EnterPasswordBelowText = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ReEnterPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OldPasswordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(72, 145);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(75, 22);
            this.ConfirmButton.TabIndex = 0;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            this.EnterPasswordBelowText.AutoSize = true;
            this.EnterPasswordBelowText.Location = new System.Drawing.Point(3, 73);
            this.EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            this.EnterPasswordBelowText.Size = new System.Drawing.Size(219, 15);
            this.EnterPasswordBelowText.TabIndex = 1;
            this.EnterPasswordBelowText.Text = "Enter Your New Chosen Password Below";
            this.EnterPasswordBelowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.PasswordTextBox.Location = new System.Drawing.Point(12, 91);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PlaceholderText = "Enter new password here.";
            this.PasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.PasswordTextBox.TabIndex = 2;
            this.PasswordTextBox.TabStop = false;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ReEnterPasswordTextBox
            // 
            this.ReEnterPasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReEnterPasswordTextBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ReEnterPasswordTextBox.Location = new System.Drawing.Point(12, 116);
            this.ReEnterPasswordTextBox.Name = "ReEnterPasswordTextBox";
            this.ReEnterPasswordTextBox.PlaceholderText = "Repeat new password here.";
            this.ReEnterPasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.ReEnterPasswordTextBox.TabIndex = 3;
            this.ReEnterPasswordTextBox.TabStop = false;
            this.ReEnterPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Your Old Password Below";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OldPasswordTextBox
            // 
            this.OldPasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OldPasswordTextBox.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.OldPasswordTextBox.Location = new System.Drawing.Point(12, 27);
            this.OldPasswordTextBox.Name = "OldPasswordTextBox";
            this.OldPasswordTextBox.PlaceholderText = "Enter old password here.";
            this.OldPasswordTextBox.Size = new System.Drawing.Size(200, 23);
            this.OldPasswordTextBox.TabIndex = 5;
            this.OldPasswordTextBox.TabStop = false;
            this.OldPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ChangePasswordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 179);
            this.Controls.Add(this.OldPasswordTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReEnterPasswordTextBox);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.EnterPasswordBelowText);
            this.Controls.Add(this.ConfirmButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ChangePasswordView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();

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