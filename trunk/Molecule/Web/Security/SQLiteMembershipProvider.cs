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
using System.IO;
using System.Text;

using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

using Mono.Data.Sqlite;
using System.Security.Cryptography;
using System.Globalization;



namespace Molecule.Web.Security
{


    public class SQLiteMembershipProvider : MembershipProvider
    {
        private string connectionString
        {
            get { return SQLiteProvidersHelper.ConnectionString; }
        }

        private string tableName = "users";
        private const string encryptionKey = "AE09F72BA97CBBB5";
        private string pApplicationName;

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SQLiteMembershipProvider));

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }


        public SQLiteMembershipProvider()
        {
            pApplicationName = SQLiteProvidersHelper.ApplicationName;
        }



        public override bool ChangePassword(string username,
                                            string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");


            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("UPDATE `" + tableName + "`" +
                    " SET Password = $Password, LastPasswordChangedDate = $LastPasswordChangedDate " +
                    " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Password", DbType.String).Value = EncodePassword(newPassword);
            cmd.Parameters.Add("$LastPasswordChangedDate", DbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;


            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion,
                                                             string newPasswordAnswer)
        {
            return true;
        }

        public override MembershipUser CreateUser(string username,
                                                  string password, string email, string passwordQuestion,
                                                  string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }



            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }


            MembershipUser u = GetUser(username, false);


            if (u == null)
            {
                DateTime createDate = DateTime.Now;

                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                SqliteConnection conn = new SqliteConnection(connectionString);
                SqliteCommand cmd = new SqliteCommand("INSERT INTO `" + tableName + "`" +
                                                      " (PKID, Username, Password, Email, PasswordQuestion, " +
                                                      " PasswordAnswer, IsApproved," +
                                                      " Comment, CreationDate, LastPasswordChangedDate, LastActivityDate," +
                                                      " ApplicationName, IsLockedOut, LastLockedOutDate," +
                                                      " FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, " +
                                                      " FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart)" +
                                                      " Values($PKID, $Username, $Password, $Email, $PasswordQuestion, " +
                                                      " $PasswordAnswer, $IsApproved, $Comment, $CreationDate, $LastPasswordChangedDate, " +
                                                      " $LastActivityDate, $ApplicationName, $IsLockedOut, $LastLockedOutDate, " +
                                                      " $FailedPasswordAttemptCount, $FailedPasswordAttemptWindowStart, " +
                                                      " $FailedPasswordAnswerAttemptCount, $FailedPasswordAnswerAttemptWindowStart)", conn);

                cmd.Parameters.Add("$PKID", DbType.String).Value = providerUserKey.ToString();
                cmd.Parameters.Add("$Username", DbType.String).Value = username;
                cmd.Parameters.Add("$Password", DbType.String).Value = EncodePassword(password);
                cmd.Parameters.Add("$Email", DbType.String).Value = email;
                cmd.Parameters.Add("$PasswordQuestion", DbType.String).Value = passwordQuestion;
                cmd.Parameters.Add("$PasswordAnswer", DbType.String).Value = EncodePassword(passwordAnswer);
                cmd.Parameters.Add("$IsApproved", DbType.Boolean).Value = isApproved;
                cmd.Parameters.Add("$Comment", DbType.String).Value = "";
                cmd.Parameters.Add("$CreationDate", DbType.DateTime).Value = createDate;
                cmd.Parameters.Add("$LastPasswordChangedDate", DbType.DateTime).Value = createDate;
                cmd.Parameters.Add("$LastActivityDate", DbType.DateTime).Value = createDate;
                cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;
                cmd.Parameters.Add("$IsLockedOut", DbType.Boolean).Value = false;
                cmd.Parameters.Add("$LastLockedOutDate", DbType.DateTime).Value = createDate;
                cmd.Parameters.Add("$FailedPasswordAttemptCount", DbType.Int32).Value = 0;
                cmd.Parameters.Add("$FailedPasswordAttemptWindowStart", DbType.DateTime).Value = createDate;
                cmd.Parameters.Add("$FailedPasswordAnswerAttemptCount", DbType.Int32).Value = 0;
                cmd.Parameters.Add("$FailedPasswordAnswerAttemptWindowStart", DbType.DateTime).Value = createDate;

                try
                {
                    conn.Open();

                    int recAdded = cmd.ExecuteNonQuery();

                    if (recAdded > 0)
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex.InnerException);
                    status = MembershipCreateStatus.ProviderError;
                }
                finally
                {
                    conn.Close();
                }


                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }


        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT PKID, Username, Email, PasswordQuestion," +
                  " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                  " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate" +
                  " FROM `" + tableName + "` WHERE PKID = $PKID", conn);

            cmd.Parameters.Add("$PKID", DbType.String).Value = providerUserKey;

            MembershipUser u = null;
            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        SqliteCommand updateCmd = new SqliteCommand("UPDATE `" + tableName + "` " +
                                  "SET LastActivityDate = $LastActivityDate " +
                                  "WHERE PKID = $PKID", conn);

                        updateCmd.Parameters.Add("$LastActivityDate", DbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("$PKID", DbType.String).Value = providerUserKey;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        public override MembershipUser GetUser(string username,
                                                 bool userIsOnline)
        {

            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT PKID, Username, Email, PasswordQuestion," +
                                                  " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                                                  " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate" +
                                                  " FROM `" + tableName + "` WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            MembershipUser u = null;
            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        SqliteCommand updateCmd = new SqliteCommand("UPDATE `" + tableName + "` " +
                                  "SET LastActivityDate = $LastActivityDate " +
                                                                    "WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

                        updateCmd.Parameters.Add("$LastActivityDate", DbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("$Username", DbType.String).Value = username;
                        updateCmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        public override bool DeleteUser(string username,
                                         bool deleteAllRelatedData)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("DELETE FROM `" + tableName + "`" +
                                                  " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();

                if (deleteAllRelatedData)
                {
                    // Process commands to delete all data for the user in the database.
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }

        public override MembershipUserCollection FindUsersByEmail(
                                                                   string emailToMatch, int pageIndex, int pageSize,
                                                                   out int totalRecords)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Count(*) FROM `" + tableName + "` " +
                                              "WHERE Email LIKE $EmailSearch AND ApplicationName = $ApplicationName", conn);
            cmd.Parameters.Add("$EmailSearch", DbType.String).Value = emailToMatch;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = ApplicationName;

            MembershipUserCollection users = new MembershipUserCollection();

            SqliteDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = "SELECT PKID, Username, Email, PasswordQuestion," +
                         " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                         " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " +
                         " FROM `" + tableName + "` " +
                         " WHERE Email LIKE $Username AND ApplicationName = $ApplicationName " +
                         " ORDER BY Username Asc";

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return users;
        }

        public override MembershipUserCollection FindUsersByName(
                                                                   string usernameToMatch, int pageIndex, int pageSize,
                                                                   out int totalRecords)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Count(*) FROM `" + tableName + "` " +
                      "WHERE Username LIKE $UsernameSearch AND ApplicationName = $ApplicationName", conn);
            cmd.Parameters.Add("$UsernameSearch", DbType.String).Value = usernameToMatch;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            MembershipUserCollection users = new MembershipUserCollection();

            SqliteDataReader reader = null;
            totalRecords = 0;
            try
            {
                conn.Open();
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = "SELECT PKID, Username, Email, PasswordQuestion," +
                  " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                  " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " +
                  " FROM `" + tableName + "` " +
                  " WHERE Username LIKE $UsernameSearch AND ApplicationName = $ApplicationName " +
                  " ORDER BY Username Asc";

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return users;
        }

        public override MembershipUserCollection GetAllUsers(
                                                               int pageIndex, int pageSize, out int totalRecords)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Count(*) FROM `" + tableName + "` " +
                                              "WHERE ApplicationName = $ApplicationName", conn);
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = ApplicationName;

            MembershipUserCollection users = new MembershipUserCollection();

            SqliteDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = "SELECT PKID, Username, Email, PasswordQuestion," +
                         " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                         " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " +
                         " FROM `" + tableName + "` " +
                         " WHERE ApplicationName = $ApplicationName " +
                         " ORDER BY Username Asc";

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return users;
        }

        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Count(*) FROM `" + tableName + "`" +
                    " WHERE LastActivityDate > $LastActivityDate AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$CompareDate", DbType.DateTime).Value = compareTime;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            int numOnline = 0;

            try
            {
                conn.Open();

                numOnline = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                conn.Close();
            }

            return numOnline;
        }

        public override string GetPassword(string username,
                                             string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Password, PasswordAnswer, IsLockedOut FROM `" + tableName + "`" +
                  " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            string password = "";
            string passwordAnswer = "";
            SqliteDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(2))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    password = reader.GetString(0);
                    passwordAnswer = reader.GetString(1);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }


            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        public override string GetUserNameByEmail(string email)
        {
            return String.Empty;
        }

        public override string ResetPassword(string username,
        string answer)
        {
            return "rdfsd";
        }

        public override bool UnlockUser(string userName)
        {

            return true;
        }

        public override void UpdateUser(MembershipUser user)
        {

        }

        public override bool ValidateUser(string username,
                                            string password)
        {
            bool isValid = false;

            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT Password, IsApproved FROM `" + tableName + "`" +
                                                  " WHERE Username = $Username AND ApplicationName = $ApplicationName AND IsLockedOut = 0", conn);

            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            SqliteDataReader reader = null;
            bool isApproved = false;
            string pwd = "";

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();
                    pwd = reader.GetString(0);
                    int iApp = Convert.ToInt32(reader.GetValue(1));
                    if (iApp == 1) isApproved = true;
                }
                else
                {
                    return false;
                }

                reader.Close();

                if (CheckPassword(password, pwd))
                {
                    if (isApproved)
                    {
                        isValid = true;

                        SqliteCommand updateCmd = new SqliteCommand("UPDATE `" + tableName + "` SET LastLoginDate = $LastLoginDate" +
                                                                    " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

                        updateCmd.Parameters.Add("$LastLoginDate", DbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("$Username", DbType.String).Value = username;
                        updateCmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    conn.Close();

                    UpdateFailureCount(username, "password");
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
			if( isValid )
			{
				Molecule.Log.LogService logService = Molecule.Log.LogService.Instance ;
				logService.AddEvent(username + " successfully logged");
			}
            return isValid;
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return true;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 0;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 0;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return 0;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Clear;
            }

        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return "kjkkj";
            }

        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return false;
            }
        }

        // A helper function that takes the current row from the SQLiteDataReader
        // and hydrates a MembershipUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        private MembershipUser GetUserFromReader(SqliteDataReader reader)
        {
            if (reader.GetString(1) == "") return null;
            object providerUserKey = null;
            string strGooid = Guid.NewGuid().ToString();
            if (reader.GetValue(0).ToString().Length > 0)
                providerUserKey = new Guid(reader.GetValue(0).ToString());
            else
                providerUserKey = new Guid(strGooid);
            string username = reader.GetString(1);
            string email = reader.GetString(2);

            string passwordQuestion = "";
            if (reader.GetValue(3) != DBNull.Value)
                passwordQuestion = reader.GetString(3);

            string comment = "";
            if (reader.GetValue(4) != DBNull.Value)
                comment = reader.GetString(4);

            bool tmpApproved = (reader.GetValue(5) == null);
            bool isApproved = false;
            if (tmpApproved)
                isApproved = reader.GetBoolean(5);

            bool tmpLockedOut = (reader.GetValue(6) == null);
            bool isLockedOut = false;
            if (tmpLockedOut)
                isLockedOut = reader.GetBoolean(6);

            DateTime creationDate = DateTime.Now;
            try
            {
                if (reader.GetValue(6) != DBNull.Value)
                    creationDate = reader.GetDateTime(7);
            }
            catch { }

            DateTime lastLoginDate = DateTime.Now;
            try
            {
                if (reader.GetValue(8) != DBNull.Value)
                    lastLoginDate = reader.GetDateTime(8);
            }
            catch { }

            DateTime lastActivityDate = DateTime.Now;
            try
            {
                if (reader.GetValue(9) != DBNull.Value)
                    lastActivityDate = reader.GetDateTime(9);
            }
            catch { }
            DateTime lastPasswordChangedDate = DateTime.Now;
            try
            {
                if (reader.GetValue(10) != DBNull.Value)
                    lastPasswordChangedDate = reader.GetDateTime(10);
            }
            catch { }

            DateTime lastLockedOutDate = DateTime.Now;
            try
            {
                if (reader.GetValue(11) != DBNull.Value)
                    lastLockedOutDate = reader.GetDateTime(11);
            }
            catch { }

            MembershipUser u = new MembershipUser(this.Name,
                                                  username,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate);

            return u;
        }

        // Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        private string EncodePassword(string password)
        {
            if (password == null) password = "";
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(encryptionKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        // Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //   Compares password values based on the MembershipPasswordFormat.
        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        private void UpdateFailureCount(string username, string failureType)
        {
            SqliteConnection conn = new SqliteConnection(connectionString);
            SqliteCommand cmd = new SqliteCommand("SELECT FailedPasswordAttemptCount, " +
                                              "  FailedPasswordAttemptWindowStart, " +
                                              "  FailedPasswordAnswerAttemptCount, " +
                                              "  FailedPasswordAnswerAttemptWindowStart " +
                                              "  FROM `" + tableName + "` " +
                                              "  WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

            SqliteDataReader reader = null;
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (failureType == "password")
                    {
                        failureCount = reader.GetInt32(0);
                        try
                        {
                            windowStart = reader.GetDateTime(1);
                        }
                        catch
                        {
                            windowStart = DateTime.Now;
                        }
                    }

                    if (failureType == "passwordAnswer")
                    {
                        failureCount = reader.GetInt32(2);
                        windowStart = reader.GetDateTime(3);
                    }
                }

                reader.Close();

                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    if (failureType == "password")
                        cmd.CommandText = "UPDATE `" + tableName + "` " +
                                          "  SET FailedPasswordAttemptCount = $Count, " +
                                          "      FailedPasswordAttemptWindowStart = $WindowStart " +
                                          "  WHERE Username = $Username AND ApplicationName = $ApplicationName";

                    if (failureType == "passwordAnswer")
                        cmd.CommandText = "UPDATE `" + tableName + "` " +
                                          "  SET FailedPasswordAnswerAttemptCount = $Count, " +
                                          "      FailedPasswordAnswerAttemptWindowStart = $WindowStart " +
                                          "  WHERE Username = $Username AND ApplicationName = $ApplicationName";

                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("$Count", DbType.Int32).Value = 1;
                    cmd.Parameters.Add("$WindowStart", DbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("$Username", DbType.String).Value = username;
                    cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

                    if (cmd.ExecuteNonQuery() < 0)
                        throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        cmd.CommandText = "UPDATE `" + tableName + "` " +
                                          "  SET IsLockedOut = $IsLockedOut, LastLockedOutDate = $LastLockedOutDate " +
                                          "  WHERE Username = $Username AND ApplicationName = $ApplicationName";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("$IsLockedOut", DbType.Boolean).Value = true;
                        cmd.Parameters.Add("$LastLockedOutDate", DbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("$Username", DbType.String).Value = username;
                        cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                            cmd.CommandText = "UPDATE `" + tableName + "` " +
                                              "  SET FailedPasswordAttemptCount = $Count" +
                                              "  WHERE Username = $Username AND ApplicationName = $ApplicationName";

                        if (failureType == "passwordAnswer")
                            cmd.CommandText = "UPDATE `" + tableName + "` " +
                                              "  SET FailedPasswordAnswerAttemptCount = $Count" +
                                              "  WHERE Username = $Username AND ApplicationName = $ApplicationName";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("$Count", DbType.Int32).Value = failureCount;
                        cmd.Parameters.Add("$Username", DbType.String).Value = username;
                        cmd.Parameters.Add("$ApplicationName", DbType.String).Value = pApplicationName;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e.InnerException);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
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


        // Decrypts or leaves the password clear based on the PasswordFormat.
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
    }

}
