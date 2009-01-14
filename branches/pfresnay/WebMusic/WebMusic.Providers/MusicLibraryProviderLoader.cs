//
// MusicLibraryProviderLoader.cs
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
using Molecule.Runtime;

namespace WebMusic.Providers
{
    public static class MusicLibraryProviderLoader
    {

       
        public static IMusicLibraryProvider CreateInstance(string providerReference, string baseDirectory)
        {
            foreach(var plugin in Plugin<IMusicLibraryProvider>.List(baseDirectory))
			{
                if(plugin.Name == providerReference)
				{
                    return plugin.CreateInstance();
				}
			}
            throw new ApplicationException("Can't find provider " + providerReference + " in " + baseDirectory);
        }

        public static IEnumerable<MusicLibraryProviderInfo> GetProviders(string baseDirectory)
        {
            foreach(var plugin in Plugin<IMusicLibraryProvider>.List(baseDirectory))
                yield return new MusicLibraryProviderInfo()
                {
                    Id = plugin.Name,
                    Name = plugin.Name,
                    Description = plugin.Description
                };
        }
    }
}
