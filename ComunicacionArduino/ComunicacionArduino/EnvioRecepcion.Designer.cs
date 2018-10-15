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
            this.MedicionesLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnviarButton
            // 
            this.EnviarButton.Location = new System.Drawing.Point(300, 47);
            this.EnviarButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.EnviarButton.Name = "EnviarButton";
            this.EnviarButton.Size = new System.Drawing.Size(56, 19);
            this.EnviarButton.TabIndex = 0;
            this.EnviarButton.Text = "Enviar";
            this.EnviarButton.UseVisualStyleBackColor = true;
            this.EnviarButton.Click += new System.EventHandler(this.EnviarButton_Click);
            // 
            // EnviarTextBox
            // 
            this.EnviarTextBox.Location = new System.Drawing.Point(105, 48);
            this.EnviarTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.EnviarTextBox.Name = "EnviarTextBox";
            this.EnviarTextBox.Size = new System.Drawing.Size(148, 20);
            this.EnviarTextBox.TabIndex = 1;
            this.EnviarTextBox.Text = "*180180180";
            // 
            // AlertaLabel
            // 
            this.AlertaLabel.AutoSize = true;
            this.AlertaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlertaLabel.ForeColor = System.Drawing.Color.Red;
            this.AlertaLabel.Location = new System.Drawing.Point(154, 85);
            this.AlertaLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AlertaLabel.Name = "AlertaLabel";
            this.AlertaLabel.Size = new System.Drawing.Size(0, 20);
            this.AlertaLabel.TabIndex = 2;
            // 
            // MedicionesLabel
            // 
            this.MedicionesLabel.AutoSize = true;
            this.MedicionesLabel.Location = new System.Drawing.Point(38, 91);
            this.MedicionesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MedicionesLabel.Name = "MedicionesLabel";
            this.MedicionesLabel.Size = new System.Drawing.Size(61, 13);
            this.MedicionesLabel.TabIndex = 3;
            this.MedicionesLabel.Text = "Mediciones";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EnvioRecepcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(456, 387);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MedicionesLabel);
            this.Controls.Add(this.AlertaLabel);
            this.Controls.Add(this.EnviarTextBox);
            this.Controls.Add(this.EnviarButton);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Label MedicionesLabel;
        private System.Windows.Forms.Button button1;
    }
}