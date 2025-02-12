using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen1
{
    internal class Program
    {
        static double totalHorasSemanales = 0;
        static int diasTrabajados = 0;
        static bool sinFaltas = true;
        static double totalRetrasos = 0;
        static double totalHorasExtras = 0;
        static double tarifaBase = 10.0;
        static string actividad;

        static void Main()
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Sistema de Registro de Horas Trabajadas");
                Console.WriteLine("1. Registrar horas trabajadas");
                Console.WriteLine("2. Calcular salario");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        SeleccionarActividad();
                        RegistrarSemana();
                        break;
                    case "2":
                        CalcularSalario();
                        break;
                    case "3":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void SeleccionarActividad()
        {
            Console.Clear();
            Console.WriteLine("Seleccione actividad realizada:");
            Console.WriteLine("1. Administrador\n2. Gerente\n3. Contador\n4. Facturador\n5. Cajero");
            Console.Write("Ingrese opción: ");
            actividad = Console.ReadLine();
        }

        static void RegistrarSemana()
        {
            totalHorasSemanales = 0;
            diasTrabajados = 0;
            totalRetrasos = 0;
            totalHorasExtras = 0;
            sinFaltas = true;

            for (int i = 1; i <= 7; i++)
            {
                Console.Clear();
                string dia = ObtenerDia(i);
                Console.WriteLine($"Registro de {dia}");
                Console.Write("¿Trabajó hoy? (s/n): ");
                string respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    diasTrabajados++;
                    Console.Write("Ingrese hora de entrada (hh:mm AM/PM): ");
                    string entrada = Console.ReadLine();
                    Console.Write("Ingrese hora de salida (hh:mm AM/PM): ");
                    string salida = Console.ReadLine();

                    if (ValidarHorario(entrada, salida, out double horasTrabajadas, out double retraso))
                    {
                        totalHorasSemanales += horasTrabajadas;
                        totalRetrasos += retraso;
                        if (horasTrabajadas > 8) totalHorasExtras += horasTrabajadas - 8;
                    }
                    else
                    {
                        sinFaltas = false;
                    }
                }
            }
        }

        static string ObtenerDia(int numDia)
        {
            switch (numDia)
            {
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miércoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sábado";
                case 7: return "Domingo";
                default: return "";
            }
        }

        static void CalcularSalario()
        {
            Console.Clear();
            Console.WriteLine("Cálculo de Salario");
            double tarifaPorActividad = ObtenerTarifaPorActividad();
            double salarioBruto = tarifaPorActividad * totalHorasSemanales;
            double deducciones = (totalRetrasos * tarifaPorActividad * 0.5);
            double bono = (sinFaltas && diasTrabajados >= 5) ? 50 : 0;
            double recargo = totalHorasExtras > 0 ? totalHorasExtras * (tarifaPorActividad * 1.5) : 0;
            double salarioNeto = salarioBruto - deducciones + bono + recargo;

            Console.WriteLine($"Salario bruto: ${salarioBruto}");
            Console.WriteLine($"Deducciones por retrasos: ${deducciones}");
            Console.WriteLine($"Bono por asistencia perfecta: ${bono}");
            Console.WriteLine($"Recargo por horas extras: ${recargo}");
            Console.WriteLine($"Salario neto: ${salarioNeto}");
            Console.WriteLine("Presione Enter para continuar...");
            Console.ReadLine();
        }

        static double ObtenerTarifaPorActividad()
        {
            switch (actividad)
            {
                case "1": return 15.0;
                case "2": return 20.0;
                case "3": return 18.0;
                case "4": return 12.0;
                case "5": return 10.0;
                default: return tarifaBase;
            }
        }

        static bool ValidarHorario(string entrada, string salida, out double horasTrabajadas, out double retraso)
        {
            horasTrabajadas = 0;
            retraso = 0;
            if (DateTime.TryParse(entrada, out DateTime horaEntrada) && DateTime.TryParse(salida, out DateTime horaSalida))
            {
                if (horaSalida > horaEntrada)
                {
                    horasTrabajadas = (horaSalida - horaEntrada).TotalHours;
                    if (horasTrabajadas < 4)
                    {
                        Console.WriteLine("Error: No se pueden registrar menos de 4 horas al día.");
                        return false;
                    }
                    if (horaEntrada.Hour > 9) // Retraso si entra después de las 9 AM
                    {
                        retraso = (horaEntrada.Hour - 9) + (horaEntrada.Minute / 60.0);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: La hora de salida no puede ser anterior a la de entrada.");
                }
            }
            return false;
        }
    }
}
