ASSEMBLY = Molecule.Console.exe
TARGET = exe
SOURCES =  \
	$(srcdir)/Program.cs \
	$(srcdir)/Properties/AssemblyInfo.cs

WRAPPER = molecule-admin

ASSEMBLIES = \
	-r:$(top_srcdir)/references/NDesk.Options.dll \
	-r:$(top_builddir)/Molecule/Molecule.dll \
	-r:$(top_srcdir)/references/log4net.dll

moleculedir=$(pkglibdir)

EXTRAS = molecule-admin.in

molecule_DATA = \
	$(ASSEMBLY)

EXTRA_DIST = \
	$(SOURCES) \
	$(EXTRAS)

$(ASSEMBLY): $(SOURCES)
	$(CSC) -out:$@ -target:$(TARGET) $(SOURCES) $(ASSEMBLIES)

bin_SCRIPTS = $(WRAPPER)

CLEANFILES = \
	$(ASSEMBLY) \
	Makefile.in

DISTCLEANFILES = \
	$(ASSEMBLY) \
	Makefile.in

MAINTAINERCLEANFILES = Makefile.in
