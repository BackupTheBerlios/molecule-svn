AC_INIT([molecule],[0.1])
AM_INIT_AUTOMAKE([1.9 tar-ustar])
AC_PATH_PROG(CSC, gmcs, no)

m4_include(build/m4/expansions.m4)

if test "x$CSC" = "x" ; then
  AC_MSG_ERROR([Can't find "gmcs" in your PATH])
fi

SHAMROCK_EXPAND_LIBDIR
SHAMROCK_EXPAND_BINDIR
SHAMROCK_EXPAND_DATADIR

PKG_CHECK_MODULES(SQLITE, sqlite3 >= 3.4)

AC_CONFIG_FILES([
Makefile
Molecule/Makefile
Molecule.Console/Makefile
Molecule.Console/molecule-admin
WebMusic/Makefile
WebMusic/WebMusic.Providers/Makefile
WebMusic/WebMusic.Base/Makefile
WebMusic/WebMusic.Providers.Banshee/Makefile
])
AC_OUTPUT
