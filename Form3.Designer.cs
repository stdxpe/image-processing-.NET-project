namespace image_process_demo5
{
    partial class Form3
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
            this.pbxNew2 = new System.Windows.Forms.PictureBox();
            this.x_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbxNew2)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxNew2
            // 
            this.pbxNew2.BackColor = System.Drawing.Color.Black;
            this.pbxNew2.Location = new System.Drawing.Point(0, 44);
            this.pbxNew2.Name = "pbxNew2";
            this.pbxNew2.Size = new System.Drawing.Size(1050, 640);
            this.pbxNew2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxNew2.TabIndex = 0;
            this.pbxNew2.TabStop = false;
            // 
            // x_Button
            // 
            this.x_Button.BackColor = System.Drawing.Color.Transparent;
            this.x_Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.x_Button.FlatAppearance.BorderSize = 0;
            this.x_Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Brown;
            this.x_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkRed;
            this.x_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.x_Button.Image = global::image_process_demo5.Properties.Resources.x_but;
            this.x_Button.Location = new System.Drawing.Point(1008, 3);
            this.x_Button.Name = "x_Button";
            this.x_Button.Size = new System.Drawing.Size(40, 40);
            this.x_Button.TabIndex = 1;
            this.x_Button.UseVisualStyleBackColor = false;
            this.x_Button.Click += new System.EventHandler(this.x_Button_Click);
            // 
            // Form3
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Olive;
            this.ClientSize = new System.Drawing.Size(1050, 688);
            this.Controls.Add(this.x_Button);
            this.Controls.Add(this.pbxNew2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3";
            this.TransparencyKey = System.Drawing.Color.Olive;
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxNew2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxNew2;
        private System.Windows.Forms.Button x_Button;
    }
}