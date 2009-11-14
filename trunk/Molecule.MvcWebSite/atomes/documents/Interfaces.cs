using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Atomes.Documents
{
    public interface IGenericInfo
    {
        string Id { get; }
        string Name { get; }
    }

    public interface IFolderInfo : IGenericInfo
    {

        IEnumerable<IFolderInfo> GetFolders();

        IEnumerable<IDocumentInfo> GetDocuments();
    }

    public interface IDocumentInfo : IGenericInfo
    {
        
    }
}
