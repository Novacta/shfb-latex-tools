// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Represents a DviSvgm process.
    /// </summary>
    /// <remarks>
    /// <see cref="DviSvgmProcessor"/> instances convert DVI files 
    /// into SVG files.
    /// </remarks>
    public class DviSvgmProcessor : FileProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DviSvgmProcessor"/> class.
        /// </summary>
        /// <param name="dvisvgmBinPath">
        /// The DviSvgm bin path.
        /// </param>
        /// <param name="workingPath">
        /// The working path.
        /// </param>
        /// <param name="defaultZoomFactor">
        /// The default zoom factor.
        /// </param>
        /// <param name="redirectFileProcessors">
        /// <c>true</c> if messages must be printed; otherwise, <c>false</c>.
        /// </param>
        public DviSvgmProcessor(
            string dvisvgmBinPath, 
            string workingPath, 
            string defaultZoomFactor,
            bool redirectFileProcessors)
        {
            this.exe = dvisvgmBinPath + Path.DirectorySeparatorChar + "dvisvgm.exe";
            this.workingPath = workingPath;
            this.defaultZoomFactor = defaultZoomFactor;
            this.verbosity = redirectFileProcessors ? "7" : "0";
        }

        private readonly string workingPath;
        private readonly string exe;
        private readonly string defaultZoomFactor;
        private readonly string verbosity;

        /// <inheritdoc />
        public override string WorkingDirectory { get { return this.workingPath; } }

        /// <inheritdoc />
        public override string Executable { get { return this.exe; } }

        /// <inheritdoc />
        public override string Arguments(string fileName, string additionalInfo)
        {
            string zoomFactor = this.defaultZoomFactor; 
            if (additionalInfo is object) {
                zoomFactor = additionalInfo;
            }

            var arguments = @" --verbosity=" + this.verbosity +
                " --no-fonts --exact --zoom=" + zoomFactor + " \"" +
                fileName + "\"";

            return arguments;
        }
    }
}
