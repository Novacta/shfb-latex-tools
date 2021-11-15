// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Novacta.Shfb.LatexTools
{
    /// <summary> 
    /// Represents a process that supports file elaborations.
    /// </summary>
    public abstract class FileProcessor
    {
        /// <summary>
        /// Runs the processor on the specified file applying 
        /// command-line arguments specific to that file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="additionalInfo">
        /// Additional information needed to
        /// evaluate arguments specific to the processed file.
        /// </param>
        /// <returns>
        /// The output of the file processor.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// The process exited with errors.
        /// </exception>
        public string Run(string fileName, string additionalInfo)
        {
            Process process = new Process();
            string output;
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = this.Executable;
                process.StartInfo.WorkingDirectory = this.WorkingDirectory;
                process.StartInfo.Arguments = this.Arguments(fileName, additionalInfo);
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                return output;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("The processor exited with errors.", e);
            }
            finally
            {
                process.Close();
            }
        }

        /// <summary>
        /// Runs the processor on the specified file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to process.
        /// </param>
        /// <returns>
        /// The output of the file processor.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// The process exited with errors.
        /// </exception>
        public string Run(string fileName)
        {
            return this.Run(fileName, null);
        }

        /// <summary>
        /// Gets the working directory of the processor.
        /// </summary>
        /// <value>
        /// The working directory.
        /// </value>
        public abstract string WorkingDirectory { get; }

        /// <summary>
        /// Returns the process arguments for the specified file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file to process.
        /// </param>
        /// <param name="additionalInfo">
        /// Additional information needed to
        /// evaluate arguments specific to the processed file.
        /// </param>
        /// <returns>
        /// A string representation of the process arguments.
        /// </returns>
        public abstract string Arguments(string fileName, string additionalInfo);

        /// <summary>
        /// Gets the path of the processor.
        /// </summary>
        /// <value>
        /// The path of the processor.
        /// </value>
        public abstract string Executable { get; }
    }
}
