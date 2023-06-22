namespace Application.Views.Forms
{
    partial class VerifyPasswordView
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
            VerifyButton = new Button();
            EnterPasswordBelowText = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // VerifyButton
            // 
            VerifyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            VerifyButton.Location = new Point(180, 220);
            VerifyButton.Margin = new Padding(7, 8, 7, 8);
            VerifyButton.Name = "VerifyButton";
            VerifyButton.Size = new Size(182, 59);
            VerifyButton.TabIndex = 0;
            VerifyButton.Text = "Verify";
            VerifyButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            EnterPasswordBelowText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EnterPasswordBelowText.AutoSize = true;
            EnterPasswordBelowText.Location = new Point(78, 49);
            EnterPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            EnterPasswordBelowText.Size = new Size(374, 41);
            EnterPasswordBelowText.TabIndex = 1;
            EnterPasswordBelowText.Text = "Enter Your Password Below";
            EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.ForeColor = SystemColors.ControlDark;
            textBox1.Location = new Point(29, 123);
            textBox1.Margin = new Padding(7, 8, 7, 8);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Enter password here.";
            textBox1.Size = new Size(483, 47);
            textBox1.TabIndex = 2;
            textBox1.TabStop = false;
            textBox1.UseSystemPasswordChar = true;
            // 
            // VerifyPasswordView
            // 
            AutoScaleDimensions = new SizeF(240F, 240F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(544, 310);
            Controls.Add(textBox1);
            Controls.Add(EnterPasswordBelowText);
            Controls.Add(VerifyButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "VerifyPasswordView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Verify Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button VerifyButton;
        private Label EnterPasswordBelowText;
        private TextBox textBox1;
    }
}