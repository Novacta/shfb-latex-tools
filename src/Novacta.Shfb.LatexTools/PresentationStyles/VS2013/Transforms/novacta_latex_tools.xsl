<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <!--
==============================================
Transform for <latexImg> nodes
==============================================

Provides support for the use of Latex formulas in SHFB documentation topics.


EXAMPLE

Let us assume that the following <latex> node is inserted in 
a documentation topic:

<latex mode='inline' depth='25'>f_X\left(x\right)=x^2</latex>

and that the Novacta SHFB Latex Tools generated a file named 'latex_1.png'
to represent the formula.
Then, it will be transformed as follows:

<latexImg>
  <name>latex_1</name>
  <format>png</format>
  <inline>1</inline>
  <depth>25</depth>
</latexImg>

Such <latexImg> node is transformed by this style sheet in different ways, 
depending on the selected presentation style.

For style 'OpenXml', it is transformed into

<img src="../media/latex_1.png" alt="LaTeX equation" style="vertical-align: -25px"></img>

For styles 'VS2010' and 'VS2013', for help outputs 'HtmlHelp1' and 'Website',
it is transformed into

<img src="../media/latex_1.png" alt ="LaTeX equation" style="vertical-align: -25px"></img>

while for help output 'MsHelpViewer', it is transformed into

<img src="media/latex_1.png" alt ="LaTeX equation" style="vertical-align: -25px"></img>

The last transform also happen for style 'Markdown'.


REMARK

At the time of this writing, SVG files are not supported by 
the OpenXml presentation style.


EXAMPLE OF INSTALLATION FOR PRESENTATION STYLE 'VS2013'

The following is the novacta_latex_tools.xsl transform.

It must be copied in the 'Transforms' folder of style 'VS2013',
and imported in the main style sheets, accordingly.
This implies that it must be imported in files

VS2013\Transforms\main_sandcastle.xsl

and

VS2013\Transforms\main_conceptual.xsl

by adding node

<xsl:import href="novacta_latex_tools.xsl"/>

before node

<xsl:output method="xml" omit-xml-declaration="yes" indent="no" encoding="utf-8"/>

In addition, the following shared content items must be added as follows.

For help outputs 'HtmlHelp1' and 'Website',

<item id="novacta_latex_tools_path">../media/{0}.{1}</item>
<item id="novacta_latex_tools_depth">vertical-align: -{0}px</item>

must be inserted in VS2013\Content\shared_content.xml

For help output 'MsHelpViewer',

<item id="novacta_latex_tools_path">media/{0}.{1}</item>

must be inserted in VS2013\Content\shared_content_mshc.xml
-->
  <xsl:template match="latexImg">
    <img alt="LaTeX equation">
      <includeAttribute name="src" item="novacta_latex_tools_path">
        <parameter>
          <xsl:value-of select="name" />
        </parameter>
        <parameter>
          <xsl:value-of select="format" />
        </parameter>
      </includeAttribute>
      <xsl:if test="inline=1">
        <includeAttribute name="style" item="novacta_latex_tools_depth">
          <parameter>
            <xsl:value-of select="depth" />
          </parameter>
        </includeAttribute>
      </xsl:if>
    </img>
  </xsl:template>
</xsl:stylesheet>