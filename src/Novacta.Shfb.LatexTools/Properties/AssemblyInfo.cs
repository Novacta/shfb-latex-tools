// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

[assembly: NeutralResourcesLanguage("en")]

[assembly: AssemblyVersion(AssemblyInfo.ProductVersion)]

internal static class AssemblyInfo
{
    public const string ProductVersion = "1.0.0";

    public const string Copyright = 
        "Copyright \xA9 2021, Giovanni Lafratta, All Rights Reserved.";
}
