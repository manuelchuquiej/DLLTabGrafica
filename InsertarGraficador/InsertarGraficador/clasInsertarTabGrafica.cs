using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConexionODBC;
using System.Data.Odbc;

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
                OdbcCommand mySqlComando = new OdbcCommand(string.Format("INSERT INTO TrGRAFICA (dfecha, ctipo, ctitulografica, cejex, cejey) VALUES('" + sFecha + "', '" + sTipoGrafica + "', '" + sTituloGrafica + "','" + sTituloEjeX + "', '" + sTituloEjeY + "', '" + sCodigoUsuario + "')"), ConexionODBC.Conexion.ObtenerConexion());
                mySqlComando.ExecuteNonQuery();
                
                MessageBox.Show("Se inserto con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //----------SE OBTIENE EL CODIGO DE LA GRAFICA INSERTADA ARRIBA PARA PODER INGRESAR LOS PUNTOS DE ESTE--------------//
                OdbcCommand mySqlComando2 = new OdbcCommand(string.Format("SELECT MAX(ncodgrafica) FROM TrGRAFICA WHERE ctipo='" + sTipoGrafica + "'"), ConexionODBC.Conexion.ObtenerConexion());
                OdbcDataReader mySqlDLector = mySqlComando2.ExecuteReader();
                while (mySqlDLector.Read())
                {
                    sCodigo = mySqlDLector.GetString(0);
                }                                

                //----------SE HACE UN FOR PARA INSERTAR CADA UNO DE LOS PUNTOS QUE LA GRAFICA TENDRA---------------//
                for (int i = 0; i < iTamano; i++)
                {
                    if (dX==null)
                    {
                        OdbcCommand mySqlComando3 = new OdbcCommand(string.Format("INSERT INTO MaPUNTO(cx, cy, ncodgrafica) VALUES('" + sX[i] + "', '" + dY[i] + "', '" + sCodigo + "')"), ConexionODBC.Conexion.ObtenerConexion());
                        mySqlComando3.ExecuteNonQuery();                        
                    }
                    else {
                        OdbcCommand mySqlComando4 = new OdbcCommand(string.Format("INSERT INTO MaPUNTO(cx, cy, ncodgrafica) VALUES('" + dX[i] + "', '" + dY[i] + "', '" + sCodigo + "')"), ConexionODBC.Conexion.ObtenerConexion());
                        mySqlComando4.ExecuteNonQuery();                        
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
            OdbcCommand mySqlComando2 = new OdbcCommand(string.Format("SELECT ctitulografica FROM TrGRAFICA"), ConexionODBC.Conexion.ObtenerConexion());
            OdbcDataReader mySqlDLector = mySqlComando2.ExecuteReader();
            while (mySqlDLector.Read())
            {
                lTitulos.Add(mySqlDLector.GetString(0));
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
            OdbcCommand mySqlComando2 = new OdbcCommand(string.Format("SELECT COUNT(MaPUNTO.cx), trgrafica.ctipo, trgrafica.ctitulografica, trgrafica.cejex, trgrafica.cejey FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='" + sFecha + "' and TrGrafica.ctitulografica='" + sTituloGrafica + "'"), ConexionODBC.Conexion.ObtenerConexion());
            OdbcDataReader mySqlDLector = mySqlComando2.ExecuteReader();
            while (mySqlDLector.Read())
            {
                sTamanoLee = mySqlDLector.GetString(0);
                dato.tipo = mySqlDLector.GetString(1);
                dato.titulo = mySqlDLector.GetString(2);
                dato.nombre_ejex = mySqlDLector.GetString(3);
                dato.nombre_ejey = mySqlDLector.GetString(4);
            }                         

            iTamano = System.Int32.Parse(sTamanoLee);
            

            dato.dx = new double[iTamano];
            dato.dy = new double[iTamano];
            dato.sx = new string[iTamano];

            //----------SELECT QUE OBTIENE TODOS LOS PUNTOS DE LA GRAFICA QUE SE DESEA--------------//
            OdbcCommand mySqlComando3 = new OdbcCommand(string.Format("SELECT MaPUNTO.cx, MaPUNTO.cy FROM MaPUNTO, TrGRAFICA WHERE MaPUNTO.ncodgrafica=TrGRAFICA.ncodgrafica and TrGRAFICA.dfecha='" + sFecha + "' and TrGrafica.ctitulografica='" + sTituloGrafica + "'"), ConexionODBC.Conexion.ObtenerConexion());
            OdbcDataReader mySqlDLector2 = mySqlComando3.ExecuteReader();
            while (mySqlDLector2.Read())
            {
                if ((dato.tipo.ToLower() == "lineal") || (dato.tipo.ToLower() == "pie"))
                {
                    dato.dx[iContador] = mySqlDLector2.GetDouble(0);
                    dato.dy[iContador] = mySqlDLector2.GetDouble(1);
                }
                else
                {
                    dato.sx[iContador] = mySqlDLector2.GetString(0);
                    dato.dy[iContador] = mySqlDLector2.GetDouble(1);
                }
                iContador++;
            }                       
            
            return dato;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
