using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;

namespace warehouse2 {
    class Connect {
        const string FILE_NAME = "warehouse.mdb";
        const string TEAM_FILE_NAME = "Teams.mdb";
#if RELEASE
        static string location = Directory.GetCurrentDirectory() + "\\App_Data\\";
#elif TEST
#warning TEST is defined
        static string location = "C:\\Users\\USER\\Desktop\\wh\\App_Data\\";
#else
        static string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\bin\\Debug", string.Empty) + "/App_Data/";
#endif

        public Connect() { }
        public static string GetConnectionString() {
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; data source=" + location + FILE_NAME;
            return ConnectionString;
        }
        public static string GetConnectionStringTeams() {
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; data source=" + location + TEAM_FILE_NAME;
            return ConnectionString;
        }
    }
}
