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
 * programador: Dylan Isaac Corado Urizar
 * Asignado por: Josue Daniel Revolorio Menendez
 * 
 */

namespace InsertarGraficador
{
    public class clasInsertarTabGrafica
    {
        
        //---------------funcion de prueba para probar la conexion--------------------
        
        public void IdentificacionGrafico(string sTipoGrafica, string sTituloGrafica, string sTituloEjeX, string sTituloEjeY, string[] sX, double[] sY) {
            DateTime fe = DateTime.Today;
            string sFecha = fe.ToString("d");
            int iTamano = sX.Length;
            string sCodigo="";
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("INSERT INTO TrGRAFICA VALUES('"+sFecha+"', '"+sTipoGrafica+"', '"+sTituloGrafica+"','"+sTituloEjeX+"', '"+sTituloEjeY+"')"), clasConexion.funConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se inserto con exito","Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT MAX(ncodgrafica) FROM TrGRAFICA WHERE ctipo='"+sTipoGrafica+"'"), clasConexion.funConexion());
                MySqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                    {
                        sCodigo = reader.GetString(0);                        
                    }

                

                for (int i = 0; i <= iTamano; i++)
                {
                    MySqlCommand comando3 = new MySqlCommand(string.Format("INSERT INTO MaPUNTO VALUES('" + sX[i] + "', '" + sY[i]+ "', '" + sCodigo + "')"), clasConexion.funConexion());
                    comando3.ExecuteNonQuery();
                }

            }
            catch {
                //System.Console.WriteLine("no se inserto");
                MessageBox.Show("Se produjo un error la creacion del Grafico!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
    }
}
