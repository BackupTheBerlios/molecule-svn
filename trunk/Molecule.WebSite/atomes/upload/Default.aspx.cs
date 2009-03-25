// Default.aspx.cs
//
// Copyright (c) 2009 Pascal Fresnay (pascalfresnay@free.fr) - Mickael Renault (mickaelrenault@free.fr) 
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
//
//

using System;
using System.Web;
using System.IO;
using System.Threading;
using System.Web.UI;
using Molecule.IO;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;

namespace Upload
{
	public partial class Default : System.Web.UI.Page
	{
		private static log4net.ILog log = log4net.LogManager.GetLogger( typeof( Default ) );
		
        protected void Page_Load(object sender, EventArgs e)
        {
		}

		protected void submitButton_Click(Object sender, EventArgs e)
		{
            string downloadPath = Upload.UploadService.DestinationPath;

			foreach(UploadedFile uploadedFile in this.multiUploadFile.Files)
            {
			    if (IsValid)
				{
                    string fileDestination = Path.Combine(downloadPath+Path.DirectorySeparatorChar, uploadedFile.FileName);
					uploadedFile.MoveTo(fileDestination,MoveToOptions.Overwrite);
                    if (log.IsInfoEnabled)
                    {
                        log.InfoFormat("File {0} saved to {1}", uploadedFile.FileName, fileDestination);
                    }
				}
			}
		}
	}
}
	