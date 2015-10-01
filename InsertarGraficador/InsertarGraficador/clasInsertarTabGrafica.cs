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
        
        public void InsertarGrafico(string sTipoGrafica, string sTituloGrafica, string sTituloEjeX, string sTituloEjeY, string[] sX, double[] sY) {
            DateTime fe = new DateTime();
            string sFecha = fe.Day+"-"+fe.Month+"-"+fe.Year;
            //string sfechan = ""
            int iTamano = sX.Length;
            string sCodigo="";
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("INSERT INTO TrGRAFICA (dfecha, ctipo, ctitulografica, cejex, cejey) VALUES('" + sFecha + "', '" + sTipoGrafica + "', '" + sTituloGrafica + "','" + sTituloEjeX + "', '" + sTituloEjeY + "')"), clasConexion.funConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se inserto con exito","Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT MAX(ncodgrafica) FROM TrGRAFICA WHERE ctipo='"+sTipoGrafica+"'"), clasConexion.funConexion());
                MySqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                    {
                        sCodigo = reader.GetString(0);                        
                    }                

                for (int i = 0; i < iTamano; i++)
                {
                    MySqlCommand comando3 = new MySqlCommand(string.Format("INSERT INTO MaPUNTO(cx, cy, ncodgrafica) VALUES('" + sX[i] + "', '" + sY[i]+ "', '" + sCodigo + "')"), clasConexion.funConexion());
                    comando3.ExecuteNonQuery();
                }

            }
            catch {
                //System.Console.WriteLine("no se inserto");
                MessageBox.Show("Se produjo un error la creacion del Grafico!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        public List<string> lConsultaTitulos()
        {
            List<string> lTitulos = new List<string>();
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT ctitulografica FROM TrGRAFICA"), clasConexion.funConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lTitulos.Add(reader.GetString(0));
            }

            return lTitulos;
        }

        public void ArregloX(string sFecha, string sTituloGrafica)
        {
            List<string> lX = new List<string>();
            
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT MaPUNTO.cx FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='"+sFecha+"' and TrGrafica.ctitulografica='"+sTituloGrafica+"'"), clasConexion.funConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lX.Add(reader.GetString(0));
            }

            string[] sX= new string[lX.Count];
            for(int i=0;i<=lX.Count;i++){
                sX[i] = lX[i];
            }
            //return lX[];
        }

        public void ArregloY(string sFecha, string sTituloGrafica)
        {
            List<string> lY = new List<string>();
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT MaPUNTO.cy FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='"+sFecha+"' and TrGrafica.ctitulografica='"+sTituloGrafica+"'"), clasConexion.funConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lY.Add(reader.GetString(0));
            }

            string[] sY = new string[lY.Count];
            for (int i = 0; i <= lY.Count; i++)
            {
                sY[i] = lY[i];
            }
        }     
    }
}
