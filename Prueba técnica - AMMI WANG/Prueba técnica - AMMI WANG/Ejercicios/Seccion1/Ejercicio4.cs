using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1
{
    internal class Ejercicio4
    {
        public static void res()
        {
            Console.Write("Ingrese el arreglo de enteros, dividido por ',': ");
            string[] entrada = Console.ReadLine().Split(',');
            int[] numeros = new int[entrada.Length];

            for (int i = 0; i < entrada.Length; i++)
            {
                numeros[i] = int.Parse(entrada[i].Trim());
            }

            Console.Write("Ingrese su objetivo: ");
            int objetivo = int.Parse(Console.ReadLine());

            if (suma(numeros, 0, objetivo))
                Console.WriteLine("En esta cadena se cumple la combinación de elementos para su objetivo :)");
            else
                Console.WriteLine("En esta cadena no se cumple la combinación de elementos para su objetivo :(");

            static bool suma(int[] nums, int i, int objetivo)
            {
                // 1. ¿Llegamos al objetivo?
                if (objetivo == 0) return true;

                // 2. ¿Nos pasamos o se acabaron los números?
                if (i >= nums.Length || objetivo < 0) return false;

                // 3. ¿Existe el camino al incluir el número O al excluirlo?
                return suma(nums, i + 1, objetivo - nums[i]) ||
                       suma(nums, i + 1, objetivo);
            }
        }
    }
}
