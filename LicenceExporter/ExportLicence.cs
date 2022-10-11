using System;
using System.Data.SqlClient;
using System.IO;


namespace LicenceExporter
{
    public class ExportLicence
    {
        public static void Export(
            string SQLServer,
            string UserName,
            string Password,
            string Database,
            SQLAuthenticationTypes ConnectionType,
            string LicenceTargetPath,
            string LicenceFilename,
            bool Overwrite)
        {
            Console.WriteLine("Export started");

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
                    if (LicenceFilename == "")
                        LicenceFilename = "licence.flf";
                    try
                    {
                        Path.GetFullPath(LicenceTargetPath);
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                        Console.WriteLine("Path " + LicenceTargetPath + " is invalid. Default path will be used.");
                        LicenceTargetPath = "";
                    }

                    if (!LicenceTargetPath.EndsWith(@"\"))
                        LicenceTargetPath += @"\";

                    if (!LicenceFilename.EndsWith(".flf"))
                        LicenceFilename += ".flf";

                    FileStream fs;
                    if (Overwrite) {
                        fs = new FileStream(LicenceTargetPath + LicenceFilename, FileMode.Create);
                    } else
                    {
                        fs = new FileStream(LicenceTargetPath + LicenceFilename, FileMode.CreateNew);
                    }
                    
                    ms.Position = 0;
                    ms.CopyTo(fs);
                    if (fs.Length == 0)
                    {
                        Console.WriteLine("Empty file created :( " + fs.Name);
                        return;
                    }
                    fs.Close();
                    Console.WriteLine("Succes, exported as " + fs.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
