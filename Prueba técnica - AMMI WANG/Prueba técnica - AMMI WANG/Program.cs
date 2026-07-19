// See https://aka.ms/new-console-template for more information
using Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Definimos los 5 ejercicios en un arreglo
        Action[] ejercicios = new Action[] {
            Ejercicio1.res,
            Ejercicio2.res,
            Ejercicio3.res,
            Ejercicio4.res,
            Ejercicio5.res,
        };

        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("=== MENÚ DE EJERCICIOS ===");
            for (int i = 0; i < ejercicios.Length; i++)
            {
                Console.WriteLine($"{i + 1}. Ejercicio {i + 1}");
            }
            Console.WriteLine("6. Salir");
            Console.Write("\nSelecciona una opción: ");

            string opcion = Console.ReadLine();

            if (int.TryParse(opcion, out int num) && num >= 1 && num <= ejercicios.Length)
            {
                Console.Clear();
                ejercicios[num - 1](); // Ejecuta el ejercicio
                Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
                Console.ReadKey();
            }
            else if (opcion == "6")
            {
                salir = true;
            }
            else
            {
                Console.WriteLine("Opción no válida.");
                Console.ReadKey();
            }
        }
    }
}
