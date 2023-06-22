namespace Application.Views.Forms
{
    partial class RegisterView
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
            RegisterButton = new Button();
            EnterPasswordBelowText = new Label();
            PasswordTextBox = new TextBox();
            ReEnterPasswordTextBox = new TextBox();
            SuspendLayout();
            // 
            // RegisterButton
            // 
            RegisterButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RegisterButton.Location = new Point(180, 221);
            RegisterButton.Margin = new Padding(7, 8, 7, 8);
            RegisterButton.Name = "RegisterButton";
            RegisterButton.Size = new Size(182, 60);
            RegisterButton.TabIndex = 0;
            RegisterButton.Text = "Register";
            RegisterButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            EnterPasswordBelowText.AutoSize = true;
            EnterPasswordBelowText.Location = new Point(29, 25);
            EnterPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            EnterPasswordBelowText.Size = new Size(482, 41);
            EnterPasswordBelowText.TabIndex = 1;
            EnterPasswordBelowText.Text = "Enter Your Chosen Password Below";
            EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
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
            // RegisterView
            // 
            AutoScaleDimensions = new SizeF(240F, 240F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(544, 309);
            Controls.Add(ReEnterPasswordTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(EnterPasswordBelowText);
            Controls.Add(RegisterButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "RegisterView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button RegisterButton;
        private Label EnterPasswordBelowText;
        private TextBox PasswordTextBox;
        private TextBox ReEnterPasswordTextBox;
    }
}