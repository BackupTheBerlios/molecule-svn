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
using System.IO;
using Molecule.Collections;
using Molecule.Metadata;
using System.Collections.Generic;
using Molecule.Security;



namespace WebPhoto.Providers.FS
{
    public class Photo : IPhoto
    {
        FileInfo file;
        ITag parent;
        JpegMetadataReader metaReader;

        public Photo(FileInfo file, ITag parent)
        {
            this.file = file;
            this.parent = parent;
            this.id = CryptoUtil.Md5Encode(file.FullName);
            metaReader = JpegMetadataReader.RetreiveFromFile(file.FullName);
        }

        #region IPhoto Members

        public IEnumerable<ITag> Tags
        {
            get { yield return parent; }
        }

        #endregion

        #region IPhotoInfo Members

        string id;

        public string Id
        {
            get { return id; }
        }

        public string MediaFilePath
        {
            get { return file.FullName; }
        }

        public IKeyedEnumerable<string, string> Metadatas
        {
            get { return metaReader.CommonMetadatas; }
        }

        public DateTime Date
        {
            get { return file.CreationTime; }
        }

        public string Description
        {
            get { return ""; }
        }

        public double? Latitude
        {
            get { return metaReader.ContainsGPSInformation ? (double?)metaReader.Latitude : null; }
        }

        public double? Longitude
        {
            get { return metaReader.ContainsGPSInformation ? (double?)metaReader.Longitude : null; }
        }

        #endregion
    }

}
