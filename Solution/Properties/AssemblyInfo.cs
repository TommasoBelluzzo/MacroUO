#region Using Directives
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
#endregion

#region Information
[assembly: AssemblyDescription("A simple macroing tool for Ultima Online.")]
[assembly: AssemblyCompany("Tommaso Belluzzo")]

#if (DEBUG)
[assembly: AssemblyConfiguration("Debug Build")]
#else
[assembly: AssemblyConfiguration("Release Build")]
#endif

[assembly: AssemblyCopyright("Copyright ©2010-2017 Tommaso Belluzzo")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyProduct("MacroUO")]
[assembly: AssemblyTitle("MacroUO")]
[assembly: AssemblyTrademark("")]
#endregion

#region Settings
[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]
[assembly: Guid("D81EE201-0759-4892-89A5-43FC7286BE68")]
[assembly: NeutralResourcesLanguageAttribute("en")]
#endregion

#region Version
[assembly: AssemblyFileVersion("3.0.0.0")]
[assembly: AssemblyInformationalVersion("3.0.0.0")]
[assembly: AssemblyVersion("3.0.0.0")]
#endregion