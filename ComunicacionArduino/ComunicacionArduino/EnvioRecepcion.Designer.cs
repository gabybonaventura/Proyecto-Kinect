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
            this.CodoArribaAbajoMas_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CodoArribaAbajoMenos_btn = new System.Windows.Forms.Button();
            this.CodoDerechaIzquierdaMenos_btn = new System.Windows.Forms.Button();
            this.CodoDerechaIzquierdaMas_btn = new System.Windows.Forms.Button();
            this.HombroAdelanteAtrasMas_btn = new System.Windows.Forms.Button();
            this.HombroAdelanteAtrasMenos_btn = new System.Windows.Forms.Button();
            this.HombroArribaAbajoMas_btn = new System.Windows.Forms.Button();
            this.HombroArribaAbajoMenos_btn = new System.Windows.Forms.Button();
            this.ConsumoHombroArribaAbajo_lbl = new System.Windows.Forms.Label();
            this.ConsumoHombroAdelanteAtras_lbl = new System.Windows.Forms.Label();
            this.ConsumoCodoArribaAbajo_lbl = new System.Windows.Forms.Label();
            this.ConsumoCodoDerechaIzquierda_lbl = new System.Windows.Forms.Label();
            this.AnguloHombroArribaAbajo_txt = new System.Windows.Forms.TextBox();
            this.AnguloHombroAdelanteAtras_txt = new System.Windows.Forms.TextBox();
            this.AnguloCodoArribaAbajo_txt = new System.Windows.Forms.TextBox();
            this.AnguloCodoDerechaIzquierda_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AngulosDefault_btn = new System.Windows.Forms.Button();
            this.AngulosAEnviar_lbl = new System.Windows.Forms.Label();
            this.EstadoEnvio_lbl = new System.Windows.Forms.Label();
            this.HistorialConsumo_txtArea = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EnviarButton
            // 
            this.EnviarButton.Location = new System.Drawing.Point(573, 514);
            this.EnviarButton.Margin = new System.Windows.Forms.Padding(2);
            this.EnviarButton.Name = "EnviarButton";
            this.EnviarButton.Size = new System.Drawing.Size(56, 19);
            this.EnviarButton.TabIndex = 0;
            this.EnviarButton.Text = "Enviar";
            this.EnviarButton.UseVisualStyleBackColor = true;
            this.EnviarButton.Visible = false;
            this.EnviarButton.Click += new System.EventHandler(this.EnviarButton_Click);
            // 
            // EnviarTextBox
            // 
            this.EnviarTextBox.Location = new System.Drawing.Point(421, 513);
            this.EnviarTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.EnviarTextBox.Name = "EnviarTextBox";
            this.EnviarTextBox.Size = new System.Drawing.Size(148, 20);
            this.EnviarTextBox.TabIndex = 1;
            this.EnviarTextBox.Text = "*180180180";
            this.EnviarTextBox.Visible = false;
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
            this.MedicionesLabel.Location = new System.Drawing.Point(418, 538);
            this.MedicionesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MedicionesLabel.Name = "MedicionesLabel";
            this.MedicionesLabel.Size = new System.Drawing.Size(61, 13);
            this.MedicionesLabel.TabIndex = 3;
            this.MedicionesLabel.Text = "Mediciones";
            this.MedicionesLabel.Visible = false;
            // 
            // CodoArribaAbajoMas_btn
            // 
            this.CodoArribaAbajoMas_btn.Location = new System.Drawing.Point(258, 265);
            this.CodoArribaAbajoMas_btn.Name = "CodoArribaAbajoMas_btn";
            this.CodoArribaAbajoMas_btn.Size = new System.Drawing.Size(46, 26);
            this.CodoArribaAbajoMas_btn.TabIndex = 4;
            this.CodoArribaAbajoMas_btn.Text = "CAB +";
            this.CodoArribaAbajoMas_btn.UseVisualStyleBackColor = true;
            this.CodoArribaAbajoMas_btn.Click += new System.EventHandler(this.CodoArribaAbajoMas_btn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ComunicacionArduino.Properties.Resources.brazo_perfil;
            this.pictureBox1.Location = new System.Drawing.Point(6, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(409, 414);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // CodoArribaAbajoMenos_btn
            // 
            this.CodoArribaAbajoMenos_btn.Location = new System.Drawing.Point(206, 265);
            this.CodoArribaAbajoMenos_btn.Name = "CodoArribaAbajoMenos_btn";
            this.CodoArribaAbajoMenos_btn.Size = new System.Drawing.Size(46, 26);
            this.CodoArribaAbajoMenos_btn.TabIndex = 6;
            this.CodoArribaAbajoMenos_btn.Text = "CAB -";
            this.CodoArribaAbajoMenos_btn.UseVisualStyleBackColor = true;
            this.CodoArribaAbajoMenos_btn.Click += new System.EventHandler(this.CodoArribaAbajoMenos_btn_Click);
            // 
            // CodoDerechaIzquierdaMenos_btn
            // 
            this.CodoDerechaIzquierdaMenos_btn.Location = new System.Drawing.Point(206, 378);
            this.CodoDerechaIzquierdaMenos_btn.Name = "CodoDerechaIzquierdaMenos_btn";
            this.CodoDerechaIzquierdaMenos_btn.Size = new System.Drawing.Size(46, 26);
            this.CodoDerechaIzquierdaMenos_btn.TabIndex = 8;
            this.CodoDerechaIzquierdaMenos_btn.Text = "CDI -";
            this.CodoDerechaIzquierdaMenos_btn.UseVisualStyleBackColor = true;
            this.CodoDerechaIzquierdaMenos_btn.Click += new System.EventHandler(this.CodoDerechaIzquierdaMenos_btn_Click);
            // 
            // CodoDerechaIzquierdaMas_btn
            // 
            this.CodoDerechaIzquierdaMas_btn.Location = new System.Drawing.Point(258, 378);
            this.CodoDerechaIzquierdaMas_btn.Name = "CodoDerechaIzquierdaMas_btn";
            this.CodoDerechaIzquierdaMas_btn.Size = new System.Drawing.Size(46, 26);
            this.CodoDerechaIzquierdaMas_btn.TabIndex = 7;
            this.CodoDerechaIzquierdaMas_btn.Text = "CDI +";
            this.CodoDerechaIzquierdaMas_btn.UseVisualStyleBackColor = true;
            this.CodoDerechaIzquierdaMas_btn.Click += new System.EventHandler(this.CodoDerechaIzquierdaMas_btn_Click);
            // 
            // HombroAdelanteAtrasMas_btn
            // 
            this.HombroAdelanteAtrasMas_btn.Location = new System.Drawing.Point(207, 156);
            this.HombroAdelanteAtrasMas_btn.Name = "HombroAdelanteAtrasMas_btn";
            this.HombroAdelanteAtrasMas_btn.Size = new System.Drawing.Size(46, 26);
            this.HombroAdelanteAtrasMas_btn.TabIndex = 10;
            this.HombroAdelanteAtrasMas_btn.Text = "HAA +";
            this.HombroAdelanteAtrasMas_btn.UseVisualStyleBackColor = true;
            this.HombroAdelanteAtrasMas_btn.Click += new System.EventHandler(this.HombroAdelanteAtrasMas_btn_Click);
            // 
            // HombroAdelanteAtrasMenos_btn
            // 
            this.HombroAdelanteAtrasMenos_btn.Location = new System.Drawing.Point(259, 156);
            this.HombroAdelanteAtrasMenos_btn.Name = "HombroAdelanteAtrasMenos_btn";
            this.HombroAdelanteAtrasMenos_btn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.HombroAdelanteAtrasMenos_btn.Size = new System.Drawing.Size(46, 26);
            this.HombroAdelanteAtrasMenos_btn.TabIndex = 9;
            this.HombroAdelanteAtrasMenos_btn.Text = "HAA -";
            this.HombroAdelanteAtrasMenos_btn.UseVisualStyleBackColor = true;
            this.HombroAdelanteAtrasMenos_btn.Click += new System.EventHandler(this.HombroAdelanteAtrasMenos_btn_Click);
            // 
            // HombroArribaAbajoMas_btn
            // 
            this.HombroArribaAbajoMas_btn.Location = new System.Drawing.Point(259, 51);
            this.HombroArribaAbajoMas_btn.Name = "HombroArribaAbajoMas_btn";
            this.HombroArribaAbajoMas_btn.Size = new System.Drawing.Size(46, 26);
            this.HombroArribaAbajoMas_btn.TabIndex = 12;
            this.HombroArribaAbajoMas_btn.Text = "HAB +";
            this.HombroArribaAbajoMas_btn.UseVisualStyleBackColor = true;
            this.HombroArribaAbajoMas_btn.Click += new System.EventHandler(this.HombroArribaAbajoMas_btn_Click);
            // 
            // HombroArribaAbajoMenos_btn
            // 
            this.HombroArribaAbajoMenos_btn.Location = new System.Drawing.Point(207, 51);
            this.HombroArribaAbajoMenos_btn.Name = "HombroArribaAbajoMenos_btn";
            this.HombroArribaAbajoMenos_btn.Size = new System.Drawing.Size(46, 26);
            this.HombroArribaAbajoMenos_btn.TabIndex = 11;
            this.HombroArribaAbajoMenos_btn.Text = "HAB -";
            this.HombroArribaAbajoMenos_btn.UseVisualStyleBackColor = true;
            this.HombroArribaAbajoMenos_btn.Click += new System.EventHandler(this.HombroArribaAbajoMenos_btn_Click);
            // 
            // ConsumoHombroArribaAbajo_lbl
            // 
            this.ConsumoHombroArribaAbajo_lbl.AutoSize = true;
            this.ConsumoHombroArribaAbajo_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsumoHombroArribaAbajo_lbl.ForeColor = System.Drawing.Color.Black;
            this.ConsumoHombroArribaAbajo_lbl.Location = new System.Drawing.Point(207, 81);
            this.ConsumoHombroArribaAbajo_lbl.Name = "ConsumoHombroArribaAbajo_lbl";
            this.ConsumoHombroArribaAbajo_lbl.Size = new System.Drawing.Size(95, 20);
            this.ConsumoHombroArribaAbajo_lbl.TabIndex = 13;
            this.ConsumoHombroArribaAbajo_lbl.Text = "Consumo: --";
            // 
            // ConsumoHombroAdelanteAtras_lbl
            // 
            this.ConsumoHombroAdelanteAtras_lbl.AutoSize = true;
            this.ConsumoHombroAdelanteAtras_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsumoHombroAdelanteAtras_lbl.ForeColor = System.Drawing.Color.Black;
            this.ConsumoHombroAdelanteAtras_lbl.Location = new System.Drawing.Point(207, 185);
            this.ConsumoHombroAdelanteAtras_lbl.Name = "ConsumoHombroAdelanteAtras_lbl";
            this.ConsumoHombroAdelanteAtras_lbl.Size = new System.Drawing.Size(95, 20);
            this.ConsumoHombroAdelanteAtras_lbl.TabIndex = 14;
            this.ConsumoHombroAdelanteAtras_lbl.Text = "Consumo: --";
            // 
            // ConsumoCodoArribaAbajo_lbl
            // 
            this.ConsumoCodoArribaAbajo_lbl.AutoSize = true;
            this.ConsumoCodoArribaAbajo_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsumoCodoArribaAbajo_lbl.ForeColor = System.Drawing.Color.Black;
            this.ConsumoCodoArribaAbajo_lbl.Location = new System.Drawing.Point(206, 294);
            this.ConsumoCodoArribaAbajo_lbl.Name = "ConsumoCodoArribaAbajo_lbl";
            this.ConsumoCodoArribaAbajo_lbl.Size = new System.Drawing.Size(95, 20);
            this.ConsumoCodoArribaAbajo_lbl.TabIndex = 15;
            this.ConsumoCodoArribaAbajo_lbl.Text = "Consumo: --";
            // 
            // ConsumoCodoDerechaIzquierda_lbl
            // 
            this.ConsumoCodoDerechaIzquierda_lbl.AutoSize = true;
            this.ConsumoCodoDerechaIzquierda_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsumoCodoDerechaIzquierda_lbl.ForeColor = System.Drawing.Color.Black;
            this.ConsumoCodoDerechaIzquierda_lbl.Location = new System.Drawing.Point(206, 407);
            this.ConsumoCodoDerechaIzquierda_lbl.Name = "ConsumoCodoDerechaIzquierda_lbl";
            this.ConsumoCodoDerechaIzquierda_lbl.Size = new System.Drawing.Size(95, 20);
            this.ConsumoCodoDerechaIzquierda_lbl.TabIndex = 16;
            this.ConsumoCodoDerechaIzquierda_lbl.Text = "Consumo: --";
            // 
            // AnguloHombroArribaAbajo_txt
            // 
            this.AnguloHombroArribaAbajo_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnguloHombroArribaAbajo_txt.Location = new System.Drawing.Point(311, 51);
            this.AnguloHombroArribaAbajo_txt.MaxLength = 3;
            this.AnguloHombroArribaAbajo_txt.Name = "AnguloHombroArribaAbajo_txt";
            this.AnguloHombroArribaAbajo_txt.Size = new System.Drawing.Size(51, 26);
            this.AnguloHombroArribaAbajo_txt.TabIndex = 17;
            this.AnguloHombroArribaAbajo_txt.Text = "0";
            this.AnguloHombroArribaAbajo_txt.TextChanged += new System.EventHandler(this.AnguloHombroArribaAbajo_txt_TextChanged);
            this.AnguloHombroArribaAbajo_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnguloHombroArribaAbajo_txt_KeyDown);
            // 
            // AnguloHombroAdelanteAtras_txt
            // 
            this.AnguloHombroAdelanteAtras_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnguloHombroAdelanteAtras_txt.Location = new System.Drawing.Point(311, 156);
            this.AnguloHombroAdelanteAtras_txt.MaxLength = 3;
            this.AnguloHombroAdelanteAtras_txt.Name = "AnguloHombroAdelanteAtras_txt";
            this.AnguloHombroAdelanteAtras_txt.Size = new System.Drawing.Size(51, 26);
            this.AnguloHombroAdelanteAtras_txt.TabIndex = 18;
            this.AnguloHombroAdelanteAtras_txt.Text = "0";
            this.AnguloHombroAdelanteAtras_txt.TextChanged += new System.EventHandler(this.AnguloHombroAdelanteAtras_txt_TextChanged);
            this.AnguloHombroAdelanteAtras_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnguloHombroAdelanteAtras_txt_KeyDown);
            // 
            // AnguloCodoArribaAbajo_txt
            // 
            this.AnguloCodoArribaAbajo_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnguloCodoArribaAbajo_txt.Location = new System.Drawing.Point(310, 265);
            this.AnguloCodoArribaAbajo_txt.MaxLength = 3;
            this.AnguloCodoArribaAbajo_txt.Name = "AnguloCodoArribaAbajo_txt";
            this.AnguloCodoArribaAbajo_txt.Size = new System.Drawing.Size(51, 26);
            this.AnguloCodoArribaAbajo_txt.TabIndex = 19;
            this.AnguloCodoArribaAbajo_txt.Text = "0";
            this.AnguloCodoArribaAbajo_txt.TextChanged += new System.EventHandler(this.AnguloCodoArribaAbajo_txt_TextChanged);
            this.AnguloCodoArribaAbajo_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnguloCodoArribaAbajo_txt_KeyDown);
            // 
            // AnguloCodoDerechaIzquierda_txt
            // 
            this.AnguloCodoDerechaIzquierda_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnguloCodoDerechaIzquierda_txt.Location = new System.Drawing.Point(310, 378);
            this.AnguloCodoDerechaIzquierda_txt.MaxLength = 3;
            this.AnguloCodoDerechaIzquierda_txt.Name = "AnguloCodoDerechaIzquierda_txt";
            this.AnguloCodoDerechaIzquierda_txt.Size = new System.Drawing.Size(51, 26);
            this.AnguloCodoDerechaIzquierda_txt.TabIndex = 20;
            this.AnguloCodoDerechaIzquierda_txt.Text = "0";
            this.AnguloCodoDerechaIzquierda_txt.TextChanged += new System.EventHandler(this.AnguloCodoDerechaIzquierda_txt_TextChanged);
            this.AnguloCodoDerechaIzquierda_txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnguloCodoDerechaIzquierda_txt_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 18);
            this.label1.TabIndex = 21;
            this.label1.Text = "Hombro Arriba Abajo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(204, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "Hombro Adelante Atras";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(203, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Codo Arriba Abajo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(207, 357);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 18);
            this.label4.TabIndex = 24;
            this.label4.Text = "Codo Derecha Izquierda";
            // 
            // AngulosDefault_btn
            // 
            this.AngulosDefault_btn.Location = new System.Drawing.Point(6, 7);
            this.AngulosDefault_btn.Name = "AngulosDefault_btn";
            this.AngulosDefault_btn.Size = new System.Drawing.Size(109, 39);
            this.AngulosDefault_btn.TabIndex = 25;
            this.AngulosDefault_btn.Text = "Iniciar";
            this.AngulosDefault_btn.UseVisualStyleBackColor = true;
            this.AngulosDefault_btn.Click += new System.EventHandler(this.AngulosDefault_btn_Click);
            // 
            // AngulosAEnviar_lbl
            // 
            this.AngulosAEnviar_lbl.AutoSize = true;
            this.AngulosAEnviar_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AngulosAEnviar_lbl.Location = new System.Drawing.Point(11, 495);
            this.AngulosAEnviar_lbl.Name = "AngulosAEnviar_lbl";
            this.AngulosAEnviar_lbl.Size = new System.Drawing.Size(136, 26);
            this.AngulosAEnviar_lbl.TabIndex = 26;
            this.AngulosAEnviar_lbl.Text = "Enviando: ---";
            // 
            // EstadoEnvio_lbl
            // 
            this.EstadoEnvio_lbl.AutoSize = true;
            this.EstadoEnvio_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EstadoEnvio_lbl.Location = new System.Drawing.Point(12, 532);
            this.EstadoEnvio_lbl.Name = "EstadoEnvio_lbl";
            this.EstadoEnvio_lbl.Size = new System.Drawing.Size(124, 20);
            this.EstadoEnvio_lbl.TabIndex = 27;
            this.EstadoEnvio_lbl.Text = "Estado envio: ---";
            // 
            // HistorialConsumo_txtArea
            // 
            this.HistorialConsumo_txtArea.BackColor = System.Drawing.Color.Gainsboro;
            this.HistorialConsumo_txtArea.ForeColor = System.Drawing.Color.Black;
            this.HistorialConsumo_txtArea.Location = new System.Drawing.Point(421, 28);
            this.HistorialConsumo_txtArea.Multiline = true;
            this.HistorialConsumo_txtArea.Name = "HistorialConsumo_txtArea";
            this.HistorialConsumo_txtArea.ReadOnly = true;
            this.HistorialConsumo_txtArea.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HistorialConsumo_txtArea.Size = new System.Drawing.Size(351, 464);
            this.HistorialConsumo_txtArea.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(421, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 18);
            this.label5.TabIndex = 29;
            this.label5.Text = "Historial Consumo";
            // 
            // EnvioRecepcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.HistorialConsumo_txtArea);
            this.Controls.Add(this.EstadoEnvio_lbl);
            this.Controls.Add(this.AngulosAEnviar_lbl);
            this.Controls.Add(this.AngulosDefault_btn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AnguloCodoDerechaIzquierda_txt);
            this.Controls.Add(this.AnguloCodoArribaAbajo_txt);
            this.Controls.Add(this.AnguloHombroAdelanteAtras_txt);
            this.Controls.Add(this.AnguloHombroArribaAbajo_txt);
            this.Controls.Add(this.ConsumoCodoDerechaIzquierda_lbl);
            this.Controls.Add(this.ConsumoCodoArribaAbajo_lbl);
            this.Controls.Add(this.ConsumoHombroAdelanteAtras_lbl);
            this.Controls.Add(this.ConsumoHombroArribaAbajo_lbl);
            this.Controls.Add(this.HombroArribaAbajoMas_btn);
            this.Controls.Add(this.HombroArribaAbajoMenos_btn);
            this.Controls.Add(this.HombroAdelanteAtrasMas_btn);
            this.Controls.Add(this.HombroAdelanteAtrasMenos_btn);
            this.Controls.Add(this.CodoDerechaIzquierdaMenos_btn);
            this.Controls.Add(this.CodoDerechaIzquierdaMas_btn);
            this.Controls.Add(this.CodoArribaAbajoMenos_btn);
            this.Controls.Add(this.CodoArribaAbajoMas_btn);
            this.Controls.Add(this.MedicionesLabel);
            this.Controls.Add(this.AlertaLabel);
            this.Controls.Add(this.EnviarTextBox);
            this.Controls.Add(this.EnviarButton);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EnvioRecepcion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EnvioRecepcion";
            this.Load += new System.EventHandler(this.EnvioRecepcion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EnviarButton;
        private System.Windows.Forms.TextBox EnviarTextBox;
        private System.Windows.Forms.Label AlertaLabel;
        private System.Windows.Forms.Label MedicionesLabel;
        private System.Windows.Forms.Button CodoArribaAbajoMas_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CodoArribaAbajoMenos_btn;
        private System.Windows.Forms.Button CodoDerechaIzquierdaMenos_btn;
        private System.Windows.Forms.Button CodoDerechaIzquierdaMas_btn;
        private System.Windows.Forms.Button HombroAdelanteAtrasMas_btn;
        private System.Windows.Forms.Button HombroAdelanteAtrasMenos_btn;
        private System.Windows.Forms.Button HombroArribaAbajoMas_btn;
        private System.Windows.Forms.Button HombroArribaAbajoMenos_btn;
        private System.Windows.Forms.Label ConsumoHombroArribaAbajo_lbl;
        private System.Windows.Forms.Label ConsumoHombroAdelanteAtras_lbl;
        private System.Windows.Forms.Label ConsumoCodoArribaAbajo_lbl;
        private System.Windows.Forms.Label ConsumoCodoDerechaIzquierda_lbl;
        private System.Windows.Forms.TextBox AnguloHombroArribaAbajo_txt;
        private System.Windows.Forms.TextBox AnguloHombroAdelanteAtras_txt;
        private System.Windows.Forms.TextBox AnguloCodoArribaAbajo_txt;
        private System.Windows.Forms.TextBox AnguloCodoDerechaIzquierda_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button AngulosDefault_btn;
        private System.Windows.Forms.Label AngulosAEnviar_lbl;
        private System.Windows.Forms.Label EstadoEnvio_lbl;
        private System.Windows.Forms.TextBox HistorialConsumo_txtArea;
        private System.Windows.Forms.Label label5;
    }
}