// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using SandcastleBuilder.Utils.BuildComponent;
using SandcastleBuilder.Utils.BuildEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Provides support in MS Help Viewer files for LaTeX content 
    /// represented via the SVG image format.
    /// </summary>
    [HelpFileBuilderPlugInExport(
        id: "Novacta.Shfb.LatexPlugIn",
        Version = AssemblyInfo.ProductVersion,
        Copyright = AssemblyInfo.Copyright,
        Description = "Provides support in MS Help Viewer files for LaTeX content " +
            "represented via the SVG image format.")]
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
                if (this.executionPoints == null)
                    this.executionPoints = new List<ExecutionPoint>
                    {
                        new ExecutionPoint(BuildStep.CompilingHelpFile, ExecutionBehaviors.Before),
                    };

                return this.executionPoints;
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
            builder = buildProcess;

            var metadata = (HelpFileBuilderPlugInExportAttribute)this.GetType()
                .GetCustomAttributes(
                    typeof(HelpFileBuilderPlugInExportAttribute), false)
                .First();

            builder.ReportProgress("{0} Version {1}\r\n{2}",
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
            if (this.builder.CurrentFormat == Sandcastle.Core.HelpFileFormats.MSHelpViewer)
            {
                this.TransformLaTeXEmbedTags();
            }
        }

        /// <summary>
        /// Transforms LaTeX img tags into embed tags.
        /// </summary>
        private void TransformLaTeXEmbedTags()
        {
            string basePath = this.builder.WorkingFolder + @"\Output\MSHelpViewer\html\";
            bool isFirstFile = true;

            foreach (string sourceFile in Directory.EnumerateFiles(basePath))
            {

                XmlDocument document = new XmlDocument();
                document.Load(sourceFile);
                XmlNode root = document.DocumentElement;

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ns", "http://www.w3.org/1999/xhtml");

                XmlNodeList list = root.SelectNodes("//ns:img[@alt='LaTeX equation']", nsmgr);

                if (list.Count > 0)
                {
                    foreach (XmlNode img in list)
                    {
                        XmlNode embed = document.CreateElement("embed");

                        XmlAttribute alt = document.CreateAttribute("alt");
                        alt.Value = "LaTeX equation";
                        embed.Attributes.Append(alt);

                        XmlAttribute type = document.CreateAttribute("type");
                        type.Value = "image/svg+xml";
                        embed.Attributes.Append(type);

                        XmlAttribute src = document.CreateAttribute("src");
                        string imgSource = img.Attributes.GetNamedItem("src").Value;
                        string fileName, fileExtension;
                        int slashPosition = imgSource.IndexOf("/", StringComparison.OrdinalIgnoreCase);
                        int dotPosition = imgSource.LastIndexOf('.');
                        fileName = imgSource.Substring(slashPosition + 1, dotPosition - slashPosition - 1);
                        fileExtension = imgSource.Substring(dotPosition + 1, imgSource.Length - dotPosition - 1);

                        XmlNode imgStyle = img.Attributes.GetNamedItem("style");
                        if (!(imgStyle is null))
                        {
                            XmlAttribute style = document.CreateAttribute("style");
                            style.Value = imgStyle.Value;
                            embed.Attributes.Append(style);
                        }

                        if (isFirstFile)
                        {
                            if (string.CompareOrdinal(fileExtension, "svg") != 0)
                            {
                                // LaTeX equations are represented using a graphic format 
                                // other than SVG
                                return;
                            }
                            isFirstFile = false;
                        }

                        src.Value = string.Format(
                            CultureInfo.InvariantCulture,
                            @"ms-xhelp:///?method=asset&id=media\{0}.{1}&package={2}.mshc&topiclocale={3}",
                            fileName,
                            "svg",
                            this.builder.ResolvedHtmlHelpName,
                            this.builder.CurrentProject.Language.Name);

                        embed.Attributes.Append(src);

                        img.ParentNode.ReplaceChild(embed, img);
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
