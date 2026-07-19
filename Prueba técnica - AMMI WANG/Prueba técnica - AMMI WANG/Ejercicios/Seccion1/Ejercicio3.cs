using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1
{
    internal class Ejercicio3
    {
        public static void res()
        {
            Console.Write("Ingrese el rango, dividido por ',': ");
            string[] num = Console.ReadLine().Split(',');
            int inicio = int.Parse(num[0].Trim());
            int fin = int.Parse(num[1].Trim());
            bool enc = false;

            for (int i = inicio; i <= fin - 2; i++)
            {
                if (primo(i) && primo(i + 2))
                {
                    Console.WriteLine($"({i},{i + 2})");
                }
            }

            if (!enc)
            {
                Console.WriteLine("0");
            }
        }

        static bool primo(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <=Math.Sqrt(n); i++)
            {
                if (n % i == 0) return false ;
            }
            return true;

        }
    }
}
