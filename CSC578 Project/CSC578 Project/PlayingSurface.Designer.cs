namespace CSC578_Project
{
    partial class PlayingSurface
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
            this.moveTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // moveTimer
            // 
            this.moveTimer.Interval = 10;
            this.moveTimer.Tick += new System.EventHandler(this.moveTimer_Tick);
            // 
            // PlayingSurface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(790, 542);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "PlayingSurface";
            this.Text = "PlayingSurface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayingSurface_FormClosing);
            this.Shown += new System.EventHandler(this.PlayingSurface_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayingSurface_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer moveTimer;
    }
}