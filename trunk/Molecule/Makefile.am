ASSEMBLY = Molecule.dll
TARGET = library
SOURCES =  \
	$(srcdir)/DateTimeUtil.cs \
	$(srcdir)/Services.cs \
	$(srcdir)/Web/Singleton.cs \
	$(srcdir)/Web/Security/SQLiteMembershipProvider.cs \
	$(srcdir)/Web/Security/SQLiteProvidersHelper.cs \
	$(srcdir)/Web/Security/SQLiteRoleProvider.cs \
	$(srcdir)/Runtime/PluginAttribute.cs \
	$(srcdir)/Runtime/PluginContainer.cs \
	$(srcdir)/Runtime/Plugin.cs \
	$(srcdir)/Serialization/Atome.cs \
	$(srcdir)/Configuration/ConfigurationClient.cs \
	$(srcdir)/Configuration/IConfigurationClient.cs \
	$(srcdir)/Configuration/MemoryConfigurationClient.cs \
	$(srcdir)/Configuration/SchemaEntry.cs \
	$(srcdir)/Configuration/XmlConfigurationClient.cs \
	$(srcdir)/Security/CryptoUtil.cs \
	$(srcdir)/Properties/AssemblyInfo.cs


ASSEMBLIES = 		\
	-r:System.Web.dll \
	-r:$(top_srcdir)/references/log4net.dll \
	-r:System.Data.dll \
	-r:Mono.Data.Sqlite.dll \
	-r:System.Configuration.dll \
	-r:System.Xml.Linq.dll

moleculedir=$(pkglibdir)

molecule_DATA = 			\
	$(ASSEMBLY)

EXTRA_DIST = \
	$(SOURCES)

$(ASSEMBLY): $(SOURCES)
	$(CSC) -out:$@ -target:$(TARGET) $(SOURCES) $(ASSEMBLIES)

CLEANFILES = \
	$(ASSEMBLY) \
	Makefile.in

DISTCLEANFILES =                        \
	$(ASSEMBLY) \
	Makefile.in

MAINTAINERCLEANFILES = Makefile.in

