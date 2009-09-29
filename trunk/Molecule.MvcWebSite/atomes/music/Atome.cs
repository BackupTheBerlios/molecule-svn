using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite.atomes.music.Controllers;

namespace Molecule.MvcWebSite.atomes.music
{
    public class Atome : IAtome
    {
        public const string Id = "music";

        #region IAtome Members


        public Type PreferencesController
        {
            get { return typeof(AdminController); }
        }

        #endregion

        #region IAtome Members


        public IEnumerable<string> ControllerNamespaces
        {
            get 
            {
                yield return "Molecule.MvcWebSite.atomes.music.Controllers";
            }
        }

        #endregion


        public Type DefaultController
        {
            get { return typeof(PlayerController); }
        }


        public bool AdminOnly
        {
            get { return false; }
        }

        public string Name
        {
            get { return Resources.webmusic.Music; }
        }
    }
}
