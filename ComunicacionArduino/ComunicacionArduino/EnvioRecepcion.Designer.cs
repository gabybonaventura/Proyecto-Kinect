namespace ComunicacionArduino
{
    partial class EnvioRecepcion
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
            this.EnviarButton = new System.Windows.Forms.Button();
            this.EnviarTextBox = new System.Windows.Forms.TextBox();
            this.AlertaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EnviarButton
            // 
            this.EnviarButton.Location = new System.Drawing.Point(400, 58);
            this.EnviarButton.Name = "EnviarButton";
            this.EnviarButton.Size = new System.Drawing.Size(75, 23);
            this.EnviarButton.TabIndex = 0;
            this.EnviarButton.Text = "Enviar";
            this.EnviarButton.UseVisualStyleBackColor = true;
            this.EnviarButton.Click += new System.EventHandler(this.EnviarButton_Click);
            // 
            // EnviarTextBox
            // 
            this.EnviarTextBox.Location = new System.Drawing.Point(140, 59);
            this.EnviarTextBox.Name = "EnviarTextBox";
            this.EnviarTextBox.Size = new System.Drawing.Size(196, 22);
            this.EnviarTextBox.TabIndex = 1;
            this.EnviarTextBox.Text = "*180180180";
            // 
            // AlertaLabel
            // 
            this.AlertaLabel.AutoSize = true;
            this.AlertaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlertaLabel.ForeColor = System.Drawing.Color.Red;
            this.AlertaLabel.Location = new System.Drawing.Point(206, 105);
            this.AlertaLabel.Name = "AlertaLabel";
            this.AlertaLabel.Size = new System.Drawing.Size(0, 25);
            this.AlertaLabel.TabIndex = 2;
            // 
            // EnvioRecepcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(608, 164);
            this.Controls.Add(this.AlertaLabel);
            this.Controls.Add(this.EnviarTextBox);
            this.Controls.Add(this.EnviarButton);
            this.Name = "EnvioRecepcion";
            this.Text = "EnvioRecepcion";
            this.Load += new System.EventHandler(this.EnvioRecepcion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnviarButton;
        private System.Windows.Forms.TextBox EnviarTextBox;
        private System.Windows.Forms.Label AlertaLabel;
    }
}