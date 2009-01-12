include $(top_srcdir)/Makefile.include

ASSEMBLY = WebMusic.Providers.Banshee.dll
TARGET = library
SOURCES =  \
	$(srcdir)/BansheeProvider.cs \
	$(srcdir)/AssemblyInfo.cs


ASSEMBLIES = 		\
	-r:$(top_srcdir)/references/log4net.dll \
	-r:$(top_builddir)/WebMusic/WebMusic.Providers/WebMusic.Providers.dll \
	-r:$(top_builddir)/WebMusic/WebMusic.Base/WebMusic.Base.dll \
	-r:$(LINK_MOLECULE_LIB) \
	-r:Mono.Data.Sqlite.dll \
	-r:System.Data.dll

moleculedir=$(pkglibdir)/website/atomes/webmusic/bin/providers/

molecule_DATA = \
	$(ASSEMBLY)

EXTRA_DIST = \
	$(SOURCES)

$(ASSEMBLY): $(SOURCES) $(LINK_MOLECULE_LIB) $(top_builddir)/WebMusic/WebMusic.Base/WebMusic.Base.dll  $(top_builddir)/WebMusic/WebMusic.Providers/WebMusic.Providers.dll
	$(CSC) -out:$@ -target:$(TARGET) $(SOURCES) $(ASSEMBLIES)

CLEANFILES = \
	$(ASSEMBLY)

DISTCLEANFILES =  \
	$(ASSEMBLY)

MAINTAINERCLEANFILES = Makefile.in
