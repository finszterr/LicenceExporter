using System;
using System.Data.SqlClient;
using System.IO;
using System.Management.Automation;
//DLL

namespace LicenceExporter
{
    // Declare the class as a cmdlet and specify the
    // appropriate verb and noun for the cmdlet name.
    [Cmdlet("Export", "Licence")]
    public class LicenceExporter : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Mandatory = true)]
        public string SQLServer { get; set; }

        [Parameter(Mandatory = true)]
        public string SQLDatabase { get; set; }

        [Parameter(Mandatory = true)]
        public string SQLUserName { get; set; }

        [Parameter(Mandatory = true)]
        public string SQLPassword { get; set; }

        [Parameter(Mandatory = true)]
        public SQLAuthenticationTypes SQLAuthenticationType { get; set; }

        [Parameter(Mandatory = false)]
        public string LicenceTargetPath { get; set; }

        [Parameter(Mandatory = false)]
        public string LicenceFilename{ get; set; }

        private bool overwrite;
        [Parameter(Mandatory = false)]
        public SwitchParameter Overwrite
        {
            get { return overwrite; }
            set { overwrite = value; }
        }

        // Override the ProcessRecord method to process
        // the supplied user name and write out a
        // greeting to the user by calling the WriteObject
        // method.
        protected override void ProcessRecord()
        {
            WriteObject("ProcessRecord: " + SQLServer + " " + SQLDatabase + " " + SQLUserName + " " + SQLPassword + " " + SQLAuthenticationType.ToString() + " " + Overwrite);
            //TODO: Error: The file 'C:\Users\finszterr\filename.flf' already exists.
            //**************
            ExportLicence.Export(SQLServer, SQLUserName, SQLPassword, SQLDatabase, SQLAuthenticationType, LicenceTargetPath, LicenceFilename, Overwrite);
        }
    }
}
