using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KahootJugador.CLASSES
{
    public class ClSocketClient
    {
        private TcpClient socket;
        private NetworkStream stream;
        private CancellationTokenSource cts;

        public event Action<string> AlRebreMissatge;
        public event Action AlDesconnectar;

        public async Task<bool> ConnectarAsync(string ip, int port)
        {
            try
            {
                socket = new TcpClient();
                // Conectamos de forma asíncrona para no congelar la UI
                await socket.ConnectAsync(ip, port);
                stream = socket.GetStream();
                cts = new CancellationTokenSource();

                // Iniciamos la escucha en un hilo aparte
                _ = EscoltarServidorAsync(cts.Token);
                return true;
            }
            catch { return false; }
        }

        private async Task EscoltarServidorAsync(CancellationToken token)
        {
            byte[] buffer = new byte[4096];
            try
            {
                while (socket.Connected && !token.IsCancellationRequested)
                {
                    int llegits = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (llegits == 0) break;

                    // NO uses .Trim() aquí arriba.
                    string msg = Encoding.UTF8.GetString(buffer, 0, llegits);

                    // Separamos por el carácter que el servidor usa para finalizar mensajes
                    string[] lineas = msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var linea in lineas)
                    {
                        // Invocamos el evento con la línea limpia
                        AlRebreMissatge?.Invoke(linea.Trim());
                    }
                }
            }
            catch { }
            finally
            {
                AlDesconnectar?.Invoke();
                socket?.Close();
            }
        }

        public async Task EnviarMissatgeAsync(string msg)
        {
            if (socket != null && socket.Connected)
            {
                byte[] dades = Encoding.UTF8.GetBytes(msg + "\n");
                await stream.WriteAsync(dades, 0, dades.Length);
            }
        }
    }
}