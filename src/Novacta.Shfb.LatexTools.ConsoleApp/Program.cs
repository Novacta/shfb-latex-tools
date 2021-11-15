// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

// See https://aka.ms/new-console-template for more information
using Novacta.Shfb.LatexTools;

// Clone supported presentation styles

var targetBasePath = "../../../../Novacta.Shfb.LatexTools/PresentationStyles";

Shfb.ClonePresentationStyle(
    presentationStyle: "VS2010",
    targetBasePath: targetBasePath);
Shfb.ClonePresentationStyle(
    presentationStyle: "VS2013",
    targetBasePath: targetBasePath);

// Add support for Latex content to styles

LatexTools.AddLatexToPresentationStyles(
    path: "../../../../Novacta.Shfb.LatexTools/");

Console.WriteLine("Success.");

Console.ReadKey();