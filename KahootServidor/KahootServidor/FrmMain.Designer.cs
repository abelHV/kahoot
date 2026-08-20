namespace KahootServidor
{
    partial class FrmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnCarregarJSON = new System.Windows.Forms.Button();
            this.lblFitxerEstat = new System.Windows.Forms.Label();
            this.lstJugadors = new System.Windows.Forms.ListBox();
            this.btnExpulsar = new System.Windows.Forms.Button();
            this.lblPreguntaTitol = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.lblRonda = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnComencarPartida = new System.Windows.Forms.Button();
            this.gbConectar = new System.Windows.Forms.GroupBox();
            this.gbIniciar = new System.Windows.Forms.GroupBox();
            this.lbEstat = new System.Windows.Forms.Label();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.lstJugadorsTop = new System.Windows.Forms.ListBox();
            this.lblPodium = new System.Windows.Forms.Label();
            this.gbConectar.SuspendLayout();
            this.gbIniciar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(70, 157);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(100, 41);
            this.btnIniciar.TabIndex = 1;
            this.btnIniciar.Text = "Conectar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnCarregarJSON
            // 
            this.btnCarregarJSON.Location = new System.Drawing.Point(205, 285);
            this.btnCarregarJSON.Name = "btnCarregarJSON";
            this.btnCarregarJSON.Size = new System.Drawing.Size(96, 32);
            this.btnCarregarJSON.TabIndex = 3;
            this.btnCarregarJSON.Text = "Escogir";
            this.btnCarregarJSON.UseVisualStyleBackColor = true;
            this.btnCarregarJSON.Click += new System.EventHandler(this.btnCarregarJSON_Click);
            // 
            // lblFitxerEstat
            // 
            this.lblFitxerEstat.AutoSize = true;
            this.lblFitxerEstat.Location = new System.Drawing.Point(233, 266);
            this.lblFitxerEstat.Name = "lblFitxerEstat";
            this.lblFitxerEstat.Size = new System.Drawing.Size(36, 16);
            this.lblFitxerEstat.TabIndex = 4;
            this.lblFitxerEstat.Text = "Arxiu";
            // 
            // lstJugadors
            // 
            this.lstJugadors.FormattingEnabled = true;
            this.lstJugadors.ItemHeight = 16;
            this.lstJugadors.Location = new System.Drawing.Point(95, 50);
            this.lstJugadors.Name = "lstJugadors";
            this.lstJugadors.Size = new System.Drawing.Size(174, 196);
            this.lstJugadors.TabIndex = 6;
            // 
            // btnExpulsar
            // 
            this.btnExpulsar.Location = new System.Drawing.Point(73, 287);
            this.btnExpulsar.Name = "btnExpulsar";
            this.btnExpulsar.Size = new System.Drawing.Size(96, 32);
            this.btnExpulsar.TabIndex = 8;
            this.btnExpulsar.Text = "Expulsar";
            this.btnExpulsar.UseVisualStyleBackColor = true;
            this.btnExpulsar.Click += new System.EventHandler(this.btnExpulsar_Click);
            // 
            // lblPreguntaTitol
            // 
            this.lblPreguntaTitol.AutoSize = true;
            this.lblPreguntaTitol.Location = new System.Drawing.Point(386, 170);
            this.lblPreguntaTitol.Name = "lblPreguntaTitol";
            this.lblPreguntaTitol.Size = new System.Drawing.Size(61, 16);
            this.lblPreguntaTitol.TabIndex = 9;
            this.lblPreguntaTitol.Text = "Pregunta";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(303, 218);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(224, 23);
            this.ProgressBar.TabIndex = 10;
            // 
            // lblRonda
            // 
            this.lblRonda.AutoSize = true;
            this.lblRonda.Location = new System.Drawing.Point(393, 244);
            this.lblRonda.Name = "lblRonda";
            this.lblRonda.Size = new System.Drawing.Size(48, 16);
            this.lblRonda.TabIndex = 11;
            this.lblRonda.Text = "Ronda";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 32);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(214, 406);
            this.txtLog.TabIndex = 12;
            // 
            // btnComencarPartida
            // 
            this.btnComencarPartida.Location = new System.Drawing.Point(146, 345);
            this.btnComencarPartida.Name = "btnComencarPartida";
            this.btnComencarPartida.Size = new System.Drawing.Size(75, 23);
            this.btnComencarPartida.TabIndex = 13;
            this.btnComencarPartida.Text = "Iniciar";
            this.btnComencarPartida.UseVisualStyleBackColor = true;
            this.btnComencarPartida.Click += new System.EventHandler(this.btnComencarPartida_Click);
            // 
            // gbConectar
            // 
            this.gbConectar.Controls.Add(this.btnIniciar);
            this.gbConectar.Location = new System.Drawing.Point(271, 32);
            this.gbConectar.Name = "gbConectar";
            this.gbConectar.Size = new System.Drawing.Size(263, 353);
            this.gbConectar.TabIndex = 14;
            this.gbConectar.TabStop = false;
            // 
            // gbIniciar
            // 
            this.gbIniciar.Controls.Add(this.btnComencarPartida);
            this.gbIniciar.Controls.Add(this.btnCarregarJSON);
            this.gbIniciar.Controls.Add(this.lblFitxerEstat);
            this.gbIniciar.Controls.Add(this.lstJugadors);
            this.gbIniciar.Controls.Add(this.btnExpulsar);
            this.gbIniciar.Location = new System.Drawing.Point(271, 32);
            this.gbIniciar.Name = "gbIniciar";
            this.gbIniciar.Size = new System.Drawing.Size(357, 396);
            this.gbIniciar.TabIndex = 15;
            this.gbIniciar.TabStop = false;
            // 
            // lbEstat
            // 
            this.lbEstat.AutoSize = true;
            this.lbEstat.Location = new System.Drawing.Point(323, 13);
            this.lbEstat.Name = "lbEstat";
            this.lbEstat.Size = new System.Drawing.Size(37, 16);
            this.lbEstat.TabIndex = 16;
            this.lbEstat.Text = "Estat";
            // 
            // btnContinuar
            // 
            this.btnContinuar.Location = new System.Drawing.Point(41, 298);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(100, 41);
            this.btnContinuar.TabIndex = 2;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // lstJugadorsTop
            // 
            this.lstJugadorsTop.FormattingEnabled = true;
            this.lstJugadorsTop.ItemHeight = 16;
            this.lstJugadorsTop.Location = new System.Drawing.Point(41, 82);
            this.lstJugadorsTop.Name = "lstJugadorsTop";
            this.lstJugadorsTop.Size = new System.Drawing.Size(499, 196);
            this.lstJugadorsTop.TabIndex = 17;
            // 
            // lblPodium
            // 
            this.lblPodium.AutoSize = true;
            this.lblPodium.Location = new System.Drawing.Point(189, 45);
            this.lblPodium.Name = "lblPodium";
            this.lblPodium.Size = new System.Drawing.Size(0, 16);
            this.lblPodium.TabIndex = 18;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 515);
            this.Controls.Add(this.lblPodium);
            this.Controls.Add(this.lstJugadorsTop);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.lbEstat);
            this.Controls.Add(this.gbIniciar);
            this.Controls.Add(this.gbConectar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblRonda);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.lblPreguntaTitol);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.gbConectar.ResumeLayout(false);
            this.gbIniciar.ResumeLayout(false);
            this.gbIniciar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnCarregarJSON;
        private System.Windows.Forms.Label lblFitxerEstat;
        private System.Windows.Forms.ListBox lstJugadors;
        private System.Windows.Forms.Button btnExpulsar;
        private System.Windows.Forms.Label lblPreguntaTitol;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label lblRonda;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnComencarPartida;
        private System.Windows.Forms.GroupBox gbConectar;
        private System.Windows.Forms.GroupBox gbIniciar;
        private System.Windows.Forms.Label lbEstat;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.ListBox lstJugadorsTop;
        private System.Windows.Forms.Label lblPodium;
    }
}

