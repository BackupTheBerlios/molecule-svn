using System;
namespace Molecule.WebSite.Services
{
    public interface IAtomeUserAuthorizations
    {
        AtomeUserAuthorization Get(string atome, string user);
        void Set(string atome, string user, bool value);
        AtomeUserAuthorization TryGet(string atome, string user);
        bool IsAuthorized(string atome, string user);
    }
}
