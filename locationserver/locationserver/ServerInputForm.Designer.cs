namespace locationserver
{
    partial class ServerInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.saveCheckBox = new System.Windows.Forms.CheckBox();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.locationLabel = new System.Windows.Forms.Label();
            this.serverLogCheckBox = new System.Windows.Forms.CheckBox();
            this.debugModeCheckBox = new System.Windows.Forms.CheckBox();
            this.timeoutTextBox = new System.Windows.Forms.TextBox();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.launchServerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(92, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(103, 17);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Server Options";
            // 
            // saveCheckBox
            // 
            this.saveCheckBox.AutoSize = true;
            this.saveCheckBox.Location = new System.Drawing.Point(12, 77);
            this.saveCheckBox.Name = "saveCheckBox";
            this.saveCheckBox.Size = new System.Drawing.Size(100, 21);
            this.saveCheckBox.TabIndex = 1;
            this.saveCheckBox.Text = "Save to file";
            this.saveCheckBox.UseVisualStyleBackColor = true;
            // 
            // locationTextBox
            // 
            this.locationTextBox.Location = new System.Drawing.Point(118, 75);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(152, 22);
            this.locationTextBox.TabIndex = 2;
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(148, 46);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(92, 17);
            this.locationLabel.TabIndex = 3;
            this.locationLabel.Text = "File Location:";
            // 
            // serverLogCheckBox
            // 
            this.serverLogCheckBox.AutoSize = true;
            this.serverLogCheckBox.Location = new System.Drawing.Point(12, 104);
            this.serverLogCheckBox.Name = "serverLogCheckBox";
            this.serverLogCheckBox.Size = new System.Drawing.Size(164, 21);
            this.serverLogCheckBox.TabIndex = 4;
            this.serverLogCheckBox.Text = "Generate Server Log";
            this.serverLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // debugModeCheckBox
            // 
            this.debugModeCheckBox.AutoSize = true;
            this.debugModeCheckBox.Location = new System.Drawing.Point(12, 131);
            this.debugModeCheckBox.Name = "debugModeCheckBox";
            this.debugModeCheckBox.Size = new System.Drawing.Size(111, 21);
            this.debugModeCheckBox.TabIndex = 5;
            this.debugModeCheckBox.Text = "Debug Mode";
            this.debugModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // timeoutTextBox
            // 
            this.timeoutTextBox.Location = new System.Drawing.Point(118, 166);
            this.timeoutTextBox.Name = "timeoutTextBox";
            this.timeoutTextBox.Size = new System.Drawing.Size(152, 22);
            this.timeoutTextBox.TabIndex = 6;
            this.timeoutTextBox.Text = "1000";
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.AutoSize = true;
            this.timeoutLabel.Location = new System.Drawing.Point(12, 169);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(63, 17);
            this.timeoutLabel.TabIndex = 7;
            this.timeoutLabel.Text = "Timeout:";
            // 
            // launchServerButton
            // 
            this.launchServerButton.Location = new System.Drawing.Point(83, 209);
            this.launchServerButton.Name = "launchServerButton";
            this.launchServerButton.Size = new System.Drawing.Size(122, 32);
            this.launchServerButton.TabIndex = 8;
            this.launchServerButton.Text = "Launch Server";
            this.launchServerButton.UseVisualStyleBackColor = true;
            this.launchServerButton.Click += new System.EventHandler(this.launchServerButton_Click);
            // 
            // ServerInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.launchServerButton);
            this.Controls.Add(this.timeoutLabel);
            this.Controls.Add(this.timeoutTextBox);
            this.Controls.Add(this.debugModeCheckBox);
            this.Controls.Add(this.serverLogCheckBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.saveCheckBox);
            this.Controls.Add(this.titleLabel);
            this.Name = "ServerInputForm";
            this.Text = "ServerInputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.CheckBox saveCheckBox;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.CheckBox serverLogCheckBox;
        private System.Windows.Forms.CheckBox debugModeCheckBox;
        private System.Windows.Forms.TextBox timeoutTextBox;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.Button launchServerButton;
    }
}