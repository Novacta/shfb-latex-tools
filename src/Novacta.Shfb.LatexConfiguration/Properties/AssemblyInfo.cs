using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

[assembly: NeutralResourcesLanguage("en")]

internal static class AssemblyInfo
{
    public const string Company = "Novacta";

    public const string ProductVersion = "1.0.0";

    public const string Description =
        "Provides support for adding LaTeX formatted formulas in " +
        "reference XML comments and conceptual content topics created with " +
        "Sandcastle Help File Builder.";

    public const string Copyright = 
        "Copyright \xA9 2018, Giovanni Lafratta, All Rights Reserved.";
}
