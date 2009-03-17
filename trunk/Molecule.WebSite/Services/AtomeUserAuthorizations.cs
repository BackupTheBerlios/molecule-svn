using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Mono.Rocks;

namespace Molecule.WebSite.Services
{
    public interface IAtomeUserAuthorizations
    {
        IEnumerable<string> Users { get; }
        IEnumerable<string> Atomes { get; }
        bool Get(string atome, string user);
        IEnumerable<bool> GetByAtome(string atome);
        IEnumerable<Tuple<string, IEnumerable<bool>>> GetAll();
    }

    [Serializable]
    public class AtomeUserAuthorizations : IAtomeUserAuthorizations
    {
        public AtomeUserAuthorizations()
        {
        }

        public AtomeUserAuthorizations(IEnumerable<string> atomes, IEnumerable<string> users)
        {
            Atomes = atomes.ToArray();
            Users = users.ToArray();
            Authorizations = new bool[Atomes.Length][];
            for(int i = 0; i< Authorizations.Length; i++)
                Authorizations[i] = new bool[Users.Length];
        }

        public string[] Atomes { get; set; }
        public string[] Users { get; set; }
        public bool[][] Authorizations { get; set; }

        public bool Get(string atome, string user)
        {
            int i = Atomes.IndexOf(atome);
            int j = Users.IndexOf(user);
            return Authorizations[i][j];
        }

        public IEnumerable<Tuple<string, IEnumerable<bool>>> GetAll()
        {
            return from atome in Atomes
                   select new Tuple<string, IEnumerable<bool>>(atome, GetByAtome(atome));
        }

        public IEnumerable<bool> GetByAtome(string atome)
        {
            return Authorizations[Atomes.IndexOf(atome)];
        }

        public bool GetOrDefault(string atome, string user)
        {
            if (Atomes.Contains(atome) && Users.Contains(user))
                return Get(atome, user);
            else
                return false;
        }

        public void Set(string atome, string user, bool value)
        {
            int i = Atomes.IndexOf(atome);
            int j = Users.IndexOf(user);
            Authorizations[i][j] = value;
        }

        #region IAtomeUserAuthorizations Members

        IEnumerable<string> IAtomeUserAuthorizations.Users
        {
            get { return Users; }
        }

        IEnumerable<string> IAtomeUserAuthorizations.Atomes
        {
            get { return Atomes; }
        }

        #endregion
    }
}
