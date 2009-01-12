ASSEMBLY = WebMusic.Providers.dll
TARGET = library
SOURCES =  \
	$(srcdir)/IArtist.cs \
	$(srcdir)/ISong.cs \
	$(srcdir)/MusicLibraryProviderInfo.cs \
	$(srcdir)/IAlbum.cs \
	$(srcdir)/IMusicLibraryProvider.cs \
	$(srcdir)/MusicLibraryProviderLoader.cs \
	$(srcdir)/Properties/AssemblyInfo.cs \
	$(srcdir)/Base/Album.cs \
	$(srcdir)/Base/Artist.cs \
	$(srcdir)/Base/Song.cs


ASSEMBLIES = 		\
	-r:ICSharpCode.SharpZipLib.dll \
	-r:$(top_builddir)/Molecule/Molecule.dll \
	-r:$(top_srcdir)/references/log4net.dll

moleculedir=$(pkglibdir)

molecule_DATA = \
	$(ASSEMBLY)

EXTRA_DIST = \
	$(SOURCES)

$(ASSEMBLY): $(SOURCES) $(top_builddir)/Molecule/Molecule.dll
	$(CSC) -out:$@ -target:$(TARGET) $(SOURCES) $(ASSEMBLIES)

CLEANFILES = \
	$(ASSEMBLY) \
	Makefile.in

DISTCLEANFILES =  \
	$(ASSEMBLY) \
	Makefile.in


MAINTAINERCLEANFILES = Makefile.in



