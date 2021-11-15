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
    /// Represents a file manager that updates shared content 
    /// items when a transaction is successfully committed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An instance of class <see cref="SharedContentItemsUpdater"/> 
    /// is expected to manage an XML file containing
    /// shared content items in the target SHFB installation.
    /// </para>
    /// <para>
    /// Property <see name="Items"/> returns a collection of tuples 
    /// having string elements named <c>Id</c> and <c>InnerText</c>. 
    /// Let <c>i</c> be a tuple included in such a collection.
    /// For each of such items, the manager checks if the XML file 
    /// contains a node tagged as "item" having    
    /// attribute <c>id</c> equal to <c>i.Id</c>; if not, such a node
    /// is added to the file content and its inner text 
    /// is set to the value of element <c>i.InnerText</c>.
    /// Otherwise, 
    /// if an item node having the specified identifier already exists,
    /// then its inner text is updated to such value.
    /// </para>
    /// </remarks>
    class SharedContentItemsUpdater : EditFileManager
    {
        readonly IEnumerable<(string Id, string InnerText)> items;

        /// <summary>
        /// Gets the items to update.
        /// </summary>
        /// <value>
        /// The items to update.
        /// </value>
        public IEnumerable<(string Id, string InnerText)> Items
        {
            get { return this.items; }
        }

        /// <summary>
        /// Initializes a new instance of 
        /// the <see cref="SharedContentItemsUpdater"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the shared content file to update.
        /// </param>
        /// <param name="items">
        /// The collection of items to update in the file.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Parameter <paramref name="path" /> is <b>null</b>.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="items" /> is <b>null</b>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter <paramref name="items" /> contains tuples 
        /// having null or empty elements.
        /// </exception>
        public SharedContentItemsUpdater(
            string path,
            IEnumerable<(string Id, string InnerText)> items) : base(path)
        {
            if (null == items)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var (Id, InnerText) in items)
            {
                if ((null == Id)
                    || (null == InnerText)
                    || (String.Empty == Id)
                    || (String.Empty == InnerText))
                {
                    throw new ArgumentException(
                        "The parameter cannot contain tuples having null or empty elements.",
                        nameof(items));
                }
            }
            this.items = items;
        }

        /// <inheritdoc/>
        protected override void OnCommit()
        {
            var document = new XmlDocument();

            document.Load(this.ManagedFileStream);
            XmlNode? contentNode = document.DocumentElement;

            if (contentNode is null)
            {
                throw new InvalidOperationException();
            }
            
            foreach (var (Id, InnerText) in this.items)
            {
                XmlNode? targetItemNode =
                    contentNode.SelectSingleNode("item[@id='" + Id + "']");
                
                if (targetItemNode != null)
                {
                    targetItemNode.RemoveAll();
                }
                else
                {
                    targetItemNode = document.CreateElement("item");
                    contentNode.PrependChild(targetItemNode);
                }
                XmlAttribute idAttribute = document.CreateAttribute("id");
                idAttribute.Value = Id;
                var targetItemNodeAttributes = targetItemNode.Attributes;

                if (targetItemNodeAttributes is null)
                {
                    throw new InvalidOperationException();
                }

                targetItemNodeAttributes.Append(idAttribute);

                targetItemNode.InnerText = InnerText;
            }

            this.ManagedFileStream.SetLength(0);
            document.Save(this.ManagedFileStream);
        }
    }
}
