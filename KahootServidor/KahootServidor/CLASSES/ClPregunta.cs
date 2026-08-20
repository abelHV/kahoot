using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahootServidor.CLASSES
{
    public class ClPregunta
    {
        public string Texto { get; set; }
        public List<string> Opciones { get; set; }
        public int RespuestaCorrecta { get; set; } // Índice 0 a 3
    }
}
