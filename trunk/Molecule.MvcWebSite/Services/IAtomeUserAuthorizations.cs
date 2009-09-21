using System;
using System.Collections.Generic;
namespace Molecule.WebSite.Services
{
    public interface IAtomeUserAuthorizations : IEnumerable<AtomeUserAuthorizationItem>
    {
        AtomeUserAuthorization Get(string atome, string user);
        void Set(string atome, string user, bool value);
        AtomeUserAuthorization TryGet(string atome, string user);
        bool IsAuthorized(string atome, string user);
    }
}
