namespace Pausify
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.adCheckBox = new System.Windows.Forms.CheckBox();
            this.pauseCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.adListBox = new System.Windows.Forms.ListBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.volLabel = new System.Windows.Forms.Label();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.adLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.defaultButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // adCheckBox
            // 
            this.adCheckBox.AutoSize = true;
            this.adCheckBox.Checked = true;
            this.adCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.adCheckBox.Location = new System.Drawing.Point(19, 116);
            this.adCheckBox.Name = "adCheckBox";
            this.adCheckBox.Size = new System.Drawing.Size(69, 17);
            this.adCheckBox.TabIndex = 0;
            this.adCheckBox.Text = "Ad Block";
            this.toolTip1.SetToolTip(this.adCheckBox, "Mutes spotify from sound mixer when an ad appears");
            this.adCheckBox.UseVisualStyleBackColor = true;
            this.adCheckBox.CheckedChanged += new System.EventHandler(this.adCheckBox_CheckedChanged);
            // 
            // pauseCheckBox
            // 
            this.pauseCheckBox.AutoSize = true;
            this.pauseCheckBox.Location = new System.Drawing.Point(19, 12);
            this.pauseCheckBox.Name = "pauseCheckBox";
            this.pauseCheckBox.Size = new System.Drawing.Size(81, 17);
            this.pauseCheckBox.TabIndex = 1;
            this.pauseCheckBox.Text = "Auto Pause";
            this.toolTip1.SetToolTip(this.pauseCheckBox, "Automatically pauses spotify when another sound plays\r\nNot tested recently, might" +
        " have some bugs");
            this.pauseCheckBox.UseVisualStyleBackColor = true;
            this.pauseCheckBox.CheckedChanged += new System.EventHandler(this.pauseCheckBox_CheckedChanged);
            // 
            // adListBox
            // 
            this.adListBox.FormattingEnabled = true;
            this.adListBox.Location = new System.Drawing.Point(19, 205);
            this.adListBox.Name = "adListBox";
            this.adListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.adListBox.Size = new System.Drawing.Size(170, 95);
            this.adListBox.TabIndex = 2;
            this.toolTip1.SetToolTip(this.adListBox, "Shift+Click to choose more than one record");
            // 
            // trackBar
            // 
            this.trackBar.AutoSize = false;
            this.trackBar.Location = new System.Drawing.Point(12, 161);
            this.trackBar.Maximum = 100;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(164, 25);
            this.trackBar.TabIndex = 10;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.trackBar, "Spotify volume on volume mixer \r\nafter an ad has played.");
            this.trackBar.Value = 100;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // volLabel
            // 
            this.volLabel.AutoSize = true;
            this.volLabel.Location = new System.Drawing.Point(16, 145);
            this.volLabel.Name = "volLabel";
            this.volLabel.Size = new System.Drawing.Size(77, 13);
            this.volLabel.TabIndex = 11;
            this.volLabel.Text = "Spotify Volume";
            this.toolTip1.SetToolTip(this.volLabel, "Spotify volume on volume mixer \r\nafter an ad has played.\r\n");
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.Enabled = false;
            this.rememberCheckBox.Location = new System.Drawing.Point(99, 145);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(77, 17);
            this.rememberCheckBox.TabIndex = 12;
            this.rememberCheckBox.Text = "Remember";
            this.toolTip1.SetToolTip(this.rememberCheckBox, "Remember the spotify volume before ad starts to play\r\n(CURRENTLY DISABLED)");
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            this.rememberCheckBox.CheckedChanged += new System.EventHandler(this.rememberCheckBox_CheckedChanged);
            // 
            // adLabel
            // 
            this.adLabel.AutoSize = true;
            this.adLabel.Location = new System.Drawing.Point(16, 189);
            this.adLabel.Name = "adLabel";
            this.adLabel.Size = new System.Drawing.Size(64, 13);
            this.adLabel.TabIndex = 4;
            this.adLabel.Text = "Marked Ads";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(195, 315);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(82, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.Location = new System.Drawing.Point(19, 315);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(82, 23);
            this.defaultButton.TabIndex = 7;
            this.defaultButton.Text = "Default";
            this.defaultButton.UseVisualStyleBackColor = true;
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(195, 240);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(82, 23);
            this.deleteButton.TabIndex = 9;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 350);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.volLabel);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.defaultButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.adLabel);
            this.Controls.Add(this.adListBox);
            this.Controls.Add(this.pauseCheckBox);
            this.Controls.Add(this.adCheckBox);
            this.Name = "SettingsForm";
            this.Text = "Pausify Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox adCheckBox;
        private System.Windows.Forms.CheckBox pauseCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox adListBox;
        private System.Windows.Forms.Label adLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label volLabel;
        private System.Windows.Forms.CheckBox rememberCheckBox;
    }
}