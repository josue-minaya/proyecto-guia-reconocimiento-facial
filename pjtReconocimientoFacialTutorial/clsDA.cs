using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data.SqlClient;

namespace pjtReconocimientoFacialTutorial
{
    public class clsDA
    {
       
        private static SqlConnection cnx = new SqlConnection("Data Source=FRANCISCO\\SQLEXPRESS;Initial Catalog=Tutoriales;Integrated Security=True");

        public static string[] Nombre;
        private static byte[] imagen;
        public static List<byte[]> ListadoRostros = new List<byte[]>();
        public static int TotalRostros;



        public static bool GuardarImagen(string Nombre, byte[] Imagen)
        {
            cnx.Close();
            cnx.Open();
            SqlCommand cmd = new SqlCommand("insert into imagen values (@id,@Nombre, @imagen)", cnx);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar);
            cmd.Parameters.Add("@imagen", SqlDbType.Image);

            cmd.Parameters["@id"].Value = 123456;
            cmd.Parameters["@Nombre"].Value = Nombre;
            MemoryStream ms = new MemoryStream();

            //pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            cmd.Parameters["@imagen"].Value = ms.GetBuffer();

            int resultado = cmd.ExecuteNonQuery();
            SqlParameter parImagen = new SqlParameter("@Imagen", SqlDbType.Binary, Imagen.Length);



            parImagen.Value = Imagen;
            //if (resultado > 0) return "exito";
            //else return "false";



            //SqlCommand cmd = new SqlCommand("INSERT INTO imagen  Values ('"+id+"','" + Nombre + "',?);", cnx);
            
            //cmd.Parameters.Add(parImagen);
            //int Resultado = cmd.ExecuteNonQuery();
            cnx.Close();

            return Convert.ToBoolean( resultado);


            //if (resultado > 0) return "exito";
            //else return "false";


        }


        public static DataTable Consultar(DataGridView DATA)
        {
            cnx.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM imagen;", cnx);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DATA.DataSource = dt;
            int Cont = dt.Rows.Count;
            Nombre = new string[Cont];
            cnx.Close();
            for (int i=0; i<Cont; i++)
            {
                Nombre[i] = dt.Rows[i]["Nombre"].ToString();
                imagen = (byte[])dt.Rows[i]["Imagen"];
                ListadoRostros.Add(imagen);

            }


            try
            {
                DATA.Columns[0].Width = 60;
                DATA.Columns[1].Width = 160;
                DATA.Columns[2].Width = 160;

                for (int i = 0; i < Cont; i++)
                {

                    DATA.Rows[i].Height = 110;
                }
            }
            catch 
            {
                
            }

            TotalRostros = Cont;
            
            return dt;

        }


        ////

        public static byte[] ConvertImgToBinary(Image img )
        {
            Bitmap bmp = new Bitmap(img);
            MemoryStream Memoria = new MemoryStream();
            bmp.Save(Memoria, ImageFormat.Bmp);

            byte[] imagen = Memoria.ToArray();

            return imagen;/// arreglo de Binario de la imagen

        }

        public static Image ConvertBinaryToImg(int C)
        {
            Image Imagen;
            byte[] img = ListadoRostros[C];
            MemoryStream Memoria = new MemoryStream(img);
            Imagen = Image.FromStream(Memoria);
            Memoria.Close();
            return Imagen;
        }
    }
}
