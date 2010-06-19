namespace T7EPreferences
{
    partial class Loading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
            this.LoadingLogo = new System.Windows.Forms.PictureBox();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.LoadingProgress = new System.Windows.Forms.ProgressBar();
            this.LoadingCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadingLogo
            // 
            this.LoadingLogo.Image = ((System.Drawing.Image)(resources.GetObject("LoadingLogo.Image")));
            this.LoadingLogo.Location = new System.Drawing.Point(6, 0);
            this.LoadingLogo.Name = "LoadingLogo";
            this.LoadingLogo.Size = new System.Drawing.Size(620, 92);
            this.LoadingLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.LoadingLogo.TabIndex = 0;
            this.LoadingLogo.TabStop = false;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.AutoSize = true;
            this.LoadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadingLabel.Location = new System.Drawing.Point(271, 98);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(90, 20);
            this.LoadingLabel.TabIndex = 1;
            this.LoadingLabel.Text = "Loading...";
            // 
            // LoadingProgress
            // 
            this.LoadingProgress.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingProgress.Location = new System.Drawing.Point(13, 122);
            this.LoadingProgress.Name = "LoadingProgress";
            this.LoadingProgress.Size = new System.Drawing.Size(607, 23);
            this.LoadingProgress.TabIndex = 2;
            // 
            // LoadingCancel
            // 
            this.LoadingCancel.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingCancel.Location = new System.Drawing.Point(545, 151);
            this.LoadingCancel.Name = "LoadingCancel";
            this.LoadingCancel.Size = new System.Drawing.Size(75, 23);
            this.LoadingCancel.TabIndex = 3;
            this.LoadingCancel.Text = "Cancel";
            this.LoadingCancel.UseVisualStyleBackColor = true;
            this.LoadingCancel.Click += new System.EventHandler(this.LoadingCancel_Click);
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(632, 180);
            this.Controls.Add(this.LoadingCancel);
            this.Controls.Add(this.LoadingProgress);
            this.Controls.Add(this.LoadingLabel);
            this.Controls.Add(this.LoadingLogo);
            this.MaximizeBox = false;
            this.Name = "Loading";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            ((System.ComponentModel.ISupportInitialize)(this.LoadingLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox LoadingLogo;
        private System.Windows.Forms.Label LoadingLabel;
        private System.Windows.Forms.ProgressBar LoadingProgress;
        private System.Windows.Forms.Button LoadingCancel;
    }
}