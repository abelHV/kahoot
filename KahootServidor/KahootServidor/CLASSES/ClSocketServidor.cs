using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace KahootServidor.CLASSES
{
    public class ClSocketServidor
    {
        private TcpListener xafarder;
        private List<TcpClient> llistaClients = new List<TcpClient>();
        private CancellationTokenSource cts;

        // Eventos para que el Formulario se entere de lo que pasa
        public event Action<string, TcpClient> AlRebreMissatge;
        public event Action<TcpClient> AlConnectar;
        public event Action<TcpClient> AlDesconnectar;

        public async Task IniciarEscoltaAsync(int port)
        {
            cts = new CancellationTokenSource();
            xafarder = new TcpListener(IPAddress.Any, port);
            xafarder.Start();

            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    // Aceptamos al nuevo cliente (Parte "Xafarder")
                    TcpClient client = await xafarder.AcceptTcpClientAsync();
                    llistaClients.Add(client);
                    AlConnectar?.Invoke(client);

                    // Escuchamos sus mensajes en un hilo aparte (Parte "Xerraire")
                    _ = GestionarFluxClientAsync(client, cts.Token);
                }
            }
            catch { /* Manejo de cierre de servidor */ }
        }

        private async Task GestionarFluxClientAsync(TcpClient client, CancellationToken token)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            string acumulado = "";

            try
            {
                while (client.Connected && !token.IsCancellationRequested)
                {
                    int llegits = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (llegits == 0) break;

                    acumulado += Encoding.UTF8.GetString(buffer, 0, llegits);
                    Console.WriteLine($"DEBUG REBUT: '{acumulado}'"); // Esto saldrá en la consola de Visual Studio

                    while (acumulado.Contains("\n"))
                    {
                        int pos = acumulado.IndexOf("\n");
                        string msg = acumulado.Substring(0, pos).Trim();
                        acumulado = acumulado.Substring(pos + 1);

                        if (!string.IsNullOrEmpty(msg))
                            AlRebreMissatge?.Invoke(msg, client);
                    }
                }
            }
            catch { }
            finally
            {
                llistaClients.Remove(client);
                AlDesconnectar?.Invoke(client);
                client.Close();
            }
        }

        // Método para enviar a un cliente específico
        public async Task EnviarAUnAsync(TcpClient client, string dades)
        {
            if (client != null && client.Connected)
            {
                byte[] msg = Encoding.UTF8.GetBytes(dades + "\n");
                await client.GetStream().WriteAsync(msg, 0, msg.Length);
            }
        }

        // Método para enviar a todos (Broadcast)
        public void EnviarATots(string dades)
        {
            byte[] msg = Encoding.UTF8.GetBytes(dades + "\n");
            foreach (var client in llistaClients.ToList())
            {
                try { client.GetStream().Write(msg, 0, msg.Length); } catch { }
            }
        }

        public void DesconnectarClient(TcpClient client)
        {
            try
            {
                if (client != null)
                {
                    client.Close(); // Esto disparará automáticamente el evento AlDesconnectar
                    llistaClients.Remove(client);
                }
            }
            catch { }
        }
    }
}