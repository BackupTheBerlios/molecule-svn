using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Molecule.Atomes.Documents.Data
{
    public class FolderData
    {
        public IFolderInfo CurrentFolder { get; set; }
    }

    public class FolderDisplayData : FolderData
    {
        public IEnumerable<IFolderInfo> Folders { get; set; }
        public IEnumerable<IDocumentInfo> Documents { get; set; }
        public IEnumerable<IFolderInfo> CurrentFolderHierarchy { get; set; }
    }

    public class FolderAddDocumentData : FolderData
    {

    }

    public class FolderCreateData : FolderData
    {

    }
}
