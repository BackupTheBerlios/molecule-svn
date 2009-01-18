//
// Plugin.cs
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
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using log4net;

namespace Molecule.Runtime
{
    public class Plugin<T>
    {
        static ILog log = LogManager.GetLogger("Molecule.Runtime.Plugin");

        public Type Type { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Plugin(Type type)
            : this(type, true, null)
        {

        }

        protected Plugin(Type type, bool check, PluginAttribute attr)
        {
            Type = type;
            var pluginType = typeof(T);

            if (check)
            {
                if (!(pluginType.IsClass || pluginType.IsInterface))
                    throw new ArgumentException("Parameterized type must be a class or interface.", "<T>");

                if (! IsSubclassOfOrImplements(type, pluginType))
                    throw new ArgumentException("Type " + type + " must be a subclass of " + pluginType, "type");
            }

            if(attr == null)
                attr = (PluginAttribute)type.GetCustomAttributes(typeof(PluginAttribute), false).FirstOrDefault();
            
            if (attr == null)
            {
                Name = type.Name;
                Description = "";
            }
            else
            {
                Name = attr.Name;
                Description = attr.Description;
            }
        }

        public Plugin(string assemblyFile, string typeName)
            : this(getTypeFromAssembly(assemblyFile, typeName))
        {
            
        }

        private static Type getTypeFromAssembly(string assemblyFile, string typeName)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            if (assembly.GetCustomAttributes(typeof(PluginContainerAttribute), false).Length == 0)
                throw new ApplicationException("Assembly " + assemblyFile + " is not declared as a plugin container.");
            return assembly.GetType(typeName, true);
        }

        public T CreateInstance()
        {
            return (T)Activator.CreateInstance(Type);
        }

        public static IEnumerable<Plugin<T>> List(string baseDirectory)
        {
            if (log.IsInfoEnabled)
                log.Info("Search for plugin of type " + typeof(T).Name + " in directory " + baseDirectory);

            string pluginContainerAttributeName = typeof(PluginContainerAttribute).Name;

            foreach(string file in Directory.GetFiles(baseDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                string fileName = Path.GetFileName(file);
                
                if(log.IsDebugEnabled)
                    log.Debug("Scan "+file);
                if (log.IsDebugEnabled)
                    log.Debug("file size : " + (new FileInfo(file).Length));
                if(log.IsDebugEnabled)
                    log.Debug("file date : " + (new FileInfo(file).CreationTime));

                Assembly assembly;
                Type[] assemblyTypes;
                try
                {
                    assembly = Assembly.LoadFrom(file);
                    //check if assembly is declared as a plugin container. 
                    if (!assembly.GetCustomAttributes(false)
                        .Any(attr => attr.GetType().Name == pluginContainerAttributeName))
                        continue;
                    assemblyTypes = assembly.GetTypes();
                }
                catch(Exception e)
                {
                    if (log.IsDebugEnabled)
                        log.Debug("An error occured while trying to read " + fileName + ", skip it.",e);
                    continue;
                }

                //check for plugin class types
                foreach (Type type in assemblyTypes)
                {
                    if(log.IsDebugEnabled)
                        log.Debug("found type : " + type.Name);
                    if (!type.IsClass)
                        continue;

                    var attr = (PluginAttribute)type.GetCustomAttributes(typeof(PluginAttribute), false)
                        .FirstOrDefault();

                    if (attr == null)
                        continue;

                    if (log.IsDebugEnabled)
                        log.Debug("Check plugin " + type.Name + " in assembly " + fileName);

                    //check if plugin implements or derivate from parameterized base type
                    var pluginType = typeof(T);
                    if (IsSubclassOfOrImplements(type, pluginType))
                    {
                        if (log.IsInfoEnabled)
                            log.Info("Found plugin " + type.Name + " of type " + pluginType.Name + " in assembly " + fileName
                                + ". Check if it can be used.");

                        //check if plugin is usable (IsUsablePlugin static boolean property)
                        var isUsable = type.GetProperties(BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public)
                                       .Any(prop => prop.PropertyType == typeof(Boolean)
                                        && prop.GetCustomAttributes(typeof(IsUsablePluginAttribute), false).Length > 0
                                        && (bool)prop.GetValue(null, null));
                            
                        if(isUsable)
                            yield return new Plugin<T>(type, false, attr);
                    }
                }    
            }
        }

        private static bool IsSubclassOfOrImplements(Type type, Type baseType)
        {
            return  type == baseType
                    || baseType.IsInterface && type.GetInterface(baseType.Name) != null
                    || baseType.IsSubclassOf(baseType);
        }
    }
}
