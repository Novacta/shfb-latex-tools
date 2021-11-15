// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using Novacta.Transactions.IO;
using Novacta.Shfb.LatexTools.FileManagers;
using System.Xml;
using System.Transactions;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Provides methods to update a specific
    /// Sandcastle Help File Builder installation.
    /// </summary>
    static class Shfb
    {
        #region State

        /// <summary>
        /// Gets the value of the environment variable SHFBROOT,
        /// or <b>null</b> if the variable is not found.
        /// </summary>
        /// <value>
        /// the value of the environment variable SHFBROOT,
        /// or <b>null</b> if the variable is not found.
        /// </value>
        public static string Root
        {
            get; private set;
        }

        /// <summary>
        /// Updates a SHFB installation having the specified path.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the update information.
        /// </typeparam>
        /// <param name="updater">
        /// A function that takes a path and enumerates the file
        /// managers encapsulating the updating logic for such specific path.
        /// </param>
        /// <param name="updateInfo">
        /// The update information.
        /// </param>
        /// <param name="path">
        /// The path of the SHFB installation to update.
        /// </param>
        /// <returns>
        /// A value equal to <c>0</c> for successful updates; nonzero otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="updater" /> is <b>null</b>.<br />
        /// -or-<br /><paramref name="path" /> is <b>null</b>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The specified path does not appear as the root of a SHFB installation.<br />
        /// -or-<br />
        /// The specified path is the root of a SHFB installation
        /// but its version differs from the target one.
        /// </exception>
        public static int Update<T>(
            Func<T, string, IEnumerable<FileManager>> updater,
            T updateInfo,
            string path)
        {
            #region Input validation

            if (updater is null)
            {
                throw new ArgumentNullException(nameof(updater));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            #endregion

            int exitCode = 0;

            var managers = updater(updateInfo, path);
            try
            {
                // Apply transaction logic
                using TransactionScope scope = new(TransactionScopeOption.RequiresNew);
                foreach (var manager in managers)
                {
                    manager.EnlistVolatile(EnlistmentOptions.None);
                }

                scope.Complete();
            }
            catch (Exception e)
            {
                var innerException = e.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);

                    innerException = innerException.InnerException;
                }
                Console.WriteLine(e.Message);
                exitCode = -1;
            }
            Console.WriteLine();
            if (exitCode == 0)
            {
                Console.WriteLine("SHFB installation successfully updated.");
            }
            else
            {
                Console.WriteLine(
                    "An error occurred. " +
                    "The SHFB installation has not been modified.");
            }

            foreach (var manager in managers)
            {
                manager.Dispose();
            }
            return exitCode;
        }

        static Shfb()
        {
            var shfbRoot = Environment.GetEnvironmentVariable(
                 "SHFBROOT",
                 EnvironmentVariableTarget.Machine);

            if (shfbRoot is null)
            {
                throw new InvalidOperationException(
                    "The environmental variable SHFBROOT cannot be found. " +
                    "Please, install SHFB and try again.");

            }

            Shfb.Root = shfbRoot;
        }

        static readonly string[] presentationStyles = new string[] {
            "Markdown",
            "OpenXml",
            "VS2010",
            "VS2013" };

        /// <summary>
        /// Gets the presentation styles supported by 
        /// the target SHFB.
        /// </summary>
        /// <value>The presentation styles supported by the 
        /// target SHFB.</value>
        public static IEnumerable<string> PresentationStyles
        {
            get { return Shfb.presentationStyles; }
        }

        #endregion

        #region Topics

        /// <summary>
        /// Validates the specified topics.
        /// </summary>
        /// <param name="topics">
        /// The topics to validate.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="topics"/> is not a field of the 
        /// <see cref="Topics"/> enumeration.
        /// </exception>
        internal static void Validate(Topics topics)
        {
            if ((Topics.All != topics)
                &&
                (Topics.Conceptual != topics)
                &&
                (Topics.Sandcastle != topics))
            {
                throw new ArgumentException(
                    String.Format(
                        "The parameter is not a field of the {0} enumeration.",
                        typeof(Topics).ToString()),
                    nameof(topics));
            }
        }

        #endregion

        #region Style sheets

        /// <summary>
        /// Gets the Sandcastle main style sheet 
        /// of the specified presentation style.
        /// </summary>
        /// <param name="presentationStyle">
        /// The presentation style.
        /// </param>
        /// <returns>
        /// The Sandcastle main style sheet corresponding 
        /// to the specified presentation style.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="presentationStyle"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="presentationStyle"/> is not supported 
        /// by the SHFB Tools.
        /// </exception>
        public static string GetSandcastleMainStyleSheet(
            string presentationStyle)
        {
            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            string mainStyleSheet = presentationStyle switch
            {
                "Markdown" or "OpenXml" => "MainSandcastle.xsl",
                "VS2010" or "VS2013" => "main_sandcastle.xsl",
                _ => throw new ArgumentException(
                        "The presentation style is not supported.",
                        nameof(presentationStyle)),
            };
            return mainStyleSheet;
        }

        /// <summary>
        /// Gets the Conceptual main style sheet 
        /// of the specified presentation style.
        /// </summary>
        /// <param name="presentationStyle">
        /// The presentation style.
        /// </param>
        /// <returns>
        /// The Conceptual main style sheet corresponding 
        /// to the specified presentation style.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="presentationStyle"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="presentationStyle"/> is not supported 
        /// by the target SHFB.
        /// </exception>
        static string GetConceptualMainStyleSheet(
            string presentationStyle)
        {
            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            string mainStyleSheet = presentationStyle switch
            {
                "Markdown" or "OpenXml" => "MainConceptual.xsl",
                "VS2010" or "VS2013" => "main_conceptual.xsl",
                _ => throw new ArgumentException(
                        "The presentation style is not supported.",
                        nameof(presentationStyle)),
            };

            return mainStyleSheet;
        }

        /// <summary>
        /// Gets the main style sheets of the specified presentation style
        /// for the given topics.
        /// </summary>
        /// <param name="presentationStyle">
        /// The presentation style.
        /// </param>
        /// <param name="topics">
        /// The topics for which the main style sheets 
        /// are to be returned.
        /// </param>
        /// <returns>
        /// The collection of main style sheets corresponding 
        /// to the specified presentation style.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="presentationStyle"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="presentationStyle"/> is not supported 
        /// by the SHFB Tools.<br/>
        /// -or-<br/>
        /// <paramref name="topics"/> is not a field of 
        /// the <see cref="Topics"/> enumeration.
        /// by the target SHFB.
        /// </exception>
        static IEnumerable<string> GetMainStyleSheets(
            string presentationStyle,
            Topics topics)
        {
            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            if (!Shfb.PresentationStyles.Contains(presentationStyle))
            {
                throw new ArgumentException(
                    "The presentation style is not supported.",
                    nameof(presentationStyle));
            }

            Shfb.Validate(topics);

            List<string> mainStyleSheets = new();

            if ((topics & Topics.Sandcastle) == Topics.Sandcastle)
            {
                mainStyleSheets.Add(GetSandcastleMainStyleSheet(presentationStyle));
            }
            if ((topics & Topics.Conceptual) == Topics.Conceptual)
            {
                mainStyleSheets.Add(GetConceptualMainStyleSheet(presentationStyle));
            }

            return mainStyleSheets;
        }

        /// <summary>
        /// Gets the style sheet <c>Hrefs</c>.
        /// </summary>
        /// <param name="styleSheets">The style sheets.</param>
        /// <returns>The collection of <c>Hrefs</c>.</returns>
        static IEnumerable<string> GetStyleSheetHrefs(
            IEnumerable<(string Href, XmlDocument Document)> styleSheets)
        {
            foreach (var (Href, _) in styleSheets)
            {
                yield return Href;
            }
        }

        /// <summary>
        /// Prepares the importation of a collection of style sheets
        /// for the specified topics and presentation style.
        /// </summary>
        /// <param name="shfbPath">
        /// The SHFB path.
        /// </param>
        /// <param name="presentationStyle">
        /// The presentation style.
        /// </param>
        /// <param name="topics">
        /// The topics for which the sheets 
        /// are to be imported.
        /// </param>
        /// <param name="styleSheets">
        /// The style sheets to import.
        /// </param>
        /// <remarks>
        /// <para>
        /// The returned file managers import style sheets in
        /// a main XSLT file of the target SHFB installation.
        /// </para>
        /// <para>
        /// Parameter <paramref name="styleSheets" /> returns a collection of tuples
        /// having a string element named <c>Href</c> and an element
        /// <c>Document</c> of type <see cref="XmlDocument" />.
        /// Let <c>i</c> be a tuple included in such a collection.
        /// For each of such items, the manager checks if the XML file
        /// contains a node tagged as <c>"xsl:import"</c> having
        /// attribute <c>href</c> equal to <c>i.Href</c>; if not, such a node
        /// is added to the file content.
        /// Furthermore, a new file representing
        /// <c>i.Document</c> is added to the
        /// directory of the managed file.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The list of file managers required for the specified
        /// operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Parameter <paramref name="shfbPath" /> is <b>null</b>.<br />
        /// -or-<br />
        /// Parameter <paramref name="presentationStyle" /> is <b>null</b>.<br />
        /// -or-<br />
        /// Parameter <paramref name="styleSheets" /> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Parameter <paramref name="shfbPath" /> is empty.<br />
        /// -or-<br />
        /// Parameter <paramref name="presentationStyle" /> is empty.<br />
        /// -or-<br />
        /// Parameter <paramref name="presentationStyle" /> is not supported.<br />
        /// -or-<br /><paramref name="topics" /> is not a field of
        /// <see cref="Topics" />.<br />
        /// -or-<br />
        /// Parameter <paramref name="styleSheets" /> contains a tuple having at
        /// least an element which is <b>null</b> or empty.
        /// </exception>
        public static IEnumerable<FileManager> PrepareStyleSheetImportation(
            string shfbPath,
            string presentationStyle,
            Topics topics,
            IEnumerable<(string Href, XmlDocument Document)> styleSheets)
        {
            #region Input validation

            if (null == shfbPath)
            {
                throw new ArgumentNullException(nameof(shfbPath));
            }

            if (String.Empty == shfbPath)
            {
                throw new ArgumentException(
                    "The parameter cannot be empty.",
                    nameof(shfbPath));
            }

            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            if (String.Empty == presentationStyle)
            {
                throw new ArgumentException(
                    "The parameter cannot be empty.",
                    nameof(presentationStyle));
            }

            Shfb.Validate(topics);

            #endregion

            var mainStyleSheets = Shfb.GetMainStyleSheets(
                presentationStyle,
                topics);

            List<FileManager> managers = new();

            #region Transforms

            var targetDirectory = Path.Combine(shfbPath,
                        "PresentationStyles",
                        presentationStyle,
                        "Transforms");

            var hRefs = GetStyleSheetHrefs(styleSheets);

            foreach (var mainStyleSheet in mainStyleSheets)
            {
                managers.Add(new StyleSheetsImporter(
                    Path.Combine(targetDirectory,
                        mainStyleSheet),
                    hRefs));
            }

            foreach (var (Href, Document) in styleSheets)
            {
                var targetSheet = Path.Combine(targetDirectory,
                        Href);

                managers.Add(new XmlFileCreator(
                    targetSheet,
                    Document));
            }

            #endregion

            return managers;
        }

        #endregion

        #region Shared content

        /// <summary>
        /// Gets the shared content files of the specified presentation style.
        /// </summary>
        /// <param name="presentationStyle">
        /// The presentation style.
        /// </param>
        /// <returns>
        /// The collection of shared content files corresponding 
        /// to the specified presentation style.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="presentationStyle"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="presentationStyle"/> is not supported 
        /// by the target SHFB.
        /// </exception>
        public static IEnumerable<string> GetSharedContentFiles(
            string presentationStyle)
        {
            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            if (!Shfb.PresentationStyles.Contains(presentationStyle))
            {
                throw new ArgumentException(
                    "The presentation style is not supported.",
                    nameof(presentationStyle));
            }

            List<string> sharedContentFiles = new();
            switch (presentationStyle)
            {
                case "Markdown":
                case "OpenXml":
                    sharedContentFiles.Add("SharedContent.xml");
                    break;
                case "VS2010":
                case "VS2013":
                    sharedContentFiles.Add("shared_content.xml");
                    sharedContentFiles.Add("shared_content_mshc.xml");
                    break;
            }

            return sharedContentFiles;
        }

        /// <summary>
        /// Prepares the modification of the given shared content items for 
        /// the specified presentation style.
        /// </summary>
        /// <param name="shfbPath">The SHFB path.</param>
        /// <param name="presentationStyle">The presentation style.</param>
        /// <param name="sharedContentFile">The shared content file.</param>
        /// <param name="items">The items to modify.</param>
        /// <returns>
        /// The file manager required for the 
        /// specified modifications.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The returned file manager modifies 
        /// the content of an XML file containing 
        /// shared content items.
        /// </para>
        /// <para>
        /// Parameter <paramref name="items"/> represents a list of tuples 
        /// having string elements named <c>Id</c> and <c>InnerText</c>. 
        /// Let <c>i</c> be a tuple included in such a list.
        /// For each of such items, the method checks if the XML file 
        /// contains a node tagged as "item" having    
        /// attribute <c>id</c> equal to <c>i.Id</c>; if not, such a node
        /// is added to the file content and its inner text 
        /// is set to the value of element <c>i.InnerText</c>.
        /// Otherwise, 
        /// if an item node having the specified identifier already exists,
        /// then its inner text is updated to such value.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Parameter <paramref name="shfbPath"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="presentationStyle"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="sharedContentFile"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="items"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Parameter <paramref name="shfbPath"/> is empty.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="presentationStyle"/> is empty.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="presentationStyle"/> is not supported.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="sharedContentFile"/> is not supported.<br/>
        /// -or-<br/>
        /// Parameter <paramref name="items"/> contains a tuple having at 
        /// least an element which is <b>null</b> or empty.
        /// </exception>
        public static FileManager PrepareSharedContentItemsModification(
            string shfbPath,
            string presentationStyle,
            string sharedContentFile,
            IEnumerable<(string Id, string InnerText)> items)
        {
            #region Input validation

            if (null == shfbPath)
            {
                throw new ArgumentNullException(nameof(shfbPath));
            }

            if (String.Empty == shfbPath)
            {
                throw new ArgumentException(
                    "The parameter cannot be empty.",
                    nameof(shfbPath));
            }

            if (null == presentationStyle)
            {
                throw new ArgumentNullException(nameof(presentationStyle));
            }

            if (String.Empty == presentationStyle)
            {
                throw new ArgumentException(
                    "The parameter cannot be empty.",
                    nameof(presentationStyle));
            }

            if (!Shfb.PresentationStyles.Contains(presentationStyle))
            {
                throw new ArgumentException(
                    "The presentation style is not supported.",
                    nameof(presentationStyle));
            }

            if (null == sharedContentFile)
            {
                throw new ArgumentNullException(nameof(sharedContentFile));
            }

            if (String.Empty == sharedContentFile)
            {
                throw new ArgumentException(
                    "The parameter cannot be empty.",
                    nameof(sharedContentFile));
            }

            if (!Shfb.GetSharedContentFiles(presentationStyle).Contains(sharedContentFile))
            {
                throw new ArgumentException(
                    "The shared content file is not supported.",
                    nameof(sharedContentFile));
            }

            #endregion

            return new SharedContentItemsUpdater(
                    System.IO.Path.Combine(shfbPath,
                        "PresentationStyles",
                        presentationStyle,
                        "Content",
                        sharedContentFile),
                    items);
        }

        #endregion

        #region Presentation styles

        /// <summary>
        /// Clones a presentation style by copying its files 
        /// to the specified path.
        /// </summary>
        /// <param name="presentationStyle">
        /// The presentation style to clone.
        /// </param>
        /// <param name="targetBasePath">
        /// The path of the folder where files are to be copied.
        /// </param>
        public static void ClonePresentationStyle(
            string presentationStyle,
            string targetBasePath)
        {
            var shfbRoot = Shfb.Root;

            var sourcePath = Path.Combine(shfbRoot,
                "Components",
                presentationStyle);

            var targetPath = Path.Combine(targetBasePath, presentationStyle);

            foreach (string dirPath in Directory.GetDirectories(
                path: sourcePath,
                searchPattern: "*",
                searchOption: SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            foreach (string newPath in Directory.GetFiles(
                path: sourcePath,
                searchPattern: "*.*",
                searchOption: SearchOption.AllDirectories))
            {
                File.Copy(
                    sourceFileName: newPath,
                    destFileName: newPath.Replace(sourcePath, targetPath),
                    overwrite: true);
            }

        }

        #endregion
    }
}
