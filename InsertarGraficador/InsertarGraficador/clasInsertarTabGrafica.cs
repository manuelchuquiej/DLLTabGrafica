using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * programador: Kevin Douglas Cajbon Asturias
 * programador: Dylan Isaac Corado Urizar
 * Asignado por: Josue Daniel Revolorio Menendez
 * 
 */

namespace InsertarGraficador
{
    public class clasInsertarTabGrafica
    {

        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------- FUNCION QUE INSERTA LOS DATOS DE LA GRAFICA QUE SE ESTA CREANDO EN LA BD--------------------------------//
        // La funcion realiza la insercion en cada una de las tablas que se encuentran en la BD
        
        public void funInsertarGrafico(string sTipoGrafica, string sTituloGrafica, string sTituloEjeX, string sTituloEjeY, string[] sX, double[] dX, double[] dY) {
            
            DateTime fe = DateTime.Today;
            string sFecha = fe.Year+"-"+fe.Month+"-"+fe.Day;   //-------- SE ALMACENA LA FECHA EN UNA VARIABLE PARA QUE ESTE SEA INSERTADO EN EL QUERY DE INSERCION--------         
            int iTamano = dY.Length;
            string sCodigo="";
            string sCodigoUsuario = "";
            try
            {                
                //----------INSERCION EN LA TABLA TrGRAFICA--------------//
                MySqlCommand comando = new MySqlCommand(string.Format("INSERT INTO TrGRAFICA (dfecha, ctipo, ctitulografica, cejex, cejey) VALUES('" + sFecha + "', '" + sTipoGrafica + "', '" + sTituloGrafica + "','" + sTituloEjeX + "', '" + sTituloEjeY + "', '"+sCodigoUsuario+"')"), clasConexion.funConexion());
                comando.ExecuteNonQuery();
                MessageBox.Show("Se inserto con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //----------SE OBTIENE EL CODIGO DE LA GRAFICA INSERTADA ARRIBA PARA PODER INGRESAR LOS PUNTOS DE ESTE--------------//
                MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT MAX(ncodgrafica) FROM TrGRAFICA WHERE ctipo='"+sTipoGrafica+"'"), clasConexion.funConexion());
                MySqlDataReader reader = comando2.ExecuteReader();

                while (reader.Read())
                    {
                        sCodigo = reader.GetString(0);                        
                    }                

                //----------SE HACE UN FOR PARA INSERTAR CADA UNO DE LOS PUNTOS QUE LA GRAFICA TENDRA---------------//
                for (int i = 0; i < iTamano; i++)
                {
                    if (dX==null)
                    {
                        MySqlCommand comando3 = new MySqlCommand(string.Format("INSERT INTO MaPUNTO(cx, cy, ncodgrafica) VALUES('" + sX[i] + "', '" + dY[i] + "', '" + sCodigo + "')"), clasConexion.funConexion());
                        comando3.ExecuteNonQuery();
                    }
                    else {
                        MySqlCommand comando3 = new MySqlCommand(string.Format("INSERT INTO MaPUNTO(cx, cy, ncodgrafica) VALUES('" + dX[i] + "', '" + dY[i] + "', '" + sCodigo + "')"), clasConexion.funConexion());
                        comando3.ExecuteNonQuery();
                    }                    
                }
            }
            catch {                
                MessageBox.Show("Se produjo un error la creacion del Grafico!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------        
        //----------------- FUNCION QUE ADQUIERE LOS TITULOS DE LAS GRAFICAS YA CONTENIDAS EN LA BD--------------------------------//
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

        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //----------------- FUNCION PARA PODER RECARGAR UNA GRAFICA EXISTENTE Y PODER CREA UNA NUEVA BASANDOSE EN ESTA--------------------------------//
        public Datos funConsultaGrafica(string sFecha, string sTituloGrafica) {
            string sTamanoLee="";            
            int iTamano;
            int iContador = 0;
            Datos dato = new Datos();

            //----------SELECT QUE OBTIENE EL NUMERO DE PUNTOS QUE LA GRAFICA POSEE--------------//
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT COUNT(MaPUNTO.cx), trgrafica.ctipo, trgrafica.ctitulografica, trgrafica.cejex, trgrafica.cejey FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='"+sFecha+"' and TrGrafica.ctitulografica='"+sTituloGrafica+"'"), clasConexion.funConexion());
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                sTamanoLee = reader.GetString(0);
                dato.tipo = reader.GetString(1);
                dato.titulo = reader.GetString(2);
                dato.nombre_ejex = reader.GetString(3);
                dato.nombre_ejey = reader.GetString(4);
            }            

            iTamano = System.Int32.Parse(sTamanoLee);
            

            dato.dx = new double[iTamano];
            dato.dy = new double[iTamano];
            dato.sx = new string[iTamano];

            //----------SELECT QUE OBTIENE TODOS LOS PUNTOS DE LA GRAFICA QUE SE DESEA--------------//
            MySqlCommand comando2 = new MySqlCommand(String.Format("SELECT MaPUNTO.cx, MaPUNTO.cy FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='" + sFecha + "' and TrGrafica.ctitulografica='" + sTituloGrafica + "'"), clasConexion.funConexion());
            MySqlDataReader reader2 = comando2.ExecuteReader();

            while (reader2.Read())
            {
                if ((dato.tipo.ToLower() == "lineal") || (dato.tipo.ToLower() == "pie"))
                {
                    dato.dx[iContador] = reader2.GetDouble(0);
                    dato.dy[iContador] = reader2.GetDouble(1);
                }
                else
                {
                    dato.sx[iContador] = reader2.GetString(0);
                    dato.dy[iContador] = reader2.GetDouble(1);
                }
                iContador++;
            }

            for (int j = 0; j < iTamano; j++) {
                System.Console.WriteLine(dato.sx[j]);
                System.Console.WriteLine(dato.dy[j]);
            }
                return dato;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
