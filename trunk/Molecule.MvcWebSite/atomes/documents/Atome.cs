using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Molecule.MvcWebSite;


namespace Molecule.Atomes.Documents
{
    /// <summary>
    /// Represent an atome. Used to retreive atome information. Will be instanciated once by molecule.
    /// Full class name must be referenced in Atome.xml.
    /// </summary>
    public class Atome : IAtome
    {
       
        /// <summary>
        /// Gets Preferences controller that enable user to customize atome behaviors.
        /// </summary>
        public Type PreferencesController
        {
            get { return typeof(Controllers.PreferencesController); }
        }

        /// <summary>
        /// Gets default controller used when no specific controller are requested by user.
        /// </summary>
        public Type DefaultController
        {
            get { return typeof(Controllers.FolderController); }
        }

        /// <summary>
        /// True if atome is only used by administrator. Atome authorizations can't be modified in Preferences page.
        /// Most atome should return false.
        /// </summary>
        public bool AdminOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Displayed atome name. Can be a localized ressource.
        /// </summary>
        public string Name
        {
            get { return "Documents"; }
        }
    }
}
