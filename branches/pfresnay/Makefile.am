REFERENCES = \
	references/log4net.dll \
	references/NDesk.Options.dll \
	references/System.Web.Extensions.dll \
	references/AjaxControlToolkit.dll


SUBDIRS = \
	Molecule \
	Molecule.Console \
	WebMusic

EXTRA_DIST = \
	$(REFERENCES) \
	build/m4/expansions.m4

referencesdir=$(pkglibdir)

references_DATA = \
	$(REFERENCES)

CLEANFILES = \
		Makefile.in

DISTCLEANFILES =  \
	Makefile.in


MAINTAINERCLEANFILES = Makefile.in
