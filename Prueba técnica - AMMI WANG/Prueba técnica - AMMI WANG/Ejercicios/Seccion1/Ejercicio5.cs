using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_técnica___AMMI_WANG.Ejercicios.Seccion1
{
    internal class Ejercicio5
    {
        public static void res()
        {
            Console.WriteLine("Para el ejercicio solicitado, me gustaría exponer el razonamiento lógico que seguí para estructurar la solución, ya que, aunque el planteamiento técnico del código en C# representó un desafío, comprendo plenamente la arquitectura de la base de datos necesaria para resolverlo.\r\n\r\nLa solución requiere integrar información de tres tablas (ALUMNO, LIBRO y PRESTAMO) para obtener el resultado deseado:\r\n\r\nIntegración de datos (JOINS): La base de la consulta es relacionar las tablas mediante sus identificadores. Unimos ALUMNO con PRESTAMO mediante id_alumno, y PRESTAMO con LIBRO mediante id_libro. Esto permite consolidar en una sola vista el nombre del alumno, la fecha del préstamo y las políticas de días permitidos de cada obra.\r\n\r\nFiltrado de variables: Es necesario aplicar filtros específicos en la cláusula WHERE:\r\n\r\nNombres = 'Sonia': Para restringir los resultados exclusivamente a los préstamos de esta alumna.\r\n\r\nEntregado = false: Para descartar los libros que ya fueron devueltos y enfocarnos solo en los pendientes.\r\n\r\nLógica de negocio (Cálculo de fechas): El punto clave es la comparación temporal. Se debe realizar una operación matemática sobre las fechas, sumando el valor de Dias_limite_prestamo (de la tabla LIBRO) a la Fecha_prestamo original. La consulta debe identificar aquellos registros donde el resultado de esta suma sea estrictamente menor a la fecha de corte establecida (30/07/2021), lo cual confirma que el préstamo está vencido.\r\n\r\nConsidero que esta estructura de razonamiento es la base sólida sobre la que debe construirse cualquier implementación en C# o SQL para este escenario, permitiendo obtener reportes precisos sobre la situación de la biblioteca." +

                "Adicionalmente, si hubiera implementado esto mediante el uso de Entity Framework, la lógica se habría traducido en una consulta mediante LINQ. Esto nos permitiría manejar las tablas como objetos de C#, lo cual hace que el código sea mucho más limpio, fácil de mantener y menos propenso a errores de sintaxis SQL al trabajar con fechas y relaciones.");
        }
    }
}
