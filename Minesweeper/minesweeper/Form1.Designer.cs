namespace minesweeper
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BombsStrip = new System.Windows.Forms.StatusStrip();
            this.BombsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.BombsStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(456, 313);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // BombsStrip
            // 
            this.BombsStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.BombsStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BombsLabel});
            this.BombsStrip.Location = new System.Drawing.Point(0, 312);
            this.BombsStrip.Name = "BombsStrip";
            this.BombsStrip.Size = new System.Drawing.Size(480, 25);
            this.BombsStrip.TabIndex = 1;
            this.BombsStrip.Text = "Bombs:10";
            // 
            // BombsLabel
            // 
            this.BombsLabel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.BombsLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BombsLabel.Name = "BombsLabel";
            this.BombsLabel.Size = new System.Drawing.Size(78, 20);
            this.BombsLabel.Text = "Bombs: 10";
            this.BombsLabel.TextChanged += new System.EventHandler(this.BombsLabel_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 337);
            this.Controls.Add(this.BombsStrip);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Minesweeper";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.BombsStrip.ResumeLayout(false);
            this.BombsStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip BombsStrip;
        private System.Windows.Forms.ToolStripStatusLabel BombsLabel;
    }
}

