# Novacta.Shfb.LatexTools

This repo contains the source code for **Novacta.Shfb.LatexTools**, a .NET library that
enables adding LaTeX content in documentation files created via the
[Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB) project.

The tools support SHFB, version 2021.10.23.0 or later.

## Features

* The 
  [LatexComponent](https://novacta.github.io/shfb-latex-tools/html/T_Novacta_Shfb_LatexTools_LatexComponent.htm)
  enables the representation of LaTeX equations
  as images in SHFB documentation files.
  Supported image formats are
  [PNG](https://en.wikipedia.org/wiki/Portable_Network_Graphics)
  and
  [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics).
* The
  [LatexPlugIn](https://novacta.github.io/shfb-latex-tools/html/T_Novacta_Shfb_LatexTools_LatexPlugIn.htm),
  which is responsible to support the *SVG* image file format for the *MSHelpViewer* help output.
* The presentation styles
  [VS2010WithLatex](https://novacta.github.io/shfb-latex-tools/html/T_Novacta_Shfb_LatexTools_PresentationStyles_VS2010WithLatex.htm)
  and
  [VS2013WithLatex](https://novacta.github.io/shfb-latex-tools/html/T_Novacta_Shfb_LatexTools_PresentationStyles_VS2013WithLatex.htm),
  which add to styles *VS2010* and *VS2013*, respectively, the ability to manage LaTeX content.

## Installation

The library is available as a [NuGet package](https://www.nuget.org/packages/Novacta.Shfb.LatexTools).

## Documentation

[Adding LaTeX equations](https://novacta.github.io/shfb-latex-tools/html/47a5afe5-726c-4f74-9ab1-90740bf0a692.htm)
includes topics showing how to define LaTeX content in help files.

Full documentation is located [here](http://novacta.github.io/shfb-latex-tools/).

## Versioning

For available versions, see the
[tags on this repository](https://github.com/novacta/shfb-latex-tools/tags).

## License

**Novacta.Shfb.LatexTools** is licensed under the
[MIT License](https://github.com/novacta/shfb-latex-tools/blob/master/LICENSE.md).
