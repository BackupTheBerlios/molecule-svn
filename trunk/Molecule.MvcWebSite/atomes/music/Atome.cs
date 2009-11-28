using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite.atomes.music.Controllers;
using Molecule.Web;
using Molecule.Webdav;

namespace Molecule.MvcWebSite.atomes.music
{
    public class Atome : IAtome
    {
        public const string Id = "music";

        #region IAtome Members


        public Atome()
        {
            Singleton<VirtualWebdavFolderService>.Instance.RegisterVirtualWebdavFolder(new VirtualWebdavFolder());
        }


        public Type PreferencesController
        {
            get { return typeof(AdminController); }
        }

        #endregion

        #region IAtome Members

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
