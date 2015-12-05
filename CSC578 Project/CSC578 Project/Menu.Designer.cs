namespace CSC578_Project
{
    partial class Menu
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
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnQuitToDesktop = new System.Windows.Forms.Button();
            this.btnToMenu = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Location = new System.Drawing.Point(6, 40);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(364, 51);
            this.btnRestart.TabIndex = 0;
            this.btnRestart.Text = "Restart Game";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnQuitToDesktop
            // 
            this.btnQuitToDesktop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnQuitToDesktop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnQuitToDesktop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuitToDesktop.Location = new System.Drawing.Point(6, 97);
            this.btnQuitToDesktop.Name = "btnQuitToDesktop";
            this.btnQuitToDesktop.Size = new System.Drawing.Size(364, 51);
            this.btnQuitToDesktop.TabIndex = 1;
            this.btnQuitToDesktop.Text = "Quit to Desktop";
            this.btnQuitToDesktop.UseVisualStyleBackColor = false;
            this.btnQuitToDesktop.Click += new System.EventHandler(this.btnQuitToDesktop_Click);
            // 
            // btnToMenu
            // 
            this.btnToMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnToMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToMenu.Location = new System.Drawing.Point(6, 154);
            this.btnToMenu.Name = "btnToMenu";
            this.btnToMenu.Size = new System.Drawing.Size(364, 51);
            this.btnToMenu.TabIndex = 2;
            this.btnToMenu.Text = "Exit to Menu";
            this.btnToMenu.UseVisualStyleBackColor = false;
            this.btnToMenu.Click += new System.EventHandler(this.btnToMenu_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(6, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(364, 51);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(382, 264);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToMenu);
            this.Controls.Add(this.btnQuitToDesktop);
            this.Controls.Add(this.btnRestart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnQuitToDesktop;
        private System.Windows.Forms.Button btnToMenu;
        private System.Windows.Forms.Button btnCancel;
    }
}