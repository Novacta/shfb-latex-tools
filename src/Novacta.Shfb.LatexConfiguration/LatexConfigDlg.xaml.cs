// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Sandcastle.Core.BuildAssembler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Novacta.Shfb.LatexConfiguration
{
    /// <summary>
    /// Represents the configuration form of the Novacta LaTeX Component.
    /// </summary>
    public partial class LatexConfigDlg : Window
    {
        #region Build component configuration editor factory for MEF

        // <summary>
        // Allows editing of the component configuration
        // </summary>
        [ConfigurationEditorExport("Novacta LaTeX Component")]
        public sealed class BuildComponentFactory : IConfigurationEditor
        {
            /// <inheritdoc />
            public bool EditConfiguration(
                XElement configuration, 
                CompositionContainer container)
            {
                var dlg = new LatexConfigDlg(configuration);

                return dlg.ShowDialog() ?? false;
            }
        }

        #endregion

        #region Configuration

        private readonly XElement configuration;

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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexConfigDlg"/> 
        /// class by parsing the configuration XML.
        /// </summary>
        /// <param name="configuration">
        /// The current configuration element.
        /// </param>
        /// <remarks>
        /// <para>
        /// An example of configuration follows:
        /// </para>
        /// <para>
        /// <code language="XML" title="Configuration Example">
        /// <![CDATA[
        /// <configuration>
        ///    <documentClass value="article" />
        ///    <imageFileFormat value="svg" />
        ///    <additionalPreambleCommands>
        ///       <line>% Add here additional preamble commands</line>
        ///    </additionalPreambleCommands>
        ///    <latexDefaultMode value="display"/>
        ///    <redirectFileProcessors value="false" />
        ///    <imageDepthCorrection value="0" />
        ///    <imageScalePercentage value="100" />
        ///    <latexBinPath value="C:\Program Files\MiKTeX 2.9\miktex\bin\x64" />
        /// </configuration>
        /// ]]>
        /// </code>
        /// </para>
        /// </remarks>
        public LatexConfigDlg(XElement configuration)
        {
            this.InitializeComponent();

            this.configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            // Additional Preamble Commands

            var additionalPreambleCommandsElement =
                configuration.Element("additionalPreambleCommands");

            if (additionalPreambleCommandsElement != null)
            {
                var lineNodes = additionalPreambleCommandsElement.Descendants();

                StringBuilder sb = new StringBuilder();

                foreach (var lineNode in lineNodes)
                {
                    sb.AppendLine(lineNode.Value);
                }

                this.c_additionalPreambleCommands.Text = sb.ToString();
            }

            // LaTeX Default Mode

            var latexDefaultModeElement =
                configuration.Element("latexDefaultMode");

            if (latexDefaultModeElement != null)
            {
                string mode =
                    latexDefaultModeElement.Attribute("value").Value;

                var latexDefaultModeCanvas = (Canvas)
                    this.g_LaTeXGrid
                        .Children
                        .OfType<GroupBox>()
                        .First(
                            r
                            => 
                            r.Name == "c_groupBoxDefaultLaTeXMode")
                        .Content;

                RadioButton defaultModeRadioButton =
                    latexDefaultModeCanvas
                        .Children
                        .OfType<RadioButton>()
                        .First(r => 0 == string.CompareOrdinal(
                            r.Content.ToString(), mode));

                defaultModeRadioButton.IsChecked = true;
            }
    
            // Image File Format

            var imageFileFormatElement =
                configuration.Element("imageFileFormat");

            if (imageFileFormatElement != null)
            {
                string format =
                    imageFileFormatElement.Attribute("value").Value;

                var imageFileFormatCanvas = (Canvas)
                    this.g_LaTeXGrid
                        .Children
                        .OfType<GroupBox>()
                        .First(
                            r
                            =>
                            r.Name == "c_groupBoxImageFileFormat")
                        .Content;

                RadioButton fileFormatRadioButton =
                    imageFileFormatCanvas
                        .Children
                        .OfType<RadioButton>()
                        .First(r => 0 == string.CompareOrdinal(
                            r.Content.ToString(), format));

                fileFormatRadioButton.IsChecked = true;
            }

            // Image Depth Correction

            var imageDepthCorrectionElement =
                configuration.Element("imageDepthCorrection");

            if (imageDepthCorrectionElement != null)
            {
                this.c_imageDepthCorrection.Text =
                    imageDepthCorrectionElement.Attribute("value").Value;
            }

            // Image Scale Factor

            var imageScalePercentageElement =
                configuration.Element("imageScalePercentage");

            if (imageScalePercentageElement != null)
            {
                this.c_imageScalePercentage.Text =
                    imageScalePercentageElement.Attribute("value").Value;
            }

            // Redirect File Processors

            var redirectFileProcessorsElement =
                configuration.Element("redirectFileProcessors");

            if (redirectFileProcessorsElement != null)
            {
                this.c_redirectFileProcessors.IsChecked = bool.Parse(
                    redirectFileProcessorsElement.Attribute("value").Value);
            }

            // LaTex Bin Folder

            var latexBinPathElement = configuration.Element("latexBinPath");

            if (latexBinPathElement != null)
            {
                this.c_latexBinFolder.Text =
                    latexBinPathElement.Attribute("value").Value;
            }
        }

#endregion

        #region Event handlers

        private void UpdateOrAddDefaultConfigurationElement(
            string elementName,
            string attributeValue)
        {
            var node = this.configuration.Element(elementName);

            if (node == null)
            {
                node = this.defaultConfiguration.Element(elementName);

                configuration.Add(node);
            }

            this.configuration.Element(elementName).Attribute("value").Value = attributeValue;
        }

        private void UpdateOrAddDefaultConfigurationSubElements(
            string elementName,
            string subElementName,
            List<string> subElementValues)
        {
            var node = this.configuration.Element(elementName);

            if (node == null)
            {
                node = this.defaultConfiguration.Element(elementName);

                configuration.Add(node);
            }

            this.configuration.Element(elementName).RemoveNodes();

            for (int i = 0; i < subElementValues.Count; i++)
            {
                var subElementValue = subElementValues[i];

                this.configuration.Element(elementName).Add(
                    new XElement(subElementName, subElementValue));
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Additional Preamble Commands

            var additionalPreambleCommandsValue =
                this.c_additionalPreambleCommands.Text;

            List<string> lines = new List<string>();

            for (int i = 0; i < this.c_additionalPreambleCommands.LineCount; i++)
            {
                var line = this.c_additionalPreambleCommands.GetLineText(i);

                lines.Add(line);
            }

            this.UpdateOrAddDefaultConfigurationSubElements(
                elementName: "additionalPreambleCommands",
                subElementName: "line",
                subElementValues: lines);

            // LaTeX Default Mode

            var defaultLatexModeCanvas = (Canvas)
                this.g_LaTeXGrid
                    .Children
                    .OfType<GroupBox>()
                    .First(
                        r 
                        => 
                        r.Name == "c_groupBoxDefaultLaTeXMode")
                    .Content;

            RadioButton modeRadioButton =
                defaultLatexModeCanvas
                .Children
                .OfType<RadioButton>()
                .First(
                    r
                    =>
                    r.IsChecked == true);

            var latexDefaultMode =
                modeRadioButton is null
                ?
                this.defaultConfiguration
                    .Element("latexDefaultMode").Attribute("value").Value
                :
                modeRadioButton.Content.ToString();

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "latexDefaultMode",
                attributeValue: latexDefaultMode);

            // Image File Format

            var imageFileFormatCanvas = (Canvas)
                this.g_LaTeXGrid
                    .Children
                    .OfType<GroupBox>()
                    .First(
                        r
                        =>
                        r.Name == "c_groupBoxImageFileFormat")
                    .Content;

            RadioButton formatRadioButton =
                imageFileFormatCanvas
                    .Children
                    .OfType<RadioButton>()
                    .FirstOrDefault(
                        r
                        =>
                        r.IsChecked == true);

            var imageFileFormat =
                formatRadioButton is null
                ?
                this.defaultConfiguration
                    .Element("imageFileFormat").Attribute("value").Value
                :
                formatRadioButton.Content.ToString();

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "imageFileFormat",
                attributeValue: imageFileFormat);

            // Image Depth Correction

            var imageDepthCorrection = this.c_imageDepthCorrection.Text;

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "imageDepthCorrection",
                attributeValue: imageDepthCorrection);

            // Image Scale Percentage

            var imageScalePercentage = this.c_imageScalePercentage.Text;

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "imageScalePercentage",
                attributeValue: imageScalePercentage);

            // Redirect File Processors

            var redirectFileProcessors = 
                this.c_redirectFileProcessors.IsChecked is null
                ?
                false
                :
                this.c_redirectFileProcessors.IsChecked.Value;

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "redirectFileProcessors",
                attributeValue: redirectFileProcessors.ToString());

            // MiKTeX Bin Folder

            var latexBinFolder = this.c_latexBinFolder.Text;

            this.UpdateOrAddDefaultConfigurationElement(
                elementName: "latexBinPath",
                attributeValue: latexBinFolder);

            this.DialogResult = true;

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Browsing folders

        private void LatexBrowseButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                // If one is selected, use that file
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.c_latexBinFolder.Text = dlg.SelectedPath;
            }
        }

        #endregion

        #endregion
    }
}
