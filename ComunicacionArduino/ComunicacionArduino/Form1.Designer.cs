namespace ComunicacionArduino
{
    partial class ComunicacionArduinoForm
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
            this.ComLabel = new System.Windows.Forms.Label();
            this.BaudioLabel = new System.Windows.Forms.Label();
            this.ComTextBox = new System.Windows.Forms.TextBox();
            this.BaudiosTextBox = new System.Windows.Forms.TextBox();
            this.SincronizarButton = new System.Windows.Forms.Button();
            this.AlertaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ComLabel
            // 
            this.ComLabel.AutoSize = true;
            this.ComLabel.Location = new System.Drawing.Point(122, 73);
            this.ComLabel.Name = "ComLabel";
            this.ComLabel.Size = new System.Drawing.Size(90, 17);
            this.ComLabel.TabIndex = 0;
            this.ComLabel.Text = "Ingrese COM";
            // 
            // BaudioLabel
            // 
            this.BaudioLabel.AutoSize = true;
            this.BaudioLabel.Location = new System.Drawing.Point(125, 138);
            this.BaudioLabel.Name = "BaudioLabel";
            this.BaudioLabel.Size = new System.Drawing.Size(110, 17);
            this.BaudioLabel.TabIndex = 1;
            this.BaudioLabel.Text = "Ingrese Baudios";
            // 
            // ComTextBox
            // 
            this.ComTextBox.Location = new System.Drawing.Point(278, 73);
            this.ComTextBox.Name = "ComTextBox";
            this.ComTextBox.Size = new System.Drawing.Size(168, 22);
            this.ComTextBox.TabIndex = 2;
            this.ComTextBox.Text = "COM5";
            // 
            // BaudiosTextBox
            // 
            this.BaudiosTextBox.Location = new System.Drawing.Point(278, 138);
            this.BaudiosTextBox.Name = "BaudiosTextBox";
            this.BaudiosTextBox.Size = new System.Drawing.Size(168, 22);
            this.BaudiosTextBox.TabIndex = 3;
            this.BaudiosTextBox.Text = "9600";
            // 
            // SincronizarButton
            // 
            this.SincronizarButton.Location = new System.Drawing.Point(190, 194);
            this.SincronizarButton.Name = "SincronizarButton";
            this.SincronizarButton.Size = new System.Drawing.Size(168, 43);
            this.SincronizarButton.TabIndex = 4;
            this.SincronizarButton.Text = "Sincronizar";
            this.SincronizarButton.UseVisualStyleBackColor = true;
            this.SincronizarButton.Click += new System.EventHandler(this.SincronizarButton_Click);
            // 
            // AlertaLabel
            // 
            this.AlertaLabel.AutoSize = true;
            this.AlertaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlertaLabel.ForeColor = System.Drawing.Color.Red;
            this.AlertaLabel.Location = new System.Drawing.Point(167, 255);
            this.AlertaLabel.Name = "AlertaLabel";
            this.AlertaLabel.Size = new System.Drawing.Size(0, 25);
            this.AlertaLabel.TabIndex = 5;
            // 
            // ComunicacionArduinoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(561, 337);
            this.Controls.Add(this.AlertaLabel);
            this.Controls.Add(this.SincronizarButton);
            this.Controls.Add(this.BaudiosTextBox);
            this.Controls.Add(this.ComTextBox);
            this.Controls.Add(this.BaudioLabel);
            this.Controls.Add(this.ComLabel);
            this.Name = "ComunicacionArduinoForm";
            this.Text = "Comunicación Arduino";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ComLabel;
        private System.Windows.Forms.Label BaudioLabel;
        private System.Windows.Forms.TextBox ComTextBox;
        private System.Windows.Forms.TextBox BaudiosTextBox;
        private System.Windows.Forms.Button SincronizarButton;
        private System.Windows.Forms.Label AlertaLabel;
    }
}

