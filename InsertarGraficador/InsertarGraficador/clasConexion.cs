﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InsertarGraficador
{
    class clasConexion
    {
        public static MySqlConnection funConexion()
        {

            MySqlConnection Conexion = new MySqlConnection("server =localhost; userid =root; password =; database =LABORATORIO");
            Conexion.Open();
            return Conexion;
        }
    }
}
