// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Represents a DviPng process.
    /// </summary>
    /// <remarks>
    /// <see cref="DviPngProcessor"/> instances convert DVI files 
    /// into PNG files.
    /// </remarks>
    public class DviPngProcessor : FileProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DviPngProcessor"/> class.
        /// </summary> 
        /// <param name="latexBinPath">
        /// The LaTeX bin path.
        /// </param>
        /// <param name="workingPath">
        /// The working path.
        /// </param>
        /// <param name="defaultImageResolution">
        /// The default image resolution.
        /// </param>
        public DviPngProcessor(string latexBinPath, string workingPath, string defaultImageResolution)
        {
            /*
             *  ‘--depth*’  
             *  Report the depth of the image. 
             *  This only works reliably when the LaTeX style ‘preview.sty’ 
             *  from preview-latex is used with the ‘active’ option. 
             *  It reports the number of pixels from the bottom of the image 
             *  to the baseline of the image. This can be used for vertical positioning 
             *  of the image in, e.g., web documents, where one would use 
             *  (Cascading StyleSheets 1):
             *  <IMG SRC="filename.png" STYLE="vertical-align: -depthpx">
             *
             * The depth is a negative offset in this case, so the minus sign is necessary, 
             * and the unit is pixels (px). 
             */
            this.exe = latexBinPath + Path.DirectorySeparatorChar + "dvipng.exe";
            this.workingFolder = workingPath;
            this.defaultImageResolution = defaultImageResolution;
        }

        private readonly string workingFolder;
        private readonly string exe;
        private readonly string defaultImageResolution;

        /// <inheritdoc />
        public override string WorkingDirectory { get { return this.workingFolder; } }

        /// <inheritdoc />
        public override string Executable { get { return this.exe; } }

        /// <inheritdoc />
        public override string Arguments(string fileName, string additionalInfo)
        {
            string resolution = this.defaultImageResolution; 
            if (additionalInfo is object) {
                resolution = additionalInfo;
            }
            var arguments = "-depth* -bg Transparent -T tight -D " + resolution +
                " -o " + "\"" + this.workingFolder +
                Path.DirectorySeparatorChar +
                fileName + ".png" + "\"" + " " + "\"" + this.workingFolder +
                Path.DirectorySeparatorChar +
                fileName + ".dvi" + "\"";

            return arguments;
        }
    }
}
