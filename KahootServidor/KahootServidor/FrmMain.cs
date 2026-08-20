using KahootServidor.CLASSES;
using Newtonsoft.Json;
using KahootServidor.CLASSES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahootServidor
{
    public partial class FrmMain : Form
    {
        private ClSocketServidor servidor;
        private ClProcessarProtocol protocolHandler;

        // Datos del juego
        private List<ClPregunta> preguntesJoc;
        private int preguntaActualIndex = -1;

        private Timer timerPregunta;
        private int tempsRestant;
        private Dictionary<string, int> puntuacions = new Dictionary<string, int>(); // Nickname -> Punts
        private Dictionary<string, int> puntsRondaActual = new Dictionary<string, int>();

        // Interfaz dinámica
        private Panel pnlRespostes;
        private Label[] lblOpcions = new Label[4];
        private Color[] colorsKahoot = { Color.Crimson, Color.RoyalBlue, Color.Goldenrod, Color.ForestGreen };


        public FrmMain()
        {
            InitializeComponent();
            ConfigurarInterficieDinamica();
            InicialitzarComponentsXarxa();
            MostrarIPsServidor();
            CanviarEstatVisual("INICI");
        }

        private void MostrarIPsServidor()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ips = string.Join(" | ", host.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString()));

            // Asume que tienes un Label o lo sacamos por Log
            Log($"IPs locals: {ips}");
        }

        private void InicialitzarComponentsXarxa()
        {
            servidor = new ClSocketServidor();
            protocolHandler = new ClProcessarProtocol();

            // 1. IMPORTANTE: Detectar conexión física inicial
            servidor.AlConnectar += (client) => {
                this.Invoke(new Action(() => {
                    Log("Nou client connectat (esperant Nickname...)");
                }));
            };

            // 2. Enlace entre Socket y Procesador de Protocolo
            servidor.AlRebreMissatge += (msg, client) => protocolHandler.Processar(msg, client);

            servidor.AlDesconnectar += (client) => {
                string nick = protocolHandler.GetNick(client);
                this.Invoke(new Action(() => {
                    // Buscamos y eliminamos el nick de la lista visual
                    if (lstJugadors.Items.Contains(nick)) lstJugadors.Items.Remove(nick);
                    protocolHandler.EliminarJugador(client);
                    Log($"Jugador desconnectat: {nick}");
                }));
            };

            // 3. Eventos del Protocolo (Aquí es donde se añade a la lista visual)
            protocolHandler.OnLoginSuccess += (nick, client) => {
                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(() => {
                        if (!lstJugadors.Items.Contains(nick))
                        {
                            lstJugadors.Items.Add(nick); // Aquí se añade a la ListBox
                        }
                        Log($"LOGIN OK: {nick}");
                        // Confirmamos al cliente que ya está dentro
                        _ = servidor.EnviarAUnAsync(client, "LOGIN_OK||{\"m\":\"Benvingut\"}");
                    }));
                }
            };

            protocolHandler.OnLoginFailed += (rao, client) => {
                Log($"LOGIN REBUTJAT: {rao}");
                _ = servidor.EnviarAUnAsync(client, $"LOGIN_ERR||{{\"m\":\"{rao}\"}}");
            };

            // 1. Configurar el Timer
            timerPregunta = new Timer();
            timerPregunta.Interval = 1000; // 1 segundo
            timerPregunta.Tick += TimerPregunta_Tick;


            protocolHandler.OnAnswerReceived += (nick, opcio, client) => {
                this.Invoke(new Action(() => {
                    if (timerPregunta.Enabled && preguntaActualIndex >= 0)
                    {
                        // Asegurar que el jugador existe en los diccionarios
                        if (!puntuacions.ContainsKey(nick)) puntuacions[nick] = 0;
                        if (!puntsRondaActual.ContainsKey(nick)) puntsRondaActual[nick] = 0;

                        var preguntaActual = preguntesJoc[preguntaActualIndex];
                        bool esCorrecto = (opcio == preguntaActual.RespuestaCorrecta);

                        if (esCorrecto)
                        {
                            // Cálculo por tiempo (Máximo 100, Mínimo 10)
                            double ratio = (double)tempsRestant / 15.0;
                            int puntosGanados = Math.Max(10, (int)(100 * ratio));

                            puntsRondaActual[nick] = puntosGanados;
                            puntuacions[nick] += puntosGanados;

                            Log($"{nick} ha contestat CORRECTE (+{puntosGanados} pts).");
                        }
                        else
                        {
                            puntsRondaActual[nick] = 0;
                            Log($"{nick} ha contestat INCORRECTE.");
                        }

                        int totalJugadors = protocolHandler.GetJugadors().Count;
                        int hanContestat = puntsRondaActual.Count;

                        if (hanContestat >= totalJugadors)
                        {
                            Log("Tots els jugadors han contestat. Finalitzant ronda immediatament.");
                            FinalitzarRondaForçada();
                        }
                    }
                }));
            };
        }
        private void ConfigurarInterficieDinamica()
        {
            // Creamos el panel de respuestas
            pnlRespostes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 300, // Ajusta esta altura según tu ventana
                Visible = false
            };

            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            for (int i = 0; i < 4; i++)
            {
                lblOpcions[i] = new Label
                {
                    Dock = DockStyle.Fill,
                    BackColor = colorsKahoot[i],
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(5),
                    Text = "" // Empezará vacío hasta que cargues la pregunta
                };
                tlp.Controls.Add(lblOpcions[i]);
            }

            pnlRespostes.Controls.Add(tlp);
            this.Controls.Add(pnlRespostes);
        }

        private void CanviarEstatVisual(string estat)
        {
            Action accio = () => {
                switch (estat)
                {
                    case "INICI":
                        gbConectar.Visible = true;
                        gbIniciar.Visible = false;
                        lstJugadorsTop.Visible = false;
                        btnContinuar.Visible = false;
                       
                        ProgressBar.Visible = false;
                        lblRonda.Visible = false;
                        lblPreguntaTitol.Visible = false;
                        pnlRespostes.Visible = false;
                        lblPodium.Visible = false;
                        lbEstat.Text = "Esperant per iniciar el servidor...";
                        break;
                    case "LOBBY":
                        gbConectar.Visible = false;
                        gbIniciar.Visible = true;
                        lblPodium.Visible = false;
                        lstJugadorsTop.Visible = false;
                        ProgressBar.Visible = false;
                        lblRonda.Visible = false;
                        lblPreguntaTitol.Visible = false;
                        lbEstat.Text = "LOBBY: Esperant jugadors...";
                        break;
                    case "JOC":
                        gbConectar.Visible = false;
                        gbIniciar.Visible = false;

                        ProgressBar.Visible = true;
                        lblRonda.Visible = true;
                        lblPreguntaTitol.Visible = true;
                        lbEstat.Visible = false;
                        pnlRespostes.Visible = true;

                        // 1. El Log se queda fijo a la izquierda
                        txtLog.Location = new Point(10, 10);

                        // 2. Calculamos el inicio de la zona de juego (Log + margen)
                        int inicioJocX = txtLog.Right + 20;
                        int ampleJoc = this.ClientSize.Width - inicioJocX - 20;

                        // 3. Posicionar Título, Barra y Ronda
                        lblPreguntaTitol.Location = new Point(inicioJocX, 30);
                        lblPreguntaTitol.Width = ampleJoc;

                        ProgressBar.Location = new Point(inicioJocX, lblPreguntaTitol.Bottom + 15);
                        ProgressBar.Width = ampleJoc;

                        lblRonda.Location = new Point(inicioJocX, ProgressBar.Bottom + 10);
                        lblRonda.Width = ampleJoc;

                        // 4. Posicionar el Panel de Botones (pnlRespostes)
                        // Lo bajamos para que no tape la barra de progreso
                        pnlRespostes.Location = new Point(inicioJocX, lblRonda.Bottom + 20);
                        pnlRespostes.Size = new Size(ampleJoc, this.ClientSize.Height - pnlRespostes.Top - 20);
    
                        pnlRespostes.BringToFront();

                        break;
                    case "RANKING":
                        // 1. Ocultamos los elementos del juego y el log
                        pnlRespostes.Visible = false;
                        ProgressBar.Visible = false;
                        lblRonda.Visible = false;
                        txtLog.Visible = false;
                        lblPreguntaTitol.Visible = false;

                        // 2. Configuramos el Título del Podio
                        lblPodium.Visible = true;
                        lblPodium.Text = "🏆 PÒDIUM FINAL 🏆";
                        lblPodium.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                        lblPodium.ForeColor = Color.Goldenrod;
                        lblPodium.TextAlign = ContentAlignment.MiddleCenter;

                        // 3. Posicionamos la Lista (tirada a la derecha)
                        lstJugadorsTop.Visible = true;

                  

                        lstJugadorsTop.Font = new Font("Consolas", 14, FontStyle.Bold);
                        lstJugadorsTop.BackColor = Color.WhiteSmoke;

                        // 4. Botón Continuar (debajo de la lista, alineado a su derecha)
                        btnContinuar.Visible = true;
                        btnContinuar.Size = new Size(200, 50);
                        btnContinuar.Text = "CONTINUAR 🔄";
                        btnContinuar.Visible = true;
                        btnContinuar.BackColor = Color.Orange;
                        btnContinuar.ForeColor = Color.White;
                        btnContinuar.FlatStyle = FlatStyle.Flat;

                        lbEstat.Visible = false;
                        break;
                }
            };

            if (this.IsHandleCreated)
            {
                this.Invoke(accio);
            }
            else
            {
                accio();
            }
        }

        private  void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                // Lanzamos el servidor en una tarea aparte para que NO bloquee el hilo de la UI
                Task.Run(() => servidor.IniciarEscoltaAsync(4444));

                Log("Servidor escoltant al port 4444...");
                CanviarEstatVisual("LOBBY");

                // Deshabilitamos el botón para no abrir el puerto dos veces
                btnIniciar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar el servidor: " + ex.Message);
            }
        }

        private void btnComencarPartida_Click(object sender, EventArgs e)
        {
            if (preguntesJoc != null && preguntesJoc.Count > 0)
            {
                puntuacions.Clear(); 
                preguntaActualIndex = -1; 
                CanviarEstatVisual("JOC");
                SeguentPregunta();
            }
            else
            {
                MessageBox.Show("Primer has de carregar un fitxer JSON.");
            }
        }

       


        private void btnCarregarJSON_Click(object sender, EventArgs e)
        {
            string rutaExecutant = AppDomain.CurrentDomain.BaseDirectory;
            // 2. Apuntamos a la carpeta 'PREGUNTES'
            string rutaPreguntes = Path.Combine(rutaExecutant, "PREGUNTES");

            // Si la carpeta no existe, la creamos para que no de error
            if (!Directory.Exists(rutaPreguntes))
            {
                Directory.CreateDirectory(rutaPreguntes);
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = rutaPreguntes, // Abre directamente nuestra carpeta
                Filter = "Arxius JSON|*.json",
                Title = "Selecciona el qüestionari del joc"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string json = File.ReadAllText(ofd.FileName);
                    preguntesJoc = JsonConvert.DeserializeObject<List<ClPregunta>>(json);

                    Log($"Carregat: {Path.GetFileName(ofd.FileName)} ({preguntesJoc.Count} preguntes).");
                    btnComencarPartida.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al llegir el JSON: " + ex.Message);
                }
            }
        }

        private void SeguentPregunta()
        {
            preguntaActualIndex++;
            if (preguntaActualIndex < preguntesJoc.Count)
            {
                var p = preguntesJoc[preguntaActualIndex];

                lblRonda.Text = $"Pregunta {preguntaActualIndex + 1} de {preguntesJoc.Count}";

                lblPreguntaTitol.Text = p.Texto;
                for (int i = 0; i < 4; i++) lblOpcions[i].Text = p.Opciones[i];

                tempsRestant = 15;
                ProgressBar.Maximum = 15;
                ProgressBar.Value = 15;
                timerPregunta.Start();

                var data = new { txt = p.Texto, opc = p.Opciones };
                servidor.EnviarATots("QUESTION||" + JsonConvert.SerializeObject(data));
            }
            else
            {
                FinalitzarPartida();
            }
        }

        private void TimerPregunta_Tick(object sender, EventArgs e)
        {
            tempsRestant--;
            if (tempsRestant >= 0) ProgressBar.Value = tempsRestant;

            if (tempsRestant <= 0)
            {
               
                FinalitzarRondaForçada();
            }
        }
        private void FinalitzarPartida()
        {
            timerPregunta.Stop();
            CanviarEstatVisual("RANKING");

            // 1. Ordenamos los jugadores por puntos de mayor a menor
            var rankingOrdenat = puntuacions.OrderByDescending(x => x.Value).ToList();

            lstJugadorsTop.Items.Clear();
            lstJugadorsTop.Font = new Font("Consolas", 14, FontStyle.Bold);

            for (int i = 0; i < rankingOrdenat.Count; i++)
            {
                string posicio = "";
                string jugador = rankingOrdenat[i].Key;
                int punts = rankingOrdenat[i].Value;

                // 2. Añadimos iconos según la posición
                switch (i)
                {
                    case 0: posicio = $"🥇 WINNER: {jugador} - {punts} pts"; break;
                    case 1: posicio = $"🥈 2nd: {jugador} - {punts} pts"; break;
                    case 2: posicio = $"🥉 3rd: {jugador} - {punts} pts"; break;
                    default: posicio = $"   {i + 1}th: {jugador} - {punts} pts"; break;
                }

                lstJugadorsTop.Items.Add(posicio);
            }

            // 3. Notificar a todos los clientes del ranking final
            string txtRanking = string.Join("\n", rankingOrdenat.Select((x, i) => $"{i + 1}. {x.Key}: {x.Value} pts"));
            servidor.EnviarATots("FINISH||" + JsonConvert.SerializeObject(new { r = txtRanking }));

            Log("Partida finalitzada. Pòdiu generat.");
        }
        private void Log(string m)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(m)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {m}{Environment.NewLine}");
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            // 1. Limpiar datos de la partida anterior
            puntuacions.Clear();
            puntsRondaActual.Clear();
            preguntaActualIndex = -1;

            // 2. Volver a la interfaz de espera
            btnContinuar.Visible = false;
            txtLog.Visible = true;
            txtLog.Clear();

            // 3. Notificar a los clientes que vuelven al Lobby
            servidor.EnviarATots("LOBBY_RESTART||{}");

            CanviarEstatVisual("LOBBY");
            Log("Sala reiniciada. Els jugadors es mantenen a l'espera.");
        }

        private void FinalitzarRondaForçada()
        {
            // 1. Detenemos el timer para evitar que se llame dos veces a esta función
            timerPregunta.Stop();

            // 2. Aseguramos que TODOS los jugadores conectados existen en el diccionario de puntos
            // (Si alguien se acaba de conectar y no ha hecho nada, debe aparecer con 0)
            var jugadors = protocolHandler.GetJugadors();
            foreach (var nick in jugadors.Values)
            {
                if (!puntuacions.ContainsKey(nick)) puntuacions[nick] = 0;
            }

            // 3. Calculamos el ranking actualizado
            var rankingActual = puntuacions.OrderByDescending(x => x.Value).ToList();

            Log("Ronda finalitzada. Enviant resultats amb posicions...");

            foreach (var entrada in jugadors)
            {
                TcpClient client = entrada.Key;
                string nick = entrada.Value;

                // 4. Calculamos posición (índice + 1). Si no está (raro), ponemos la última.
                int posicio = rankingActual.FindIndex(x => x.Key == nick) + 1;
                if (posicio <= 0) posicio = rankingActual.Count;

                int total = puntuacions.ContainsKey(nick) ? puntuacions[nick] : 0;
                int ganados = puntsRondaActual.ContainsKey(nick) ? puntsRondaActual[nick] : 0;
                bool fueCorrecto = ganados > 0;

                var resultadoRonda = new
                {
                    correcte = fueCorrecto,
                    puntsTotals = total,
                    puntsGuanyats = ganados,
                    posicioActual = posicio // Ahora este campo SIEMPRE se enviará
                };

                string mensaje = "ROUND_RESULT||" + JsonConvert.SerializeObject(resultadoRonda);
                _ = servidor.EnviarAUnAsync(client, mensaje);
            }

            // 5. Limpieza y siguiente pregunta
            puntsRondaActual.Clear();

            // Usamos Invoke para el delay y asegurar que SeguentPregunta corre en el hilo UI
            Task.Delay(4000).ContinueWith(_ => {
                if (!this.IsDisposed) this.Invoke(new Action(() => SeguentPregunta()));
            });
        }

        private void btnExpulsar_Click(object sender, EventArgs e)
        {
            if (lstJugadors.SelectedItem == null)
            {
                MessageBox.Show("Siusplau, selecciona un jugador de la llista per expulsar.");
                return;
            }

            string nickSeleccionat = lstJugadors.SelectedItem.ToString();

            // 2. Buscamos su TcpClient a través del protocolo
            TcpClient clientAExpulsar = protocolHandler.GetClientByNick(nickSeleccionat);

            if (clientAExpulsar != null)
            {
                // 3. Opcional: Enviamos un mensaje de "poca cortesía" antes de cerrar
                _ = servidor.EnviarAUnAsync(clientAExpulsar, "KICKED||{\"m\":\"Has estat expulsat de la sala por el moderador.\"}");

                // 4. Cerramos la conexión
                servidor.DesconnectarClient(clientAExpulsar);

                Log($"Jugador {nickSeleccionat} expulsat correctament.");
            }
        }
    }
}
