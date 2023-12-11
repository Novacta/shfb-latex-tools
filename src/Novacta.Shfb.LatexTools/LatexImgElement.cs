// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Xml.Linq;
using Sandcastle.Core;
using Sandcastle.Core.PresentationStyle.Transformation;
using Sandcastle.Core.PresentationStyle.Transformation.Elements;
using SandcastleBuilder.Utils.BuildEngine;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Represents a LatexImg element.
    /// </summary>
    public class LatexImgElement : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LatexImgElement"/> class.
        /// </summary>
        public LatexImgElement() : base("latexImg")
        {
        }

        /// <inheritdoc />
        public override void Render(
            TopicTransformationCore transformation,
            XElement element)
        {
            // element examples:
            //
            //  <latexImg name="latex_12" format="svg" depth="4" />
            //
            // or
            //
            // <latexImg name="latex_12" format="svg" depth="4" />
            //
            // A missing "depth" signals that the equation "mode" is "display",
            // hence no "style" attribute must be added to the 
            // replacing element.

            if (transformation == null)
                throw new ArgumentNullException(nameof(transformation));

            if (element == null)
                throw new ArgumentNullException(nameof(element));

            XAttribute nameAttribute = element.Attribute("name");
            XAttribute formatAttribute = element.Attribute("format");

            string nameAttributeValue = nameAttribute.Value;
            string formatAttributeValue = formatAttribute.Value;

            string replacingElementSrcAttributeValue;

            switch (transformation.SupportedFormats)
            {
                case HelpFileFormats.Website:
                case HelpFileFormats.HtmlHelp1 | HelpFileFormats.Website | HelpFileFormats.MSHelpViewer:
                    {
                        replacingElementSrcAttributeValue =
                          $"../media/{nameAttributeValue}.{formatAttributeValue}";
                    }
                    break;
                case HelpFileFormats.OpenXml:
                    {
                        if (string.CompareOrdinal(formatAttributeValue, "svg") == 0)
                        {
                            throw new InvalidOperationException(
                                "The SVG image file format is not supported under " +
                                "the Open XML presentation style.");
                        }

                        replacingElementSrcAttributeValue =
                            $"../media/{nameAttributeValue}.{formatAttributeValue}";
                    }
                    break;
                case HelpFileFormats.Markdown:
                    {
                        replacingElementSrcAttributeValue =
                            $"media/{nameAttributeValue}.{formatAttributeValue}";
                    }
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported combination of help file formats. " +
                        "Please, refer to the documentation for more information.");
            }

            XElement replacingElement = new XElement(
                name: "img",
                new XAttribute(
                    name: "alt", 
                    value: "LaTeX equation"),
                new XAttribute(
                    name: "src",
                    value: replacingElementSrcAttributeValue));

            XAttribute depthAttribute = element.Attribute("depth");

            bool isLatexModeInline = depthAttribute != null;

            if (isLatexModeInline)
            {
                string depthAttributeValue = depthAttribute.Value;
                int depth = Convert.ToInt32(depthAttributeValue);

                if (depth != 0)
                {
                    replacingElement.SetAttributeValue(
                        name: "style",
                        value: $"vertical-align: -{depthAttributeValue}px");
                }
            }

            transformation.CurrentElement.Add(replacingElement);
        }
    }
}
