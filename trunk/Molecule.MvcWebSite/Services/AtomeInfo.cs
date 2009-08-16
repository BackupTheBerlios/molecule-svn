﻿//
// AtomeInfo.cs
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
using Molecule.MvcWebSite;

namespace Molecule.WebSite.Services
{
    internal class AtomeInfo : IAtomeInfo
    {
        Molecule.Serialization.Atome atome;
        string atomePath;
        IAtome atomeInstance;

        internal AtomeInfo(Molecule.Serialization.Atome atome, string atomePath)
        {
            this.atome = atome;
            this.atomePath = atomePath;
            if(atome.ClassName != null)
                atomeInstance = (IAtome)Activator.CreateInstance(Type.GetType(atome.ClassName));
        }
        public string Path { get { return atomePath; } }
        public string Name { get { return atome.Name; } }
        public string ClassName { get { return atome.ClassName; } }

        private string mapAtomeRelativePath(string relativePath)
        {
            if (String.IsNullOrEmpty(relativePath))
                return null;
            return System.IO.Path.Combine(atomePath, relativePath);
        }
        public string PreferencesControllerName
        {
            get
            {
                return atomeInstance.PreferencesController != null ?
                    atomeInstance.PreferencesController.Name.Replace("Controller", "") : null;
            }
        }

        public bool HasPreferences
        {
            get { return atomeInstance.PreferencesController != null; }
        }

        public override bool Equals(object obj)
        {
            AtomeInfo tobj = obj as AtomeInfo;
            return tobj != null && tobj.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #region IAtome Members

        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            if(atomeInstance != null)
                atomeInstance.RegisterRoutes(routes);
        }

        #endregion
    }
}
