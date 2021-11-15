// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Sandcastle.Core.BuildAssembler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Novacta.Shfb.LatexConfiguration
{
    /// <summary>
    /// Represents a configuration dialog box for the 
    /// Novacta LaTeX build component.
    /// </summary>
    public partial class LatexConfigDlg : Form
    {
        #region Build component configuration editor factory for MEF
        //=====================================================================

        /// <summary>
        /// Provides a method to edit the component configuration.
        /// </summary>
        [ConfigurationEditorExport("Novacta.Shfb.LatexComponent")]
        public sealed class BuildComponentFactory : IConfigurationEditor
        {
            /// <inheritdoc />
            public bool EditConfiguration(XElement configuration, CompositionContainer container)
            {
                using (var dlg = new LatexConfigDlg(configuration))
                {
                    return dlg.ShowDialog() == DialogResult.OK;
                }
            }
        }

        #endregion

        #region State

        private readonly XElement configuration;

        #endregion

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
        /// <?xml version="1.0" encoding="utf-8"?>
        /// <component id="Latex Component">
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
        ///    <dvisvgmBinPath value="C:\Program Files\MiKTeX 2.9\miktex\bin\x64" />
        ///    <helpType value="{@HelpFileFormat}" />
        ///    <basePath value="{@WorkingFolder}" />
        ///    <languagefilter value="true" />
        /// </component>
        /// ]]>
        /// </code>
        /// </para>
        /// </remarks>
        public LatexConfigDlg(XElement configuration)
        {
            this.InitializeComponent();

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;

            // Additional Preamble Commands
            var additionalPreambleCommands = configuration
                            .Element("additionalPreambleCommands");
            var lineNodes = additionalPreambleCommands.Descendants();
            List<string> lines = new List<string>();
            foreach (var lineNode in lineNodes)
            {
                lines.Add(lineNode.Value);
            }

            this.c_additionalPreambleCommands.Lines = lines.ToArray();

            // LaTeX Default Mode
            string mode;
            RadioButton defaultModeRadioButton;

            mode = configuration
                .Element("latexDefaultMode")
                .Attribute("value").Value;

            defaultModeRadioButton =
                this.c_groupBoxDefaultLaTeXMode
                    .Controls
                    .OfType<RadioButton>()
                    .First(r => 0 == string.CompareOrdinal(
                        r.Text, mode));

            defaultModeRadioButton.Checked = true;

            // Image File Format
            string format;
            RadioButton fileFormatRadioButton;

            format = configuration.Element("imageFileFormat")
                 .Attribute("value").Value;

            fileFormatRadioButton =
                this.c_groupBoxFileFormat.Controls.OfType<RadioButton>()
                    .First(r => 0 == string.CompareOrdinal(r.Text, format));

            fileFormatRadioButton.Checked = true;

            // Image Depth Correction
            this.c_imageDepthCorrection.Value = decimal.Parse(
                configuration.Element("imageDepthCorrection")
                      .Attribute("value").Value,
                CultureInfo.InvariantCulture);

            // Image Scale Factor
            this.c_imageScalePercentage.Value = decimal.Parse(
                configuration.Element("imageScalePercentage")
                      .Attribute("value").Value,
                CultureInfo.InvariantCulture);

            // Redirect File Processors
            this.c_redirectFileProcessors.Checked = bool.Parse(
                configuration.Element("redirectFileProcessors")
                      .Attribute("value").Value);

            // MiKTeX Bin Folder
            this.c_latexBinFolder.Text = configuration
                .Element("latexBinPath")
                .Attribute("value").Value;

            // DviSvgm Bin Folder
            this.c_dvisvgmBinFolder.Text = configuration
                .Element("dvisvgmBinPath")
                .Attribute("value").Value;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            // Additional Preamble Commands
            var additionalPreambleCommands =
                this.c_groupBoxPreambleAdditionalItems.Controls
                    .OfType<TextBox>()
                    .First().Lines;

            // LaTeX Default Mode
            RadioButton modeRadioButton;

            modeRadioButton = this.c_groupBoxDefaultLaTeXMode.Controls
                .OfType<RadioButton>()
                .First(r => r.Checked);

            var latexDefaultMode = modeRadioButton.Text;

            // Image File Format
            RadioButton formatRadioButton;

            formatRadioButton = this.c_groupBoxFileFormat.Controls
                .OfType<RadioButton>()
                .First(r => r.Checked);
            var imageFileFormat = formatRadioButton.Text;

            // Image Depth Correction
            var imageDepthCorrection = Convert.ToInt32(this.c_imageDepthCorrection.Value);

            // Image Scale Percentage
            var imageScalePercentage = Convert.ToDouble(this.c_imageScalePercentage.Value);

            // Redirect File Processors
            var redirectFileProcessors = this.c_redirectFileProcessors.Checked;

            // MiKTeX Bin Folder
            var latexBinFolder = this.c_latexBinFolder.Text;

            // DviSvgm Bin Folder
            var dviSvgmBinFolder = this.c_dvisvgmBinFolder.Text;

            // Update configuration
            {
                // additionalPreambleCommands
                var additionalPreambleCommandsNode =
                    this.configuration.Element("additionalPreambleCommands");

                additionalPreambleCommandsNode.RemoveNodes();
                for (int i = 0; i < additionalPreambleCommands.Length; i++)
                {
                    additionalPreambleCommandsNode.Add(
                        new XElement("line", additionalPreambleCommands[i]));
                }

                // latexDefaultMode
                this.configuration.Element("latexDefaultMode").Attribute("value")
                    .SetValue(latexDefaultMode);

                // imageFileFormat
                this.configuration.Element("imageFileFormat").Attribute("value")
                    .SetValue(imageFileFormat);

                // imageDepthCorrection
                this.configuration.Element("imageDepthCorrection").Attribute("value")
                   .SetValue(imageDepthCorrection);

                // imageScaleFactor
                this.configuration.Element("imageScalePercentage").Attribute("value")
                   .SetValue(imageScalePercentage);

                // redirectFileProcessors
                this.configuration.Element("redirectFileProcessors").Attribute("value")
                    .SetValue(redirectFileProcessors);

                // latexBinPath
                this.configuration.Element("latexBinPath").Attribute("value")
                     .SetValue(latexBinFolder);

                // dvisvgmBinPath
                this.configuration.Element("dvisvgmBinPath").Attribute("value")
                    .SetValue(dviSvgmBinFolder);

            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Browsing folders

        private void LatexBrowseButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(this.SelectLatexFolder)
            {
                IsBackground = true
            };
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        private void SelectLatexFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetLatexText(dialog.SelectedPath);
            }

            dialog.Dispose();
        }

        // This method implements a pattern for making thread-safe
        // calls on a Windows Forms control. 
        //
        // If the calling thread is different from the thread that
        // created the TextBox control, this method creates a
        // SetTextCallback and calls itself asynchronously using the
        // Invoke method.
        //
        // If the calling thread is the same as the thread that created
        // the TextBox control, the Text property is set directly. 

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);

        private void SetLatexText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.c_latexBinFolder.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetLatexText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.c_latexBinFolder.Text = text;
            }
        }

        private void DvisvgmBrowseButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(this.SelectDviSvgmFolder)
            {
                IsBackground = true
            };
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void SetDviSvgmText(string text)
        {
            // InvokeRequired compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.c_dvisvgmBinFolder.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetDviSvgmText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.c_dvisvgmBinFolder.Text = text;
            }
        }

        private void SelectDviSvgmFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetDviSvgmText(dialog.SelectedPath);
            }

            dialog.Dispose();
        }

        #endregion
    }
}
