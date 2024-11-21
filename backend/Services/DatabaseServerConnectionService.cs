using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace backend.Services
{
    //Service which based on the OS platform, returns the appropriate connection string.
    //This is done because of the different development machines connection types
    //Windows connection type:
    //Connecting to Local SQL Server Instance
    //MacOS connection type: 
    //Connecting to remote SQL Server Instance
    public class DatabaseServerConnectionService
    {
        //Method to get the connection string based on the OS platform and the environment where the backend application is running, Production or Development, Production uses remote SQL Server instance, while Development based on the local development machine environment uses either remote or local SQL Server instance.
        public static string GetConnectionString(IConfiguration config, IHostEnvironment env)
        {
            //Check is the application running in the Development environment
            if (env.IsDevelopment())
            {
                //Check is the development machine running on MacOS, if so, use the remote SQL server connection string
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return config.GetConnectionString("DevelopmentMacOSConnection");
                }

                //Check is the development machine running on Windows, if so, use the local SQL server connection string
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return config.GetConnectionString("DevelopmentWindowsConnection");
                }
            }

            //If the application is running in the Production environment, use the remote SQL server connection string
            if (env.IsProduction())
            {
                return config.GetConnectionString("ProductionConnection");
            }

            throw new InvalidOperationException("Unknown environment. Cannot determine the connection string.");

        }
    }
}