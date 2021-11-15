// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Novacta.Transactions.IO;

namespace Novacta.Shfb.LatexTools.FileManagers
{
    /// <summary>
    /// Represents a resource manager that creates a 
    /// file having the specified content in bytes
    /// when a transaction is successfully committed.
    /// </summary>
    class FromByteArrayFileCreator : CreateFileManager
    {
        readonly byte[] byteContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FromByteArrayFileCreator"/> class.
        /// </summary>
        /// <param name="path">
        /// The path of the managed file.
        /// </param>
        /// <param name="byteContent">
        /// The content, in bytes, of the file to create.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="path"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="byteContent"/> is <b>null</b>.
        /// </exception>
        public FromByteArrayFileCreator(
            string path,
            byte[] byteContent) : base(path, overwrite: true)
        {
            this.byteContent = byteContent ?? throw new ArgumentNullException(nameof(byteContent));
        }

        /// <summary>
        /// Called when the transaction is successfully committed.
        /// </summary>
        protected override void OnCommit()
        {
            using BinaryWriter binaryWriter = new(base.ManagedFileStream);
            
            binaryWriter.Write(this.byteContent, 0, this.byteContent.Length);
        }
    }

}

