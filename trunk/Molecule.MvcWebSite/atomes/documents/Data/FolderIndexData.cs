using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Atomes.Documents;

namespace Molecule.Atomes.Documents.Data
{
    public class FolderIndexData
    {
        public IFolderInfo CurrentFolder { get; set; }
        public IEnumerable<IFolderInfo> Folders { get; set; }
        public IEnumerable<IDocumentInfo> Documents { get; set; }
    }
}
