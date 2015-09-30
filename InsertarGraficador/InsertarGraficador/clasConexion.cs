using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InsertarGraficador
{
    class clasConexion
    {


        //-----------Se creo la Clase para conexion de inico de datos-----------------
        public static MySqlConnection funConexion()
        {

            MySqlConnection Conexion = new MySqlConnection("server =localhost; userid =root; password =; database = graficas");
            Conexion.Open();
            return Conexion;
        }
    }
}
