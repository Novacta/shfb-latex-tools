// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General assembly information
[assembly: AssemblyCopyright(AssemblyInfo.Copyright)]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// Resources contained within the assembly are English
[assembly: NeutralResourcesLanguage("en")]

[assembly: AssemblyVersion(AssemblyInfo.ProductVersion)]
[assembly: AssemblyFileVersion(AssemblyInfo.ProductVersion)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.ProductVersion)]

// This defines constants that can be used here and in the custom presentation style export attribute
internal static partial class AssemblyInfo
{
    public const string ProductVersion = "3.0.0";

    public const string Description = 
        "Provides support for LaTeX formatted formulas " +
            "in XML comments and conceptual content topics.";
    
    public const string Copyright = "Copyright \xA9 2023, Giovanni Lafratta, All Rights Reserved.";
}
