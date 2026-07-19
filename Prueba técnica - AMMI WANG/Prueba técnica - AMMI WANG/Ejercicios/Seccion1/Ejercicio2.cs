using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1
{
    internal class Ejercicio2
    {

        public static void res()
        {
            Console.Write("Por favor, ingrese una cadena de números, separados por ',' o espacio: ");
            string respuesta = Console.ReadLine();
            string[] num = respuesta.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] nums = new int[num.Length];
            List<int> lista = new List<int>();

            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = int.Parse(num[i]);
            }

            foreach (int i in nums)
            {
                if (i % 2 == 0 && !lista.Contains(i))
                {
                    lista.Add(i);
                }
            }

            Console.Write("Resultado: " + string.Join(", ", lista));
        }
    }
}
