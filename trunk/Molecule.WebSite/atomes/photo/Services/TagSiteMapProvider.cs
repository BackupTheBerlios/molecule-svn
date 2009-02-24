using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using log4net;
using WebPhoto.Providers;
using System.Linq;

namespace WebPhoto.Services
{
    /// <summary>
    /// Summary description for TagSiteMapSource
    /// </summary>
    public class TagSiteMapProvider : StaticSiteMapProvider
    {

        static ILog log = LogManager.GetLogger(typeof(TagSiteMapProvider));
        public TagSiteMapProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        SiteMapNode root;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
        {
            if (log.IsInfoEnabled)
                log.Info("Initializing...");
            base.Initialize(name, attributes);
            if (root == null)
            {
                init();
            }
            if (log.IsInfoEnabled)
                log.Info("Initialization done.");
        }

        object _lock = new object();

        public void Reload()
        {
            lock (_lock)
            {
                base.Clear();
                init();
            }
        }

        private void init()
        {
            root = new SiteMapNode(this, "rootTags", null, "Tags");
            AddNode(root);
            foreach (ITag t in PhotoLibrary.GetRootTags())
            {
                SiteMapNode node = createTag(t);
                AddNode(node, RootNode);
            }
        }

        public static string GetUrlFor(string tagId)
        {
            return "Tag.aspx?id=" + System.Web.HttpUtility.UrlEncode(tagId);
        }

        private SiteMapNode createTag(ITag tag)
        {
            string navigateUrl = tag.Photos.Any() ? GetUrlFor(tag.Id) : "";

            SiteMapNode node = new SiteMapNode(this, tag.Id, navigateUrl, tag.Name);
 
            foreach (ITag t in tag.ChildTags)
            {
                SiteMapNode childNode = createTag(t);
                AddNode(childNode, node);
            }
            return node;
        }

        public override SiteMapNode BuildSiteMap()
        {
            return root;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return root;
        }


    }

}