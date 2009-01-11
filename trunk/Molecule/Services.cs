//
// Service.cs
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
using System.Collections.Generic;
using System.Text;
using Molecule.Web.Security;
using System.Web.Security;

namespace Molecule
{
    public static class Services
    {

        static SQLiteMembershipProvider mprovider = new SQLiteMembershipProvider();

        static Services()
        {

        }

        public static bool HasUsers
        {
            get
            {
                int nbUsers;
                mprovider.GetAllUsers(0, 1, out nbUsers);
                return nbUsers > 0;
            }
        }
        public static void CreateUser(string login, string password, bool isAdmin)
        {
            MembershipCreateStatus status;
            Membership.CreateUser(login, password, "", null, null, true, null, out status);
            if (isAdmin)
            {
                Roles.AddUsersToRoles(new string[] { login },
                    new string[] { SQLiteProvidersHelper.AdminRoleName });
            }
        }

        public static IEnumerable<string> GetAllUsers()
        {
            int nbUsers;
            foreach (var user in mprovider.GetAllUsers(0, Int32.MaxValue, out nbUsers))
                yield return user.ToString();
        }
    }
}
