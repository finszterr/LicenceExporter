using System;
using System.Data.SqlClient;
using System.IO;


namespace LicenceExporter
{
    public class ExportLicence
    {
        public static void Export(string SQLServer, string UserName, string Password, string Database, SQLAuthenticationTypes ConnectionType)
        {
            Console.WriteLine("Export start");

            try
            {
                string table;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = SQLServer
                };
                if (string.IsNullOrEmpty(Database))
                {
                    builder.InitialCatalog = "master";
                    table = "[$ndo$srvproperty]";
                }
                else
                {
                    builder.UserID = UserName;
                    builder.Password = Password;
                    builder.InitialCatalog = Database;
                    table = "[$ndo$dbproperty]";
                }
                if (ConnectionType == SQLAuthenticationTypes.Windows)
                    builder.IntegratedSecurity = true;

                Console.WriteLine(builder.ConnectionString);
                SqlConnection Con = new SqlConnection(builder.ConnectionString);
                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT license from " + table, Con);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.Read())
                {
                    MemoryStream ms = new MemoryStream();
                    data.GetStream(0).CopyTo(ms);
                    //SaveFileDialog sfd = new SaveFileDialog();
                    //sfd.Filter = "License|*.flf";
                    //if (sfd.ShowDialog() == true)
                    //{
                    FileStream fs = new FileStream("filename.flf", FileMode.CreateNew);
                    ms.Position = 0;
                    ms.CopyTo(fs);
                    fs.Close();
                    Console.WriteLine("Succes, exported as " + "filename.flf");
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
