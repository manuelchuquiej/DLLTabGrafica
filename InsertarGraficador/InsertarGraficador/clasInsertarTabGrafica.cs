using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                MySqlCommand comando = new MySqlCommand(string.Format("insert INTO puntos VALUES('"+dato+"','"+dato2+"')"), clasConexion.funConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se inserto con exito","Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch {
                //System.Console.WriteLine("no se inserto");
                MessageBox.Show("Se produjo un error en tabla empleado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

    }
}
