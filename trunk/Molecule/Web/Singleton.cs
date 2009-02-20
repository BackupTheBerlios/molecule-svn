//
// Singleton.cs
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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;

namespace Molecule.Web
{
    public class Singleton<T> where T : class
    {
        private static volatile T __instance = null;

        static object instanceLock = new object();

        public static T Instance
        {
            get
            {
                if (__instance != null)
                    return __instance;

                if(HttpContext.Current != null)
                    __instance = (T)HttpContext.Current.Application[TypeId];

                if (__instance == null)
                {
                    lock (instanceLock)
                    {
                        if (__instance == null)
                        {
                            __instance = createInstance();
                            if(HttpContext.Current != null)
                                HttpContext.Current.Application[TypeId] = __instance;
                        }
                    }
                }
                return __instance;
            }
        }

        private static T createInstance()
        {
            try
            {
                return (T)typeof(T).InvokeMember(typeof(T).Name,
                                      BindingFlags.CreateInstance |
                                      BindingFlags.Instance |
                                      BindingFlags.NonPublic,
                                      null, null, null);
            }
            catch (TargetInvocationException tiex)
            {//http://www.dotnetjunkies.com/WebLog/chris.taylor/archive/2004/03/03/8353.aspx
                // NB: Error checking etc. excluded
                // Get the _remoteStackTraceString of the Exception class
                FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString",
                BindingFlags.Instance | BindingFlags.NonPublic);

                // Set the InnerException._remoteStackTraceString to the current InnerException.StackTrace
                remoteStackTraceString.SetValue(tiex.InnerException,
                tiex.InnerException.StackTrace + Environment.NewLine);

                // Throw the new exception
                throw tiex.InnerException;

            }
        }

        private static string TypeId
        {
            get
            {
                return "Singleton_" + typeof(T).FullName;
            }
        }
    }
}
