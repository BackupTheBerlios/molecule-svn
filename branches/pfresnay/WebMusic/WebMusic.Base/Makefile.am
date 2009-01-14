ASSEMBLY = WebMusic.Base.dll
TARGET = library
SOURCES =  \
	$(srcdir)/CompressionHelper.cs \
	$(srcdir)/XdgBaseDirectorySpec.cs \
	$(srcdir)/AssemblyInfo.cs

ASSEMBLIES = 		\
	-r:ICSharpCode.SharpZipLib.dll \
	-r:$(top_builddir)/WebMusic/WebMusic.Providers/WebMusic.Providers.dll \
	-r:$(top_srcdir)/references/log4net.dll

moleculedir=$(pkglibdir)

molecule_DATA = 			\
	$(ASSEMBLY)

EXTRA_DIST = \
	$(SOURCES)

$(ASSEMBLY): $(SOURCES) $(top_builddir)/WebMusic/WebMusic.Providers/WebMusic.Providers.dll
	$(CSC) -out:$@ -target:$(TARGET) $(SOURCES) $(ASSEMBLIES)

CLEANFILES = \
	$(ASSEMBLY)

DISTCLEANFILES =                        \
	$(ASSEMBLY)
