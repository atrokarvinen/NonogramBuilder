namespace NonogramBuilder
{
    partial class NonogramUI
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
            this.OriginalPB = new System.Windows.Forms.PictureBox();
            this.ResultPB = new System.Windows.Forms.PictureBox();
            this.OriginalImageLabel = new System.Windows.Forms.Label();
            this.ResultImageLabel = new System.Windows.Forms.Label();
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.SaveImageButton = new System.Windows.Forms.Button();
            this.ComputeButton = new System.Windows.Forms.Button();
            this.ThresholdMinInput = new System.Windows.Forms.TextBox();
            this.ThresholdMinLabel = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressBarLabel = new System.Windows.Forms.Label();
            this.ThresholdMaxLabel = new System.Windows.Forms.Label();
            this.ThresholdMaxInput = new System.Windows.Forms.TextBox();
            this.ColumnCountLabel = new System.Windows.Forms.Label();
            this.ColumnCountInput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.OriginalPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultPB)).BeginInit();
            this.SuspendLayout();
            // 
            // OriginalPB
            // 
            this.OriginalPB.Location = new System.Drawing.Point(281, 38);
            this.OriginalPB.Name = "OriginalPB";
            this.OriginalPB.Size = new System.Drawing.Size(100, 50);
            this.OriginalPB.TabIndex = 0;
            this.OriginalPB.TabStop = false;
            // 
            // ResultPB
            // 
            this.ResultPB.Location = new System.Drawing.Point(454, 38);
            this.ResultPB.Name = "ResultPB";
            this.ResultPB.Size = new System.Drawing.Size(100, 50);
            this.ResultPB.TabIndex = 1;
            this.ResultPB.TabStop = false;
            // 
            // OriginalImageLabel
            // 
            this.OriginalImageLabel.AutoSize = true;
            this.OriginalImageLabel.Location = new System.Drawing.Point(278, 22);
            this.OriginalImageLabel.Name = "OriginalImageLabel";
            this.OriginalImageLabel.Size = new System.Drawing.Size(97, 13);
            this.OriginalImageLabel.TabIndex = 2;
            this.OriginalImageLabel.Text = "OriginalImageLabel";
            // 
            // ResultImageLabel
            // 
            this.ResultImageLabel.AutoSize = true;
            this.ResultImageLabel.Location = new System.Drawing.Point(451, 22);
            this.ResultImageLabel.Name = "ResultImageLabel";
            this.ResultImageLabel.Size = new System.Drawing.Size(92, 13);
            this.ResultImageLabel.TabIndex = 3;
            this.ResultImageLabel.Text = "ResultImageLabel";
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(34, 342);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(75, 23);
            this.LoadImageButton.TabIndex = 4;
            this.LoadImageButton.Text = "LoadImageButton";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // SaveImageButton
            // 
            this.SaveImageButton.Location = new System.Drawing.Point(34, 371);
            this.SaveImageButton.Name = "SaveImageButton";
            this.SaveImageButton.Size = new System.Drawing.Size(75, 23);
            this.SaveImageButton.TabIndex = 4;
            this.SaveImageButton.Text = "SaveImageButton";
            this.SaveImageButton.UseVisualStyleBackColor = true;
            this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
            // 
            // ComputeButton
            // 
            this.ComputeButton.Location = new System.Drawing.Point(34, 400);
            this.ComputeButton.Name = "ComputeButton";
            this.ComputeButton.Size = new System.Drawing.Size(75, 23);
            this.ComputeButton.TabIndex = 5;
            this.ComputeButton.Text = "ComputeButton";
            this.ComputeButton.UseVisualStyleBackColor = true;
            this.ComputeButton.Click += new System.EventHandler(this.ComputeButton_Click);
            // 
            // ThresholdMinInput
            // 
            this.ThresholdMinInput.Location = new System.Drawing.Point(322, 378);
            this.ThresholdMinInput.Name = "ThresholdMinInput";
            this.ThresholdMinInput.Size = new System.Drawing.Size(100, 20);
            this.ThresholdMinInput.TabIndex = 6;
            // 
            // ThresholdMinLabel
            // 
            this.ThresholdMinLabel.AutoSize = true;
            this.ThresholdMinLabel.Location = new System.Drawing.Point(240, 381);
            this.ThresholdMinLabel.Name = "ThresholdMinLabel";
            this.ThresholdMinLabel.Size = new System.Drawing.Size(74, 13);
            this.ThresholdMinLabel.TabIndex = 7;
            this.ThresholdMinLabel.Text = "Threshold Min";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(294, 428);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 23);
            this.ProgressBar.TabIndex = 8;
            // 
            // ProgressBarLabel
            // 
            this.ProgressBarLabel.AutoSize = true;
            this.ProgressBarLabel.Location = new System.Drawing.Point(305, 412);
            this.ProgressBarLabel.Name = "ProgressBarLabel";
            this.ProgressBarLabel.Size = new System.Drawing.Size(90, 13);
            this.ProgressBarLabel.TabIndex = 9;
            this.ProgressBarLabel.Text = "ProgressBarLabel";
            // 
            // ThresholdMaxLabel
            // 
            this.ThresholdMaxLabel.AutoSize = true;
            this.ThresholdMaxLabel.Location = new System.Drawing.Point(444, 381);
            this.ThresholdMaxLabel.Name = "ThresholdMaxLabel";
            this.ThresholdMaxLabel.Size = new System.Drawing.Size(77, 13);
            this.ThresholdMaxLabel.TabIndex = 11;
            this.ThresholdMaxLabel.Text = "Threshold Max";
            // 
            // ThresholdMaxInput
            // 
            this.ThresholdMaxInput.Location = new System.Drawing.Point(526, 378);
            this.ThresholdMaxInput.Name = "ThresholdMaxInput";
            this.ThresholdMaxInput.Size = new System.Drawing.Size(100, 20);
            this.ThresholdMaxInput.TabIndex = 10;
            // 
            // ColumnCountLabel
            // 
            this.ColumnCountLabel.AutoSize = true;
            this.ColumnCountLabel.Location = new System.Drawing.Point(640, 381);
            this.ColumnCountLabel.Name = "ColumnCountLabel";
            this.ColumnCountLabel.Size = new System.Drawing.Size(47, 13);
            this.ColumnCountLabel.TabIndex = 13;
            this.ColumnCountLabel.Text = "Columns";
            // 
            // ColumnCountInput
            // 
            this.ColumnCountInput.Location = new System.Drawing.Point(722, 378);
            this.ColumnCountInput.Name = "ColumnCountInput";
            this.ColumnCountInput.Size = new System.Drawing.Size(100, 20);
            this.ColumnCountInput.TabIndex = 12;
            // 
            // NonogramUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 450);
            this.Controls.Add(this.ColumnCountLabel);
            this.Controls.Add(this.ColumnCountInput);
            this.Controls.Add(this.ThresholdMaxLabel);
            this.Controls.Add(this.ThresholdMaxInput);
            this.Controls.Add(this.ProgressBarLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.ThresholdMinLabel);
            this.Controls.Add(this.ThresholdMinInput);
            this.Controls.Add(this.ComputeButton);
            this.Controls.Add(this.SaveImageButton);
            this.Controls.Add(this.LoadImageButton);
            this.Controls.Add(this.ResultImageLabel);
            this.Controls.Add(this.OriginalImageLabel);
            this.Controls.Add(this.ResultPB);
            this.Controls.Add(this.OriginalPB);
            this.Name = "NonogramUI";
            this.Text = "Nonogram builder";
            this.Resize += new System.EventHandler(this.ShadowsHelperUI_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.OriginalPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OriginalPB;
        private System.Windows.Forms.PictureBox ResultPB;
        private System.Windows.Forms.Label OriginalImageLabel;
        private System.Windows.Forms.Label ResultImageLabel;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Button SaveImageButton;
        private System.Windows.Forms.Button ComputeButton;
        private System.Windows.Forms.TextBox ThresholdMinInput;
        private System.Windows.Forms.Label ThresholdMinLabel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label ProgressBarLabel;
        private System.Windows.Forms.Label ThresholdMaxLabel;
        private System.Windows.Forms.TextBox ThresholdMaxInput;
        private System.Windows.Forms.Label ColumnCountLabel;
        private System.Windows.Forms.TextBox ColumnCountInput;
    }
}

