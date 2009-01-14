//
// SQLiteRoleProvider.cs
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
	
using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using log4net;


namespace Molecule.Web.Security
{

	public sealed class SQLiteRoleProvider : RoleProvider
	{
		private string rolesTable = "Roles";
		private string usersInRolesTable = "UsersInRoles";
		
		private static string ConnectionString
		{
			get
			{
				return SQLiteProvidersHelper.ConnectionString;
			}
		}
		
		public override void Initialize(string name, NameValueCollection config)
		{
			// Initialize values from web.config.
			if (config == null)
				throw new ArgumentNullException("config");
			
			if (name == null || name.Length == 0)
				name = "SqliteRoleProvider";
			
			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "Sample Sqlite Role provider");
			}
			
			pApplicationName = SQLiteProvidersHelper.ApplicationName;
			
			// Initialize the abstract base class.
			base.Initialize(name, config);
		}
		
		private string pApplicationName;

		public override string ApplicationName
		{
			get { return pApplicationName; }
			set { pApplicationName = value; }
		}
		
		public override void AddUsersToRoles(string[] usernames, string[] rolenames)
		{
			foreach (string rolename in rolenames)
			{
				if (!RoleExists(rolename))
				{
					throw new ProviderException("Role name not found.");
				}
			}
			
			foreach (string username in usernames)
			{
				if (username.IndexOf(',') > 0)
				{
					throw new ArgumentException("User names cannot contain commas.");
				}
				
				foreach (string rolename in rolenames)
				{
					if (IsUserInRole(username, rolename))
					{
						throw new ProviderException("User is already in role.");
					}
				}
            }
			
			
            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("INSERT INTO `" + usersInRolesTable + "`" +
                    " (Username, Rolename, ApplicationName) " +
                    " Values($Username, $Rolename, $ApplicationName)", conn);


            SqliteParameter userParm = cmd.Parameters.Add("$Username", DbType.String, 255);
            SqliteParameter roleParm = cmd.Parameters.Add("$Rolename", DbType.String, 255);
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SqliteException)
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        public override void CreateRole(string rolename)
        {
            if (rolename.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("INSERT INTO `" + rolesTable + "`" +
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
        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("DELETE FROM `" + rolesTable + "`" +
                    " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;


            SqliteCommand cmd2 = new SqliteCommand("DELETE FROM `" + usersInRolesTable + "`" +
                    " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd2.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd2.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                cmd2.Transaction = tran;

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (SqliteException)
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }

            return true;
        }

        public override string[] GetAllRoles()
        {
            string tmpRoleNames = String.Empty;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Rolename FROM `" + rolesTable + "`" +
                      " WHERE ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        public override string[] GetRolesForUser(string username)
        {
            string tmpRoleNames = String.Empty;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Rolename FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String, 255).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        public override string[] GetUsersInRole(string rolename)
        {
            string tmpUserNames = String.Empty;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Username FROM `" + usersInRolesTable + "`" +
                      " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }
        public override bool IsUserInRole(string username, string rolename)
        {
            bool userIsInRole = false;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String, 255).Value = username;
            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();

                long numRecs = (long)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    userIsInRole = true;
                }
            }
            finally
            {
                conn.Close();
            }

            return userIsInRole;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }


            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("DELETE FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            SqliteParameter userParm = cmd.Parameters.Add("$Username", DbType.String, 255);
            SqliteParameter roleParm = cmd.Parameters.Add("$Rolename", DbType.String, 255);
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SqliteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SqliteException)
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT COUNT(*) FROM `" + rolesTable + "`" +
                      " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();

                long numRecs = (long)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    exists = true;
                }
            }
            finally
            {
                conn.Close();
            }

            return exists;
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Username FROM `" + usersInRolesTable + "` " +
                      "WHERE Username LIKE $UsernameSearch AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);
            cmd.Parameters.Add("$UsernameSearch", DbType.String, 255).Value = usernameToMatch;
            cmd.Parameters.Add("$RoleName", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = pApplicationName;

            string tmpUserNames = String.Empty;
            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

			if (tmpUserNames.Length > 0)
			{
				// Remove trailing comma.
				tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
				return tmpUserNames.Split(',');
			}

			return new string[0];
		}
		
		public override string Name
		{
			get
			{
				if (base.Name == null)
                    return this.GetType().Name;
				return base.Name;
            }
		}
		

    }
}