//
// Global.asax.cs
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
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.IO;
using log4net;
using WebMusic.Services;

namespace Molecule.WebSite
{


    public class Global : System.Web.HttpApplication
    {
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            log4net.GlobalContext.Properties["currentuser"] = Environment.UserName;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(HttpContext.Current.Request.PhysicalApplicationPath + "Log4Net.config"));
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        ILog log = LogManager.GetLogger(typeof(Global));

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            log.Error("Unhandled exception", Context.Error);
            Application["Exception"] = Context.Error;
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
        }
    }
}
