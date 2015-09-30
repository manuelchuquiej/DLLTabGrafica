using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * programador: Kevin Douglas Cajbon Asturias
 * programador: Manuel Alejandro Chuquiej Buch
 * Asignado por: Josue Daniel Revolorio Menendez
 * 
 */

namespace InsertarGraficador
{
    public class clasInsertarTabGrafica
    {
        
        //---------------funcion de prueba para probar la conexion--------------------
        public void prueba(string dato, string dato2) {
            try
            {
                MySqlCommand mComandop = new MySqlCommand(String.Format("Select * for trgrafica where madato='"+dato+"' and madato2='"+dato2+"'"), clasConexion.funConexion());
                MySqlDataReader mReader = mComandop.ExecuteReader();

                while(mReader.Read()){
                    dato = mReader.GetString(0);
                    dato2 = mReader.GetString(1);
                }
            }
            catch {
                
            }
        
        }

    }
}
