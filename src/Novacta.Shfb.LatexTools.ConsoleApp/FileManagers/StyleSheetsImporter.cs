// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Novacta.Transactions.IO;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Novacta.Shfb.LatexTools.FileManagers
{
    /// <summary>
    /// Represents a file manager that imports style sheets
    /// in a SHFB main XSLT file 
    /// when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="StyleSheetsImporter"/> 
    /// is expected to manage a main XSLT file in SHFB, importing
    /// style sheets in the target SHFB installation.
    /// </para>
    /// <para>
    /// Property <see name="StyleSheets"/> returns the collection of  
    /// style sheet names. 
    /// Let <c>i</c> be a tuple included in such a collection.
    /// For each item in <see cref="StyleSheets"/>, the manager checks 
    /// if the XML file 
    /// contains a node tagged as "xsl:import" having    
    /// attribute <c>href</c> equal to the item; if not, such a node
    /// is added to the file content.
    /// </para>
    /// </remarks>
    class StyleSheetsImporter : EditFileManager
    {
        readonly IEnumerable<string> styleSheets;

        /// <summary>
        /// Gets the style sheets to import.
        /// </summary>
        /// <value>
        /// The style sheets to import.
        /// </value>
        public IEnumerable<string> StyleSheets
        {
            get { return this.styleSheets; }
        }

        /// <summary>
        /// Initializes a new instance of 
        /// the <see cref="StyleSheetsImporter"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the managed main transform file.
        /// </param>
        /// <param name="styleSheets">
        /// The collection of style sheets to import in the file.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Parameter <paramref name="path" /> is <b>null</b>.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="styleSheets" /> is <b>null</b>.
        /// </exception>
        public StyleSheetsImporter(
            string path,
            IEnumerable<string> styleSheets) : base(path)
        {
            this.styleSheets = styleSheets ?? throw new ArgumentNullException(nameof(styleSheets));
        }

        /// <inheritdoc/>
        protected override void OnCommit()
        {
            var document = new XmlDocument();

            document.Load(this.ManagedFileStream);
            XmlNode? stylesheetNode = document.DocumentElement;

            if (stylesheetNode is null)
            {
                throw new InvalidOperationException();
            }

            string xslNamespace = "http://www.w3.org/1999/XSL/Transform";
            XmlNamespaceManager nsmgr = new(document.NameTable);
            nsmgr.AddNamespace("xsl", xslNamespace);

            XmlNode? outputNode =
                stylesheetNode.SelectSingleNode("xsl:output", nsmgr);

            foreach (var sheet in this.styleSheets)
            {
                XmlNode? targetImportNode =
                    stylesheetNode.SelectSingleNode("xsl:import[@href='" + sheet + "']", nsmgr);

                if (targetImportNode == null)
                {
                    targetImportNode = document.CreateElement("xsl", "import", xslNamespace);
                    XmlAttribute hrefAttribute = document.CreateAttribute("href");
                    hrefAttribute.Value = sheet;
                    var targetImportNodeAttributes = targetImportNode.Attributes;

                    if (targetImportNodeAttributes is null)
                    {
                        throw new InvalidOperationException();
                    }

                    targetImportNodeAttributes.Append(hrefAttribute);
                    stylesheetNode.InsertBefore(targetImportNode, outputNode);
                }
            }

            this.ManagedFileStream.SetLength(0);
            document.Save(this.ManagedFileStream);
        }
    }
}
