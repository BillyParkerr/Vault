namespace Application.Views.Forms
{
    partial class LoginView
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
            this.LoginButton = new Button();
            this.EnterPasswordBelowText = new Label();
            this.textBox1 = new TextBox();
            SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.LoginButton.Location = new Point(180, 220);
            this.LoginButton.Margin = new Padding(7, 8, 7, 8);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new Size(182, 59);
            this.LoginButton.TabIndex = 0;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            // 
            // EnterPasswordBelowText
            // 
            this.EnterPasswordBelowText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.EnterPasswordBelowText.AutoSize = true;
            this.EnterPasswordBelowText.Location = new Point(78, 49);
            this.EnterPasswordBelowText.Margin = new Padding(7, 0, 7, 0);
            this.EnterPasswordBelowText.Name = "EnterPasswordBelowText";
            this.EnterPasswordBelowText.Size = new Size(374, 41);
            this.EnterPasswordBelowText.TabIndex = 1;
            this.EnterPasswordBelowText.Text = "Enter Your Password Below";
            this.EnterPasswordBelowText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.textBox1.BorderStyle = BorderStyle.FixedSingle;
            this.textBox1.ForeColor = SystemColors.ControlDark;
            this.textBox1.Location = new Point(29, 123);
            this.textBox1.Margin = new Padding(7, 8, 7, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "Enter password here.";
            this.textBox1.Size = new Size(483, 47);
            this.textBox1.TabIndex = 2;
            this.textBox1.TabStop = false;
            this.textBox1.UseSystemPasswordChar = true;
            // 
            // LoginView
            // 
            AutoScaleDimensions = new SizeF(240F, 240F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(544, 310);
            Controls.Add(this.textBox1);
            Controls.Add(this.EnterPasswordBelowText);
            Controls.Add(this.LoginButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7, 8, 7, 8);
            MaximizeBox = false;
            Name = "LoginView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoginButton;
        private Label EnterPasswordBelowText;
        private TextBox textBox1;
    }
}