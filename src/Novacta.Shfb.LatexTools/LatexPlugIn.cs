// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Sandcastle.Core;
using SandcastleBuilder.Utils.BuildComponent;
using SandcastleBuilder.Utils.BuildEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Provides support for LaTeX formatted formulas in 
    /// reference XML comments and conceptual content topics.
    /// </summary>
    [HelpFileBuilderPlugInExport(id: "Novacta LaTeX PlugIn",
        Version = AssemblyInfo.ProductVersion,
        Copyright = AssemblyInfo.Copyright,
        Description = AssemblyInfo.Description)]
    public sealed class LatexPlugIn : IPlugIn
    {
        #region State

        private List<ExecutionPoint> executionPoints;

        private BuildProcess builder;

        #endregion

        #region IPlugIn implementation

        /// <summary>
        /// Returns a collection of execution points that define 
        /// when the plug-in should be invoked during the build process.
        /// </summary>
        public IEnumerable<ExecutionPoint> ExecutionPoints
        {
            get
            {
                if (executionPoints == null)
                    executionPoints = new List<ExecutionPoint>
                    {
                        new ExecutionPoint(
                            BuildStep.CreateBuildAssemblerConfigs,
                            ExecutionBehaviors.Before),
                        new ExecutionPoint(
                            BuildStep.CompilingHelpFile,
                            ExecutionBehaviors.Before)
                    };

                return executionPoints;
            }
        }

        /// <summary>
        /// Initializes the plug-in at the start of the build process.
        /// </summary>
        /// <param name="buildProcess">
        /// The current build process.
        /// </param>
        /// <param name="configuration">
        /// The configuration data that the plug-in should use to initialize itself.
        /// </param>        
        public void Initialize(BuildProcess buildProcess, XElement configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.builder = buildProcess;

            var metadata = (HelpFileBuilderPlugInExportAttribute)this.GetType()
                .GetCustomAttributes(
                    typeof(HelpFileBuilderPlugInExportAttribute), false)
                .First();

            this.builder.ReportProgress("{0} Version {1}\r\n{2}",
                metadata.Id,
                metadata.Version,
                metadata.Copyright);
        }

        /// <summary>
        /// Executes the plug-in during the build process.
        /// </summary>
        /// <param name="context">
        /// The current execution context.
        /// </param>
        public void Execute(ExecutionContext context)
        {
            switch (context.BuildStep)
            {
                case BuildStep.CreateBuildAssemblerConfigs:
                    {
                        this.builder.ReportProgress(
                            "Novacta.Shfb.LatexPlugIn: added latexImg element transformation.");

                        var transformation =
                            this.builder.PresentationStyle.TopicTransformation;

                        transformation.AddElement(new LatexImgElement());
                    }
                    break;
                case BuildStep.CompilingHelpFile:
                    {
                        if (this.builder.CurrentFormat == HelpFileFormats.MSHelpViewer)
                        {
                            this.builder.ReportProgress(
                                "Novacta.Shfb.LatexPlugIn: " +
                                "transforming MS Help Viewer files to represent " +
                                "LaTeX equations using a supported image file format.");
                            this.TransformLatexEquationRepresentations();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Transforms LaTeX img tags into embed tags.
        /// </summary>
        private void TransformLatexEquationRepresentations()
        {
            string imgSrcValueFormat =
                @"ms-xhelp:///?method=asset&id=media\{0}.{1}&package={2}.mshc&topiclocale={3}";

            string basePath = this.builder.WorkingFolder + @"\Output\MSHelpViewer\html\";
            bool isFirstLatexNode = true;
            bool isSvgSelected = true;

            foreach (string sourceFile in Directory.EnumerateFiles(basePath))
            {

                XmlDocument document = new XmlDocument();
                document.Load(sourceFile);
                XmlNode root = document.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ns", "http://www.w3.org/1999/xhtml");

                XmlNodeList latexImglist = root.SelectNodes("//ns:img[@alt='LaTeX equation']", nsmgr);

                if (latexImglist.Count > 0)
                {
                    foreach (XmlNode img in latexImglist)
                    {
                        XmlAttribute imgSrc = img.Attributes["src"];
                        string imgSrcValue = imgSrc.Value;
                        string fileName, fileExtension;
                        int slashPosition = imgSrcValue.LastIndexOf("/");
                        int dotPosition = imgSrcValue.LastIndexOf('.');

                        fileName = imgSrcValue.Substring(
                            startIndex: slashPosition + 1,
                            length: dotPosition - slashPosition - 1);

                        fileExtension = imgSrcValue.Substring(
                            startIndex: dotPosition + 1,
                            length: imgSrcValue.Length - dotPosition - 1);

                        if (isFirstLatexNode)
                        {
                            isSvgSelected =
                                string.CompareOrdinal(fileExtension, "svg") == 0;

                            isFirstLatexNode = false;
                        }

                        switch (isSvgSelected)
                        {
                            case false:
                                {
                                    imgSrc.Value = string.Format(
                                        imgSrcValueFormat,
                                        fileName,
                                        fileExtension,
                                        this.builder.ResolvedHtmlHelpName,
                                        this.builder.CurrentProject.Language.Name);
                                }
                                break;
                            case true:
                                {
                                    XmlNode embed = document.CreateElement("embed");

                                    XmlAttribute alt = document.CreateAttribute("alt");
                                    alt.Value = "LaTeX equation";
                                    embed.Attributes.Append(alt);

                                    XmlAttribute type = document.CreateAttribute("type");
                                    type.Value = "image/svg+xml";
                                    embed.Attributes.Append(type);

                                    XmlAttribute src = document.CreateAttribute("src");

                                    XmlNode imgStyle = img.Attributes.GetNamedItem("style");

                                    if (!(imgStyle is null))
                                    {
                                        XmlAttribute style = document.CreateAttribute("style");
                                        style.Value = imgStyle.Value;
                                        embed.Attributes.Append(style);
                                    }

                                    src.Value = string.Format(
                                        imgSrcValueFormat,
                                        fileName,
                                        fileExtension,
                                        this.builder.ResolvedHtmlHelpName,
                                        this.builder.CurrentProject.Language.Name);

                                    embed.Attributes.Append(src);

                                    img.ParentNode.ReplaceChild(embed, img);
                                }
                                break;
                        }
                    }

                    document.Save(sourceFile);
                }
            }
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Allows this instance to try to free resources and perform other 
        /// cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~LatexPlugIn()
        {
            this.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
