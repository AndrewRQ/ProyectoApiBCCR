using ProyectoApiBCCR.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoApiBCCR
{
    internal class Program
    {
        // API Data
        private static string fechaConsulta = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        private static string codCompra = "317";
        private static string codVenta = "318";
        private static string nombre = "";
        private static string subNiveles = "N";
        private static string correo = "";
        private static string token = "";

        // Connection Data
        /* MODIFICAR ESTE STRING DE CONEXION POR EL PROPIO */
        private static string connectionString = "Data Source = KRAZIUSRQ\\SQLEXPRESS;Initial Catalog = PruebaApiBCCR; Integrated Security = True; Pooling=False;Encrypt=False;TrustServerCertificate=False";

        static async Task Main()
        {
            Console.WriteLine("Ejecutando...");
            var timer = new Timer(async _ => await Job(), null, TimeSpan.Zero, TimeSpan.FromHours(1));
            await Task.Delay(Timeout.Infinite);
        }
        private static async Task Job()
        {
            var cliente = new cr.fi.bccr.gee.wsindicadoreseconomicos();
            try
            {
                var tipoCambioCompra = cliente.ObtenerIndicadoresEconomicos(codCompra, fechaConsulta, fechaConsulta, nombre, subNiveles, correo, token);
                var tipoCambioVenta = cliente.ObtenerIndicadoresEconomicos(codVenta, fechaConsulta, fechaConsulta, nombre, subNiveles, correo, token);

                var nData = new BCCRDataModel
                {
                    fechaConsulta = DateTime.Parse(tipoCambioCompra.Tables[0].Rows[0].ItemArray[1].ToString()),
                    tipoCambioCompra = tipoCambioCompra.Tables[0].Rows[0].ItemArray[2].ToString(),
                    tipoCambioVenta = tipoCambioVenta.Tables[0].Rows[0].ItemArray[2].ToString()
                };
                await RegistrarEnDB(nData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al conectar con el api: " + ex.ToString());
            }
        }
        private static async Task RegistrarEnDB(BCCRDataModel data)
        {
            string query = "INSERT INTO [Resultados] ([FechaConsulta], [TipoCambioCompra], [TipoCambioVenta]) VALUES (@FechaConsulta, @TipoCambioCompra, @TipoCambioVenta)";

            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@FechaConsulta", data.fechaConsulta);
                            cmd.Parameters.AddWithValue("@TipoCambioCompra", data.tipoCambioCompra);
                            cmd.Parameters.AddWithValue("@TipoCambioVenta", data.tipoCambioVenta);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected > 0 ? "Datos guardados correctamente." : "No se pudo guardar en la base de datos.");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar en DB: " + ex.ToString());
            }
        }

    }
}
