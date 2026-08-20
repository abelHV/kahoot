namespace KahootJugador
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
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtNickname = new System.Windows.Forms.TextBox();
            this.btnConnectar = new System.Windows.Forms.Button();
            this.pnlLobby = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblPunts = new System.Windows.Forms.Label();
            this.lblCorrecte = new System.Windows.Forms.Label();
            this.lblPuntsGuanyats = new System.Windows.Forms.Label();
            this.lblEstat = new System.Windows.Forms.Label();
            this.lblPosicio = new System.Windows.Forms.Label();
            this.pnlLobby.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(93, 63);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 22);
            this.txtIP.TabIndex = 0;
            // 
            // txtNickname
            // 
            this.txtNickname.Location = new System.Drawing.Point(93, 117);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(100, 22);
            this.txtNickname.TabIndex = 2;
            // 
            // btnConnectar
            // 
            this.btnConnectar.Location = new System.Drawing.Point(103, 171);
            this.btnConnectar.Name = "btnConnectar";
            this.btnConnectar.Size = new System.Drawing.Size(75, 23);
            this.btnConnectar.TabIndex = 3;
            this.btnConnectar.Text = "Start";
            this.btnConnectar.UseVisualStyleBackColor = true;
            this.btnConnectar.Click += new System.EventHandler(this.btnConnectar_Click);
            // 
            // pnlLobby
            // 
            this.pnlLobby.Controls.Add(this.label2);
            this.pnlLobby.Controls.Add(this.txtNickname);
            this.pnlLobby.Controls.Add(this.label1);
            this.pnlLobby.Controls.Add(this.txtIP);
            this.pnlLobby.Controls.Add(this.btnConnectar);
            this.pnlLobby.Location = new System.Drawing.Point(243, 104);
            this.pnlLobby.Name = "pnlLobby";
            this.pnlLobby.Size = new System.Drawing.Size(231, 243);
            this.pnlLobby.TabIndex = 4;
            this.pnlLobby.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Nickname : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "IP Servidor :";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(33, 79);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(173, 327);
            this.txtLog.TabIndex = 5;
            // 
            // lblPunts
            // 
            this.lblPunts.AutoSize = true;
            this.lblPunts.Location = new System.Drawing.Point(311, 443);
            this.lblPunts.Name = "lblPunts";
            this.lblPunts.Size = new System.Drawing.Size(0, 16);
            this.lblPunts.TabIndex = 6;
            // 
            // lblCorrecte
            // 
            this.lblCorrecte.AutoSize = true;
            this.lblCorrecte.Location = new System.Drawing.Point(314, 470);
            this.lblCorrecte.Name = "lblCorrecte";
            this.lblCorrecte.Size = new System.Drawing.Size(0, 16);
            this.lblCorrecte.TabIndex = 7;
            // 
            // lblPuntsGuanyats
            // 
            this.lblPuntsGuanyats.AutoSize = true;
            this.lblPuntsGuanyats.Location = new System.Drawing.Point(311, 470);
            this.lblPuntsGuanyats.Name = "lblPuntsGuanyats";
            this.lblPuntsGuanyats.Size = new System.Drawing.Size(0, 16);
            this.lblPuntsGuanyats.TabIndex = 8;
            // 
            // lblEstat
            // 
            this.lblEstat.AutoSize = true;
            this.lblEstat.Location = new System.Drawing.Point(254, 378);
            this.lblEstat.Name = "lblEstat";
            this.lblEstat.Size = new System.Drawing.Size(0, 16);
            this.lblEstat.TabIndex = 0;
            // 
            // lblPosicio
            // 
            this.lblPosicio.AutoSize = true;
            this.lblPosicio.Location = new System.Drawing.Point(250, 457);
            this.lblPosicio.Name = "lblPosicio";
            this.lblPosicio.Size = new System.Drawing.Size(0, 16);
            this.lblPosicio.TabIndex = 9;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 506);
            this.Controls.Add(this.lblPosicio);
            this.Controls.Add(this.lblPuntsGuanyats);
            this.Controls.Add(this.lblCorrecte);
            this.Controls.Add(this.lblPunts);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblEstat);
            this.Controls.Add(this.pnlLobby);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.pnlLobby.ResumeLayout(false);
            this.pnlLobby.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtNickname;
        private System.Windows.Forms.Button btnConnectar;
        private System.Windows.Forms.GroupBox pnlLobby;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblPunts;
        private System.Windows.Forms.Label lblCorrecte;
        private System.Windows.Forms.Label lblPuntsGuanyats;
        private System.Windows.Forms.Label lblEstat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPosicio;
    }
}

