using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teste_COVID
{
    public class Test
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP { get; set; }
        public DateTime DataTest { get; set; }
        public RezultatTest Rezultat { get; set; }
    }

    public enum RezultatTest { 
        Pozitiv,
        Negativ,
        Indecis
    }
}
