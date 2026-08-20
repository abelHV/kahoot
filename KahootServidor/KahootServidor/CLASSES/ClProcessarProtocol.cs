using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Linq;

namespace KahootServidor.CLASSES
{
    public class ClProcessarProtocol
    {
        // Eventos para comunicar resultados al Formulario
        public event Action<string, TcpClient> OnLoginSuccess;
        public event Action<string, TcpClient> OnLoginFailed;
        public event Action<string, int, TcpClient> OnAnswerReceived; // Nick, Opción, Client

        // Diccionario para saber qué Nickname corresponde a cada Socket
        // Muy importante para saber quién responde cada cosa
        private Dictionary<TcpClient, string> diccionariJugadors = new Dictionary<TcpClient, string>();

        public void Processar(string msg, TcpClient client)
        {
            try
            {
                string[] parts = msg.Split(new string[] { "||" }, StringSplitOptions.None);
                if (parts.Length < 2) return;

                string tipus = parts[0];
                string json = parts[1];

                switch (tipus)
                {
                    case "LOGIN":
                        GestionarLogin(json, client);
                        break;

                    case "ANSWER":
                        GestionarResposta(json, client);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al processar protocol: " + ex.Message);
            }
        }

        private void GestionarLogin(string json, TcpClient client)
        {
            try
            {
                // Cambiamos 'dynamic' por la clase explícita
                var dades = JsonConvert.DeserializeObject<ClLoginData>(json);
                string nick = dades?.Nickname;

                if (string.IsNullOrEmpty(nick)) return;

                if (diccionariJugadors.Values.Contains(nick))
                {
                    OnLoginFailed?.Invoke("Nickname ja ocupat", client);
                }
                else
                {
                    diccionariJugadors[client] = nick;
                    OnLoginSuccess?.Invoke(nick, client);
                }
            }
            catch (Exception ex)
            {
                // Esto te dirá en la consola si el JSON venía mal
                Console.WriteLine("Error decodificant Login: " + ex.Message);
            }
        }

        private void GestionarResposta(string json, TcpClient client)
        {
            var dades = JsonConvert.DeserializeObject<dynamic>(json);
            int opcio = dades.OpcióTriada;

            if (diccionariJugadors.ContainsKey(client))
            {
                string nick = diccionariJugadors[client];
                OnAnswerReceived?.Invoke(nick, opcio, client);
            }
        }

        // Método para cuando un cliente se desconecta
        public void EliminarJugador(TcpClient client)
        {
            if (diccionariJugadors.ContainsKey(client))
                diccionariJugadors.Remove(client);
        }
        public Dictionary<TcpClient, string> GetJugadors()
        {
            return diccionariJugadors;
        }

        public TcpClient GetClientByNick(string nick)
        {
            // Buscamos el primer cliente que tenga ese apodo
            return diccionariJugadors.FirstOrDefault(x => x.Value == nick).Key;
        }


        public string GetNick(TcpClient client) => diccionariJugadors.ContainsKey(client) ? diccionariJugadors[client] : "Anònim";
    }
}