using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using Molecule.Web.Security;


namespace Molecule.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();

            if (!Services.HasUsers)
            {
                createUser(null, null, true);
                return;
            }

            bool showHelp = args.Length == 0;


            var options = new OptionSet(){
                {"h|?|help","show this message.",v => showHelp = true},
                {"l|list-users", "list all users.", v => printUsers()},
                {"au|add-user=", "add a new user.", (user,password) => createUser(user, password, false)},
                {"aa|add-admin=", "add a new user with administration rights.", (user,password) => createUser(user, password, true)}
            };

            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                System.Console.WriteLine(e.Message);
            }
            

            if (showHelp)
                options.WriteOptionDescriptions(System.Console.Out);

        }

        private static void createUser(string username, string password, bool isAdmin)
        {
            if (String.IsNullOrEmpty(username))
            {
                username = Environment.UserName;
                System.Console.Write("Enter username (" + username + "): ");
                string inputUsername = System.Console.ReadLine();
                if (!String.IsNullOrEmpty(inputUsername))
                    username = inputUsername;
            }

            while (String.IsNullOrEmpty(password))
            {
                System.Console.Write("Enter password for user " + username + ": ");
                password = System.Console.ReadLine();
                if (String.IsNullOrEmpty(password))
                    System.Console.Write("Password can't be empty !");
            }

            Services.CreateUser(username, password, isAdmin);
        }

        private static void printUsers()
        {
            foreach (var user in Services.GetAllUsers())
            {
                System.Console.WriteLine(user);
            }
        }
    }
}
