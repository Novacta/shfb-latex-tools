// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Sandcastle.Core;
using Sandcastle.Core.BuildAssembler;
using Sandcastle.Core.BuildAssembler.BuildComponent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Provides support for LaTeX formatted formulas in 
    /// reference XML comments and conceptual content topics.
    /// </summary>
    public class LatexComponent : BuildComponentCore
    {
        #region Build component factory for MEF
        
        /// <summary>
        /// Creates a new instance of the build component
        /// </summary>
        [BuildComponentExport("Novacta LaTeX Component", 
            IsVisible = true, 
            Version = AssemblyInfo.ProductVersion,
            Copyright = AssemblyInfo.Copyright, 
            Description = AssemblyInfo.Description)]
        public sealed class Factory : BuildComponentFactory
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            public Factory()
            {
                // Build placement tells tools such as the
                // Sandcastle Help File Builder how to insert the
                // component into build configurations in projects
                // to which it is added.

                // Set placement for reference builds 
                this.ReferenceBuildPlacement = 
                    new ComponentPlacement(
                        PlacementAction.Before,
                        "Transform Component");

                // Set placement for conceptual builds
                this.ConceptualBuildPlacement = 
                    new ComponentPlacement(
                        PlacementAction.Before,
                        "Transform Component");
            }

            /// <inheritdoc />
            public override BuildComponentCore Create()
            {
                return new LatexComponent(this.BuildAssembler);
            }

            /// <inheritdoc />
            public override string DefaultConfiguration =>
                @"<documentClass value=""article"" />" +
                @"<imageFileFormat value=""svg"" />" +
                @"<additionalPreambleCommands>" +
                @"<line>" +
                @"% Paste here your additional preamble commands" +
                @"</line>" +
                @"</additionalPreambleCommands>" +
                @"<latexDefaultMode value=""display""/>" +
                @"<imageDepthCorrection value=""0"" />" +
                @"<imageScalePercentage value=""100"" />" +
                @"<redirectFileProcessors value=""false"" />" +
                @"<latexBinPath value="""" />" +
                @"<helpType value=""{@HelpFileFormat}"" />" +
                @"<basePath value=""{@WorkingFolder}"" />" +
                @"<languagefilter value=""true"" />";
        }

        #endregion

        #region State

        private readonly XElement defaultConfiguration =
            new XElement("configuration",
                new XElement("documentClass", new XAttribute("value", "article")),
                new XElement("imageFileFormat", new XAttribute("value", "svg")),
                new XElement("additionalPreambleCommands",
                    new XElement("line", "% Paste here your additional preamble commands")),
                new XElement("latexDefaultMode", new XAttribute("value", "display")),
                new XElement("imageDepthCorrection", new XAttribute("value", "0")),
                new XElement("imageScalePercentage", new XAttribute("value", "100")),
                new XElement("redirectFileProcessors", new XAttribute("value", "false")),
                new XElement("latexBinPath", new XAttribute("value", ""))
            );

        /// <summary>
        /// Each LaTeX equation is represented as a LaTeX file.
        /// The following field is an equation identifier,
        /// and is used to form distinct names for the corresponding 
        /// LaTeX files.
        /// </summary>
        private static int EquationId;

        private readonly XPathExpression referenceRoot =
            XPathExpression.Compile("document/comments");

        private bool isFileFormatPng;

        private string initialTexDocument;

        /// <summary>
        /// Gets or sets the additional preamble commands.
        /// </summary>
        /// <value>The additional preamble commands.</value>
        private string[] AdditionalPreambleCommands;

        private static string[] GetAdditionalPreambleCommands(
            XPathNavigator additionalPreambleCommands)
        {
            List<string> lines = new List<string>();

            if (additionalPreambleCommands.MoveToFirstChild())
            {
                lines.Add(additionalPreambleCommands.Value);
                while (additionalPreambleCommands.MoveToNext())
                {
                    lines.Add(additionalPreambleCommands.Value);
                }
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Gets or sets the LaTeX default mode.
        /// </summary>
        /// <value>The LaTeX default mode.</value>
        private string LatexDefaultMode;

        /// <summary>
        /// Gets the image file format.
        /// </summary>
        /// <value>The image file format.</value>
        private string ImageFileFormat;

        /// <summary>
        /// Gets or sets the image depth correction.
        /// </summary>
        /// <value>The image depth correction.</value>
        private int ImageDepthCorrection;

        /// <summary>
        /// Gets or sets the image scale percentage.
        /// </summary>
        /// <value>The image scale percentage.</value>
        private double ImageScalePercentage;

        /// <summary>
        /// Gets a value indicating whether file processors
        /// standard output must be redirected to the 
        /// Sandcastle Help File Builder Log.
        /// </summary>
        /// <value><c>true</c> if file processors must be redirected;
        /// otherwise, <c>false</c>.</value>
        private bool RedirectFileProcessors;

        /// <summary>
        /// Gets the LaTeX file processor.
        /// </summary>
        /// <value>The LaTeX file processor.</value>
        private FileProcessor Latex;

        /// <summary>
        /// Gets the DviPng file processor.
        /// </summary>
        /// <value>The DviPng file processor.</value>
        private FileProcessor DviPng;

        /// <summary>
        /// Gets the DviSvgm file processor.
        /// </summary>
        /// <value>The DviSvgm file processor.</value>
        private FileProcessor DviSvgm;

        /// <summary>
        /// Gets the output folders.
        /// </summary>
        /// <value>The output folders.</value>
        private string[] OutputFolders;

        /// <summary>
        /// Gets the working folder for file processors.
        /// </summary>
        /// <remarks>
        /// It is equivalent to basePath + "\LaTeX\".
        /// </remarks>
        /// <value>The working folder for file processors.</value>
        private string WorkingFolder;

        #region LaTeX mode

        private static readonly string[] SupportedLaTeXModes =
            new string[2] { "inline", "display" };

        /// <summary>
        /// Determines whether the specified mode is a supported LaTeX mode.
        /// </summary>
        /// <param name="mode">
        /// The mode to be checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified mode is supported; 
        /// otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLaTeXModeSupported(string mode)
        {
            foreach (var supportedMode in SupportedLaTeXModes)
            {
                if (0 == string.CompareOrdinal(supportedMode, mode))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region LaTeX scale

        private const double BasePngResolution = 120.0;
        private const double BaseSvgZoomFactor = 1.2;

        private static readonly string[] PredefinedScaleNames = new string[10] {
            "tiny",
            "scriptsize",
            "footnotesize",
            "small",
            "normalsize",
            "large",
            "Large",
            "LARGE",
            "huge",
            "Huge" };

        private static Dictionary<string, double> InitializePredefinedScaleFactors()
        {
            Dictionary<string, double> zoomFactors = new Dictionary<string, double>();

            double[] factors = new double[10] {
                0.5,
                0.7,
                0.8,
                0.9,
                1,
                1.2,
                1.44,
                1.728,
                2.074,
                2.488
            };

            for (int i = 0; i < 10; i++)
            {
                zoomFactors[PredefinedScaleNames[i]] = factors[i];
            }

            return zoomFactors;
        }

        private static readonly Dictionary<string, double> PredefinedScaleFactors =
            InitializePredefinedScaleFactors();

        /// <summary>
        /// Determines whether the specified scale is supported.
        /// </summary>
        /// <param name="scale">
        /// The scale to be checked.
        /// </param>
        /// <param name="scaleFactor">
        /// When this method returns, contains the factor associated 
        /// with the specified scale, if the scale is supported; otherwise, zero. 
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified scale is supported;
        /// otherwise, <c>false</c>.
        /// </returns>
        private static bool TryGetScaleFactor(string scale, out double scaleFactor)
        {
            bool isScalePredefined = PredefinedScaleFactors.TryGetValue(scale, out scaleFactor);
            if (isScalePredefined)
            {
                return true;
            }

            bool isParsable = Double.TryParse(scale, out double scalePercentage);
            scaleFactor = scalePercentage / 100.0;
            if (!isParsable)
            {
                return false;
            }
            if (scaleFactor <= 0.0)
            {
                scaleFactor = 0.0;
                return false;
            }

            return true;
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexComponent"/> class
        /// with the specified build assembler.
        /// </summary>
        /// <param name="buildAssembler">
        /// The build assembler.
        /// </param>
        protected LatexComponent(IBuildAssembler buildAssembler) : base(buildAssembler)
        {
        }

        #endregion

        #region Abstract method implementations

        /// <summary>
        /// Initializes the build component.
        /// </summary>
        /// <param name="configuration">
        /// The component configuration.
        /// </param>
        public override void Initialize(XPathNavigator configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            
            this.WriteMessage(MessageLevel.Info,
                "[{0}, version {1}]\r\n    {2}",
                "Novacta LaTeX Component",
                AssemblyInfo.ProductVersion,
                AssemblyInfo.Copyright);

            #region Configuration

            #region Additional preamble commands

            var additionalPreambleCommands =
                configuration.SelectSingleNode("//additionalPreambleCommands");

            this.AdditionalPreambleCommands =
                (additionalPreambleCommands is null)
                ?
                    new string[1]
                        {
                            this.defaultConfiguration
                                .Element("additionalPreambleCommands")
                                .Element("line").Value
                        }
                :
                GetAdditionalPreambleCommands(
                    configuration.SelectSingleNode("//additionalPreambleCommands"));

            var texBuilder = new StringBuilder();
            texBuilder.Append("\\documentclass[10pt]{article}\r\n");
            texBuilder.Append("\\usepackage[active,textmath,displaymath]{preview}\r\n");
            texBuilder.Append("\\pagestyle{empty}\r\n");

            for (int i = 0; i < this.AdditionalPreambleCommands.Length; i++)
            {
                texBuilder.AppendLine(this.AdditionalPreambleCommands[i]);
            }

            texBuilder.Append("\\begin{document}\r\n");

            this.initialTexDocument = texBuilder.ToString();

            #endregion

            #region LaTeX default mode

            var latexDefaultMode = configuration.SelectSingleNode("//latexDefaultMode");
            this.LatexDefaultMode =
                (latexDefaultMode is null)
                ?
                    this.defaultConfiguration
                        .Element("latexDefaultMode")
                        .Attribute("value").Value
                :
                latexDefaultMode
                    .GetAttribute("value", String.Empty);

            #endregion

            #region Image file format

            var imageFormatNode = configuration.SelectSingleNode("//imageFileFormat");

            this.ImageFileFormat =
                (imageFormatNode is null)
                ?
                    this.defaultConfiguration
                        .Element("imageFileFormat")
                        .Attribute("value").Value
                :
                    imageFormatNode
                        .GetAttribute("value", String.Empty);

            this.isFileFormatPng =
                (0 == String.CompareOrdinal(this.ImageFileFormat, "png"));

            #endregion

            #region Image depth correction

            var imageDepthCorrectionNode = configuration.SelectSingleNode("//imageDepthCorrection");

            this.ImageDepthCorrection =
                Convert.ToInt32(
                    (imageDepthCorrectionNode is null)
                    ?
                        this.defaultConfiguration
                            .Element("imageDepthCorrection")
                            .Attribute("value").Value
                    :
                        imageDepthCorrectionNode
                            .GetAttribute("value", String.Empty),
                    CultureInfo.InvariantCulture);

            #endregion

            #region Image scale factor

            var imageScalePercentageNode = configuration.SelectSingleNode("//imageScalePercentage");

            this.ImageScalePercentage =
                     Convert.ToDouble(
                    (imageScalePercentageNode is null)
                    ?
                        this.defaultConfiguration
                            .Element("imageScalePercentage")
                            .Attribute("value").Value
                    :
                        imageScalePercentageNode.GetAttribute("value", String.Empty),
                    CultureInfo.InvariantCulture);

            #endregion

            #region Redirect file processors

            // redirectFileProcessors

            var redirectFileProcessorsNode =
                configuration.SelectSingleNode("//redirectFileProcessors");

            this.RedirectFileProcessors =
                 Convert.ToBoolean(
                    (redirectFileProcessorsNode is null)
                    ?
                        this.defaultConfiguration
                            .Element("redirectFileProcessors")
                            .Attribute("value").Value
                    :
                        redirectFileProcessorsNode.GetAttribute("value", String.Empty),
                CultureInfo.InvariantCulture);

            #endregion

            #region LaTeX processor

            #region Working folder

            var basePathNode = configuration.SelectSingleNode("//basePath");

            var basePath = basePathNode.GetAttribute("value", String.Empty) +
                @"Help\Working\";

            string workingFolder = basePath + "LaTeX";

            this.WorkingFolder = workingFolder;

            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            #endregion

            #region Bin folder

            // latexBinPath

            var latexBinPathNode = configuration.SelectSingleNode("//latexBinPath");

            var latexBinFolder =
                (latexBinPathNode is null)
                ?
                    this.defaultConfiguration
                        .Element("latexBinPath")
                        .Attribute("value").Value
                :
                    latexBinPathNode.GetAttribute("value", String.Empty);

            #endregion

            this.Latex = new LatexProcessor(latexBinFolder, workingFolder);

            string defaultPngResolution = Convert.ToString(
                BasePngResolution,
                CultureInfo.InvariantCulture);
            if (this.ImageScalePercentage != 100.0)
            {
                defaultPngResolution = Convert.ToString(
                    Math.Ceiling(BasePngResolution * this.ImageScalePercentage / 100.0),
                    CultureInfo.InvariantCulture);
            }

            this.DviPng = new DviPngProcessor(latexBinFolder, workingFolder, defaultPngResolution);

            #endregion

            #region DviSvgm processor

            // dvisvgmBinPath

            var dvisvgmBinFolder = latexBinFolder;

            string defaultSvgZoomFactor = Convert.ToString(
                BaseSvgZoomFactor,
                CultureInfo.InvariantCulture);

            if (this.ImageScalePercentage != 100.0)
            {
                defaultSvgZoomFactor = Convert.ToString(
                    BaseSvgZoomFactor * this.ImageScalePercentage / 100.0,
                    CultureInfo.InvariantCulture);
            }

            this.DviSvgm = new DviSvgmProcessor(
                dvisvgmBinFolder,
                workingFolder,
                defaultSvgZoomFactor,
                this.RedirectFileProcessors);

            #endregion

            #region Documentation output folders

            var helpTypeNode = configuration.SelectSingleNode("//helpType");

            var helpTypes = helpTypeNode.GetAttribute("value", String.Empty);
            var types = helpTypes.Split(',');

            var outputFolders = new string[types.Length];

            for (var i = 0; i < outputFolders.Length; i++)
            {
                var type = types[i].Trim();

                string path;
                if (type.Equals("HtmlHelp1", StringComparison.OrdinalIgnoreCase))
                {
                    path = @"Output\HtmlHelp1\media\";
                }
                else if (type.Equals("Website", StringComparison.OrdinalIgnoreCase))
                {
                    path = @"Output\Website\media\";
                }
                else if (type.Equals("MSHelpViewer", StringComparison.OrdinalIgnoreCase))
                {
                    path = @"Output\MSHelpViewer\media\";
                }
                else if (type.Equals("OpenXml", StringComparison.OrdinalIgnoreCase))
                {
                    path = @"Output\OpenXml\media\";
                }
                else if (type.Equals("Markdown", StringComparison.OrdinalIgnoreCase))
                {
                    path = @"Output\Markdown\media\";
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "Help file format {0} is not supported " +
                            "by the Novacta Latex Tools.", type));
                }

                outputFolders[i] = basePath + path;
            }

            this.OutputFolders = outputFolders;

            #endregion

            #endregion

            #region Component configuration messages

            this.WriteMessage(MessageLevel.Info, "Additional preamble commands:");
            for (int i = 0; i < this.AdditionalPreambleCommands.Length; i++)
            {
                this.WriteMessage(MessageLevel.Info,
                    this.AdditionalPreambleCommands[i]);
            }

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Documentation working folder: {0}",
                    basePath));

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Component working folder: {0}",
                    workingFolder));

            this.WriteMessage(MessageLevel.Info,
                "Default LaTex mode: " + this.LatexDefaultMode);

            this.WriteMessage(MessageLevel.Info,
                "Image File Format: " + this.ImageFileFormat);

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                     CultureInfo.InvariantCulture,
                   "Image depth correction: {0}",
                    this.ImageDepthCorrection));

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Image scale percentage: {0}",
                    this.ImageScalePercentage));

            this.WriteMessage(MessageLevel.Info,
                "Redirect file processors: " +
                this.RedirectFileProcessors.ToString());

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "LaTeX binary folder: {0}",
                    latexBinFolder));

            this.WriteMessage(MessageLevel.Info,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "DviSvgm binary folder: {0}",
                    dvisvgmBinFolder));

            #endregion
        }

        /// <summary>
        /// Performs the component tasks
        /// using different settings for conceptual and reference topics.
        /// </summary>
        /// <param name="document">
        /// The XML document under study.
        /// </param>
        /// <param name="namePrefix">
        /// The prefix for LaTeX equation names.
        /// </param>
        /// <param name="list">
        /// The list of latex nodes to be transformed.
        /// </param>
        /// <param name="isTopicConceptual">
        /// if set to <c>true</c> the topic is conceptual; otherwise, <c>false</c>.
        /// </param>
        private void Apply(XmlDocument document,// string key,
            string namePrefix, XmlNodeList list, bool isTopicConceptual)
        {
            if (list is null)
            {
                return;
            }

            string defaultConceptualNamespace = isTopicConceptual ?
                "http://ddue.schemas.microsoft.com/authoring/2003/5" : null;
            StringBuilder texBuilder = new StringBuilder();

            foreach (XmlNode node in list)
            {

                #region Mode attribute

                // LaTeX mode attribute defaults to a configuration option
                string latexMode = this.LatexDefaultMode;
                if (!(node.Attributes["mode"] is null))
                {
                    string mode = node.Attributes["mode"].InnerText;
                    if (IsLaTeXModeSupported(mode))
                    {
                        latexMode = mode;
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized LaTeX mode: \"" +
                            mode + "\". Using default mode: \"" + latexMode + "\".");
                    }
                }

                #endregion

                string equationName = namePrefix + EquationId;

                #region Latex generation

                texBuilder.Clear();

                string texFileName = equationName + ".tex";
                string defaultLatexScale = "normalsize";
                string latexNodeInnerText = node.InnerText.Replace("\\\\", "\\\\\\\\").Trim();

                texBuilder.Append(this.initialTexDocument);

                switch (latexMode)
                {
                    case "inline":
                        texBuilder.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "\\begin{{{0}}}${1}$\\end{{{2}}}\r\n",
                            defaultLatexScale, latexNodeInnerText, defaultLatexScale);
                        break;
                    case "display":
                        texBuilder.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "\\begin{{{0}}}\\[{1}\\]\\end{{{2}}}\r\n",
                            defaultLatexScale, latexNodeInnerText, defaultLatexScale);
                        break;
                }

                texBuilder.Append("\\end{document}\r\n");

                using (StreamWriter stringWriter = new StreamWriter(
                    this.WorkingFolder + Path.DirectorySeparatorChar + texFileName))
                {
                    stringWriter.Write(texBuilder.ToString());
                }

                #endregion

                #region Scale attribute

                string pngResolution = null;
                string svgZoomFactor = null;
#pragma warning disable IDE0018 // Inline variable declaration
                double scaleFactor;
#pragma warning restore IDE0018 // Inline variable declaration

                if (!(node.Attributes["scale"] is null))
                {
                    string scale = node.Attributes["scale"].InnerText;
                    if (TryGetScaleFactor(scale, out scaleFactor))
                    {
                        pngResolution = Convert.ToString(
                            Math.Ceiling(scaleFactor * BasePngResolution),
                            CultureInfo.InvariantCulture);

                        svgZoomFactor = Convert.ToString(
                            scaleFactor * BaseSvgZoomFactor,
                            CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized scale: \"" +
                            scale + "\". Using default scale percentage: \"" + "100" + "\".");
                    }
                }

                #endregion

                #region Dvi generation

                string latexOutput = this.Latex.Run(texFileName);
                if (this.RedirectFileProcessors)
                {
                    this.WriteMessage(MessageLevel.Info, "Running LaTeX on " + texFileName + ".");
                    this.WriteMessage(MessageLevel.Info, latexOutput);
                }

                #endregion

                string outputFile, dviFileName = equationName + ".dvi";

                #region Png generation

                string dvipngOutput = this.DviPng.Run(equationName, pngResolution);
                if (this.RedirectFileProcessors)
                {
                    this.WriteMessage(MessageLevel.Info, "Running DviPng on " + dviFileName + ".");
                    this.WriteMessage(MessageLevel.Info, dvipngOutput);
                }

                string pngFileName = equationName + ".png";
                string pngFilePath = this.WorkingFolder +
                     Path.DirectorySeparatorChar + pngFileName;

                if (this.isFileFormatPng)
                {
                    foreach (var outputFolder in this.OutputFolders)
                    {
                        outputFile = outputFolder + pngFileName;
                        WriteMessage(MessageLevel.Info,
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Copying {0} to {1}", pngFilePath, outputFile));

                        File.Copy(pngFilePath, outputFile, true);
                    }

                }

                #endregion

                #region Depth attribute

                int imageDepth = 0;

                bool applyCorrectedDvipngDepth = true;

                if (!(node.Attributes["depth"] is null))
                {
                    string depth = node.Attributes["depth"].InnerText;
                    if (Int32.TryParse(depth, out imageDepth))
                    {
                        applyCorrectedDvipngDepth = false;
                    }
                    else
                    {
                        this.WriteMessage(MessageLevel.Warn, "Unrecognized depth: \"" +
                            depth + "\". Using corrected DviPng depth.");
                    }
                }
                if (applyCorrectedDvipngDepth)
                {
                    // Determine the DviPng image depth
                    int firstDepthPosition = dvipngOutput.IndexOf(
                        "depth=", StringComparison.OrdinalIgnoreCase) + 6;
                    int lastDepthPosition = dvipngOutput.IndexOf(
                        "]",
                        firstDepthPosition, StringComparison.OrdinalIgnoreCase) - 1;
                    int dvipngImageDepth = Convert.ToInt32(
                        dvipngOutput.Substring(firstDepthPosition, 1 + lastDepthPosition - firstDepthPosition),
                        CultureInfo.InvariantCulture);
                    imageDepth = -this.ImageDepthCorrection + dvipngImageDepth;
                }

                #endregion

                if (!this.isFileFormatPng)
                {
                    #region Svg generation

                    string dvisvgmOutput = this.DviSvgm.Run(equationName, svgZoomFactor);
                    if (this.RedirectFileProcessors)
                    {
                        this.WriteMessage(MessageLevel.Info, "Running DviSvgm on " + dviFileName + ".");
                        this.WriteMessage(MessageLevel.Info, dvisvgmOutput);
                    }

                    string svgFileName = equationName + ".svg";

                    string svgFilePath = this.WorkingFolder +
                        Path.DirectorySeparatorChar + svgFileName;

                    foreach (var outputFolder in this.OutputFolders)
                    {
                        outputFile = outputFolder + svgFileName;
                        WriteMessage(MessageLevel.Info,
                            string.Format(
                                CultureInfo.CurrentCulture,
                                "Copying {0} to {1}", svgFilePath, outputFile));

                        File.Copy(svgFilePath, outputFile, true);
                    }

                    #endregion
                }

                #region Latex node emission

                var latex = document.CreateElement("latexImg");

                XmlAttribute nameAttribute = document.CreateAttribute("name");
                nameAttribute.Value = equationName;
                latex.SetAttributeNode(nameAttribute);

                XmlAttribute formatAttribute = document.CreateAttribute("format");
                formatAttribute.Value = this.ImageFileFormat;
                latex.SetAttributeNode(formatAttribute);

                bool isInlined = 0 == string.CompareOrdinal(latexMode, "inline");

                if (isInlined)
                {
                    XmlAttribute depthAttribute = document.CreateAttribute("depth");
                    depthAttribute.Value =
                        Convert.ToString(
                            imageDepth,
                            CultureInfo.InvariantCulture);
                    latex.SetAttributeNode(depthAttribute);
                }

                this.WriteMessage(MessageLevel.Info,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Node for LaTeX image {0}: \n {1}",
                        equationName, latex.OuterXml));

                if (!isInlined)
                {
                    if (isTopicConceptual)
                    {
                        XmlElement beforeMarkup = document.CreateElement("markup",
                            defaultConceptualNamespace);
                        beforeMarkup.AppendChild(document.CreateElement("br"));
                        beforeMarkup.AppendChild(document.CreateElement("br"));

                        node.ParentNode.InsertBefore(beforeMarkup, node);

                        XmlElement afterMarkup = document.CreateElement("markup",
                            defaultConceptualNamespace);
                        afterMarkup.AppendChild(document.CreateElement("br"));
                        afterMarkup.AppendChild(document.CreateElement("br"));

                        node.ParentNode.InsertAfter(afterMarkup, node);
                    }
                    else
                    {
                        node.ParentNode.InsertBefore(document.CreateElement("br"), node);
                        node.ParentNode.InsertBefore(document.CreateElement("br"), node);
                        node.ParentNode.InsertAfter(document.CreateElement("br"), node);
                        node.ParentNode.InsertAfter(document.CreateElement("br"), node);
                    }
                }
                node.ParentNode.ReplaceChild(latex, node);

                #endregion

                EquationId++;
            }
        }

        /// <summary>
        /// Performs the component tasks.
        /// </summary>
        /// <param name="document">
        /// The XML document.
        /// </param>
        /// <param name="key">
        /// The key (member name) of the item being documented.
        /// </param>
        public override void Apply(XmlDocument document, string key)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            XPathNavigator root, navDoc = document.CreateNavigator();
            root = navDoc.SelectSingleNode(this.referenceRoot);

            // If root is not null, it's a reference (API) build.  
            // If null, it's a conceptual build.
            if (root == null)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ltx", "http://www.novacta.net/2018/XSL/ShfbLatexTools");

                XmlNodeList list = document.SelectNodes("//ltx:latex", nsmgr);
                Apply(document, "clatex_", list, true);
            }
            else
            {
                XmlNodeList list = document.SelectNodes("//latex");
                Apply(document, "latex_", list, false);
            }
        }

        #endregion
    }
}
