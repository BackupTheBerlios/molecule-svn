//
// SQLiteProvidersHelper.cs
//
// Copyright (c) 2009 Pascal Fresnay (dev.molecule@free.fr) - Mickael Renault (dev.molecule@free.fr) 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mono.Data.Sqlite;
using System.IO;

namespace Molecule.Web.Security
{
    public class SQLiteProvidersHelper
    {
        public static string ConnectionString { get; private set; }
        public const string ApplicationName = "molecule";
        public const string AdminRoleName = "admin";

        static SQLiteProvidersHelper()
        {
            // the folder of webmusic exist ?
            string configDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationConfigDir = Path.Combine(configDir, ApplicationName);
            string databasePath = Path.Combine(applicationConfigDir, "users.db");
            ConnectionString = String.Format("URI=file:{0},version=3", databasePath);
            
            if (!File.Exists(databasePath))
            {
                InitDatabase(applicationConfigDir);
            }
        }

        private static void InitDatabase(string applicationConfigDir)
        {
            if (!Directory.Exists(applicationConfigDir))
            {
                Directory.CreateDirectory(applicationConfigDir);
            }

            using (IDbConnection dbcon = (IDbConnection)new SqliteConnection(ConnectionString))
            {
                dbcon.Open();
                CreateDatabase(dbcon);
            }
            CreateRole(AdminRoleName);
        }

        private static void CreateDatabase(IDbConnection dbcon)
        {
            IDbCommand dbcmd = dbcon.CreateCommand();

            var initDbSqlStream = typeof(SQLiteProvidersHelper).Assembly.GetManifestResourceStream("Molecule.Web.Security.InitDb.sql");
            using (var reader = new System.IO.StreamReader(initDbSqlStream))
            {
                dbcmd.CommandText = reader.ReadToEnd();
            }
            dbcmd.ExecuteNonQuery();
        }

        public static void CreateRole(string rolename)
        {
            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("INSERT INTO `Roles`" +
                    " (Rolename, ApplicationName) " +
                    " Values($Rolename, $ApplicationName)", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
