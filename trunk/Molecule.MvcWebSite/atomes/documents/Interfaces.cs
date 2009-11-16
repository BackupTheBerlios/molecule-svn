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
        string Path { get; }
    }

    public interface IFolderInfo : IGenericInfo
    {

        
    }

    public interface IDocumentInfo : IGenericInfo
    {
        
    }
}
