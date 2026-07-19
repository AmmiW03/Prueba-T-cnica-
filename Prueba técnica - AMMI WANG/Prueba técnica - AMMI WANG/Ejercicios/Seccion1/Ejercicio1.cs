using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1
{
    internal class Ejercicio1
    {
        public static void res()
        {
            Console.Write("Por favor, ingresa una frase: ");
            string respuesta = Console.ReadLine();
            string[] palabra = respuesta.Split(' ');
            string resultado = "";
            foreach (string listPalabra in palabra)
            {
                string palabInvert = "";
                for (int i = listPalabra.Length - 1; i >= 0; i--)
                {
                    palabInvert += listPalabra[i];
                }
                resultado += palabInvert + " ";
            }
            Console.Write("Tu palabra invertida es: " + resultado);
        }
    }
}
