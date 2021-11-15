// Copyright (c) Giovanni Lafratta. All rights reserved.
// Licensed under the MIT license. 
// See the LICENSE file in the project root for more information.

using System;

namespace Novacta.Shfb.LatexTools
{
    /// <summary>
    /// Contains constants to identify the topics
    /// available when documenting a project with the 
    /// Sandcastle Help File Builder.
    /// </summary>
    [Flags]
    public enum Topics
    {
        /// <summary>
        /// Identifies conceptual topics.
        /// </summary>
        Conceptual = 1,
        /// <summary>
        /// Identifies Sandcastle topics.
        /// </summary>
        Sandcastle = 2,
        /// <summary>
        /// All topics.
        /// </summary>
        All = Conceptual | Sandcastle
    }
}
