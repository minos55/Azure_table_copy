namespace Azure_table_copy
{
    partial class Form1
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
            this.Start = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.sourceConnectionTextBox = new System.Windows.Forms.TextBox();
            this.sourceTableNameTextBox = new System.Windows.Forms.TextBox();
            this.targetConnectionTextBox = new System.Windows.Forms.TextBox();
            this.targetTableNameTextBox = new System.Windows.Forms.TextBox();
            this.SourceConnectionLabel = new System.Windows.Forms.Label();
            this.SourceTableLabel = new System.Windows.Forms.Label();
            this.TargetConnectionLabel = new System.Windows.Forms.Label();
            this.TargetTableLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.batchSizeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BatchSizeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.batchSizeNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Start.Location = new System.Drawing.Point(325, 179);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(123, 49);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 234);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1008, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // sourceConnectionTextBox
            // 
            this.sourceConnectionTextBox.Location = new System.Drawing.Point(12, 41);
            this.sourceConnectionTextBox.Name = "sourceConnectionTextBox";
            this.sourceConnectionTextBox.Size = new System.Drawing.Size(1008, 20);
            this.sourceConnectionTextBox.TabIndex = 2;
            // 
            // sourceTableNameTextBox
            // 
            this.sourceTableNameTextBox.Location = new System.Drawing.Point(12, 87);
            this.sourceTableNameTextBox.Name = "sourceTableNameTextBox";
            this.sourceTableNameTextBox.Size = new System.Drawing.Size(165, 20);
            this.sourceTableNameTextBox.TabIndex = 3;
            // 
            // targetConnectionTextBox
            // 
            this.targetConnectionTextBox.Location = new System.Drawing.Point(12, 133);
            this.targetConnectionTextBox.Name = "targetConnectionTextBox";
            this.targetConnectionTextBox.Size = new System.Drawing.Size(1008, 20);
            this.targetConnectionTextBox.TabIndex = 4;
            // 
            // targetTableNameTextBox
            // 
            this.targetTableNameTextBox.Location = new System.Drawing.Point(12, 179);
            this.targetTableNameTextBox.Name = "targetTableNameTextBox";
            this.targetTableNameTextBox.Size = new System.Drawing.Size(165, 20);
            this.targetTableNameTextBox.TabIndex = 5;
            // 
            // SourceConnectionLabel
            // 
            this.SourceConnectionLabel.AutoSize = true;
            this.SourceConnectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SourceConnectionLabel.Location = new System.Drawing.Point(8, 18);
            this.SourceConnectionLabel.Name = "SourceConnectionLabel";
            this.SourceConnectionLabel.Size = new System.Drawing.Size(195, 20);
            this.SourceConnectionLabel.TabIndex = 6;
            this.SourceConnectionLabel.Text = "Source connection string";
            // 
            // SourceTableLabel
            // 
            this.SourceTableLabel.AutoSize = true;
            this.SourceTableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SourceTableLabel.Location = new System.Drawing.Point(8, 64);
            this.SourceTableLabel.Name = "SourceTableLabel";
            this.SourceTableLabel.Size = new System.Drawing.Size(149, 20);
            this.SourceTableLabel.TabIndex = 7;
            this.SourceTableLabel.Text = "Source table name";
            // 
            // TargetConnectionLabel
            // 
            this.TargetConnectionLabel.AutoSize = true;
            this.TargetConnectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TargetConnectionLabel.Location = new System.Drawing.Point(8, 110);
            this.TargetConnectionLabel.Name = "TargetConnectionLabel";
            this.TargetConnectionLabel.Size = new System.Drawing.Size(190, 20);
            this.TargetConnectionLabel.TabIndex = 8;
            this.TargetConnectionLabel.Text = "Target connection string";
            // 
            // TargetTableLabel
            // 
            this.TargetTableLabel.AutoSize = true;
            this.TargetTableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TargetTableLabel.Location = new System.Drawing.Point(8, 156);
            this.TargetTableLabel.Name = "TargetTableLabel";
            this.TargetTableLabel.Size = new System.Drawing.Size(144, 20);
            this.TargetTableLabel.TabIndex = 9;
            this.TargetTableLabel.Text = "Target table name";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(543, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 49);
            this.button1.TabIndex = 10;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // batchSizeNumericUpDown
            // 
            this.batchSizeNumericUpDown.Location = new System.Drawing.Point(875, 179);
            this.batchSizeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.batchSizeNumericUpDown.Name = "batchSizeNumericUpDown";
            this.batchSizeNumericUpDown.Size = new System.Drawing.Size(140, 20);
            this.batchSizeNumericUpDown.TabIndex = 11;
            this.batchSizeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // BatchSizeLabel
            // 
            this.BatchSizeLabel.AutoSize = true;
            this.BatchSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BatchSizeLabel.Location = new System.Drawing.Point(871, 156);
            this.BatchSizeLabel.Name = "BatchSizeLabel";
            this.BatchSizeLabel.Size = new System.Drawing.Size(91, 20);
            this.BatchSizeLabel.TabIndex = 12;
            this.BatchSizeLabel.Text = "Batch Size";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 265);
            this.Controls.Add(this.BatchSizeLabel);
            this.Controls.Add(this.batchSizeNumericUpDown);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TargetTableLabel);
            this.Controls.Add(this.TargetConnectionLabel);
            this.Controls.Add(this.SourceTableLabel);
            this.Controls.Add(this.SourceConnectionLabel);
            this.Controls.Add(this.targetTableNameTextBox);
            this.Controls.Add(this.targetConnectionTextBox);
            this.Controls.Add(this.sourceTableNameTextBox);
            this.Controls.Add(this.sourceConnectionTextBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Azure table copy";
            ((System.ComponentModel.ISupportInitialize)(this.batchSizeNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox sourceConnectionTextBox;
        private System.Windows.Forms.TextBox sourceTableNameTextBox;
        private System.Windows.Forms.TextBox targetConnectionTextBox;
        private System.Windows.Forms.TextBox targetTableNameTextBox;
        private System.Windows.Forms.Label SourceConnectionLabel;
        private System.Windows.Forms.Label SourceTableLabel;
        private System.Windows.Forms.Label TargetConnectionLabel;
        private System.Windows.Forms.Label TargetTableLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown batchSizeNumericUpDown;
        private System.Windows.Forms.Label BatchSizeLabel;
    }
}

