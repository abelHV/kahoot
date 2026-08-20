using KahootJugador.CLASSES;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahootJugador
{
    public partial class FrmMain : Form
    {
        private ClSocketClient cliente;
        private Button[] btnColors = new Button[4];
        private Color[] colorsKahoot = { Color.Crimson, Color.RoyalBlue, Color.Goldenrod, Color.ForestGreen };
        private Panel pnlComandament;
        public FrmMain()
        {
            InitializeComponent();
            ConfigurarBotoresColors();
            cliente = new ClSocketClient();
            cliente.AlRebreMissatge += ProcessarProtocol;
            cliente.AlDesconnectar += () => this.Invoke(new Action(() => {
                Log("S'ha perdut la connexió amb el servidor.");
                ResetUIClient();
            }));
        }


        private void ConfigurarBotoresColors()
        {
            pnlComandament = new Panel { Dock = DockStyle.Fill, Visible = false };
            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };

            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            for (int i = 0; i < 4; i++)
            {
                btnColors[i] = new Button
                {
                    Dock = DockStyle.Fill,
                    BackColor = colorsKahoot[i],
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false
                };
                int index = i;
                btnColors[i].Click += (s, e) => EnviarResposta(index);
                tlp.Controls.Add(btnColors[i]);
            }
            pnlComandament.Controls.Add(tlp);
            this.Controls.Add(pnlComandament);
            pnlComandament.BringToFront();
        }

        private void ProcessarProtocol(string msg)
        {
            string[] parts = msg.Split(new string[] { "||" }, 2, StringSplitOptions.None);
            string tipus = parts[0];

            this.Invoke(new Action(() => {
                switch (tipus)
                {
                    case "LOGIN_OK":
                        pnlLobby.Visible = false;
                        pnlComandament.Visible = true;
                        txtLog.Visible = false;
                        lblEstat.Text = "Connectat! Esperant partida...";
                        break;

                    case "QUESTION":
                        // Empezamos ronda: botones visibles y labels de resultado ocultos
                        pnlComandament.Visible = true;
                        lblCorrecte.Visible = false;
                        lblEstat.Visible = true;
                        lblEstat.Text = "¡TRIA UNA RESPOSTA!";
                        lblEstat.ForeColor = Color.Green;

                        ResetBotons();
                        ActivarBotons(true);
                        break;

                    case "TIME_UP":
                        // Fin de tiempo: ocultamos botones para que se vean los labels detrás
                        pnlComandament.Visible = false;
                        lblEstat.Text = "TEMPS ESGOTAT";
                        lblEstat.ForeColor = Color.Red;
                        break;

                    case "ROUND_RESULT":
                        // 1. Ocultamos el panel de juego para dejar espacio libre
                        pnlComandament.Visible = false;

                        var dades = JsonConvert.DeserializeObject<dynamic>(parts[1]);
                        bool esCorrecte = (bool)dades.correcte;
                        int pos = (int)dades.posicioActual; // Cast a int para evitar el error de null

                        // 2. Configuración de textos y colores
                        lblPunts.Text = $"PUNTUACIÓ TOTAL: {dades.puntsTotals} pts";
                        lblPuntsGuanyats.Text = $"+{dades.puntsGuanyats} Punts en aquesta ronda";
                        lblEstat.Text = "Espera la següent pregunta...";

                        // Estilo específico para la posición
                        lblPosicio.Text = (pos == 1) ? "👑 ETS EL LÍDER! #" + pos : $"Estàs a la posició: #{pos}";
                        lblPosicio.ForeColor = (pos <= 3) ? Color.Goldenrod : Color.DarkSlateGray;

                        if (esCorrecte)
                        {
                            lblCorrecte.Text = "¡CORRECTE! 🌟";
                            lblCorrecte.ForeColor = Color.ForestGreen;
                            lblPuntsGuanyats.ForeColor = Color.LimeGreen;
                        }
                        else
                        {
                            lblCorrecte.Text = "INCORRECTE ❌";
                            lblCorrecte.ForeColor = Color.Crimson;
                            lblPuntsGuanyats.Text = "+0 Punts";
                            lblPuntsGuanyats.ForeColor = Color.Gray;
                        }

                        // 3. FUENTES Y POSICIONAMIENTO
                        lblCorrecte.Font = new Font("Segoe UI", 32, FontStyle.Bold);
                        lblPuntsGuanyats.Font = new Font("Segoe UI", 20, FontStyle.Bold);
                        lblPosicio.Font = new Font("Segoe UI", 18, FontStyle.Bold); // Fuente destacada para posición
                        lblPunts.Font = new Font("Segoe UI", 14, FontStyle.Regular);
                        lblEstat.Font = new Font("Segoe UI", 12, FontStyle.Italic);

                        // Hacerlos visibles
                        lblCorrecte.Visible = true;
                        lblPuntsGuanyats.Visible = true;
                        lblPunts.Visible = true;
                        lblPosicio.Visible = true;

                        // CENTRADO DINÁMICO
                        lblCorrecte.Location = new Point((this.ClientSize.Width - lblCorrecte.Width) / 2, 80);
                        lblPuntsGuanyats.Location = new Point((this.ClientSize.Width - lblPuntsGuanyats.Width) / 2, lblCorrecte.Bottom + 15);

                        // Posicionamos el lblPosicio entre los puntos de ronda y el total
                        lblPosicio.Location = new Point((this.ClientSize.Width - lblPosicio.Width) / 2, lblPuntsGuanyats.Bottom + 30);

                        lblPunts.Location = new Point((this.ClientSize.Width - lblPunts.Width) / 2, lblPosicio.Bottom + 20);
                        lblEstat.Location = new Point((this.ClientSize.Width - lblEstat.Width) / 2, this.ClientSize.Height - 60);

                        break;

                    case "LOBBY_RESTART":
                        // Ocultamos todos los labels de resultados
                        lblCorrecte.Visible = false;
                        lblPuntsGuanyats.Visible = false;
                        lblPunts.Visible = false;
                        pnlLobby.Visible = false;
                        lblPosicio.Visible = false; 

                        // Volvemos a mostrar el estado de espera
                        lblEstat.Visible = true;
                        lblEstat.Text = "El servidor ha reiniciat la sala. Esperant nova partida...";
                        lblEstat.Location = new Point((this.ClientSize.Width - lblEstat.Width) / 2, (this.ClientSize.Height / 2));
                        break;
                }
            }));
        }
        private void ActivarBotons(bool estat)
        {
            foreach (var b in btnColors) b.Enabled = estat;
        }

        private async void EnviarResposta(int index)
        {
            // Bloqueamos inmediatamente para evitar múltiples clics
            ActivarBotons(false);

            lblEstat.Text = "Resposta enviada. Esperant...";
            lblEstat.ForeColor = Color.Orange;

            // Efecto visual: oscurecemos los botones no elegidos
            for (int i = 0; i < 4; i++)
            {
                if (i != index)
                {
                    btnColors[i].BackColor = Color.FromArgb(50, btnColors[i].BackColor);
                }
            }

            var resposta = new ClPeticioResposta { OpcióTriada = index };
            await cliente.EnviarMissatgeAsync("ANSWER||" + JsonConvert.SerializeObject(resposta));
        }
        private async void btnConnectar_Click(object sender, EventArgs e)
        {
            string nick = txtNickname.Text.Trim();
            if (string.IsNullOrEmpty(nick)) return;

            bool ok = await cliente.ConnectarAsync(txtIP.Text, 4444);
            if (ok)
            {
                var loginMsg = new { Nickname = nick };
                await cliente.EnviarMissatgeAsync("LOGIN||" + JsonConvert.SerializeObject(loginMsg));
            }
            else { MessageBox.Show("No s'ha pogut connectar al servidor."); }
        }

        private void ResetBotons()
        {
            for (int i = 0; i < 4; i++)
            {
                btnColors[i].BackColor = colorsKahoot[i];
            }
        }
        private void ResetUIClient()
        {
            pnlComandament.Visible = false;
            pnlLobby.Visible = true;
            txtLog.Visible = true; // Volvemos a mostrar el log para ver el error
            lblCorrecte.Visible = false;
            lblPuntsGuanyats.Visible = false;
            lblPunts.Visible = false;
            lblEstat.Visible = false;
            btnConnectar.Enabled = true;
        }

        private void Log(string mensaje)
        {
            if (txtLog.InvokeRequired)
            {
                this.Invoke(new Action(() => Log(mensaje)));
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}");

            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    }
}

