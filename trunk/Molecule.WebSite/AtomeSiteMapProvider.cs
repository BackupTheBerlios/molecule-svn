//
// AtomeSiteMapProvider.cs
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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using Molecule.WebSite.Services;
using log4net;

namespace Molecule.WebSite
{
    public class AtomeSiteMapProvider : StaticSiteMapProvider
    {
        static ILog log = LogManager.GetLogger(typeof(AtomeSiteMapProvider));
        SiteMapNode rootNode;
        string rootTitle;
        string rootUrl;
        string rootDescription;

        public override void Initialize(string name, NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
            rootTitle = attributes.Get("title") ?? "defaultTitle";
            rootUrl = attributes.Get("url");
            rootDescription = attributes.Get("description");

            if (log.IsDebugEnabled)
                log.Debug("Initialized (root title = " + rootTitle + ")");
        }

        public override SiteMapNode BuildSiteMap()
        {
            if (rootNode == null)
                lock (this)
                {
                    if (rootNode == null)
                    {
                        if (log.IsDebugEnabled)
                            log.Debug("Dynamically filling site map with atome site maps...");
                        rootNode = BuildRootNode();
                        int i = 0;
                        var homeNode = new SiteMapNode(this, "home", "/", "Accueil");
                        AddNode(homeNode, rootNode);
                        foreach (AtomeInfo atome in AtomeService.GetAtomes())
                        {
                            var node = new SiteMapNode(this, "atome" + i++, atome.Path, atome.Name);
                            AddNode(node, rootNode);
                        }
                        var prefNode = new SiteMapNode(this, "preferences", "admin", "Préférences");
                        AddNode(prefNode, rootNode);
                    }
                }
            return rootNode;
        }

        protected virtual SiteMapNode BuildRootNode()
        {
            return new SiteMapNode(this, rootTitle, rootUrl, rootTitle, rootDescription);
        }


        public override SiteMapNode RootNode
        {
            get
            {
                return BuildSiteMap();
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }
    }
}
