// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Novacta.Transactions.IO;
using System.Xml;

namespace Novacta.Shfb.LatexTools.FileManagers
{
    /// <summary>
    /// Represents a resource manager that creates a new XML file 
    /// when a transaction is successfully committed.
    /// </summary>
    sealed class XmlFileCreator : CreateFileManager
    {
        readonly XmlDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileCreator"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the managed file.
        /// </param>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="path"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="document"/> is <b>null</b>.
        /// </exception>
        public XmlFileCreator(
            string path,
            XmlDocument document) : base(path, overwrite: true)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>
        /// Called when the transaction is successfully committed.
        /// </summary>
        protected override void OnCommit()
        {
            this.document.Save(this.ManagedFileStream);
        }
    }
}

